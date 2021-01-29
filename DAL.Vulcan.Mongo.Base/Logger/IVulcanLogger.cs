using System;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Base.Logger
{
    public interface IVulcanLogger
    {
        void Log(VulcanLogLevel logLevel, string className, string methodName, string message, bool sendEmail = false, bool writeToEventLog = false, Dictionary<string, object> parameters = null);
        void Log(string className, string methodName, Exception exception, bool sendEmail = false, bool writeToEventLog = false, Dictionary<string, object> parameters = null);
    }
}