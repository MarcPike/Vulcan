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
    
    public partial class CompensationHistory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompensationHistory()
        {
            this.CompensationHistorySupportingDocument = new HashSet<CompensationHistorySupportingDocument>();
            this.CompensationSupportingDocument = new HashSet<CompensationSupportingDocument>();
        }
    
        public int OID { get; set; }
        public Nullable<int> Compensation { get; set; }
        public Nullable<int> IncreaseType { get; set; }
        public Nullable<int> PayFrequencyType { get; set; }
        public Nullable<int> PayGradeType { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public byte[] CreatedOn { get; set; }
        public byte[] EffectiveDate { get; set; }
        public byte[] PayRateAmount { get; set; }
        public byte[] AnnualSalary { get; set; }
        public byte[] PercentOfIncrease { get; set; }
        public byte[] ActualIncreaseAmount { get; set; }
    
        public virtual Compensation Compensation1 { get; set; }
        public virtual IncreaseType IncreaseType1 { get; set; }
        public virtual PayGradeType PayGradeType1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompensationHistorySupportingDocument> CompensationHistorySupportingDocument { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompensationSupportingDocument> CompensationSupportingDocument { get; set; }
    }
}
