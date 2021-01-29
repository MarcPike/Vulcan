using Vulcan.IMetal.Helpers;

namespace Vulcan.IMetal.Models
{
    public class BaseModel<TBaseHelper>
        where TBaseHelper : BaseHelper, new()
    {
        public static TBaseHelper Helper = new TBaseHelper();

    }
}
