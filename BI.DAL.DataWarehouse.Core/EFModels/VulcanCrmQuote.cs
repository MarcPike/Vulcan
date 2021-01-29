using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VulcanCrmQuote
    {
        public int QuoteId { get; set; }
        public string ObjectId { get; set; }
        public string Coid { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string ProspectName { get; set; }
        public int RevisionNumber { get; set; }
        public string TeamName { get; set; }
        public bool IsProspect { get; set; }
        public string QuoteStatus { get; set; }
        public string ExportImetalStatus { get; set; }
        public string ExportRequestedBy { get; set; }
        public int SalesOrderId { get; set; }
        public string CustomerNotes { get; set; }
        public string SalesPersonNotes { get; set; }
        public string SalesPerson { get; set; }
        public bool IsLost { get; set; }
        public string LostReason { get; set; }
        public string PaymentTerms { get; set; }
        public string FreightTerms { get; set; }
        public int ValidityDays { get; set; }
        public string RfqNumber { get; set; }
        public string SalesGroupCode { get; set; }
        public DateTime SubmitDateTimeUtc { get; set; }
        public DateTime WonDateTimeUtc { get; set; }
        public DateTime? ReportDateTimeUtc { get; set; }
        public DateTime CreateDateTimeUtc { get; set; }
        public DateTime ModifiedDateTimeUtc { get; set; }
        public DateTime ImportDateTimeUtc { get; set; }
        public DateTime? LostDateTimeUtc { get; set; }
        public DateTime? ExpiredDateTimeUtc { get; set; }
        public string QuoteLinkId { get; set; }
        public string QuoteLinkType { get; set; }
        public bool Bid { get; set; }
        public string OrderClassificationCode { get; set; }
        public string OrderClassificationDescription { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactMiddleName { get; set; }
        public string ContactEmailAddress { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string LostComments { get; set; }
    }
}
