using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Analysis;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Models;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Builders
{
    [TestFixture]
    public class ProductWinLossAnalysisBuilderTest
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void Execute()
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (var team in Team.Helper.GetAll())
            {
                ProductWinLossAnalysisBuilder.AddTeamQuotes(team);
            }
            sw.Stop();
            Console.WriteLine($"{ProductWinLossData.Helper.GetRowCount()} rows Elapsed: {sw.Elapsed}" );
        }

        //[Test]
        //public void DumpAllAnalysisModels()
        //{
        //    var allProducts = ProductWinLossData.Helper.GetAll().Where(x=>x.History.Count >20).Take(10);
        //    foreach (var productWinLossAnalysis in allProducts)
        //    {
        //        var model = new ProductWinLossAnalysisModel(productWinLossAnalysis,"USD");
        //        Console.WriteLine(ObjectDumper.Dump(model));
        //    }
        //}

    }
}
