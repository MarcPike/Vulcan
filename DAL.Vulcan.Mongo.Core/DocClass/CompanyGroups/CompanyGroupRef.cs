using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.CompanyGroups
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
