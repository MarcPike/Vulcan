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
    
    public partial class DisciplineSupportingDocument
    {
        public System.Guid Oid { get; set; }
        public Nullable<System.Guid> File { get; set; }
        public Nullable<int> Discipline { get; set; }
        public Nullable<int> DisciplineHistory { get; set; }
        public Nullable<int> DocumentType { get; set; }
        public string Comments { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<System.DateTime> DocumentDate { get; set; }
    
        public virtual Discipline Discipline1 { get; set; }
        public virtual DisciplineHistory DisciplineHistory1 { get; set; }
        public virtual DisciplineSupportingDocumentType DisciplineSupportingDocumentType { get; set; }
        public virtual MyFileData MyFileData { get; set; }
    }
}
