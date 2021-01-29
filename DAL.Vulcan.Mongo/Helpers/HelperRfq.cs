using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.EMail;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.RequestForQuoteExternal;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperRfq : IHelperRfq
    {
        public List<string> GetLocationNamesForRfq()
        {
            return new List<string>()
            {
                "Texas",
                "Louisiana",
                "Oklahoma",
                "Canada",
                "Cumbernauld",
                "Sheffield"
            };
        }

        private TeamRef GetTeamForLocation(string location)
        {
            var teams = new MongoRawQueryHelper<Team>().GetAll();

            if (location == "Texas") return teams.Single(x => x.Name == "Houston Sales").AsTeamRef();
            if (location == "Louisiana") return teams.Single(x => x.Name == "Louisiana Ragin Cajuns").AsTeamRef();
            if (location == "Oklahoma") return teams.Single(x => x.Name == "Oklahoma Sooner's").AsTeamRef();
            if (location == "Canada") return teams.Single(x => x.Name == "Canada Sales").AsTeamRef();
            if (location == "Cumbernauld") return teams.Single(x => x.Name == "Cumbernauld Sales Team").AsTeamRef();
            if (location == "Sheffield") return teams.Single(x => x.Name == "Sheffield Sales Team").AsTeamRef();

            throw new Exception("Invalid Location specified");
        }

        public List<RfqExternalModel> GetAllForTeam(string application, string userId, TeamRef team, DateTime fromDate, DateTime toDate)
        {
            var queryHelper = new MongoRawQueryHelper<RfqExternal>();
            var filter = queryHelper.FilterBuilder.
                Where(x => x.Team.Id == team.Id && x.Requested >= fromDate && x.Requested <= toDate && x.Status != RfqExternalStatus.Initial);
            var result = queryHelper.Find(filter).ToList().OrderByDescending(x => x.Requested)
                .ToList();
            foreach (var rfq in result)
            {
                rfq.BuildAcceptedText();
                rfq.BuildRejectedText();
                rfq.CountMatchingWords();
                
                queryHelper.Upsert(rfq);
            }

            return result.Select(x => new RfqExternalModel(application, userId,x)).ToList();
        }

        public RfqCustomerModel GetNewRfqExternalModelForCustomer()
        {
            var queryHelper = new MongoRawQueryHelper<RfqExternal>();

            var rfq = queryHelper.Upsert(new RfqExternal()
            {
                RequestForQuoteId = GetNextId(queryHelper),
                CompanyName = String.Empty,
                ContactEmailAddress = String.Empty,
                ContactName = String.Empty,
                KeywordMatches = 0,
                RfqText = String.Empty,
                Status = RfqExternalStatus.Initial,
                Requested = DateTime.Now,
                Reviewed = null
            });

            return new RfqCustomerModel()
            {
                Id = rfq.Id.ToString(),
                CompanyName = rfq.CompanyName,
                ContactEmailAddress = rfq.ContactEmailAddress,
                ContactName = rfq.ContactName,
                Location = string.Empty,
                RequestForQuoteId = rfq.RequestForQuoteId,
                RfqText = rfq.RfqText
            };

        }

        public void SaveRfqExternal(RfqCustomerModel model)
        {
            var queryHelper = new MongoRawQueryHelper<RfqExternal>();
            var team = GetTeamForLocation(model.Location);

            var rfq = queryHelper.FindById(model.Id);
            if (rfq == null) throw new Exception("RFQ could not be found");

            rfq.Team = team;
            rfq.Status = RfqExternalStatus.Pending;
            rfq.CompanyName = model.CompanyName;
            rfq.ContactEmailAddress = model.ContactEmailAddress;
            rfq.ContactName = model.ContactName;
            rfq.ContactPhone = model.ContactPhone;
            rfq.RfqText = model.RfqText;

            rfq.CountMatchingWords();

            queryHelper.Upsert(rfq);

        }

        public RfqExternalModel Accept(RfqExternalModel model)
        {
            var queryHelper = new MongoRawQueryHelper<RfqExternal>();

            var rfq = queryHelper.FindById(model.Id);
            if (rfq == null) throw new Exception("RFQ not found");

            rfq.Status = RfqExternalStatus.Accepted;
            rfq.SalesPerson = model.SalesPerson;
            rfq.Reviewed = DateTime.Now;
            rfq.AcceptText = model.AcceptText;

            try
            {
                EmailRfq(rfq);
            }
            catch (Exception ex)
            {
                var support = new List<string>()
                {
                    "marc.pike@howcogroup.com",
                    "isidro.gallegos@howcogroup.com"
                };

                EMailSupport.SendEmailToSupport(subject:$"RFQ Email Excaption for {rfq.RequestForQuoteId}", 
                    emailRecipients:support, body:$"{ex.Message} \n {ex.InnerException?.Message ?? String.Empty}");
                Console.WriteLine(ex);
            }
            queryHelper.Upsert(rfq);

            return new RfqExternalModel(model.Application, model.UserId, rfq);
        }

        public RfqExternalModel Reject(RfqExternalModel model)
        {
            var queryHelper = new MongoRawQueryHelper<RfqExternal>();

            var rfq = queryHelper.FindById(model.Id);
            if (rfq == null) throw new Exception("RFQ not found");

            rfq.Status = RfqExternalStatus.Rejected;
            rfq.SalesPerson = model.SalesPerson;
            rfq.Reviewed = DateTime.Now;
            rfq.RejectText = model.RejectText;

            try
            {
                EmailRfq(rfq);
            }
            catch (Exception ex)
            {
                var support = new List<string>()
                {
                    "marc.pike@howcogroup.com",
                    "isidro.gallegos@howcogroup.com"
                };

                EMailSupport.SendEmailToSupport(subject: $"RFQ Email Excaption for {rfq.RequestForQuoteId}",
                    emailRecipients: support, body: $"{ex.Message} \n {ex.InnerException?.Message ?? String.Empty}");
                Console.WriteLine(ex);
            }

            queryHelper.Upsert(rfq);
            return new RfqExternalModel(model.Application, model.UserId, rfq);
        }

        private void EmailRfq(RfqExternal rfq)
        {
            var salesPerson = rfq.SalesPerson.AsCrmUser();
            var user = salesPerson.User.AsUser();
            var emailAddress = user.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business);

            if (emailAddress == null) throw new Exception("SalesPerson email address not found");

            var subject = $"{rfq.RequestForQuoteId} {rfq.Status}";

            var bodyBuilder = new StringBuilder();

            bodyBuilder.Append((rfq.Status == RfqExternalStatus.Accepted) ? rfq.AcceptText : rfq.RejectText);

            bodyBuilder.AppendLine();

            bodyBuilder.AppendLine("Request:");
            bodyBuilder.AppendLine();
            bodyBuilder.Append(rfq.RfqText);

            var emailBuilder = new EMailBuilder
            {
                Subject = subject,
                Body = bodyBuilder.ToString(),
                EMailFromAddress = "noreply@howcogroup.com"
            };

            emailBuilder.Recipients.Add(emailAddress.Address);
            emailBuilder.Recipients.Add(rfq.ContactEmailAddress);

            emailBuilder.Send(false);
        }

        public RfqTeamConfigModel GetTeamConfig(string application, string userId, TeamRef team)
        {
            var queryHelper = new MongoRawQueryHelper<RfqTeamConfig>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Team.Id == team.Id);
            var rfqTeamConfig = queryHelper.Find(filter).FirstOrDefault();
            if (rfqTeamConfig == null)
            {
                rfqTeamConfig = new RfqTeamConfig()
                {
                    Team = team,
                };
                queryHelper.Upsert(rfqTeamConfig);
            }

            return new RfqTeamConfigModel(application, userId, rfqTeamConfig);

        }
        public RfqTeamConfigModel SaveTeamConfig(RfqTeamConfigModel model)
        {
            var queryHelper = new MongoRawQueryHelper<RfqTeamConfig>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Team.Id == model.Team.Id);
            var rfqTeamConfig = queryHelper.Find(filter).FirstOrDefault();
            if (rfqTeamConfig == null)
            {
                rfqTeamConfig = new RfqTeamConfig()
                {
                    Team = model.Team,
                };
            }

            rfqTeamConfig.Keywords = model.Keywords;
            rfqTeamConfig.AcceptText = model.AcceptText;
            rfqTeamConfig.RejectText = model.RejectText;
            queryHelper.Upsert(rfqTeamConfig);
            return new RfqTeamConfigModel(model.Application, model.UserId, rfqTeamConfig);

        }

        private int GetNextId(MongoRawQueryHelper<RfqExternal> queryHelper)
        {
            var filter = queryHelper.FilterBuilder.Empty;
            var project = queryHelper.ProjectionBuilder.Expression(x => x.RequestForQuoteId);
            var idList = queryHelper.FindWithProjection(filter, project).ToList();
            if (!idList.Any()) return 5000;
            return idList.Max() + 1;
        }

    }
}
