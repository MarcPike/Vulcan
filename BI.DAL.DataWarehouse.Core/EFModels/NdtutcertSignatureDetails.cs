using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class NdtutcertSignatureDetails
    {
        public int Id { get; set; }
        public string Directory { get; set; }
        public string Inspector { get; set; }
        public string InspectorT1 { get; set; }
        public string InspectorT2 { get; set; }
        public string DigitallySignedBy { get; set; }
        public byte[] SignatureImage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
    }
}
