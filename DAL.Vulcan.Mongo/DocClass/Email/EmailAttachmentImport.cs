using System.IO;
using DAL.Quotes.Mongo.DocClass.FileAttachment;
using DAL.Vulcan.Mongo.Base.FileAttachment;
using Microsoft.Exchange.WebServices.Data;

namespace DAL.Vulcan.Mongo.DocClass.Email
{
    public static class EmailAttachmentImport
    {
        public static void Execute(Email email, AttachmentCollection attachments)
        {
            for (int i = 0; i <= attachments.Count-1; i++)
            {
                var attachment = attachments[i];
                if (attachment is FileAttachment)
                {
                    FileAttachment fileAttachment = attachment as FileAttachment;

                    if (fileAttachment.IsInline) continue;

                    var thisFolder = Path.GetTempPath();
                    var fileName = thisFolder + attachment.Name;

                    fileAttachment.Load(fileName);

                    var attachmentId = FileAttachmentsVulcan.SaveFileAttachment(email, FileAttachmentType.EmailAttachment, fileName, "EmailService");
                    email.AttachmentObjectIds.Add(attachmentId);

                    File.Delete(fileName);
                    
                }
            }

        }
    }
}