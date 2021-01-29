using System;
using System.Collections.Generic;
using System.Text;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using MongoDB.Bson;

namespace DAL.Marketing.Core.Docs
{
    public class MarketingAccount : BaseDocument
    {

        public MarketingAccountType AccountType { get; set; } = MarketingAccountType.Strategic;
        public string Name { get; set; }
        public MarketingAccountFolder MarketingAccountFolder { get; set; } = new MarketingAccountFolder();
        
        public MarketingAccountFolderNode AsTreeNode()
        {
            return new MarketingAccountFolderNode(MarketingAccountFolder, this);
        }

        public List<CompanyRef> GetAllCompanies()
        {
            return MarketingAccountFolder.GetAllCompanies();
        }

        public string GetFolderPath(string folderId)
        {
            var result = new StringBuilder();
            var folderGuid = Guid.Parse(folderId);
            var folder = MarketingAccountFolder.FindFolder(folderGuid);
            if (folder == null) return result.ToString();

            result.Append(folder.Name);
            while (folder.ParentObjectId != Guid.Empty)
            {
                folder = folder.FindFolder(folder.ParentObjectId);
                if (folder == null) break;
                result.Insert(0, folder.Name + " / ");
            }

            return result.ToString();
        }

        public MarketingAccountFolder AddChildNode(Guid folderId, string name, string marketingAccountId)
        {
            var parent = MarketingAccountFolder.FindFolder(folderId);
            if (parent == null) throw new Exception("Could not find folder");

            var newFolder = new MarketingAccountFolder()
            {
                ParentObjectId = folderId,
                Name = name,
                MarketingAccountId = ObjectId.Parse(marketingAccountId)
            };

            parent.Children.Add(newFolder);
            new RepositoryBase<MarketingAccount>().Upsert(this);
            return newFolder;
        }

        public void RemoveNode(Guid folderId)
        {
            var removeMe = MarketingAccountFolder.FindFolder(folderId);
            if (removeMe == null) throw new Exception("Could not find folder");

            // must be root node
            if (removeMe.ParentObjectId == Guid.Empty)
            {
                MarketingAccountFolder.Children.Remove(removeMe);
            }
            // sibling node
            else
            {
                var parent = MarketingAccountFolder.FindFolder(removeMe.ParentObjectId);
                parent.Children.Remove(removeMe);
            }

            new RepositoryBase<MarketingAccount>().Upsert(this);
        }

        public void MoveNode(Guid folderId, Guid originalParent, Guid newParent)
        {
            var moveMe = MarketingAccountFolder.FindFolder(folderId);
            if (moveMe == null) throw new Exception("Could not find folder");



            MarketingAccountFolder removeFromParent = null;

            if (originalParent != Guid.Empty)
                removeFromParent = MarketingAccountFolder.FindFolder(originalParent);

            MarketingAccountFolder addToParent = null;

            if (newParent != Guid.Empty)
                addToParent = MarketingAccountFolder.FindFolder(newParent);

            if (addToParent != null)
            {
                addToParent.Children.Add(moveMe);
            }
            else
            {
                MarketingAccountFolder.Children.Add(moveMe);
            }

            if (removeFromParent != null)
            {
                removeFromParent.Children.Remove(moveMe);
            }

            new RepositoryBase<MarketingAccount>().Upsert(this);
        }


        public List<CompanyRef> GetAllCompaniesForFolder(Guid folderId)
        {
            var result = new List<CompanyRef>();
            var folder = MarketingAccountFolder.FindFolder(folderId);
            if (folder != null)
            {
                result.AddRange(folder.GetAllCompanies());
            }

            return result;
        }

    }
}