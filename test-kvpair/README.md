# 일반 키-값 쌍 요소
테스트에 사용된 json 데이터

**Raw source**:

    {
        "name":"Apple 15.4 MacBook Pro Retina",
        "price":3490000.00,
        "cpu":"Intel 7th Gen 2.9GHz Quadcore processor, up to 3.9GHz with TurboBoost",
        "mem":"16GB LPDDR3 2,133MHz RAM",
        "storage":"512GB SSD",
        "gpu":"Radeon Pro 560 4GB VRAM",
        "ports":"4x Thunderbolt 3 port",
        "comments":"역시 애플껀 비싸..."
    }

**Minified source**:
`{"name":"Apple 15.4 MacBook Pro Retina","price":3490000,"cpu":"Intel 7th Gen 2.9GHz Quadcore processor, up to 3.9GHz with TurboBoost","mem":"16GB LPDDR3 2,133MHz RAM","storage":"512GB SSD","gpu":"Radeon Pro 560 4GB VRAM","ports":"4x Thunderbolt 3 port","comments":"역시 애플껀 비싸..."}`

----------

**Results**:

[Test Result Image](https://oss.navercorp.com/UGC-Xamarin/minified-json-parser/raw/master/test-kvpair/result.png)

**Test Source codes**:

    const string TestData = "{\"name\":\"Apple 15.4 MacBook Pro Retina\",\"price\":3490000,\"cpu\":\"Intel 7th Gen 2.9GHz Quadcore processor, up to 3.9GHz with TurboBoost\",\"mem\":\"16GB LPDDR3 2,133MHz RAM\",\"storage\":\"512GB SSD\",\"gpu\":\"Radeon Pro 560 4GB VRAM\",\"ports\":\"4x Thunderbolt 3 port\",\"comments\":\"역시 애플껀 비싸...\"}";
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