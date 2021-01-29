using System;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    [BsonIgnoreExtraElements]
    public class QuoteRef : ReferenceObject<CrmQuote>
    {
        public int QuoteId { get; set; }
        public CompanyRef Company { get; set; }
        public PipelineStatus Status { get; set; }

        public DateTime CreateDateTime { get; set; }
        public DateTime? ReportDate { get; set; }

        public QuoteRef()
        {
            
        }

        public QuoteRef(CrmQuote doc) : base(doc)
        {
            QuoteId = doc.QuoteId;
            Company = doc.Company;
            Status = doc.Status;
            CreateDateTime = doc.CreateDateTime;
            ReportDate = doc.ReportDate;
        }

        public CrmQuote AsQuote()
        {
            return ToBaseDocument();
        }
    }
}