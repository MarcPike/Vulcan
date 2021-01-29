using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Messages;
using DAL.Vulcan.Mongo.Core.DocClass.Notifications;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace Vulcan.SVC.WebApi.Core.Hubs
{
    //[HubName("team")]
    public class TeamHub : Hub, ITeamHub, ISendNotificationRefreshToSignalR
    {
        //private readonly IHelperTeam _helperTeam;
        private readonly IHelperUser _helperUser;

        private readonly TeamHub _teamHub;
        
        private readonly IHubContext<TeamHub> _hubContext;
        private readonly IHelperNotifications _helperNotifications;
        private readonly IHelperQuote _helperQuote;


        public TeamHub(
            //IHelperTeam helperTeam, 
            IHelperUser helperUser,
            IHelperNotifications helperNotifications,
            IHelperQuote helperQuote,
            IHubContext<TeamHub> hubContext
            )
        {
            //_helperTeam = helperTeam;
            _helperUser = helperUser;
            _helperNotifications = helperNotifications;
            _helperQuote = helperQuote;
            _teamHub = this;

            _hubContext = hubContext;

            
            NotificationRouter.RegisterSignalR(this);
        }

        public TeamHub GetHub()
        {
            return _teamHub;
        }

        public async Task QuoteExportStatusChanged(string quoteId)
        {
            await _hubContext.Clients.All.SendAsync("quoteExportStatusChanged", quoteId);
        }

        public async Task PublishUserTeamMessage(string teamMessageId)
        {
            var teamMessage = new RepositoryBase<UserTeamMessage>().Find(teamMessageId);
            if (teamMessage != null)
            {
                var model = new UserTeamMessageModel("vulcancrm", teamMessage.CreatedByUserId, teamMessage);

                var recipients = (model.Recipients.Any() ? model.Recipients : teamMessage.Team.AsTeam().CrmUsers);
                foreach (var crmUserRef in recipients)
                {
                    try
                    {
                        var crmUser = crmUserRef.AsCrmUser();
                        var currentUserTeam = crmUser.ViewConfig.Team;

                        if (model.Team.Id != currentUserTeam.Id) continue;

                        await _hubContext.Clients.All.SendAsync("handleNewTeamMessage", crmUserRef, model);
                        

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error occurred in PublishUserTeamMessage for {crmUserRef.FullName}: \n{e.Message}");
                        throw;
                    }

                }
            }
        }

        public async Task YourTeamSettingsHaveChanged(CrmUserRef userRef)
        {
            await _hubContext.Clients.All.SendAsync("yourTeamSettingsHaveChanged",userRef.Id);
        }

        public async Task SendWonQuoteMessageFromUser(string application, string userId, string quoteId)
        {
            try
            {
                var crmUser = _helperUser.GetCrmUser(application, userId);
                var currentTeam = crmUser.ViewConfig.Team.AsTeam();

                var wonBySalesPerson = crmUser.User.GetFullName();

                var coid = crmUser.User.AsUser().Coid;

                var currency = GetDefaultCurrencyForCoid(coid);

                var quote = _helperQuote.GetQuote(quoteId);
                if (quote.WonDate != null)
                {
                    var quoteTotal = new QuoteTotal(quote.Items.Select(x => x.AsQuoteItem()).ToList(), false);

                    await _hubContext.Clients.All.SendAsync(
                        "receiveQuoteWonMessage",
                        crmUser.UserId,
                        currentTeam.Id.ToString(),
                        wonBySalesPerson,
                        currency,
                        quote.QuoteId,
                        quoteTotal.TotalPrice);
                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private string GetDefaultCurrencyForCoid(string coid)
        {
            var currency = "USD";
            if (coid == "EUR")
                currency = "GBP";
            if (coid == "CAN")
                currency = "CAD";
            return currency;
        }


        public async Task SendRefreshRemindersForUser(CrmUserRef crmUserRef, NotificationRef notificationRef, string type)
        {
            await _hubContext.Clients.All.SendAsync("sendRefreshRemindersForUser",crmUserRef, notificationRef, type);
        }

        public async Task SendRefreshPrivateMessages(string userId, MessageObject messageObject)
        {
            await _hubContext.Clients.All.SendAsync("SendRefreshPrivateMessages",userId,messageObject);
        }

        public async Task SendRefreshTeamMessages(string teamId, MessageObject messageObject)
        {
            await _hubContext.Clients.All.SendAsync("sendRefreshTeamMessages",teamId,messageObject);
        }

        public async Task SendRefreshGroupMessages(string groupId, MessageObject messageObject)
        {
            await _hubContext.Clients.All.SendAsync("sendRefreshGroupMessages",groupId, messageObject);
        }

        public async Task SendRefreshNotificationsForUser(CrmUserRef crmUserRef, string label)
        {
            await _hubContext.Clients.All.SendAsync("sendRefreshNotificationsForUser",crmUserRef, label);
        }

        public async Task SendRefreshActionsForUser(CrmUserRef crmUserRef)
        {
            await _hubContext.Clients.All.SendAsync("sendRefreshActionsForUser",crmUserRef);
        }

        public async Task SendRefreshEmailForUser(string userId)
        {
            await _hubContext.Clients.All.SendAsync("sendRefreshEmailForUser",userId);
        }

        public async Task SendRefreshEmailForContact(string contactId)
        {
            await _hubContext.Clients.All.SendAsync("sendRefreshEmailForContact",contactId);
        }
    }
}