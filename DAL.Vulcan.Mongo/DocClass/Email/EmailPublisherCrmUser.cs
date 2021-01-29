using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.PublishSignalR;
using MongoDB.Driver.Linq;
using Task = System.Threading.Tasks.Task;

namespace DAL.Vulcan.Mongo.DocClass.Email
{
    public static class EmailPublisherCrmUser
    {
        public static void Execute(Email email)
        {
            var emailRef = email.AsEmailRef();
            AddToUsersIfFound(email, emailRef);
            
        }

        private static void AddToUsersIfFound(Email email, EmailRef emailRef)
        {
            var rep = new RepositoryBase<CrmUser>();
            var usersFound = rep.AsQueryable()
                .Where(x => x.User.UserName == email.From.Name).ToList();
            foreach (var userFound in usersFound)
            {
                if (userFound.Emails == null) userFound.Emails = new ReferenceList<Email, EmailRef>();
                if (userFound.Emails.All(x => x.Id != emailRef.Id))
                {
                    userFound.Emails.Add(emailRef);
                    userFound.SaveToDatabase();
                    var task = Task.Run(() =>
                        PublishSignalREvents.SendRefreshEmailToUser(userFound.AsCrmUserRef()));
                    task.Wait();
                }

            }
            foreach (var userName in email.To.Select(x => x.Name).ToList())
            {
                usersFound = rep.AsQueryable()
                    .Where(x => x.User.UserName == userName).ToList();
                foreach (var userFound in usersFound)
                {
                    if (userFound.Emails == null) userFound.Emails = new ReferenceList<Email, EmailRef>();
                    if (userFound.Emails.All(x => x.Id != emailRef.Id))
                    {
                        userFound.Emails.Add(emailRef);
                        userFound.SaveToDatabase();
                        var task = Task.Run(() =>
                            PublishSignalREvents.SendRefreshEmailToUser(userFound.AsCrmUserRef()));
                        task.Wait();
                    }
                }
            }
            foreach (var userName in email.Cc.Select(x => x.Name).ToList())
            {
                usersFound = rep.AsQueryable()
                    .Where(x => x.User.UserName == userName).ToList();
                foreach (var userFound in usersFound)
                {
                    if (userFound.Emails == null) userFound.Emails = new ReferenceList<Email, EmailRef>();
                    if (userFound.Emails.All(x => x.Id != emailRef.Id))
                    {
                        userFound.Emails.Add(emailRef);
                        userFound.SaveToDatabase();
                        var task = Task.Run(() =>
                            PublishSignalREvents.SendRefreshEmailToUser(userFound.AsCrmUserRef()));
                        task.Wait();
                    }
                }
            }
            foreach (var userName in email.Bcc.Select(x => x.Name).ToList())
            {
                usersFound = rep.AsQueryable()
                    .Where(x => x.User.UserName == userName).ToList();
                foreach (var userFound in usersFound)
                {
                    if (userFound.Emails == null) userFound.Emails = new ReferenceList<Email, EmailRef>();
                    if (userFound.Emails.All(x => x.Id != emailRef.Id))
                    {
                        userFound.Emails.Add(emailRef);
                        userFound.SaveToDatabase();
                        var task = Task.Run(() =>
                            PublishSignalREvents.SendRefreshEmailToUser(userFound.AsCrmUserRef()));
                        task.Wait();
                    }
                }
            }

        }

    }
}