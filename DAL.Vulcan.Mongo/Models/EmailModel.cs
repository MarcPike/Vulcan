using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DAL.Vulcan.Mongo.Base.FileAttachment;
using DAL.Vulcan.Mongo.DocClass.Email;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Models
{
    public class EmailModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public class EmailAttachment
        {
            public string AttachmentId { get; set; }
            public string FileName { get; set; }
            public long Size { get; set; }
        }

        public ObjectId Id { get; set; }
        public Email.EmailAddressData From { get; set; }
        public List<Email.EmailAddressData> To { get; set; } 
        public List<Email.EmailAddressData> Cc { get; set; } 
        public List<Email.EmailAddressData> Bcc { get; set; } 
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public DateTime Sent { get; set; }
        public DateTime Received { get; set; }
        public int AttachmentsCount { get; set; } = 0;
        public List<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
        public List<string> SearchTags { get; set; } = new List<string>();

        public EmailModel(string application, string userId, Email email)
        {
            try
            {
                Application = application;
                UserId = userId;
                Id = email.Id;
                From = email.From;
                To = email.To;
                Cc = email.Cc ?? new List<Email.EmailAddressData>();
                Bcc = email.Bcc ?? new List<Email.EmailAddressData>();
                Subject = email.Subject;
                //Body = email.Body.Contains(" ________________________________ ") ? 
                //    email.Body.Substring(0, email.Body.IndexOf(" ________________________________ ", StringComparison.Ordinal)) 
                //  : email.Body;
                Body = RemoveImageFromBody(email.Body ?? "");
                Created = email.Created;
                Sent = email.Sent;
                Received = email.Received;
                AttachmentsCount = email.AttachmentObjectIds?.Count ?? 0;
                SearchTags = email.SearchTags;
                if (AttachmentsCount > 0)
                {
                    var result = FileAttachmentsVulcan.GetAllAttachmentsForDocument(email);
                    foreach (var fsFileInfo in result)
                    {
                        Attachments.Add(new EmailAttachment()
                        {
                            AttachmentId = fsFileInfo.Id.ToString(),
                            FileName = Path.GetFileName(fsFileInfo.Filename),
                            Size = fsFileInfo.Length
                        });
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private string RemoveImageFromBody(string emailBody)
        {
            var result = emailBody;

            var indexOfImageStart = emailBody.IndexOf("[cid:", StringComparison.Ordinal);
            if (indexOfImageStart > -1)
            {
                var indexOfImageEnd = emailBody.IndexOf("]", indexOfImageStart, StringComparison.Ordinal);
                if (indexOfImageEnd > indexOfImageStart)
                {
                    result = result.Remove(indexOfImageStart, indexOfImageEnd - indexOfImageStart);
                }
            }
            return result;
        }
    }
}
