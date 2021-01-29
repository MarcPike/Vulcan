using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.SqlServer.Model;
using NUnit.Framework;

namespace DAL.HRS.SqlServer
{
    [TestFixture]
    public class BenefitsBuilder
    {
        private HrsContext _context = new HrsContext();
        public BenefitsModelHrs Data;

        [Test]
        public void TestEmployee()
        {
            var employee = _context.Employee.SingleOrDefault(x => x.LastName == "Madhavan" && x.FirstName == "Anu");
            if (employee != null)
                GetBaseBenefitsHrs(employee.OID);

            Console.WriteLine(ObjectDumper.Dump(Data));
            
        }

        public void GetBaseBenefitsHrs(int employeeId, HrsContext context = null)
        {

            if (context != null) _context = context;

            Data = new BenefitsModelHrs();

            var benefitEnrollments = _context.Benefits.AsNoTracking().FirstOrDefault(x => x.Employee == employeeId && x.GCRecord == null)?.BenefitEnrollment
                .ToList();

            if (benefitEnrollments != null)
            {
                foreach (var benefitEnrollment in benefitEnrollments)
                {

                    var benefitEnrollmentHrs = new BenefitEnrollmentHrs()
                    {
                        PlanType = benefitEnrollment.BenefitPlanType1?.Name ?? "Unknown",
                        OptionTypeCode = benefitEnrollment.BenefitOptionType1?.Code ?? "Unknown",
                        OptionTypeName = benefitEnrollment.BenefitOptionType1?.Name ?? "Unknown",
                        BeginDate = benefitEnrollment.BeginDate,
                        EndDate = benefitEnrollment.EndDate,
                        EmployeeContribution = benefitEnrollment.EmployeeCost ?? 0,
                        EmployerContribution = benefitEnrollment.EmployerCost ?? 0,
                        CoverageType = benefitEnrollment.BenefitCoverageType1?.Name ?? "Unknown",
                        StatusChangeType = benefitEnrollment.BenefitEnrollmentStatusChangeType1?.Name ?? "Unknown"
                    };

                    Data.BenefitEnrollments.Add(benefitEnrollmentHrs);

                }
            }
            else
            {
                //Console.WriteLine("No Enrollments");
            }

            var benefitDependents = _context.BenefitDependent.AsNoTracking().Where(x => x.Benefits1.Employee == employeeId && x.GCRecord == null).ToList();

            if (benefitDependents.Any())
            {
                foreach (var benefitDependent in benefitDependents)
                {
                    var dependent = new BenefitDependentHrs()
                    {
                        DependentName = benefitDependent.Name,
                        RelationShip = benefitDependent.BenefitDependentRelationshipType.Name,
                        Gender = benefitDependent.Gender1.GenderName,
                        GovernmentId = benefitDependent.GovernmentId,
                        DateOfBirth = benefitDependent.DateOfBirth,
                        PrimaryCarePhysicianID = benefitDependent.PrimaryCarePhysicianID ?? "Unknown",
                        Surcharge = benefitDependent.Surcharge ?? false,
                        BeginDate = benefitDependent.BeginDate,
                        EndDate = benefitDependent.EndDate,
                        DesignationType = benefitDependent.BeneficiaryDesignationType?.Name ?? "Unknown",
                        BeneficiaryPercentageType = benefitDependent.BeneficiaryPercentageType1?.Name ?? "Unknown"
                    };

                    Data.Dependents.Add(dependent);
                }
            }
            else
            {
                //Console.WriteLine("No Dependents");
            }

            var enrollmentHistory = _context.BenefitEnrollmentHistory.AsNoTracking().Where(x => x.Benefits1.Employee == employeeId && x.GCRecord == null)
                .ToList();
            if (enrollmentHistory.Any())
            {
                foreach (var benefitEnrollmentHistory in enrollmentHistory)
                {
                    var enrollmentHistoryHrs = new EnrollmentHistoryHrs()
                    {
                        CreatedOn = benefitEnrollmentHistory.CreatedOn,
                        BenefitPlan = benefitEnrollmentHistory.BenefitPlanType1?.Name ?? "Unknown",
                        OptionTypeCode = benefitEnrollmentHistory.BenefitOptionType1.Code,
                        OptionName = benefitEnrollmentHistory.BenefitOptionType1?.Name ?? "Unknown",
                        BeginDate = benefitEnrollmentHistory.BeginDate,
                        EndDate = benefitEnrollmentHistory.EndDate,
                        EmployeeCost = benefitEnrollmentHistory.EmployeeCost ?? 0,
                        EmployerCost = benefitEnrollmentHistory.EmployerCost ?? 0,
                        CoverageType = benefitEnrollmentHistory.BenefitCoverageType1?.Name ?? "Unknown",
                        StatusChangeType = benefitEnrollmentHistory.BenefitEnrollmentStatusChangeType1?.Name ?? "Unknown"
                    };

                    Data.EnrollmentHistory.Add(enrollmentHistoryHrs);
                }
            }
            else
            {
                //Console.WriteLine("No Enrollment History");
            }

            var dependentHistory = _context.BenefitDependentHistory.AsNoTracking().Where(x => x.Benefits1.Employee == employeeId && x.GCRecord == null)
                .ToList();
            if (dependentHistory.Any())
            {
                foreach (var benefitDependentHistory in dependentHistory)
                {
                    var dependentHistoryHrs = new DependentHistoryHrs()
                    {
                        CreatedOn = benefitDependentHistory.CreatedOn,
                        DependentName = benefitDependentHistory.Name,
                        RelationShip = benefitDependentHistory.BenefitDependentRelationshipType.Name,
                        GovernmentId = benefitDependentHistory.GovernmentId,
                        DateOfBirth = benefitDependentHistory.DateOfBirth,
                        BeginDate = benefitDependentHistory.BeginDate,
                        EndDate = benefitDependentHistory.EndDate,
                    };

                    Data.DependentHistory.Add(dependentHistoryHrs);
                }
            }
            else
            {
                //Console.WriteLine("No Dependent History");
            }

        }


    }

