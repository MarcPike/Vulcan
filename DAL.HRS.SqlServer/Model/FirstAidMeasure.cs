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
    
    public partial class FirstAidMeasure
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FirstAidMeasure()
        {
            this.CoshhProducts_FirstAidMeasures = new HashSet<CoshhProducts_FirstAidMeasures>();
        }
    
        public int OID { get; set; }
        public Nullable<int> ExposureType { get; set; }
        public string Action { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CoshhProducts_FirstAidMeasures> CoshhProducts_FirstAidMeasures { get; set; }
        public virtual RouteOfExposureType RouteOfExposureType { get; set; }
    }
}
