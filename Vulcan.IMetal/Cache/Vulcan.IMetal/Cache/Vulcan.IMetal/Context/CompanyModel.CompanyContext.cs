﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using LinqConnect template.
// Code is generated on: 4/8/2019 10:47:06 AM
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using Devart.Data.Linq;
using Devart.Data.Linq.Mapping;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace Vulcan.IMetal.Context.Company
{

    [DatabaseAttribute(Name = "live_emetal")]
    [ProviderAttribute(typeof(Devart.Data.PostgreSql.Linq.Provider.PgSqlDataProvider))]
    public partial class CompanyContext : Devart.Data.Linq.DataContext
    {
        public static CompiledQueryCache compiledQueryCache = CompiledQueryCache.RegisterDataContext(typeof(CompanyContext));
        private static MappingSource mappingSource = new Devart.Data.Linq.Mapping.AttributeMappingSource();

        #region Extensibility Method Definitions
    
        partial void OnCreated();
        partial void OnSubmitError(Devart.Data.Linq.SubmitErrorEventArgs args);
        partial void InsertContactFrequencyCode(ContactFrequencyCode instance);
        partial void UpdateContactFrequencyCode(ContactFrequencyCode instance);
        partial void DeleteContactFrequencyCode(ContactFrequencyCode instance);
        partial void InsertCompany(Company instance);
        partial void UpdateCompany(Company instance);
        partial void DeleteCompany(Company instance);
        partial void InsertContactTypeCode(ContactTypeCode instance);
        partial void UpdateContactTypeCode(ContactTypeCode instance);
        partial void DeleteContactTypeCode(ContactTypeCode instance);
        partial void InsertContactTitleCode(ContactTitleCode instance);
        partial void UpdateContactTitleCode(ContactTitleCode instance);
        partial void DeleteContactTitleCode(ContactTitleCode instance);
        partial void InsertCurrencyCode(CurrencyCode instance);
        partial void UpdateCurrencyCode(CurrencyCode instance);
        partial void DeleteCurrencyCode(CurrencyCode instance);
        partial void InsertBranchLocation(BranchLocation instance);
        partial void UpdateBranchLocation(BranchLocation instance);
        partial void DeleteBranchLocation(BranchLocation instance);
        partial void InsertCategoryGroup(CategoryGroup instance);
        partial void UpdateCategoryGroup(CategoryGroup instance);
        partial void DeleteCategoryGroup(CategoryGroup instance);
        partial void InsertCountryCode(CountryCode instance);
        partial void UpdateCountryCode(CountryCode instance);
        partial void DeleteCountryCode(CountryCode instance);
        partial void InsertContact(Contact instance);
        partial void UpdateContact(Contact instance);
        partial void DeleteContact(Contact instance);
        partial void InsertBranch(Branch instance);
        partial void UpdateBranch(Branch instance);
        partial void DeleteBranch(Branch instance);
        partial void InsertContactStatusCode(ContactStatusCode instance);
        partial void UpdateContactStatusCode(ContactStatusCode instance);
        partial void DeleteContactStatusCode(ContactStatusCode instance);
        partial void InsertAddress(Address instance);
        partial void UpdateAddress(Address instance);
        partial void DeleteAddress(Address instance);
        partial void InsertCustomerAnalysisCode(CustomerAnalysisCode instance);
        partial void UpdateCustomerAnalysisCode(CustomerAnalysisCode instance);
        partial void DeleteCustomerAnalysisCode(CustomerAnalysisCode instance);
        partial void InsertCompanyStatusCode(CompanyStatusCode instance);
        partial void UpdateCompanyStatusCode(CompanyStatusCode instance);
        partial void DeleteCompanyStatusCode(CompanyStatusCode instance);
        partial void InsertCompanyNoteTemplate(CompanyNoteTemplate instance);
        partial void UpdateCompanyNoteTemplate(CompanyNoteTemplate instance);
        partial void DeleteCompanyNoteTemplate(CompanyNoteTemplate instance);
        partial void InsertCompanyTypeCode(CompanyTypeCode instance);
        partial void UpdateCompanyTypeCode(CompanyTypeCode instance);
        partial void DeleteCompanyTypeCode(CompanyTypeCode instance);
        partial void InsertCompanySubAddress(CompanySubAddress instance);
        partial void UpdateCompanySubAddress(CompanySubAddress instance);
        partial void DeleteCompanySubAddress(CompanySubAddress instance);
        partial void InsertCategory(Category instance);
        partial void UpdateCategory(Category instance);
        partial void DeleteCategory(Category instance);
        partial void InsertCompanyTotal(CompanyTotal instance);
        partial void UpdateCompanyTotal(CompanyTotal instance);
        partial void DeleteCompanyTotal(CompanyTotal instance);
        partial void InsertCertificationRequirement(CertificationRequirement instance);
        partial void UpdateCertificationRequirement(CertificationRequirement instance);
        partial void DeleteCertificationRequirement(CertificationRequirement instance);
        partial void InsertTerm(Term instance);
        partial void UpdateTerm(Term instance);
        partial void DeleteTerm(Term instance);
        partial void InsertTransportTypeCode(TransportTypeCode instance);
        partial void UpdateTransportTypeCode(TransportTypeCode instance);
        partial void DeleteTransportTypeCode(TransportTypeCode instance);
        partial void InsertSalesGroup(SalesGroup instance);
        partial void UpdateSalesGroup(SalesGroup instance);
        partial void DeleteSalesGroup(SalesGroup instance);

        #endregion

        public CompanyContext() :
        base(GetConnectionString("LiveEmetalDataContextConnectionString"), mappingSource)
        {
            OnCreated();
        }

        public CompanyContext(MappingSource mappingSource) :
        base(GetConnectionString("LiveEmetalDataContextConnectionString"), mappingSource)
        {
            OnCreated();
        }

        private static string GetConnectionString(string connectionStringName)
        {
            System.Configuration.ConnectionStringSettings connectionStringSettings = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionStringSettings == null)
                throw new InvalidOperationException("Connection string \"" + connectionStringName +"\" could not be found in the configuration file.");
            return connectionStringSettings.ConnectionString;
        }

        public CompanyContext(string connection) :
            base(connection, mappingSource)
        {
          OnCreated();
        }

        public CompanyContext(System.Data.IDbConnection connection) :
            base(connection, mappingSource)
        {
          OnCreated();
        }

        public CompanyContext(string connection, MappingSource mappingSource) :
            base(connection, mappingSource)
        {
          OnCreated();
        }

        public CompanyContext(System.Data.IDbConnection connection, MappingSource mappingSource) :
            base(connection, mappingSource)
        {
          OnCreated();
        }

        public Devart.Data.Linq.Table<ContactFrequencyCode> ContactFrequencyCode
        {
            get
            {
                return this.GetTable<ContactFrequencyCode>();
            }
        }

        public Devart.Data.Linq.Table<Company> Company
        {
            get
            {
                return this.GetTable<Company>();
            }
        }

        public Devart.Data.Linq.Table<ContactTypeCode> ContactTypeCode
        {
            get
            {
                return this.GetTable<ContactTypeCode>();
            }
        }

        public Devart.Data.Linq.Table<ContactTitleCode> ContactTitleCode
        {
            get
            {
                return this.GetTable<ContactTitleCode>();
            }
        }

        public Devart.Data.Linq.Table<CurrencyCode> CurrencyCode
        {
            get
            {
                return this.GetTable<CurrencyCode>();
            }
        }

        public Devart.Data.Linq.Table<BranchLocation> BranchLocation
        {
            get
            {
                return this.GetTable<BranchLocation>();
            }
        }

        public Devart.Data.Linq.Table<CategoryGroup> CategoryGroup
        {
            get
            {
                return this.GetTable<CategoryGroup>();
            }
        }

        public Devart.Data.Linq.Table<CountryCode> CountryCode
        {
            get
            {
                return this.GetTable<CountryCode>();
            }
        }

        public Devart.Data.Linq.Table<Contact> Contact
        {
            get
            {
                return this.GetTable<Contact>();
            }
        }

        public Devart.Data.Linq.Table<Branch> Branch
        {
            get
            {
                return this.GetTable<Branch>();
            }
        }

        public Devart.Data.Linq.Table<ContactStatusCode> ContactStatusCode
        {
            get
            {
                return this.GetTable<ContactStatusCode>();
            }
        }

        public Devart.Data.Linq.Table<Address> Address
        {
            get
            {
                return this.GetTable<Address>();
            }
        }

        public Devart.Data.Linq.Table<CustomerAnalysisCode> CustomerAnalysisCode
        {
            get
            {
                return this.GetTable<CustomerAnalysisCode>();
            }
        }

        public Devart.Data.Linq.Table<CompanyStatusCode> CompanyStatusCode
        {
            get
            {
                return this.GetTable<CompanyStatusCode>();
            }
        }

        public Devart.Data.Linq.Table<CompanyNoteTemplate> CompanyNoteTemplate
        {
            get
            {
                return this.GetTable<CompanyNoteTemplate>();
            }
        }

        public Devart.Data.Linq.Table<CompanyTypeCode> CompanyTypeCode
        {
            get
            {
                return this.GetTable<CompanyTypeCode>();
            }
        }

        public Devart.Data.Linq.Table<CompanySubAddress> CompanySubAddress
        {
            get
            {
                return this.GetTable<CompanySubAddress>();
            }
        }

        public Devart.Data.Linq.Table<Category> Category
        {
            get
            {
                return this.GetTable<Category>();
            }
        }

        public Devart.Data.Linq.Table<CompanyTotal> CompanyTotal
        {
            get
            {
                return this.GetTable<CompanyTotal>();
            }
        }

        public Devart.Data.Linq.Table<CertificationRequirement> CertificationRequirement
        {
            get
            {
                return this.GetTable<CertificationRequirement>();
            }
        }

        public Devart.Data.Linq.Table<Term> Term
        {
            get
            {
                return this.GetTable<Term>();
            }
        }

        public Devart.Data.Linq.Table<TransportTypeCode> TransportTypeCode
        {
            get
            {
                return this.GetTable<TransportTypeCode>();
            }
        }

        public Devart.Data.Linq.Table<SalesGroup> SalesGroup
        {
            get
            {
                return this.GetTable<SalesGroup>();
            }
        }
    }
}
