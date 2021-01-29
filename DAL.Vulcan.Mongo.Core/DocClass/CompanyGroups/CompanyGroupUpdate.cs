using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.CompanyGroups
{
    public class CompanyGroupUpdate
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExecutedOn { get; set; } = new DateTime();
        public List<CompanyRef> AddedCompanies { get; set; } = new List<CompanyRef>();
    }
}