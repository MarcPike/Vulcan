using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Currency;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Security;
using DAL.Vulcan.Mongo.Importers;
using DAL.WindowsAuthentication.MongoDb;
using MongoDB.Bson;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;
using ObjectDumper = DAL.Vulcan.Test.ObjectDumper;

namespace DAL.Vulcan.NUnit.Tests.DatabaseUtils.Prepare_Database.QA
{
    [TestFixture]
    public class ConfigureDatabase
    {
        private RepositoryBase<CompanyGroup> _companyGroupRep;

        [SetUp]
        public void Initialize()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            _companyGroupRep = new RepositoryBase<CompanyGroup>();
        }

        [Test]
        public void Execute()
        {
            AddApplication();
            CurrencyType.GenerateDefaults();
            Location.GenerateDefaults();
            var importer = new CompanyImporter();
            importer.Execute("USA");
            importer.Execute("EUR");
            importer.Execute("SIN");
            importer.Execute("MSA");
            importer.Execute("DUB");
            CreateAllCompanyGroups();
            LoadLdapUsers();
            InitializeUsers();
        }

        //[Test]
        public void LoadLdapUsers()
        {
            UserAuthentication.RefreshAll();
        }

        //[Test]
        public void FindUser()
        {
            var rep = new RepositoryBase<LdapUser>();
            var sdeatons = rep.AsQueryable().Where(x => x.UserName.Contains("Papandrea")).ToList();
            foreach (var ldapUser in sdeatons)
            {
                Console.Write(Test.ObjectDumper.Dump(ldapUser));
            }
        }

        //[Test]
        public void InitializeUsers()
        {
            var userRep = new RepositoryBase<LdapUser>();

            var mpike = userRep.AsQueryable().FirstOrDefault(x => x.NetworkId == "mpike");

            var newCrmUser = new Mongo.DocClass.CRM.CrmUser(mpike, CrmUserType.Manager) { IsAdmin = true };
            newCrmUser.SaveToDatabase();

            var isidro = userRep.AsQueryable().FirstOrDefault(x => x.NetworkId == "igallego");
            newCrmUser = new Mongo.DocClass.CRM.CrmUser(isidro, CrmUserType.Manager) { IsAdmin = true };
            newCrmUser.SaveToDatabase();
        }

        //[Test]
        public void AddApplication()
        {
            new RepositoryBase<Application>().Upsert(new Application()
            {
                Name = "vulcancrm"
            });
        }

        //[Test]
        public void ReloadUsers()
        {
            LoadLdapUsers();
            InitializeUsers();
        }

        //[Test]
        public void CreateAllCompanyGroups()
        {
            _companyGroupRep.RemoveAllFromCollection();
            CreateAlliances();
            CreateNonAlliances();
            LoadCompaniesForAlliances();
            LoadCompaniesForNonAlliances();

        }

        private void LoadCompaniesForAlliances()
        {
            var companyGroups = _companyGroupRep.AsQueryable()
                .Where(x => x.IsAlliance && x.Branch != String.Empty && x.NameContains != String.Empty)
                .ToList();
            foreach (var companyGroup in companyGroups)
            {
                companyGroup.RefreshAllianceCompanyList(_companyGroupRep);
            }
        }

        private void LoadCompaniesForNonAlliances()
        {
            var companyGroups = _companyGroupRep.AsQueryable()
                .Where(x => x.IsAlliance == false && x.Branch != String.Empty && x.ChildGroupsIds.Count == 0)
                .ToList();
            foreach (var companyGroup in companyGroups)
            {
                companyGroup.RefreshNonAllianceCompanyList(_companyGroupRep);
            }
        }

        [Test]
        public void CreateAlliances()
        {
            if (_companyGroupRep.AsQueryable().SingleOrDefault(x => x.Name == "Alliances") != null) return;

            var allianceGroup = new CompanyGroup()
            {
                Name = "Alliances",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                ParentObjectId = ObjectId.Empty
            };
            allianceGroup.Save();

            CreateBakerGroups();
            CreateFmcGroups();
            CreateHalliburtonGroups();
            CreateSchlumbergerGroups();
        }

        [Test]
        public void CreateNonAlliances()
        {
            if (_companyGroupRep.AsQueryable().SingleOrDefault(x => x.Name == "Non-Alliances") != null) return;

            var nonAllianceGroup = new CompanyGroup()
            {
                Name = "Non-Alliances",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = false,
                ParentObjectId = ObjectId.Empty
            };
            nonAllianceGroup.Save();

            CreateNonAlliancesGroups();
        }


