using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vulcan.IMetal.Models
{
    public class DimensionsModel
    {
        public decimal OuterDiameter { get; set; }
        public decimal InsideDiameter { get; set; }
        public decimal Width { get; set; }
        public decimal Thick { get; set; }
        public decimal Length { get; set; }
        public decimal Density { get; set; }
    }
}
