using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Vulcan.Mongo.Base.DocClass
{
    public class ApplicationLog: BaseDocument
    {
        public static MongoRawQueryHelper<ApplicationLog> Helper = new MongoRawQueryHelper<ApplicationLog>();
        public string ClassName { get; set; } = string.Empty;
        public string MethodName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string LogLevel { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
}
