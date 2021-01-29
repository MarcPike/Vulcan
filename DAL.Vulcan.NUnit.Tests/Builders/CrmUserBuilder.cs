using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Helpers;

namespace DAL.Vulcan.NUnit.Tests.Builders
{
    public static class CrmUserBuilder
    {
        private static HelperUser _helperUser = new HelperUser(new HelperPerson());
        public static Mongo.DocClass.CRM.CrmUser GetMarcPike()
        {
            return _helperUser.GetCrmUser("vulcancrm","mpike");
        }

        public static Mongo.DocClass.CRM.CrmUser GetIsidro()
        {
            return _helperUser.GetCrmUser("igallego", "mpike");
        }

        public static Mongo.DocClass.CRM.CrmUser GetAnu()
        {
            return _helperUser.GetCrmUser("vulcancrm", "amadhavan");
        }

        public static Mongo.DocClass.CRM.CrmUser GetCrmUser(string networkId)
        {
            return _helperUser.GetCrmUser("vulcancrm", networkId);
        }
    }
}
