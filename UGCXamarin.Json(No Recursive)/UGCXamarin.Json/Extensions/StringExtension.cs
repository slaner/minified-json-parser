using System;

namespace UGCXamarin.Utils.Json.Extensions {
    static class StringExtension {
        /// <summary>
        /// 지정된 문자열이 <see cref="JsonControlConst.NullReferenceString" /> 과 같은 값을 가지고 있는지 검사합니다.
        /// </summary>
        /// <param name="s">검사할 문자열입니다.</param>
        /// <returns>지정된 문자열이 <see cref="JsonControlConst.NullReferenceString" /> 과 같은 값을 가진 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public static bool IsNullReferenceString(this string s) {
            return string.Equals(s, JsonControlConst.NullReferenceString);
        }
        
        public static object ResolveObject(this string s, JsonValueType type) {
            switch (type) {
                case JsonValueType.Boolean:
                    return bool.Parse(s);
                case JsonValueType.Decimal:
                    return decimal.Parse(s);
                case JsonValueType.NullReference:
                    if (!s.IsNullReferenceString()) throw new ArgumentException("s != \"null\"");
                    return null;
                default:
                    throw new ArgumentException($"Cannot resolve string. (s = \"{s}\"");
            }
        }
    }
}