using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.Companies;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperContact : HelperBase, IHelperContact
    {
        private readonly IHelperPerson _helperPerson;
        private readonly IHelperUser _helperUser;

        public HelperContact(
            IHelperPerson helperPerson,
            IHelperUser helperUser)
        {
            _helperPerson = helperPerson;
            _helperUser = helperUser;
        }

        public Contact GetContact(string contactId)
        {
            var contact = new RepositoryBase<Contact>().Find(contactId);
            return contact;
        }

        public void RemoveContact(string application, string userId, string contactId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            foreach (var userContact in crmUser.Contacts.Where(x => x.Id == contactId).ToList())
            {
                crmUser.Contacts.RemoveDocumentRef(userContact);
                crmUser.SaveToDatabase();
            }

            var isNorway = crmUser.ViewConfig.Team.Name.Contains("Norway");

            var contact = new RepositoryBase<Contact>().Find(contactId);
            var contactRef = contact.AsContactRef();
            foreach (var company in contact.Companies.Select(x=>x.AsCompany()).ToList())
            {
                var removeMe = company.Contacts.Where(x => x.Id == contactId).ToList();
                foreach (var removeContact in removeMe)
                {
                    company.Contacts.Remove(removeContact);

                    var companyDefaults = CompanyDefaults.GetCompanyDefaults(company.Location.GetCoid(), company, isNorway);
                    if (companyDefaults.ContactId == removeContact.Id)
                    {
                        companyDefaults.ContactId = String.Empty;
                        companyDefaults.SaveToDatabase();
                    }

                }
                company.SaveToDatabase();
            }
            foreach (var prospect in contact.Prospects.Select(x => x.AsProspect()).ToList())
            {
                var removeMe = prospect.Contacts.Where(x => x.Id == contactId).ToList();
                foreach (var removeContact in removeMe)
                {
                    prospect.Contacts.Remove(contactRef);
                }
                prospect.SaveToDatabase();
            }
        }

        public ContactModel UseExistingContact(string application, string userId, string contactId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var crmUserRef = crmUser.AsCrmUserRef();

            var rep = new RepositoryBase<Contact>();
            var contact = rep.Find(contactId);
            if (contact == null) throw new Exception("Contact not found");

            if (contact.CrmUsers.All(x => x.Id != crmUserRef.Id))
            {
                contact.CrmUsers.Add(crmUserRef);
                contact.SaveToDatabase();
            }

            if (crmUser.Contacts.All(x => x.Id != contact.Id.ToString()))
            {
                crmUser.Contacts.Add(contact.AsContactRef());
                crmUser.SaveToDatabase();
            }

            return new ContactModel(application,userId,contact);
        }

        public Contact SaveContact(ContactModel model)
        {
            var crmUser = _helperUser.GetCrmUser(model.Application,model.UserId);
            _helperPerson.ValidateModel(model.Person);

            var rep = new RepositoryBase<Contact>();
            var repUser = new RepositoryBase<CrmUser>();

            var contact = rep.Find(model.Id) ?? new Contact
            {
                Id = ObjectId.Parse(model.Id),
                CreateDateTime = DateTime.Now
            };

            if (contact.Person == null) contact.Person = new Person();

            UpdateAddresses(model.Person, contact);
            UpdateEmailAddresses(model.Person, contact);
            UpdatePhoneNumbers(model.Person, contact);

            contact.Person.FirstName = model.Person.FirstName;
            contact.Person.LastName = model.Person.LastName;
            contact.Person.MiddleName = model.Person.MiddleName;
            contact.Notes.ResynchWithList(model.Notes);
            contact.Companies.ResyncWithList(model.Companies);
            contact.Prospects.ResyncWithList(model.Prospects);
            contact.CrmUsers.ResyncWithList(model.CrmUsers);
            contact.Actions.ResyncWithList(model.Actions);
            contact.ReportsTo = model.ReportsTo;
            contact.Position = model.Position;

            rep.Upsert(contact);
            
            var contactRef = contact.AsContactRef();

            UpdateContactCrmUsers();

            UpdateContactCompanies();

            UpdateContactProspects();

            UpdateContactActions();

            return contact;

            void UpdateContactActions()
            {
                foreach (var actionRef in model.Actions)
                {
                    var action = actionRef.AsAction();

                    var contactRefForCompany = action.Contacts.SingleOrDefault(x => x.Id == contactRef.Id);

                    if (contactRefForCompany == null)
                    {
                        action.Contacts.Add(contactRef);
                        action.SaveToDatabase();
                    }
                    else
                    {
                        var indexOf = action.Contacts.IndexOf(contactRefForCompany);
                        action.Contacts[indexOf] = contactRef;
                        action.SaveToDatabase();
                    }
                }
            }

            void UpdateContactCompanies()
            {
                foreach (var companyRef in model.Companies)
                {
                    var company = companyRef.AsCompany();

                    var contactRefForCompany = company.Contacts.SingleOrDefault(x => x.Id == contactRef.Id);

                    if (contactRefForCompany == null)
                    {
                        company.Contacts.Add(contactRef);
                        company.SaveToDatabase();
                    }
                    else
                    {
                        var indexOf = company.Contacts.IndexOf(contactRefForCompany);
                        company.Contacts[indexOf] = contactRef;
                        company.SaveToDatabase();
                    }
                }
            }

            void UpdateContactProspects()
            {
                foreach (var prospectRef in model.Prospects)
                {
                    var prospect = prospectRef.AsProspect();
                    var contactRefForProspect = prospect.Contacts.SingleOrDefault(x => x.Id == contactRef.Id);

                    if (contactRefForProspect == null)
                    {
                        prospect.Contacts.Add(contactRef);
                        prospect.SaveToDatabase();
                    }
                    else
                    {
                        var indexOf = prospect.Contacts.IndexOf(contactRefForProspect);
                        prospect.Contacts[indexOf] = contactRef;
                        prospect.SaveToDatabase();
                    }
                }
            }

            void UpdateContactCrmUsers()
            {
                foreach (var userContact in crmUser.Contacts.Where(x => x.Id == contactRef.Id).ToList())
                {
                    crmUser.Contacts.Remove(userContact);
                }

                crmUser.Contacts.Add(contactRef);
                repUser.Upsert(crmUser);
            }
        }

        public ContactModel GetNewContact(string application, string userId)
        {

            return new ContactModel(application, userId, new Contact());
        }

        public List<ContactRef> GetAllAvailableContactsForCompany(
            string application, string userId, string companyId)
        {
            var company = new RepositoryBase<Company>().Find(companyId);
            return company.Contacts;
        }

        public List<ContactRef> GetAllAvailableContactsForProspect(string application, string userId, string prospectId)
        {
            var prospect = new RepositoryBase<Prospect>().Find(prospectId);
            return prospect.Contacts;
        }

        public List<ContactRef> GetAllAvailableContacts(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var rep = new RepositoryBase<Contact>();
            var allContacts = rep.AsQueryable().ToList();
            var contactList = new List<ContactRef>();
            foreach (var contact in allContacts.ToList())
            {
                if (crmUser.Contacts.All(x => x.Id != contact.Id.ToString()))
                {
                    contactList.Add(contact.AsContactRef());
                }
            }
            return contactList;
        }

        public List<ContactRef> GetAllAvailableContactsForCompanyGroup(
            string application, string userId, string companyGroupId)
        {
            var repGroup = new RepositoryBase<CompanyGroup>();
            var group = repGroup.AsQueryable().SingleOrDefault(x => x.Id.ToString() == companyGroupId);
            if (group == null) throw new Exception("Company Group not found");

            var companies = group.Companies;
            var crmUser = _helperUser.GetCrmUser(application, userId);

            var rep = new RepositoryBase<Contact>();
            var allContacts = rep.AsQueryable().ToList();
            var contactList = new List<Contact>();
            foreach (var contact in allContacts.ToList())
            {
                if (crmUser.Contacts.All(x => x.Id != contact.Id.ToString()))
                {
                    contactList.AddRange(
                        from company in contact.Companies
                        where companies.Any(x => x.Id == company.Id)
                        select contact);
                }
            }
            return contactList.Select(x=>x.AsContactRef()).ToList();
        }

        public List<ContactRef> GetAllContacts(
            string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var contacts = new RepositoryBase<Contact>().AsQueryable().ToList();

            return contacts.Select(x => x.AsContactRef()).ToList();
        }

        public (ContactModel contactModel, CompanyContactTree companyContactTree) SetReportsTo(string application, string userId, Company company, ContactRef contact, ContactRef reportsTo)
        {
            if (company.Contacts.All(x => x.Id != contact.Id))
            {
                throw new Exception($"Contact is not a member of ({company.Name}) Contacts list");
            }
            if (company.Contacts.All(x => x.Id != reportsTo.Id))
            {
                throw new Exception($"ReportsTo is not a member of ({company.Name}) Contacts list");
            }

            var c = contact.AsContact();
            c.ReportsTo = reportsTo;
            Contact.Helper.Upsert(c);

            var contactModel = new ContactModel(application, userId, c);
            var companyContactTree = new CompanyContactTree(company.AsCompanyRef());
            return (contactModel, companyContactTree);
        }

        public CompanyContactTree GetCompanyContactTree(CompanyRef company)
        {
            return new CompanyContactTree(company);
        }

        private static void UpdatePhoneNumbers(ContactPersonModel model, Contact contact)
        {
            if (model.PhoneNumbers.Count == 0)
            {
                contact.Person.PhoneNumbers = new List<PhoneNumber>();
            }
            else
            {
                contact.Person.PhoneNumbers = model.PhoneNumbers.Select(x => x.ToBaseValue()).ToList();
            }
        }

        private static void UpdateEmailAddresses(ContactPersonModel model, Contact contact)
        {
            if (model.EmailAddresses.Count == 0)
            {
                contact.Person.EmailAddresses = new List<EmailAddress>();
            }
            else
            {
                contact.Person.EmailAddresses = model.EmailAddresses.Select(x => x.ToBaseValue()).ToList();
            }
        }

        private static void UpdateAddresses(ContactPersonModel model, Contact contact)
        {
            if (model.Addresses.Count == 0)
            {
                contact.Person.Addresses = new List<Address>();
            }
            else
            {
                contact.Person.Addresses = model.Addresses.Select(x => x.ToBaseValue()).ToList();
            }
        }
    }
}