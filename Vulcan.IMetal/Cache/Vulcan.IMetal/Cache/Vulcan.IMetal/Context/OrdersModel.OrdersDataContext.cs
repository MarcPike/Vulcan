﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using LinqConnect template.
// Code is generated on: 9/19/2017 10:27:27 AM
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

namespace Vulcan.IMetal.Context.Orders
{

    [DatabaseAttribute(Name = "live_emetal")]
    [ProviderAttribute(typeof(Devart.Data.PostgreSql.Linq.Provider.PgSqlDataProvider))]
    public partial class OrdersDataContext : Devart.Data.Linq.DataContext
    {
        public static CompiledQueryCache compiledQueryCache = CompiledQueryCache.RegisterDataContext(typeof(OrdersDataContext));
        private static MappingSource mappingSource = new Devart.Data.Linq.Mapping.AttributeMappingSource();

        #region Extensibility Method Definitions
    
        partial void OnCreated();
        partial void OnSubmitError(Devart.Data.Linq.SubmitErrorEventArgs args);
        partial void InsertCompany(Company instance);
        partial void UpdateCompany(Company instance);
        partial void DeleteCompany(Company instance);
        partial void InsertProductControl(ProductControl instance);
        partial void UpdateProductControl(ProductControl instance);
        partial void DeleteProductControl(ProductControl instance);
        partial void InsertProductCategory(ProductCategory instance);
        partial void UpdateProductCategory(ProductCategory instance);
        partial void DeleteProductCategory(ProductCategory instance);
        partial void InsertUnitsOfMeasure(UnitsOfMeasure instance);
        partial void UpdateUnitsOfMeasure(UnitsOfMeasure instance);
        partial void DeleteUnitsOfMeasure(UnitsOfMeasure instance);
        partial void InsertSalesType(SalesType instance);
        partial void UpdateSalesType(SalesType instance);
        partial void DeleteSalesType(SalesType instance);
        partial void InsertSalesGroup(SalesGroup instance);
        partial void UpdateSalesGroup(SalesGroup instance);
        partial void DeleteSalesGroup(SalesGroup instance);
        partial void InsertWarehouse(Warehouse instance);
        partial void UpdateWarehouse(Warehouse instance);
        partial void DeleteWarehouse(Warehouse instance);
        partial void InsertProductLevelAllocation(ProductLevelAllocation instance);
        partial void UpdateProductLevelAllocation(ProductLevelAllocation instance);
        partial void DeleteProductLevelAllocation(ProductLevelAllocation instance);
        partial void InsertContact(Contact instance);
        partial void UpdateContact(Contact instance);
        partial void DeleteContact(Contact instance);
        partial void InsertBranch(Branch instance);
        partial void UpdateBranch(Branch instance);
        partial void DeleteBranch(Branch instance);
        partial void InsertProduct(Product instance);
        partial void UpdateProduct(Product instance);
        partial void DeleteProduct(Product instance);
        partial void InsertProductSubGroup(ProductSubGroup instance);
        partial void UpdateProductSubGroup(ProductSubGroup instance);
        partial void DeleteProductSubGroup(ProductSubGroup instance);
        partial void InsertSalesItem(SalesItem instance);
        partial void UpdateSalesItem(SalesItem instance);
        partial void DeleteSalesItem(SalesItem instance);
        partial void InsertAddress(Address instance);
        partial void UpdateAddress(Address instance);
        partial void DeleteAddress(Address instance);
        partial void InsertSalesStatusCode(SalesStatusCode instance);
        partial void UpdateSalesStatusCode(SalesStatusCode instance);
        partial void DeleteSalesStatusCode(SalesStatusCode instance);
        partial void InsertDimensionValue(DimensionValue instance);
        partial void UpdateDimensionValue(DimensionValue instance);
        partial void DeleteDimensionValue(DimensionValue instance);
        partial void InsertTransportTypeCode(TransportTypeCode instance);
        partial void UpdateTransportTypeCode(TransportTypeCode instance);
        partial void DeleteTransportTypeCode(TransportTypeCode instance);
        partial void InsertEnquiryLostReason(EnquiryLostReason instance);
        partial void UpdateEnquiryLostReason(EnquiryLostReason instance);
        partial void DeleteEnquiryLostReason(EnquiryLostReason instance);
        partial void InsertCompanySubAddress(CompanySubAddress instance);
        partial void UpdateCompanySubAddress(CompanySubAddress instance);
        partial void DeleteCompanySubAddress(CompanySubAddress instance);
        partial void InsertSalesHeader(SalesHeader instance);
        partial void UpdateSalesHeader(SalesHeader instance);
        partial void DeleteSalesHeader(SalesHeader instance);
        partial void InsertProductClass(ProductClass instance);
        partial void UpdateProductClass(ProductClass instance);
        partial void DeleteProductClass(ProductClass instance);
        partial void InsertPersonnel(Personnel instance);
        partial void UpdatePersonnel(Personnel instance);
        partial void DeletePersonnel(Personnel instance);
        partial void InsertCurrencyCode(CurrencyCode instance);
        partial void UpdateCurrencyCode(CurrencyCode instance);
        partial void DeleteCurrencyCode(CurrencyCode instance);
        partial void InsertSalesTotal(SalesTotal instance);
        partial void UpdateSalesTotal(SalesTotal instance);
        partial void DeleteSalesTotal(SalesTotal instance);
        partial void InsertCostItem(CostItem instance);
        partial void UpdateCostItem(CostItem instance);
        partial void DeleteCostItem(CostItem instance);
        partial void InsertSalesCharge(SalesCharge instance);
        partial void UpdateSalesCharge(SalesCharge instance);
        partial void DeleteSalesCharge(SalesCharge instance);
        partial void InsertCostGroupCode(CostGroupCode instance);
        partial void UpdateCostGroupCode(CostGroupCode instance);
        partial void DeleteCostGroupCode(CostGroupCode instance);
        partial void InsertSalesChargeType(SalesChargeType instance);
        partial void UpdateSalesChargeType(SalesChargeType instance);
        partial void DeleteSalesChargeType(SalesChargeType instance);

