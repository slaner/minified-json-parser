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
    /// 값을 변환해주는 작업이 수행될 메서드를 캡슐화하는 대리자입니다.
    /// </summary>
    /// <param name="s">변환 작업을 수행하는데 참조할 문자열입니다.</param>
    /// <param name="isArrayElement">배열 요소인지의 여부를 나타냅니다. 문자열 값의 경우 무시됩니다.</param>
    /// <param name="p">변환 작업이 시작될 문자열 내 인덱스입니다.</param>
    /// <param name="p_new">변환 작업이 끝나는 문자열 내 인덱스가 저장될 변수입니다.</param>
    public delegate object JsonValueConvertCallback(string s, bool isArrayElement, int p, out int p_new);
}