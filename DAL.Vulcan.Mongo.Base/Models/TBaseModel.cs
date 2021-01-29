using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Vulcan.Mongo.Base.Models
{
    public class BaseModel<TBaseDocument> where TBaseDocument : BaseDocument
    {
        public string Id { get; set; }

        public TBaseDocument GetBaseDocument()
        {
            var queryHelper = new MongoRawQueryHelper<TBaseDocument>();
            return queryHelper.Find(Id).SingleOrDefault();
        }
    }
}
