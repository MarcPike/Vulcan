using DAL.Vulcan.Mongo.Base.Core.Context;
using DAL.Vulcan.Mongo.Base.Core.Logger;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Notifications;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Core.PublishSignalR
{
    public static class PublishSignalREvents
    {
        private static readonly VulcanLogger _logger = new VulcanLogger();
        private static HttpClient _httpClient;

        public static string BaseAddress => EnvironmentSettings.GetBaseAddress() + "/signalRExternal";

        private static void InitializeHttpClient()
        {
            if (_httpClient == null)
            {
                var handler = new HttpClientHandler()
                {
                    AllowAutoRedirect = false
                };
                _httpClient = new HttpClient(handler) { BaseAddress = new Uri(BaseAddress) };
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            }
        }

        private static async Task<bool> CallSignalR(string command)
        {
            InitializeHttpClient();
            
            var parameters = new Dictionary<string, object>();
            parameters.Add("External Call", BaseAddress + command);
            try
            {
                HttpResponseMessage result = await _httpClient.GetAsync(BaseAddress + command);
                Console.WriteLine($"Command: {command} Result: {result.IsSuccessStatusCode}");
                return result.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.Log(className: "PublishSignalREvents", methodName: "CallSignalR", sendEmail: true, exception: ex, parameters: parameters);

                Console.WriteLine(ex);
                throw;
            }
        }

        public static async Task<bool> SendNewReminderToUser(CrmUserRef crmUserRef, NotificationRef notificationRef, string type)
        {
            var command = $"/SendNewReminderToUser/{crmUserRef.UserId}/{notificationRef.Id}/{type}";
            return await CallSignalR(command);
        }

        public static async Task<bool> SendRefreshNotificationsForUser(CrmUserRef crmUserRef)
        {
            var command = $"/sendRefreshNotificationsForUser/vulcancrm/{crmUserRef.UserId}";
             return await CallSignalR(command);
        }

        public static async Task<bool> RefreshActionsForUser(CrmUserRef crmUserRef)
        {
            var command = $"/refreshActionsForUser/vulcancrm/{crmUserRef.UserId}";
            return await CallSignalR(command);
        }

        public static async Task<bool> SendRefreshEmailToUser(CrmUserRef crmUserRef)
        {
            var command = $"/SendRefreshEmailToUser/vulcancrm/{crmUserRef.UserId}";
            return await CallSignalR(command);
        }

        public static async Task<bool> PublishRefreshEmailForContact(ContactRef contactRef)
        {
            var command = $"/RefreshEmailForContact/vulcancrm/{contactRef.Id}";
            return await CallSignalR(command);
        }

        public static async Task<bool> PublishYourTeamSettingsHaveChanged(CrmUserRef crmUserRef)
        {
            var command = $"/RefreshEmailForContact/vulcancrm/{crmUserRef.Id}";
            return await CallSignalR(command);
        }

        public static async Task<bool> QuoteExportStatusChanged(string quoteId)
        {
            var command = $"/QuoteExportStatusChanged/{quoteId}";
            return await CallSignalR(command);
        }

        public static async Task<bool> PublishTeamMessage(string teamMessageId)
        {
            var command = $"/PublishTeamMessage/{teamMessageId}";
            return await CallSignalR(command);
        }

    }
}
