using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.DocClass.Config
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
