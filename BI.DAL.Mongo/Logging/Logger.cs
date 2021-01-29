using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.EMail;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Logger;
using DAL.Vulcan.Mongo.Base.Repository;

namespace BI.DAL.Mongo.Logging
{
    public class Logger : ILogger
    {
        private readonly LoggerConfig _loggerConfig;
        private readonly RepositoryBase<Log> _rep = new RepositoryBase<Log>();
        public Logger(LoggerConfig config = null)
        {
            _loggerConfig = config ?? new LoggerConfig();
        }

        public void Log(LogLevel logLevel, string className, string methodName, string message, bool sendEmail = false, Dictionary<string, object> parameters = null)
        {
            if (_loggerConfig.CaptureLogLevels.All(x => x != logLevel)) return;
            //var applicationLog = new Log()
            //{
            //    LogLevel = logLevel.ToString(),
            //    ClassName = className,
            //    MethodName = methodName,
            //    Message = message,
            //    CreatedByUserId = "vulcan",
            //    Parameters = parameters
            //};
            //_rep.Upsert(applicationLog);

            if (sendEmail)
            {
                var recipients = new List<string>() { "marc.pike@howcogroup.com" };
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

            Log(LogLevel.Error, className, methodName, message, sendEmail, parameters);
        }
    }

}
