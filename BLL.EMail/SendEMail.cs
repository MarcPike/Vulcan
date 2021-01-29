using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BLL.EMail
{
    public static class SendEMail
    {
        public static void Execute(string subject, List<string> emailRecipients, Exception ex, string smtpHost = null, string emailFromAddress = null)
        {
            var body = "The following error occurred:\n\n" + ex.Message;

            if (ex.InnerException != null) body += "\n\n" + ex.InnerException.Message;


            body += "\nError occurred on the following lines:\n"+Environment.StackTrace;

            using (var emailBuilder = new EMailBuilder()
            {
                Subject = subject,
                Body = body,
                Recipients = emailRecipients
            })
            {
                if (smtpHost != null)
                    emailBuilder.SmtpHost = smtpHost;

                if (emailFromAddress != null)
                    emailBuilder.EMailFromAddress = emailFromAddress;

                emailBuilder.Send();
                emailBuilder.Dispose();

            }



            //var emailBuilder = new EMailBuilder()
            //{
            //    Subject = subject,
            //    Body = body,
            //    Recipients = emailRecipients
            //};

            //if (smtpHost != null)
            //    emailBuilder.SmtpHost = smtpHost;

            //if (emailFromAddress != null)
            //    emailBuilder.EMailFromAddress = emailFromAddress;

            //emailBuilder.Send();

        }

        public static void Execute(string subject, string body, List<string> emailRecipients, string emailFromAddress, bool html)
        {
            using (var emailBuilder = new EMailBuilder()
            {
                Subject = subject,
                Body = body,
                Recipients = emailRecipients
            })
            {
                if (emailFromAddress != null)
                    emailBuilder.EMailFromAddress = emailFromAddress;

                emailBuilder.Send(html);
                emailBuilder.Dispose();


            }



            //var emailBuilder = new EMailBuilder()
            //{
            //    Subject = subject,
            //    Body = body,
            //    Recipients = emailRecipients
            //};



            //if (emailFromAddress != null)
            //    emailBuilder.EMailFromAddress = emailFromAddress;

            //emailBuilder.Send(html);

        }

        public static void Execute(string subject, List<string> emailRecipients, string body, string smtpHost = null, string emailFromAddress = null)
        {
            using (var emailBuilder = new EMailBuilder()
            {
                Subject = subject,
                Body = body,
                Recipients = emailRecipients
            })
            {
                if (smtpHost != null)
                    emailBuilder.SmtpHost = smtpHost;

                if (emailFromAddress != null)
                    emailBuilder.EMailFromAddress = emailFromAddress;

                emailBuilder.Send();
                emailBuilder.Dispose();

            }


            //var emailBuilder = new EMailBuilder()
            //{
            //    Subject = subject,
            //    Body = body,
            //    Recipients = emailRecipients
            //};

            //if (smtpHost != null)
            //    emailBuilder.SmtpHost = smtpHost;

            //if (emailFromAddress != null)
            //    emailBuilder.EMailFromAddress = emailFromAddress;

            //emailBuilder.Send();

        }

        public static void Execute(string subject, string emailRecipientsCommaDelimited, string body, string smtpHost = null, string emailFromAddress = null)
        {
            using (var emailBuilder = new EMailBuilder()
            {
                Subject = subject,
                Body = body,
                Recipients = emailRecipientsCommaDelimited.Split(Convert.ToChar(",")).ToList()
            })
            {
                if (smtpHost != null)
                    emailBuilder.SmtpHost = smtpHost;

                if (emailFromAddress != null)
                    emailBuilder.EMailFromAddress = emailFromAddress;

                emailBuilder.Send();
                emailBuilder.Dispose();


            }


            //var emailBuilder = new EMailBuilder()
            //{
            //    Subject = subject,
            //    Body = body,
            //    Recipients = emailRecipientsCommaDelimited.Split(Convert.ToChar(",")).ToList()
            //};

            //if (smtpHost != null)
            //    emailBuilder.SmtpHost = smtpHost;

            //if (emailFromAddress != null)
            //    emailBuilder.EMailFromAddress = emailFromAddress;

            //emailBuilder.Send();

        }

        public static void Execute(string subject, string emailRecipientsCommaDelimited, string body, bool isBodyHtml, string smtpHost = null, string emailFromAddress = null )
        {
            using (var emailBuilder = new EMailBuilder()
            {
                Subject = subject,
                Body = body,
                Recipients = emailRecipientsCommaDelimited.Split(Convert.ToChar(",")).ToList()
            })
            {
                if (smtpHost != null)
                    emailBuilder.SmtpHost = smtpHost;

                if (emailFromAddress != null)
                    emailBuilder.EMailFromAddress = emailFromAddress;

                emailBuilder.Send(isBodyHtml);
                emailBuilder.Dispose();


            }

            //var emailBuilder = new EMailBuilder()
            //{
            //    Subject = subject,
            //    Body = body,
            //    Recipients = emailRecipientsCommaDelimited.Split(Convert.ToChar(",")).ToList()
            //};

            //if (smtpHost != null)
            //    emailBuilder.SmtpHost = smtpHost;

            //if (emailFromAddress != null)
            //    emailBuilder.EMailFromAddress = emailFromAddress;

            //emailBuilder.Send(isBodyHtml);

        }

        public static void Execute(string subject, string body, List<string> emailRecipients, string emailFromAddress, string attachment)
        {
            using (var emailBuilder = new EMailBuilder()
            {
                Subject = subject,
                Body = body,
                Recipients = emailRecipients
            })
            {
                if (emailFromAddress != null)
                    emailBuilder.EMailFromAddress = emailFromAddress;

                emailBuilder.AttachedFileName = attachment;

                emailBuilder.Send();
                emailBuilder.Dispose();


            }


            //var emailBuilder = new EMailBuilder()
            //{
            //    Subject = subject,
            //    Body = body,
            //    Recipients = emailRecipients
            //};

            //if (emailFromAddress != null)
            //    emailBuilder.EMailFromAddress = emailFromAddress;

            //emailBuilder.AttachedFileName = attachment;

            //emailBuilder.Send();
        }

        public static void Execute(string subject, string body, List<string> emailRecipients, string emailFromAddress, string fileName, Stream stream)
        {
            using (var emailBuilder = new EMailBuilder()
            {
                Subject = subject,
                Body = body,
                Recipients = emailRecipients
            })
            {
                if (emailFromAddress != null)
                    emailBuilder.EMailFromAddress = emailFromAddress;

                emailBuilder.AttachedFileStream = stream;
                emailBuilder.AttachedFileName = fileName;

                emailBuilder.Send();
                emailBuilder.Dispose();

            }

            //var emailBuilder = new EMailBuilder()
            //{
            //    Subject = subject,
            //    Body = body,
            //    Recipients = emailRecipients
            //};

            //if (emailFromAddress != null)
            //    emailBuilder.EMailFromAddress = emailFromAddress;

            //emailBuilder.AttachedFileStream = stream;
            //emailBuilder.AttachedFileName = fileName;

            //emailBuilder.Send();
        }

    }
}
