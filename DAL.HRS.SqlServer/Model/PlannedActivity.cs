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
    
    public partial class PlannedActivity
    {
        public int OID { get; set; }
        public Nullable<int> Title { get; set; }
        public Nullable<int> ActivityType { get; set; }
        public Nullable<int> TrainingCourse { get; set; }
        public string Description { get; set; }
        public Nullable<int> DaysToComplete { get; set; }
        public string Comments { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
    
        public virtual RequiredActivityType RequiredActivityType { get; set; }
        public virtual TitleType TitleType { get; set; }
        public virtual TrainingCourse TrainingCourse1 { get; set; }
    }
}
