using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.ValueBucket;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class CompanyContactTree
    {
        public CompanyRef Company { get; set; }
        public List<ContactRef> CompanyContacts { get; set; }

        public List<CompanyTreeData> TreeData { get; set; } = new List<CompanyTreeData>();

        public CompanyContactTree(CompanyRef companyRef)
        {
            Company = companyRef;
            var companyContacts = Contact.Helper.Find(x => x.Companies.Any(c => c.Id == companyRef.Id)).ToList();

            var company = companyRef.AsCompany();

            var foundMissing = false;
            foreach (var companyContact in companyContacts)
            {
                if (company.Contacts.All(x => x.Id != companyContact.Id.ToString()))
                {
                    company.Contacts.Add(companyContact.AsContactRef());
                    foundMissing = true;
                }
            }

            if (foundMissing) DAL.Vulcan.Mongo.DocClass.Companies.Company.Helper.Upsert(company);

            CompanyContacts = companyContacts.Select(x => x.AsContactRef()).ToList();

            TreeData.Add(new CompanyTreeData()
            {
                Id = "(root)",
                Parent = true,
                Company =  companyRef
            });

            TreeData.AddRange(companyContacts.Select(x => new CompanyTreeData(x)).ToList());

            //var topLevel = CompanyContacts.Where(x => x.ReportsTo == null).ToList();
            //foreach (var contact in topLevel)
            //{
            //    var node = new CompanyContactTreeNode()
            //    {
            //        Label = contact.FullName,
            //        Contact =  contact.AsContactRef(),
            //        Position = contact.Position,
            //    };
            //    node.Children = AddToContactsTreeRecursive(node);
            //    ContactTree.Add(node);
            //}

        }

        //public List<CompanyContactTreeNode> AddToContactsTreeRecursive(CompanyContactTreeNode parent)
        //{
        //    var result = new List<CompanyContactTreeNode>();
        //    foreach (var contact in CompanyContacts.Where(x=>x.ReportsTo != null && x.ReportsTo.Id == parent.Contact.Id))
        //    {
        //        var node = new CompanyContactTreeNode()
        //        {
        //            Label = contact.FullName,
        //            Contact = contact.AsContactRef(),
        //            Position = contact.Position,
        //        };
        //        node.Children = AddToContactsTreeRecursive(node);
        //        result.Add(node);
        //    }

        //    return result;
        //}
    }

    public class CompanyTreeData
    {
        public string ParentId { get; set; }
        public string Id { get; set; }
        public ContactRef Contact { get; set; }
        public bool Parent { get; set; }
        public CompanyRef Company { get; set; }

        public CompanyTreeData()
        {

        }

        public CompanyTreeData(Contact c)
        {
            if (c.ReportsTo != null)
            {
                ParentId = c.ReportsTo.Id;
            }
            else
            {
                ParentId = "(root)";
            }
            Id = c.Id.ToString();
            Contact = c.AsContactRef();
            Parent = false;
        }
    }
}