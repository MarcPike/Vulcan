using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Test;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.OemList
{
    [TestFixture]
    public class OemUpdateTester
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void GetAll()
        {
            var oemUpdateModels = OemUpdateModel.GetAll("vulcancrm", "599b1573b508d62d0c75a115");
            foreach (var oemUpdateModel in oemUpdateModels)
            {
                Console.WriteLine(ObjectDumper.Dump(oemUpdateModel));
            }
        }

        [Test]
        public void PreCheck()
        {
            var oemUpdateModels = OemUpdateModel.GetAll("vulcancrm", "599b1573b508d62d0c75a115");

            var test = oemUpdateModels.FirstOrDefault(x => x.OldValue == "BASINTEK");
            if (test != null)
            {
                test.NewValue = "Abaco Drilling";
                var results = OemUpdateModel.PreCheck(test);

                Console.WriteLine($"Rows: {results.RowsAffected} Removal: {results.OemTypeWillBeRemoved}");
            }

        }

        [Test]
        public void Execute()
        {
            var oemUpdateModels = OemUpdateModel.GetAll("vulcancrm", "599b1573b508d62d0c75a115");

            var test = oemUpdateModels.FirstOrDefault(x => x.OldValue == "BASINTEK");
            if (test != null)
            {
                test.NewValue = "Abaco Drilling";
                var newList = OemUpdateModel.Execute(test);
                foreach (var oemUpdateModel in newList.OrderBy(x=>x.OldValue).ToList())
                {
                    Console.WriteLine(ObjectDumper.Dump(oemUpdateModel));
                }
            }

        }

        [Test]
        public void RevertBack()
        {
            var oemUpdateModels = OemUpdateModel.GetAll("vulcancrm", "599b1573b508d62d0c75a115");

            var test = oemUpdateModels.FirstOrDefault(x => x.OldValue == "Abaco Drilling");
            if (test != null)
            {
                test.NewValue = "BASINTEK";
                var newList = OemUpdateModel.Execute(test);
                foreach (var oemUpdateModel in newList.OrderBy(x => x.OldValue).ToList())
                {
                    Console.WriteLine(ObjectDumper.Dump(oemUpdateModel));
                }
            }

        }

    }

}
