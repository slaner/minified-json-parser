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