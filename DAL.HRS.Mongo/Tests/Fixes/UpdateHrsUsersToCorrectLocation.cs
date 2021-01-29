using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Fixes
{
    [TestFixture]
    public class UpdateHrsUsersToCorrectLocation
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void SearchForLocationIdAndCorrectReferences()
        {
            var users = HrsUser.Helper.GetAll();
            foreach (var hrsUser in users)
            {
                //if ((hrsUser.LastName != "Martin") && (hrsUser.FirstName != "Lori")) continue;

                var changesMade = false;
                var location = Location.Helper.FindById(hrsUser.Location.Id);
                var ldapUser = LdapUser.Helper.FindById(hrsUser.UserId);
                if (ldapUser.Location.Id != location.Id.ToString())
                {
                    Console.WriteLine("We have serious problems");
                    Assert.AreEqual(1,0);
                }


                if (hrsUser.Location.Office != location.Office)
                {
                    hrsUser.Location = location.AsLocationRef();
                    changesMade = true;
                }

                if (hrsUser.User.Location.Office != location.Office)
                {
                    hrsUser.User.Location = location.AsLocationRef();
                    changesMade = true;
                }

                if (hrsUser.Employee.Location.Office != location.Office)
                {
                    hrsUser.Employee.Location = location.AsLocationRef();
                    changesMade = true;
                }

                foreach (var currentLocation in hrsUser.HrsSecurity.Locations.ToList())
                {
                    var actualLocation = Location.Helper.FindById(currentLocation.Id);

                    if (currentLocation.Office != actualLocation.Office)
                    {
                        currentLocation.Office = actualLocation.Office;
                        changesMade = true;
                    }
                }

                foreach (var currentLocation in hrsUser.HseSecurity.Locations.ToList())
                {
                    var actualLocation = Location.Helper.FindById(currentLocation.Id);

                    if (currentLocation.Office != actualLocation.Office)
                    {
                        currentLocation.Office = actualLocation.Office;
                        changesMade = true;
                    }
                }


                foreach (var hrsSecurityMedicalLocation in hrsUser.HrsSecurity.MedicalLocations.ToList())
                {
                    var actualMedicalLocation = Location.Helper.FindById(hrsSecurityMedicalLocation.Id);

                    if (hrsSecurityMedicalLocation.Office != actualMedicalLocation.Office)
                    {
                        hrsSecurityMedicalLocation.Office = actualMedicalLocation.Office;
                        changesMade = true;
                    }
                }

                foreach (var hseSecurityMedicalLocation in hrsUser.HseSecurity.MedicalLocations.ToList())
                {
                    var actualMedicalLocation = Location.Helper.FindById(hseSecurityMedicalLocation.Id);

                    if (hseSecurityMedicalLocation.Office != actualMedicalLocation.Office)
                    {
                        hseSecurityMedicalLocation.Office = actualMedicalLocation.Office;
                        changesMade = true;
                    }
                }


                if (changesMade)
                {
                    HrsUser.Helper.Upsert(hrsUser);
                }

            }
        }
    }
}
