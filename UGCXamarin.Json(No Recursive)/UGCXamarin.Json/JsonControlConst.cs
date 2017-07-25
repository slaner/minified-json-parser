namespace UGCXamarin.Utils.Json {
    /// <summary>
    /// Json 제어 상수가 정의된 클래스입니다.
    /// </summary>
    public static class JsonControlConst {
        /// <summary>
        /// null 값을 나타내는 문자열입니다.
        /// </summary>
        public const string NullReferenceString = "null";
        /// <summary>
        /// 문자열을 나타내는 문자입니다.
        /// </summary>
        public const char StringIndicator = '"';
        /// <summary>
        /// 요소를 구분하는 문자입니다.
        /// </summary>
        public const char ElementSeparator = ',';
        /// <summary>
        /// 키와 값을 구분하는 문자입니다.
        /// </summary>
        public const char KeyValueSeparator = ':';
        /// <summary>
        /// 블록의 시작(Start of Block)을 나타내는 문자입니다.
        /// </summary>
        public const char SOB = '{';
        /// <summary>
        /// 블록의 끝(End of Block)을 나타내는 문자입니다.
        /// </summary>
        public const char EOB = '}';
        /// <summary>
        /// 배열의 시작(Start of Array)을 나타내는 문자입니다.
        /// </summary>
        public const char SOA = '[';
        /// <summary>
        /// 배열의 끝(End of Array)을 나타내는 문자입니다.
        /// </summary>
        public const char EOA = ']';
        /// <summary>
        /// 이스케이프 시퀀스를 나타내는 문자입니다.
        /// </summary>
        public const char EscapeSequence = '\\'; 
    }
}