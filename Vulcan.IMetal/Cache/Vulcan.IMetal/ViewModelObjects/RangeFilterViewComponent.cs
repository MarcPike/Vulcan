using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vulcan.IMetal.ViewModelObjects
{
    public class RangeFilterViewComponent<TType, TEntity>: BaseFilterViewComponent
    {
        public string Name { get; set; }
        public TType MinValue { get; set; }
        public bool MinUsed { get; set; }
        public TType MaxValue { get; set; }
        public bool MaxUsed { get; set; }
        public TType EqualToValue { get; set; }
        public bool EqualUsed { get; set; }
    }
}
