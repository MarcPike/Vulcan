using System;
using System.Collections.Generic;
using DAL.HRS.Mongo.DocClass.Benefits;
using DAL.HRS.Mongo.Logger;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperBenefits
    {
        List<BenefitsGridModel> GetBenefitsGrid(string userId);
        BenefitsModel GetBenefitsModel(string employeeId);
        BenefitsModel SaveBenefitsModel(BenefitsModel model);
        BenefitEnrollmentModel GetNewEnrollmentModel();
        BenefitEnrollmentHistoryModel GetNewEnrollmentHistoryModel();
        BenefitDependentModel GetNewDependentModel();
        BenefitDependentHistoryModel GetNewDependentHistoryModel();
        VulcanLogger Logger { get; set; }
        Dictionary<string, object> GetParametersDictionary();
        string GetClassName();
        BenefitsModel RenewBenefitsEnrollment(string employeeId, string enrollmentId, DateTime endDate, DateTime newStartDate, DateTime? newEndDate);
    }
}