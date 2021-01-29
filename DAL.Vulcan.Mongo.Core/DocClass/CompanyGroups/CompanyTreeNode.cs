using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;

namespace DAL.Vulcan.Mongo.Core.DocClass.CompanyGroups
{
    public class CompanyTreeNode
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Data { get; set; }
        public List<CompanyRef> Companies { get; set; }
        public string ExpandedIcon { get; set; } = "fa fa-folder-open";
        public string CollapsedIcon { get; set; } = "fa fa-folder";
        public List<CompanyTreeNode> Children { get; set; } = new List<CompanyTreeNode>();
        public bool IsAnalytical { get; set; }
        public bool IsGlobal { get; set; }
    }
}
