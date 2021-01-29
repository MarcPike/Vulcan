using DAL.Vulcan.Mongo.Base.Core.DocClass;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DocClass.Config
{
    public class BaseConfiguration: BaseDocument
    {
        public Configuration Configuration { get; set; }

        public void SetValue(string name, object data)
        {
            Configuration.SetValue(name, data);
        }
        public object GetValue(string name, object defaultData)
        {
            return Configuration.GetValue(name, defaultData);
        }

        public Dictionary<string, object> GetAllValues()
        {
            return Configuration.Tags;
        }

    }
}
