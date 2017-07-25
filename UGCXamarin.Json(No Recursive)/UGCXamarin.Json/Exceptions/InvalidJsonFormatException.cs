using System;

namespace UGCXamarin.Utils.Json.Exceptions {
    /// <summary>
    /// Json 데이터의 형식이 잘못된 경우에 발생하는 예외입니다.
    /// </summary>
    public class InvalidJsonFormatException : Exception {
        /// <summary>
        /// 예외 메세지를 이용하여 <see cref="InvalidJsonFormatException" /> 예외 클래스의 개체를 만듭니다.
        /// </summary>
        /// <param name="message">예외가 발생한 원인을 나타내는 메세지입니다.</param>
        public InvalidJsonFormatException(string message) : base(message) { }


        public static Exception CreateInvalidPairSeperator(string s, int p) {
            return new InvalidJsonFormatException($"키-값을 구분하는 구분자가 아닙니다. ({nameof(s)}[{nameof(p)}++] != {JsonControlConst.KeyValueSeparator} / {nameof(s)}[{nameof(p)}] = {s[p]} / {nameof(p)} = {p})");
        }
        public static Exception CreateInvalidCharacterPosition(string s, int p) {
            return new InvalidJsonFormatException($"문자 '{s[p]}' 가 잘못된 위치에 있습니다. ({nameof(p)} = {p})");
        }
    }
}