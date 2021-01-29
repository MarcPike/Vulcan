using System;
using System.Collections.Generic;
using System.Net;
using DAL.Vulcan.Mongo.DocClass.CRM;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Logger;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.Mongo.PublishSignalR
{
    public static class PublishSignalREvents
    {
        private static readonly VulcanLogger _logger = new VulcanLogger();
        //public static string BaseAddress { get; set; } = "http://s-us-vapi01.howcogroup.com:80/signalRExternal";
        //public static string BaseAddress { get; set; } = "http://s-us-vapiqc.howcogroup.com:80/signalRExternal";
        //public static string BaseAddress { get; set; } = "http://192.168.102.223:5000/signalRExternal";
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
            }
        }
        
        private static async Task<bool> CallSignalR(string command)
        {
            InitializeHttpClient();
            
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var parameters = new Dictionary<string, object>();
            parameters.Add("External Call", BaseAddress + command);
            try
            {
                HttpResponseMessage result = await _httpClient.GetAsync(BaseAddress + command);
                //_logger.Log(logLevel: VulcanLogLevel.Debug, message: "No errors found", className: "PublishSignalREvents", methodName: "CallSignalR", sendEmail: true, parameters: parameters);
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
