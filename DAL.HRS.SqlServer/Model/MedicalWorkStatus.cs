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
    
    public partial class MedicalWorkStatus
    {
        public int OID { get; set; }
        public Nullable<int> Incident { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public byte[] Date { get; set; }
        public byte[] Comment { get; set; }
    
        public virtual Incident Incident1 { get; set; }
        public virtual MedicalWorkStatusType MedicalWorkStatusType { get; set; }
    }
}
