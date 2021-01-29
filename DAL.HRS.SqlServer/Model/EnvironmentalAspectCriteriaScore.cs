//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.HRS.SqlServer.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class EnvironmentalAspectCriteriaScore
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EnvironmentalAspectCriteriaScore()
        {
            this.EnvironmentalAspectCriteriaScore_Location = new HashSet<EnvironmentalAspectCriteriaScore_Location>();
            this.EnvironmentalAspectCriteriaScoreSupportingDocument = new HashSet<EnvironmentalAspectCriteriaScoreSupportingDocument>();
            this.EnvironmentalAspectCriteriaScoreSupportingDocumentLink = new HashSet<EnvironmentalAspectCriteriaScoreSupportingDocumentLink>();
            this.EnvironmentalAspectReview = new HashSet<EnvironmentalAspectReview>();
            this.EnvironmentalAspectScore = new HashSet<EnvironmentalAspectScore>();
            this.EnvironmentalAspectScoreHistory = new HashSet<EnvironmentalAspectScoreHistory>();
        }
    
        public int OID { get; set; }
        public Nullable<int> EnvironmentalAspectCode { get; set; }
        public string EnvironmentalAspectTypeDescription { get; set; }
        public string EnvironmentalAspectTypeActivity { get; set; }
        public string EnvironmentalAspectTypeImpacts { get; set; }
        public string EnvironmentalAspectTypeOther { get; set; }
        public Nullable<bool> AssignedWorkAreaMachineShop { get; set; }
        public Nullable<bool> AssignedWorkAreaHeatTreatment { get; set; }
        public Nullable<bool> AssignedWorkAreaSawShop { get; set; }
        public Nullable<bool> AssignedWorkAreaOffice { get; set; }
        public Nullable<bool> AssignedWorkAreaExteriorArea { get; set; }
        public Nullable<bool> AssignedWorkAreaTestHouse { get; set; }
        public string Comments { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<bool> AssignedWorkAreaFabrication { get; set; }
        public Nullable<bool> AssignedWorkAreaPaintShop { get; set; }
        public Nullable<bool> AssignedWorkAreaShotBlasting { get; set; }
    
        public virtual EnvironmentalAspectAspectCode EnvironmentalAspectAspectCode { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnvironmentalAspectCriteriaScore_Location> EnvironmentalAspectCriteriaScore_Location { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnvironmentalAspectCriteriaScoreSupportingDocument> EnvironmentalAspectCriteriaScoreSupportingDocument { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnvironmentalAspectCriteriaScoreSupportingDocumentLink> EnvironmentalAspectCriteriaScoreSupportingDocumentLink { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnvironmentalAspectReview> EnvironmentalAspectReview { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnvironmentalAspectScore> EnvironmentalAspectScore { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnvironmentalAspectScoreHistory> EnvironmentalAspectScoreHistory { get; set; }
    }
}