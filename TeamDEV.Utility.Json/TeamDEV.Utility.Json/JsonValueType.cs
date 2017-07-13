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
namespace TeamDEV.Utility.Json {
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
        /// 키-쌍 값입니다.
        /// </summary>
        KeyValuePair,
        /// <summary>
        /// 배열 값입니다.
        /// </summary>
        Array,
        /// <summary>
        /// null 값입니다.
        /// </summary>
        NullReference,
    }
}