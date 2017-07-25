/*
 *  2017. 07. 25
 *  UGC Xamarin, 황혜원
 *  
 *  수동 형식 변환을 위한 특성 클래스
 * 
 */
using System;

namespace UGCXamarin.Utils.Json.Attributes {
    /// <summary>
    /// 사용자가 수동으로 형식을 변환해야 함을 나타내는 특성입니다.<br />
    /// 이 특성이 적용된 속성은 <see cref="JsonConverter.FromDictionary{T}(Dictionary{string, object}, bool)" /> 메서드에 의해 값이 설정되지 않습니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class ManualTypeConversionAttribute : Attribute { }
}