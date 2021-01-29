using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Base.Logger
{
    public class VulcanLoggerConfig
    {
        public List<VulcanLogLevel> CaptureLogLevels { get; set; } = new List<VulcanLogLevel>()
        {
            VulcanLogLevel.Critical,
            VulcanLogLevel.Error,
            VulcanLogLevel.Debug,
            VulcanLogLevel.Trace,
            VulcanLogLevel.Warning,
            VulcanLogLevel.Information
        };
    }
}