﻿namespace TradingEngineServer.Logging.Configuration
{
    public class LoggingConfiguration
    {
        public LoggerType LoggerType { get; set; }
        // public DatabaseLoggerConfiguration DatabaseLoggerConfiguration { get; set; }
        public TextLoggerConfiguration TextLoggerConfiguration { get; set; }
    }

    public class DatabaseLoggerConfiguration
    {

    }

    public class TextLoggerConfiguration
    {
        public string Directory { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}