using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DAO
{
    public class AppConfig
    {
        private string m_file_name;
        private JObject m_configRoot;

        internal static readonly AppConfig Instance = new AppConfig();
        public string ConnectionString { get; set; }
        public string AppName { get; set; }
        public float Version { get; set; }
        private AppConfig()
        {
            Init();
        }

        internal void Init(string file_name = @"C:\projects\HackerU\FlightsProject\DAO\ConfigFile.json")
        {
            m_file_name = file_name;

            if (!File.Exists(m_file_name))
            {
                Environment.Exit(-1);
            }

            var reader = File.OpenText(m_file_name);
            string json_string = reader.ReadToEnd();

            JObject jo = (JObject)JsonConvert.DeserializeObject(json_string);
            m_configRoot = (JObject)jo["Website"];
            ConnectionString = m_configRoot["ConnectionString"].Value<string>();
            AppName = m_configRoot["AppName"].Value<string>();
            Version = m_configRoot["Version"].Value<float>();
        }
    }
}
