using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.Core.Queries;

namespace DAL.Vulcan.Mongo.Base.Core.DocClass
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
