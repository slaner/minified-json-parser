using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UGCXamarin.Json;
using JsonMap = System.Collections.Generic.Dictionary<string, object>;
namespace UGCXamarin.Json {
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
    }
    static class Extensions {
        public static JsonValueType GetValueType(this char c) {
            if (c == '-' || char.IsDigit(c)) return JsonValueType.Decimal;
            if (c == 't' || c == 'f') return JsonValueType.Boolean;
            if (c == '"') return JsonValueType.String;
            if (c == '{') return JsonValueType.KeyValuePair;
            if (c == '[') return JsonValueType.Array;
            return JsonValueType.Unknown;
        }
    }
}
namespace MinifiedJsonParserTest {
    class InvalidJsonFormatException : Exception {
        public InvalidJsonFormatException(string message) : base(message) { }
    }

    /// <summary>
    /// Json 요소를 나타내는 클래스입니다.
    /// </summary>
    public class JsonElement {
        /// <summary>
        /// 빈 요소를 나타내는 문자열 상수 값입니다.
        /// </summary>
        public const string EmptyText = "EmptyElement";

        static readonly JsonElement g_emptyElement = new JsonElement();
        readonly string g_name;
        readonly object g_value;
        readonly bool g_isEmpty;

        JsonElement() {
            g_isEmpty = true;
        }
        /// <summary>
        /// 지정된 이름과 값을 가지는 <see cref="JsonElement" /> 개체를 만듭니다.
        /// </summary>
        /// <param name="name">요소를 나타내는 이름입니다.</param>
        /// <param name="value">요소가 저장하는 값입니다.</param>
        public JsonElement(string name, object value) {
            g_name = name;
            g_value = value;
        }

        /// <summary>
        /// 이 요소를 나타내는 이름을 가져옵니다.
        /// </summary>
        public string Name {
            get { return g_name; }
        }
        /// <summary>
        /// 이 요소에 저장된 값을 가져옵니다.
        /// </summary>
        public object Value {
            get { return g_value; }
        }
        /// <summary>
        /// 이 요소가 빈 요소인지를 나타내는 값을 가져옵니다.
        /// </summary>
        public bool IsEmpty {
            get { return g_isEmpty; }
        }
        /// <summary>
        /// 이름도 없고 값도 없는 빈 요소를 나타내는 값을 가져옵니다.
        /// </summary>
        public static JsonElement Empty {
            get { return g_emptyElement; }
        }

        /// <summary>
        /// 이 개체를 문자열로 표현한 값을 반환합니다.
        /// </summary>
        public override string ToString() {
            if (g_isEmpty) return EmptyText;
            return $"{g_name} = {g_value}";
        }
    }

    class ClassInitializerTest {
        int test;

        public int Test {
            get { return test; }
            set { test = value; }
        }
    }


    static class MinifiedJson {
        /// <summary>
        /// 문자열을 나타내는 문자입니다.
        /// </summary>
        const char StringIndicator = '"';
        /// <summary>
        /// 요소를 구분하는 문자입니다.
        /// </summary>
        const char ElementSeparator = ',';
        /// <summary>
        /// 키와 값을 구분하는 문자입니다.
        /// </summary>
        const char KeyValueSeparator = ':';
        /// <summary>
        /// 블록의 시작을 나타내는 문자입니다.
        /// </summary>
        const char StartOfBlock = '{';
        /// <summary>
        /// 블록의 끝을 나타내는 문자입니다.
        /// </summary>
        const char EndOfBlock = '}';
        /// <summary>
        /// 배열의 시작을 나타내는 문자입니다.
        /// </summary>
        const char StartOfArray = '[';
        /// <summary>
        /// 배열의 끝을 나타내는 문자입니다.
        /// </summary>
        const char EndOfArray = ']';

        public static JsonMap Parse(string s) {
            if (s == null) throw new ArgumentNullException("s");
            if (s[0] != StartOfBlock) throw new InvalidJsonFormatException($"s[0] != '{StartOfBlock}'");

            JsonMap map = new JsonMap();
            JsonElement element = null;
            int p = 1;
            while (true) {
                element = __internalParseWorker(s, false, p, out p);
                if (element == null) break;

                map.Add(element.Name, element.Value);
                if (s[p] == ElementSeparator) p++;
            }
            return map;
        }
        static JsonElement __internalParseWorker(string s, bool isArrayElement, int p, out int p_new) {
            if (p == (s.Length - 1)) {
                if (s[p] != EndOfBlock) throw new InvalidJsonFormatException($"Last character of minified json string is not '{EndOfBlock}'");
                p_new = 0;
                return null;
            }

            string test = s.Substring(p);
            string name = __getPropertyStringValue(s, p, out p);
            if (s[p++] != KeyValueSeparator) throw new InvalidJsonFormatException($"s[p++] != '{KeyValueSeparator}'");

            object value;
            JsonValueType valueType = s[p].GetValueType();
            if (valueType == JsonValueType.String) {
                value = __getPropertyStringValue(s, p, out p_new);
                return new JsonElement(name, value);
            } else if (valueType == JsonValueType.Boolean) {
                value = bool.Parse(__getPropertyPrimitiveValue(s, isArrayElement, p, out p_new));
                return new JsonElement(name, value);
            } else if (valueType == JsonValueType.Decimal) {
                value = decimal.Parse(__getPropertyPrimitiveValue(s, isArrayElement, p, out p_new));
                return new JsonElement(name, value);
            } else if (valueType == JsonValueType.Array)
                return new JsonElement(name, __internalParseArray(s, p + 1, out p_new));
            else if (valueType == JsonValueType.KeyValuePair) {
                JsonMap map = new JsonMap();
                JsonElement child = null;
                
                for (;;) {
                    child = __internalParseWorker(s, isArrayElement, p, out p);
                    if (child == null) break;

                    map.Add(child.Name, child.Value);

                    if (s[p] == EndOfBlock) break;
                    if (s[p] == ElementSeparator) p++;
                }

                p_new = p + 1;
                return new JsonElement(name, map);
            } else throw new InvalidJsonFormatException($"Unknown value type: '{s[p]}'");
        }
        static object[] __internalParseArray(string s, int p, out int p_new) {
            List<object> array = new List<object>();
            string test = s.Substring(p);

