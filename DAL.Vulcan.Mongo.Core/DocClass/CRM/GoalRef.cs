using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    [BsonIgnoreExtraElements]
    public class GoalRef : ReferenceObject<Goal>
    {
        public string Label { get; set; }
        public GoalRef(Goal document) : base(document)
        {
        }

        public Goal AsGoal()
        {
            return ToBaseDocument();
        }

        public GoalRef()
        {
            
        }
    }
}