namespace DAL.IntegrationDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class import_sales_items
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

        public string import_notes { get; set; }

        [StringLength(10)]
        public string part_customer_code { get; set; }

        [StringLength(35)]
        public string part_specification_code { get; set; }

        [StringLength(24)]
        public string product_code { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim1_negative_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim1_positive_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim2_negative_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim2_positive_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim3_negative_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim3_positive_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim4 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim4_negative_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim4_positive_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim5 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim5_negative_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dim5_positive_tolerance { get; set; }

        [StringLength(16)]
        public string mark { get; set; }

        public string description { get; set; }

        public string works_notes { get; set; }

        public string production_notes { get; set; }

        public string invoice_notes { get; set; }

        public int? required_pieces { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? required_quantity { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? required_weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? original_quantity { get; set; }

        [StringLength(3)]
        public string original_quantity_unit_code { get; set; }

        [StringLength(3)]
        public string weight_units_code { get; set; }

        [StringLength(3)]
        public string sales_group_code { get; set; }

        [Column(TypeName = "date")]
        public DateTime? due_date { get; set; }

        [StringLength(16)]
        public string manual_date { get; set; }

        [StringLength(30)]
        public string specification_value1 { get; set; }

        [StringLength(30)]
        public string specification_value2 { get; set; }

        [StringLength(30)]
        public string specification_value3 { get; set; }

        [StringLength(30)]
        public string specification_value4 { get; set; }

        [StringLength(30)]
        public string specification_value5 { get; set; }

        [StringLength(30)]
        public string specification_value6 { get; set; }

        [StringLength(30)]
        public string specification_value7 { get; set; }

        [StringLength(30)]
        public string specification_value8 { get; set; }

        [StringLength(30)]
        public string specification_value9 { get; set; }

        [StringLength(30)]
        public string specification_value10 { get; set; }

        public int? credited_item { get; set; }

        public int? credited_order_number { get; set; }

        public int? credited_order_item { get; set; }

        public int? credited_despatch_item { get; set; }

        [StringLength(30)]
        public string credited_customer_order { get; set; }

        [StringLength(3)]
        public string despatch_item_branch_code { get; set; }

        public int? despatch_item_header_number { get; set; }

        public int? despatch_item_number { get; set; }

        [StringLength(3)]
        public string order_item_branch_code { get; set; }

        public int? order_item_header_number { get; set; }

        public int? order_item_number { get; set; }

        [StringLength(6)]
        public string delivery_branch_code { get; set; }

        [StringLength(6)]
        public string delivery_warehouse_code { get; set; }

        public bool? show_prices { get; set; }

        public bool? use_minimum_grade { get; set; }

        [StringLength(1)]
        public string margin_type { get; set; }

        [StringLength(1)]
        public string transfer_type { get; set; }

        public int? tax_class_1 { get; set; }

        public int? tax_class_2 { get; set; }

        public int? tax_class_3 { get; set; }

        public int? tax_class_4 { get; set; }

        public int? tax_class_5 { get; set; }

        public bool? tax_exEmpt1 { get; set; }

        public bool? tax_exEmpt2 { get; set; }

        public bool? tax_exEmpt3 { get; set; }

        public bool? tax_exEmpt4 { get; set; }

        public bool? tax_exEmpt5 { get; set; }

        [StringLength(35)]
        public string part_number { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? outside_diameter { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? outside_diameter_minimum { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? outside_diameter_maximum { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pack_height { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pack_height_minimum { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pack_height_maximum { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pack_weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pack_weight_minimum { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? pack_weight_maximum { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? inside_diameter { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? inside_diameter_minimum { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? inside_diameter_maximum { get; set; }

        public int? pack_count_minimum { get; set; }

        public int? pack_count_maximum { get; set; }

        [StringLength(30)]
        public string working_specification_code { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? invoice_costing_weight { get; set; }

        public int? blanket_item_number { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? mixture_price { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? fabrication_price { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? adjustment_price { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? yield_percentage { get; set; }

        public bool? mechanical_cert { get; set; }

        public bool? show_country_of_material_origin { get; set; }

        public bool? show_country_of_primary_processing { get; set; }

        public bool? show_country_of_final_processing { get; set; }

        public string acknowledgement_notes { get; set; }

        public string despatch_notes { get; set; }

        public bool? invoice_packing { get; set; }
    }
}
