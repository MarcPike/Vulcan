using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using HrsContext = DAL.HRS.SqlServer.Model.HrsContext;
using KronosPayRuleHistory = DAL.HRS.Mongo.DocClass.Compensation.KronosPayRuleHistory;

namespace DAL.HRS.Import.ImportHrs
{
    [TestFixture()]
    public class KronosOnlyUpdate
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var rep = new RepositoryBase<DAL.HRS.Mongo.DocClass.Employee.Employee>();
            var employees = rep.AsQueryable().ToList();
            foreach (var employee in employees)
            {
                AddKronosPayRulesForEmployee(employee, rep);
            }
        }

        public void AddKronosPayRulesForEmployee(DAL.HRS.Mongo.DocClass.Employee.Employee emp, RepositoryBase<Employee> rep)
        {
            using (var context = new HrsContext())
            {
                try
                {
                    var employeeHrs = context.Employee.Include("Compensation").SingleOrDefault(x => x.OID == emp.OldHrsId);
                    var comp = employeeHrs?.Compensation?.ToList();
                    if (comp == null) return;
                    foreach (var compensation in comp)
                    {
                        var payRules = compensation?.KronosPayRuleHistory.FirstOrDefault();
                        if (payRules != null)
                        {
                            emp.Compensation.KronosPayRuleEffectiveDate = payRules.EffectiveDate;
                            emp.Compensation.KronosPayRuleType = PropertyBuilder.CreatePropertyValue("KronosPayRuleType",
                                    "Kronos Pay Rule Type", payRules.KronosPayRule1?.Name ?? "(empty)", "")
                                .AsPropertyValueRef();
                        }
                        rep.Upsert(emp);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }
    }
}
