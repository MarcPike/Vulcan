using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Marketing.Docs
{
    public class MarketingAccountFolder
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public List<MarketingAccountFolder> Children { get; set; } = new List<MarketingAccountFolder>();
        public List<CompanyRef> Companies { get; set; } = new List<CompanyRef>();
        public List<TeamRef> SalesTeams { get; set; } = new List<TeamRef>();
        public List<MarketingSalesTeamRef> MarketingSalesTeams { get; set; } = new List<MarketingSalesTeamRef>();
        public Guid ParentObjectId { get; set; } = Guid.Empty;
        public string ExpandedIcon { get; set; } = "fa-folder-open";
        public string CollapsedIcon { get; set; } = "fa-folder";

        public ObjectId MarketingAccountId { get; set; } = ObjectId.Empty;

        public Marketing.Docs.MarketingAccount GetMarketingAccount()
        {
            if (MarketingAccountId == ObjectId.Empty) return null;

            return new RepositoryBase<Marketing.Docs.MarketingAccount>().Find(MarketingAccountId);
        }

        public Marketing.Docs.MarketingAccountType? GetMarketingAccountType()
        {
            var marketingAccount = GetMarketingAccount();

            return marketingAccount?.AccountType;
        }
        
        public MarketingAccountFolder FindFolder(Guid id)
        {
            if (Id == id) return this;

            var result = Children.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                foreach (var child in Children)
                {
                    result = child.FindFolder(id);
                    if (result != null) return result;
                }
            }

            return result;
        }

        [BsonIgnore]
        public MarketingAccountFolderNode AsFolderNode
        {
            get
            {
                var account = GetMarketingAccount();
                var node = new MarketingAccountFolderNode()
                {
                    Id = Id.ToString(),
                    Label = Name,
                    Companies = Companies.ToList(),
                    Data = Id.ToString(),
                    ParentObjectId = ParentObjectId.ToString(),
                    
                };
                if (Children.Any())
                {
                    foreach (var child in Children.OrderBy(x => x.Name).ToList())
                    {
                        var asNode = child.AsFolderNode;
                        if (asNode != null)
                            node.Children.Add(asNode);
                    }
                }
                return node;
            }
        }

        public List<CompanyRef> GetAllCompanies()
        {
            var companies = Companies;

            foreach (var child in Children)
            {
                companies.AddRange(child.GetAllCompanies());
            }

            return companies.Distinct(new DistinctCompanyRefComparer()).ToList();
        }


        //public List<CompanyRef> GetAllCompanies(List<CompanyRef> existingCompanies)
        //{
        //    foreach (var companyRef in Companies)
        //    {
        //        if (existingCompanies.SingleOrDefault(x => x.Id == companyRef.Id) == null)
        //        {
        //            existingCompanies.Add(companyRef);
        //        }
        //    }

        //    return existingCompanies;

        //}

    }

}
