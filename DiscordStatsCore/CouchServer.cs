using MyCouch;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordStatsCore
{
    class CouchServer : DatabaseServer
    {
        public Config Config { get; set; }

        public CouchServer(Config config)
        {
            Config = config;
        }

        async public void UpdateData(int aantalOnline, JArray jArray)
        {
            using (var client = new MyCouchClient(Config.ConnectionString, Config.DatabaseNaam))
            {
                await client.Documents.PostAsync($"{{\"tijdstip\":{DateTime.Now.Ticks},\"aantal\":{aantalOnline},\"personen\":{jArray.ToString()}}}");
            }
        }
    }
}
