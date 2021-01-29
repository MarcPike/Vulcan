using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Fixes
{
    [TestFixture]
    public class FixInvalidPayrollRegionRefs
    {
        private MongoRawQueryHelper<PayrollRegion> Helper;
        private PayrollRegionRef CurrentRegion;
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
            Helper = new MongoRawQueryHelper<PayrollRegion>();
        }

        private List<PayrollRegionRef> GetPayrollRegionReferences()
        {
            var filter = Helper.FilterBuilder.Empty;
            return Helper.Find(filter).Select(x => x.AsPayrollRegionRef()).ToList();
        }

        [Test]
        public void CheckEachPayrollRegion()
        {
            var payrollRegions = GetPayrollRegionReferences();
            foreach (var region in payrollRegions)
            {
                CurrentRegion = region;
                //FixLocations();
                //FixEmployees();
                //FixHrsSecurity();
            }
        }
        public void FixLocations()
        {
            var filter = Location.Helper.FilterBuilder.Where(x => x.PayrollRegions.Any(r => r.Id == CurrentRegion.Id));
            var locations = Location.Helper.Find(filter).ToList();
            foreach (var location in locations)
            {
                var changesMade = false;
                foreach (var locationPayrollRegion in location.PayrollRegions)
                {
                    if (locationPayrollRegion.Id == CurrentRegion.Id)
                    {
                        if (locationPayrollRegion.Name != CurrentRegion.Name)
                        {
                            locationPayrollRegion.Name = CurrentRegion.Name;
                            changesMade = true;
                        }
                    }
                }

                if (changesMade)
                {
                    Location.Helper.Upsert(location);
                }
            }
        }

        private void FixEmployees()
        {
            var filter = Employee.Helper.FilterBuilder.Where(x =>
                x.PayrollRegion.Id == CurrentRegion.Id && x.PayrollRegion.Name != CurrentRegion.Name);
            var employees = Employee.Helper.Find(filter).ToList();
            foreach (var employee in employees)
            {
                Console.WriteLine($"PayrollRegion: [{employee.PayrollRegion.Name}] corrected to [{CurrentRegion.Name}] for {employee.FirstName} {employee.LastName}");
                employee.PayrollRegion = CurrentRegion;
                Employee.Helper.Upsert(employee);
            }

        }

        private void FixHrsSecurity()
        {
            var filter = HrsUser.Helper.FilterBuilder.Where(x =>
                x.HrsSecurity.PayrollRegionsForCompensation.Any(r => r.Id == CurrentRegion.Id));
            var hrsUserCompensationPayrollRegions = HrsUser.Helper.Find(filter);
            foreach (var hrsUser in hrsUserCompensationPayrollRegions)
            {
                var changesMade = false;
                foreach (var payrollRegionRef in hrsUser.HrsSecurity.PayrollRegionsForCompensation.Where(x=>x.Id == CurrentRegion.Id))
                {
                    if (payrollRegionRef.Name != CurrentRegion.Name)
                    {
                        payrollRegionRef.Name = CurrentRegion.Name;
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
