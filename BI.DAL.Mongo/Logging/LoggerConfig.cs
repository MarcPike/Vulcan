using System.Collections.Generic;

namespace BI.DAL.Mongo.Logging
{
    public class LoggerConfig
    {
        public List<LogLevel> CaptureLogLevels { get; set; } = new List<LogLevel>()
        {
            LogLevel.Critical,
            LogLevel.Error,
            LogLevel.Debug,
            LogLevel.Trace,
            LogLevel.Warning,
            LogLevel.Information
        };
    }
}