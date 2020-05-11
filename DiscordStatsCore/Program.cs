using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace DiscordStatsCore
{
    public enum Database { CouchDB, MongoDB, MySQL }
    class MainClass
    {
        public static void Main(string[] args)
        {
            Config config;
            try
            {
                config = JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText(@".discordstatsconfig.json"));
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.Write("Kies database server; CouchDB(1), MongoDB (2), MySQL (3): ");
                Database database = (Console.ReadLine()) switch
                {
                    "1" => Database.CouchDB,
                    "2" => Database.MongoDB,
                    _ => Database.MySQL,
                };
                Console.Write($"Geef {database} server IP (meestal localhost): ");
                string serverIP = Console.ReadLine();
                Console.Write($"Geef {database} database naam: ");
                string databaseNaam = Console.ReadLine();
                Console.Write($"Geef {database} {(database == Database.MySQL ? "Uid (meestal root)" : "gebruikersnaam")}: ");
                string uid = Console.ReadLine();
                Console.Write($"Geef {database} wachtwoord: ");
                string wachtwoord = WachtwoordInvoer();
                Console.WriteLine();
                Console.Write("Geef discord server ID: ");
                string discordServerID = Console.ReadLine();
                var connect = database switch
                {
                    Database.MongoDB => $"mongodb://{(uid != "" ? $"{uid}:{wachtwoord}@" : "")}{serverIP}:27017/{databaseNaam}",
                    Database.MySQL => $"Server={serverIP};Database={databaseNaam};Uid={uid};Pwd={wachtwoord};",
                    _ => $"http://{Uri.EscapeDataString(uid)}:{Uri.EscapeDataString(wachtwoord)}@{serverIP}:5984",
                };
                config = new Config(connect, discordServerID, database, databaseNaam);
                string configString = JsonConvert.SerializeObject(config);
                using StreamWriter streamWriter = File.CreateText(@".discordstatsconfig.json");
                streamWriter.WriteLine(configString);
            }
            TelOnline telOnline = new TelOnline(config);
        }

        private static string WachtwoordInvoer()
        {
            StringBuilder invoer = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo toets = Console.ReadKey(true);
                if (toets.Key == ConsoleKey.Enter)
                    break;
                if (toets.Key == ConsoleKey.Backspace && invoer.Length > 0)
                    invoer.Remove(invoer.Length - 1, 1);
                else if (toets.Key != ConsoleKey.Backspace)
                    invoer.Append(toets.KeyChar);
            }
            return invoer.ToString();
        }

    }
}
