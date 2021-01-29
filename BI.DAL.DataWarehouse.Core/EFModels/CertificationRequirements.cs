using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class CertificationRequirements
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public bool? ChemicalCert { get; set; }
        public bool? MechanicalCert { get; set; }
        public bool? MillCert { get; set; }
        public bool? ComplianceCert { get; set; }
        public int? DeliveryCopies { get; set; }
        public int? InvoiceCopies { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? CertificateOfConformityRule { get; set; }
        public bool? SeparateCertificatesRequired { get; set; }
    }
}
