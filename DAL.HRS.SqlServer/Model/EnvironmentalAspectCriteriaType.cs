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
    
    public partial class EnvironmentalAspectCriteriaType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EnvironmentalAspectCriteriaType()
        {
            this.EnvironmentalAspectCriteriaScoreSupportingDocument = new HashSet<EnvironmentalAspectCriteriaScoreSupportingDocument>();
            this.EnvironmentalAspectCriteriaScoreSupportingDocumentLink = new HashSet<EnvironmentalAspectCriteriaScoreSupportingDocumentLink>();
            this.EnvironmentalAspectScore = new HashSet<EnvironmentalAspectScore>();
            this.EnvironmentalAspectScoreHistory = new HashSet<EnvironmentalAspectScoreHistory>();
        }
    
        public int OID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnvironmentalAspectCriteriaScoreSupportingDocument> EnvironmentalAspectCriteriaScoreSupportingDocument { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnvironmentalAspectCriteriaScoreSupportingDocumentLink> EnvironmentalAspectCriteriaScoreSupportingDocumentLink { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnvironmentalAspectScore> EnvironmentalAspectScore { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnvironmentalAspectScoreHistory> EnvironmentalAspectScoreHistory { get; set; }
    }
}
