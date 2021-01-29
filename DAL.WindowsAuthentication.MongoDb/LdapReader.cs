using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Locations;

namespace DAL.WindowsAuthentication.MongoDb
{
    public partial class LdapReader
    {
        private readonly RepositoryBase<LdapUser> _userRepository = new RepositoryBase<LdapUser>();
        private readonly RepositoryBase<Location> _locationRepository = new RepositoryBase<Location>();

        public List<LdapUserLocation> BadLocations = new List<LdapUserLocation>();

        public void RefreshUserListFromLdap()
        {

            //LdapUserLocation userLocation;

            //DirectoryEntry rootEntry = new DirectoryEntry("LDAP://s-gl-dc01.howcogroup.com/OU=Howco,DC=howcogroup,DC=com");
            DirectoryEntry rootEntry = new DirectoryEntry("LDAP://OU=Howco,DC=howcogroup,DC=com");
            DirectorySearcher search = new DirectorySearcher(rootEntry)
            {
                Filter = "(&(objectClass=user)(objectCategory=Person))",
                SearchScope = SearchScope.Subtree
            };

            search.PropertiesToLoad.Add("sAMAccountName");
            search.PropertiesToLoad.Add("displayName");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("physicaldeliveryofficename");

            SearchResultCollection results = search.FindAll();

            foreach (SearchResult searchResult in results)
            {
                ResultPropertyCollection resultPropertyCollection = searchResult.Properties;


                //if (resultPropertyCollection.PropertyNames == null) throw new Exception("Unable to find Properties");

                var networkId = searchResult.Properties["sAMAccountName"][0].ToString();
                if (networkId.ToLower() == "crozpoc1")
                {
                    //var m = "";
                }
                var email = "";
                var location = "";
                try
                {
                    email = searchResult.Properties["mail"][0].ToString();
                }
                catch (Exception)
                {
                    Console.WriteLine($"No email found for user: {networkId}");
                    continue;
                }
                var userName = searchResult.Properties["displayName"][0].ToString();
                try
                {
                    location = searchResult.Properties["physicaldeliveryofficename"][0].ToString();
                }
                catch (Exception)
                {
                    Console.WriteLine($"No location found for {networkId}");
                    continue;
                }

                var userLocation = new LdapUserLocation()
                {
                    NetworkId = networkId,
                    Email = email,
                    UserName = userName,
                    Location = location,
                };


                if ((userLocation.NetworkId != null) && (userLocation.UserName != null) &&
                    (userLocation.Email != null) && userLocation.Location != null)
                {
                    DoVerifyAndSaveUserAndLocation(userLocation);
                }

            }
        }

        private void DoVerifyAndSaveUserAndLocation(LdapUserLocation userLocation)
        {
            var coidDict = new Dictionary<string, string>
            {
                {"???", "(Unknown)"},
                {"DUB", "Dubai"},
                {"SPL", "Europe"},
                {"SIN", "Singapore"},
                {"CHI", "China"},
                {"CHN", "China"},
                {"CAN", "Canada"},
                {"INC", "United States"},
                {"MSA", "Malaysia"}
            };

            if (userLocation.Location == "cumbernauld") userLocation.Location = "Cumbernauld";
            if (userLocation.Location == "Gray (Houma)") userLocation.Location = "Gray";
            if (userLocation.Location == "LSC") userLocation.Location = "Lafayette";


            var emailAddress = userLocation.Email;

            var user = _userRepository.AsQueryable().SingleOrDefault(x => x.NetworkId == userLocation.NetworkId);

            var location = _locationRepository.AsQueryable().SingleOrDefault(x => x.Office == userLocation.Location);

            if (location == null)
            {
                location = _locationRepository.AsQueryable().FirstOrDefault(x => x.Country == userLocation.Location);
            }

            if (location == null && userLocation.Location == "cumbernauld")
            {
                location = _locationRepository.AsQueryable().SingleOrDefault(x => x.Office == "Cumbernauld");
            }

            if (location == null && userLocation.Location == "cumbernauld")
            {
                location = _locationRepository.AsQueryable().SingleOrDefault(x => x.Office == "Cumbernauld");
            }

            if (location == null)
            {
                
                BadLocations.Add(userLocation);
                return;
            }


            if (user == null)
            {
                var locationFound = _locationRepository.AsQueryable()
                    .FirstOrDefault(x => x.Office == userLocation.Location || x.Country == userLocation.Location);
                if (locationFound != null)
                {
                    user = new LdapUser()
                    {
                        NetworkId = userLocation.NetworkId,
                        UserName = userLocation.UserName,
                        Location = locationFound
                            .AsLocationRef()
                    };
                    user.Person = new Person(user);

                    user.Person.EmailAddresses.Add(new EmailAddress()
                    {
                        Type = EmailType.Business,
                        Address = emailAddress,
                    });
                    _userRepository.Upsert(user);
                    Console.WriteLine($"NEW USER: {user.FullName} was added");
                }
            }
            else
            {
                if (emailAddress == null)
                {
                    Console.WriteLine($"User: {user.FullName} is missing Email Address");
                    return;
                }

                if (user.Person == null)
                {
                    user.Person = new Person(user);
                }

                if ((user.Person.EmailAddresses == null) ||
                    (user.Person.EmailAddresses.All(x => x.Type != EmailType.Business)))
                {
                    var businessEmail = user.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business);
                    if (businessEmail == null)
                    {
                        businessEmail = new EmailAddress()
                        {
                            Type = EmailType.Business,
                            Address = emailAddress
                        };

                        user.Person.EmailAddresses.Add(businessEmail);
                        user.SaveToDatabase();
                    }

                }

                // Did they change locations
                if (location.AsLocationRef().Id != user.Location.Id)
                {
                    user.Location = location.AsLocationRef();
                    user.SaveToDatabase();
                    Console.WriteLine($"User: {user.FullName} changed Location to {location.Office}");
                }
            }

        }
    }
}
