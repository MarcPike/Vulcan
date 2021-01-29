using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Models
{
    public class IncomingAnalysisQueryModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string ProductCode { get; set; }
        public List<string> CoidList { get; set; }
        public string DisplayCurrency { get; set; }
    }
}
