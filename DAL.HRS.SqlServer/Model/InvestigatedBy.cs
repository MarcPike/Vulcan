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
    
    public partial class InvestigatedBy
    {
        public int OID { get; set; }
        public string InvestigatorName { get; set; }
        public Nullable<System.DateTime> InvestigatedDate { get; set; }
        public Nullable<int> Incident { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
    
        public virtual Incident Incident1 { get; set; }
    }
}