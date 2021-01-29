using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Ldap;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.HRS.Mongo.Tests.Locations;
using DAL.Vulcan.Mongo.Base.Queries;

namespace UTL.Hrs.EmployeeImport
{
    public class EmployeeImport : BaseImportData 
    {
        private readonly HelperEmployee _helperEmployee = new HelperEmployee();
        private readonly HelperProperties _helperProperties = new HelperProperties();
        private readonly MongoRawQueryHelper<PropertyType> _queryHelperPropertyType = new MongoRawQueryHelper<PropertyType>();
        private readonly MongoRawQueryHelper<PropertyValue> _queryHelperPropertyValue = new MongoRawQueryHelper<PropertyValue>();
        private readonly MongoRawQueryHelper<Location> _queryHelperLocation = new MongoRawQueryHelper<Location>();
        private readonly MongoRawQueryHelper<JobTitle> _queryHelperJobTitle = new MongoRawQueryHelper<JobTitle>();
        private readonly MongoRawQueryHelper<Employee> _queryHelperEmployee = new MongoRawQueryHelper<Employee>();
        private readonly MongoRawQueryHelper<HrsUser> _queryHelperHrsUser = new MongoRawQueryHelper<HrsUser>();
        private List<Location> _locations = new List<Location>();
        private EntityRef _entity;

        private class ManagerLink
        {
            public string PayrollId;
            public string ManagerName;
        }

        public EmployeeImport(string fileName) : base(fileName)
        {
        }

