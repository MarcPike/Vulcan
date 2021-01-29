namespace DAL.IntegrationDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("XRefCustomerId")]
    public partial class XRefCustomerId
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string CustomerId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(32)]
        public string CustomerIdForHowco { get; set; }

        [StringLength(32)]
        public string HowcoIdForCustomer { get; set; }

        [StringLength(8)]
        public string HowcoBranchForCustomer { get; set; }

        [StringLength(30)]
        public string HowcoCompanyForCustomer { get; set; }
    }
}
