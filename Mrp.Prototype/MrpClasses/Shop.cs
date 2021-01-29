using System.Collections.Generic;
using System.Linq;

namespace Mrp.Prototype.MrpClasses
{
    public class Shop : MrpObject
    {
        public string Name { get; set; }
        public List<ShopWorker> ShopWorkers { get; set; } = new List<ShopWorker>();
        public List<ShopManager> ShopManagers { get; set; } = new List<ShopManager>();
        public List<ShopScheduler> ShopSchedulers { get; set; } = new List<ShopScheduler>();
        public List<Resource> Resources { get; set; } = new List<Resource>();
        public List<ShopScheduledWorkItem> ScheduledWorkItems { get; set; } = new List<ShopScheduledWorkItem>();

        public List<TransportJob> TransportJobs { get; set; } = new List<TransportJob>();

        public ShopWorker GetShopWorker(string pin)
        {
            return ShopWorkers.FirstOrDefault(x => x.GetPinCode == pin);
        }

        public ShopManager GetShopManager(string pin)
        {
            return ShopManagers.FirstOrDefault(x => x.GetPinCode == pin);
        }
    }
}