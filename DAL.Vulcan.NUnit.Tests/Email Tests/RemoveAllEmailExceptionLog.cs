using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Email;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace DAL.Vulcan.NUnit.Tests.Email_Tests
{
    [TestFixture]
    public class RemoveAllEmailExceptionLog
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            var minDate = DateTime.Parse("2/1/2019");
            var maxDate = DateTime.Now.Date;
            var onDate = minDate;
            while (onDate <= maxDate)
            {
                var onDateEndOfDay = onDate.AddHours(23).AddMinutes(59);
                var filter =
                    EmailExceptionLog.Helper.FilterBuilder.Where(x => x.DateOf >= onDate && x.DateOf <= onDateEndOfDay);
                EmailExceptionLog.Helper.DeleteMany(filter);
            }
        }
    }
}
