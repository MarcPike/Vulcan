using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Common.DocClass
{
    public class CountryValue: BaseDocument, ICommonDatabaseObject
    {
        public string CountryName { get; set; }
        public List<StateValue> States { get; set; } = new List<StateValue>();

        public static MongoRawQueryHelper<CountryValue> Helper = new MongoRawQueryHelper<CountryValue>();

        public CountryValue()
        {
            
        }
    }
}
