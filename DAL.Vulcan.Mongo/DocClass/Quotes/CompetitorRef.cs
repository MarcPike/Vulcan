using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    [BsonIgnoreExtraElements]
    public class CompetitorRef : ReferenceObject<Competitor>
    {
        public string Name { get; set; }
        public CompetitorRef()
        {
            
        }

        public CompetitorRef(Competitor document) : base(document)
        {
            
        }

        public Competitor AsCompetitor()
        {
            return ToBaseDocument();
        }
    }
}