using System.Linq;
using System.Threading.Tasks;

namespace Vulcan.WebApi2.Models
{
    public class RangeMinMax<TType>
    {
        public TType Min { get; set; } 
        public TType Max { get; set; } 
    }
}
