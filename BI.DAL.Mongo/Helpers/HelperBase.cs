using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.Encryption;
using DAL.Vulcan.Mongo.Base.Logger;

namespace BI.DAL.Mongo.Helpers
{
 
    public class HelperBase
    {
        protected Encryption _encryption = Encryption.NewEncryption;

        public VulcanLogger Logger { get; set; } = new VulcanLogger();

        public Dictionary<string, object> GetParametersDictionary()
        {
            return new Dictionary<string, object>();
        }

        public string GetClassName()
        {
            return this.GetType().Name;
        }

    }
}
