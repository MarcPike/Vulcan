using System;
using System.Collections.Generic;

namespace DAL.HRS.Mongo.Logger
{
    public interface IVulcanLogger
    {
        void Log(VulcanLogLevel logLevel, string className, string methodName, string message, bool sendEmail = false, Dictionary<string, object> parameters = null);
        void Log(string className, string methodName, Exception exception, bool sendEmail = false, Dictionary<string, object> parameters = null);
    }
}