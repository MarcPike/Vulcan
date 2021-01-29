using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperContact
    {
        Contact GetContact(string contactId);
        Contact SaveContact(ContactModel model);
        ContactModel GetNewContact(string application, string userId);
        void RemoveContact(string application, string userId, string contactId);
        ContactModel UseExistingContact(string application, string userId, string contactId);

        List<ContactRef> GetAllAvailableContactsForCompanyGroup(
            string application, string userId, string companyGroupId);

        List<ContactRef> GetAllAvailableContactsForCompany(
            string application, string userId, string companyId);
        List<ContactRef> GetAllAvailableContactsForProspect(
            string application, string userId, string prospectId);

        List<ContactRef> GetAllAvailableContacts(string application, string userId);
        List<ContactRef> GetAllContacts(string application, string userId);

        (ContactModel contactModel, CompanyContactTree companyContactTree) 
            SetReportsTo(string application, string userId, 
                Company company, ContactRef contact, ContactRef reportsTo);
        CompanyContactTree GetCompanyContactTree(CompanyRef company);
    }
}
