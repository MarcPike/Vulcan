using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.CompensationUpload
{
    [TestFixture]
    public class ComnpensationUploadExample
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void RunTest()
        {
            var enc = Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;
            var payrollId = "001026";
            var myComp = Employee.Helper.Find(x=>x.PayrollId == payrollId).First().Compensation;
            var payRateAmount = enc.Decrypt<decimal>(myComp.PayRateAmount);
            payRateAmount += 5;
            var effectiveDate = DateTime.Now;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            LoadCompensationValues(payrollId, payRateAmount,myComp.PayRateType, myComp.PayFrequencyType, effectiveDate, myComp.CurrencyType, myComp.IncreaseType, myComp.BaseHours);
            sw.Stop();
            Console.WriteLine("Time to update one: "+sw.Elapsed);
        }


        public void LoadCompensationValues(
            string payrollId,
            decimal payRateAmount,
            PropertyValueRef payRateType,
            PropertyValueRef payFrequencyType,
            DateTime effectiveDate,
            PropertyValueRef currencyType,
            PropertyValueRef increaseType,
            decimal baseHours)
        {
            var emp = Employee.Helper.Find(x => x.PayrollId == payrollId).FirstOrDefault();
            if (emp == null) throw new Exception("Employee not found");

            var comp = emp.Compensation;
            var enc = Vulcan.Mongo.Base.Encryption.Encryption.NewEncryption;

            comp.PayRateAmount = enc.Encrypt(payRateAmount);
            comp.PayRateType = payRateType;
            comp.PayFrequencyType = payFrequencyType;
            comp.EffectiveDate = enc.Encrypt(effectiveDate);
            comp.CurrencyType = currencyType;
            comp.IncreaseType = increaseType;
            comp.BaseHours = baseHours;

            Employee.Helper.Upsert(emp);
        }

    }
}
