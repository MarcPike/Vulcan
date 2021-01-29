using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Vulcan.Mongo.DocClass.Email
{
    public class EmailExceptionLog: BaseDocument
    {
        public DateTime DateOf { get; set; } = DateTime.Now;
        public string Error { get; set; } = String.Empty;
        public string InsideError { get; set; } = String.Empty;
        public string StackTrace { get; set; } = String.Empty;

        public static MongoRawQueryHelper<EmailExceptionLog> Helper { get; set; } = new MongoRawQueryHelper<EmailExceptionLog>();

        public static void CreateEmailExceptionLog(Exception ex)
        {
            var log = new EmailExceptionLog()
            {
                Error = ex.Message,
                InsideError = ex.InnerException?.Message,
                StackTrace = Environment.StackTrace
            };
            log.SaveToDatabase();
        }
    }
}
