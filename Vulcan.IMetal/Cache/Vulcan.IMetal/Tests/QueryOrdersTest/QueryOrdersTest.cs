using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Devart.Data.Linq.Monitoring;
using NUnit.Framework;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Queries.Orders;

namespace Vulcan.IMetal.Tests.QueryOrdersTest
{
    [TestFixture]
    public class QueryOrdersTest
    {
        private readonly LinqMonitor _monitor = new LinqMonitor();

        [Test]
        public void BasicTest()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var coid = "INC";

            using (var context = ContextFactory.GetOrdersContextForCoid(coid))
            {
                context.Connection.Open();
                var companyCodes = new List<string>();

                var query  = OrdersAdvancedQuery.AsQueryable(coid);

                companyCodes.Add("00113");
                companyCodes.Add("02247");
                companyCodes.Add("00167");
                companyCodes.Add("02760");
                companyCodes.Add("00015");
                companyCodes.Add("02298");
                companyCodes.Add("00710");
                companyCodes.Add("04178");
                companyCodes.Add("02000");
                companyCodes.Add("00016");
                companyCodes.Add("01834");
                companyCodes.Add("00092");
                companyCodes.Add("03011");
                companyCodes.Add("02729");
                companyCodes.Add("02575");
                companyCodes.Add("04000");
                companyCodes.Add("00011");
                companyCodes.Add("00009");
                companyCodes.Add("00005");
                companyCodes.Add("00213");
                companyCodes.Add("01642");
                companyCodes.Add("02588");

                query = query.Where(x => companyCodes.Contains(x.CompanyCode));
                query = query.Where(x => x.OrderSaleTypeCode == "ORD");
                //queryOrder.OrderTypeCode.Equals("ORD");

                query = query.Where(x=>x.OrderSaleDate >= DateTime.Now.AddDays(-14));


                stopwatch.Stop();
                Console.WriteLine($"Query construction time: {stopwatch.Elapsed}");
                stopwatch.Start();
                _monitor.IsActive = true;
                var result = query.ToList();
                _monitor.IsActive = false;
                stopwatch.Stop();
                Console.WriteLine($"Query execution time: {stopwatch.Elapsed} | {result.Count} row(s) found");
                stopwatch.Start();
                foreach (var rowFound in result.ToList())
                {
                    var margin = rowFound.Margin;

                    Console.WriteLine($"Material => Cost: {margin.Material.Cost} Charge: {margin.Material.Charge} Margin: {margin.Material.Margin} %: {margin.Material.MarginPercent}");
                    Console.WriteLine($"Transport => Cost: {margin.Transport.Cost} Charge: {margin.Transport.Charge} Margin: {margin.Transport.Margin} %: {margin.Transport.MarginPercent}");
                    Console.WriteLine($"Production => Cost: {margin.Production.Cost} Charge: {margin.Production.Charge} Margin: {margin.Production.Margin} %: {margin.Production.MarginPercent}");
                    Console.WriteLine($"Miscellaneous => Cost: {margin.Miscellaneous.Cost} Charge: {margin.Miscellaneous.Charge} Margin: {margin.Miscellaneous.Margin} %: {margin.Miscellaneous.MarginPercent}");
                    Console.WriteLine($"Surcharge => Cost: {margin.Surcharge.Cost} Charge: {margin.Surcharge.Charge} Margin: {margin.Surcharge.Margin} %: {margin.Surcharge.MarginPercent}");
                    Console.WriteLine($"Total => Cost: {margin.Total.Cost} Charge: {margin.Total.Charge} Margin: {margin.Total.Margin} %: {margin.Total.MarginPercent}");

                }
                stopwatch.Stop();
                Console.WriteLine($"Post Query time: {stopwatch.Elapsed}");
                context.Connection.Close();
            }

            
        }