            object value = null;
            bool keyValueMode = false;
            for (;;) {
                if (s[p] == ']') break;
                if (s[p] == '}') keyValueMode = false;

                if (keyValueMode)
                    value = __internalParseWorker(s, true, p, out p);
                else {
                    JsonValueType valueType = s[p].GetValueType();
                    if (valueType == JsonValueType.Array)
                        value = __internalParseArray(s, p + 1, out p);
                    else if (valueType == JsonValueType.KeyValuePair) {
                        value = __internalParseWorker(s, true, p, out p);
                        keyValueMode = true;
                    } else if (valueType == JsonValueType.String)
                        value = __getPropertyStringValue(s, p, out p);
                    else if (valueType == JsonValueType.Boolean)
                        value = bool.Parse(__getPropertyPrimitiveValue(s, true, p, out p));
                    else if (valueType == JsonValueType.Decimal)
                        value = decimal.Parse(__getPropertyPrimitiveValue(s, true, p, out p));
                    else throw new InvalidJsonFormatException($"Unknown value type: '{s[p]}'");
                }

                array.Add(value);
                if (s[p] == EndOfArray) break;

                p++;
            }

            p_new = p;
            return array.ToArray();
        }
        static string __getPropertyPrimitiveValue(string s, bool isArrayElement, int p, out int p_new) {
            p_new = p;
            bool found = false;

            for (int i = p; i < s.Length; i++) {
                if (s[i] == EndOfBlock) {
                    if (isArrayElement) throw new InvalidJsonFormatException("isArrayElement and no ']' found, but block ended");
                    found = true;
                    p_new = i;
                    break;
                }

                if (s[i] == ElementSeparator) {
                    found = true;
                    p_new = i;
                    break;
                }

                if (s[i] == EndOfArray) {
                    if (!isArrayElement) throw new InvalidJsonFormatException("!isArrayElement but ']' found");
                    found = true;
                    p_new = i;
                    break;
                }
            }

            if (!found) throw new InvalidJsonFormatException("valeu never ends");
            return s.Substring(p, p_new - p);
        }
        static string __getPropertyStringValue(string s, int p, out int p_new, bool needConsiderEscapeSequence = false) {
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
    }

    class Program {
        //const string TestData = "{\"name\":\"Hye won, Hwang\",\"age\":25,\"hasJob\":true,\"isIntern\":true,\"isEmployeed\":false,\"graduated\":false,\"grade\":3,\"studentNo\":\"52121736\",\"department\":{\"location\":\"Cheon-an\",\"name\":\"Dankook University\",\"major\":\"Computer Science\"},\"last_score\":[{\"Network Programming\":\"A+\",\"Advanced Database\":\"B\",\"Software Architecture\":\"A\",\"Algorithm\":\"C+\",\"Operating System\":\"C+\",\"Programming I\":\"B\",\"Computer Network\":\"C\",\"Overall\":[\"A+\",\"A\",\"B\",\"B\",\"C+\",\"C+\",\"C\"],\"Adjusted\":[\"B+\",\"B\",\"B\",\"B\",\"B\",\"B\",\"B\"]},[\"no\",\"yes\",\"ok\",\"cancel\"]]}";
        //const string TestData = "{\"name\":{\"first\":\"hye won\",\"last\":\"hwang\",\"root\":{\"alias\":\"chang-won\",\"name\":\"hwang\"}}}";
        //const string TestData = "{\"name\":\"Apple 15.4 MacBook Pro Retina\",\"price\":3490000,\"cpu\":\"Intel 7th Gen 2.9GHz Quadcore processor, up to 3.9GHz with TurboBoost\",\"mem\":\"16GB LPDDR3 2,133MHz RAM\",\"storage\":\"512GB SSD\",\"gpu\":\"Radeon Pro 560 4GB VRAM\",\"ports\":\"4x Thunderbolt 3 port\",\"comments\":\"역시 애플껀 비싸...\"}";
        const string TestData = "{\"name\":{\"first\":\"hye won\",\"last\":\"hwang\",\"root\":{\"alias\":\"chang-won\",\"name\":\"hwang\"},\"next element\":\"this is a value\",\"another\":\"value\"},\"oh....?\":\"hello\"}";
        static void Main(string[] args) {
            JsonMap map = MinifiedJson.Parse(TestData);
            Inspect(map);
        }
        static void Inspect(JsonMap map, int depth = 0) {
            foreach (KeyValuePair<string, object> pair in map) {
                InternalInspect(pair, depth + 1);
            }
        }
        static void InternalInspect(KeyValuePair<string, object> pair, int depth) {
            string indent = new string(' ', depth * 2);
            Console.Write("{0}{1}: ", indent, pair.Key);
            if (pair.Value is JsonMap) {
                Console.WriteLine();
                JsonMap map = (JsonMap)pair.Value;
                Inspect(map, depth + 1);
            } else Console.WriteLine("{0}", pair.Value);
        }
    }
}