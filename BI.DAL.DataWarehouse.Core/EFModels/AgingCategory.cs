using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class AgingCategory
    {
        public int AgingCategoryId { get; set; }
        public int SortOrder { get; set; }
        public string Aging { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
    }
}
