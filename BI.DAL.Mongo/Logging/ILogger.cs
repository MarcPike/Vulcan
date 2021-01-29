using System;
using System.Collections.Generic;

namespace BI.DAL.Mongo.Logging
{
    public interface ILogger
    {
        void Log(LogLevel logLevel, string className, string methodName, string message, bool sendEmail = false, Dictionary<string, object> parameters = null);
        void Log(string className, string methodName, Exception exception, bool sendEmail = false, Dictionary<string, object> parameters = null);
    }
}