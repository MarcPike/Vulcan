using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class LostReason: BaseDocument
    {
        public static MongoRawQueryHelper<LostReason> Helper = new MongoRawQueryHelper<LostReason>();
        public string Reason { get; set; }

        public LostReason()
        {
        }


    }
}
