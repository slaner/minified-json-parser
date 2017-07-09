using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonMap = System.Collections.Generic.Dictionary<string, object>;

namespace MinifiedJsonParserTest {
    class InvalidJsonFormatException : Exception {
        public InvalidJsonFormatException(string message) : base(message) { }

    }
    static class MinifiedJson {
        /// <summary>
        /// 문자열을 나타내는 문자입니다.
        /// </summary>
        const char StringIndicator = '"';

        public static JsonMap Parse(string s) {
            if (s == null) throw new ArgumentNullException("s");

            int temp;
            return __parse(s, 0, 0, out temp);
        }

        static JsonMap __parse(string s, int depth, int p, out int p_new) {
            if (s[p] != '{') throw new InvalidJsonFormatException("s[p] != '{'");
            p++;
            
            JsonMap map = new JsonMap();
            int next = 1;
            while (s[next] != '}') {
                string name = __getPropertyName(s, p, out p);
                if (s[p] != ':') throw new InvalidJsonFormatException("s[p] != ':'");

                object value = __getPropertyValue(s, p, out p);

                map.Add(name, value);
            }

            p_new = p;
            return map;
        }

        static string __getPropertyName(string s, int p, out int p_new, bool needConsiderEscapeSequence = false) {
            int index = s.IndexOf(StringIndicator, p);
            if (index == -1) throw new InvalidJsonFormatException("index == -1");

            int endIndex = s.IndexOf(StringIndicator, index + 1);
            while (true) {
                if (endIndex == -1) throw new InvalidJsonFormatException("endIndex == -1");
                if (needConsiderEscapeSequence) {
                    if (s[endIndex - 1] == '\\') {
                        endIndex = s.IndexOf(StringIndicator, endIndex + 1);
                        continue;
                    }
                }
                break;
            }

            p_new = endIndex + 1;
            return s.Substring(index + 1, endIndex - (index + 1));
        }
        static object __getPropertyValue(string s, int p, out int p_new) {
            p_new = 0;

            return null;
        }
    }

    class Program {
        const string TestData = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Point\",\"coordinates\":[0,0]},\"properties\":{\"PostName\":\"a_place\",\"Description\":\"fun place to go\"}}";
        static void Main(string[] args) {
            JsonMap map = MinifiedJson.Parse(TestData);
            
        }
    }
}