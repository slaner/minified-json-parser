/*
  Copyright (c) 2017 TeamDEV Korea.
  
  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files (the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions:
  
  The above copyright notice and this permission notice shall be included in all
  copies or substantial portions of the Software.
  
  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
  SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Text;
using TeamDEV.Utility.Json.Extensions;
namespace TeamDEV.Utility.Json {
    /// <summary>
    /// 값을 변환해주는 작업을 노출하는 클래스입니다.
    /// </summary>
    public static class JsonValueConverter {
        static readonly Dictionary<JsonValueType, JsonValueConvertCallback> cachedConvertCallbacks;
        static JsonValueConverter() {
            cachedConvertCallbacks = new Dictionary<JsonValueType, JsonValueConvertCallback>() {
                { JsonValueType.String, new JsonValueConvertCallback(StringConvertCallback) },
                { JsonValueType.Boolean, new JsonValueConvertCallback(BooleanConvertCallback) },
                { JsonValueType.Decimal, new JsonValueConvertCallback(DecimalConvertCallback) },
                { JsonValueType.KeyValuePair, new JsonValueConvertCallback(KeyValuePairConvertCallback) },
                { JsonValueType.Array, new JsonValueConvertCallback(ArrayConvertCallback) },
                { JsonValueType.NullReference, new JsonValueConvertCallback(NullReferenceConvertCallback) },
            };
        }

        /// <summary>
        /// Json 데이터가 포함된 문자열에서 Json 값을 변환하고, 변환된 값을 반환합니다.
        /// </summary>
        /// <param name="s">Json 데이터가 포함된 문자열입니다.</param>
        /// <param name="isArrayElement">배열 요소인지의 여부를 나타냅니다. 문자열 또는 키-값 쌍의 경우 무시됩니다.</param>
        /// <param name="p">변환 작업이 시작될 문자열 내 인덱스입니다.</param>
        /// <param name="p_new">변환 작업이 끝나는 문자열 내 인덱스가 저장될 변수입니다.</param>
        /// <returns>변환된 Json 값입니다.</returns>
        public static object Convert(string s, bool isArrayElement, int p, out int p_new) {
            JsonValueType valueType = s[p].GetValueType();
            if (!cachedConvertCallbacks.ContainsKey(valueType)) throw new InvalidJsonFormatException($"잘못된 Json 값 형식입니다. (valueType = {valueType})");
            return cachedConvertCallbacks[valueType](s, isArrayElement, p, out p_new);
        }

        static object StringConvertCallback(string s, bool isArrayElement, int p, out int p_new) {
            return ReadStringValue(s, p + 1, out p_new);
        }
        static object BooleanConvertCallback(string s, bool isArrayElement, int p, out int p_new) {
            string value = ReadPrimitiveValue(s, isArrayElement, p, out p_new);
            return bool.Parse(value);
        }
        static object DecimalConvertCallback(string s, bool isArrayElement, int p, out int p_new) {
            string value = ReadPrimitiveValue(s, isArrayElement, p, out p_new);
            return decimal.Parse(value);
        }
        static object KeyValuePairConvertCallback(string s, bool isArrayElement, int p, out int p_new) {
            // 빈 키-값 쌍인지 확인한다.
            if (s[++p] == JsonControlConst.EOB) {
                p_new = p + 1;
                return "Empty Key-Value Pair";
            }

            // p가 마지막을 나타내고 있다면
            if (p == (s.Length - 1)) {
                // EOB인지 검사, 아니면 예외를 발생시킨다.
                if (s[p] != JsonControlConst.EOB) throw new InvalidJsonFormatException($"경량화된 Json 문자열의 마지막 문자가 '{JsonControlConst.EOB}'가 아닙니다!");

                // 임시
                p_new = 0;

                // 옼희돜희-요!
                return null;
            }
            
            Dictionary<string, object> dicKVPairs = new Dictionary<string, object>();
            string name;
            object value;
            while (true) {
                // 요소의 키를 읽는다.
                name = ReadStringValue(s, p + 1, out p);

                // 읽은 후 다음 값이 키-값 구분자가 아니면 예외를 발생시킨다.
                if (s[p++] != JsonControlConst.KeyValueSeparator) throw new InvalidJsonFormatException($"키-값을 구분하는 구분자가 아닙니다. ({nameof(s)}[{nameof(p)}++] != {JsonControlConst.KeyValueSeparator})");

                // 값을 변환하고 사전에 추가한다.
                value = Convert(s, false, p, out p);
                dicKVPairs.Add(name, value);

                // 블록의 끝이라면 반복을 종료한다.
                if (s[p] == JsonControlConst.EOB) break;

                // 구분자인 경우 p의 값을 1 증가시킨다.
                if (s[p] == JsonControlConst.ElementSeparator) {
                    p++;
                    continue;
                }

                // DANGER AREA
                throw new InvalidJsonFormatException("이 구역은 실행되면 안됩니다!");
            }

            p_new = p + 1;
            if (dicKVPairs.Count <= 0) throw new InvalidJsonFormatException("키-값 쌍 사전에 데이터가 없습니다.");

            // 사전에 포함된 값이 2개 이상이면 사전을 반환한다.
            if (dicKVPairs.Count > 1) return dicKVPairs;

            // 한 개만 포함되어 있다면, 키-값 쌍을 반환한다.
            dicKVPairs.Clear();
            return new KeyValuePair<string, object>(name, value);
        }
        static object ArrayConvertCallback(string s, bool isArrayElement, int p, out int p_new) {
            // 빈 배열인지 검사한다.
            if (s[++p] == JsonControlConst.EOA) {
                p_new = p + 1;
                return new object[] { };
            }

            // 임시로 값을 저장할 변수
            object value = null;

            // 배열의 요소를 저장할 리스트를 만든다.
            List<object> array = new List<object>();

            // 무한 반복
            while (true) {
                // 값을 변환한 후 리스트에 추가한다.
                value = Convert(s, true, p, out p);
                array.Add(value);

                // 다음 요소를 찾아야 한다면 p 값을 1 증가시키고 다시 반복한다.
                if (s[p] == JsonControlConst.ElementSeparator) {
                    p++;
                    continue;
                }


                if (s[p] == JsonControlConst.EOA) break;
                throw new InvalidJsonFormatException($"문자 '{s[p]}' 가 잘못된 위치에 있습니다.");
            }

            p_new = p + 1;
            return array.ToArray();
        }
        static object NullReferenceConvertCallback(string s, bool isArrayElement, int p, out int p_new) {
            string value = ReadPrimitiveValue(s, isArrayElement, p, out p_new);
            if (!value.IsNullReferenceString()) throw new InvalidJsonFormatException($"값이 n으로 시작했지만, \"{JsonControlConst.NullReferenceString}\"이 아닙니다.");
            return null;
        }

        /// <summary>
        /// 문자열 값을 읽습니다.
        /// </summary>
        /// <param name="s">읽을 문자열 값이 포함된 문자열입니다.</param>
        /// <param name="p">읽기를 시작할 문자열 내 인덱스입니다.</param>
        /// <param name="p_new">문자열이 끝나는 인덱스를 저장할 변수입니다.</param>
        /// <param name="needConsiderEscapeSequence">이스케이프 문자를 처리할 것인지를 나타내는 값입니다.</param>
        /// <remarks>
        /// 이 함수를 호출할 때, 문자열이 시작되는 인덱스에 1을 더한 값이 p에 전달되므로 마지막 문자열만 검사하면 됨
        /// </remarks>
        /// <returns>문자열 값 또는 (이스케이프 문자를 처리하는 경우) 이스케이프 문자가 치환된 문자열 값을 반환합니다.</returns>
        static string ReadStringValue(string s, int p, out int p_new, bool needConsiderEscapeSequence = true) {
            // 문자열의 범위를 넘어서는지 검사한다.
            if ((p + 1) >= s.Length) throw new IndexOutOfRangeException($"({nameof(p)} + 1) >= {nameof(s.Length)} ({p + 1} >= {s.Length})");

            // 문자열이 끝나는 인덱스를 가져온다.
            int endIndex = s.IndexOf(JsonControlConst.StringIndicator, p);
            if (endIndex == -1) throw new InvalidJsonFormatException($"{nameof(endIndex)} == -1");

            // 결과 저장용 변수
            string result = null;

            // 이스케이프 문자를 처리해야 하는 경우
            if (needConsiderEscapeSequence) {
                //우선 반복한다.
                while (true) {
                    // 따옴표 전의 문자가 이스케이프 문자면 다시 찾는다.
                    if (s[endIndex - 1] == JsonControlConst.EscapeSequence) {
                        endIndex = s.IndexOf(JsonControlConst.StringIndicator, endIndex + 1);
                        if (endIndex == -1) throw new InvalidJsonFormatException($"문자열이 끝나지 않습니다. ({nameof(endIndex)} == -1)");
                        continue;
                    }

                    // 따옴표 전의 문자가 이스케이프 문자가 아니면 탈출!!
                    break;
                }

                // 문자열의 특정 부분을 잘라내기 위해 StringBuilder를 사용한다.
                // ------------------------------------------------------------
                // string.Substring과 StringBuilder의 생성자에서 문자열을 잘라내는 것은 비슷하지만
                // string.Substring을 사용하여 반환된 문자열을 가지고 Replace를 호출하면 또 다른 문자열 개체가 생성된다.
                // StringBuilder에서 Replace를 호출하는 경우 문자열 개체가 생성되지 않으며 기존에 있던 영역을 재사용하므로 더 효율적이다.
                int length = endIndex - p;
                StringBuilder buffer = new StringBuilder(s, p, length, length);

                // 치환한다.
                buffer = buffer.ReplaceEscapeSequences();
                result = buffer.ToString();
            } else result = s.Substring(p, endIndex - p);

            p_new = endIndex + 1;
            return result;
        }
        /// <summary>
        /// 정수나 실수, <see cref="bool" /> 또는 null 값을 읽습니다.
        /// </summary>
        /// <param name="s">읽을 값이 포함된 문자열입니다.</param>
        /// <param name="isArrayElement">배열 내의 요소인지의 여부를 나타내는 값입니다.</param>
        /// <param name="p">읽기를 시작할 문자열 내 인덱스입니다.</param>
        /// <param name="p_new">값이 끝나는 인덱스를 저장할 변수입니다.</param>
        /// <returns>문자열로 표현된 값을 반환합니다.</returns>
        static string ReadPrimitiveValue(string s, bool isArrayElement, int p, out int p_new) {
            // 출력 변수 초기화
            p_new = -1;

            for (int i = p; i < s.Length; i++) {
                // 블록의 끝을 만났다!
                if (s[i] == JsonControlConst.EOB) {
                    // 배열 요소인 경우 예외를 발생시킨다.
                    if (isArrayElement) throw new InvalidJsonFormatException("배열이 끝나지 않았는데 블록이 끝났습니다.");
                    p_new = i;
                    break;
                }

                // 요소 구분자
                if (s[i] == JsonControlConst.ElementSeparator) {
                    p_new = i;
                    break;
                }

                if (s[i] == JsonControlConst.EOA) {
                    if (!isArrayElement) throw new InvalidJsonFormatException("배열 내의 요소가 아니지만 배열이 끝났습니다.");
                    p_new = i;
                    break;
                }
            }

            if (p_new == -1) throw new InvalidJsonFormatException("문자열의 끝에 도달했지만 값이 끝나지 않았습니다.");
            return s.Substring(p, p_new - p);
        }
    }
}