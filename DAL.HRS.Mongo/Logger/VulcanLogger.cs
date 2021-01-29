using System;
using System.Collections.Generic;
using System.Linq;
using BLL.EMail;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.Logger
{
    public class VulcanLogger : IVulcanLogger
    {
        private readonly VulcanLoggerConfig _loggerConfig;
        private readonly RepositoryBase<ApplicationLog> _rep = new RepositoryBase<ApplicationLog>();
        public VulcanLogger(VulcanLoggerConfig config = null)
        {
            _loggerConfig = config ?? new VulcanLoggerConfig();
        }

        public void Log(VulcanLogLevel logLevel, string className, string methodName, string message, bool sendEmail = false, Dictionary<string,object> parameters = null)
        {
            if (_loggerConfig.CaptureLogLevels.All(x => x != logLevel)) return;
            var applicationLog = new ApplicationLog()
            {
                LogLevel = logLevel.ToString(),
                ClassName = className,
                MethodName = methodName,
                Message = message,
                CreatedByUserId = "vulcan",
                Parameters = parameters
            };
            _rep.Upsert(applicationLog);

            if (sendEmail)
            {
                var recipients = new List<string>() {"shannen.reese@howcogroup.com", "marc.pike@howcogroup.com", "isidro.gallegos@howcogroup.com", "rebecca.levine@howcogroup.com"};

                var subject = $"Exception occurred {className}.{methodName} Environment: {EnvironmentSettings.CurrentEnvironment.ToString()}";
                var body = message; 
                if (parameters != null)
                {
                    body = body + "\n\nParameters";
                    foreach (var parametersKey in parameters.Keys)
                    {
                        body += $"\n\t{parametersKey} => {parameters[parametersKey]}";
                    }
                }


                EMailSupport.SendEmailToSupport(
                    subject: subject, 
                    emailRecipients: recipients, 
                    body: body
                    );
            }
        }

        public void Log(string className, string methodName, Exception exception, bool sendEmail = false, Dictionary<string, object> parameters = null)
        {
            var message = exception.Message;
            if (exception.InnerException != null)
            {
                message += "\n" + exception.InnerException;
            }

            Log(VulcanLogLevel.Error, className, methodName, message, sendEmail, parameters);
        }
    }
}