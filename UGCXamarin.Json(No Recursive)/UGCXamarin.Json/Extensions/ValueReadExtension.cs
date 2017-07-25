using System;
using System.Text;
using UGCXamarin.Utils.Json.Exceptions;

namespace UGCXamarin.Utils.Json.Extensions {
    /// <summary>
    /// 내부적으로 사용되는 값을 읽는 작업을 노출하는 클래스입니다.
    /// </summary>
    internal static class ValueReadExtension {
        /// <summary>
        /// <see cref="StringBuilder" /> 개체를 사용하여 문자열의 일부분을 잘라내고 이스케이프 시퀀스를 처리하여 반환합니다.
        /// </summary>
        /// <param name="s">잘라낼 문자열입니다.</param>
        /// <param name="startIndex">잘라내기를 시작할 문자열 내의 인덱스입니다.</param>
        /// <param name="endIndex">잘라내기가 끝날 문자열 내의 인덱스입니다.</param>
        /// <returns>이스케이프 처리된 부분 문자열</returns>
        public static string FastSubstring(this string s, int startIndex, int endIndex, bool handleEscape = true) {
            if (startIndex > endIndex) throw new ArgumentException($"{nameof(startIndex)} >= {nameof(endIndex)}");
            if (startIndex == endIndex) return string.Empty;

            int length = endIndex - startIndex;

            // 문자열의 특정 부분을 잘라내기 위해 StringBuilder를 사용한다.
            // ------------------------------------------------------------
            // string.Substring과 StringBuilder의 생성자에서 문자열을 잘라내는 것은 비슷하지만
            // string.Substring을 사용하여 반환된 문자열을 가지고 Replace를 호출하면 또 다른 문자열 개체가 생성된다.
            // StringBuilder에서 Replace를 호출하는 경우 문자열 개체가 생성되지 않으며 기존에 있던 영역을 재사용하므로 더 효율적이다.
            StringBuilder buffer = new StringBuilder(s, startIndex, length, length);
            if (handleEscape) return buffer.ReplaceEscapeSequences().ToString();
            else return buffer.ToString();
        }

        /// <summary>
        /// 문자열이 끝나는 지점을 가져옵니다.
        /// </summary>
        /// <param name="s">문자열이 포함된 문자열입니다.</param>
        /// <param name="p">읽기를 시작할 문자열 내의 인덱스입니다.(시작하는 따옴표가 있는 위치)</param>
        /// <returns>문자열이 끝나는 인덱스 값</returns>
        public static int GetStringEndpoint(this string s, int p) {
            if (s[p] != JsonControlConst.StringIndicator) throw new ArgumentException($"{nameof(s)}[{nameof(p)}] != {JsonControlConst.StringIndicator} ({nameof(s)}[{nameof(p)}] = '{s[p]}')");
            if ((p + 1) >= s.Length) throw new IndexOutOfRangeException($"({nameof(p)} + 1) >= {nameof(s.Length)} ({p + 1} >= {s.Length})");

            // 문자열이 끝나는 인덱스를 가져온다.
            int endIndex = s.IndexOf(JsonControlConst.StringIndicator, p + 1);
            if (endIndex == -1) throw new InvalidJsonFormatException($"{nameof(endIndex)} == -1");
            
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

            return endIndex;
        }
        
        /// <summary>
        /// 정수나 실수, <see cref="bool" /> 또는 null 값을 읽습니다.
        /// </summary>
        /// <param name="s">읽을 값이 포함된 문자열입니다.</param>
        /// <param name="isArrayElement">배열 내의 요소인지의 여부를 나타내는 값입니다.</param>
        /// <param name="p">읽기를 시작할 문자열 내 인덱스입니다.</param>
        /// <param name="p_new">값이 끝나는 인덱스를 저장할 변수입니다.</param>
        /// <returns>문자열로 표현된 값을 반환합니다.</returns>
        public static int ReadPrimitiveValue(this string s, bool isArrayElement, int p) {
            for (int i = p; i < s.Length; i++) {
                // 블록의 끝을 만났다!
                if (s[i] == JsonControlConst.EOB) {
                    // 배열 요소인 경우 예외를 발생시킨다.
                    if (isArrayElement) throw new InvalidJsonFormatException($"배열이 끝나지 않았는데 블록이 끝났습니다. ({nameof(i)} == {i})");
                    return i;
                }
                
                if (s[i] == JsonControlConst.ElementSeparator) return i;

                if (s[i] == JsonControlConst.EOA) {
                    if (!isArrayElement) throw new InvalidJsonFormatException($"배열 내의 요소가 아니지만 배열이 끝났습니다. ({nameof(i)} == {i})");
                    return i;
                }
            }

            throw new InvalidJsonFormatException($"문자열의 끝에 도달했지만 값이 끝나지 않았습니다.");
        }
    }
}