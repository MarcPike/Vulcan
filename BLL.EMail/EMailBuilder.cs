using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace BLL.EMail
{
    public class EMailBuilder : IDisposable
    {
        public string SmtpHost { get; set; }
        public string EMailFromAddress { get; set; }

        public List<string> Recipients { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string AttachedFileName { get; set; }
        private byte[] AttachedFileBytes { get; set; } = new byte[0];
        public Stream AttachedFileStream { get; set; }

        public EMailBuilder()
        {
            SmtpHost = "S-US-MAIL01"; // S-US-MAIL01 After 5/1/2015
            EMailFromAddress = "no-reply@howcogroup.com";
            Recipients = new List<string>();
            Subject = "Unknown";
            Body = "Unknown";
        }

        public void Send(bool isBodyHtml=false)
        {
            var email = new MailMessage();

            foreach (var addr in Recipients) email.To.Add(new MailAddress(addr));
            email.From = new MailAddress(EMailFromAddress);

            email.Subject = Subject;
            email.Body = Body;
            email.IsBodyHtml = isBodyHtml;
            email.Priority = MailPriority.Normal;

            //using(FileStream fileStream = new FileStream(AttachedFileName,))
            //email.Attachments.Add(new Attachment());


            if (AttachedFileStream != null)
            {
                AttachedFileStream.Position = 0;
                Attachment attachment = new Attachment(AttachedFileStream, new ContentType(MediaTypeNames.Application.Octet));
                attachment.ContentDisposition.FileName = AttachedFileName;
                attachment.ContentDisposition.Size = AttachedFileStream.Length;

                email.Attachments.Add(attachment);
            }

            using (var smtp = new SmtpClient(SmtpHost))
            {
                smtp.Send(email);
            }

        }

        public void Dispose()
        {
            AttachedFileBytes = null;
            AttachedFileStream?.Dispose();
        }
    }
}