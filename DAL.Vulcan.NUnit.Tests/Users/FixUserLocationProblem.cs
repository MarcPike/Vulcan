using System.Diagnostics;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Locations;
using MongoDB.Bson;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Users
{
    [TestFixture]
    public class FixUserLocationProblem
    {
        [Test]
        public void DoIt()
        {
            var repUser = new RepositoryBase<LdapUser>();
            var repLocation = new RepositoryBase<Location>();

            foreach (var user in repUser.AsQueryable().Where(x=>x.Location != null).ToList())
            {
                var location = repLocation.AsQueryable().SingleOrDefault(x => x.Id.ToString() == user.Location.Id);
                if (location == null)
                {
                    Trace.WriteLine($"{user.FirstName} {user.LastName} has invalid location");

                    var userLocation = user.Location.AsLocation();

                    location = repLocation.AsQueryable()
                        .SingleOrDefault(x => x.Region == userLocation.Region && x.Office == userLocation.Office &&
                                              x.Country == userLocation.Country);
                    Assert.IsNotNull(location);
                    user.Location = location.AsLocationRef();
                    repUser.Upsert(user);
                }
            }

        }
    }
}
