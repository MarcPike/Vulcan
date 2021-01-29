using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.Logger;
using DAL.Vulcan.Mongo.Base.Encryption;


namespace DAL.HRS.Mongo.Helpers
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
