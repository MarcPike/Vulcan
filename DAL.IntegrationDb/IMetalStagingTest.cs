namespace DAL.IntegrationDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IMetalStagingTest")]
    public partial class IMetalStagingTest
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string CustomerOrderNumber { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string SourceTradingPartnerID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string DestinationTradingPartnerID { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Action { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Key]
        [Column(Order = 5)]
        public float ProductQuantity { get; set; }
    }
}
