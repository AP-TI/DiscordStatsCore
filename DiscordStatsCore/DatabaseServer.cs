﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordStatsCore
{
    public interface DatabaseServer
    {
        void UpdateData(int aantalOnline, JArray jArray);
    }
}
