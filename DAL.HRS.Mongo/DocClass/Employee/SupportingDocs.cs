using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.DocClass.Employee
{

    public class SupportingDocs
    {

        public PropertyValueRef DocType { get; set; }
        public string DocDate { get; set; }
        public string File { get; set; }
        public string Comments { get; set; }
    }
}