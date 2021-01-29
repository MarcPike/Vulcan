using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using Vulcan.IMetal.Helpers;
using Vulcan.IMetal.Queries.PurchaseOrderItems;

namespace DAL.Vulcan.Mongo.Models
{
    public class IncomingAnalysisResultModel
    {
        public ProductMaster ProductInfo { get; }
        public int OrderCount { get; set; }
        public List<string> CoidsFound { get; set; } = new List<string>();
        public IncomingAnalysisDetailsModel Days7 { get; set; }
        public IncomingAnalysisDetailsModel Days15 { get; set; }
        public IncomingAnalysisDetailsModel Days30 { get; set; }
        public IncomingAnalysisDetailsModel Days60 { get; set; }
        public IncomingAnalysisDetailsModel Days90 { get; set; }
        public IncomingAnalysisDetailsModel Days120 { get; set; }
        public IncomingAnalysisDetailsModel DaysGreaterThan120 { get; set; }

        private class IncomingQuery
        {
            public string Coid { get; set; }
            public string ProductCode { get; set; }
            public string DisplayCurrency { get; set; }
            public decimal ExchangeRate { get; set; }
            public List<IncomingAnalysisDataModel> Orders { get; set; }


            public IncomingQuery(string coid, string productCode, string displayCurrency)
            {
                Coid = coid;
                ProductCode = productCode;
                DisplayCurrency = displayCurrency;
                var helperCurrency = new HelperCurrencyForIMetal();
                var baseCurrency = helperCurrency.GetDefaultCurrencyForCoid(coid);
                ExchangeRate = helperCurrency.ConvertValueFromCurrencyToCurrency(1, baseCurrency, displayCurrency);

            }

            public void PerformQuery()
            {
                Orders = PurchaseOrderItemsAdvancedQuery.AsQueryable(Coid).Where(x => x.ProductCode == ProductCode && x.ItemDueDate > DateTime.Now.Date).Select(x=> new IncomingAnalysisDataModel(x, ExchangeRate)).ToList();
            }
        }

        public IncomingAnalysisResultModel( IncomingAnalysisQueryModel model)
        {

            if (!model.CoidList.Any())
            {
                model.CoidList = new List<string>() {"INC","CAN","EUR","SIN","MSA","DUB"};
            }

            List <IncomingQuery> incomingQueries = new List<IncomingQuery>();
            foreach (var coid in model.CoidList)
            {
                incomingQueries.Add(new IncomingQuery(coid, model.ProductCode, model.DisplayCurrency));
            }

            List<Task> tasks = new List<Task>();
            foreach (var incomingQuery in incomingQueries)
            {
                tasks.Add(Task.Factory.StartNew(() => incomingQuery.PerformQuery()));
            }

            Task.WaitAll(tasks.ToArray());

            List<IncomingAnalysisDataModel> allOrders = new List<IncomingAnalysisDataModel>();
            foreach (var incomingQuery in incomingQueries)
            {
                allOrders.AddRange(incomingQuery.Orders);
                if (incomingQuery.Orders.Any())
                {
                    CoidsFound.Add(incomingQuery.Coid);
                }
            }

            if (!allOrders.Any()) return;

            var firstOrder = allOrders.First();
            ProductInfo = new ProductMaster(firstOrder.Coid, model.ProductCode);
            OrderCount = allOrders.Count;
            Calculate(allOrders);
            
        }

        private void Calculate(List<IncomingAnalysisDataModel> orders)
        {
            var today = DateTime.Now.Date;
            var days7 = today.AddDays(7);
            var days15 = today.AddDays(15);
            var days30 = today.AddDays(30);
            var days60 = today.AddDays(60);
            var days90 = today.AddDays(90);
            var days120 = today.AddDays(120);

            Days7 = new IncomingAnalysisDetailsModel(orders.Where(x => x.ItemDueDate > today && x.ItemDueDate <= days7).ToList());
            Days15 = new IncomingAnalysisDetailsModel(orders.Where(x => x.ItemDueDate > days7 && x.ItemDueDate <= days15).ToList());
            Days30 = new IncomingAnalysisDetailsModel(orders.Where(x => x.ItemDueDate > days15 && x.ItemDueDate <= days30).ToList());
            Days60 = new IncomingAnalysisDetailsModel(orders.Where(x => x.ItemDueDate > days30 && x.ItemDueDate <= days60).ToList());
            Days90 = new IncomingAnalysisDetailsModel(orders.Where(x => x.ItemDueDate > days60 && x.ItemDueDate <= days90).ToList());
            Days120 = new IncomingAnalysisDetailsModel(orders.Where(x => x.ItemDueDate > days90 && x.ItemDueDate <= days120).ToList());
            DaysGreaterThan120 = new IncomingAnalysisDetailsModel(orders.Where(x => x.ItemDueDate > days120).ToList());

        }

    }
}
