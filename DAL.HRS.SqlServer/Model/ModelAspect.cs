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
    
    public partial class ModelAspect
    {
        public int OID { get; set; }
        public Nullable<int> ModelDifferences { get; set; }
        public string Aspect { get; set; }
        public string XmlData { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
    
        public virtual ModelDiffsBase ModelDiffsBase { get; set; }
    }
}
