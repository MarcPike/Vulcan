using System;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Tests;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using NUnit.Framework;

namespace UTL.Hrs.EmployeeImport
{
    [TestFixture]
    public class ImportActions
    {
        [SetUp]
        public void SetUp()
        {
            
            EnvironmentSettings.HrsDevelopment();
        }

        //[Test]
        //public static void ImportDemographics()
        //{
        //    var importer = new EmployeeImport(@"D:\Code\Vulcan.CRM\UTL.Hrs.EmployeeImport\ImportFolder\HRS Demographic Import - EM 12.04.2019 copy.xlsx");
        //    importer.Execute();
        //}

        [Test]
        public static void ImportCompensation()
        {
            var importer = new CompensationImport(@"C:\src\tfs\Vulcan Technology\UTL.Hrs.EmployeeImport\ImportFolder\Compensation Import.xlsx");
            importer.Execute();
        }



        //[Test]
        //public void RemoveInvalidProperties()
        //{
        //    var propertyTypes = new MongoRawQueryHelper<PropertyType>().GetAll();
        //    var queryHelper = new MongoRawQueryHelper<PropertyValue>();
        //    foreach (var propertyType in propertyTypes)
        //    {
        //        var filterThisProperty = queryHelper.FilterBuilder.Where(x => x.Type == propertyType.Type && x.Code == "<unspecified>");
        //        var unspecifiedPropertyValues = queryHelper.Find(filterThisProperty);
        //        if (unspecifiedPropertyValues.Count > 1)
        //        {
        //            Console.WriteLine(propertyType.Type +" has "+unspecifiedPropertyValues.Count);
        //        }
        //    }
        //}

        //[Test]
        //public void AddBackKronosDepartmentUnspecified()
        //{
        //    var propertyValue = PropertyBuilder.New("KronosDepartment", "Kronos Department", "<unspecified>", "");
        //    if (propertyValue != null)
        //    {
        //        var queryHelper = new MongoRawQueryHelper<Employee>();
        //        foreach (var employee in queryHelper.GetAll())
        //        {
        //            employee.KronosDepartmentCode = propertyValue;
        //            queryHelper.Upsert(employee);
        //        }
        //    }
        //}

        //[Test]
        //public void SetEthnicityUnspecified()
        //{
        //    var propertyValue = PropertyBuilder.New("Ethnicity", "Type of Ethnicity", "<unspecified>", "");
        //    if (propertyValue != null)
        //    {
        //        var queryHelper = new MongoRawQueryHelper<Employee>();
        //        foreach (var employee in queryHelper.GetAll())
        //        {
        //            if (employee.EthnicityCode == null)
        //            {
        //                employee.EthnicityCode = propertyValue;
        //                queryHelper.Upsert(employee);
        //            }
        //        }
        //    }
        //}

        //[Test]
        //public void FixKronosDepartment()
        //{
        //    var queryHelperPropertyValues = new MongoRawQueryHelper<PropertyValue>();
        //    var filterProperties = queryHelperPropertyValues.FilterBuilder.Where(x => x.Type == "KronosDepartment" && x.Code == "<unspecified>");
        //    var unspecifiedProperties = queryHelperPropertyValues.Find(filterProperties).ToList();
        //    var firstProperty = unspecifiedProperties.First();

        //    var removeThese = unspecifiedProperties.Where(x => x.Id != firstProperty.Id).ToList();
        //    foreach (var propertyValue in removeThese)
        //    {
        //        queryHelperPropertyValues.DeleteOne(propertyValue.Id);
        //    }

        //    var queryHelperEmployees = new MongoRawQueryHelper<Employee>();
        //    foreach (var employee in queryHelperEmployees.GetAll())
        //    {
        //        employee.KronosDepartmentCode = firstProperty.AsPropertyValueRef();
        //        queryHelperEmployees.Upsert(employee);
        //    }

        //}

        //[Test]
        //public void CheckForMissingEntity()
        //{
        //    var queryHelper = new MongoRawQueryHelper<HrsUser>();
        //    var filter = queryHelper.FilterBuilder.Where(x => x.Entity == null);
        //    var usersWithNoEntity = queryHelper.Find(filter);
        //    foreach (var hrsUser in usersWithNoEntity)
        //    {
        //        Console.WriteLine(hrsUser.FullName);
        //    }
        //}

        //[Test]
        //public void RecreateHrsUserEmployeeLocations()
        //{
        //    //EnvironmentSettings.HrsDevelopment();
        //    var queryHelper = new MongoRawQueryHelper<HrsUser>();
        //    var allUsers = queryHelper.GetAll();
        //    foreach (var hrsUser in allUsers)
        //    {
        //        hrsUser.Location = hrsUser.Location.AsLocation().AsLocationRef();
        //        queryHelper.Upsert(hrsUser);
        //    }
        //}

