using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
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