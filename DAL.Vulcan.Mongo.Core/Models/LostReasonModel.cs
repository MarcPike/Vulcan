using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class LostReasonModel
    {
        public string Id { get; set; }
        public string Reason { get; set; }

        public static List<LostReasonModel> GetList()
        {
            var filter = LostReason.Helper.FilterBuilder.Empty;
            var project = LostReason.Helper.ProjectionBuilder
                .Expression(x => new LostReasonModel() {Id = x.Id.ToString(), Reason = x.Reason});
            return LostReason.Helper.FindWithProjection(filter, project).ToList().OrderBy(x => x.Reason).ToList();

        }
    }
}
