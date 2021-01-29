using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SpecCerthead
    {
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? Cuser { get; set; }
        public int? Muser { get; set; }
        public string State { get; set; }
        public string Branch { get; set; }
        public int? CertNumber { get; set; }
        public string CustNumber { get; set; }
        public string CustName { get; set; }
        public string CustAdd1 { get; set; }
        public string CustAdd2 { get; set; }
        public string CustAdd3 { get; set; }
        public string CustCity { get; set; }
        public string CustPostcode { get; set; }
        public string CustCountry { get; set; }
        public string CustFaxno { get; set; }
        public string OrderBranch { get; set; }
        public int? OrderNumber { get; set; }
        public int? OrderItem { get; set; }
        public string BolBranch { get; set; }
        public int? BolNumber { get; set; }
        public int? BolItem { get; set; }
        public string CustOrder { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? DelivDate { get; set; }
        public int? PartCustNumber { get; set; }
        public string PartNumber { get; set; }
        public string Mill { get; set; }
        public string CastNumber { get; set; }
        public string SendMethod { get; set; }
        public string EmailAddress { get; set; }
        public string Printer { get; set; }
        public DateTime? SendDate { get; set; }
        public string ItemDescription1 { get; set; }
        public string ItemDescription2 { get; set; }
        public string CreationType { get; set; }
        public bool? PrintCertsTo { get; set; }
        public bool? PrintMisc { get; set; }
        public bool? PrintExtras { get; set; }
        public string FaxNumber { get; set; }
        public string TestNumber { get; set; }
        public int? Pieces { get; set; }
        public int? Weight { get; set; }
        public string CertSource { get; set; }
        public string BuyerItemNumber { get; set; }
        public string SalesCategory { get; set; }
        public string PrintCertMsg { get; set; }
        public string PrintQtcMsg { get; set; }
        public string CertOrientation { get; set; }
        public string OrderType { get; set; }
        public string DespatchType { get; set; }
        public string OrderTypePostImport { get; set; }
    }
}
