# 중첩된 키-값 쌍 및 배열 요소
테스트에 사용된 json 데이터

**Raw source**:

    {
        "name":"Hye won, Hwang",
        "age":25,
        "hasJob":true,
        "isIntern":true,
        "isEmployeed":false,
        "graduated":false,
        "grade":3,
        "studentNo":"52121736",
        "department":{
            "location":"Cheon-an",
            "name":"Dankook University",
            "major":"Computer Science"
        },
        "last_score":[
            {
                "Network Programming":"A+",
                "Advanced Database":"B",
                "Software Architecture":"A",
                "Algorithm":"C+",
                "Operating System":"C+",
                "Programming I":"B",
                "Computer Network":"C",
                "Overall":["A+","A","B","B","C+","C+","C"],
                "Adjusted":["B+","B","B","B","B","B","B"]
            },
            ["no", "yes", "ok", "cancel"]
        ]
    }

**Minified source**:

`{"name":"Hye won, Hwang","age":25,"hasJob":true,"isIntern":true,"isEmployeed":false,"graduated":false,"grade":3,"studentNo":"52121736","department":{"location":"Cheon-an","name":"Dankook University","major":"Computer Science"},"last_score":[{"Network Programming":"A+","Advanced Database":"B","Software Architecture":"A","Algorithm":"C+","Operating System":"C+","Programming I":"B","Computer Network":"C","Overall":["A+","A","B","B","C+","C+","C"],"Adjusted":["B+","B","B","B","B","B","B"]},["no","yes","ok","cancel"]]}`

----------

**Results**:

[Test Result Image](https://oss.navercorp.com/UGC-Xamarin/minified-json-parser/raw/master/test-nested-kvpair-and-array/result.png)

**Test Source codes**:

    const string TestData = "{\"name\":\"Hye won, Hwang\",\"age\":25,\"hasJob\":true,\"isIntern\":true,\"isEmployeed\":false,\"graduated\":false,\"grade\":3,\"studentNo\":\"52121736\",\"department\":{\"location\":\"Cheon-an\",\"name\":\"Dankook University\",\"major\":\"Computer Science\"},\"last_score\":[{\"Network Programming\":\"A+\",\"Advanced Database\":\"B\",\"Software Architecture\":\"A\",\"Algorithm\":\"C+\",\"Operating System\":\"C+\",\"Programming I\":\"B\",\"Computer Network\":\"C\",\"Overall\":[\"A+\",\"A\",\"B\",\"B\",\"C+\",\"C+\",\"C\"],\"Adjusted\":[\"B+\",\"B\",\"B\",\"B\",\"B\",\"B\",\"B\"]},[\"no\",\"yes\",\"ok\",\"cancel\"]]}";
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
