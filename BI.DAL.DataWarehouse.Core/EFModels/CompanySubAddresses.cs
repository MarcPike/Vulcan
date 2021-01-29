using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class CompanySubAddresses
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? AddressId { get; set; }
        public int? TerritoryId { get; set; }
        public string Telephone { get; set; }
        public string FastDial { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public string PartnerCode { get; set; }
        public string TransportRateCode { get; set; }
        public int? ConsignmentBranchId { get; set; }
        public int? ConsignmentWarehouseId { get; set; }
        public int? UseCompanyTaxes { get; set; }
        public int? TaxClass1 { get; set; }
        public int? TaxClass2 { get; set; }
        public int? TaxClass3 { get; set; }
        public int? TaxClass4 { get; set; }
        public int? TaxClass5 { get; set; }
        public bool? CommercialTermsMandatory { get; set; }
        public int? DefaultCommercialTermsId { get; set; }
        public string TaxGroup { get; set; }
        public string TaxAuthority1 { get; set; }
        public string TaxAuthority2 { get; set; }
        public string TaxAuthority3 { get; set; }
        public string TaxAuthority4 { get; set; }
        public string TaxAuthority5 { get; set; }
        public string TaxRegistration1 { get; set; }
        public string TaxRegistration2 { get; set; }
        public string TaxRegistration3 { get; set; }
        public string TaxRegistration4 { get; set; }
        public string TaxRegistration5 { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public bool? DefaultAddress { get; set; }
        public bool? TestCertificateUseCompany { get; set; }
        public string TestCertificateSendMethod { get; set; }
        public string TestCertificateDestination { get; set; }
        public string TestCertificateHold { get; set; }
        public int? DefaultItemClass1 { get; set; }
        public int? DefaultItemClass2 { get; set; }
        public int? DefaultItemClass3 { get; set; }
        public int? DefaultItemClass4 { get; set; }
        public int? DefaultItemClass5 { get; set; }
        public int? OutworkWarehouseId { get; set; }
        public int? DefaultTransportTypeId { get; set; }
        public string TransportComment { get; set; }
        public bool? UseCompanyProductionRequirements { get; set; }
        public bool? InvoicePacking { get; set; }
        public decimal? OutsideDiameter { get; set; }
        public decimal? OutsideDiameterMinimum { get; set; }
        public decimal? OutsideDiameterMaximum { get; set; }
        public decimal? PackHeight { get; set; }
        public decimal? PackHeightMinimum { get; set; }
        public decimal? PackHeightMaximum { get; set; }
        public decimal? PackWeight { get; set; }
        public decimal? PackWeightMinimum { get; set; }
        public decimal? PackWeightMaximum { get; set; }
        public decimal? InsideDiameter { get; set; }
        public decimal? InsideDiameterMinimum { get; set; }
        public decimal? InsideDiameterMaximum { get; set; }
        public int? PackCountMinimum { get; set; }
        public int? PackCountMaximum { get; set; }
    }
}
