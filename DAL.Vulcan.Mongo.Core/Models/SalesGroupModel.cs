using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class SalesGroupModel
    {
        public string Id { get; set; }
        public string Coid { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public SalesGroupModel()
        {

        }

        public SalesGroupModel(SalesGroup g)
        {
            Id = g.Id.ToString();
            Coid = g.Coid;
            Code = g.Code;
            Description = g.Description;
            IsActive = g.IsActive;
        }
    }
}
