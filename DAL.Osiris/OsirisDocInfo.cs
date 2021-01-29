using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Osiris
{
    public class OsirisDocInfo
    {
        public long ID { get; set; }
        public string COID { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public int FormatID { get; set; }
        public string FormatName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long DKL_DocID { get; set; }
        public string FileName => $"{Name}.{FormatName}";
    }
}
