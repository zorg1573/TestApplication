using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Text.Json;
using TestApp.MODEL;

namespace TestApp.DAL
{
    public static class DbConfigHelper
    {
        private static readonly string configPath = "dbconfig.json";

        public static void SaveConfig(DbConfig config)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(configPath, json);
        }

        public static DbConfig LoadConfig()
        {
            if (!File.Exists(configPath)) return null;

            string json = File.ReadAllText(configPath);
            return JsonSerializer.Deserialize<DbConfig>(json);
        }

    }
}
