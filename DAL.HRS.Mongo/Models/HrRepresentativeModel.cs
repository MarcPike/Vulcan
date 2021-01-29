using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Models
{
    public class HrRepresentativeModel
    {
        public LocationRef Location { get; set; }
        public EmployeeRef Representative { get; set; }

        public HrRepresentativeModel()
        {

        }
        public HrRepresentativeModel(HrRepresentative rep)
        {
            Location = rep.Location;
            Representative = rep.Representative;
        }

        public HrRepresentativeModel(LocationRef location)
        {
            var rep = HrRepresentative.Helper.Find(x => x.Location.Id == location.Id).FirstOrDefault();
            if (rep == null)
            {
                Location = location;
            }
            else
            {
                Location = location;
                Representative = rep.Representative;
            }

        }

        public static List<HrRepresentativeModel> GetForAllLocations()
        {
            var result = new List<HrRepresentativeModel>();
            foreach (var location in Common.DocClass.Location.Helper.GetAll())
            {
                result.Add( new HrRepresentativeModel(location.AsLocationRef()));
            }

            return result;
        }

        [TestFixture]
        private class Test
        {
            [SetUp]
            public void SetUp()
            {
                EnvironmentSettings.HrsDevelopment();
            }

            [Test]
            public void Execute()
            {
                var models = HrRepresentativeModel.GetForAllLocations();
                foreach (var locationHrRepresentativeModel in models)
                {
                    Console.WriteLine($"{locationHrRepresentativeModel.Location.Office} - Rep: {locationHrRepresentativeModel.Representative?.FullName}");
                }
            }
        }


    }

}
