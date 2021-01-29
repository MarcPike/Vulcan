using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson;
using System;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperProspect : HelperBase, IHelperProspect
    {
        private readonly IHelperUser _helperUser;
        private readonly IHelperLocation _helperLocation;

        public HelperProspect()
        {
            var helperPerson = new HelperPerson();
            _helperUser = new HelperUser(helperPerson);
            _helperLocation = new HelperLocation();
        }

        public ProspectModel GetNewProspectModel(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            return new ProspectModel(application,userId,crmUser.User.AsUser().Location);
        }

        public ProspectModel GetNewProspectModel(string application, string userId, string locationId)
        {
            var location = _helperLocation.GetLocation(locationId);
            return new ProspectModel(application, userId, location.AsLocationRef());
        }

        public (ProspectModel ProspectModel, ProspectRef ProspectRef) GetProspect(string application, string userId,
            string prospectId)
        {
            var rep = new RepositoryBase<Prospect>();

            var prospect = rep.Find(prospectId);
            if (prospect == null)
            {
                throw new Exception("Prospect not found");
            }

            var prospectModel = new ProspectModel(prospect, application, userId );
            var prospectRef = prospect.AsProspectRef();
            return (prospectModel, prospectRef);

        }

        public (ProspectModel ProspectModel, ProspectRef ProspectRef) SaveProspect(ProspectModel model)
        {
            var rep = new RepositoryBase<Prospect>();
           
            var prospect = rep.Find(model.Id);
            if (prospect == null)
            {
                prospect = new Prospect()
                {
                    Id = ObjectId.Parse(model.Id),
                    CreatedByUserId = model.UserId
                };
            }
            else
            {
                prospect.ModifiedByUserId = model.UserId;
            }
            prospect.Name = model.Name;
            //prospect.ShortName = model.ShortName;
            //prospect.Code = model.Code;
            prospect.Location = model.Location;
            prospect.Branch = model.Branch;
            prospect.Addresses = model.Addresses;
            prospect.Notes = model.Notes;
            prospect.PhoneNumbers = model.PhoneNumbers;
            prospect.Contacts = model.Contacts;
            prospect.Company = model.Company;
            prospect.Contacts = model.Contacts;
            prospect.AddedToSystem = model.AddedToSystem;
            prospect.SearchTags = model.SearchTags;
            rep.Upsert(prospect);


            var prospectRef = prospect.AsProspectRef();

            foreach (var contact in prospect.Contacts.Select(x=>x.AsContact()))
            {
                if (contact.Prospects.All(x=>x.Id != prospect.Id.ToString()))
                {
                    contact.Prospects.Add(prospectRef);
                    contact.SaveToDatabase();
                }
            }

            var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);
            var teamRef = crmUser.ViewConfig.Team;
            if (teamRef != null)
            {
                var helperTeam = new HelperTeam(_helperUser);
                var team = teamRef.AsTeam();

                if (team.Prospects.All(x => x.Id != prospectRef.Id))
                {
                    var teamModel = new TeamModel(team, model.Application, model.UserId);

                    teamModel.Prospects.Add(prospectRef);
                    helperTeam.SaveTeam(teamModel);
                }
            }

            return (new ProspectModel(prospect, model.Application, model.UserId), prospectRef);
        }

        public Prospect GetProspect(string prospectId)
        {
            return new RepositoryBase<Prospect>().Find(prospectId);
        }

        public void ConvertProspectIntoCompany(string prospectId, string companyId, string teamId)
        {
            var prospect = GetProspect(prospectId);
            var company = new RepositoryBase<Company>().Find(companyId);
            var companyRef = company.AsCompanyRef();

            var team = new RepositoryBase<Team>().Find(teamId);

            // Get latest stuff from iMetal
            CompanyResolver.Execute(company);
            
            // force reload of company
            company = new RepositoryBase<Company>().Find(companyId);

            if (!company.Addresses.Any())
            {
                throw new Exception("Company has no defined addresses");
            }


            ValidateProspectAndCompanyAndTeam();
            LookForNewContacts();
            ModifyQuotesToUseCompanyInsteadOfProspect();
            ModifyProspectCompany();
            MakeSureCompanyIsInTeam();
            DisableProspectFromFutureUse();


            void LookForNewContacts()
            {
                foreach (var contactRef in prospect.Contacts)
                {
                    if (company.Contacts.All(x => x.Id != contactRef.Id))
                    {
                        company.Contacts.Add(contactRef);
                        company.SaveToDatabase();

                    }

                    var contact = contactRef.AsContact();
                    if (contact.Companies.All(x => x.Id != companyRef.Id))
                    {
                        contact.Companies.Add(companyRef);
                        contact.SaveToDatabase();
                    }

                }
            }

            void ValidateProspectAndCompanyAndTeam()
            {
                if (prospect == null) throw new Exception("Prospect not found");
                if (company == null) throw new Exception("Company not found");
                if (team == null) throw new Exception("Team not found");
            }

            void ModifyQuotesToUseCompanyInsteadOfProspect()
            {
                var quotesForProspect =
                    new RepositoryBase<CrmQuote>().AsQueryable().Where(x => x.Prospect != null && x.Prospect.Id == prospectId).ToList();
                foreach (var quote in quotesForProspect)
                {
                    quote.Company = companyRef;
                    quote.ShipToAddress = company.Addresses.First();
                    quote.Prospect = null;
                    quote.SaveToDatabase();
                }
            }

            void ModifyProspectCompany()
            {
                prospect.Company = companyRef;
                prospect.AddedToSystem = DateTime.Now;
                prospect.SaveToDatabase();
            }

            void MakeSureCompanyIsInTeam()
            {
                if (team.Companies.All(x => x.Id != companyRef.Id))
                {
                    team.Companies.Add(companyRef);
                    team.SaveToDatabase();
                }
            }

            void DisableProspectFromFutureUse()
            {
                
            }
        }
    }

}
