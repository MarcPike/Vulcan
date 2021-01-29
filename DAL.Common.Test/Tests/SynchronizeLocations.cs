using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;
using Location = DAL.Common.DocClass.Location;

namespace DAL.Common.Test.Tests
{
    /*
    [TestFixture]
    public class SynchronizeLocations
    {
        [Test]
        public void SynchronizeHrsLocations()
        {
            EnvironmentSettings.HrsDevelopment();
            var hrsQueryHelper = new MongoRawQueryHelper<DAL.HRS.Mongo.DocClass.Locations.Location>();
            var hrsLocations = hrsQueryHelper.GetAll();

            var commonLocations = new MongoRawQueryHelper<Location>().GetAll();

            foreach (var location in commonLocations)
            {
                if (hrsLocations.All(x => x.Office != location.Office))
                {
                    var newLocation = new DAL.HRS.Mongo.DocClass.Locations.Location
                    {
                        Id = location.Id
                    };
                    foreach (var locationAddress in location.Addresses)
                    {
                        DAL.HRS.Mongo.DocClass.Locations.AddressType addressType;
                        Enum.TryParse(locationAddress.Type.ToString(), out addressType);
                        newLocation.Addresses.Add(new DAL.HRS.Mongo.DocClass.Locations.Address()
                        {
                            AddressLine1 = locationAddress.AddressLine1,
                            AddressLine2 = locationAddress.AddressLine2,
                            City = locationAddress.City,
                            Country = locationAddress.Country,
                            County = locationAddress.County,
                            Name = locationAddress.Name,
                            PostalCode = locationAddress.PostalCode,
                            StateProvince = locationAddress.StateProvince,
                            Type = addressType,
                        });
                    }

                    newLocation.Office = location.Office;
                    newLocation.Branch = location.Branch;
                    newLocation.Country = location.Country;
                    newLocation.Phone = location.Phone;
                    newLocation.Fax = location.Fax;
                    newLocation.PhoneTollFree = location.PhoneTollFree;
                    if (location.MapLocation != null)
                    {
                        newLocation.MapLocation = new DAL.HRS.Mongo.DocClass.Locations.MapLocation(location.MapLocation.Coordinates.X, location.MapLocation.Coordinates.Y);
                    }
                    newLocation.Region = location.Region;
                    //newLocation.PayrollRegions = PayrollRegion. location.PayrollRegions


                    Console.WriteLine(ObjectDumper.Dump(newLocation));
                }
            }

        }
    }*/
}
