using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class Milestone: IObjectWithGuidForId
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public List<string> SearchTags { get; set; } = new List<string>();

    }
}
