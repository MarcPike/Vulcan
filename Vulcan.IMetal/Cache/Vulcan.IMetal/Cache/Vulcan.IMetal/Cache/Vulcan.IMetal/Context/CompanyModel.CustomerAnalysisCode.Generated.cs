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
    /// There are no comments for Vulcan.IMetal.Context.Company.CustomerAnalysisCode in the schema.
    /// </summary>
    [Table(Name = @"public.customer_analysis_codes")]
    public partial class CustomerAnalysisCode : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);
        #pragma warning disable 0649

        private int _Id;

        private int? _Version;

        private System.DateTime? _Cdate;

        private int? _CuserId;

        private System.DateTime? _Mdate;

        private int? _MuserId;

        private string _Status;

        private string _Code;

        private string _Description;

        private int? _Number = 1;
        #pragma warning restore 0649

        private EntitySet<Company> _Company_AnalysisCode1Id;

        private EntitySet<Company> _Company_AnalysisCode2Id;

        private EntitySet<Company> _Company_AnalysisCode3Id;

        private EntitySet<Company> _Company_AnalysisCode4Id;

        private EntitySet<Company> _Company_AnalysisCode5Id;

        private EntitySet<Company> _Company_AnalysisCode6Id;
    
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
        partial void OnCuserIdChanging(int? value);
        partial void OnCuserIdChanged();
        partial void OnMdateChanging(System.DateTime? value);
        partial void OnMdateChanged();
        partial void OnMuserIdChanging(int? value);
        partial void OnMuserIdChanged();
        partial void OnStatusChanging(string value);
        partial void OnStatusChanged();
        partial void OnCodeChanging(string value);
        partial void OnCodeChanged();
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        partial void OnNumberChanging(int? value);
        partial void OnNumberChanged();
        #endregion

        public CustomerAnalysisCode()
        {
            this._Company_AnalysisCode1Id = new EntitySet<Company>(new Action<Company>(this.attach_Company_AnalysisCode1Id), new Action<Company>(this.detach_Company_AnalysisCode1Id));
            this._Company_AnalysisCode2Id = new EntitySet<Company>(new Action<Company>(this.attach_Company_AnalysisCode2Id), new Action<Company>(this.detach_Company_AnalysisCode2Id));
            this._Company_AnalysisCode3Id = new EntitySet<Company>(new Action<Company>(this.attach_Company_AnalysisCode3Id), new Action<Company>(this.detach_Company_AnalysisCode3Id));
            this._Company_AnalysisCode4Id = new EntitySet<Company>(new Action<Company>(this.attach_Company_AnalysisCode4Id), new Action<Company>(this.detach_Company_AnalysisCode4Id));
            this._Company_AnalysisCode5Id = new EntitySet<Company>(new Action<Company>(this.attach_Company_AnalysisCode5Id), new Action<Company>(this.detach_Company_AnalysisCode5Id));
            this._Company_AnalysisCode6Id = new EntitySet<Company>(new Action<Company>(this.attach_Company_AnalysisCode6Id), new Action<Company>(this.detach_Company_AnalysisCode6Id));
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
        [Column(Name = @"code", Storage = "_Code", DbType = "VARCHAR(60)", UpdateCheck = UpdateCheck.Never)]
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
        /// There are no comments for Number in the schema.
        /// </summary>
        [Column(Name = @"""number""", Storage = "_Number", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public int? Number
        {
            get
            {
                return this._Number;
            }
            set
            {
                if (this._Number != value)
                {
                    this.OnNumberChanging(value);
                    this.SendPropertyChanging("Number");
                    this._Number = value;
                    this.SendPropertyChanged("Number");
                    this.OnNumberChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Company_AnalysisCode1Id in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="CustomerAnalysisCode_Company", Storage="_Company_AnalysisCode1Id", ThisKey="Id", OtherKey="AnalysisCode1Id", DeleteRule="NO ACTION")]
        public EntitySet<Company> Company_AnalysisCode1Id
        {
            get
            {
                return this._Company_AnalysisCode1Id;
            }
            set
            {
                this._Company_AnalysisCode1Id.Assign(value);
            }
        }

    
        /// <summary>
        /// There are no comments for Company_AnalysisCode2Id in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="CustomerAnalysisCode_Company1", Storage="_Company_AnalysisCode2Id", ThisKey="Id", OtherKey="AnalysisCode2Id", DeleteRule="NO ACTION")]
        public EntitySet<Company> Company_AnalysisCode2Id
        {
            get
            {
                return this._Company_AnalysisCode2Id;
            }
            set
            {
                this._Company_AnalysisCode2Id.Assign(value);
            }
        }

    
        /// <summary>
        /// There are no comments for Company_AnalysisCode3Id in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="CustomerAnalysisCode_Company2", Storage="_Company_AnalysisCode3Id", ThisKey="Id", OtherKey="AnalysisCode3Id", DeleteRule="NO ACTION")]
        public EntitySet<Company> Company_AnalysisCode3Id
        {
            get
            {
                return this._Company_AnalysisCode3Id;
            }
            set
            {
                this._Company_AnalysisCode3Id.Assign(value);
            }
        }

    
        /// <summary>
        /// There are no comments for Company_AnalysisCode4Id in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="CustomerAnalysisCode_Company3", Storage="_Company_AnalysisCode4Id", ThisKey="Id", OtherKey="AnalysisCode4Id", DeleteRule="NO ACTION")]
        public EntitySet<Company> Company_AnalysisCode4Id
        {
            get
            {
                return this._Company_AnalysisCode4Id;
            }
            set
            {
                this._Company_AnalysisCode4Id.Assign(value);
            }
        }

    
        /// <summary>
        /// There are no comments for Company_AnalysisCode5Id in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="CustomerAnalysisCode_Company4", Storage="_Company_AnalysisCode5Id", ThisKey="Id", OtherKey="AnalysisCode5Id", DeleteRule="NO ACTION")]
        public EntitySet<Company> Company_AnalysisCode5Id
        {
            get
            {
                return this._Company_AnalysisCode5Id;
            }
            set
            {
                this._Company_AnalysisCode5Id.Assign(value);
            }
        }

    
        /// <summary>
        /// There are no comments for Company_AnalysisCode6Id in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="CustomerAnalysisCode_Company5", Storage="_Company_AnalysisCode6Id", ThisKey="Id", OtherKey="AnalysisCode6Id", DeleteRule="NO ACTION")]
        public EntitySet<Company> Company_AnalysisCode6Id
        {
            get
            {
                return this._Company_AnalysisCode6Id;
            }
            set
            {
                this._Company_AnalysisCode6Id.Assign(value);
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

        private void attach_Company_AnalysisCode1Id(Company entity)
        {
            this.SendPropertyChanging("Company_AnalysisCode1Id");
            entity.CustomerAnalysisCode_AnalysisCode1Id = this;
        }
    
        private void detach_Company_AnalysisCode1Id(Company entity)
        {
            this.SendPropertyChanging("Company_AnalysisCode1Id");
            entity.CustomerAnalysisCode_AnalysisCode1Id = null;
        }

        private void attach_Company_AnalysisCode2Id(Company entity)
        {
            this.SendPropertyChanging("Company_AnalysisCode2Id");
            entity.CustomerAnalysisCode_AnalysisCode2Id = this;
        }
    
        private void detach_Company_AnalysisCode2Id(Company entity)
        {
            this.SendPropertyChanging("Company_AnalysisCode2Id");
            entity.CustomerAnalysisCode_AnalysisCode2Id = null;
        }

        private void attach_Company_AnalysisCode3Id(Company entity)
        {
            this.SendPropertyChanging("Company_AnalysisCode3Id");
            entity.CustomerAnalysisCode_AnalysisCode3Id = this;
        }
    
        private void detach_Company_AnalysisCode3Id(Company entity)
        {
            this.SendPropertyChanging("Company_AnalysisCode3Id");
            entity.CustomerAnalysisCode_AnalysisCode3Id = null;
        }

        private void attach_Company_AnalysisCode4Id(Company entity)
        {
            this.SendPropertyChanging("Company_AnalysisCode4Id");
            entity.CustomerAnalysisCode_AnalysisCode4Id = this;
        }
    
        private void detach_Company_AnalysisCode4Id(Company entity)
        {
            this.SendPropertyChanging("Company_AnalysisCode4Id");
            entity.CustomerAnalysisCode_AnalysisCode4Id = null;
        }

        private void attach_Company_AnalysisCode5Id(Company entity)
        {
            this.SendPropertyChanging("Company_AnalysisCode5Id");
            entity.CustomerAnalysisCode_AnalysisCode5Id = this;
        }
    
        private void detach_Company_AnalysisCode5Id(Company entity)
        {
            this.SendPropertyChanging("Company_AnalysisCode5Id");
            entity.CustomerAnalysisCode_AnalysisCode5Id = null;
        }

        private void attach_Company_AnalysisCode6Id(Company entity)
        {
            this.SendPropertyChanging("Company_AnalysisCode6Id");
            entity.CustomerAnalysisCode_AnalysisCode6Id = this;
        }
    
        private void detach_Company_AnalysisCode6Id(Company entity)
        {
            this.SendPropertyChanging("Company_AnalysisCode6Id");
            entity.CustomerAnalysisCode_AnalysisCode6Id = null;
        }
    }

}
