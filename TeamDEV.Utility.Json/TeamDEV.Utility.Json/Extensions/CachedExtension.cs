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
using System.Collections.Generic;
using System.Text;
namespace TeamDEV.Utility.Json.Extensions {
    static class CachedExtension {
        static readonly Dictionary<char, JsonValueType> typeCachedMap = new Dictionary<char, JsonValueType>() {
            { '-', JsonValueType.Decimal }, { '0', JsonValueType.Decimal }, { '1', JsonValueType.Decimal }, { '2', JsonValueType.Decimal }, { '3', JsonValueType.Decimal }, { '4', JsonValueType.Decimal }, { '5', JsonValueType.Decimal }, { '6', JsonValueType.Decimal }, { '7', JsonValueType.Decimal }, { '8', JsonValueType.Decimal }, { '9', JsonValueType.Decimal },
            { 't', JsonValueType.Boolean }, { 'f', JsonValueType.Boolean },
            { 'n', JsonValueType.NullReference },
            { '"', JsonValueType.String },
            { '{', JsonValueType.KeyValuePair },
            { '[', JsonValueType.Array },
        };
        static readonly Dictionary<string, string> escapeSequenceCachedMap = new Dictionary<string, string>() {
            { "\n", "\\n" },
            { "\r", "\\r" },
            { "\"", "\\\"" },
            { "\t", "\\t" },
        };

        /// <summary>
        /// 지정된 문자의 Json 값 형식을 반환합니다.
        /// </summary>
        /// <param name="c">검사할 문자입니다.</param>
        /// <returns>
        /// 지정된 문자의 값 형식이 매핑되어 있는 경우, 해당 문자의 값 형식을 반환합니다.<br />
        /// 문자의 값 형식이 매핑되어 있지 않은 경우 <see cref="JsonValueType.Unknown" />을 반환합니다.
        /// </returns>
        public static JsonValueType GetValueType(this char c) {
            if (!typeCachedMap.ContainsKey(c)) return JsonValueType.Unknown;
            return typeCachedMap[c];
        }
        /// <summary>
        /// 지정된 <see cref="StringBuilder" /> 개체에 들어있는 이스케이프 시퀀스 문자열을 이스케이프 시퀀스 문자로 치환합니다.
        /// </summary>
        /// <param name="sb">치환할 이스케이프 문자열이 들어있는 <see cref="StringBuilder" /> 개체입니다.</param>
        /// <returns></returns>
        public static StringBuilder ReplaceEscapeSequences(this StringBuilder sb) {
            StringBuilder result = null;
            foreach (KeyValuePair<string, string> pair in escapeSequenceCachedMap)
                result = sb.Replace(pair.Value, pair.Key);
            return result;
        }
    }
}