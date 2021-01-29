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
    
    public partial class BBS_Observation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BBS_Observation()
        {
            this.BBS_ObservationItem = new HashSet<BBS_ObservationItem>();
        }
    
        public int OID { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<int> Observer { get; set; }
        public Nullable<int> ShiftType { get; set; }
        public Nullable<int> Department { get; set; }
        public Nullable<int> EmployeeType { get; set; }
        public Nullable<int> NumberOfPeopleObserved { get; set; }
        public Nullable<int> TaskType { get; set; }
        public string ObserverComments { get; set; }
        public string WorkerComments { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<int> Location { get; set; }
        public Nullable<int> ObservationType { get; set; }
        public Nullable<int> BBS_Department { get; set; }
        public Nullable<int> BBS_DepartmentSubCategory { get; set; }
        public Nullable<int> TaskTypeSubCategory { get; set; }
    
        public virtual BBS_Department BBS_Department1 { get; set; }
        public virtual BBS_DepartmentSubCategory BBS_DepartmentSubCategory1 { get; set; }
        public virtual BBS_EmployeeType BBS_EmployeeType { get; set; }
        public virtual Department Department1 { get; set; }
        public virtual Location Location1 { get; set; }
        public virtual BBS_ObservationType BBS_ObservationType { get; set; }
        public virtual BBS_Observer BBS_Observer { get; set; }
        public virtual PositionShiftType PositionShiftType { get; set; }
        public virtual BBS_TaskType BBS_TaskType { get; set; }
        public virtual BBS_TaskTypeSubCategory BBS_TaskTypeSubCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BBS_ObservationItem> BBS_ObservationItem { get; set; }
    }
}
