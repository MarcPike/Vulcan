using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.Companies;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.CompanyGroups
{
    public class CompanyGroupUpdate
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExecutedOn { get; set; } = new DateTime();
        public List<CompanyRef> AddedCompanies { get; set; } = new List<CompanyRef>();
    }
}