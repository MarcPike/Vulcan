using System;
using System.Collections.Generic;
using System.Linq;
//using BLL.Common.ScreenCapture;

namespace BLL.EMail.Core
{
    public class SentEmail
    {
        public string Subject { get; set; }
        public int SentTotal { get; set; }
    }

    public static class EMailSupport
    {
        public static List<SentEmail> PreviousSentEmails = new List<SentEmail>();
        public static void SendEMailToSupportException(string subject, List<string> emailRecipients, Exception exception, string smtpHost = null, string emailFromAddress = null  )
        {

            var emailBuilder = new EMailBuilder()
            {
                Subject = subject,
                Body = FormatExceptionForEMail(exception),
                Recipients = emailRecipients,
                //AttachedFileName = ScreenCapture.CaptureScreenToRandomFile()

            };

            if (smtpHost != null)
                emailBuilder.SmtpHost = smtpHost;

            if (emailFromAddress != null)
                emailBuilder.EMailFromAddress = emailFromAddress;

            emailBuilder.Send();


        }

        public static void SendEmailToSupport(string subject, List<string> emailRecipients, string body, string smtpHost = null, string emailFromAddress = null)
        {
            var previousSent = PreviousSentEmails.FirstOrDefault(x => x.Subject == subject);
            if (previousSent == null)
            {
                previousSent = new SentEmail()
                {
                    Subject = subject,
                    SentTotal = 1
                };
                PreviousSentEmails.Add(previousSent);
            }
            else
            {
                if (previousSent.SentTotal <= 3)
                {
                    previousSent.SentTotal++;
                    PreviousSentEmails[PreviousSentEmails.IndexOf(previousSent)] = previousSent;
                }
                else
                {
                    return;
                }
            }

            var emailBuilder = new EMailBuilder()
            {
                Subject = subject,
                Body = body,
                Recipients = emailRecipients,
                //AttachedFileName = ScreenCapture.CaptureScreenToRandomFile()

            };

            if (smtpHost != null)
                emailBuilder.SmtpHost = smtpHost;

            if (emailFromAddress != null)
                emailBuilder.EMailFromAddress = emailFromAddress;

            emailBuilder.Send();


        }


        public static void SendEMailToSupportException(string subject, string emailRecipientsCommaDelimited, Exception exception, string smtpHost = null, string emailFromAddress = null)
        {

            var emailBuilder = new EMailBuilder()
            {
                Subject = subject,
                Body = FormatExceptionForEMail(exception),
                Recipients = emailRecipientsCommaDelimited.Split(Convert.ToChar(",")).ToList(),
                //AttachedFileName = ScreenCapture.CaptureScreenToRandomFile()
            };

            if (smtpHost != null)
                emailBuilder.SmtpHost = smtpHost;

            if (emailFromAddress != null)
                emailBuilder.EMailFromAddress = emailFromAddress;

            emailBuilder.Send();


        }


        private static string FormatExceptionForEMail(Exception exception)
        {
            var errorMessage = "Exception thrown: ";

            errorMessage += "\n\n" + exception.Message;
            if (exception.InnerException != null)
            {
                errorMessage += "\n\n" + exception.InnerException.Message;
            }

            errorMessage += "\n\nStack Trace:\n" + Environment.StackTrace;
            return errorMessage;
        }

    }
}
