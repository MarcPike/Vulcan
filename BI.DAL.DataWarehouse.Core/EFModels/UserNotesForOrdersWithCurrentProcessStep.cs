using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class UserNotesForOrdersWithCurrentProcessStep
    {
        public int Id { get; set; }
        public string Coid { get; set; }
        public int OrderItemId { get; set; }
        public string UserName { get; set; }
        public string UserNotes1 { get; set; }
        public string UserNotes2 { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
    }
}
