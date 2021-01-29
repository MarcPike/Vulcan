using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.ViewModelObjects
{
    public class StringFilterViewComponent<TEntity> : BaseFilterViewComponent
    {
        public string Value { get; set; }
        public List<string> Values { get; set; }
        public StringFilterType StringFilterType { get; set; }
        public string StringFilterName { get; set; }
        public bool HasSuggestions { get; set; }
        public string Name { get; set; }
    }
}
