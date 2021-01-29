using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class TemplateProcessPlans
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? Process1Id { get; set; }
        public int? Process2Id { get; set; }
        public int? Process3Id { get; set; }
        public int? Process4Id { get; set; }
        public int? Process5Id { get; set; }
        public int? Process6Id { get; set; }
        public int? Process7Id { get; set; }
        public int? Process8Id { get; set; }
        public int? Process9Id { get; set; }
        public int? Process10Id { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
