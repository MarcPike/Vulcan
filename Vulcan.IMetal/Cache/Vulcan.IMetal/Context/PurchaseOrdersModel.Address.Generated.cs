﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using LinqConnect template.
// Code is generated on: 2/13/2018 9:26:28 AM
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

namespace Vulcan.IMetal.Context.PurchaseOrders
{

    /// <summary>
    /// There are no comments for Vulcan.IMetal.Context.PurchaseOrders.Address in the schema.
    /// </summary>
    [Table(Name = @"public.addresses")]
    public partial class Address : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);
        #pragma warning disable 0649

        private int _Id;

        private System.Nullable<int> _Version;

        private System.Nullable<System.DateTime> _Cdate;

        private System.Nullable<int> _CuserId;

        private System.Nullable<System.DateTime> _Mdate;

        private System.Nullable<int> _MuserId;

        private string _Status;

        private string _Address1;

        private string _Town;

        private string _County;

        private string _Postcode;

        private System.Nullable<int> _CountryId;

        private System.Nullable<int> _TransportAreaId;
        #pragma warning restore 0649

        private EntitySet<Company> _Company;

        private EntitySet<PurchaseOrderHeader> _PurchaseOrderHeader_DeliverFromAddressId;

        private EntitySet<PurchaseOrderHeader> _PurchaseOrderHeader_DeliveryAddressId;

        private EntitySet<PurchaseOrderHeader> _PurchaseOrderHeader_SupplierAddressId;

        private EntitySet<Warehouse> _Warehouse;

        private EntitySet<CompanySubAddress> _CompanySubAddress;

        private EntityRef<CountryCode> _CountryCode;
    
        #region Extensibility Method Definitions

        partial void OnLoaded();
        partial void OnValidate(ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnVersionChanging(System.Nullable<int> value);
        partial void OnVersionChanged();
        partial void OnCdateChanging(System.Nullable<System.DateTime> value);
        partial void OnCdateChanged();
        partial void OnCuserIdChanging(System.Nullable<int> value);
        partial void OnCuserIdChanged();
        partial void OnMdateChanging(System.Nullable<System.DateTime> value);
        partial void OnMdateChanged();
        partial void OnMuserIdChanging(System.Nullable<int> value);
        partial void OnMuserIdChanged();
        partial void OnStatusChanging(string value);
        partial void OnStatusChanged();
        partial void OnAddress1Changing(string value);
        partial void OnAddress1Changed();
        partial void OnTownChanging(string value);
        partial void OnTownChanged();
        partial void OnCountyChanging(string value);
        partial void OnCountyChanged();
        partial void OnPostcodeChanging(string value);
        partial void OnPostcodeChanged();
        partial void OnCountryIdChanging(System.Nullable<int> value);
        partial void OnCountryIdChanged();
        partial void OnTransportAreaIdChanging(System.Nullable<int> value);
        partial void OnTransportAreaIdChanged();
        #endregion

        public Address()
        {
            this._Company = new EntitySet<Company>(new Action<Company>(this.attach_Company), new Action<Company>(this.detach_Company));
            this._PurchaseOrderHeader_DeliverFromAddressId = new EntitySet<PurchaseOrderHeader>(new Action<PurchaseOrderHeader>(this.attach_PurchaseOrderHeader_DeliverFromAddressId), new Action<PurchaseOrderHeader>(this.detach_PurchaseOrderHeader_DeliverFromAddressId));
            this._PurchaseOrderHeader_DeliveryAddressId = new EntitySet<PurchaseOrderHeader>(new Action<PurchaseOrderHeader>(this.attach_PurchaseOrderHeader_DeliveryAddressId), new Action<PurchaseOrderHeader>(this.detach_PurchaseOrderHeader_DeliveryAddressId));
            this._PurchaseOrderHeader_SupplierAddressId = new EntitySet<PurchaseOrderHeader>(new Action<PurchaseOrderHeader>(this.attach_PurchaseOrderHeader_SupplierAddressId), new Action<PurchaseOrderHeader>(this.detach_PurchaseOrderHeader_SupplierAddressId));
            this._Warehouse = new EntitySet<Warehouse>(new Action<Warehouse>(this.attach_Warehouse), new Action<Warehouse>(this.detach_Warehouse));
            this._CompanySubAddress = new EntitySet<CompanySubAddress>(new Action<CompanySubAddress>(this.attach_CompanySubAddress), new Action<CompanySubAddress>(this.detach_CompanySubAddress));
            this._CountryCode  = default(EntityRef<CountryCode>);
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
        public System.Nullable<int> Version
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
        public System.Nullable<System.DateTime> Cdate
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
        /// There are no comments for CuserId in the schema.
        /// </summary>
        [Column(Name = @"cuser_id", Storage = "_CuserId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> CuserId
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
        /// There are no comments for Mdate in the schema.
        /// </summary>
        [Column(Name = @"mdate", Storage = "_Mdate", DbType = "TIMESTAMPTZ", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<System.DateTime> Mdate
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
        /// There are no comments for MuserId in the schema.
        /// </summary>
        [Column(Name = @"muser_id", Storage = "_MuserId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> MuserId
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
        /// There are no comments for Address1 in the schema.
        /// </summary>
        [Column(Name = @"address", Storage = "_Address1", DbType = "TEXT", UpdateCheck = UpdateCheck.Never)]
        public string Address1
        {
            get
            {
                return this._Address1;
            }
            set
            {
                if (this._Address1 != value)
                {
                    this.OnAddress1Changing(value);
                    this.SendPropertyChanging("Address1");
                    this._Address1 = value;
                    this.SendPropertyChanged("Address1");
                    this.OnAddress1Changed();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Town in the schema.
        /// </summary>
        [Column(Name = @"town", Storage = "_Town", DbType = "VARCHAR(60)", UpdateCheck = UpdateCheck.Never)]
        public string Town
        {
            get
            {
                return this._Town;
            }
            set
            {
                if (this._Town != value)
                {
                    this.OnTownChanging(value);
                    this.SendPropertyChanging("Town");
                    this._Town = value;
                    this.SendPropertyChanged("Town");
                    this.OnTownChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for County in the schema.
        /// </summary>
        [Column(Name = @"county", Storage = "_County", DbType = "VARCHAR(60)", UpdateCheck = UpdateCheck.Never)]
        public string County
        {
            get
            {
                return this._County;
            }
            set
            {
                if (this._County != value)
                {
                    this.OnCountyChanging(value);
                    this.SendPropertyChanging("County");
                    this._County = value;
                    this.SendPropertyChanged("County");
                    this.OnCountyChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Postcode in the schema.
        /// </summary>
        [Column(Name = @"postcode", Storage = "_Postcode", DbType = "VARCHAR(10)", UpdateCheck = UpdateCheck.Never)]
        public string Postcode
        {
            get
            {
                return this._Postcode;
            }
            set
            {
                if (this._Postcode != value)
                {
                    this.OnPostcodeChanging(value);
                    this.SendPropertyChanging("Postcode");
                    this._Postcode = value;
                    this.SendPropertyChanged("Postcode");
                    this.OnPostcodeChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for CountryId in the schema.
        /// </summary>
        [Column(Name = @"country_id", Storage = "_CountryId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> CountryId
        {
            get
            {
                return this._CountryId;
            }
            set
            {
                if (this._CountryId != value)
                {
                    if (this._CountryCode.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }

                    this.OnCountryIdChanging(value);
                    this.SendPropertyChanging("CountryId");
                    this._CountryId = value;
                    this.SendPropertyChanged("CountryId");
                    this.OnCountryIdChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for TransportAreaId in the schema.
        /// </summary>
        [Column(Name = @"transport_area_id", Storage = "_TransportAreaId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> TransportAreaId
        {
            get
            {
                return this._TransportAreaId;
            }
            set
            {
                if (this._TransportAreaId != value)
                {
                    this.OnTransportAreaIdChanging(value);
                    this.SendPropertyChanging("TransportAreaId");
                    this._TransportAreaId = value;
                    this.SendPropertyChanged("TransportAreaId");
                    this.OnTransportAreaIdChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Company in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="Address_Company", Storage="_Company", ThisKey="Id", OtherKey="AddressId", DeleteRule="NO ACTION")]
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
        /// There are no comments for PurchaseOrderHeader_DeliverFromAddressId in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="Address_PurchaseOrderHeader", Storage="_PurchaseOrderHeader_DeliverFromAddressId", ThisKey="Id", OtherKey="DeliverFromAddressId", DeleteRule="NO ACTION")]
        public EntitySet<PurchaseOrderHeader> PurchaseOrderHeader_DeliverFromAddressId
        {
            get
            {
                return this._PurchaseOrderHeader_DeliverFromAddressId;
            }
            set
            {
                this._PurchaseOrderHeader_DeliverFromAddressId.Assign(value);
            }
        }

    
        /// <summary>
        /// There are no comments for PurchaseOrderHeader_DeliveryAddressId in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="Address_PurchaseOrderHeader1", Storage="_PurchaseOrderHeader_DeliveryAddressId", ThisKey="Id", OtherKey="DeliveryAddressId", DeleteRule="NO ACTION")]
        public EntitySet<PurchaseOrderHeader> PurchaseOrderHeader_DeliveryAddressId
        {
            get
            {
                return this._PurchaseOrderHeader_DeliveryAddressId;
            }
            set
            {
                this._PurchaseOrderHeader_DeliveryAddressId.Assign(value);
            }
        }

    
        /// <summary>
        /// There are no comments for PurchaseOrderHeader_SupplierAddressId in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="Address_PurchaseOrderHeader2", Storage="_PurchaseOrderHeader_SupplierAddressId", ThisKey="Id", OtherKey="SupplierAddressId", DeleteRule="NO ACTION")]
        public EntitySet<PurchaseOrderHeader> PurchaseOrderHeader_SupplierAddressId
        {
            get
            {
                return this._PurchaseOrderHeader_SupplierAddressId;
            }
            set
            {
                this._PurchaseOrderHeader_SupplierAddressId.Assign(value);
            }
        }

    
        /// <summary>
        /// There are no comments for Warehouse in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="Address_Warehouse", Storage="_Warehouse", ThisKey="Id", OtherKey="AddressId", DeleteRule="NO ACTION")]
        public EntitySet<Warehouse> Warehouse
        {
            get
            {
                return this._Warehouse;
            }
            set
            {
                this._Warehouse.Assign(value);
            }
        }

    
        /// <summary>
        /// There are no comments for CompanySubAddress in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="Address_CompanySubAddress", Storage="_CompanySubAddress", ThisKey="Id", OtherKey="AddressId", DeleteRule="NO ACTION")]
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

    
        /// <summary>
        /// There are no comments for CountryCode in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="CountryCode_Address", Storage="_CountryCode", ThisKey="CountryId", OtherKey="Id", IsForeignKey=true)]
        public CountryCode CountryCode
        {
            get
            {
                return this._CountryCode.Entity;
            }
            set
            {
                CountryCode previousValue = this._CountryCode.Entity;
                if ((previousValue != value) || (this._CountryCode.HasLoadedOrAssignedValue == false))
                {
                    this.SendPropertyChanging("CountryCode");
                    if (previousValue != null)
                    {
                        this._CountryCode.Entity = null;
                        previousValue.Address.Remove(this);
                    }
                    this._CountryCode.Entity = value;
                    if (value != null)
                    {
                        this._CountryId = value.Id;
                        value.Address.Add(this);
                    }
                    else
                    {
                        this._CountryId = default(System.Nullable<int>);
                    }
                    this.SendPropertyChanged("CountryCode");
                }
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
            entity.Address = this;
        }
    
        private void detach_Company(Company entity)
        {
            this.SendPropertyChanging("Company");
            entity.Address = null;
        }

        private void attach_PurchaseOrderHeader_DeliverFromAddressId(PurchaseOrderHeader entity)
        {
            this.SendPropertyChanging("PurchaseOrderHeader_DeliverFromAddressId");
            entity.Address_DeliverFromAddressId = this;
        }
    
        private void detach_PurchaseOrderHeader_DeliverFromAddressId(PurchaseOrderHeader entity)
        {
            this.SendPropertyChanging("PurchaseOrderHeader_DeliverFromAddressId");
            entity.Address_DeliverFromAddressId = null;
        }

        private void attach_PurchaseOrderHeader_DeliveryAddressId(PurchaseOrderHeader entity)
        {
            this.SendPropertyChanging("PurchaseOrderHeader_DeliveryAddressId");
            entity.Address_DeliveryAddressId = this;
        }
    
        private void detach_PurchaseOrderHeader_DeliveryAddressId(PurchaseOrderHeader entity)
        {
            this.SendPropertyChanging("PurchaseOrderHeader_DeliveryAddressId");
            entity.Address_DeliveryAddressId = null;
        }

        private void attach_PurchaseOrderHeader_SupplierAddressId(PurchaseOrderHeader entity)
        {
            this.SendPropertyChanging("PurchaseOrderHeader_SupplierAddressId");
            entity.Address_SupplierAddressId = this;
        }
    
        private void detach_PurchaseOrderHeader_SupplierAddressId(PurchaseOrderHeader entity)
        {
            this.SendPropertyChanging("PurchaseOrderHeader_SupplierAddressId");
            entity.Address_SupplierAddressId = null;
        }

        private void attach_Warehouse(Warehouse entity)
        {
            this.SendPropertyChanging("Warehouse");
            entity.Address = this;
        }
    
        private void detach_Warehouse(Warehouse entity)
        {
            this.SendPropertyChanging("Warehouse");
            entity.Address = null;
        }

        private void attach_CompanySubAddress(CompanySubAddress entity)
        {
            this.SendPropertyChanging("CompanySubAddress");
            entity.Address = this;
        }
    
        private void detach_CompanySubAddress(CompanySubAddress entity)
        {
            this.SendPropertyChanging("CompanySubAddress");
            entity.Address = null;
        }
    }

}
