# 중첩된 키-값 쌍 요소
테스트에 사용된 json 데이터

**Raw source**:

    {
        "name":{
            "first":"hye won",
            "last":"hwang",
            "root":{
                "alias":"chang-won",
                "name":"hwang"
            }
        }
    }

**Minified source**:

`{"name":{"first":"hye won","last":"hwang","root":{"alias":"chang-won","name":"hwang"}}}`

----------

**Results**:

[Test Result Image](https://oss.navercorp.com/UGC-Xamarin/minified-json-parser/blob/master/test-nested-kvpair/result.png)

**Test Source codes**:

    const string TestData = "{\"name\":{\"first\":\"hye won\",\"last\":\"hwang\",\"root\":{\"alias\":\"chang-won\",\"name\":\"hwang\"}}}";
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
