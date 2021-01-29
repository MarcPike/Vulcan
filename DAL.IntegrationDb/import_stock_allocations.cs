namespace DAL.IntegrationDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class import_stock_allocations
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string import_company_reference { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string import_source { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int import_batch_number { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(6)]
        public string stock_branch_code { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(16)]
        public string stock_item_number { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(100)]
        public string composite_key { get; set; }

        [StringLength(1)]
        public string import_status { get; set; }

        [Column(TypeName = "text")]
        public string import_notes { get; set; }

        [StringLength(50)]
        public string import_user_name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? import_date { get; set; }

        [StringLength(1)]
        public string import_action { get; set; }

        [StringLength(1)]
        public string allocation_type { get; set; }

        [StringLength(6)]
        public string allocation_branch_code { get; set; }

        public int? allocation_import_number { get; set; }

        public int? allocation_header_number { get; set; }

        public int allocation_item_number { get; set; }

        public int? allocated_pieces { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? allocated_quantity { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? allocated_weight { get; set; }

        [Column(TypeName = "text")]
        public string comments { get; set; }

        public bool? firm { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? packing_weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? invoice_weight { get; set; }

        [Required]
        [StringLength(1)]
        public string automated_process_type { get; set; }
    }
}
