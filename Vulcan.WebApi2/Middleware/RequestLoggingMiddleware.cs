using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using DAL.Vulcan.Mongo.RequestLogging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using BLL.EMail;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Models;
using Newtonsoft.Json.Linq;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace Vulcan.WebApi2.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                try
                {
                    if (HandleSaveQuoteException(context, exception)) return;
                    HandleOtherExceptions(context, exception);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            finally
            {
                var actualCall = context.Request?.Path.Value ?? string.Empty;
                var statusCode = context.Response?.StatusCode ?? -1000;
                if ((actualCall.ToUpper().Contains("QUOTES/")) || (actualCall.ToUpper().Contains("IMETAL/")))
                {
                    if (statusCode != 204)
                    {
                        RequestLog.Create(context.Request?.Method, context.Request?.Path.Value, statusCode);
                    }
                }
            }
        }

        private static bool HandleSaveQuoteException(HttpContext context, Exception exception)
        {

            var actualCall = context.Request?.Path.Value ?? string.Empty;
            if (!actualCall.Contains("SaveQuote")) return false;

            context.Request.Body.Seek(0, SeekOrigin.Begin);
            using (StreamReader stream = new StreamReader(context.Request.Body))
            {
                try
                {
                    var body = new StringBuilder();

                    var emailBuilder = new EMailBuilder()
                    {
                        Subject = $"{EnvironmentSettings.CurrentEnvironment.ToString()} - Unhandled Vulcan Exception for Route[{context.Request?.Path.Value}]",
                    };

                    string requestBody = stream.ReadToEnd();
                    var model = JObject.Parse(requestBody);
                    var modelData = model.GetValue("QuoteModel");
                    var quoteModel = modelData.ToObject<QuoteModel>();

                    emailBuilder.Subject +=
                        $"QuoteId: {quoteModel.QuoteId} SalesPerson: {quoteModel.SalesPerson.FullName}";

                    body.AppendLine(exception.Message);
                    if (exception.InnerException != null)
                    {
                        body.AppendLine();
                        body.AppendLine($"Exception: {exception.Message}");
                        body.AppendLine(exception.StackTrace);

                        if (exception.InnerException != null)
                        {
                            body.AppendLine($"Inner Exception: {exception.InnerException.Message}");
                            body.AppendLine(exception.InnerException.StackTrace);
                        }

                        emailBuilder.Recipients.Add("marc.pike@howcogroup.com");
                        emailBuilder.Recipients.Add("isidro.gallegos@howcogroup.com");
                        emailBuilder.Send();
                    }

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private static void HandleOtherExceptions(HttpContext context, Exception exception)
        {

            var actualCall = context.Request?.Path.Value ?? string.Empty;

            var body = new StringBuilder();

            var emailBuilder = new EMailBuilder()
            {
                Subject = $"{EnvironmentSettings.CurrentEnvironment.ToString()} - Unhandled Vulcan Exception for Route[{context.Request?.Path.Value}]",
            };

            body.AppendLine(exception.Message);
            if (exception.InnerException != null)
            {
                body.AppendLine();
                body.AppendLine($"Exception: {exception.Message}");
                body.AppendLine(exception.StackTrace);

                if (exception.InnerException != null)
                {
                    body.AppendLine($"Inner Exception: {exception.InnerException.Message}");
                    body.AppendLine(exception.InnerException.StackTrace);
                }

                emailBuilder.Recipients.Add("marc.pike@howcogroup.com");
                emailBuilder.Recipients.Add("isidro.gallegos@howcogroup.com");
                emailBuilder.Send();
            }

        }

    }
}