        public override void Execute()
        {

            ConfigureEntityInfo();
            var managerLinks = new List<ManagerLink>();
            var queryHelper = new MongoRawQueryHelper<Employee>();
            var onRow = 0;
            var defaultUserFilter = _queryHelperHrsUser.FilterBuilder.Where(x => x.FirstName == "Lori");
            var modifyUser = _queryHelperHrsUser.Find(defaultUserFilter).FirstOrDefault();
            foreach (DataRow dataRow in Data.Rows)
            {
                if ((dataRow["LastName"].ToString() != "Campbell") && (dataRow["FirstName"].ToString() != "Robert"))
                {
                    continue;
                }

                
                onRow++;
                RemoveExisting(dataRow, queryHelper);
                var model = _helperEmployee.GetNewEmployeeModel();


                //var filter = JobTitle.Helper.FilterBuilder.Where(x => x.Name == "Commercial Director");
                //var jobTitle = JobTitle.Helper.Find(filter).First();
                //model.JobTitle = jobTitle.AsJobTitleRef();

                model.ModifiedByUser = modifyUser.AsHrsUserRef();
                model.Entity = _entity;
                model.PayrollId = GetPayrollId(dataRow, onRow);
                model.GovernmentId = dataRow["GovernmentId"].ToString();
                model.LastName = dataRow["LastName"].ToString();
                model.FirstName = dataRow["FirstName"].ToString();
                model.MiddleName = dataRow["MiddleName"].ToString();
                model.PreferredName = dataRow["PreferredName"].ToString();

                if (dataRow["BirthDay"].ToString() != string.Empty)
                {
                    model.Birthday = GetDate(dataRow["BirthDay"].ToString());
                }

                model.WorkEmailAddress = dataRow["EmailWork"].ToString();
                model.PersonalEmailAddress = dataRow["EmailPersonal"].ToString();
                model.Address1 = dataRow["Address1"].ToString();
                model.Address2 = dataRow["Address2"].ToString();
                model.Address3 = dataRow["Address3"].ToString();
                model.City = dataRow["City"].ToString();
                model.State = dataRow["State"].ToString();
                model.PostalCode = dataRow["PostalCode"].ToString();
                model.Country = GetPropertyValueFor( "Country", dataRow["Country"].ToString(),true);

                var phoneHome = dataRow["PhoneHome"].ToString();
                var phoneMobile = dataRow["PhoneMobile"].ToString();
                var employeePhoneNumbers = new List<EmployeePhoneNumber>();
                if (phoneHome != string.Empty) employeePhoneNumbers.Add(new EmployeePhoneNumber("Home", phoneHome, model.Country.Code));
                if (phoneMobile != string.Empty) employeePhoneNumbers.Add(new EmployeePhoneNumber("Mobile", phoneMobile, model.Country.Code));

                foreach (var employeePhoneNumber in employeePhoneNumbers)
                {
                    model.PhoneNumbers.Add(new EmployeePhoneNumberModel(employeePhoneNumber));
                }

                var location = GetLocation(dataRow["Office"].ToString());
                if (location != null)
                {
                    model.Location = location;
                }
                model.ExternalLocationText = dataRow["Office"].ToString();
                model.CostCenterCode = GetPropertyValueFor("CostCenter", dataRow["CostCenterCode"].ToString(), false,
                    "<invalid>");
                model.EthnicityCode = GetPropertyValueFor("Ethnicity", dataRow["Ethnicity"].ToString(), false,
                    "<invalid>");
                model.GenderCode = GetPropertyValueFor("Gender", dataRow["Gender"].ToString(), false,
                    "<invalid>");
                model.MaritalStatusCode = GetPropertyValueFor("MaritalStatus", dataRow["MaritalStatus"].ToString(), false,
                    "<invalid>");
                model.CountryOfOriginCode = GetPropertyValueFor("CountryOfOrigin", dataRow["CountryOfOriginCode"].ToString(), false,
                    "<not specified>");
                model.CostCenterCode = GetPropertyValueFor("CostCenter", dataRow["CostCenterCode"].ToString(), false,
                    "<not specified>");
                model.NationalityCode = GetPropertyValueFor("Nationality", dataRow["Nationality"].ToString(), false,
                    "<not specified>");
                if (model.Location != null)
                {
                    if (model.Location.AsLocation().PayrollRegions.Any())
                    {
                        model.PayrollRegion = model.Location.AsLocation().PayrollRegions.First();
                    }
                }



                model.JobTitle = GetJobTitle(dataRow["JobTitle"].ToString());

                var manager = dataRow["Manager"].ToString();
                if (manager != string.Empty)
                {
                    managerLinks.Add(new ManagerLink()
                    {
                        PayrollId = model.PayrollId,
                        ManagerName = manager
                    });
                }

                model.Status1Code =
                    GetPropertyValueFor("Status1", dataRow["Status1"].ToString(), true, "<unspecified>");
                model.Status2Code =
                    GetPropertyValueFor("Status2", dataRow["Status2"].ToString(), true, "<unspecified>");

                if (dataRow["OriginalHireDate"].ToString() != string.Empty)
                    model.OriginalHireDate = GetDate(dataRow["OriginalHireDate"].ToString());

                if (dataRow["LastRehireDate"].ToString() != string.Empty)
                    model.LastRehireDate = GetDate(dataRow["LastRehireDate"].ToString());

                if (dataRow["ConfirmationDate"].ToString() != string.Empty)
                    model.ConfirmationDate = GetDate(dataRow["ConfirmationDate"].ToString());

                if (dataRow["PriorServiceDate"].ToString() != string.Empty)
                    model.PriorServiceDate = GetDate(dataRow["PriorServiceDate"].ToString());

                if (dataRow["TerminationDate"].ToString() != string.Empty)
                    model.TerminationDate = GetDate(dataRow["TerminationDate"].ToString());

                if (dataRow["TerminationCode"].ToString() != string.Empty)
                {
                    model.TerminationCode =
                        GetPropertyValueFor("TerminationCode", dataRow["TerminationCode"].ToString(), true);
                }

                if (dataRow["TerminationExplanation"].ToString() != string.Empty)
                {
                    model.TerminationExplanation = dataRow["TerminationExplanation"].ToString();
                }

                if (dataRow["RehireStatus"].ToString() != string.Empty)
                {
                    model.RehireStatusCode =
                        GetPropertyValueFor("RehireStatus", dataRow["RehireStatus"].ToString(), true);
                }

                var emergencyContactName = dataRow["EmergencyContactName"].ToString();
                var emergencyContactPhoneNumber = dataRow["EmergencyContactPhoneNumber"].ToString();
                var emergencyContactRelationship = dataRow["emergencyContactRelationship"].ToString();

                if ((emergencyContactName != string.Empty) && (emergencyContactPhoneNumber != String.Empty) &&
                    (emergencyContactRelationship != string.Empty))
                {
                    model.EmergencyContacts.Add(new EmergencyContact()
                    {
                        Name = emergencyContactName,
                        PhoneNumber = emergencyContactPhoneNumber,
                        Relationship = emergencyContactRelationship
                    });
                }

                // ignored for now
                if (dataRow["AdditionalGovernmentIdString"].ToString() != string.Empty)
                {
                }
                if (dataRow["AdditionalGovernmentIdNumber"].ToString() != string.Empty)
                {
                }

                model.KronosDepartmentCode = GetPropertyValueFor("KronosDepartment",
                    dataRow["KronosDepartment"].ToString(), false, "<unspecified>");


                _helperEmployee.SaveEmployee(model);
            }

            ResolveManager(managerLinks);

        }

        public DateTime GetDate(string dateValue)
        {
            //DateTimeFormatInfo usDtfi = new CultureInfo("en-US", false).DateTimeFormat;
            DateTimeFormatInfo ukDtfi = new CultureInfo("en-GB", false).DateTimeFormat;
            return Convert.ToDateTime(dateValue, ukDtfi);
        }

