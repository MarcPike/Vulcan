using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.Email;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperEmail
    {
        List<Email> GetMyEmails(string application, string userId);
        List<Email> GetMyTeamEmails(string application, string userId);
        List<Email> GetContactEmails(string contactId);
    }
}