using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class GraphData
    {
        public string Name { get; set; }
        public decimal Y { get; set; }
    }

    public class CrmUserLogSummary
    {
        public class ModuleHit
        {
            public string Module { get; set; }
            public int Visits { get; set; }
            public decimal Percent { get; set; }
        }

        public string Id { get; set; }
        public string UserName { get; }
        public DateTime? LastConnected { get; }
        public List<ModuleHit> ModuleHits { get;  } = new List<ModuleHit>();

        public List<GraphData> GraphData
        {
            get
            {
                var result = new List<GraphData>();
                foreach (var hit in ModuleHits)
                {
                    result.Add(new GraphData()
                    {
                        Name = hit.Module,
                        Y = hit.Percent
                    });
                }

                return result;
            }
        }

        private List<String> IgnoreModules =>
            new List<string>()
            {
                "Action"
            };

        private string RemoveControllerFromName(string controllerName)
        {
            return controllerName.Replace("Controller", "");
        }

        public CrmUserLogSummary(CrmUserLog log)
        {
            Id = log.User.Id;
            UserName = log.User.FullName;
            LastConnected = (log.LastConnected == DateTime.MinValue) ? (DateTime?) null : log.LastConnected;
            var controllers = log.ControllerMethodHistory.Select(x => $"{x.Controller} ({x.Method})"  ).Distinct().ToList();
            foreach (var controller in controllers)
            {
                var module = RemoveControllerFromName(controller);
                if (IgnoreModules.Any(x => module.Contains(x)))
                {
                    continue;
                }

                ModuleHits.Add(new ModuleHit()
                {
                    Module = module,
                    Visits = log.ControllerMethodHistory.Count(x => $"{x.Controller} ({x.Method})" == controller)
                });
            }

            var controllerVisitTotal = (decimal)ModuleHits.Sum(x => x.Visits);
            foreach (var moduleHit in ModuleHits)
            {
                moduleHit.Percent = (decimal)(moduleHit.Visits / controllerVisitTotal);
            }
        }
    }
    public class CrmUserLogSummaryModel
    {
        private List<CrmUserLog> _crmUserLogs = new List<CrmUserLog>();
        public List<CrmUserLogSummary> UserLogSummary { get; } = new List<CrmUserLogSummary>();
        public CrmUserLogSummaryModel()
        {

        }

        public CrmUserLogSummaryModel(List<CrmUserLog> users)
        {
            _crmUserLogs = users;
            foreach (var crmUserLog in _crmUserLogs.OrderBy(x=>x.User.FullName))
            {
                UserLogSummary.Add(new CrmUserLogSummary(crmUserLog));
            }
        }
    }
}
