using DAL.HRS.Mongo.DocClass.Benefits;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.SqlServer;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Import.ImportHrs
{
    public static class BenefitsTransformer
    {
        public static void TransformBenefits(Employee employee, BenefitsModelHrs model)
        {
            employee.Benefits = new Benefits();

            var unknownOptionTypeCode = PropertyBuilder.CreatePropertyValue("BenefitOptionType",
                "Type of Benefits Option",
                "Unknown", "Unknown Option Type").AsPropertyValueRef();

            foreach (var enrollment in model.BenefitEnrollments)
            {
                
                employee.Benefits.BenefitEnrollment.Add(new BenefitEnrollment()
                {
                    PlanType = PropertyBuilder.CreatePropertyValue("BenefitPlanType", "Type of Benefits Plan",
                        enrollment.PlanType, "Plan type").AsPropertyValueRef(),
                    OptionTypeCode = (enrollment.OptionTypeCode == null || enrollment.OptionTypeName == null) ? unknownOptionTypeCode :
                        PropertyBuilder.CreatePropertyValue("BenefitOptionType", "Type of Benefits Option",
                        enrollment.OptionTypeCode, enrollment.OptionTypeName).AsPropertyValueRef(), 
                    BeginDate = enrollment.BeginDate,
                    EndDate = enrollment.EndDate,
                    EmployeeContribution = enrollment.EmployeeContribution,
                    EmployerContribution = enrollment.EmployerContribution,
                    CoverageType = PropertyBuilder.CreatePropertyValue("BenefitCoverageType", "Type of Benefits Coverage",
                        enrollment.CoverageType, "Coverage type").AsPropertyValueRef(),
                    StatusChangeType = PropertyBuilder.CreatePropertyValue("BenefitStatusChangeType", "Type of Status Change",
                        enrollment.StatusChangeType, "Status Change type").AsPropertyValueRef(),
                });

            }

            foreach (var hist in model.EnrollmentHistory)
            {
                employee.Benefits.BenefitEnrollmentHistory.Add(new BenefitEnrollmentHistory()
                {
                    BeginDate = hist.BeginDate,
                    EndDate = hist.EndDate,
                    BenefitPlan = PropertyBuilder.CreatePropertyValue("BenefitPlanType", "Type of Benefits Plan",
                        hist.OptionTypeCode, hist.OptionName).AsPropertyValueRef(),
                    CoverageType = PropertyBuilder.CreatePropertyValue("BenefitCoverageType", "Type of Benefits Coverage",
                    hist.CoverageType, "Coverage type").AsPropertyValueRef(),
                    CreatedOn = hist.CreatedOn,
                    EmployeeCost = hist.EmployeeCost,
                    EmployerCost = hist.EmployerCost,
                    OptionTypeCode = (hist.OptionTypeCode == null || hist.OptionName == null) ? unknownOptionTypeCode :
                        PropertyBuilder.CreatePropertyValue("BenefitOptionType", "Type of Benefits Option",
                            hist.OptionTypeCode, hist.OptionName).AsPropertyValueRef(),
                    StatusChangeType = PropertyBuilder.CreatePropertyValue("BenefitStatusChangeType", "Type of Status Change",
                        hist.StatusChangeType, "Status Change type").AsPropertyValueRef(),
                });
            }


            foreach (var dependent in model.Dependents)
            {
                employee.Benefits.BenefitDependent.Add(new BenefitDependent()
                {
                    DependentName = dependent.DependentName,
                    RelationShip = PropertyBuilder.CreatePropertyValue("DependentRelationshipType","Dependent Relationship type",
                        dependent.RelationShip, "Relationship type").AsPropertyValueRef(),
                    Gender = PropertyBuilder.CreatePropertyValue("Gender", "Gender", 
                            dependent.Gender, "Employee Gender").AsPropertyValueRef(),
                    GovernmentId = dependent.GovernmentId,
                    DateOfBirth = dependent.DateOfBirth,
                    PrimaryCarePhysicianId = dependent.PrimaryCarePhysicianID,
                    Surcharge = dependent.Surcharge,
                    BeginDate = dependent.BeginDate,
                    EndDate = dependent.EndDate,
                    BeneficiaryPercentageType = PropertyBuilder.CreatePropertyValue("BeneficiaryPercentageType", "Beneficiary Percentage Type",
                        dependent.BeneficiaryPercentageType, "Percentage type").AsPropertyValueRef(),
                    DesignationType = PropertyBuilder.CreatePropertyValue("DependentDesignationType", "Dependent Designation Type",
                        dependent.DesignationType, "Designation type").AsPropertyValueRef(),
                });
            }

            foreach (var hist in model.DependentHistory)
            {
                employee.Benefits.BenefitDependentHistory.Add(new BenefitDependentHistory()
                {
                    BeginDate = hist.BeginDate,
                    EndDate = hist.EndDate,
                    CreatedOn = hist.CreatedOn,
                    DateOfBirth = hist.DateOfBirth,
                    DependentName = hist.DependentName,
                    GovernmentId = hist.GovernmentId,
                    RelationShip = PropertyBuilder.CreatePropertyValue("DependentRelationshipType", "Dependent Relationship type",
                        hist.RelationShip, "Relationship type").AsPropertyValueRef(),
                    
                });
            }



        }
    }
}
