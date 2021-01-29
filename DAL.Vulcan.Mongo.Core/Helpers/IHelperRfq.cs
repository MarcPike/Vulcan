using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Models;
using DAL.Vulcan.Mongo.Core.RequestForQuoteExternal;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperRfq
    {
        RfqCustomerModel GetNewRfqExternalModelForCustomer();
        List<string> GetLocationNamesForRfq();
        void SaveRfqExternal(RfqCustomerModel model);
        List<RfqExternalModel> GetAllForTeam(string application, string userId, TeamRef team, DateTime fromDate, DateTime toDate);
        RfqExternalModel Accept(RfqExternalModel model);
        RfqExternalModel Reject(RfqExternalModel model);
        RfqTeamConfigModel GetTeamConfig(string application, string userId, TeamRef team);
        RfqTeamConfigModel SaveTeamConfig(RfqTeamConfigModel model);
    }
}