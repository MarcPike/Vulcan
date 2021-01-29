using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Warehouses
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public bool? MandatoryLocation { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public int? WarehouseTypeId { get; set; }
        public int? BranchId { get; set; }
        public int? DefaultStockStatusId { get; set; }
        public int? AddressId { get; set; }
        public int? BayStartCharacter { get; set; }
        public int? BayLength { get; set; }
        public int? RackStartCharacter { get; set; }
        public int? RackLength { get; set; }
        public int? BinStartCharacter { get; set; }
        public int? BinLength { get; set; }
        public bool? SendMasterYardOrder { get; set; }
        public int? SubAddressId { get; set; }
        public bool? ProductionWarehouse { get; set; }
        public string LedgerSegmentCode { get; set; }
        public bool? RestrictedVisibility { get; set; }
        public int? DefaultCertPrinterId { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? GoodsInwardsPurchaseGroupOverrideId { get; set; }
    }
}
