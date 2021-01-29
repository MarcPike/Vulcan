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

    /// <summary>
    /// There are no comments for Vulcan.IMetal.Context.Company.TransportTypeCode in the schema.
    /// </summary>
    [Table(Name = @"public.transport_type_codes")]
    public partial class TransportTypeCode : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);
        #pragma warning disable 0649

        private int _Id;

        private int? _Version;

        private System.DateTime? _Cdate;

        private System.DateTime? _Mdate;

        private int? _CuserId;

        private int? _MuserId;

        private string _Status;

        private string _Code;

        private string _Description;

        private bool? _ExternalCost;

        private bool? _InternalCost;

        private bool? _ExternalCharge;

        private bool? _InclusiveCharge;

        private string _Type;

        private bool? _PlanTransport = true;

        private int? _SalesServiceProductId;

        private int? _PurchaseGroupId;
        #pragma warning restore 0649

        private EntitySet<Company> _Company;

        private EntitySet<CompanySubAddress> _CompanySubAddress;
    
        #region Extensibility Method Definitions

        partial void OnLoaded();
        partial void OnValidate(ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnVersionChanging(int? value);
        partial void OnVersionChanged();
        partial void OnCdateChanging(System.DateTime? value);
        partial void OnCdateChanged();
        partial void OnMdateChanging(System.DateTime? value);
        partial void OnMdateChanged();
        partial void OnCuserIdChanging(int? value);
        partial void OnCuserIdChanged();
        partial void OnMuserIdChanging(int? value);
        partial void OnMuserIdChanged();
        partial void OnStatusChanging(string value);
        partial void OnStatusChanged();
        partial void OnCodeChanging(string value);
        partial void OnCodeChanged();
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        partial void OnExternalCostChanging(bool? value);
        partial void OnExternalCostChanged();
        partial void OnInternalCostChanging(bool? value);
        partial void OnInternalCostChanged();
        partial void OnExternalChargeChanging(bool? value);
        partial void OnExternalChargeChanged();
        partial void OnInclusiveChargeChanging(bool? value);
        partial void OnInclusiveChargeChanged();
        partial void OnTypeChanging(string value);
        partial void OnTypeChanged();
        partial void OnPlanTransportChanging(bool? value);
        partial void OnPlanTransportChanged();
        partial void OnSalesServiceProductIdChanging(int? value);
        partial void OnSalesServiceProductIdChanged();
        partial void OnPurchaseGroupIdChanging(int? value);
        partial void OnPurchaseGroupIdChanged();
        #endregion

        public TransportTypeCode()
        {
            this._Company = new EntitySet<Company>(new Action<Company>(this.attach_Company), new Action<Company>(this.detach_Company));
            this._CompanySubAddress = new EntitySet<CompanySubAddress>(new Action<CompanySubAddress>(this.attach_CompanySubAddress), new Action<CompanySubAddress>(this.detach_CompanySubAddress));
            OnCreated();
        }

    
        /// <summary>
        /// There are no comments for Id in the schema.
        /// </summary>
        [Column(Name = @"id", Storage = "_Id", AutoSync = AutoSync.OnInsert, CanBeNull = false, DbType = "SERIAL NOT NULL", IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if (this._Id != value)
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging("Id");
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Version in the schema.
        /// </summary>
        [Column(Name = @"""version""", Storage = "_Version", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public int? Version
        {
            get
            {
                return this._Version;
            }
            set
            {
                if (this._Version != value)
                {
                    this.OnVersionChanging(value);
                    this.SendPropertyChanging("Version");
                    this._Version = value;
                    this.SendPropertyChanged("Version");
                    this.OnVersionChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Cdate in the schema.
        /// </summary>
        [Column(Name = @"cdate", Storage = "_Cdate", DbType = "TIMESTAMPTZ", UpdateCheck = UpdateCheck.Never)]
        public System.DateTime? Cdate
        {
            get
            {
                return this._Cdate;
            }
            set
            {
                if (this._Cdate != value)
                {
                    this.OnCdateChanging(value);
                    this.SendPropertyChanging("Cdate");
                    this._Cdate = value;
                    this.SendPropertyChanged("Cdate");
                    this.OnCdateChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Mdate in the schema.
        /// </summary>
        [Column(Name = @"mdate", Storage = "_Mdate", DbType = "TIMESTAMPTZ", UpdateCheck = UpdateCheck.Never)]
        public System.DateTime? Mdate
        {
            get
            {
                return this._Mdate;
            }
            set
            {
                if (this._Mdate != value)
                {
                    this.OnMdateChanging(value);
                    this.SendPropertyChanging("Mdate");
                    this._Mdate = value;
                    this.SendPropertyChanged("Mdate");
                    this.OnMdateChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for CuserId in the schema.
        /// </summary>
        [Column(Name = @"cuser_id", Storage = "_CuserId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public int? CuserId
        {
            get
            {
                return this._CuserId;
            }
            set
            {
                if (this._CuserId != value)
                {
                    this.OnCuserIdChanging(value);
                    this.SendPropertyChanging("CuserId");
                    this._CuserId = value;
                    this.SendPropertyChanged("CuserId");
                    this.OnCuserIdChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for MuserId in the schema.
        /// </summary>
        [Column(Name = @"muser_id", Storage = "_MuserId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public int? MuserId
        {
            get
            {
                return this._MuserId;
            }
            set
            {
                if (this._MuserId != value)
                {
                    this.OnMuserIdChanging(value);
                    this.SendPropertyChanging("MuserId");
                    this._MuserId = value;
                    this.SendPropertyChanged("MuserId");
                    this.OnMuserIdChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Status in the schema.
        /// </summary>
        [Column(Name = @"status", Storage = "_Status", DbType = "CHAR(1)", UpdateCheck = UpdateCheck.Never)]
        public string Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                if (this._Status != value)
                {
                    this.OnStatusChanging(value);
                    this.SendPropertyChanging("Status");
                    this._Status = value;
                    this.SendPropertyChanged("Status");
                    this.OnStatusChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Code in the schema.
        /// </summary>
        [Column(Name = @"code", Storage = "_Code", DbType = "VARCHAR(6)", UpdateCheck = UpdateCheck.Never)]
        public string Code
        {
            get
            {
                return this._Code;
            }
            set
            {
                if (this._Code != value)
                {
                    this.OnCodeChanging(value);
                    this.SendPropertyChanging("Code");
                    this._Code = value;
                    this.SendPropertyChanged("Code");
                    this.OnCodeChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Description in the schema.
        /// </summary>
        [Column(Name = @"description", Storage = "_Description", DbType = "VARCHAR(255)", UpdateCheck = UpdateCheck.Never)]
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                if (this._Description != value)
                {
                    this.OnDescriptionChanging(value);
                    this.SendPropertyChanging("Description");
                    this._Description = value;
                    this.SendPropertyChanged("Description");
                    this.OnDescriptionChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for ExternalCost in the schema.
        /// </summary>
        [Column(Name = @"external_cost", Storage = "_ExternalCost", DbType = "BOOL", UpdateCheck = UpdateCheck.Never)]
        public bool? ExternalCost
        {
            get
            {
                return this._ExternalCost;
            }
            set
            {
                if (this._ExternalCost != value)
                {
                    this.OnExternalCostChanging(value);
                    this.SendPropertyChanging("ExternalCost");
                    this._ExternalCost = value;
                    this.SendPropertyChanged("ExternalCost");
                    this.OnExternalCostChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for InternalCost in the schema.
        /// </summary>
        [Column(Name = @"internal_cost", Storage = "_InternalCost", DbType = "BOOL", UpdateCheck = UpdateCheck.Never)]
        public bool? InternalCost
        {
            get
            {
                return this._InternalCost;
            }
            set
            {
                if (this._InternalCost != value)
                {
                    this.OnInternalCostChanging(value);
                    this.SendPropertyChanging("InternalCost");
                    this._InternalCost = value;
                    this.SendPropertyChanged("InternalCost");
                    this.OnInternalCostChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for ExternalCharge in the schema.
        /// </summary>
        [Column(Name = @"external_charge", Storage = "_ExternalCharge", DbType = "BOOL", UpdateCheck = UpdateCheck.Never)]
        public bool? ExternalCharge
        {
            get
            {
                return this._ExternalCharge;
            }
            set
            {
                if (this._ExternalCharge != value)
                {
                    this.OnExternalChargeChanging(value);
                    this.SendPropertyChanging("ExternalCharge");
                    this._ExternalCharge = value;
                    this.SendPropertyChanged("ExternalCharge");
                    this.OnExternalChargeChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for InclusiveCharge in the schema.
        /// </summary>
        [Column(Name = @"inclusive_charge", Storage = "_InclusiveCharge", DbType = "BOOL", UpdateCheck = UpdateCheck.Never)]
        public bool? InclusiveCharge
        {
            get
            {
                return this._InclusiveCharge;
            }
            set
            {
                if (this._InclusiveCharge != value)
                {
                    this.OnInclusiveChargeChanging(value);
                    this.SendPropertyChanging("InclusiveCharge");
                    this._InclusiveCharge = value;
                    this.SendPropertyChanged("InclusiveCharge");
                    this.OnInclusiveChargeChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Type in the schema.
        /// </summary>
        [Column(Name = @"""type""", Storage = "_Type", DbType = "CHAR(1)", UpdateCheck = UpdateCheck.Never)]
        public string Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                if (this._Type != value)
                {
                    this.OnTypeChanging(value);
                    this.SendPropertyChanging("Type");
                    this._Type = value;
                    this.SendPropertyChanged("Type");
                    this.OnTypeChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for PlanTransport in the schema.
        /// </summary>
        [Column(Name = @"plan_transport", Storage = "_PlanTransport", DbType = "BOOL", UpdateCheck = UpdateCheck.Never)]
        public bool? PlanTransport
        {
            get
            {
                return this._PlanTransport;
            }
            set
            {
                if (this._PlanTransport != value)
                {
                    this.OnPlanTransportChanging(value);
                    this.SendPropertyChanging("PlanTransport");
                    this._PlanTransport = value;
                    this.SendPropertyChanged("PlanTransport");
                    this.OnPlanTransportChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for SalesServiceProductId in the schema.
        /// </summary>
        [Column(Name = @"sales_service_product_id", Storage = "_SalesServiceProductId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public int? SalesServiceProductId
        {
            get
            {
                return this._SalesServiceProductId;
            }
            set
            {
                if (this._SalesServiceProductId != value)
                {
                    this.OnSalesServiceProductIdChanging(value);
                    this.SendPropertyChanging("SalesServiceProductId");
                    this._SalesServiceProductId = value;
                    this.SendPropertyChanged("SalesServiceProductId");
                    this.OnSalesServiceProductIdChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for PurchaseGroupId in the schema.
        /// </summary>
        [Column(Name = @"purchase_group_id", Storage = "_PurchaseGroupId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public int? PurchaseGroupId
        {
            get
            {
                return this._PurchaseGroupId;
            }
            set
            {
                if (this._PurchaseGroupId != value)
                {
                    this.OnPurchaseGroupIdChanging(value);
                    this.SendPropertyChanging("PurchaseGroupId");
                    this._PurchaseGroupId = value;
                    this.SendPropertyChanged("PurchaseGroupId");
                    this.OnPurchaseGroupIdChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Company in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="TransportTypeCode_Company", Storage="_Company", ThisKey="Id", OtherKey="DefaultTransportTypeId", DeleteRule="NO ACTION")]
        public EntitySet<Company> Company
        {
            get
            {
                return this._Company;
            }
            set
            {
                this._Company.Assign(value);
            }
        }

    
        /// <summary>
        /// There are no comments for CompanySubAddress in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="TransportTypeCode_CompanySubAddress", Storage="_CompanySubAddress", ThisKey="Id", OtherKey="DefaultTransportTypeId", DeleteRule="NO ACTION")]
        public EntitySet<CompanySubAddress> CompanySubAddress
        {
            get
            {
                return this._CompanySubAddress;
            }
            set
            {
                this._CompanySubAddress.Assign(value);
            }
        }
   
        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
		        var handler = this.PropertyChanging;
            if (handler != null)
                handler(this, emptyChangingEventArgs);
        }

        protected virtual void SendPropertyChanging(System.String propertyName) 
        {    
		        var handler = this.PropertyChanging;
            if (handler != null)
                handler(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void SendPropertyChanged(System.String propertyName)
        {    
		        var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void attach_Company(Company entity)
        {
            this.SendPropertyChanging("Company");
            entity.TransportTypeCode = this;
        }
    
        private void detach_Company(Company entity)
        {
            this.SendPropertyChanging("Company");
            entity.TransportTypeCode = null;
        }

        private void attach_CompanySubAddress(CompanySubAddress entity)
        {
            this.SendPropertyChanging("CompanySubAddress");
            entity.TransportTypeCode = this;
        }
    
        private void detach_CompanySubAddress(CompanySubAddress entity)
        {
            this.SendPropertyChanging("CompanySubAddress");
            entity.TransportTypeCode = null;
        }
    }

}
