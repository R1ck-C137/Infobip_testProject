using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UnpackingJSON
{
    internal class Program
    {
        private const string CompressedFileName = "../../archive.gz";

        static void Main(string[] args)
        {
            DecompressFile();
            Console.ReadKey();
        }

        private static void DecompressFile()
        {
            using (FileStream compressedFileStream = File.Open(CompressedFileName, FileMode.Open))
            {
                using (var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                {
                    Line line = new Line();
                    using (StreamReader r = new StreamReader(decompressor))
                    {
                        while (true)
                        {
                            if (!r.EndOfStream)
                            {
                                string json = r.ReadLine();
                                line = JsonConvert.DeserializeObject<Line>(json);
                                Console.WriteLine(
                                    $"{line.externalPersonId} \t {line.definitionId} \t {line.occurredTime} \t {line.personId} \t {line.eventId} \t {line.sessionId} \t {line.personType} \t {line.source}");
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
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
