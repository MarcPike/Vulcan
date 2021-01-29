using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Email;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperEmail : HelperBase, IHelperEmail
    {
        private readonly IHelperUser _helperUser;
        private readonly HelperTeam _helperTeam;

        public HelperEmail()
        {
            _helperUser = new HelperUser(new HelperPerson());
            _helperTeam = new HelperTeam(_helperUser);
        }
        
        public List<Email> GetMyEmails(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var rep = new RepositoryBase<Email>();
            var result = crmUser.Emails.Select(x => rep.Find(x.Id)).ToList();

            return result;
        }

        public List<Email> GetMyTeamEmails(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            if (crmUser.ViewConfig.Team == null) return new List<Email>();
            var users = _helperTeam.GetAllUsers(crmUser.ViewConfig.Team);
            var result = new List<Email>();
            foreach (var user in users.ToList())
            {
                try
                {
                    var emailsFound = GetMyEmails(application, user.Id);
                    foreach (var email in emailsFound)
                    {
                        if (result.All(x => x.EmailId.ToString() != email.EmailId.ToString()))
                        {
                            result.Add(email);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return result.OrderByDescending(x => x.Sent).ToList();

        }

        public List<Email> GetContactEmails(string contactId)
        {
            var contact = new RepositoryBase<Contact>().AsQueryable().SingleOrDefault(x=>x.Id == ObjectId.Parse(contactId));
            if (contact == null) throw new Exception("Contact not found");
            var emailsFound = contact.Emails.Select(x => x.AsEmail()).ToList();
            return emailsFound.OrderByDescending(x => x.Sent).ToList();
        }
    }

}
