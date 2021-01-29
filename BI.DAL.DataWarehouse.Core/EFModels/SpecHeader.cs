using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SpecHeader
    {
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? Cuser { get; set; }
        public int? Muser { get; set; }
        public string State { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string OrderBranch { get; set; }
        public string OrderNumber { get; set; }
        public string OrderItem { get; set; }
        public string MaterialSpecCode { get; set; }
        public string HeatTreatmentCode { get; set; }
        public string TestSpecCode1 { get; set; }
        public string TestSpecCode2 { get; set; }
        public string TestSpecCode3 { get; set; }
        public string TestSpecCode4 { get; set; }
        public string CustomerNumber { get; set; }
        public string EditHistory { get; set; }
        public DateTime? SendDate { get; set; }
        public string SendMethod { get; set; }
        public string EmailAddress { get; set; }
        public string FaxNumber { get; set; }
        public string Printer { get; set; }
        public string OrderType { get; set; }
        public string OrderTypePostImport { get; set; }
    }
}
