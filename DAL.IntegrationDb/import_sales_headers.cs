namespace DAL.IntegrationDb
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class import_sales_headers
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

        [StringLength(1)]
        public string import_status { get; set; }

        public string import_notes { get; set; }

        [StringLength(50)]
        public string import_user_name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? import_date { get; set; }

        [StringLength(1)]
        public string import_action { get; set; }

        [StringLength(6)]
        public string branch_code { get; set; }

        [StringLength(3)]
        public string type_code { get; set; }

        [StringLength(30)]
        public string customer_order_number { get; set; }

        [StringLength(30)]
        public string job_number { get; set; }

        [StringLength(30)]
        public string release_number { get; set; }

        [StringLength(10)]
        public string customer_code { get; set; }

        [StringLength(50)]
        public string customer_name_override { get; set; }

        public string customer_address { get; set; }

        [StringLength(60)]
        public string customer_town { get; set; }

        [StringLength(60)]
        public string customer_county { get; set; }

        [StringLength(60)]
        public string customer_postcode { get; set; }

        [StringLength(3)]
        public string customer_country_code { get; set; }

        [StringLength(16)]
        public string customer_transport_area_code { get; set; }

        [StringLength(10)]
        public string deliver_to_address_code { get; set; }

        public string deliver_to_address { get; set; }

        [StringLength(60)]
        public string deliver_to_town { get; set; }

        [StringLength(60)]
        public string deliver_to_county { get; set; }

        [StringLength(60)]
        public string deliver_to_postcode { get; set; }

        [StringLength(3)]
        public string deliver_to_country_code { get; set; }

        [StringLength(16)]
        public string deliver_to_transport_area_code { get; set; }

        [StringLength(50)]
        public string salesperson_name { get; set; }

        [StringLength(50)]
        public string inside_salesperson_name { get; set; }

        [StringLength(6)]
        public string delivery_branch_code { get; set; }

        [StringLength(6)]
        public string delivery_warehouse_code { get; set; }

        [StringLength(50)]
        public string delivery_name { get; set; }

        public string delivery_address { get; set; }

        [StringLength(60)]
        public string delivery_town { get; set; }

        [StringLength(60)]
        public string delivery_county { get; set; }

        [StringLength(60)]
        public string delivery_postcode { get; set; }

        [StringLength(3)]
        public string delivery_country_code { get; set; }

        [StringLength(16)]
        public string delivery_transport_area_code { get; set; }

        [Column(TypeName = "date")]
        public DateTime? sale_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? due_date { get; set; }

        [StringLength(16)]
        public string manual_date { get; set; }

        [StringLength(6)]
        public string transport_type_code { get; set; }

        [StringLength(60)]
        public string delivery_point { get; set; }

        [StringLength(10)]
        public string carrier_code { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? transport_cost_rate { get; set; }

        [StringLength(3)]
        public string transport_cost_rate_unit_code { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? transport_cost_amount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? transport_exchange_rate { get; set; }

        [StringLength(1)]
        public string transport_exchange_rate_type_code { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? exchange_rate { get; set; }

        [StringLength(1)]
        public string exchange_rate_type_code { get; set; }

        [StringLength(6)]
        public string terms_code { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? settlement_discount { get; set; }

        public bool? chemical_cert { get; set; }

        public bool? mechanical_cert { get; set; }

        public bool? mill_cert { get; set; }

        public bool? compliance_cert { get; set; }

        public int? delivery_copies { get; set; }

        public int? invoice_copies { get; set; }

        public string header_text { get; set; }

        public string internal_text { get; set; }

        public string despatch_text { get; set; }

        [Column(TypeName = "date")]
        public DateTime? payment_due_date { get; set; }

        [StringLength(6)]
        public string credited_invoice_branch_code { get; set; }

        public int? credited_invoice { get; set; }

        [StringLength(30)]
        public string credit_reference { get; set; }

        [StringLength(6)]
        public string order_branch_code { get; set; }

        public int? order_number { get; set; }

        [StringLength(50)]
        public string deliver_to_name_override { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? transport_charge_rate { get; set; }

        [StringLength(3)]
        public string transport_charge_rate_unit_code { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? transport_charge_amount { get; set; }

        [StringLength(3)]
        public string despatch_branch_code { get; set; }

        public int? despatch_number { get; set; }

        [Column(TypeName = "date")]
        public DateTime? followup_date { get; set; }

        public bool? update_hold { get; set; }

        public bool? use_minimum_grade { get; set; }

        [StringLength(6)]
        public string credited_despatch_branch_code { get; set; }

        public int? credited_despatch { get; set; }

        [StringLength(6)]
        public string credited_order_branch_code { get; set; }

        public int? credited_order { get; set; }

        public string credit_release_notes { get; set; }

        public bool? require_proforma { get; set; }

        [StringLength(1)]
        public string transfer_type { get; set; }

        [StringLength(12)]
        public string tax_group_code { get; set; }

        public int? tax_class_1 { get; set; }

        public int? tax_class_2 { get; set; }

        public int? tax_class_3 { get; set; }

        public int? tax_class_4 { get; set; }

        public int? tax_class_5 { get; set; }

        [StringLength(6)]
        public string transfer_to_branch_code { get; set; }

        [StringLength(6)]
        public string transfer_to_warehouse_code { get; set; }

        [StringLength(12)]
        public string tax_authority1 { get; set; }

        [StringLength(12)]
        public string tax_authority2 { get; set; }

        [StringLength(12)]
        public string tax_authority3 { get; set; }

        [StringLength(12)]
        public string tax_authority4 { get; set; }

        [StringLength(12)]
        public string tax_authority5 { get; set; }

        public bool? tax_exEmpt1 { get; set; }

        public bool? tax_exEmpt2 { get; set; }

        public bool? tax_exEmpt3 { get; set; }

        public bool? tax_exEmpt4 { get; set; }

        public bool? tax_exEmpt5 { get; set; }

        public int? document_delivery_type_id { get; set; }

        [StringLength(50)]
        public string contact_forename { get; set; }

        [StringLength(50)]
        public string contact_surname { get; set; }

        [StringLength(20)]
        public string contact_telephone_override { get; set; }

        [StringLength(20)]
        public string contact_mobile_override { get; set; }

        [StringLength(20)]
        public string contact_fax_override { get; set; }

        [StringLength(255)]
        public string contact_email_override { get; set; }

        [StringLength(255)]
        public string contact_web_address_override { get; set; }

        public bool contact_creation_allowed { get; set; }

        public bool? consignment_order { get; set; }

        public string footer_internal_text { get; set; }

        public string footer_external_text { get; set; }

        [StringLength(3)]
        public string blanket_header_branch_code { get; set; }

        public int? blanket_header_number { get; set; }

        public bool? transport_charged { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fix_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? expiry_date { get; set; }

        public bool? no_fixed_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? credited_delivery_date { get; set; }

        [StringLength(3)]
        public string sales_group_code { get; set; }

        public int? certificate_of_conformity_rule { get; set; }

        public bool? separate_certificates_required { get; set; }
    }
}
