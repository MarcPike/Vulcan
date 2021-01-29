using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class CompanyContactTree
    {
        public CompanyContactTree(CompanyRef companyRef)
        {
            Company = companyRef;

            var company = companyRef.AsCompany();
            var companyContacts = company.Contacts.Where(x => x != null).ToList();


            // var foundMissing = false;
            // foreach (var companyContact in companyContacts)
            // {
            //     if (company.Contacts.All(x => x.Id != companyContact.Id.ToString()))
            //     {
            //         company.Contacts.Add(companyContact.AsContactRef());
            //         foundMissing = true;
            //     }
            // }
            //
            // if (foundMissing) Companies.Company.Helper.Upsert(company);

            CompanyContacts = companyContacts;

            TreeData.Add(new CompanyTreeData
            {
                Id = "(root)",
                Parent = true,
                Company = companyRef
            });

            TreeData.AddRange(companyContacts.Select(x => new CompanyTreeData(x.AsContact())).ToList());
        }

        public CompanyRef Company { get; set; }
        public List<ContactRef> CompanyContacts { get; set; }

        public List<CompanyTreeData> TreeData { get; set; } = new List<CompanyTreeData>();


        public class CompanyTreeData
        {
            public CompanyTreeData()
            {
            }

            public CompanyTreeData(Contact c)
            {
                if (c == null) return;

                if (c.ReportsTo != null)
                    ParentId = c.ReportsTo.Id;
                else
                    ParentId = "(root)";
                Id = c.Id.ToString();
                Contact = c.AsContactRef();
                Parent = false;
            }

            public string ParentId { get; set; }
            public string Id { get; set; }
            public ContactRef Contact { get; set; }
            public bool Parent { get; set; }
            public CompanyRef Company { get; set; }
        }
    }
}