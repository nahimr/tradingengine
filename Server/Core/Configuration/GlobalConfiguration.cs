namespace TradingEngine.Server.Core.Configuration
{
    class GlobalConfiguration
    {
        public ServerSettings ServerSettings { get; set; }

        public override string ToString()
        {
            return $"GlobalConfiguration:\nServerSettings:\n\tPort: {ServerSettings.Port}";
        }
    }

    class ServerSettings
    {
        public int Port { get; set; }
    }
}
