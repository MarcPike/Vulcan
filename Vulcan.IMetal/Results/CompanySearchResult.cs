using System;
using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.Models;
using Vulcan.IMetal.Queries.Companies;

namespace Vulcan.IMetal.Results
{
    /// <summary>
    /// TODO: Finalize requirements for Companies
    /// </summary>
    public class CompanySearchResult
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public string Branch { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Code { get; set; }
        public string CompanyType { get; set; }
        public string StdIndustrialClassCode { get; set; }
        public string StdIndustrialClassDescription { get; set; }
        public string CustomerGroupCode { get; set; }
        public string CustomerGroupDescription { get; set; }
        public string TypeDescription { get; set; } 
        public string Status { get; set; } 

        public string CountryCode { get; set; }
        public string CountryName { get; set; }

        public List<CompanyAddressModel> Addresses { get; set; }

        public CompanyAddressModel PrimaryAddress { get; set; }

        public string Telephone { get; set; }
        public string FastDial { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Categories { get; set; }

        public string ProductsNote { get; set; }

        public string CompetitionNote { get; set; }

        public string AccountsNote { get; set; }

        public string GeneralNote { get; set; }
        public string PopupNotes { get; set; }

        public bool PaymentHold { get; set; }

        public CertificationRequirement CertificationRequirement { get; set; }

        public bool CertRequiredForChemical { get; set; }
        public bool CertRequiredForMechanical { get; set; }
        public bool CertRequiredForMill { get; set; }
        public bool CertRequiredForCompliance { get; set; }

        public DateTime? Modified { get; set; }

        public DateTime? Created { get; set; }

        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public Term Term { get; set; }
        public string DefaultSalesGroupCode { get; set; }

        public CompanySearchResult(CompanyContext context, string coid, Company company, bool withAddresses)
        {
            Coid = coid;
            Id = company.Id;
            Name = company.Name;
            ShortName = company.ShortName;
            Code = company.Code;
            Branch = company.Branch_BranchId.Name;
            StdIndustrialClassCode = company.CustomerAnalysisCode_AnalysisCode2Id?.Code;
            StdIndustrialClassDescription = company.CustomerAnalysisCode_AnalysisCode2Id?.Description;
            CustomerGroupCode = company.CustomerAnalysisCode_AnalysisCode1Id?.Code;
            CustomerGroupDescription = company.CustomerAnalysisCode_AnalysisCode1Id?.Description;

            CountryCode = company.Address.CountryCode?.Code;
            CountryName = company.Address.CountryCode?.Description;

            if (company.SalesGroup != null)
            {
                DefaultSalesGroupCode = company.SalesGroup.Code ?? string.Empty;
            }

            Term = company.Term;

            if (withAddresses)
            {
                Addresses = company.CompanySubAddress.Select(x => new CompanyAddressModel(x, coid)).ToList();
            }

            var primaryAddress = context.Address.SingleOrDefault(x => x.Id == company.AddressId);
            if (primaryAddress != null)
                PrimaryAddress = new CompanyAddressModel()
                {
                    Coid = coid,
                    Name = Name,
                    Address = primaryAddress.Address1,
                    AddressId = primaryAddress.Id,
                    County = primaryAddress.County,
                    Town = primaryAddress.Town,
                    PostCode = primaryAddress.Postcode,
                    CountryName = CountryName,
                };

            Telephone = company.Telephone;
            Fax = company.Fax;
            FastDial = company.FastDial;
            Email = company.Email;
            GeneralNote = company.GeneralNote;
            AccountsNote = company.AccountsNote;
            CompetitionNote = company.CompetitionNote;
            ProductsNote = company.ProductsNote;
            Created = company.Cdate;
            Modified = company.Mdate;
            Categories = company.Category;

            PopupNotes = company.PopupNote;
            PaymentHold = company.PaymentHold ?? false;
            var certificationRequirements = company.CertificationRequirement;
            if (certificationRequirements != null)
            {
                CertRequiredForChemical = certificationRequirements.ChemicalCert ?? false;
                CertRequiredForMechanical = certificationRequirements.MechanicalCert ?? false;
                CertRequiredForMill = certificationRequirements.MillCert ?? false;
                CertRequiredForCompliance = certificationRequirements.ComplianceCert ?? false;
            }

            CompanyType = company.CompanyTypeCode.Code;
            TypeDescription = company.CompanyTypeCode.Description;
            Status = company.Status;
            CurrencyCode = company.CurrencyCode.Code;
            CurrencyName = company.CurrencyCode.Name;
        }

    }
}