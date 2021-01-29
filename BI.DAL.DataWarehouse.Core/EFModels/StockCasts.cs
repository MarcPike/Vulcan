using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class StockCasts
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int MuserId { get; set; }
        public string Status { get; set; }
        public int? MillId { get; set; }
        public string CastNumber { get; set; }
        public string OtherReference { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public int? ProductId { get; set; }
        public int? GradeId { get; set; }
        public string CertNumber { get; set; }
        public string DeliveryReference { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string Comments { get; set; }
        public string ImageIndex { get; set; }
        public int? CountryOfMaterialOriginId { get; set; }
        public int? CountryOfPrimaryProcessingId { get; set; }
        public int? CountryOfFinalProcessingId { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
    }
}