        private void ResolveManager(List<ManagerLink> managerLinks)
        {
            var filter = _queryHelperEmployee.FilterBuilder.Where(x => x.Manager == null);
            var employees = _queryHelperEmployee.Find(filter);
            var possibleManagers = employees.Select(x => x.AsEmployeeRef()).ToList();
            foreach (var employee in employees)
            {
                var managerLink = managerLinks.SingleOrDefault(x => x.PayrollId == employee.PayrollId);
                if (managerLink != null)
                {
                    var managerFound = possibleManagers.SingleOrDefault(x => x.GetFormalName() == managerLink.ManagerName);
                    if (managerFound != null)
                    {
                        employee.Manager = managerFound;
                        _queryHelperEmployee.Upsert(employee);
                    }
                }
            }
        }

        private JobTitleRef GetJobTitle(string jobTitle)
        {
            var filter = _queryHelperJobTitle.FilterBuilder.Where(x => x.Name == jobTitle);
            var jobTitleFound = _queryHelperJobTitle.Find(filter).FirstOrDefault();
            if (jobTitleFound != null) return jobTitleFound.AsJobTitleRef();

            if (jobTitle == string.Empty) jobTitle = "<unspecified>";

            jobTitleFound = new JobTitle()
            {
                Name = jobTitle
            };
            _queryHelperJobTitle.Upsert(jobTitleFound);
            return jobTitleFound.AsJobTitleRef();

        }

        private LocationRef GetLocation(string office)
        {
            if (_locations.Count == 0)
            {
                var locationFilter = _queryHelperLocation.FilterBuilder.Where(x => x.Entity.Id == _entity.Id);
                _locations = _queryHelperLocation.Find(locationFilter);
            }

            if ((office == string.Empty) || (_locations.All(x=>x.Office != office)))
            {
                office = "<unknown>";
            }

            var locationFound = _locations.SingleOrDefault(x => x.Office == office);

            return locationFound?.AsLocationRef();
        }

        private void ConfigureEntityInfo()
        {
            var queryHelper = new MongoRawQueryHelper<Entity>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Name == "Edgen Murray");
            var entity = queryHelper.Find(filter).First();
            _entity = entity.AsEntityRef();
        }

        private PropertyValueRef GetPropertyValueFor(string type, string code, bool addIfNotFound, string defaultValue = "")
        {

            var typeFilter = _queryHelperPropertyType.FilterBuilder.Where(x => x.Type == type);
            var propertyType = _queryHelperPropertyType.Find(typeFilter).Single();

            var valueFilter = _queryHelperPropertyValue.FilterBuilder.Where(x => x.Type == type && x.Code == code);
            var valueFound = _queryHelperPropertyValue.Find(valueFilter).FirstOrDefault();
            if (valueFound == null)
            {
                if (addIfNotFound && code != string.Empty)
                {
                    valueFound = new PropertyValue()
                    {
                        Active = true,
                        Type = type,
                        Code = code,
                        Entity = _entity,
                        Description = "",
                        Locations = new List<LocationRef>(),
                    };

                    _queryHelperPropertyValue.Upsert(valueFound);
                }
                else
                {
                    var valueFoundFilter =
                        _queryHelperPropertyValue.FilterBuilder.Where(x => x.Type == type && x.Code == defaultValue);
                    valueFound = _queryHelperPropertyValue.Find(valueFoundFilter).FirstOrDefault();

                    if (valueFound == null)
                    {
                        valueFound = new PropertyValue()
                        {
                            Active = false,
                            Type = type,
                            Code = defaultValue,
                            Entity = _entity,
                            Description = "",
                            Locations = new List<LocationRef>(),
                        };
                        _queryHelperPropertyValue.Upsert(valueFound);
                    }

                }

            }
            return valueFound.AsPropertyValueRef();
        }

        private string GetPayrollId(DataRow dataRow, int onRow)
        {
            var payrollId = dataRow["PayrollId"].ToString();
            if (payrollId == String.Empty)
            {
                payrollId = "EM" + dataRow["LastName"] + dataRow["FirstName"].ToString().Substring(1, 1) + onRow;
            }

            return payrollId;
        }

        private static void RemoveExisting(DataRow dataRow, MongoRawQueryHelper<Employee> queryHelper)
        {
            var payrollId = dataRow["PayrollId"].ToString();
            var filter = queryHelper.FilterBuilder.Where(x => x.PayrollId == payrollId);
            queryHelper.DeleteOne(filter);
        }
    }
    
}