using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Currency;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.DatabaseChanges
{
    /*
    [TestFixture()]
    public class ConvertLinksToReferenceList
    {
        [Test]
        public void UpdateManagers()
        {
            var rep = new RepositoryBase<Manager>();
            foreach (var manager in rep.AsQueryable().ToList())
            {
                var tasks = new LinkResolver<Mongo.DocClass.CRM.Task>(manager).GetAllLinkedDocuments();
                foreach (var task in tasks)
                {
                    manager.Tasks.AddReferenceObject(task.AsTaskRef());
                    manager.RemoveLink(task);
                    manager.SaveAccount();

                    task.Managers.AddReferenceObject(manager.AsManagerRef());
                    task.RemoveLink(manager);
                    task.SaveAccount();
                }
                var teams = new LinkResolver<Team>(manager).GetAllLinkedDocuments();
                foreach (var team in teams)
                {
                    manager.Teams.AddReferenceObject(team.AsTeamRef());
                    manager.RemoveLink(team);
                    manager.SaveAccount();

                    team.Managers.AddReferenceObject(manager.AsManagerRef());
                    team.RemoveLink(manager);
                    team.SaveAccount();
                }
            }
        }

        [Test]
        public void UpdateSalesPersons()
        {
            var rep = new RepositoryBase<SalesPerson>();
            foreach (var salesPerson in rep.AsQueryable().ToList())
            {
                var tasks = new LinkResolver<Mongo.DocClass.CRM.Task>(salesPerson).GetAllLinkedDocuments();
                foreach (var task in tasks)
                {
                    salesPerson.Tasks.AddReferenceObject(task.AsTaskRef());
                    salesPerson.RemoveLink(task);

                    task.SalesPersons.AddReferenceObject(salesPerson.AsSalesPersonRef());
                    task.RemoveLink(salesPerson);
                }

                var teams = new LinkResolver<Team>(salesPerson).GetAllLinkedDocuments();
                foreach (var team in teams)
                {
                    salesPerson.Teams.AddReferenceObject(team.AsTeamRef());
                    salesPerson.RemoveLink(team);
                    salesPerson.SaveAccount();

                    team.SalesPersons.AddReferenceObject(salesPerson.AsSalesPersonRef());
                    team.RemoveLink(salesPerson);
                    team.SaveAccount();
                }
            }



        }

    }
    */
}
