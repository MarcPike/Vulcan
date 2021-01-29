namespace DAL.IntegrationDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class import_sales_costs
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int import_number { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int import_item { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int item_no { get; set; }

        public string import_notes { get; set; }

        public bool? internal_cost { get; set; }

        [StringLength(3)]
        public string cost_group_code { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? cost { get; set; }

        [StringLength(3)]
        public string cost_unit_code { get; set; }

        [StringLength(3)]
        public string po_branch_code { get; set; }

        public int? po_number { get; set; }

        public int? po_item { get; set; }

        [StringLength(10)]
        public string supplier_code { get; set; }

        [StringLength(50)]
        public string billing_reference { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? exchange_rate { get; set; }

        public int? exchange_rate_type { get; set; }

        [StringLength(50)]
        public string description { get; set; }

        [StringLength(3)]
        public string purchase_group_code { get; set; }

        [StringLength(1)]
        public string cost_fix_status { get; set; }

        [StringLength(1)]
        public string visibility { get; set; }
    }
}
