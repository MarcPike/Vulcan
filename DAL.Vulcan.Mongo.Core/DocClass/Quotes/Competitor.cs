
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Queries;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class Competitor : BaseDocument
    {
        public static MongoRawQueryHelper<Competitor> Helper = new MongoRawQueryHelper<Competitor>();
        public string Name { get; set; }

        public CompetitorRef AsCompetitorRef()
        {
            return new CompetitorRef(this);
        }
    }
}