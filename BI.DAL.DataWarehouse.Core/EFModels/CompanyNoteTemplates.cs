﻿using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class CompanyNoteTemplates
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? CompanyId { get; set; }
        public int? CompanySubAddressId { get; set; }
        public int? NoteTypeId { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
