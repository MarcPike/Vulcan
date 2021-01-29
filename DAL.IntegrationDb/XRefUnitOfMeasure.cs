namespace DAL.IntegrationDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("XRefUnitOfMeasure")]
    public partial class XRefUnitOfMeasure
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string PartnerId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string PartnerUnitOfMeasure { get; set; }

        [StringLength(3)]
        public string HowcoUnitOfMeasure { get; set; }
    }
}
