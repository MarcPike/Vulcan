using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class QngVSalesHeadersExt
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? OrderNumber { get; set; }
        public string BranchCode { get; set; }
        public string OrderTypeCode { get; set; }
        public string OrderTypeDescription { get; set; }
        public string OrderHeaderStatusCode { get; set; }
        public string OrderHeaderStatusDescription { get; set; }
        public int? InternalStatusId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroup { get; set; }
        public string CustomerSegment { get; set; }
        public string ContactName { get; set; }
        public string DeliverToCode { get; set; }
        public string DeliverToName { get; set; }
        public string TransferToBranchCode { get; set; }
        public string TransferToBranchDescription { get; set; }
        public string TransferToWarehouseCode { get; set; }
        public string TransferToWarehouseDescription { get; set; }
        public string TransportTypeDescription { get; set; }
        public string CarrierName { get; set; }
        public string DeliverFromBranchCode { get; set; }
        public string DeliverFromBranchName { get; set; }
        public string DeliverFromWarehouseCode { get; set; }
        public string DeliverFromWarehouseName { get; set; }
        public string SalesRep { get; set; }
        public string Salesperson { get; set; }
        public bool? HasChemicalCertificate { get; set; }
        public bool? HasMechanicalCertificate { get; set; }
        public bool? HasMillCertificate { get; set; }
        public bool? HasCertificateOfCompliance { get; set; }
        public int? CopiesWithDelivery { get; set; }
        public int? CopiesWithInvoice { get; set; }
        public string CurrencyCode { get; set; }
        public string TermsDescription { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? SaleDate { get; set; }
    }
}
