using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UGCXamarin.Utils.Json;

namespace ConsoleTest {
    class Program {
        static void Main(string[] args) {
            string path;
            while (true) {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("테스트할 JSON 파일이 있는 디렉터리 경로를 입력하세요(exit 입력 시 종료): ");
                path = Console.ReadLine();

                if (string.Equals(path, "exit")) return;
                if (!Directory.Exists(path)) {
                    Console.WriteLine("오류: 디렉터리가 존재하지 않습니다. 다시 입력하세요.");
                    continue;
                }

                DirectoryInfo directory = new DirectoryInfo(path);
                FileInfo[] files = directory.GetFiles("*.json");
                Console.WriteLine($"{files.Length} 개의 JSON 파일이 검사될 것입니다...");

                int pass = 0;
                string fileContent = null;
                for (int i = 0; i < files.Length; i++) {
                    using (StreamReader sr = files[i].OpenText()) fileContent = sr.ReadToEnd();

                    Console.Write("파일: {0,-50}", files[i].Name);
                    Stopwatch sw = new Stopwatch();
                    object result = null;
                    Exception e = null;
                    sw.Start();
                    try { result = JsonConverter.FromString(fileContent); }
                    catch (Exception ex) { e = ex; }

                    sw.Stop();
                    string elapsedTime = string.Format("{0:N3}s", sw.Elapsed.TotalSeconds);
                    Console.Write("{0,-8}", elapsedTime);

                    if (e == null) {
                        Success();
                        pass++;
                    } else {
                        Failure();
                        Console.WriteLine($"  >> {e.Message}");
                    }
                }

                Console.WriteLine("결과: {0}/{1} ({2:P2})", pass, files.Length, (float)pass / files.Length);

                Console.WriteLine("새 테스트를 시작하려면 아무 키나 누르세요!");
                Console.ReadKey(true);
                Console.Clear();
            }

        }
        static void Success() {
            Console.Write("[  ");
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("OK");
            Console.ForegroundColor = previous;
            Console.WriteLine("  ]");
        }
        static void Failure() {
            Console.Write("[ ");
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("FAIL");
            Console.ForegroundColor = previous;
            Console.WriteLine(" ]");
        }
    }
}