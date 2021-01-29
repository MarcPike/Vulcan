using System.Collections.Generic;
using DAL.Marketing.Docs;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Marketing.Models
{
    public class MarketingAccountFolderModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string MarketingAccountId { get; set; }
        public string MarketingAccountName { get; set; }
        public string Id { get; set; } 
        public string Name { get; set; }
        public string ParentObjectId { get; set; }
        public List<CompanyRef> Companies { get; set; }
        public List<TeamRef> SalesTeams { get; set; }
        public List<MarketingSalesTeamRef> MarketingSalesTeams { get; set; }

        public string FolderPath { get; set; }

        public MarketingAccountFolderModel()
        {

        }

        public MarketingAccountFolderModel(string application, string userId, MarketingAccount account, MarketingAccountFolder folder)
        {
            Id = folder.Id.ToString();
            Name = folder.Name;
            Companies = folder.Companies;
            ParentObjectId = folder.ParentObjectId.ToString();

            MarketingAccountId = account.Id.ToString();
            MarketingAccountName = account.Name;
            SalesTeams = folder.SalesTeams;
            MarketingSalesTeams = folder.MarketingSalesTeams;
            FolderPath = account.GetFolderPath(folder.Id.ToString());
            Application = application;
            UserId = userId;
        }

    }
}