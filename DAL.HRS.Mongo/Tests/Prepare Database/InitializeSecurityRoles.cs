using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Security;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.Tests.Prepare_Database
{
    [TestFixture()]
    public class InitializeSecurityRoles
    {
        private HelperSecurity _helperSecurity;
        private Helpers.HelperUser _helperUser;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
            _helperSecurity = new HelperSecurity();
            _helperUser = new Helpers.HelperUser();
        }

        [Test]
        public void CreateHrsRoles()
        {
            //ClearData();
            //AddModules();

            //CreateHrsSystemAdminRole();
            //CreateHseSystemAdminRole();

            //CreateHrIAdmin();
            //CreateHrIIManager();
            //CreateHrIIICoordinator();
            //CreateHrIIIMCoordinator();
            //CreateHrIVRegionHeads();
            //CreateHrsVSupMgr();
            //CreateHrsVMSupMgr();
            //CreateHrsVIAssistants();
            //CreateHrsVIIEmployee();
            //CreateHrVIIIExecutive();
            //CreateDashboards();
            //AddSystemAdminUsers();
            CreateHrsVMQuality();
        }


        [Test]
        public void CreateHrsSystemAdminRole()
        {

            var roleTypeRef = _helperSecurity.DefineNewRoleType("HrsSystemAdmin", true, false , true);
            var roleType = roleTypeRef.AsSecurityRoleType();
            roleType.IsHseRole = false;
            roleType.SaveToDatabase();
            roleTypeRef = roleType.AsSecurityRoleTypeRef();

            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeRef.Name);

            var allModules = _helperSecurity.GetAllHrsModuleTypes();
            foreach (var systemModuleModel in allModules.Where(x=>x.IsHrsModule).ToList())
            {
                var module = _helperSecurity.GetSystemModuleModel(systemModuleModel.Id);
                module.AllAccess();
                roleModel.Modules.Add(module);
            }

            roleModel.DirectReportsOnly = false;
            _helperSecurity.SaveRole(roleModel);
        

        }

        public void CreateHseSystemAdminRole()
        {

            var roleTypeRef = _helperSecurity.DefineNewRoleType("HseSystemAdmin", false, true, true);
            var roleType = roleTypeRef.AsSecurityRoleType();
            roleType.IsHseRole = true;
            roleType.SaveToDatabase();
            roleTypeRef = roleType.AsSecurityRoleTypeRef();

            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeRef.Name);

            var allModules = _helperSecurity.GetAllHrsModuleTypes();
            foreach (var systemModuleModel in allModules.Where(x => x.IsHseModule).ToList())
            {
                var module = _helperSecurity.GetSystemModuleModel(systemModuleModel.Id);
                module.AllAccess();
                roleModel.Modules.Add(module);
            }
            roleModel.DirectReportsOnly = false;

            _helperSecurity.SaveRole(roleModel);


        }

        [Test]
        public void AddModules()
        {
            var modulesHrs = new List<string>()
            {
                "Employee Details",
                "Work Status",
                "Emergency Contacts",
                "Education Certification",
                "Employment Verification",
                "Issued Equipment",
                "Supporting Documents",
                "Compensation",
                "Pay Grades & Position Control",
                "Performance",
                "Discipline",
                "Required Activities",
                "TrainingEvent",
                "TrainingEvent Batch Processing",
                "Benefits",
                "Time Tracking",
                "Personal & Confidential-Medical",
                "Personal & Confidential-Regulatory",

            };

            foreach (var moduleName in modulesHrs)
            {
                _helperSecurity.DefineNewModuleType(moduleName, true, false);
            }

            var modulesHse = new List<string>()
            {
                "Incident Reporting Screen",
                "Investigation",
                "Medical",
                "Environmental",
                "Observations",
                "BBS-Observations"
            };


            foreach (var moduleName in modulesHse)
            {
                _helperSecurity.DefineNewModuleType(moduleName, false, true);
            }

            _helperSecurity.DefineNewModuleType("HR Dashboard", true, false);
            _helperSecurity.DefineNewModuleType("HSE Dashboard", true, false);
            _helperSecurity.DefineNewModuleType("Executive Dashboard", true, false);

        }


        private void ClearData()
        {
            new RepositoryBase<HrsUser>().RemoveAllFromCollection();
            new RepositoryBase<HrsUserToken>().RemoveAllFromCollection();
            new RepositoryBase<SecurityRole>().RemoveAllFromCollection();
            //new RepositoryBase<SystemModule>().RemoveAllFromCollection();
            new RepositoryBase<SecurityRoleType>().RemoveAllFromCollection();
            new RepositoryBase<SystemModuleType>().RemoveAllFromCollection();
        }

        [Test]
        public void AddSystemAdminUsers()
        {

            var roleTypeHrs = _helperSecurity.GetRoleTypeForName("HrsSystemAdmin");
            var roleTypeHse = _helperSecurity.GetRoleTypeForName("HseSystemAdmin");

            var repLdap = new RepositoryBase<LdapUser>();
            var testUsers = new List<LdapUser>();

            testUsers.Add(repLdap.AsQueryable().Single(
                x => x.Person.FirstName == "Marc" && x.Person.LastName == "Pike"));
            testUsers.Add(repLdap.AsQueryable().Single(
                x => x.Person.FirstName == "Isidro" && x.Person.LastName == "Gallegos"));
            testUsers.Add(repLdap.AsQueryable().Single(
                x => x.Person.FirstName == "Shannen" && x.Person.LastName == "Reese"));
            testUsers.Add(repLdap.AsQueryable().Single(
                x => x.Person.FirstName == "Stanton" && x.Person.LastName == "Fraser"));
            //testUsers.Add(repLdap.AsQueryable().Single(
            //    x => x.Person.FirstName == "Anu" && x.Person.LastName == "Madhavan"));

            var queryHelperEmployee = new MongoRawQueryHelper<Employee>();
            var newHrsUsersList = new List<NewHrsUserModel>();
            foreach (var testUser in testUsers)
            {

                var employeeFilter = queryHelperEmployee.FilterBuilder.Where(x =>
                    x.FirstName == testUser.FirstName && x.LastName == testUser.LastName);

                var employee = queryHelperEmployee.Find(employeeFilter).FirstOrDefault();
                if (employee == null)
                {
                    Console.WriteLine($"Could not find {testUser.FullName}");
                    continue;
                }
                newHrsUsersList.Add(new NewHrsUserModel()
                {
                    LdapUser = testUser.AsLdapUserRef(),
                    Employee = employee.AsEmployeeRef()
                });
            }

            var currentHrsUsers = new MongoRawQueryHelper<HrsUser>().GetAll();
            foreach (var newHrsUser in newHrsUsersList)
            {
                if (currentHrsUsers.All(x => x.Employee.Id != newHrsUser.Employee.Id))
                {
                    _helperSecurity.AddUser(newHrsUser.Employee.Id.ToString(), newHrsUser.LdapUser.Id, roleTypeHrs.Id, roleTypeHse.Id, Entity.GetRefByName("Howco"));
                }
            }


        }

        [Test]
        public void CreateDashboards()
        {
            //var repModule = new RepositoryBase<SystemModule>();


            var hrDashModuleType = _helperSecurity.GetHrsModuleTypeForName("HR Dashboard");
            var hseDashModuleType = _helperSecurity.GetHrsModuleTypeForName("HSE Dashboard");
            var execDashModuleType = _helperSecurity.GetHrsModuleTypeForName("Executive Dashboard");



            var hrDashboard = new SystemModule(hrDashModuleType) {View = true};
            //repModule.Upsert(hrDashboard);

            var hseDashboard = new SystemModule(hseDashModuleType) {View = true};
            //repModule.Upsert(hseDashboard);

            var executiveDashboard = new SystemModule(execDashModuleType) {View = true};
            //repModule.Upsert(executiveDashboard);

            // HR I - Administrator
            {
                var roleBase = _helperSecurity.GetSecurityRoleModelForName("HR I - Administrator");

                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(hrDashModuleType)));
                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(hseDashModuleType)));
                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(execDashModuleType)));
                roleBase.DirectReportsOnly = false;

                _helperSecurity.SaveRole(roleBase);
            }

            // HR II - Manager
            {
                var roleBase = _helperSecurity.GetSecurityRoleModelForName("HR II - Manager");

                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(hrDashModuleType)));
                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(hseDashModuleType)));
                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(execDashModuleType)));
                roleBase.DirectReportsOnly = false;

                _helperSecurity.SaveRole(roleBase);
            }

            // HR III - Coordinator
            {
                var roleBase = _helperSecurity.GetSecurityRoleModelForName("HR III - Coordinator");

                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(hrDashModuleType)));
                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(hseDashModuleType)));
                roleBase.DirectReportsOnly = false;

                _helperSecurity.SaveRole(roleBase);
            }

            // HR IIIM - Coordinator
            {
                var roleBase = _helperSecurity.GetSecurityRoleModelForName("HR IIIM - Coordinator");

                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(hrDashModuleType)));
                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(hseDashModuleType)));
                roleBase.DirectReportsOnly = false;

                _helperSecurity.SaveRole(roleBase);
            }

            // HR VIII - Executive
            {
                var roleBase = _helperSecurity.GetSecurityRoleModelForName("HR VIII - Executive");

                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(execDashModuleType)));
                roleBase.DirectReportsOnly = false;

                _helperSecurity.SaveRole(roleBase);
            }

            // SystemAdmin
            {
                var roleBase = _helperSecurity.GetSecurityRoleModelForName("HrsSystemAdmin");

                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(hrDashModuleType)));
                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(hseDashModuleType)));
                roleBase.Modules.Add(new SystemModuleModel(SystemModule.Create(execDashModuleType)));
                roleBase.DirectReportsOnly = false;

                _helperSecurity.SaveRole(roleBase);
            }

        }


        [Test]
        public void CreateHrIAdmin()
        {

            var roleTypeDef = _helperSecurity.DefineNewRoleType("HR I - Administrator", true, false, true);
            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeDef.Name);
            roleModel.DirectReportsOnly = false;
            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Employee Details");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                ////module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Work Status");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                ////module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Emergency Contacts");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                ////module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Education Certification");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                ////module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Employment Verification");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Issued Equipment");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Supporting Documents");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Compensation");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Pay Grades & Position Control");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Performance");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Discipline");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Required Activities");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent Batch Processing");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Benefits");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Time Tracking");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Personal & Confidential-Medical");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Personal & Confidential-Regulatory");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }
            
            _helperSecurity.SaveRole(roleModel);
        }

        [Test]
        public void CreateHrIIManager()
        {
            var roleTypeDef = _helperSecurity.DefineNewRoleType("HR II - Manager", true,false,true);
            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeDef.Name);
            roleModel.DirectReportsOnly = false;

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Employee Details");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Work Status");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Emergency Contacts");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Education Certification");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Employment Verification");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Issued Equipment");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Supporting Documents");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Compensation");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Pay Grades & Position Control");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Performance");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Discipline");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Required Activities");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent Batch Processing");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Benefits");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Time Tracking");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Personal & Confidential-Medical");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Personal & Confidential-Regulatory");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            _helperSecurity.SaveRole(roleModel);
        }

        private void CreateHrIIICoordinator()
        {
            var roleTypeDef = _helperSecurity.DefineNewRoleType("HR III - Coordinator", true, false,true);
            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeDef.Name);
            roleModel.DirectReportsOnly = false;

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Employee Details");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Work Status");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Emergency Contacts");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Education Certification");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Employment Verification");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Issued Equipment");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Supporting Documents");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Compensation");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Pay Grades & Position Control");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Performance");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Discipline");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Required Activities");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent Batch Processing");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Benefits");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Time Tracking");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Personal & Confidential-Medical");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Personal & Confidential-Regulatory");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite().CanViewAll();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            _helperSecurity.SaveRole(roleModel);

        }

        private void CreateHrIIIMCoordinator()
        {
            var roleTypeDef = _helperSecurity.DefineNewRoleType("HR IIIM - Coordinator", true, false,true);
            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeDef.Name);
            roleModel.DirectReportsOnly = false;

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Employee Details");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Work Status");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Emergency Contacts");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Education Certification");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Employment Verification");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Issued Equipment");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Supporting Documents");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Performance");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Discipline");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Required Activities");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent Batch Processing");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Benefits");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Time Tracking");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Personal & Confidential-Medical");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Personal & Confidential-Regulatory");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            _helperSecurity.SaveRole(roleModel);

        }

        private void CreateHrIVRegionHeads()
        {
            var roleTypeDef = _helperSecurity.DefineNewRoleType("HR IV - Regional Heads", true, false,true);
            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeDef.Name);
            roleModel.DirectReportsOnly = false;

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Compensation");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Pay Grades & Position Control");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Performance");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Discipline");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Required Activities");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Time Tracking");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            _helperSecurity.SaveRole(roleModel);

        }

        private void CreateHrsVSupMgr()
        {
            var roleTypeDef = _helperSecurity.DefineNewRoleType("HR V - Regional Supervisor/Manager", true, false,true);
            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeDef.Name);
            roleModel.DirectReportsOnly = true;

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Compensation");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Pay Grades & Position Control");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Performance");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Discipline");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Required Activities");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Time Tracking");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            _helperSecurity.SaveRole(roleModel);

        }

        private void CreateHrsVMSupMgr()
        {
            var roleTypeDef = _helperSecurity.DefineNewRoleType("HR VM - Regional Supervisor/Manager", true, false,true);
            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeDef.Name);
            roleModel.DirectReportsOnly = true;


            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Performance");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Discipline");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Required Activities");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Time Tracking");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            _helperSecurity.SaveRole(roleModel);

        }

        private void CreateHrsVMQuality()
        {
            var roleTypeDef = _helperSecurity.DefineNewRoleType("HR VM - Quality", true, false, true);
            var role = _helperSecurity.AddNewRole(roleTypeDef);
            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeDef.Name);
            roleModel.DirectReportsOnly = true;

            
            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Training - Quality");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            

            _helperSecurity.SaveRole(roleModel);

        }

        private void CreateHrsVIAssistants()
        {
            var roleTypeDef = _helperSecurity.DefineNewRoleType("HR VI - Assistants", true, false,true);
            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeDef.Name);
            roleModel.DirectReportsOnly = true;

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent Batch Processing");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadWrite();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }


            _helperSecurity.SaveRole(roleModel);

        }

        private void CreateHrsVIIEmployee()
        {
            var roleTypeDef = _helperSecurity.DefineNewRoleType("HR VII - Employee", true, false,true);
            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeDef.Name);
            roleModel.DirectReportsOnly = true;

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Required Activities");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Benefits");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Time Tracking");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.ReadOnly();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            _helperSecurity.SaveRole(roleModel);
        }

        private void CreateHrVIIIExecutive()
        {
            var roleTypeDef = _helperSecurity.DefineNewRoleType("HR VIII - Executive", true, false,true);
            var roleModel = _helperSecurity.GetSecurityRoleModelForName(roleTypeDef.Name);
            roleModel.DirectReportsOnly = true;

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Employee Details");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Work Status");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Emergency Contacts");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Education Certification");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Employment Verification");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Issued Equipment");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Supporting Documents");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Compensation");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Pay Grades & Position Control");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Performance");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Discipline");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Required Activities");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("TrainingEvent Batch Processing");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Benefits");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Time Tracking");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Personal & Confidential-Medical");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            {
                var moduleType = _helperSecurity.GetHrsModuleTypeForName("Personal & Confidential-Regulatory");
                var module = _helperSecurity.GetSystemModuleModel(moduleType.Id);
                module.AllAccess();
                //module = _helperSecurity.SaveModule(module);
                roleModel.Modules.Add(module);
            }

            _helperSecurity.SaveRole(roleModel);
        }


    }
}