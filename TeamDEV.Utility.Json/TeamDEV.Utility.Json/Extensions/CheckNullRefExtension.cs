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
namespace TeamDEV.Utility.Json.Extensions {
    static class CheckNullRefExtension {
        /// <summary>
        /// 지정된 문자열이 <see cref="JsonControlConst.NullReferenceString" /> 과 같은 값을 가지고 있는지 검사합니다.
        /// </summary>
        /// <param name="s">검사할 문자열입니다.</param>
        /// <returns>지정된 문자열이 <see cref="JsonControlConst.NullReferenceString" /> 과 같은 값을 가진 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public static bool IsNullReferenceString(this string s) {
            return string.Equals(s, JsonControlConst.NullReferenceString);
        }
    }
}