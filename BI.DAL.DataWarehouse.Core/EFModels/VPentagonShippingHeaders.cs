using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VPentagonShippingHeaders
    {
        public string Coid { get; set; }
        public string DespatchBranchCode { get; set; }
        public int? DespatchNumber { get; set; }
        public string DespatchStatusCode { get; set; }
        public DateTime? DateRaised { get; set; }
        public DateTime? DateDespatched { get; set; }
        public DateTime? DateEta { get; set; }
        public DateTime? DatePrinted { get; set; }
        public DateTime? DateDue { get; set; }
        public string Soreference { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTown { get; set; }
        public string CustomerCounty { get; set; }
        public string CustomerPostalCode { get; set; }
        public string CustomerArea { get; set; }
        public string CustomerCountry { get; set; }
        public string DeliverToCode { get; set; }
        public string DeliverToName { get; set; }
        public string DeliverTo { get; set; }
        public string DeliverToAddress { get; set; }
        public string DeliverToTown { get; set; }
        public string DeliverToCounty { get; set; }
        public string DeliverToPostalCode { get; set; }
        public string DeliverToArea { get; set; }
        public string DeliverToCountry { get; set; }
        public string TransportTypeCode { get; set; }
        public string TransportType { get; set; }
        public string VehicleRegistration { get; set; }
        public string DeliverFromBranchCode { get; set; }
        public string DeliverFromBranch { get; set; }
        public string DeliverFromWarehouseCode { get; set; }
        public string DeliverFromWarehouse { get; set; }
        public string DeliverFromAddress { get; set; }
        public string DeliverFromTown { get; set; }
        public string DeliverFromCounty { get; set; }
        public string DeliverFromPostalCode { get; set; }
        public string DeliverFromArea { get; set; }
        public string DeliverFromCountry { get; set; }
    }
}
