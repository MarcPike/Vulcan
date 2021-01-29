using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class CompanyTotals
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public decimal? OpenOrderValue { get; set; }
        public decimal? TotalSalesMtd { get; set; }
        public decimal? TotalCostMtd { get; set; }
        public decimal? TotalWeightMtd { get; set; }
        public decimal? TotalSalesYtd { get; set; }
        public decimal? TotalCostYtd { get; set; }
        public decimal? TotalWeightYtd { get; set; }
        public decimal? OpenOrderTax { get; set; }
        public decimal? UnexportedInvoiceValue { get; set; }
        public decimal? UnexportedInvoiceTax { get; set; }
        public decimal? UncheckedOrderValue { get; set; }
        public decimal? UncheckedOrderTax { get; set; }
        public decimal? HeldOrderValue { get; set; }
        public decimal? HeldOrderTax { get; set; }
        public decimal? ExternalOpenOrderValue { get; set; }
        public decimal? ExternalOpenOrderTax { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
