using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;

namespace DAL.HRS.Mongo.DocClass.Benefits
{
    public class Benefits : ISupportLocationNameChangesNested
    {
        public List<BenefitEnrollment> BenefitEnrollment { get; set; } = new List<BenefitEnrollment>();
        public List<BenefitDependent> BenefitDependent { get; set; } = new List<BenefitDependent>();
        public List<BenefitEnrollmentHistory> BenefitEnrollmentHistory { get; set; } = new List<BenefitEnrollmentHistory>();
        public List<BenefitDependentHistory> BenefitDependentHistory { get; set; } = new List<BenefitDependentHistory>();

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            foreach (var benefitEnrollment in BenefitEnrollment)
            {
                modified = benefitEnrollment.ChangeOfficeName(locationId, newName, modified);
            }

            foreach (var dependent in BenefitDependent)
            {
                modified = dependent.ChangeOfficeName(locationId, newName, modified);
            }

            foreach (var history in BenefitEnrollmentHistory)
            {
                modified = history.ChangeOfficeName(locationId, newName, modified);

            }

            foreach (var history in BenefitDependentHistory)
            {
                modified = history.ChangeOfficeName(locationId, newName, modified);
            }


            return modified;
        }
    }
}