using System;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Base.Core.DocClass
{
    public interface IObjectWithGuidForId
    {
        Guid Id { get; set; }
        List<string> SearchTags { get; set; }

    }
}
