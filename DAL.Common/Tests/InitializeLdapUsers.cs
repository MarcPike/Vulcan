using DAL.Common.DocClass;
using DAL.Common.Ldap.DAL.WindowsAuthentication.MongoDb;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Common.Tests
{
    [TestFixture]
    public class InitializeLdapUsers
    {
        [Test]
        public void Execute()
        {
            var reader = new LdapReader();
            reader.RefreshUserListFromLdap();
        }

        [Test]
        public void ParseAllNameFields()
        {
            var queryHelper = new MongoRawQueryHelper<LdapUser>();
            foreach (var ldapUser in queryHelper.GetAll())
            {
                ldapUser.ParseUserName();
                queryHelper.Upsert(ldapUser);
            }
        }

        [Test]
        public void ListUnknownLocations()
        {
            var queryHelper = new CommonMongoRawQueryHelper<LdapUser>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Location.Office == "<unknown>");
            var project = queryHelper.ProjectionBuilder.Expression(x => x.LocationText);
            var missingLocations = new List<string>();
            foreach (var missingLocation in queryHelper.FindWithProjection(filter,project).ToList())
            {
                if (missingLocations.All(x=> x != missingLocation)) missingLocations.Add(missingLocation);
            }

            foreach (var missingLocation in missingLocations)
            {
                Console.WriteLine(missingLocation);
            }
        }

        [Test]
        public void ListValidLocations()
        {
            var queryHelper = new MongoRawQueryHelper<Location>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Office != "<unknown>");
            var project = queryHelper.ProjectionBuilder.Expression(x => x.Office);
            foreach (var location in queryHelper.FindWithProjection(filter, project).ToList().OrderBy(x=>x ))
            {
                Console.WriteLine(location);
            }
            
        }


        [Test]
        public void ListAllEdgenMurrayUsers()
        {
            var queryHelper = new CommonMongoRawQueryHelper<LdapUser>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Location.Entity.Name == "Edgen Murray");
            var project = queryHelper.ProjectionBuilder.Expression(x => new { x.ActiveDirectoryId, x.Location.Office});
            var allUsers = queryHelper.FindWithProjection(filter, project).ToList();
            Dictionary<string, int> totals = new Dictionary<string, int>();
            foreach (var office in allUsers)
            {
                if (totals.ContainsKey(office.Office))
                {
                    totals[office.Office]++;
                }
                else
                {
                    totals.Add(office.Office, 1);
                }
            }

            var totalEmployees = 0;
            foreach (var total in totals)
            {
                totalEmployees += total.Value;
                Console.WriteLine($"Office [{total.Key}]: {total.Value}");
            }

            filter = queryHelper.FilterBuilder.Where(x => x.LocationText == "Home Office");
            var homeOfficeUsers = queryHelper.Find(filter);

            Console.WriteLine("Total Employees: "+ totalEmployees);
            Console.WriteLine("Home Office Employees: " + homeOfficeUsers.Count);
            totalEmployees += homeOfficeUsers.Count;
            Console.WriteLine("Total = "+totalEmployees );
        }

        [Test]
        public void EdgenMurrayLocations()
        {
            var queryHelper = new MongoRawQueryHelper<Location>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Entity.Name == "Edgen Murray");
            var project = queryHelper.ProjectionBuilder.Expression(x => x.Office);
            var sort = queryHelper.SortBuilder.Ascending(x => x.Office);

            var offices = queryHelper.FindWithProjection(filter, project, sort).ToList();
            foreach (var office in offices)
            {
                Console.WriteLine(office);
            }
        }

    }

    
}
