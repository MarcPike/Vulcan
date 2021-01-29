using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VHalliburtonBillingItems
    {
        public int Id { get; set; }
        public string ViewId { get; set; }
        public int HowcoBatchNumber { get; set; }
        public string Spreadsheet { get; set; }
        public string Salesperson { get; set; }
        public DateTime PostDate { get; set; }
        public string HalliburtonPartNumber { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityUom { get; set; }
        public int MovementCode { get; set; }
        public string MaterialDoc { get; set; }
        public int MaterialDocItem { get; set; }
        public string BillOfLading { get; set; }
        public string BatchNumber { get; set; }
        public string HeatNumber { get; set; }
        public string Plant { get; set; }
        public string Sloc { get; set; }
        public string StorBin { get; set; }
        public string Ind { get; set; }
        public string VendorNumber { get; set; }
        public string Ponumber { get; set; }
        public string PoitemNumber { get; set; }
        public string JobNumber { get; set; }
        public string HowcoTagNumbers { get; set; }
        public int? StockItemId { get; set; }
        public string WarehouseCode { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerNumber { get; set; }
        public int? ProductId { get; set; }
        public string ProductCode { get; set; }
        public string HowcoStatusCode1 { get; set; }
        public string HowcoStatusCode1Desc { get; set; }
        public string HowcoStatusCode2 { get; set; }
        public string HowcoStatusCode2Desc { get; set; }
        public bool BatchFailed { get; set; }
        public string ImetalImportCompanyReference { get; set; }
        public int? ImetalImportBatchNumber { get; set; }
        public string ImetalOrderNumber { get; set; }
        public int? ImetalLineNumber { get; set; }
        public string ImetalStatusCode { get; set; }
        public string ImetalStatusCodeDesc { get; set; }
        public string ImetalStatusNotes { get; set; }
        public DateTime? ImetalStatusUpdateTimeUtc { get; set; }
        public byte ManualProcessing { get; set; }
        public string TagDiagnostics { get; set; }
        public string Notes { get; set; }
        public decimal? Price { get; set; }
        public string PriceUom { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
    }
}
