using System.Linq;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Queries;

namespace DAL.Vulcan.Mongo.Base.Core.Models
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
