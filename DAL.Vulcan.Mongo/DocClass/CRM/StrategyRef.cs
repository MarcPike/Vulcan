using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    [BsonIgnoreExtraElements]
    public class StrategyRef: ReferenceObject<Strategy>
    {
        public string Label { get; set; }
        public StrategyRef(Strategy document) : base(document)
        {
        }

        public StrategyRef()
        {
            
        }

        public Strategy AsStrategy()
        {
            return ToBaseDocument();
        }
    }

}
