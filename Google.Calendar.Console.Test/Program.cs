using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Collections.Generic;


namespace Google.Calendar.Console.Test
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] CalendarScopes = { CalendarService.Scope.CalendarReadonly };
        static string[] DriveScopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";
        private static UserCredential CalendarCreds;
        private static UserCredential DriveCreds;

        protected static void GetCalendarEvents()
        {
            using (var stream =
    new FileStream("CalendarCreds.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "calendarToken.json";
                CalendarCreds = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    CalendarScopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                System.Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = CalendarCreds,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            System.Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    System.Console.WriteLine("{0} ({1})", eventItem.Summary, when);
                }
            }
            else
            {
                System.Console.WriteLine("No upcoming events found.");
            }

        }

        protected static void GetDriveFiles()
        {
            using (var stream =
                new FileStream("DriveCreds.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "driveToken.json";
                DriveCreds = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    DriveScopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                System.Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = DriveCreds,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            System.Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    System.Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                System.Console.WriteLine("No files found.");
            }
        }

        static void Main(string[] args)
        {
            GetCalendarEvents();
            GetDriveFiles();
            System.Console.Read();

        }


    }
}
