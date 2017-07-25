using System.Collections.Generic;
using System.Text;

namespace UGCXamarin.Utils.Json.Extensions {
    static class CachedExtension {
        static readonly Dictionary<char, JsonValueType> typeCachedMap = new Dictionary<char, JsonValueType>() {
            { '-', JsonValueType.Decimal }, { '0', JsonValueType.Decimal }, { '1', JsonValueType.Decimal }, { '2', JsonValueType.Decimal }, { '3', JsonValueType.Decimal }, { '4', JsonValueType.Decimal }, { '5', JsonValueType.Decimal }, { '6', JsonValueType.Decimal }, { '7', JsonValueType.Decimal }, { '8', JsonValueType.Decimal }, { '9', JsonValueType.Decimal },
            { 't', JsonValueType.Boolean }, { 'f', JsonValueType.Boolean },
            { 'n', JsonValueType.NullReference },
            { '"', JsonValueType.String },
            { '{', JsonValueType.PairStart }, { '}', JsonValueType.PairEnd },
            { '[', JsonValueType.ArrayStart }, { ']', JsonValueType.ArrayEnd },
        };
        static readonly List<KeyValuePair<string, string>> cachedEscapeSequenceList = new List<KeyValuePair<string, string>>() {
            new KeyValuePair<string, string>("\\n", "\n"),
            new KeyValuePair<string, string>("\\r", "\r"),
            new KeyValuePair<string, string>("\\\"", "\""),
            new KeyValuePair<string, string>("\\t", "\t"),
            new KeyValuePair<string, string>("\\\\", "\\"),
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
            StringBuilder result = sb;
            foreach (KeyValuePair<string, string> pair in cachedEscapeSequenceList)
                result = result.Replace(pair.Key, pair.Value);
            return result;
        }
    }
}