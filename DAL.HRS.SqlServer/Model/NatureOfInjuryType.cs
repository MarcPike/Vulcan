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
    
    public partial class NatureOfInjuryType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NatureOfInjuryType()
        {
            this.NatureOfInjury = new HashSet<NatureOfInjury>();
        }
    
        public int OID { get; set; }
        public string NatureOfInjuryName { get; set; }
        public Nullable<bool> IsCommentRequired { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NatureOfInjury> NatureOfInjury { get; set; }
    }
}
