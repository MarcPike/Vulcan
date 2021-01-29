using DAL.Vulcan.Mongo.Base.Core.Queries;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Models;
using DAL.Vulcan.Mongo.Core.Portal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public class HelperPortal : IHelperPortal
    {
        public NewPortalInvitationModel CreateNewPortalInvitationModel(string companyId, string contactId, string salesPersonId)
        {
            var company = new RepositoryBase<Company>().Find(companyId);
            if (company == null) throw new Exception("Company not found");

            var contact = new RepositoryBase<Contact>().Find(contactId);
            if (contact == null) throw new Exception("Contact not found");

            var salesPerson = new RepositoryBase<CrmUser>().Find(salesPersonId);
            if (salesPerson == null) throw new Exception("SalesPerson not found");

            if ((salesPerson.UserType == CrmUserType.Resource) || (salesPerson.UserType == CrmUserType.Accountant))
            {
                throw new Exception($"User is not a SalesPerson: {salesPerson.UserType}");
            }

            return new NewPortalInvitationModel()
            {
                Company = company.AsCompanyRef(),
                Contact = contact.AsContactRef(),
                SalesPerson = salesPerson.AsCrmUserRef(),
                EmailAddress = contact.Person.EmailAddresses.FirstOrDefault()?.Address ?? string.Empty,
                EmailBody = string.Empty,
                EmailSubject = string.Empty
            };

        }

        public PortalInvitationModel SendPortalInvitation(NewPortalInvitationModel model)
        {
            try
            {
                var queryHelper = new MongoRawQueryHelper<PortalInvitation>();
                var invitation = new PortalInvitation()
                {
                    SalesPerson = model.SalesPerson,
                    Company = model.Company,
                    Contact = model.Contact,
                    DateSent = DateTime.Now,
                    EmailBody = model.EmailBody,
                    EmailAddress = model.EmailAddress,
                    Status = PortalInvitationStatus.Pending
                };
                queryHelper.Upsert(invitation);
                invitation.SendEmail();
                return new PortalInvitationModel(invitation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public List<PortalInvitationModel> GetInvitationsSentToContact(string contactId)
        {
            var queryHelper = new MongoRawQueryHelper<PortalInvitation>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Contact.Id == contactId);
            var project = queryHelper.ProjectionBuilder.Expression(x => new PortalInvitationModel(x));
            var result = queryHelper.FindWithProjection(filter, project).OrderByDescending(x => x.DateSent).ToList();
            return result;
        }

        public List<PortalInvitationModel> GetInvitationsSentBySalesPerson(string salesPersonId)
        {
            var queryHelper = new MongoRawQueryHelper<PortalInvitation>();
            var filter = queryHelper.FilterBuilder.Where(x => x.SalesPerson.Id == salesPersonId);
            var project = queryHelper.ProjectionBuilder.Expression(x => new PortalInvitationModel(x));
            var result = queryHelper.FindWithProjection(filter, project).OrderByDescending(x => x.DateSent).ToList();
            return result;
        }

        public List<PortalInvitationModel> GetInvitationsSentToCompany(string companyId)
        {
            var queryHelper = new MongoRawQueryHelper<PortalInvitation>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Company.Id == companyId);
            var project = queryHelper.ProjectionBuilder.Expression(x => new PortalInvitationModel(x));
            var result = queryHelper.FindWithProjection(filter, project).OrderByDescending(x => x.DateSent).ToList();
            return result;
        }

        public List<PortalInvitationModel> GetInvitationsForTeam(string teamId)
        {
            var result = new List<PortalInvitationModel>();
            var team = new MongoRawQueryHelper<Team>().FindById(teamId);
            if (team == null) throw new Exception("Team not found");

            foreach (var salesPerson in team.CrmUsers)
            {
                result.AddRange(GetInvitationsSentBySalesPerson(salesPerson.Id));
            }

            return result.OrderByDescending(x => x.DateSent).ToList();
        }

    }
}
