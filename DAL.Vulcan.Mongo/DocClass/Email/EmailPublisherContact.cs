using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.PublishSignalR;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.DocClass.Email
{
    public static class EmailPublisherContact
    {
        
        public static void Execute(Email email)
        {
            var emailRef = email.AsEmailRef();
            var rep = new RepositoryBase<DAL.Vulcan.Mongo.DocClass.CRM.Contact>();
            var contactsFound = rep.AsQueryable()
                .Where(x => x.Person.EmailAddresses.Any(e => e.Address == email.From.Address)).ToList();
            foreach (var contact in contactsFound)
            {
                if (contact.Emails == null) contact.Emails = new ReferenceList<Email, EmailRef>();
                contact.Emails.Add(emailRef);
                contact.SaveToDatabase();
                var task = Task.Run(() => PublishSignalREvents.PublishRefreshEmailForContact(contact.AsContactRef()));
                task.Wait();
            }

            foreach (var emailAddress in email.To.Select(x => x.Address).ToList())
            {
                contactsFound = rep.AsQueryable()
                    .Where(x => x.Person.EmailAddresses.Any(e => e.Address == emailAddress)).ToList();
                foreach (var contact in contactsFound)
                {
                    if (contact.Emails == null) contact.Emails = new ReferenceList<Email, EmailRef>();
                    contact.Emails.Add(emailRef);
                    contact.SaveToDatabase();
                    var task = Task.Run(() => PublishSignalREvents.PublishRefreshEmailForContact(contact.AsContactRef()));
                    task.Wait();
                }
            }

            foreach (var emailAddress in email.Cc.Select(x => x.Address).ToList())
            {
                contactsFound = rep.AsQueryable()
                    .Where(x => x.Person.EmailAddresses.Any(e => e.Address == emailAddress)).ToList();
                foreach (var contact in contactsFound)
                {
                    if (contact.Emails == null) contact.Emails = new ReferenceList<Email, EmailRef>();
                    contact.Emails.Add(emailRef);
                    contact.SaveToDatabase();
                    var task = Task.Run(() => PublishSignalREvents.PublishRefreshEmailForContact(contact.AsContactRef()));
                    task.Wait();
                }
            }

            foreach (var emailAddress in email.Bcc.Select(x => x.Address).ToList())
            {
                contactsFound = rep.AsQueryable()
                    .Where(x => x.Person.EmailAddresses.Any(e => e.Address == emailAddress)).ToList();
                foreach (var contact in contactsFound)
                {
                    if (contact.Emails == null) contact.Emails = new ReferenceList<Email, EmailRef>();
                    contact.Emails.Add(emailRef);
                    contact.SaveToDatabase();
                    var task = Task.Run(() => PublishSignalREvents.PublishRefreshEmailForContact(contact.AsContactRef()));
                    task.Wait();
                }
            }

        }
    }
}