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
    
    public partial class BenefitPlanTypeBenefitPlanTypes_BenefitCoverageTypeBenefitCoverageTypes
    {
        public Nullable<int> BenefitCoverageTypes { get; set; }
        public Nullable<int> BenefitPlanTypes { get; set; }
        public int OID { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
    
        public virtual BenefitCoverageType BenefitCoverageType { get; set; }
        public virtual BenefitPlanType BenefitPlanType { get; set; }
    }
}
