using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class CrmUserInfo
    {
        public string Coid { get; } = string.Empty;
        public bool IsAdmin { get; } = false;
        public string DefaultCurrency { get; } = string.Empty;
        public ViewConfig ViewConfig { get; } 
        public List<TeamRef> Teams { get; } = new List<TeamRef>();
        public string UserType { get; } = string.Empty;
        public UserRef User { get; } 
        public CrmUserRef CrmUserRef { get; }

        public bool IsCalcAdmin { get; set; }

        public CrmUserInfo()
        {
        }

        public CrmUserInfo(CrmUser crmUser)
        {
            var coid = crmUser.Coid;

            var defaultCurrency = "USD";
            if (coid == "EUR") defaultCurrency = "GBP";
            if (coid == "CAN") defaultCurrency = "CAD";

            Coid = coid;
            IsAdmin = crmUser.IsAdmin;
            DefaultCurrency = defaultCurrency;
            ViewConfig = crmUser.ViewConfig;
            Teams = crmUser.Teams;
            UserType = crmUser.UserType.ToString();
            User = crmUser.User;
            CrmUserRef = crmUser.AsCrmUserRef();
            IsCalcAdmin = crmUser.IsCalcAdmin;

        }
    }
}
