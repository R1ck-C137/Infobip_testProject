using Npgsql;
using System;
using System.Data;
using System.Threading;

namespace Infobip_testProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connString = "Host=localhost;Username=postgres;Password=unit123456;Database=TestDB_Infobip";

            NpgsqlConnection nc = new NpgsqlConnection(connString);
            nc.Open();
            if (nc.FullState == ConnectionState.Broken || nc.FullState == ConnectionState.Closed)
            {
                Console.WriteLine("Сouldn't open the database");
                return;
            }
            //GetAllValues(nc);


            DbRow dbRow = new DbRow();
            enteringValues(dbRow);
            PostValue(nc,dbRow);
            
            Console.ReadKey();
        }

        private static void GetAllValues(NpgsqlConnection nc)
        {
            NpgsqlCommand npgc = new NpgsqlCommand("SELECT * FROM public.event_table ORDER BY personid ASC ", nc);
            
            NpgsqlDataReader ndr = npgc.ExecuteReader(); //Если запрос возвращает таблицу

            if (ndr.HasRows)
            {
                while (ndr.Read())
                {
                    object id = ndr.GetValue(0);
                    object exPersonId = ndr.GetValue(1);
                    object defid = ndr.GetValue(2);
                    object occuredTime = ndr.GetValue(3);
                    object eventId = ndr.GetValue(4);

                    Console.WriteLine("{0} \t{1} \t{2} \t{3} \t{4}", id, exPersonId, defid, occuredTime, eventId);
                }
            }
        }

        private static void PostValue(NpgsqlConnection nc, DbRow dbRow)
        {
            //NpgsqlCommand npgc = new NpgsqlCommand($"INSERT INTO public.event_table VALUES ( 75221, '44351', 'personCreated', '2022-09-22T06:50:32.270', 'create_487840_75221')", nc);
            NpgsqlCommand npgc = new NpgsqlCommand($"INSERT INTO public.event_table VALUES ({dbRow.Id}, '{dbRow.ExternalPersonID}', '{dbRow.DefinitionId}', '{dbRow.OccurredTime}', '{dbRow.EventId}')", nc);
            int rowsChanged = npgc.ExecuteNonQuery();   //Если запрос не возвращает таблицу

            Console.WriteLine(rowsChanged);
        }

        static void enteringValues(DbRow dbRow)
        {
            Console.WriteLine("Enter PersonId");
            dbRow.Id = Console.ReadLine();
            Console.WriteLine("Enter externalPersonID");
            dbRow.ExternalPersonID = Console.ReadLine();
            Console.WriteLine("Enter definitionId");
            dbRow.DefinitionId = Console.ReadLine();
            Console.WriteLine("Enter occurredTime");
            dbRow.OccurredTime = Console.ReadLine();
            Console.WriteLine("Enter eventId");
            dbRow.EventId = Console.ReadLine();
        }
    }

    public class DbRow
    {
        public object Id { get; set; }
        public object ExternalPersonID { get; set; }
        public object DefinitionId { get; set; }
        public object OccurredTime { get; set; }
        public object EventId { get; set; }
    }
}
