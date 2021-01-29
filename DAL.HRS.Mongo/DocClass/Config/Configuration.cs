using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Config
{
    public class Configuration: ObjectWithTags
    {
        public string JsonData { get; set; } = string.Empty;

        public void SetValue(string name, object data)
        {
            SetTagValue(name, data);
        }

        public object GetValue(string name, object defaultData)
        {
            return GetTagValue(name, defaultData);
        }

        public Configuration()
        {

        }
    }
}
