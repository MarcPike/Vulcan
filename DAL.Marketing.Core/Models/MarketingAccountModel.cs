using DAL.Marketing.Core.Docs;

namespace DAL.Marketing.Core.Models
{
    public class MarketingAccountModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccountType { get; set; }
        public MarketingAccountFolderNode FolderNodes { get; set; }

        public MarketingAccountModel()
        {
        }

        public MarketingAccountModel(string application, string userId, MarketingAccount account)
        {
            Id = account.Id.ToString();
            Name = account.Name;
            FolderNodes = account.AsTreeNode();
            AccountType = account.AccountType.ToString();

            Application = application;
            UserId = userId;
        }
    }
}