/*
 *  2017. 07. 24
 *  UGC Xamarin, 황혜원
 *  
 *  Json 값을 변환해주는 작업을 노출하는 클래스
 *  제네릭 형식으로의 변환 지원 기능 추가
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UGCXamarin.Utils.Json.Attributes;
using UGCXamarin.Utils.Json.Exceptions;
using UGCXamarin.Utils.Json.Extensions;
namespace UGCXamarin.Utils.Json {
    /// <summary>
    /// 값을 변환해주는 작업을 노출하는 클래스입니다.
    /// </summary>
    public static class JsonConverter {
        enum JsonStateFlags : byte {
            Default = 0x00,
            Array = 0x01,
            Name = 0x02,
            Value = 0x04,
        }
        /// <summary>
        /// Json 형식의 문자열을 변환합니다.
        /// </summary>
        /// <param name="s">변환할 Json 형식의 문자열입니다.</param>
        /// <returns>사전 또는 배열</returns>
        public static object FromString(string s) {
            if (s == null) throw new ArgumentNullException("s");
            return InternalConvert(s);
        }
        /// <summary>
        /// 사전의 내용을 개체로 변환합니다.
        /// </summary>
        /// <typeparam name="T">변환할 개체의 형식입니다.</typeparam>
        /// <param name="dictionary">값이 저장되어 있는 사전입니다.</param>
        /// <param name="caseSensitive">개체의 속성 이름을 검색할 때 대,소문자를 구분할 것인지를 결정합니다.</param>
        /// <returns>변환된 개체</returns>
        public static T FromDictionary<T>(Dictionary<string, object> dictionary, bool caseSensitive = false) {
            // 전달된 형식 매개변수의 형식과 속성을 가져온다.
            Type baseType = typeof(T);
            List<PropertyInfo> properties = new List<PropertyInfo>(baseType.GetRuntimeProperties());

            // 속성의 이름을 찾을 때 대소문자를 구분할 것인지를 결정한다.
            StringComparison comparison = caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;

            // 매개변수가 없는 생성자를 이용하여 개체를 생성한다.
            // TODO: 생성자가 필요한 경우엔 Expression을 사용해서 처리해야 하는데, 이 부분은 어떻게 처리해야 할까..?
            T instance = Activator.CreateInstance<T>();

            // 속성을 모두 반복한다.
            for (int i = properties.Count - 1; i >= 0; i--) {
                PropertyInfo property = properties[i];

                // ManualTypeConversion 특성이 적용되어 있다면 건너뛴다.
                ManualTypeConversionAttribute manualTypeConversion = property.GetCustomAttribute<ManualTypeConversionAttribute>();
                if (manualTypeConversion != null) continue;
                
                // 읽기 전용 속성이면 건너뛴다.
                if (property.SetMethod == null) continue;

                // 속성의 setter가 public이 아니어도 건너뛴다.
                if (!property.SetMethod.IsPublic) continue;

                // 일치하는 속성을 찾았는지 검사하기 위한 변수
                KeyValuePair<string, object> target = default(KeyValuePair<string, object>);
                bool found = false;

                // 사전을 순환한다.
                foreach (KeyValuePair<string, object> pair in dictionary) {
                    // 일치하면 반복을 빠져나간다.
                    if (string.Equals(pair.Key, property.Name, comparison)) {
                        target = pair;
                        found = true;
                        break;
                    }
                }
                
                // 값을 찾지 못한 경우 다음으로 넘어간다.
                if (!found) continue;

                // 값을 설정한다.
                property.SetValueAuto(instance, target.Value);
                
                // 사전에서 키를 제거한다.
                dictionary.Remove(target.Key);

                // 목록에서도 속성을 제거한다.
                properties.RemoveAt(i);
            }

            // 반환한다.
            return instance;
        }

        /// <summary>
        /// 내부 변환 함수입니다.
        /// </summary>
        static object InternalConvert(string s) {
            // 처음에 문자열이 어떤 문자로 시작하는지 검사
            if ((s[0] != JsonControlConst.SOA) && (s[0] != JsonControlConst.SOB)) throw new InvalidJsonFormatException($"Invalid json start character '{s[0]}'");

            int length = s.Length;
            int startIndex = 0, endIndex;
            int pairDepth = 0, arrayDepth = 0;

            // 배열 및 키-값 쌍을 저장하기 위한 리스트
            List<Queue<object>> array = new List<Queue<object>>();
            List<Dictionary<string, object>> pair = new List<Dictionary<string, object>>();

            // 상태를 저장하기 위한 스택 및 변수
            Stack<JsonStateFlags> stateStack = new Stack<JsonStateFlags>();
            JsonStateFlags currentState = JsonStateFlags.Default;

            // 키를 저장하기 위한 스택
            Stack<string> keyStack = new Stack<string>();

            // 일시적으로 값을 저장하기 위한 변수
            object value = null;
            object result = null;
            while (startIndex < length) {
                #region 요소 구분자 처리
                // 요소 구분자일 경우
                if (s[startIndex] == JsonControlConst.ElementSeparator) {
                    // 배열 요소가 아닐 경우의 처리
                    if (currentState != JsonStateFlags.Array) {
                        // 이름 또는 값이 필요한데 구분자가 나온 경우의 예외 처리
                        if (currentState == JsonStateFlags.Name) throw new InvalidJsonFormatException($"{nameof(currentState)} = {currentState}, but {JsonControlConst.ElementSeparator} found. ({nameof(startIndex)} = {startIndex})");
                        if (currentState == JsonStateFlags.Value) throw new InvalidJsonFormatException($"{nameof(currentState)} = {currentState}, but {JsonControlConst.ElementSeparator} found. ({nameof(startIndex)} = {startIndex})");

                        // 이름이 필요하다! (배열 요소가 아니면 키-값 쌍 내의 요소임)
                        currentState = JsonStateFlags.Name;
                    }

                    // 배열/키-값 쌍 요소에 관계 없이 시작 인덱스를 1 증가시킨다.
                    startIndex++;
                }
                #endregion

                #region 값 형식 처리
                JsonValueType valueType = s[startIndex].GetValueType();
                if (valueType == JsonValueType.Unknown) throw InvalidJsonFormatException.CreateInvalidCharacterPosition(s, startIndex);
                #endregion

                #region 키-값 쌍
                #region 키-값 쌍 시작
                if (valueType == JsonValueType.PairStart) {
                    // 키-값 쌍 요소를 저장하기 위해 리스트의 끝에 새로운 사전(맵)을 생성한다.
                    pair.Add(new Dictionary<string, object>());

                    // 상태를 복원하기 위해 현재 상태를 스택에 추가한다.
                    stateStack.Push(currentState);
                    currentState = JsonStateFlags.Name;

                    // 배열 깊이, 시작 인덱스를 1 증가시킨다.
                    pairDepth++;
                    startIndex++;
                    continue;
                }
                #endregion
                #region 키-값 쌍 끝
                if (valueType == JsonValueType.PairEnd) {
                    // pairDepth가 0 이상이면 닫힐 수 있지만, 아니면 닫힐 수 없다.
                    if (pairDepth <= 0) throw new InvalidJsonFormatException($"{nameof(pairDepth)} <= 0 ({nameof(pairDepth)} = {pairDepth}), but '{JsonControlConst.EOB}' found.");

                    // 현재(닫히기 전)의 맵을 가져온다.
                    Dictionary<string, object> currentMap = pair.GetLast();

                    // 리스트에서 마지막에 있는 맵(currentMap)을 제거한다.
                    pair.RemoveLast();

                    // 상태를 복구한다.
                    currentState = stateStack.Pop();

                    // 기본 상태일 경우, 마지막일 수도 있으므로 result에 값을 담는다.
                    if (currentState == JsonStateFlags.Default) result = currentMap;
                    // 값이면 사전에 키 값과 같이 추가한다.
                    else if (currentState == JsonStateFlags.Value) {
                        string key = keyStack.Pop();
                        pair.GetLast().Add(key, currentMap);
                        currentState = JsonStateFlags.Default;
                    }
                    // 배열에 추가한다.
                    else if (currentState == JsonStateFlags.Array) array.GetLast().Enqueue(currentMap);
                    else throw new InvalidJsonFormatException($"Invalid state for current value type: {currentState}");
                    
                    // 배열 깊이를 1 감소시키고 시작 인덱스를 1 증가시킨다.
                    pairDepth--;
                    startIndex++;
                    continue;
                }
                #endregion
                #endregion

                #region 배열
                #region 배열 시작
                if (valueType == JsonValueType.ArrayStart) {
                    // 배열 내의 요소를 저장하기 위해 리스트의 끝에 새로운 큐를 생성한다.
                    array.Add(new Queue<object>());

                    // 배열이 닫히게 되면 상태를 복원해야 하므로, 복원하기 위해 스택에 값을 추가한다.
                    stateStack.Push(currentState);
                    currentState = JsonStateFlags.Array;

                    // 배열 깊이, 시작 인덱스를 1 증가시킨다.
                    arrayDepth++;
                    startIndex++;
                    continue;
                }
                #endregion
                #region 배열 끝
                if (valueType == JsonValueType.ArrayEnd) {
                    // arrayDepth가 0 이상이면 닫힐 수 있지만, 아니면 닫힐 수 없다.
                    if (arrayDepth <= 0) throw new InvalidJsonFormatException($"{nameof(arrayDepth)} <= 0 ({nameof(arrayDepth)}  = {arrayDepth}), but '{JsonControlConst.EOB}' found.");

                    // 현재(닫히기 전)의 배열 큐를 가져온다.
                    Queue<object> currentQueue = array.GetLast();

                    // 현재 큐를 배열로 변환한다.
                    object[] elements = currentQueue.ToArray();

                    // 리스트의 마지막에 있는 큐를 제거한다.
                    array.RemoveLast();

                    // 상태를 복구한다.
                    currentState = stateStack.Pop();

                    // 기본 상태일 경우, 마지막일 수도 있으므로 result에 배열 값을 담는다.
                    if (currentState == JsonStateFlags.Default) result = elements;
                    // 값이면 사전에 키 값과 같이 추가한다.
                    else if (currentState == JsonStateFlags.Value) {
                        string key = keyStack.Pop();
                        pair.GetLast().Add(key, elements);
                        currentState = JsonStateFlags.Default;
                    }
                    // 상태가 배열인데 마지막일 수는 없다.
                    else if (currentState == JsonStateFlags.Array) array.GetLast().Enqueue(elements);
                    else throw new InvalidJsonFormatException($"Invalid state for current value type: {currentState}");

                    // 배열 깊이를 1 감소시키고 시작 인덱스를 1 증가시킨다.
                    arrayDepth--;
                    startIndex++;
                    continue;
                }
                #endregion
                #endregion

                #region 문자열
                if (valueType == JsonValueType.String) {
                    endIndex = s.GetStringEndpoint(startIndex);
                    string strValue = s.FastSubstring(startIndex + 1, endIndex);
                    startIndex = endIndex + 1;

                    // 현재 상태가 이름이 와야하는 경우 스택에 이름(키)을 넣는다.
                    if (currentState == JsonStateFlags.Name) {
                        keyStack.Push(strValue);
                        currentState = JsonStateFlags.Value;

                        // startIndex에 있는 값이 콜론인지 검사한다.
                        if (s[startIndex++] != JsonControlConst.KeyValueSeparator) throw new InvalidJsonFormatException($"Invalid character '{s[startIndex]}' on current position. ({nameof(startIndex)} = {startIndex})");
                    }
                    // 값이 와야하는 경우, 키 스택에서 키를 빼내고 사전에 추가한다.
                    else if (currentState == JsonStateFlags.Value) {
                        string key = keyStack.Pop();
                        pair.GetLast().Add(key, strValue);
                        currentState = JsonStateFlags.Default;
                    }
                    // 이름이 아닌 배열 내 요소인 경우, 배열 큐에 값을 넣는다.
                    else if (currentState == JsonStateFlags.Array) array.GetLast().Enqueue(strValue);
                    else throw new InvalidJsonFormatException($"Invalid state for current value type: {currentState}");
                    continue;
                }
                #endregion

                #region 기본 자료형
                endIndex = s.ReadPrimitiveValue(currentState == JsonStateFlags.Array, startIndex);
                string strTemp = s.FastSubstring(startIndex, endIndex, false);
                value = strTemp.ResolveObject(valueType);
                startIndex = endIndex;

                if (currentState == JsonStateFlags.Value) {
                    string key = keyStack.Pop();
                    pair.GetLast().Add(key, value);
                    currentState = JsonStateFlags.Default;
                }
                else if (currentState == JsonStateFlags.Array) array.GetLast().Enqueue(value);
                else throw new InvalidJsonFormatException($"Invalid state for current value type: {currentState}");
                #endregion
            };

            // 반복은 끝났는데 arrayDepth, pairDepth의 값이 0이 아니면 뭔가 문제가 생긴 것이다..
            if ((arrayDepth != 0) || (pairDepth != 0)) throw new InvalidJsonFormatException("Array or key-value pair was not ended correctly.");
            // 상태 값도 Default가 아니면 뭔가 문제가 생긴 것이다.
            if (currentState != JsonStateFlags.Default) throw new InvalidJsonFormatException($"Loop has ended, but state is not Default. ({nameof(currentState)} = {currentState})");

            return result;
        }
    }
}
