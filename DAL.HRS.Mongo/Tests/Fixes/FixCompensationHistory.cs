using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.Tests.Fixes
{
    [TestFixture]
    public class FixCompensationHistory
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void Execute()
        {
            var enc = DAL.Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;
            var empRep = new RepositoryBase<Employee>();
            var queryHelperEmployee = new MongoRawQueryHelper<Employee>();

            var filter = Employee.Helper.FilterBuilder.Where(x => x.Compensation.CompensationHistory.Any());
            var projection = Employee.Helper.ProjectionBuilder.Expression(x => new 
            { 
                x.Compensation.PayRateType,
                x.Compensation.PayRateAmount,
                x.Compensation.BaseHours,
                x.Compensation.CompensationHistory,
                x.Id
            });

            var employeesCache = Employee.Helper.FindWithProjection(filter, projection).ToList();



            foreach (var emp in employeesCache)
            {
                var isDirty = false;                

                foreach (var comp in emp.CompensationHistory)
                {
                    var salary = enc.Decrypt<decimal>(comp.AnnualSalary);
                    var annualized = enc.Decrypt<decimal>(comp.AnnualAdjustmentAmount);

                    if (emp.PayRateType == null)
                    {
                        break;
                    }

                    if (salary == 0 && emp.PayRateType.Code == "Hourly")
                    {
                        var rate = enc.Decrypt<decimal>(comp.PayRateAmount);
                        var newSalary = rate * emp.BaseHours * 52;
                        comp.AnnualSalary = enc.Encrypt(newSalary);

                        isDirty = true;
                    } 
                    else if (salary == 0 && emp.PayRateType.Code == "BiWeekly")
                    {
                        var rate = enc.Decrypt<decimal>(comp.PayRateAmount);
                        var newSalary = rate * 26;
                        comp.AnnualSalary = enc.Encrypt(newSalary);

                        isDirty = true;
                    }
                    else if (salary == 0 && emp.PayRateType.Code == "SemiMonthly")
                    {
                        var rate = enc.Decrypt<decimal>(comp.PayRateAmount);
                        var newSalary = rate * 24;
                        comp.AnnualSalary = enc.Encrypt(newSalary);

                        isDirty = true;
                    }
                    else if (salary == 0 && emp.PayRateType.Code == "Monthly")
                    {
                        var rate = enc.Decrypt<decimal>(comp.PayRateAmount);
                        var newSalary = rate * 12;
                        comp.AnnualSalary = enc.Encrypt(newSalary);

                        isDirty = true;
                    }

                    //else if (annualized == 0)
                    //{
                    //    for (int i = 0; i < emp.CompensationHistory.Count - 1; i++)
                    //    {
                    //        var thisComp = emp.CompensationHistory[i];
                    //        if ((i + 1) > (emp.CompensationHistory.Count - 1)) break;
                    //        var thisNewComp = enc.Decrypt<decimal>(thisComp.AnnualSalary);

                    //        var prevComp = emp.CompensationHistory[i + 1];
                    //        var prevNewComp = enc.Decrypt<decimal>(prevComp.AnnualSalary);
                    //        var newAnnual = thisNewComp - prevNewComp;

                    //        if (newAnnual > 0)
                    //        {
                    //            thisComp.AnnualAdjustmentAmount = enc.Encrypt(newAnnual);
                    //        }

                    //    }

                    //}
                }
               

                if (isDirty)
                {
                    var employee = queryHelperEmployee.FindById(emp.Id);
                    employee.Compensation.CompensationHistory = emp.CompensationHistory;

                    queryHelperEmployee.Upsert(employee);
                }
            }
        }
    }
}
