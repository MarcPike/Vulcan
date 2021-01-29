using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Bson;
using NUnit.Framework;
using Vulcan.IMetal.Context.Company;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Company_Groups
{
    [TestFixture]
    public class CompanyGroupLoader
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
            _companyGroupRep = new RepositoryBase<CompanyGroup>();
        }

        private RepositoryBase<CompanyGroup> _companyGroupRep;

        [Test]
        public void LoadSingapore()
        {
            LoadSingaporeAlliances();
            LoadMalaysiaNonAlliances();
        }

        [Test]
        public void LoadMalaysia()
        {
            LoadMalaysiaAlliances();
            LoadMalaysiaNonAlliances();
        }

        [Test]
        public void LoadDubai()
        {
            LoadDubaiAlliances();
            LoadDubaiNonAlliances();
        }

        [Test]
        public void LoadDubaiAlliances()
        {
            var branch = "DUB";
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var companyGroups = _companyGroupRep.AsQueryable().Where(x => x.Branch == branch && x.IsAlliance).ToList();

            foreach (var companyGroup in companyGroups)
            {
                companyGroup.Companies.Clear();

                var companies = repCompany.AsQueryable()
                    .Where(x => x.Location.Branch == branch && x.Name.Contains(companyGroup.NameContains))
                    //.Select(x=> new CompanyRef(x))
                    .ToList();
                var addedRow = false;
                foreach (var company in companies)
                {

                    company.IsAlliance = true;
                    CompanyResolver.CompanyRefresher.Refresh(company);
                    company.SaveToDatabase();

                    if (companyGroup.Companies.All(x => x.SqlId != company.SqlId))
                    {
                        companyGroup.Companies.Add(company.AsCompanyRef());
                        addedRow = true;
                    }
                }
                if (addedRow)
                {
                    var repCompanyGroup = new RepositoryBase<CompanyGroup>();
                    repCompanyGroup.Upsert(companyGroup);
                }

            }
        }

        [Test]
        public void LoadDubaiNonAlliances()
        {
            var branch = "DUB";
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var companyGroups = _companyGroupRep.AsQueryable().Where(x => x.Branch == branch && !x.IsAlliance).ToList();

            foreach (var companyGroup in companyGroups)
            {
                companyGroup.Companies.Clear();
                var companies = repCompany.AsQueryable()
                    .Where(x => x.Location.Branch == branch && !x.IsAlliance)
                    //.Select(x=> new CompanyRef(x))
                    .ToList();
                var addedRow = false;
                foreach (var company in companies)
                {
                    CompanyResolver.CompanyRefresher.Refresh(company);

                    if (companyGroup.Companies.All(x => x.SqlId != company.SqlId))
                    {
                        companyGroup.Companies.Add(company.AsCompanyRef());
                        addedRow = true;
                    }
                }
                if (addedRow)
                {
                    var repCompanyGroup = new RepositoryBase<CompanyGroup>();
                    repCompanyGroup.Upsert(companyGroup);
                }

            }
        }


        [Test]
        public void LoadMalaysiaAlliances()
        {
            var branch = "MSA";
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var companyGroups = _companyGroupRep.AsQueryable().Where(x => x.Branch == branch && x.IsAlliance).ToList();

            foreach (var companyGroup in companyGroups)
            {
                companyGroup.Companies.Clear();

                var companies = repCompany.AsQueryable()
                    .Where(x => x.Location.Branch == branch && x.Name.Contains(companyGroup.NameContains))
                    //.Select(x=> new CompanyRef(x))
                    .ToList();
                var addedRow = false;
                foreach (var company in companies)
                {

                    company.IsAlliance = true;
                    CompanyResolver.CompanyRefresher.Refresh(company);
                    company.SaveToDatabase();

                    if (companyGroup.Companies.All(x => x.SqlId != company.SqlId))
                    {
                        companyGroup.Companies.Add(company.AsCompanyRef());
                        addedRow = true;
                    }
                }
                if (addedRow)
                {
                    var repCompanyGroup = new RepositoryBase<CompanyGroup>();
                    repCompanyGroup.Upsert(companyGroup);
                }

            }
        }

        [Test]
        public void LoadMalaysiaNonAlliances()
        {
            var branch = "MSA";
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var companyGroups = _companyGroupRep.AsQueryable().Where(x => x.Branch == branch && !x.IsAlliance).ToList();

            foreach (var companyGroup in companyGroups)
            {
                companyGroup.Companies.Clear();
                var companies = repCompany.AsQueryable()
                    .Where(x => x.Location.Branch == branch && !x.IsAlliance)
                    //.Select(x=> new CompanyRef(x))
                    .ToList();
                var addedRow = false;
                foreach (var company in companies)
                {
                    CompanyResolver.CompanyRefresher.Refresh(company);

                    if (companyGroup.Companies.All(x => x.SqlId != company.SqlId))
                    {
                        companyGroup.Companies.Add(company.AsCompanyRef());
                        addedRow = true;
                    }
                }
                if (addedRow)
                {
                    var repCompanyGroup = new RepositoryBase<CompanyGroup>();
                    repCompanyGroup.Upsert(companyGroup);
                }

            }
        }




        [Test]
        public void LoadSingaporeAlliances()
        {
            var branch = "SIN";
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var companyGroups = _companyGroupRep.AsQueryable().Where(x => x.Branch == branch && x.IsAlliance).ToList();

            foreach (var companyGroup in companyGroups)
            {
                companyGroup.Companies.Clear();

                var companies = repCompany.AsQueryable()
                    .Where(x => x.Location.Branch == branch && x.Name.Contains(companyGroup.NameContains))
                    //.Select(x=> new CompanyRef(x))
                    .ToList();
                var addedRow = false;
                foreach (var company in companies)
                {

                    company.IsAlliance = true;
                    CompanyResolver.CompanyRefresher.Refresh(company);
                    company.SaveToDatabase();

                    if (companyGroup.Companies.All(x => x.SqlId != company.SqlId))
                    {
                        companyGroup.Companies.Add(company.AsCompanyRef());
                        addedRow = true;
                    }
                }
                if (addedRow)
                {
                    var repCompanyGroup = new RepositoryBase<CompanyGroup>();
                    repCompanyGroup.Upsert(companyGroup);
                }

            }
        }

        [Test]
        public void LoadSingaporeNonAlliances()
        {
            var branch = "SIN";
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var companyGroups = _companyGroupRep.AsQueryable().Where(x => x.Branch == branch && !x.IsAlliance).ToList();

            foreach (var companyGroup in companyGroups)
            {
                companyGroup.Companies.Clear();
                var companies = repCompany.AsQueryable()
                    .Where(x => x.Location.Branch == branch && !x.IsAlliance)
                    //.Select(x=> new CompanyRef(x))
                    .ToList();
                var addedRow = false;
                foreach (var company in companies)
                {
                    CompanyResolver.CompanyRefresher.Refresh(company);

                    if (companyGroup.Companies.All(x => x.SqlId != company.SqlId))
                    {
                        companyGroup.Companies.Add(company.AsCompanyRef());
                        addedRow = true;
                    }
                }
                if (addedRow)
                {
                    var repCompanyGroup = new RepositoryBase<CompanyGroup>();
                    repCompanyGroup.Upsert(companyGroup);
                }

            }
        }



        [Test]
        public void LoadEuropeAlliances()
        {
            var branch = "EUR";
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var alliancesGroupsForEurope = _companyGroupRep.AsQueryable().Where(x => x.Branch == branch && x.IsAlliance).ToList();

            foreach (var companyGroup in alliancesGroupsForEurope)
            {
                var companies = repCompany.AsQueryable()
                    .Where(x => x.Location.Branch == branch && x.Name.Contains(companyGroup.NameContains))
                    //.Select(x=> new CompanyRef(x))
                    .ToList();
                var addedRow = false;
                foreach (var company in companies)
                {

                    company.IsAlliance = true;
                    company.SaveToDatabase();

                    if (companyGroup.Companies.All(x => x.SqlId != company.SqlId))
                    {
                        companyGroup.Companies.Add(company.AsCompanyRef());
                        addedRow = true;
                    }
                }
                if (addedRow)
                {
                    var repCompanyGroup = new RepositoryBase<CompanyGroup>();
                    repCompanyGroup.Upsert(companyGroup);
                }

            }
        }

        [Test]
        public void LoadEuropeNonAlliances()
        {
            var branch = "EUR";
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var nonAllianceGroupsForEurope = _companyGroupRep.AsQueryable().Where(x => x.Branch == branch && !x.IsAlliance).ToList();

            foreach (var companyGroup in nonAllianceGroupsForEurope)
            {
                var companies = repCompany.AsQueryable()
                    .Where(x => x.Location.Branch == branch && !x.IsAlliance)
                    //.Select(x=> new CompanyRef(x))
                    .ToList();
                var addedRow = false;
                foreach (var company in companies)
                {

                    if (companyGroup.Companies.All(x => x.SqlId != company.SqlId))
                    {
                        companyGroup.Companies.Add(company.AsCompanyRef());
                        addedRow = true;
                    }
                }
                if (addedRow)
                {
                    var repCompanyGroup = new RepositoryBase<CompanyGroup>();
                    repCompanyGroup.Upsert(companyGroup);
                }

            }
        }


        [Test]
        public void LoadCanadaAlliances()
        {
            var branch = "CAN";
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var alliancesGroupsForCanada = _companyGroupRep.AsQueryable().Where(x => x.Branch == branch && x.IsAlliance).ToList();

            foreach (var companyGroup in alliancesGroupsForCanada)
            {
                var companies = repCompany.AsQueryable()
                    .Where(x => x.Location.Branch == branch && x.Name.Contains(companyGroup.NameContains))
                    //.Select(x=> new CompanyRef(x))
                    .ToList();
                var addedRow = false;
                foreach (var company in companies)
                {

                    company.IsAlliance = true;
                    company.SaveToDatabase();

                    if (companyGroup.Companies.All(x => x.SqlId != company.SqlId))
                    {
                        companyGroup.Companies.Add(company.AsCompanyRef());
                        addedRow = true;
                    }
                }
                if (addedRow)
                {
                    var repCompanyGroup = new RepositoryBase<CompanyGroup>();
                    repCompanyGroup.Upsert(companyGroup);
                }

            }
        }

        [Test]
        public void LoadCanadaNonAlliances()
        {
            var branch = "CAN";
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var nonAllianceGroupsForCanada = _companyGroupRep.AsQueryable().Where(x => x.Branch == branch && !x.IsAlliance).ToList();

            foreach (var companyGroup in nonAllianceGroupsForCanada)
            {
                var companies = repCompany.AsQueryable()
                    .Where(x => x.Location.Branch == branch && !x.IsAlliance)
                    //.Select(x=> new CompanyRef(x))
                    .ToList();
                var addedRow = false;
                foreach (var company in companies)
                {

                    if (companyGroup.Companies.All(x => x.SqlId != company.SqlId))
                    {
                        companyGroup.Companies.Add(company.AsCompanyRef());
                        addedRow = true;
                    }
                }
                if (addedRow)
                {
                    var repCompanyGroup = new RepositoryBase<CompanyGroup>();
                    repCompanyGroup.Upsert(companyGroup);
                }

            }
        }

        [Test]
        public void ReloadIncNonAlliances()
        {
            var branch = "USA";
            var repCompany = new RepositoryBase<Mongo.DocClass.Companies.Company>();

            var nonAllianceGroupsForUsa = _companyGroupRep.AsQueryable().Where(x => x.Branch == branch && !x.IsAlliance).ToList();

            foreach (var companyGroup in nonAllianceGroupsForUsa)
            {
                var companies = repCompany.AsQueryable()
                    .Where(x => x.Location.Branch == branch && !x.IsAlliance)
                    //.Select(x=> new CompanyRef(x))
                    .ToList();
                var addedRow = false;
                foreach (var company in companies)
                {

                    if (companyGroup.Companies.All(x => x.SqlId != company.SqlId))
                    {
                        companyGroup.Companies.Add(company.AsCompanyRef());
                        addedRow = true;
                    }
                }
                if (addedRow)
                {
                    var repCompanyGroup = new RepositoryBase<CompanyGroup>();
                    repCompanyGroup.Upsert(companyGroup);
                }

            }

        }


        [Test]
        public void CreateAllGroups()
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
