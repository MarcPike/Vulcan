using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using System;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.Tests.Prepare_Database
{
    public class AddProperties
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            //Location.GenerateDefaults();
            CreatePayrollRegion();
            //BuildDefaultRegionForEachEmployee();
        }

        [Test]
        public void BuildDefaultRegionForEachEmployee()
        {
            var rep = new RepositoryBase<Employee>();
            var defaultLocation = new RepositoryBase<Location>().AsQueryable().First(x=>x.Office == "Telge").AsLocationRef();
            foreach (var employee in rep.AsQueryable().ToList())
            {
                if (employee.Location == null)
                {
                    employee.Location = defaultLocation;
                }
                employee.PayrollRegion = employee.Location.AsLocation().PayrollRegions.First();
                //foreach (var workStatusHistory in employee.WorkStatusHistory.Where(x=>x.EffectiveDate == null))
                //{
                //    workStatusHistory.EffectiveDate = DateTime.Now;
                //}
                rep.Upsert(employee);
            }
        }

        [Test]
        public void SetAllPropertyValuesEntityToHowco()
        {
            var howco = Entity.GetRefByName("Howco");
            var queryHelper = new MongoRawQueryHelper<PropertyValue>();
            var filter = queryHelper.FilterBuilder.Empty;
            var update = queryHelper.UpdateBuilder.Set(x => x.Entity, howco);
            queryHelper.UpdateMany(filter, update);
        }

        [Test]
        public static void CreatePayrollRegion()
        {
            PayrollRegion.PopulateValues();
            var payrollRegions = new RepositoryBase<PayrollRegion>().AsQueryable().ToList();

            var repLocations = new RepositoryBase<Location>();
            var locations = repLocations.AsQueryable().ToList();

            foreach (var location in locations.Where(x => x.Country == "China").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("China")); 
                repLocations.Upsert(location);
            }


            foreach (var location in locations.Where(x => x.Region == "Europe").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Europe")); 
                repLocations.Upsert(location);
            }


            foreach (var location in locations.Where(x => x.Country == "Malaysia").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Malaysia")); 
                repLocations.Upsert(location);
            }


            foreach (var location in locations.Where(x => x.Country == "UAE").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Middle East")); 
                repLocations.Upsert(location);
            }


            foreach (var location in locations.Where(x => x.Country == "Singapore").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Singapore"));
                repLocations.Upsert(location);
            }

            foreach (var location in locations.Where(x => x.Region == "Western Hemisphere").ToList())
            {
                location.PayrollRegions.Add(GetPayrollRegionFor("Western Hemisphere")); 
                repLocations.Upsert(location);
            }

            locations = repLocations.AsQueryable().ToList();
            foreach (var location in locations.Where(x => x.PayrollRegions.Count == 0).ToList())
            {
                Console.WriteLine(location.Office);
            }

            PayrollRegionRef GetPayrollRegionFor(string name)
            {
                return payrollRegions.First(x=>x.Name == name).AsPayrollRegionRef();
            }
        }
    }
}
