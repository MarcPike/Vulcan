using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Models
{
    public class OsirisDocumentListForPortal
    {
        public string Coid { get; set; } = string.Empty;
        public string TagNumber { get; set; } = string.Empty;
        public string HeatNumber { get; set; } = string.Empty;

        public OsirisDocumentListForPortal()
        {
            
        }
    }
}
