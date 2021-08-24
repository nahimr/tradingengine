using System;

namespace TradingEngineServer.Core.Configuration
{
    class GlobalConfiguration
    {
        public ServerSettings ServerSettings { get; set; }

        public override string ToString()
        {
            return $"Configuration:\n\tPort: {ServerSettings.Port}";
        }
    }

    class ServerSettings
    {
        public int Port { get; set; }
    }
}
