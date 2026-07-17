using System;
using System.Collections.Generic;
using System.Text;

namespace RfidReader.Configurations
{
    public class ProgramSettings
    {
        public string ServerName { get; set; }
        public int Port { get; set; }
        public string BaseRoute { get; set; }

        public string ServiceUrl { get; set; }

        public ProgramSettings()
        {
            ServerName = "192.168.43.123";
            Port = 7228;
            BaseRoute = "api/Users";
            ServiceUrl = $"https://{ServerName}:{Port}/{BaseRoute}";
        }
    }
}
