/*
 *  2017. 07. 12
 *  UGC Xamarin, 황혜원
 *  
 *  Json 값의 형식이 정의되어 있는 열거형
 * 
 */
 namespace UGCXamarin.Utils.Json {
    /// <summary>
    /// Json 값의 형식을 열거합니다.
    /// </summary>
    public enum JsonValueType {
        /// <summary>
        /// 알 수 없는 값입니다. 일반적으로 오류를 나타낼 때 사용됩니다.
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// 문자열 값입니다.
        /// </summary>
        String,
        /// <summary>
        /// 부울 값입니다.
        /// </summary>
        Boolean,
        /// <summary>
        /// 숫자 값입니다.
        /// </summary>
        Decimal,
        /// <summary>
        /// 키-쌍 구간을 여는 값입니다.
        /// </summary>
        PairStart,
        /// <summary>
        /// 키-쌍 구간을 닫는 값입니다.
        /// </summary>
        PairEnd,
        /// <summary>
        /// 배열 구간을 여는 값입니다.
        /// </summary>
        ArrayStart,
        /// <summary>
        /// 배열 구간을 닫는 값입니다.
        /// </summary>
        ArrayEnd,
        /// <summary>
        /// null 값입니다.
        /// </summary>
        NullReference,
    }
}