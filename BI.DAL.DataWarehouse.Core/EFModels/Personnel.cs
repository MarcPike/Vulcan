using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Personnel
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string FastDial { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? Enabled { get; set; }
        public string SecurityRoles { get; set; }
        public int? BranchId { get; set; }
        public int? PersonnelGroupId { get; set; }
        public int? DefaultPrinterId { get; set; }
        public string TableEditingPermissionLevel { get; set; }
        public int? DefaultCustomerId { get; set; }
        public string DiaryGroupsCode { get; set; }
        public decimal? ReconciliationVarianceValue { get; set; }
        public decimal? ReconciliationVariancePercentage { get; set; }
        public decimal? MaxSupplierDocumentValue { get; set; }
        public string LedgerSegmentCode { get; set; }
        public string GoogleCalendarUser { get; set; }
        public string GoogleCalendarPassword { get; set; }
        public bool? MemberOfPickupCallsGroup { get; set; }
        public bool? ViewAllEnquiryItemsAllowed { get; set; }
        public int? DefaultLabelPrinterId { get; set; }
        public string LoginName { get; set; }
        public int? DefaultSalesGroupId { get; set; }
        public bool? SupportUser { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public bool? GuiConfigurator { get; set; }
        public string LocaleLanguage { get; set; }
        public string LocaleCountry { get; set; }
        public string LocaleVariant { get; set; }
        public string StrongPassword { get; set; }
        public int? MssSettingsId { get; set; }
        public bool? MssUserFlag { get; set; }
    }
}