        [Test]
        public void CreateBakerGroups()
        {
            var allianceGroup = _companyGroupRep.AsQueryable().Single(x => x.Name == "Alliances");

            var newGroup = new CompanyGroup
            {
                Name = "Baker Alliance",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                ParentObjectId = allianceGroup.Id,
            };
            newGroup.Save();

            var newChild = new CompanyGroup()
            {
                Name = "Baker United States",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "USA",
                NameContains = "Baker"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Baker Canada",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "CAN",
                NameContains = "Baker"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Baker Europe",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "EUR",
                NameContains = "Baker"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Baker Singapore",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "SIN",
                NameContains = "Baker"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Baker Malaysia",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "MSA",
                NameContains = "Baker"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Baker Dubai",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "DUB",
                NameContains = "Baker"
            };
            newGroup.AddChild(newChild);

            allianceGroup.AddChild(newGroup);
            allianceGroup.Save();
        }

        [Test]
        public void CreateFmcGroups()
        {
            var allianceGroup = _companyGroupRep.AsQueryable().Single(x => x.Name == "Alliances");

            var newGroup = new CompanyGroup
            {
                Name = "FMC Alliance",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                ParentObjectId = allianceGroup.Id,
            };
            newGroup.Save();

            var newChild = new CompanyGroup()
            {
                Name = "FMC United States",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "USA",
                NameContains = "FMC"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "FMC Canada",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "CAN",
                NameContains = "FMC"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "FMC Europe",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "EUR",
                NameContains = "FMC"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "FMC Singapore",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "SIN",
                NameContains = "FMC"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "FMC Malaysia",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "MSA",
                NameContains = "FMC"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "FMC Dubai",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "DUB",
                NameContains = "FMC"
            };
            newGroup.AddChild(newChild);

            allianceGroup.AddChild(newGroup);
            allianceGroup.Save();
        }

        [Test]
        public void CreateHalliburtonGroups()
        {
            var allianceGroup = _companyGroupRep.AsQueryable().Single(x => x.Name == "Alliances");

            var newGroup = new CompanyGroup
            {
                Name = "Halliburton Alliance",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                ParentObjectId = allianceGroup.Id,
            };
            newGroup.Save();

            var newChild = new CompanyGroup()
            {
                Name = "Halliburton United States",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "USA",
                NameContains = "Halliburton"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Halliburton Canada",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "CAN",
                NameContains = "Halliburton"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Halliburton Europe",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "EUR",
                NameContains = "Halliburton"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Halliburton Singapore",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "SIN",
                NameContains = "Halliburton"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Halliburton Malaysia",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "MSA",
                NameContains = "Halliburton"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Halliburton Dubai",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "DUB",
                NameContains = "Halliburton"
            };
            newGroup.AddChild(newChild);

            allianceGroup.AddChild(newGroup);
            allianceGroup.Save();
        }

        [Test]
        public void CreateSchlumbergerGroups()
        {
            var allianceGroup = _companyGroupRep.AsQueryable().Single(x => x.Name == "Alliances");

            var newGroup = new CompanyGroup
            {
                Name = "Schlumberger Alliance",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                ParentObjectId = allianceGroup.Id,
            };
            newGroup.Save();

            var newChild = new CompanyGroup()
            {
                Name = "Schlumberger United States",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "USA",
                NameContains = "Schlumberger"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Schlumberger Canada",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "CAN",
                NameContains = "Schlumberger"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Schlumberger Europe",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "EUR",
                NameContains = "Schlumberger"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Schlumberger Singapore",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "SIN",
                NameContains = "Schlumberger"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Schlumberger Malaysia",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "MSA",
                NameContains = "Schlumberger"
            };
            newGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Schlumberger Dubai",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = true,
                Branch = "DUB",
                NameContains = "Schlumberger"
            };
            newGroup.AddChild(newChild);

            allianceGroup.AddChild(newGroup);
            allianceGroup.Save();
        }

        [Test]
        public void CreateNonAlliancesGroups()
        {
            var nonAllianceGroup = _companyGroupRep.AsQueryable().Single(x => x.Name == "Non-Alliances");

            var newChild = new CompanyGroup()
            {
                Name = "US",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = false,
                Branch = "USA"
            };
            nonAllianceGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Canada",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = false,
                Branch = "CAN"
            };
            nonAllianceGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Europe",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = false,
                Branch = "EUR"
            };
            nonAllianceGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Singapore",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = false,
                Branch = "SIN"
            };
            nonAllianceGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Malaysia",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = false,
                Branch = "MSA"
            };
            nonAllianceGroup.AddChild(newChild);

            newChild = new CompanyGroup()
            {
                Name = "Dubai",
                IsAnalytical = false,
                IsGlobal = true,
                IsAlliance = false,
                Branch = "DUB"
            };
            nonAllianceGroup.AddChild(newChild);

            nonAllianceGroup.Save();
        }

    }
}
