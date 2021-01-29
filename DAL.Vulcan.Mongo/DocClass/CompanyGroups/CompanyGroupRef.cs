using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.CompanyGroups
{
    [BsonIgnoreExtraElements]

    public class CompanyGroupRef: ReferenceObject<CompanyGroup>
    {
        public string Name { get; set; }


        public CompanyGroup AsCompanyGroup()
        {
            return ToBaseDocument();
        }

        public CompanyGroupRef(CompanyGroup document) : base(document)
        {
        }

        public CompanyGroupRef()
        {
            
        }
    }

}