    public class BenefitsModelHrs
    {
        public List<BenefitEnrollmentHrs> BenefitEnrollments { get; set; } = new List<BenefitEnrollmentHrs>();
        public List<BenefitDependentHrs> Dependents { get; set; } = new List<BenefitDependentHrs>();

        public List<EnrollmentHistoryHrs> EnrollmentHistory { get; set; } = new List<EnrollmentHistoryHrs>();
        public List<DependentHistoryHrs> DependentHistory { get; set; } = new List<DependentHistoryHrs>();

    }

    public class DependentHistoryHrs

    {
        public DateTime? CreatedOn { get; set; }
        public string DependentName { get; set; }
        public string RelationShip { get; set; }
        public string GovernmentId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class EnrollmentHistoryHrs
    {
        public DateTime? CreatedOn { get; set; }
        public string BenefitPlan { get; set; }
        public string OptionTypeCode { get; set; }
        public string OptionName { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal EmployeeCost { get; set; }
        public decimal EmployerCost { get; set; }
        public decimal TotalCost => EmployeeCost + EmployerCost;
        public string CoverageType { get; set; }
        public string StatusChangeType { get; set; }
    }

    public class BenefitEnrollmentHrs
    {
        public string PlanType { get; set; } = string.Empty;
        public string OptionTypeCode { get; set; } = string.Empty;
        public string OptionTypeName { get; set; } = string.Empty;
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal EmployeeContribution { get; set; }
        public decimal EmployerContribution { get; set; }
        public decimal TotalCost => EmployeeContribution + EmployerContribution;
        public string CoverageType { get; set; } = string.Empty;
        public string StatusChangeType { get; set; } = string.Empty;

    }

    public class BenefitDependentHrs
    {
        public string DependentName { get; set; }
        public string RelationShip { get; set; }
        public string Gender { get; set; }
        public string GovernmentId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PrimaryCarePhysicianID { get; set; }
        public bool? Surcharge { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DesignationType { get; set; }
        public string BeneficiaryPercentageType { get; set; }
    }
}
