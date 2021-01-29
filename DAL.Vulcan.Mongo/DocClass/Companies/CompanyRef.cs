using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

//using DAL.iMetal.Queries;
//using DAL.iMetal.SearchResults;
//using DAL.iMetal.Views;

namespace DAL.Vulcan.Mongo.DocClass.Companies
{
    [BsonIgnoreExtraElements]
    [Serializable]
    public class CompanyRef: ReferenceObject<Company>
    {

        public int SqlId { get; set; }
        public string Coid { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Branch { get; set; }

        public bool IsActive { get; set; } = true;

        public string AddressLine1 { get; set; } 
        public string AddressLine2 { get; set; } 
        public string City { get; set; } 
        public string County { get; set; } 
        public string StateProvince { get; set; } 
        public string PostalCode { get; set; } 
        public string Country { get; set; } 


        public string CodePlusName => Code + " - " + Name;

        public Company AsCompany()
        {
            return ToBaseDocument();
        }

        public CompanyRef(Company document) : base(document)
        {
            Coid = document.Location.GetCoid();
        }

        public CompanyRef()
        {
        }

    }

}