        [Test]
        public void GetSingleOrderTest()
        {
            var coid = "INC";
            using (var context = ContextFactory.GetOrdersContextForCoid(coid))
            {
                var query = new QueryOrders(coid);
                query.OrderNumber.Equals("2069048");
                var ordersAsQueryable = query.GetAsQueryable();
                var result = ordersAsQueryable.Select(x => new { x.SalesType, Company = x.Company, x.SalesHeader.Number, x.SalesHeader, x.CostItems, x.SalesCharges }).FirstOrDefault();

                //.FirstOrDefault();
                Assert.IsNotNull(result);

                var totals = result.SalesHeader.GetTotalMargin(coid, context, result.SalesHeader.SalesItem, result.CostItems, result.SalesCharges);

                Console.WriteLine($"Material => Cost: {totals.Material.Cost} Charge: {totals.Material.Charge} Margin: {totals.Material.Margin} %: {totals.Material.MarginPercent}");
                Console.WriteLine($"Transport => Cost: {totals.Transport.Cost} Charge: {totals.Transport.Charge} Margin: {totals.Transport.Margin} %: {totals.Transport.MarginPercent}");
                Console.WriteLine($"Production => Cost: {totals.Production.Cost} Charge: {totals.Production.Charge} Margin: {totals.Production.Margin} %: {totals.Production.MarginPercent}");
                Console.WriteLine($"Miscellaneous => Cost: {totals.Miscellaneous.Cost} Charge: {totals.Miscellaneous.Charge} Margin: {totals.Miscellaneous.Margin} %: {totals.Miscellaneous.MarginPercent}");
                Console.WriteLine($"Surcharge => Cost: {totals.Surcharge.Cost} Charge: {totals.Surcharge.Charge} Margin: {totals.Surcharge.Margin} %: {totals.Surcharge.MarginPercent}");
                Console.WriteLine($"Total => Cost: {totals.Total.Cost} Charge: {totals.Total.Charge} Margin: {totals.Total.Margin} %: {totals.Total.MarginPercent}");

            }

        }

        [Test]
        public void OrderDetailAdvancedQueryTest()
        {
            var result = OrderDetailsAdvancedQuery.GetProjectionForOrderNumber("INC", 37805);
            Console.WriteLine($"Order: {result.Coid}-{result.OrderNumber} Company: {result.CompanyCode}-{result.CompanyName}");
            Console.WriteLine($"Created: {result.OrderCreated} Modified: {result.OrderModied} Due: {result.OrderDueDate} SaleDate: {result.OrderSaleDate}");
            foreach (var item in result.SalesItems)
            {
                var margin = item.Margin;
                Console.WriteLine($"Item Number => {item.ItemNumber} Product: {item.ProductionCode} Required => Pieces: {item.RequiredPieces} Qty: {item.RequiredQuantity} Weight: {item.RequiredWeight}");
                Console.WriteLine(string.Concat(Enumerable.Repeat("=", 50)));
                Console.WriteLine($"Material => Cost: {margin.Material.Cost} Charge: {margin.Material.Charge} Margin: {margin.Material.Margin} %: {margin.Material.MarginPercent}");
                Console.WriteLine($"Transport => Cost: {margin.Transport.Cost} Charge: {margin.Transport.Charge} Margin: {margin.Transport.Margin} %: {margin.Transport.MarginPercent}");
                Console.WriteLine($"Production => Cost: {margin.Production.Cost} Charge: {margin.Production.Charge} Margin: {margin.Production.Margin} %: {margin.Production.MarginPercent}");
                Console.WriteLine($"Miscellaneous => Cost: {margin.Miscellaneous.Cost} Charge: {margin.Miscellaneous.Charge} Margin: {margin.Miscellaneous.Margin} %: {margin.Miscellaneous.MarginPercent}");
                Console.WriteLine($"Surcharge => Cost: {margin.Surcharge.Cost} Charge: {margin.Surcharge.Charge} Margin: {margin.Surcharge.Margin} %: {margin.Surcharge.MarginPercent}");
                Console.WriteLine($"Total => Cost: {margin.Total.Cost} Charge: {margin.Total.Charge} Margin: {margin.Total.Margin} %: {margin.Total.MarginPercent}");
                Console.WriteLine(string.Concat(Enumerable.Repeat("=", 50)));

            }
        }

        [Test]
        public void FastQueryTest()
        {
            _monitor.IsActive = true;

            var coid = "INC";
            var query = OrdersAdvancedQuery.AsQueryable(coid);
            query = query.Where(x => x.OrderNumber == 2069048);
            var result = query.ToList();

            _monitor.IsActive = false;
            foreach (var rowFound in result.ToList())
            {
                //if (rowFound.SalesItems == null) continue;
                
                var margin = rowFound.Margin;

                Console.WriteLine($"Material => Cost: {margin.Material.Cost} Charge: {margin.Material.Charge} Margin: {margin.Material.Margin} %: {margin.Material.MarginPercent}");
                Console.WriteLine($"Transport => Cost: {margin.Transport.Cost} Charge: {margin.Transport.Charge} Margin: {margin.Transport.Margin} %: {margin.Transport.MarginPercent}");
                Console.WriteLine($"Production => Cost: {margin.Production.Cost} Charge: {margin.Production.Charge} Margin: {margin.Production.Margin} %: {margin.Production.MarginPercent}");
                Console.WriteLine($"Miscellaneous => Cost: {margin.Miscellaneous.Cost} Charge: {margin.Miscellaneous.Charge} Margin: {margin.Miscellaneous.Margin} %: {margin.Miscellaneous.MarginPercent}");
                Console.WriteLine($"Surcharge => Cost: {margin.Surcharge.Cost} Charge: {margin.Surcharge.Charge} Margin: {margin.Surcharge.Margin} %: {margin.Surcharge.MarginPercent}");
                Console.WriteLine($"Total => Cost: {margin.Total.Cost} Charge: {margin.Total.Charge} Margin: {margin.Total.Margin} %: {margin.Total.MarginPercent}");

            }
        }

