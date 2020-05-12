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
                DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime utcNow = DateTime.UtcNow;
                TimeSpan elapsedTime = utcNow - unixEpoch;
                double millis = Math.Round(elapsedTime.TotalMilliseconds);
                await client.Documents.PostAsync($"{{\"tijdstip\":{millis},\"aantal\":{aantalOnline},\"personen\":{jArray.ToString()}}}");
            }
        }
    }
}
