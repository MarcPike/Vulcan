using Devart.Data.Linq.Monitoring;
using Newtonsoft.Json;
using System.Collections.Generic;
using Devart.Data.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.StockItems;

namespace Vulcan.IMetal.ViewFilterObjects
{
    public class QueryBase<TResult>
    { 

        [JsonIgnore]
        private readonly LinqMonitor _monitor = new LinqMonitor();
        public string Coid { get; private set; }

        public List<EnumValue> StringFilterTypes => EnumValue.FromEnum("StringFilterType");

        //[JsonIgnore]
        //public DataContext Context { get; set; }
        public void TurnOnMonitor()
        {
            _monitor.IsActive = true;
        }

        public void TurnOffMonitor()
        {
            _monitor.IsActive = false;
        }

        public void SetCoid(string coid)
        {
            Coid = coid;
            //Context = ContextFactory.GetContextForCoid(Coid);
        }

        public QueryBase(string coid)
        {
            Coid = coid;
        }

        public virtual List<TResult> Execute()
        {
            return new List<TResult>();   
        }

        public virtual List<TResult> ExecuteWithLimit(int numberToTake)
        {
            return new List<TResult>();
        }

        public List<TResult> MergeWith(List<TResult> products, string otherCoid)
        {
            this.SetCoid(otherCoid);
            var result = Execute();
            products.AddRange(result);
            return products;
        }

    }
}
