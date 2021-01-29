using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Base.DocClass
{
    public interface IObjectWithGuidForId
    {
        Guid Id { get; set; }
        List<string> SearchTags { get; set; }

    }
}
