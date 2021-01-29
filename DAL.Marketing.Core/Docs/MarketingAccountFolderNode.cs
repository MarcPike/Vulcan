using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Marketing.Core.Docs
{
   public  class MarketingAccountFolderNode
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Data { get; set; }
        public string ParentObjectId { get; set; }
        public List<CompanyRef> Companies { get; set; } = new List<CompanyRef>();
        public List<TeamRef> SalesTeams { get; set; } = new List<TeamRef>();
        public List<MarketingSalesTeamRef> MarketingSalesTeams { get; set; } = new List<MarketingSalesTeamRef>();
        public string ExpandedIcon { get; set; } = "fa-folder-open";
        public string CollapsedIcon { get; set; } = "fa-folder";
        public List<MarketingAccountFolderNode> Children { get; set; } = new List<MarketingAccountFolderNode>();

        public MarketingAccountFolderNode()
        {
        }

        public MarketingAccountFolderNode(MarketingAccountFolder folder, MarketingAccount account)
        {
            Id = folder.Id.ToString();
            Label = folder.Name;
            Data = folder.Id.ToString();
            ParentObjectId = folder.ParentObjectId.ToString();
            Companies = folder.Companies;
            MarketingSalesTeams = folder.MarketingSalesTeams;
            SalesTeams = folder.SalesTeams;
            foreach (var strategicAccountFolder in folder.Children)
            {
                Children.Add(new MarketingAccountFolderNode(strategicAccountFolder, account));
            }
        }

        public MarketingAccountFolder AsStrategicAccountFolder()
        {
            var result = new MarketingAccountFolder()
            {
                Id = Guid.Parse(Id),
                Name = Label,
                Companies = Companies,
                ParentObjectId = Guid.Parse(ParentObjectId),
                MarketingSalesTeams = MarketingSalesTeams,
                SalesTeams = SalesTeams,
                Children = new List<MarketingAccountFolder>()
            };
            foreach (var strategicAccountFolderNode in Children)
            {
                result.Children.Add(strategicAccountFolderNode.AsStrategicAccountFolder());
            }

            return result;
        }

    }
}
