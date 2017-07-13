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
using System;
namespace TeamDEV.Utility.Json {
    /// <summary>
    /// 경량화된 Json 문자열을 파싱하는 작업을 노출하는 클래스입니다.
    /// </summary>
    public static class MinifiedJsonParser {
        /// <summary>
        /// Json 문자열을 사전 리스트로 변환한 값을 반환합니다.
        /// </summary>
        /// <param name="s">변환할 Json 문자열입니다.</param>
        /// <returns>Json 데이터가 포함된 사전 리스트가 반환됩니다.</returns>
        public static object Parse(string s) {
            if (s == null) throw new ArgumentNullException("s");

            int p;
            return JsonValueConverter.Convert(s, false, 0, out p);
        }
    }
}