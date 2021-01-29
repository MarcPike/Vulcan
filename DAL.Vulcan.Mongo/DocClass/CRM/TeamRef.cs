using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.Locations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.CRM
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