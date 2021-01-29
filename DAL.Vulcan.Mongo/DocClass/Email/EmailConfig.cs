using System;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Email
{
    public class EmailConfig: BaseDocument
    {
        public DateTime LastEmailScan { get; set; } = DateTime.MinValue;
        public string UserName { get; set; } = "vulcancrm@howcogroup.com";
        public string Password { get; set; } = "xbFaC9Q2CO3u7erLN1ps";
    }
}
