using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.TargetPercentageFix
{
    [TestFixture]
    public class FixTargetPercentages
    {

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void ConvertToPercentages()
        {
            var rep = new RepositoryBase<TargetPercentage>();
            var targetPercentage = rep.AsQueryable().FirstOrDefault();

            Assert.IsNotNull(targetPercentage);
            var index = 0;
            var newValues = new List<decimal>();
            foreach (var targetPercentageValue in targetPercentage.Values)
            {
                if (targetPercentageValue > 0)
                {
                    newValues.Add(targetPercentageValue / 100); 
                }
                else
                {
                    newValues.Add(targetPercentageValue);
                }

                index++;
            }

            targetPercentage.Values = newValues;
            rep.Upsert(targetPercentage);


        }
    }
}
