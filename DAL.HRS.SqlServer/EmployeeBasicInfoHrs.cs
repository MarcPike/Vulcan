using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.SqlServer.Model;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.SqlServer
{

    [TestFixture()]
    public class EmployeeBasicInfoHrs
    {
        private HrsContext _context = new HrsContext();

        //[Test]
        //public void BasicTest()
        //{
        //    var employee = _context.Employee.FirstOrDefault(x => x.OID == 1032);

        //    var data = GetEmployeeBasicInfo(employee);
        //    var firstEmployeeWithDiscipline = data.FirstOrDefault(x => x.Discipline.Count > 0);
        //    var firstEmployeeWithDisciplineHistory = data.FirstOrDefault(x => x.DisciplineHistory.Count > 0);
        //    //var firstEmployeeWithPerformance = data.FirstOrDefault(x => x.Performance.Count > 0);

        //}

        public List<EmployeeModelHrs> GetEmployeeBasicInfo(int testEmployeeId = 0)
        {
            var result = new List<EmployeeModelHrs>();
            var minTerminationDate = DateTime.Parse("1/1/2019");

            //var employees = (testEmployee != null) ? _context.Employee.Where(x => x.OID == testEmployee.OID) : _context.Employee.AsNoTracking().Where(x=>x.TerminationDate == null || x.TerminationDate >= minTerminationDate);

            IQueryable<Employee> employees;
            if (testEmployeeId > 0)
            {
                employees = _context.Employee.Where(x => x.OID == testEmployeeId);
            }
            else
            {
                //employees = _context.Employee.AsNoTracking().Where(x => x.TerminationDate == null || x.TerminationDate >= minTerminationDate);
                employees = _context.Employee.AsNoTracking().Where(x=>x.GCRecord == null);
            }



            foreach (var emp in employees)
            {

                var medicalInfo = emp.MedicalInfo.ToList();
                
                try
                {
                    var newEmployeeModel = new EmployeeModelHrs
                    {
                        OldHrsId = emp.OID,
                        PayrollId = emp.PayrollID,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        MiddleName = emp.MiddleName,
                        PreferredName = emp.PreferredName,
                        GovernmentId = emp.GovernmentID,
                        OriginalHireDate = emp.OriginalHireDate,
                        PhoneType = emp.EmployeePhoneNumbers?.FirstOrDefault()?.PhoneType1?.PhoneTypeName ?? "None",
                        Birthday = emp.Birthday,
                        WorkEmail = emp.WorkEmail,
                        PersonalEmail = emp.Email,
                        Address1 = emp.Address1,
                        Address2 = emp.Address2,
                        Address3 = emp.Address3,
                        City = emp.City,
                        State = emp.State_Province_Codes1?.StateName,
                        Country = emp.CountryCodes?.CountryName,
                        PostalCode = emp.PostalCode,
                        Mobile = emp.EmployeePhoneNumbers.FirstOrDefault(p => p.PhoneType == 1)?.Number ?? string.Empty,
                        Home = emp.EmployeePhoneNumbers.FirstOrDefault(p => p.PhoneType == 3)?.Number ?? string.Empty,
                        KronosClockType = emp.KronosClockType1?.Name ?? string.Empty,
                        PriorServiceDate = emp.PriorServiceDate,
                        LastRehireDate = emp.LastRehireDate,
                        ConfirmationDate = emp.ConfirmationDate,
                        TerminationDate = emp.TerminationDate,
                        TerminationCode = emp.TerminationCodes1?.CodeName ?? string.Empty,
                        TerminationExplanation = emp.TerminationExplanation1?.Explanation ?? string.Empty,
                        Ethnicity = emp.Ethnicity1?.EthnicityName,
                        Gender = emp.Gender1?.GenderName,
                        MaritalStatus = emp.MaritalStatus1?.MaritalStatusName,
                        CountryOfOrigin = emp.CountryCodes1?.CountryName,
                        Nationality = emp.Nationality1?.NationalityName,
                        Title = emp.TitleType1?.TitleName,
                        Login = emp.ActiveDirectoryLogin,
                        TimeTrackingAccrualProfile = emp.TimeTracking2?.FirstOrDefault()?.AccrualProfile1.Name,
                        TimeTrackingAccrualProfileEffectiveDate = emp.TimeTracking2?.FirstOrDefault()?.EffectiveDate,
                        CostCenterCode = emp.Department1.DepartmentCode,
                        CostCenterName = emp.Department1.DepartmentName,
                        Location = emp.Location1.Name,
                        EEO_Code = emp.EEO_Code1.EEO_CodeName,
                        RehireStatus = emp.RehireStatus1?.RehireName ?? string.Empty,
                        KronosDepartment = emp.KronosDepartment1?.Name ?? string.Empty,
                        //KronosLaborLevel = emp.KronosLaborLevel1?.Name ?? string.Empty,
                        PayrollRegion = emp.PayrollRegion1?.Name ?? string.Empty,
                        BusinessRegion = emp.BusinessRegion1?.Name ?? string.Empty,
                        ////IssuedEquipment =
                        ////{
                        //    AccessCard = emp.AccessCard ?? false,
                        //    CellPhone = emp.Blackberry ?? false,
                        //    CompanyCar = emp.CompanyCar ?? false,
                        //    CreditCard = emp.CreditCard ?? false,
                        //    Desktop = emp.Desktop ?? false,
                        //    GasCard = emp.GasCard ?? false,
                        //    Laptop = emp.Laptop ?? false,
                        //    PPE = emp.PPE ?? false,
                        //    ShopKeys = emp.Shopkeys ?? false,
                        //    Uniform = emp.Uniform ?? false
                        //},
                        WorkArea = emp.WorkArea1?.WorkAreaName ?? "Unknown",
                        
                    };

                    try
                    {
                        
                        GetEmployeePerformance(newEmployeeModel, _context, emp.PerformanceEvaluationType1);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                    try
                    {
                        GetEmployeeDiscipline(_context, newEmployeeModel);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                    try
                    {
                        GetWorkStatusHistory(newEmployeeModel, emp);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                    try
                    {
                        GetBenefits(newEmployeeModel, emp.OID, _context);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }



                    var manager = _context.Employee.FirstOrDefault(x => x.OID == emp.Manager);
                    if (manager != null)
                    {
                        newEmployeeModel.ManagerPayrollId = manager.PayrollID;
                        newEmployeeModel.ManagerName =
                            $"{manager.LastName}, {manager.FirstName} {manager.MiddleName}".TrimEnd();
                    }


                    var status1Id = emp.StatusType1 ?? 0;
                    newEmployeeModel.Status1 = _context.US_StatusTypes.SingleOrDefault(x => x.OID == status1Id)?.StatusName;

                    var status2Id = emp.StatusType2 ?? 0;
                    newEmployeeModel.Status2 = _context.UK_StatusTypes.SingleOrDefault(x => x.OID == status2Id)?.StatusName;

                    var emergencyContacts = emp.EmergencyContacts.ToList();
                    foreach (var emergencyContact in emergencyContacts)
                    {
                        newEmployeeModel.EmergencyContacts.Add(new EmergencyContactModelHrs()
                        {
                            Name = emergencyContact.ContactName,
                            PhoneNumber = emergencyContact.ContactPhone,
                            Relationship = emergencyContact.Relationship

                        });
                    }

                    var employeeVerifications = emp.EmploymentVerification.ToList();
                    foreach (var employmentVerification in employeeVerifications)
                    {

                        newEmployeeModel.EmployeeVerifications.Add(new EmployeeVerificationModelHrs()
                        {
                            DocumentNumber = employmentVerification.VerificationNumber,
                            DocumentType = employmentVerification.EmploymentVerificationDocumentType.Name,
                            DocumentExpiration = employmentVerification.Expiration ?? new DateTime(1900, 1, 1),
                            
                        });
                    }

                    result.Add(newEmployeeModel);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
            return result;
        }

        private void GetBenefits(EmployeeModelHrs newEmployeeModel, int employeeId, HrsContext context)
        {
            var benefitsBuilder = new BenefitsBuilder();
            benefitsBuilder.GetBaseBenefitsHrs(employeeId, context);
            newEmployeeModel.Benefits = benefitsBuilder.Data;
        }

        private void GetEmployeePerformance(EmployeeModelHrs model, HrsContext context, PerformanceEvaluationType evalType )
        {
            model.Performance = new PerformanceHrs();

            var performance = context.Performance.AsNoTracking().FirstOrDefault(x => x.Employee == model.OldHrsId && x.GCRecord == null);
            if (performance == null) return;

            var perfHist = new List<PerformanceHistoryHrs>();

            var performanceHistory = context.PerformanceHistory.Where(x => x.Performance == performance.OID).ToList();
            foreach (var history in performanceHistory)
            {
                perfHist.Add(new PerformanceHistoryHrs()
                {
                    CreatedOn = history.CreatedOn,
                    Date = history.Date,
                    DateOfNextReview = history.DateOfNextReview,
                    GradeRatingType = history.GradeRatingType1?.Name ?? string.Empty,
                    Notes = history.Notes,
                    Reviewer = history.Reviewer,
                    PerformanceReviewType = history.PerformanceReviewType?.Name ?? string.Empty,
                    RecommendPayIncrease = GetPerformanceRecommended(history.RecommendPayIncrease),
                    RecommendPromotion = GetPerformanceRecommended(history.RecommendPromotion),
                });

            }

            var perfRateOutcome = new List<PerformanceRateOutcomeHrs>();

            if (performance.RatingOutcome != null)
            {
                foreach (var rateOutcome in performance.RatingOutcome)
                {
                    perfRateOutcome.Add(new PerformanceRateOutcomeHrs()
                    {
                        Comment = rateOutcome.Comment,
                        RatingOutcomeType = rateOutcome.RatingOutcomeType1?.Name ?? string.Empty,

                    });
                }
            }


            model.Performance = new PerformanceHrs()
            {
                DateOf = performance.Date,
                DateOfNextReview = performance.DateOfNextReview,
                GradeRatingType = performance.GradeRatingType1?.Name ?? string.Empty,
                Notes = performance.Notes,
                PerformanceHistory = perfHist,
                PerformanceReviewType = performance.PerformanceReviewType?.Name ?? "Unknown",
                RatingOutcomes = perfRateOutcome,
                RecommendPayIncrease = GetPerformanceRecommended(performance.RecommendPayIncrease),
                RecommendPromotion = GetPerformanceRecommended(performance.RecommendPromotion),
                ReviewerId = performance.Reviewer,
            };

            try
            {
                if (evalType != null)
                {
                    model.EvaluationTypeCode = evalType.Code ?? string.Empty;
                    model.EvaluationTypeDescription = evalType.Description ?? string.Empty;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        private string GetPerformanceRecommended(int? value)
        {
            if (value == null) return "Unknown";
            if (value == 0) return "No";
            if (value == 1) return "Yes";
            if (value == 2) return "N/A";
            return "Unknown";
        }

        private void GetEmployeeDiscipline(HrsContext context, EmployeeModelHrs newEmployeeModel)
        {
            newEmployeeModel.Discipline.Clear();
            newEmployeeModel.DisciplineHistory.Clear();

            var discipline = context.Discipline.FirstOrDefault(x => x.Employee == newEmployeeModel.OldHrsId && x.GCRecord == null);
            if (discipline == null) return;

            var hist = new List<DisciplineHistoryHrs>();

            var empDisciplineHistory = context.DisciplineHistory.AsNoTracking().Where(x => x.Discipline == discipline.OID).ToList();

            foreach (var disciplineHistory in empDisciplineHistory.ToList())
            {
                hist.Add(new DisciplineHistoryHrs()
                {
                    CreatedOn = disciplineHistory.CreatedOn,
                    DateOfAction = disciplineHistory.DateOfAction,
                    DateOfViolation = disciplineHistory.DateOfViolation,
                    DisciplinaryActionType = disciplineHistory.DisciplinaryActionType1.Name,
                    EmployeeInAgreement = GetEmployeeInAgreement(disciplineHistory.EmployeeInAgreement),
                    EmployeeStatement = disciplineHistory.EmployeeStatement,
                    NatureOfViolationType = disciplineHistory.NatureOfViolationType1.Name,
                    ManagerId = disciplineHistory.Manager,
                    ManagerStatement = disciplineHistory.ManagerStatement,
                    LocationBranch = disciplineHistory.Location1?.Name ?? string.Empty,
                });
            }

            newEmployeeModel.Discipline.Add(new DisciplineHrs()
            {
                DateOfAction = discipline.DateOfAction,
                DateOfActionAppeals = discipline.DateOfActionAppeals,
                DateOfViolation = discipline.DateOfViolation,
                DisciplinaryActionType = discipline.DisciplinaryActionType1?.Name ?? string.Empty,
                EmployeeInAgreement = GetEmployeeInAgreement(discipline.EmployeeInAgreement),
                EmployeeStatement = discipline.EmployeeStatement,
                ManagerId = discipline.Manager,
                ManagerStatement = discipline.ManagerStatement,
                LocationBranch = discipline.Location1?.Name ?? string.Empty,
                NatureOfViolationType = discipline.NatureOfViolationType1.Name,
                RepresentativeName = discipline.RepresentativeName,
                RepresentativePresent = discipline.RepresentativePresent,
                DisciplineHistory = hist
            });

        }

        private string GetEmployeeInAgreement(int? disciplineHistoryEmployeeInAgreement)
        {
            if (disciplineHistoryEmployeeInAgreement == null) return "Unknown";
            if (disciplineHistoryEmployeeInAgreement == 0) return "No";
            if (disciplineHistoryEmployeeInAgreement == 1) return "Yes";
            if (disciplineHistoryEmployeeInAgreement == 2) return "No Response";
            return "Unknown";
        }


        private void GetWorkStatusHistory(EmployeeModelHrs model, Employee employee)
        {
            var workStatusHistoryRows = employee.WorkStatusHistory.Where(x => x.GCRecord == null).ToList();
            foreach (var workStatusHistory in workStatusHistoryRows)
            {
                model.WorkStatusHistory.Add(new WorkStatusHistoryHrs()
                {
                    ReasonForChangeType = workStatusHistory.WorkStatusHistoryReasonForChangeType?.Name ?? "Unknown",
                    Comments = workStatusHistory.Comments,
                    EffectiveDate = workStatusHistory.EffectiveDate,
                    FieldChanged = workStatusHistory.FieldChanged,
                    OldValue = workStatusHistory.OldValue,
                    NewValue = workStatusHistory.NewValue,
                    
                });
            }
        }

    }

    public class PerformanceRateOutcomeHrs
    {
        public string Comment { get; set; }
        public string RatingOutcomeType { get; set; }
    }

    public class PerformanceHistoryHrs
    {
        public DateTime? CreatedOn { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateOfNextReview { get; set; }
        public string GradeRatingType { get; set; }
        public string Notes { get; set; }
        public string PerformanceReviewType { get; set; }
        public string RecommendPayIncrease { get; set; }
        public string RecommendPromotion { get; set; }
        public int? Reviewer { get; set; }
    }

    public class WorkStatusHistoryHrs
    {
        public DateTime? EffectiveDate { get; set; }
        public string ReasonForChangeType { get; set; }
        public string Comments { get; set; }
        public string FieldChanged { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }

    public class EmployeeIssuedEquipmentHrs
    {
        public bool AccessCard { get; set; }
        public bool CellPhone { get; set; }
        public bool CompanyCar { get; set; }
        public bool CreditCard { get; set; }
        public bool Desktop { get; set; }
        public bool GasCard { get; set; }
        public bool Laptop { get; set; }
        public bool PPE { get; set; }
        public bool ShopKeys { get; set; }
        public bool Uniform { get; set; }
        public string Notes { get; set; }

    }

    public class EmployeeModelHrs
    {
        public int OldHrsId { get; set; }
        public string PayrollId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public byte[] GovernmentId { get; set; }
        public DateTime? OriginalHireDate { get; set; }
        public byte[] Birthday { get; set; }

        public string WorkEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Location { get; set; }
        public string Mobile { get; set; }
        public string Home { get; set; }
        public string CostCenterCode { get; set; }
        public string CostCenterName { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string WorkArea { get; set; } 
        public string Ethnicity { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Nationality { get; set; }
        public string Title { get; set; }
        public string ManagerPayrollId { get; set; }
        public string ManagerName { get; set; }
        public string Login { get; set; }

        public string KronosClockType { get; set; }

        public DateTime? PriorServiceDate { get; set; }
        public DateTime? LastRehireDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string TerminationCode { get; set; } = string.Empty;
        public string TerminationExplanation { get; set; } = string.Empty;
        public List<EmergencyContactModelHrs> EmergencyContacts { get; set; } = new List<EmergencyContactModelHrs>();

        public List<EmployeeVerificationModelHrs> EmployeeVerifications { get; set; } = new List<EmployeeVerificationModelHrs>();
        public string TimeTrackingAccrualProfile { get; set; }
        public DateTime? TimeTrackingAccrualProfileEffectiveDate { get; set; }

        public string EEO_Code { get; set; }
        public string RehireStatus { get; set; }
        public string KronosDepartment { get; set; }
        //public string KronosLaborLevel { get; set; }
        public string PayrollRegion { get; set; }
        public string BusinessRegion { get; set; }

        //public EmployeeIssuedEquipmentHrs IssuedEquipment { get; set; } = new EmployeeIssuedEquipmentHrs();
        public List<RequiredActivityHrs> RequiredActivities { get; set; } = new List<RequiredActivityHrs>();
        public List<WorkStatusHistoryHrs> WorkStatusHistory { get; set; } = new List<WorkStatusHistoryHrs>();
        public List<DisciplineHrs> Discipline { get; set; } = new List<DisciplineHrs>();
        public List<DisciplineHistoryHrs> DisciplineHistory { get; set; } = new List<DisciplineHistoryHrs>();

        public PerformanceHrs Performance { get; set; } = new PerformanceHrs();

        public string PhoneType { get; set; } = string.Empty;
        public string EvaluationTypeCode { get; set; }
        public string EvaluationTypeDescription { get; set; }
        public BenefitsModelHrs Benefits { get; set; }

        public MedicalInfo MedicalInfo { get; set; }
    }

    public class PerformanceHrs
    {
        public DateTime? DateOf { get; set; }
        public DateTime? DateOfNextReview { get; set; }
        public string GradeRatingType { get; set; }
        public string Notes { get; set; }
        public List<PerformanceHistoryHrs> PerformanceHistory { get; set; } = new List<PerformanceHistoryHrs>();
        public string PerformanceReviewType { get; set; }
        public List<PerformanceRateOutcomeHrs> RatingOutcomes { get; set; }
        public string RecommendPayIncrease { get; set; }
        public string RecommendPromotion { get; set; }
        public int? ReviewerId { get; set; }
    }

    public class DisciplineHistoryHrs
    {
        public DateTime? CreatedOn { get; set; }
        public DateTime? DateOfAction { get; set; }
        public DateTime? DateOfViolation { get; set; }
        public string DisciplinaryActionType { get; set; }
        public string EmployeeInAgreement { get; set; }
        public string EmployeeStatement { get; set; }
        public string NatureOfViolationType { get; set; }
        public int? ManagerId { get; set; }
        public string LocationBranch { get; set; }
        public string ManagerStatement { get; set; }
    }

    public class DisciplineHrs
    {
        public DateTime? DateOfAction { get; set; }
        public DateTime? DateOfActionAppeals { get; set; }
        public DateTime? DateOfViolation { get; set; }
        public string DisciplinaryActionType { get; set; }
        public string EmployeeInAgreement { get; set; }
        public string EmployeeStatement { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerStatement { get; set; }
        public string LocationBranch { get; set; }
        public string NatureOfViolationType { get; set; }
        public string RepresentativeName { get; set; }
        public bool? RepresentativePresent { get; set; }
        public List<DisciplineHistoryHrs> DisciplineHistory { get; set; } = new List<DisciplineHistoryHrs>();
    }

    public class RequiredActivityHrs
    {
        public string ColorCode { get; set; }
        public int DaysRemaining { get; set; }

        public string ActivityTypeCode { get; set; }

        public string Description { get; set; }

        public DateTime CompletionDeadline { get; set; }

        public DateTime RevCompDeadline { get; set; }

        public DateTime? DateCompleted { get; set; }

        public string CompletionStatus { get; set; }

        public bool EmployeeIsActive { get; set; }
        public bool HasSupportDocs { get; set; }

        public string TrainingCourseType { get; set; }
        public string TrainingCourseName { get; set; }

        public RequiredActivityHrs(RequiredActivity a)
        {
            
        }

    }

    public class EmployeeVerificationModelHrs
    {
        private static Encryption _encryption = Encryption.NewEncryption;
        public string DocumentNumber { get; set; }
        public string DocumentType { get; set; }
        public DateTime? DocumentExpiration { get; set; }
        public byte[] AdditionalGovernmentIdString { get; set; } = _encryption.Encrypt("");
        public byte[] AdditionalGovernmentIdNumber { get; set; } = _encryption.Encrypt(0);
    }

    public class EmergencyContactModelHrs
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Relationship { get; set; }
    }
}
