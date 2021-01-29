using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Bson.Serialization.Attributes;
using Vulcan.IMetal.Queries.Companies;

namespace DAL.Vulcan.Mongo.Importers
{
    public class CompanyImporter: BaseDocument
    {
        public string Coid { get; set; } = string.Empty;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]

        public DateTime ExecutedOn = DateTime.Now;

        public List<Company> Imported { get; set; } = new List<Company>();
        public List<Company> Removed { get; set; } = new List<Company>();
        public List<Company> Updated { get; set; } = new List<Company>();
        public List<Company> Skipped { get; set; } = new List<Company>();
        public List<string> SkipReasons { get; set; } = new List<string>();

        public List<TeamRef> TeamsUpdated { get; set; } = new List<TeamRef>();

        public bool DebugMode { get; set; } = true;

        public void Execute(string coid)
        {
            Coid = coid;
            var repCompany = new RepositoryBase<Company>();

            var repLocation = new RepositoryBase<Location>();
            var branch = coid;
            if (coid == "INC") branch = "USA";
            var location = repLocation.AsQueryable().First(x => x.Branch == branch);


            var queryCompany = new QueryCompany(coid);
            var companies = queryCompany.GetCompanies(coid);

            foreach (var result in companies)
            {

                var isActive = result.StatusId == 1;

                var existingCompany = repCompany.AsQueryable()
                    .FirstOrDefault(x => x.Location.Branch == branch && x.SqlId == result.SqlId);
                if (existingCompany == null)
                {
                    var company = new Company
                    {
                        Location = location,
                        Code = result.Code,
                        CreatedByUserId = "admin",
                        Name = result.Name,
                        ShortName = result.ShortName,
                        SqlId = result.SqlId,
                        IsActive = isActive,
                        IsAlliance = false
                    };
                    if (result.Address != null)
                    {
                        company.Addresses.Add(new Address
                        {
                            Country = location.Country,
                            AddressLine1 = result.Address.Address1,
                            City = result.Address.Town,
                            StateProvince = result.Address.County,
                            County = result.Address.County,
                            PostalCode = result.Address.Postcode,
                            Type = AddressType.Primary,
                            AddressLine2 = String.Empty
                        });
                    }

                    if (!DebugMode)
                    {
                        company.CreatedByUserId = "SystemAdmin";
                        repCompany.Upsert(company);
                        company.LoadContactsFromIMetal();
                    }
                    Imported.Add(company);
                }
                else
                {
                    foreach (var companySubAddress in result.Addresses.ToList())
                    {
                        if (companySubAddress.Address1 == string.Empty) continue;

                        var addressFound = existingCompany.Addresses.FirstOrDefault(x =>
                            x.AddressLine1 == companySubAddress.Address1 &&
                            x.City == companySubAddress.Town &&
                            x.StateProvince == companySubAddress.County &&
                            x.PostalCode == companySubAddress.Postcode);
                        if (addressFound == null)
                        {
                            var newAddress = new Address
                            {
                                Country = location.Country,
                                AddressLine1 = companySubAddress.Address1,
                                City = companySubAddress.Town,
                                StateProvince = companySubAddress.County,
                                County = companySubAddress.County,
                                PostalCode = companySubAddress.Postcode,
                                Type = AddressType.Other,
                                AddressLine2 = String.Empty
                            };
                            existingCompany.Addresses.Add(newAddress);
                            repCompany.Upsert(existingCompany);
                            existingCompany.CompanyUpdates.Add(new CompanyUpdateHistory("NewAddress", "(added)", $"{newAddress.AddressLine1}\n{newAddress.City}, {newAddress.StateProvince} {newAddress.PostalCode}"));
                        }
                    }

                    if (
                        (result.Code != existingCompany.Code)
                        ||
                        (result.Name != existingCompany.Name)
                        ||
                        (result.ShortName != existingCompany.ShortName)
                        ||
                        (isActive != existingCompany.IsActive)
                    )
                    {

                        

                        if (existingCompany.Code != result.Code)
                        {
                            existingCompany.CompanyUpdates.Add(new CompanyUpdateHistory("Code", existingCompany.Code, result.Code));
                        }

                        if (existingCompany.Name != result.Name)
                        {
                            existingCompany.CompanyUpdates.Add(new CompanyUpdateHistory("Name",existingCompany.Name, result.Name));
                        }

                        if (existingCompany.ShortName != result.ShortName)
                        {
                            existingCompany.CompanyUpdates.Add(new CompanyUpdateHistory("ShortName",existingCompany.ShortName, result.ShortName));
                        }

                        if (existingCompany.IsActive != isActive)
                        {
                            existingCompany.CompanyUpdates.Add(new CompanyUpdateHistory("IsActive", existingCompany.IsActive.ToString(), isActive.ToString()));
                        }

                        existingCompany.Code = result.Code;
                        existingCompany.Name = result.Name;
                        existingCompany.ShortName = result.ShortName;
                        existingCompany.IsActive = isActive;
                        if (!DebugMode)
                        {
                            existingCompany.ModifiedByUserId = "SystemAdmin";
                            repCompany.Upsert(existingCompany);
                        }
                        Updated.Add(existingCompany);
                    }
                }
            }

            foreach (var company in repCompany.AsQueryable().Where(x => x.Location.Branch == branch).ToList())
            {
                if (companies.All(x => x.SqlId != company.SqlId))
                {
                    var quotesUsingThisCompany = new RepositoryBase<CrmQuote>().AsQueryable()
                        .Where(x => x.Company.Id == company.Id.ToString()).ToList();
                    if (quotesUsingThisCompany.Any())
                    {
                        foreach (var crmQuote in quotesUsingThisCompany)
                        {
                            SkipReasons.Add(
                                $"QuoteId: {crmQuote.QuoteId} is using Invalid Supplier {company.Code} : {company.Name}");
                        }
                        Skipped.Add(company);
                    }
                    else
                    {
                        if (!DebugMode)
                        {
                            repCompany.RemoveOne(company);
                        }
                        Removed.Add(company);
                    }
                }
            }
            var results = LoadMissingCompaniesForAllGroups.Execute();
            TeamsUpdated = results.TeamsUpdated;

            UpdateTeamCompaniesThatWereModifiedDuringImport.Execute(Updated);

            this.SaveToDatabase();
        }
    }
}