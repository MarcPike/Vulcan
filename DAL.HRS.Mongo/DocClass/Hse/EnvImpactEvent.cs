using DAL.HRS.Mongo.DocClass.Properties;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class EnvImpactEvent
    {
        public string Comment { get; set; }
        public PropertyValueRef Type { get; set; }
    }
}