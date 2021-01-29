using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VDespatchInstructions
    {
        public string ViewId { get; set; }
        public string Coid { get; set; }
        public string CustomerNumber { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int? TypeId { get; set; }
        public string DespatchInstructions { get; set; }
    }
}
