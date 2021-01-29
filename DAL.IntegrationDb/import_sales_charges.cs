namespace DAL.IntegrationDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class import_sales_charges
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

        [StringLength(3)]
        public string cost_group_code { get; set; }

        [StringLength(50)]
        public string description { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? charge { get; set; }

        [StringLength(3)]
        public string charge_unit_code { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? exchange_rate { get; set; }

        [StringLength(1)]
        public string charge_fix_status { get; set; }

        [StringLength(1)]
        public string charge_visibility { get; set; }

        public bool? confirm_at_invoicing { get; set; }
    }
}
