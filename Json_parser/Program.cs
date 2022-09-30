using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Json_parser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LoadJson();
        }

        public static void LoadJson()
        {
            //List<Line> items;

            Line line = new Line();
            using (StreamReader r = new StreamReader("../../file1.json"))
            {
                while (true) 
                {
                    if (!r.EndOfStream)
                    {
                        string json = r.ReadLine();
                        line = JsonConvert.DeserializeObject<Line>(json);
                        Console.WriteLine($"{line.externalPersonId} \t|\t {line.definitionId} \t|\t {line.occurredTime}");
                    }
                    else
                    {
                        break;
                    }
                }
            }
            Console.ReadKey();
        }

        public class Line
        {
            public string externalPersonId;
            public string definitionId;
            public DateTime occurredTime;
            public int personId;
            public string eventId;
            public string sessionId;
            public string personType;
            public string source;
        }
    }
}
