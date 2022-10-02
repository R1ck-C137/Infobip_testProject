using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;
using Npgsql;

namespace UnpackingJSON
{
    internal class Program
    {
        private const string CompressedFileName = "../../archive.gz";

        static void Main(string[] args)
        {
            DecompressFile();

            Console.WriteLine("Done");
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
                        NpgsqlConnection nc = Connect();
                        while (true)
                        {
                            if (!r.EndOfStream)
                            {
                                string json = r.ReadLine();
                                
                                line = JsonConvert.DeserializeObject<Line>(json);
                                PostValue(nc, line);
                            }
                            else
                            {
                                break;
                            }
                        }
                        nc.Close();
                    }
                }
            }
        }
        private static void PostValue(NpgsqlConnection nc, Line line)
        {
            NpgsqlCommand npgc = new NpgsqlCommand($"INSERT INTO public.event_table_test (eventid, eventname, userversion, personid, createdate, properties) VALUES ('{line.eventId}', '{line.definitionId}', {Convert.ToInt32(line.externalPersonId)}, {Convert.ToInt32(line.personId)}, '{line.occurredTime}', '{{\"personType\":\"{line.personType}\",\"source\":\"{line.source}\"}}')", nc);
            int rowsChanged = npgc.ExecuteNonQuery();   //Если запрос не возвращает таблицу
        }

        public static NpgsqlConnection Connect()
        {
            string connString = "Host=localhost;Username=postgres;Password=unit123456;Database=Infobip";

            NpgsqlConnection nc = new NpgsqlConnection(connString);
            nc.Open();
            if (nc.FullState == ConnectionState.Broken || nc.FullState == ConnectionState.Closed)
            {
                Console.WriteLine("Сouldn't open the database");
            }

            return nc;
        }
    }

    public class Line
    {
        public string definitionId { get; set; }
        public string customeventid { get; set; }
        public string externalPersonId { get; set; }//int
        public string personId { get; set; }//int
        public DateTime occurredTime { get; set; }
        public string eventId { get; set; }
        public string personType { get; set; }
        public string source { get; set; }
    }
}