        [Test]
        public void GroupByTest()
        {
            var coid = "INC";
            var context = ContextFactory.GetOrdersContextForCoid(coid);
            var orderNumber = 2069048;
            var q =
                (
                    from h in context.SalesHeader
                    join c in context.Company on h.CustomerId equals c.Id
                    join dta in context.Address on h.DeliverToAddressId equals dta.Id
                    join da in context.Address on h.DeliveryAddressId equals da.Id
                    join st in context.SalesType on h.TypeId equals st.Id
                    join s in context.Personnel on h.InsideSalespersonId equals s.Id

                    join i in context.SalesItem on h.Id equals i.SalesHeaderId
                    where h.Number == orderNumber
                    select new
                    {
                        Coid = coid,
                        OrderNumber = h.Number ?? 0,
                        OrderVersion = h.Version ?? 0,
                        OrderDueDate = h.DueDate ?? new DateTime(1980, 1, 1),
                        OrderSaleDate = h.SaleDate ?? new DateTime(1980, 1, 1),
                        OrderCreated = h.Cdate ?? new DateTime(1980, 1, 1),
                        OrderModied = h.Mdate ?? new DateTime(1980, 1, 1),
                        OrderSaleTypeCode = st.Code,
                        OrderSaleTypeStatus = st.Status,
                        OrderSaleTypeDescription = st.Description,
                        SalesPerson = s.Name,
                        CompanyCode = c.Code,
                        CompanyName = c.Name,
                        CompanyShortName = c.ShortName,

                        i.ItemNumber,
                        i.Product.Code,

                        i.RequiredQuantity,
                        i.RequiredPiece,
                        i.RequiredWeight,

                        MaterialCosts = (context.CostItem.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode.Code == "MAT").Sum(ci => ci.BaseValue ?? 0)),
                        ProductionCosts = (context.CostItem.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode.Code == "PRD").Sum(ci => ci.BaseValue ?? 0)),
                        TransportCosts = (context.CostItem.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode.Code == "TRN").Sum(ci => ci.BaseValue ?? 0)),
                        MiscellaneousCosts = (context.CostItem.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode.Code == "MSC").Sum(ci => ci.BaseValue ?? 0)),
                        SurchargeCosts = (context.CostItem.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode.Code == "SUR").Sum(ci => ci.BaseValue ?? 0)),

                        MaterialCharges = (context.SalesCharge.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode == "MAT").Sum(ci => ci.BaseValue ?? 0)),
                        ProductionCharges = (context.SalesCharge.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode == "PRD").Sum(ci => ci.BaseValue ?? 0)),
                        TransportCharges = (context.SalesCharge.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode == "TRN").Sum(ci => ci.BaseValue ?? 0)),
                        MiscellaneousCharges = (context.SalesCharge.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode == "MSC").Sum(ci => ci.BaseValue ?? 0)),
                        SurchargeCharges = (context.SalesCharge.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode == "SUR").Sum(ci => ci.BaseValue ?? 0)),
                    } into orderValues
                    group orderValues by new
                    {
                        orderValues.Coid,
                        orderValues.OrderNumber,
                        orderValues.OrderDueDate,
                        orderValues.OrderSaleDate,
                        orderValues.OrderCreated,
                        orderValues.OrderModied,
                        orderValues.CompanyCode,
                        orderValues.CompanyName,
                        orderValues.CompanyShortName,
                        orderValues.OrderSaleTypeCode,
                        orderValues.OrderSaleTypeStatus,
                        orderValues.OrderSaleTypeDescription,
                        orderValues.SalesPerson
                    } into results
                    select new
                    {
                        results.Key.Coid,
                        results.Key.OrderNumber,
                        results.Key.CompanyCode,
                        results.Key.CompanyName,
                        results.Key.CompanyShortName,
                        results.Key.OrderCreated,
                        results.Key.OrderModied,
                        results.Key.OrderDueDate,
                        results.Key.OrderSaleDate,
                        Items = results
                    });

            var result = q.ToList();
        }

    }
}
