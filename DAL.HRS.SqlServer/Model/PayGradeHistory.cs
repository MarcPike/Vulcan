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
    
    public partial class PayGradeHistory
    {
        public int OID { get; set; }
        public Nullable<int> Compensation { get; set; }
        public Nullable<int> PayGradeType { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public byte[] CreatedOn { get; set; }
        public byte[] Minimum { get; set; }
        public byte[] Maximum { get; set; }
    
        public virtual Compensation Compensation1 { get; set; }
        public virtual PayGradeType PayGradeType1 { get; set; }
    }
}
