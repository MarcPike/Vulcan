using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Common.Ldap
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.Linq;

    namespace DAL.WindowsAuthentication.MongoDb
    {
        public class LdapReader
        {
            public class LdapUserLocation
            {
                public Guid ObjectGuid;
                public string NetworkId;
                public string UserName;
                public string Location;
                public string Email;
            }


            private readonly CommonMongoRawQueryHelper<LdapUser> _userHelper = new CommonMongoRawQueryHelper<LdapUser>();
            private readonly CommonMongoRawQueryHelper<Location> _locationHelper = new CommonMongoRawQueryHelper<Location>();

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

                search.PropertiesToLoad.Add("objectGuid");
                search.PropertiesToLoad.Add("sAMAccountName");
                search.PropertiesToLoad.Add("displayName");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("physicaldeliveryofficename");
                search.PropertiesToLoad.Add("givenName");
                search.PropertiesToLoad.Add("sn");

                SearchResultCollection results = search.FindAll();

                foreach (SearchResult searchResult in results)
                {
                    ResultPropertyCollection resultPropertyCollection = searchResult.Properties;


                    //if (resultPropertyCollection.PropertyNames == null) throw new Exception("Unable to find Properties");

                    var objectGuid = searchResult.Properties["objectGuid"][0];
                    var networkId = searchResult.Properties["sAMAccountName"][0].ToString();
                    var email = "";
                    var location = "";
                    var firstName = "";
                    var lastName = "";
                    try
                    {
                        email = searchResult.Properties["mail"][0].ToString();
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine($"No email found for user: {networkId}");
                        continue;
                    }

                    var userName = searchResult.Properties["displayName"][0].ToString();
                    

                    try
                    {
                        firstName = searchResult.Properties["givenName"][0].ToString();
                    }
                    catch
                    {
                        continue;
                    }

                    try
                    {
                        lastName = searchResult.Properties["sn"][0].ToString();
                    }
                    catch
                    {
                        continue;
                    }


                    //if (userName == "Saifee, Siti Suhana Binte")
                    //{
                    //    Console.WriteLine("Saifee, Siti Suhana Binte");
                    //}


                    try
                    {
                        location = searchResult.Properties["physicaldeliveryofficename"][0].ToString();
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine($"No location found for {networkId}");
                        continue;
                    }

                    if (objectGuid != null)
                    {
                        var userLocation = new LdapUserLocation()
                        {
                            ObjectGuid = new Guid(objectGuid as byte[]),
                            NetworkId = networkId,
                            Email = email,
                            UserName = userName,
                            Location = location,
                        };



                        if ((userLocation.NetworkId != null) && (userLocation.UserName != null) &&
                            (userLocation.Email != null) && userLocation.Location != null)
                        {
                            DoVerifyAndSaveUserAndLocation(userLocation, firstName, lastName);
                        }
                    }
                }
            }

            private void DoVerifyAndSaveUserAndLocation(LdapUserLocation userLocation, string firstName, string lastName)
            {
                //    var coidDict = new Dictionary<string, string>
                //{
                //    {"???", "(Unknown)"},
                //    {"DUB", "Dubai"},
                //    {"SPL", "Europe"},
                //    {"SIN", "Singapore"},
                //    {"CHI", "China"},
                //    {"CHN", "China"},
                //    {"CAN", "Canada"},
                //    {"INC", "United States"},
                //    {"MSA", "Malaysia"}
                //};

                var locationText = userLocation.Location;
                if (locationText == null) return;

                if (userLocation.Location == "cumbernauld") userLocation.Location = "Cumbernauld";
                if (userLocation.Location == "Gray (Houma)") userLocation.Location = "Gray";
                if (userLocation.Location == "LSC") userLocation.Location = "Lafayette";
                if ((userLocation.Location == "Emmott") || (userLocation.Location == "Emmot"))
                    userLocation.Location = "Emmott Road";
                if (userLocation.Location == "Houma") userLocation.Location = "Gray";
                if ((userLocation.Location == "Newbridge") || (userLocation.Location == "New Bridge"))
                    userLocation.Location = "EM - Newbridge";
                if ((userLocation.Location == "Brisbane") || (userLocation.Location == "Austria"))
                    userLocation.Location = "EM - Australia";
                if (userLocation.Location == "Paris") userLocation.Location = "EM - France";
                if (userLocation.Location == "Stockton") userLocation.Location = "EM - Stockton-on-Tees";
                if (userLocation.Location == "Bredburry") userLocation.Location = "Bredbury";
                if (userLocation.Location == "Newbury") userLocation.Location = "HSP - Newbury";

                if (userLocation.Location == "FZE") userLocation.Location = "EM - Middle East";




                var emailAddress = userLocation.Email;

                var filterUser = _userHelper.FilterBuilder.Where(x => x.NetworkId == userLocation.NetworkId);
                var user = _userHelper.Find(filterUser).SingleOrDefault();

                var location = ResolveLocation(userLocation);


                if (user == null)
                {
                    user = new LdapUser()
                    {
                        ActiveDirectoryId = userLocation.ObjectGuid.ToString(),
                        NetworkId = userLocation.NetworkId,
                        UserName = userLocation.UserName,
                        Location = location
                            .AsLocationRef(),
                        LocationText = userLocation.Location
                    };
                    user.ParseUserName();
                    var person = ResolvePerson(user, emailAddress);
                    user.Person = person;

                    _userHelper.Upsert(user);
                    //Console.WriteLine($"NEW USER: {user.FullName} was added");
                    
                }
                else
                {
                    
                    if ((string.IsNullOrEmpty(user.FirstName)) || (string.IsNullOrEmpty(user.LastName)))
                    {
                        user.ParseUserName();
                        _userHelper.Upsert(user);
                    }
                    if (emailAddress == null)
                    {
                        //Console.WriteLine($"User: {user.FullName} is missing Email Address");
                        return;
                    }

                    if (user.Person == null)
                    {
                        user.Person = new Person(user);
                        _userHelper.Upsert(user);
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
                            _userHelper.Upsert(user);
                        }

                    }

                    // Did they change locations
                    if (location.AsLocationRef().Id != user.Location.Id)
                    {
                        user.Location = location.AsLocationRef();
                        _userHelper.Upsert(user);
                        //Console.WriteLine($"User: {user.FullName} changed Location to {location.Office}");
                    }

                    if (user.UserName != userLocation.UserName)
                    {
                        user.UserName = userLocation.UserName;
                        user.LastName = lastName;
                        user.FirstName = firstName;
                        user.Person.LastName = lastName;
                        user.Person.FirstName = firstName;
                        _userHelper.Upsert(user);
                    }
                }

            }

            private static Person ResolvePerson(LdapUser user, string emailAddress)
            {
                var person = new Person(user);
                person.EmailAddresses.Add(new EmailAddress()
                {
                    Type = EmailType.Business,
                    Address = emailAddress,
                });
                return person;
            }

            private Location ResolveLocation(LdapUserLocation userLocation)
            {
                var filterLocation = _locationHelper.FilterBuilder.Where(x => x.Office == userLocation.Location);
                var location = _locationHelper.Find(filterLocation).SingleOrDefault();

                if (location == null)
                {
                    filterLocation = _locationHelper.FilterBuilder.Where(x => x.Country == userLocation.Location);
                    location = _locationHelper.Find(filterLocation).FirstOrDefault();
                }

                if (location == null && userLocation.Location == "cumbernauld")
                {
                    filterLocation = _locationHelper.FilterBuilder.Where(x => x.Office == "Cumbernauld");
                    location = _locationHelper.Find(filterLocation).FirstOrDefault();
                }

                if (location == null)
                {
                    filterLocation = _locationHelper.FilterBuilder.Where(x => x.Office == "<unknown>");
                    location = _locationHelper.Find(filterLocation).First();

                    //Console.WriteLine($"User Location Not found: {userLocation.Location} Defaulting to <unknown>");
                    BadLocations.Add(userLocation);
                    //return;
                }

                return location;
            }
        }
    }
}
