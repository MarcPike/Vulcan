//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.QNG.Reader.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Companies
    {
        public string COID { get; set; }
        public int id { get; set; }
        public Nullable<int> version { get; set; }
        public Nullable<System.DateTime> cdate { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
        public Nullable<int> cuser_id { get; set; }
        public Nullable<int> muser_id { get; set; }
        public string status { get; set; }
        public string code { get; set; }
        public Nullable<int> branch_id { get; set; }
        public Nullable<int> personnel_id { get; set; }
        public Nullable<int> relationship_id { get; set; }
        public Nullable<int> status_id { get; set; }
        public string name { get; set; }
        public Nullable<int> territory_id { get; set; }
        public string telephone { get; set; }
        public string fast_dial { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string web_address { get; set; }
        public Nullable<int> contact_frequency { get; set; }
        public Nullable<bool> accept_mail { get; set; }
        public Nullable<bool> accept_email { get; set; }
        public Nullable<bool> accept_fax { get; set; }
        public Nullable<bool> accept_calls { get; set; }
        public Nullable<bool> accept_visits { get; set; }
        public string general_note { get; set; }
        public string products_note { get; set; }
        public string competition_note { get; set; }
        public string categories { get; set; }
        public string access_method { get; set; }
        public string password { get; set; }
        public Nullable<int> parent_company_id { get; set; }
        public Nullable<System.DateTime> last_contact_date { get; set; }
        public string idxfti { get; set; }
        public Nullable<int> terms_id { get; set; }
        public string accounts_note { get; set; }
        public Nullable<int> address_id { get; set; }
        public string short_name { get; set; }
        public string rank { get; set; }
        public Nullable<int> currency_id { get; set; }
        public Nullable<int> vat_type1_id { get; set; }
        public Nullable<int> vat_type2_id { get; set; }
        public Nullable<int> vat_type3_id { get; set; }
        public Nullable<int> vat_type4_id { get; set; }
        public Nullable<int> type_id { get; set; }
        public Nullable<int> sales_group_id { get; set; }
        public Nullable<int> company_totals_id { get; set; }
        public Nullable<int> company_credit_rules_id { get; set; }
        public Nullable<bool> show_prices { get; set; }
        public Nullable<bool> require_sales_acknowledgement { get; set; }
        public Nullable<int> certifications_id { get; set; }
        public Nullable<int> invoice_company_id { get; set; }
        public string partner_code { get; set; }
        public string credit_comments { get; set; }
        public string registration_number { get; set; }
        public Nullable<bool> require_proforma { get; set; }
        public Nullable<int> crm_account_id { get; set; }
        public Nullable<bool> statement_required { get; set; }
        public Nullable<System.DateTime> synchronisation_date { get; set; }
        public Nullable<System.DateTime> date_appointed { get; set; }
        public Nullable<int> document_delivery_type_id { get; set; }
        public Nullable<int> default_contact_id { get; set; }
        public Nullable<int> default_buyer_id { get; set; }
        public string ledger_segment_code { get; set; }
        public Nullable<int> supplier_rolling_days { get; set; }
        public Nullable<int> despatch_note_item_rule_id { get; set; }
        public string tax_group { get; set; }
        public string tax_authority1 { get; set; }
        public string tax_authority2 { get; set; }
        public string tax_authority3 { get; set; }
        public string tax_authority4 { get; set; }
        public string tax_authority5 { get; set; }
        public Nullable<int> tax_class1 { get; set; }
        public Nullable<int> tax_class2 { get; set; }
        public Nullable<int> tax_class3 { get; set; }
        public Nullable<int> tax_class4 { get; set; }
        public Nullable<int> tax_class5 { get; set; }
        public string tax_registration1 { get; set; }
        public string tax_registration2 { get; set; }
        public string tax_registration3 { get; set; }
        public string tax_registration4 { get; set; }
        public string tax_registration5 { get; set; }
        public Nullable<bool> tax_exempt1 { get; set; }
        public Nullable<bool> tax_exempt2 { get; set; }
        public Nullable<bool> tax_exempt3 { get; set; }
        public Nullable<bool> tax_exempt4 { get; set; }
        public Nullable<bool> tax_exempt5 { get; set; }
        public string popup_notes { get; set; }
        public Nullable<int> analysis_code_1_id { get; set; }
        public Nullable<int> analysis_code_2_id { get; set; }
        public Nullable<int> analysis_code_3_id { get; set; }
        public Nullable<int> analysis_code_4_id { get; set; }
        public Nullable<decimal> packing_weight_percentage { get; set; }
        public Nullable<int> default_nominal_code { get; set; }
        public Nullable<bool> invoice_packing { get; set; }
        public string stock_item_prefix { get; set; }
        public Nullable<int> default_order_classification_id { get; set; }
        public Nullable<int> invoice_base_currency_flag { get; set; }
        public Nullable<bool> intercompany_account { get; set; }
        public Nullable<bool> payment_hold { get; set; }
        public Nullable<System.DateTime> ETLCreateDate { get; set; }
        public Nullable<System.DateTime> ETLUpdateDate { get; set; }
        public string test_certificate_send_method { get; set; }
        public string test_certificate_destination { get; set; }
        public string test_certificate_hold { get; set; }
        public string invoice_weight_rounding_mode { get; set; }
        public Nullable<int> default_item_class1 { get; set; }
        public Nullable<int> default_item_class2 { get; set; }
        public Nullable<int> default_item_class3 { get; set; }
        public Nullable<int> default_item_class4 { get; set; }
        public Nullable<int> default_item_class5 { get; set; }
        public Nullable<int> analysis_code_5_id { get; set; }
        public Nullable<int> analysis_code_6_id { get; set; }
        public Nullable<int> outwork_branch_id { get; set; }
        public Nullable<int> default_transport_type_id { get; set; }
        public Nullable<bool> counter_sales_default { get; set; }
    }
}