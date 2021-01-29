using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class DespatchItems
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? DespatchHeaderId { get; set; }
        public int? ItemNumber { get; set; }
        public int? OrderItemId { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int? DeliveredPieces { get; set; }
        public decimal? DeliveredQuantity { get; set; }
        public decimal? DeliveredWeight { get; set; }
        public decimal? PackingWeight { get; set; }
        public decimal? InvoiceWeight { get; set; }
        public int? StatusId { get; set; }
        public bool? Completed { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string Type { get; set; }
        public bool? MechanicalCert { get; set; }
        public bool? ShowCountryOfMaterialOrigin { get; set; }
        public bool? ShowCountryOfPrimaryProcessing { get; set; }
        public bool? ShowCountryOfFinalProcessing { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
