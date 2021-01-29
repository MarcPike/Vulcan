using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Test;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.MachinedParts
{
    [TestFixture]
    public class BasicTesting
    {
        [Test]
        public void CreateMachinedPartCostPriceValue()
        {
            var value = new MachinedPartCostPriceValue("CAN", 4331058, 1, "USD");

            Console.WriteLine(ObjectDumper.Dump(value));
        }

        [Test]
        public void CreateQuoteMachinedPartModel()
        {
            var value = new MachinedPartCostPriceValue("CAN", 4331058, 1, "USD");
            var model = new QuoteMachinedPartModel("vulcancrm", "599b1573b508d62d0c75a115",value);
            Console.WriteLine(ObjectDumper.Dump(model));
        }

        [Test]
        public void ChangeMarginOverride()
        {
            var value = new MachinedPartCostPriceValue("CAN", 4331058, 1, "USD");
            var model = new QuoteMachinedPartModel("vulcancrm", "599b1573b508d62d0c75a115", value);


            model.MarginOverride = 20;
            model = QuoteMachinedPartModel.Calculate(model);

            model.PiecePriceOverride = 9000;
            model = QuoteMachinedPartModel.Calculate(model);

            model.Pieces = 3;
            model = QuoteMachinedPartModel.Calculate(model);
            //model.DisplayCurrency = "CAD";

            //Console.WriteLine(ObjectDumper.Dump(model));

        }

        [Test]
        public void GetMachinedPartTest()
        {
            var productMasterResult = ProductMaster.FromStockId("INC", 7288516);
        }

    }


}
