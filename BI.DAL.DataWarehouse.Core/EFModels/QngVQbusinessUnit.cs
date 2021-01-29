using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class QngVQbusinessUnit
    {
        public int Id { get; set; }
        public string BusinessUnit { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Directory { get; set; }
    }
}