        #endregion

        public OrdersDataContext() :
        base(GetConnectionString("LiveEmetalDataContextConnectionString"), mappingSource)
        {
            OnCreated();
        }

        public OrdersDataContext(MappingSource mappingSource) :
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

        public OrdersDataContext(string connection) :
            base(connection, mappingSource)
        {
          OnCreated();
        }

        public OrdersDataContext(System.Data.IDbConnection connection) :
            base(connection, mappingSource)
        {
          OnCreated();
        }

        public OrdersDataContext(string connection, MappingSource mappingSource) :
            base(connection, mappingSource)
        {
          OnCreated();
        }

        public OrdersDataContext(System.Data.IDbConnection connection, MappingSource mappingSource) :
            base(connection, mappingSource)
        {
          OnCreated();
        }

        public Devart.Data.Linq.Table<Company> Company
        {
            get
            {
                return this.GetTable<Company>();
            }
        }

        public Devart.Data.Linq.Table<ProductControl> ProductControl
        {
            get
            {
                return this.GetTable<ProductControl>();
            }
        }

        public Devart.Data.Linq.Table<ProductCategory> ProductCategory
        {
            get
            {
                return this.GetTable<ProductCategory>();
            }
        }

        public Devart.Data.Linq.Table<UnitsOfMeasure> UnitsOfMeasure
        {
            get
            {
                return this.GetTable<UnitsOfMeasure>();
            }
        }

        public Devart.Data.Linq.Table<SalesType> SalesType
        {
            get
            {
                return this.GetTable<SalesType>();
            }
        }

        public Devart.Data.Linq.Table<SalesGroup> SalesGroup
        {
            get
            {
                return this.GetTable<SalesGroup>();
            }
        }

        public Devart.Data.Linq.Table<Warehouse> Warehouse
        {
            get
            {
                return this.GetTable<Warehouse>();
            }
        }

        public Devart.Data.Linq.Table<ProductLevelAllocation> ProductLevelAllocation
        {
            get
            {
                return this.GetTable<ProductLevelAllocation>();
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

        public Devart.Data.Linq.Table<Product> Product
        {
            get
            {
                return this.GetTable<Product>();
            }
        }

        public Devart.Data.Linq.Table<ProductSubGroup> ProductSubGroup
        {
            get
            {
                return this.GetTable<ProductSubGroup>();
            }
        }

        public Devart.Data.Linq.Table<SalesItem> SalesItem
        {
            get
            {
                return this.GetTable<SalesItem>();
            }
        }

        public Devart.Data.Linq.Table<Address> Address
        {
            get
            {
                return this.GetTable<Address>();
            }
        }

        public Devart.Data.Linq.Table<SalesStatusCode> SalesStatusCode
        {
            get
            {
                return this.GetTable<SalesStatusCode>();
            }
        }

        public Devart.Data.Linq.Table<DimensionValue> DimensionValue
        {
            get
            {
                return this.GetTable<DimensionValue>();
            }
        }

        public Devart.Data.Linq.Table<TransportTypeCode> TransportTypeCode
        {
            get
            {
                return this.GetTable<TransportTypeCode>();
            }
        }

        public Devart.Data.Linq.Table<EnquiryLostReason> EnquiryLostReason
        {
            get
            {
                return this.GetTable<EnquiryLostReason>();
            }
        }

        public Devart.Data.Linq.Table<CompanySubAddress> CompanySubAddress
        {
            get
            {
                return this.GetTable<CompanySubAddress>();
            }
        }

        public Devart.Data.Linq.Table<SalesHeader> SalesHeader
        {
            get
            {
                return this.GetTable<SalesHeader>();
            }
        }

        public Devart.Data.Linq.Table<ProductClass> ProductClass
        {
            get
            {
                return this.GetTable<ProductClass>();
            }
        }

        public Devart.Data.Linq.Table<Personnel> Personnel
        {
            get
            {
                return this.GetTable<Personnel>();
            }
        }

        public Devart.Data.Linq.Table<CurrencyCode> CurrencyCode
        {
            get
            {
                return this.GetTable<CurrencyCode>();
            }
        }

        public Devart.Data.Linq.Table<SalesTotal> SalesTotal
        {
            get
            {
                return this.GetTable<SalesTotal>();
            }
        }

        public Devart.Data.Linq.Table<CostItem> CostItem
        {
            get
            {
                return this.GetTable<CostItem>();
            }
        }

        public Devart.Data.Linq.Table<SalesCharge> SalesCharge
        {
            get
            {
                return this.GetTable<SalesCharge>();
            }
        }

        public Devart.Data.Linq.Table<CostGroupCode> CostGroupCode
        {
            get
            {
                return this.GetTable<CostGroupCode>();
            }
        }

        public Devart.Data.Linq.Table<SalesChargeType> SalesChargeType
        {
            get
            {
                return this.GetTable<SalesChargeType>();
            }
        }
    }
}