        //[Test]
        //public void AddSpacesToLocationOffice()
        //{
        //    var queryHelper = new MongoRawQueryHelper<Location>();
        //    var filter = queryHelper.FilterBuilder.Where(x => x.Entity.Name == "Edgen Murray");
        //    var edgenLocations = queryHelper.Find(filter).ToList();
        //    foreach (var edgenLocation in edgenLocations)
        //    {
        //        edgenLocation.Office = edgenLocation.Office.Replace("EM-", "EM - ");
        //        edgenLocation.Office = edgenLocation.Office.Replace("HSP-", "HSP - ");
        //        queryHelper.Upsert(edgenLocation);
        //    }
        //}

        //[Test]
        //public void RemoveInvalidLocations()
        //{
        //    var queryHelper = new MongoRawQueryHelper<Location>();
        //    var locations = queryHelper.GetAll();
        //    var badLocations = locations.Where(x => x.Coid == "<unknown>").ToList();
        //    foreach (var badLocation in badLocations)
        //    {
        //        queryHelper.DeleteOne(badLocation.Id);
        //    }
        //}

        //[Test]
        //public void GetEmployeesWithNotLocation()
        //{
        //    var queryHelper = new MongoRawQueryHelper<Employee>();
        //    var filter = queryHelper.FilterBuilder.Where(x => x.Location == null);
        //    var project =
        //        queryHelper.ProjectionBuilder.Expression(x => new
        //            { x.Id, x.FirstName, x.LastName, x.ExternalLocationText });
        //    var results = queryHelper.FindWithProjection(filter, project);
        //    foreach (var emp in results)
        //    {
        //        Console.WriteLine(ObjectDumper.Dump(emp));
        //    }
        //}

        //[Test]
        //public void FetchPropertiesForSpreadsheet()
        //{

        //    #region All Types
        //    /*
        //        //var propertyTypes = new MongoRawQueryHelper<PropertyType>().GetAll();
        //        //foreach (var propertyType in propertyTypes)
        //        //{
        //        //    Console.WriteLine($"{propertyType.Id} - {propertyType.Type}");
        //        //}

