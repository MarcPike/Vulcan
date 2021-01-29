using DAL.Vulcan.Mongo.Base.Core.DocClass;
using System;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class Milestone: IObjectWithGuidForId
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public List<string> SearchTags { get; set; } = new List<string>();

    }
}
