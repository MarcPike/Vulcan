using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.DocClass;

namespace DAL.Vulcan.Mongo.Core.DocClass.QueueSchedule
{
    public class QueueError: BaseDocument
    {
        public DateTime DateOf { get; set; } = DateTime.Now;
        public string Error { get; set; } = String.Empty;
        public string InsideError { get; set; } = String.Empty;
        public string StackTrace { get; set; } = String.Empty;

        public static void CreateEmailExceptionLog(Exception ex)
        {
            var error = new QueueError()
            {
                Error = ex.Message,
                InsideError = ex.InnerException?.Message,
                StackTrace = Environment.StackTrace
            };
            error.SaveToDatabase();
        }

    }
}
