using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    [BsonIgnoreExtraElements]
    public class TeamRef: ReferenceObject<Team>
    {
        public string Name { get; set; }
        public Team AsTeam()
        {
            return ToBaseDocument();
        }

        public TeamRef()
        {
            
        }

        public TeamRef(Team document) : base(document)
        {
        }
    }
}