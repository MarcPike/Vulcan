namespace DAL.IntegrationDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class import_part_specifications
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string import_company_reference { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int import_batch_number { get; set; }

        [StringLength(1)]
        public string import_status { get; set; }

        public string import_notes { get; set; }

        [StringLength(50)]
        public string import_user_name { get; set; }

        [StringLength(50)]
        public string import_source { get; set; }

        public DateTime? import_date { get; set; }

        [StringLength(1)]
        public string import_action { get; set; }

        [StringLength(1)]
        public string import_part_type { get; set; }

        [StringLength(10)]
        public string company_code { get; set; }

        [StringLength(35)]
        public string part_number { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [StringLength(30)]
        public string customer_order_number { get; set; }

        public string description { get; set; }

        [StringLength(24)]
        public string sold_product_code { get; set; }

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

        [Column(TypeName = "numeric")]
        public decimal? entry_quantity { get; set; }

        [StringLength(3)]
        public string entry_quantity_unit_code { get; set; }

        public int? standard_pieces { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? standard_quantity { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? standard_weight { get; set; }

        public bool? override_allowed { get; set; }

        [StringLength(24)]
        public string consumed_product_code { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim1_negative_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim1_positive_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim2_negative_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim2_positive_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim3 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim3_negative_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim3_positive_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim4 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim4_negative_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim4_positive_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim5 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim5_negative_tolerance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? consumed_dim5_positive_tolerance { get; set; }

        public string work_notes { get; set; }

        public string production_notes { get; set; }

        public string invoice_notes { get; set; }

        public string part_notes { get; set; }

        public string internal_notes { get; set; }

        public string goods_inwards_notes { get; set; }

        public string acknowledgement_notes { get; set; }

        public string despatch_notes { get; set; }

        [StringLength(3)]
        public string sales_group_code { get; set; }

        [StringLength(3)]
        public string purchase_group_code { get; set; }

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

        public bool? show_prices { get; set; }

        public bool? use_minimum_grade { get; set; }

        [StringLength(6)]
        public string end_use_code { get; set; }

        [StringLength(30)]
        public string document_number { get; set; }

        [StringLength(30)]
        public string revision_number { get; set; }

        [StringLength(30)]
        public string drawing_number { get; set; }

        [StringLength(40)]
        public string approved_by_name { get; set; }

        public DateTime? approved_by_date { get; set; }

        public DateTime? last_used_date { get; set; }

        [StringLength(1)]
        public string part_status { get; set; }

        public string description_formula { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? outside_diameter { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? outside_diameter_minimum { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? outside_diameter_maximum { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? inside_diameter { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? inside_diameter_minimum { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? inside_diameter_maximum { get; set; }

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

        public int? pack_count_minimum { get; set; }

        public int? pack_count_maximum { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? adjustment_price { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? yield_percentage { get; set; }

        [Column("Price Code")]
        [StringLength(3)]
        public string Price_Code { get; set; }

        public bool? mechanical_cert_required { get; set; }

        public bool? show_country_of_material_origin { get; set; }

        public bool? show_country_of_primary_processing { get; set; }

        public bool? show_country_of_final_processing { get; set; }

        public bool? invoice_packing { get; set; }

        [StringLength(6)]
        public string part_process_type_code { get; set; }
    }
}