        //        5cf91da4ae7ad539fc27b529 - CostCenter
        //        5cf91da5ae7ad539fc27b531 - EEO
        //        5cf91da5ae7ad539fc27b534 - Country
        //        5cf91da5ae7ad539fc27b537 - CountryOfOrigin
        //        5cf91da5ae7ad539fc27b53a - Ethnicity
        //        5cf91da5ae7ad539fc27b540 - RehireStatus
        //        5cf91da5ae7ad539fc27b543 - KronosDepartment
        //        5cf91da5ae7ad539fc27b549 - MaritalStatus
        //        5cf91da5ae7ad539fc27b54c - Nationality
        //        5cf91da5ae7ad539fc27b54f - PayrollRegion
        //        5cf91da5ae7ad539fc27b552 - BusinessRegion
        //        5cf91da5ae7ad539fc27b555 - Status1
        //        5cf91da5ae7ad539fc27b558 - Status2
        //        5cf91da5ae7ad539fc27b55b - TerminationCode
        //        5d161270ae7ad531d4aa4d29 - BonusType
        //        5d161271ae7ad531d4aa4d44 - BonusSchemeType
        //        5d161271ae7ad531d4aa4d47 - PayFrequencyType
        //        5d161271ae7ad531d4aa4d50 - CurrencyType
        //        5d161271ae7ad531d4aa4d53 - IncreaseType
        //        5d161271ae7ad531d4aa4d59 - PayGradeType
        //        5d161271ae7ad531d4aa4d5c - PayRateType
        //        5d161272ae7ad531d4aa4f91 - CompensationType
        //        5d2607e90de3c60e7c715902 - DocumentType
        //        5d263729095f5832dc03d832 - WorkHistoryReasonForChange
        //        5d2648a1095f5842a847de78 - WorkArea
        //        5d2727ba095f5842f4e52df3 - EmployeeVerificationDocumentType
        //        5d2c6a05095f58360cd882fb - NatureOfViolationType
        //        5d2c6a05095f58360cd882fd - DisciplinaryActionType
        //        5d2c6a05095f58360cd882ff - EmployeeInAgreement
        //        5d2c6a07095f58360cd8856d - PerformanceGradeRatingType
        //        5d2c6a07095f58360cd8856f - PerformanceReviewType
        //        5d2c6a08095f58360cd88571 - PerformanceRecommendPayIncrease
        //        5d2c6a08095f58360cd88573 - RecommendPromotion
        //        5d2c6a08095f58360cd8858d - PerformanceRateOutcomeType
        //        5d2dc3b6095f584d1c4fa5d4 - PhoneType
        //        5d39faac095f58086c4c8f81 - RepresentativePresent
        //        5d3f3f56095f5846d447ed6d - BenefitPlanType
        //        5d3f3f56095f5846d447ed6f - BenefitOptionType
        //        5d3f3f56095f5846d447ed71 - BenefitCoverageType
        //        5d3f3f56095f5846d447ed73 - BenefitStatusChangeType
        //        5d3f3f57095f5846d447eead - DependentRelationshipType
        //        5d3f3f57095f5846d447eeb1 - BeneficiaryPercentageType
        //        5d3f3f57095f5846d447eeb3 - DependentDesignationType
        //        5d4832ad095f5840f45362eb - EmailType
        //        5d4b1b9b095f584a5051608b - BenefitsDocumentType
        //        5d517d16095f584b58e3329e - KronosLaborLevel
        //        5d519f68095f58375464012b - CountryPhone
        //        5d52d3000de3c620b4a05bfe - BusinessUnit
        //        5d52d5050de3c620b4a0fd4d - CompanyNumber
        //        5d555eeb095f5819bc2d6529 - Bonus Eligible Type
        //        5d5597c7095f5840e86b2c65 - CompensationDocumentType
        //        5d55aaef095f585fc43dd58e - PerformanceDocumentType
        //        5d55b01d095f580304fd1cfa - DisciplineDocumentType
        //        5d55b372095f5855ecc4b3c5 - EmployeeDocumentType
        //        5d8a26d1095f58474c4b5065 - TrainingCourseType
        //        5d8a26d1095f58474c4b5067 - TrainingGroupClassification
        //        5d8a26d1095f58474c4b5069 - TrainingVenueType
        //        5d8a30d4095f584cc861bb01 - RequiredActivityType
        //        5d8a30d4095f584cc861bb03 - RequiredActivityCompleteStatus
        //        5d8cd75c095f5855f0db2c0f - TrainingAttendeeDocumentType
        //        5d8cdd9c095f582f7008a622 - TrainingEventDocumentType
        //        5d93a086095f583fec201d9a - KronosPayRuleType
        //        5da7273e095f594e28126931 - Employee DetailsDocumentType
        //        5da72793095f595a706d163e - TrainingDocumentType
        //        5db06caf095f5902e87223d8 - HseObservationClass
        //        5db06caf095f5902e87223da - HseObservationType
        //        5db06caf095f5902e87223dc - HseObservationStatus
        //        5db1ff08095f5945f82b5634 - CoshhExposureType
        //        5db1ff08095f5945f82b563b - Department
        //        5db1ff08095f5945f82b563e - RiskRating
        //        5db1ff09095f5945f82b5640 - CoshhProductType
        //        5dc17a92095f5924c807b854 - RequiredActivityDocumentType
        //        5dc17a93095f5924c807b87f - Required ActivitiesDocumentType
        //        5dcc187e0de3c6235cfdc4cb - Test Property
        //        5dcc7a5f0de3c6203473a454 - AwsEligible
        //        5dcd84200de3c61ae8d1128a - Gender
        //        5ddea537095f5966105501e4 - DrugTestType
        //        5ddea537095f5966105501e7 - DrugTestResult
        //        5ddea644095f5937ec62c086 - FirstAidTreatmentType
        //        5ddea6de095f593cec28b674 - MedicalTreatmentAdministeredType
        //        5ddea6f7095f593cec28b6a9 - Type
        //        5ddea6f7095f593cec28b6ac - Body contact type
        //        5ddea6f7095f593cec28b6b5 - LackOfControlType
        //        5ddea6f7095f593cec28b6bb - MedicalWorkStatusType
        //        5ddea6f7095f593cec28b6ca - Incident Reporting ScreenDocumentType
        //        5ddea870095f5941e468a5d5 - EquipmentCode
        //        5ddea870095f5941e468a5d8 - EquipmentType
        //        5ddea870095f5941e468a661 - JobFactorType
        //        5ddebe4b095f5904bc11f2c8 - BodyPartType
        //        5ddebe4b095f5904bc11f2cb - BodyContactType
        //        5ddebe4b095f5904bc11f2ce - ContactWithType
        //        5de55ac8095f59527411ea7c - NearMissType
        //        5de55ac8095f59527411ea7f - SeverityType
        //        5de57596095f593cf091b28a - IncidentType
        //        5de57596095f593cf091b28d - NearMissTypeCode
        //        5de57596095f593cf091b290 - SeverityTypeCode
        //        5dea97ba0de3c617fcfef675 - IncidentStatusType
        //     */
        //    #endregion

        //    var helperProperties = new HelperProperties();
        //    var propertyValues =
        //        helperProperties.GetOnlyActivePropertyValuesForProperty("PayGradeType",
        //            Entity.GetRefByName("Howco").Id);
        //    foreach (var propertyValue in propertyValues)
        //    {
        //        Console.WriteLine($"{propertyValue.Code}, {propertyValue.Description}");
        //    }
        //}


    }
}