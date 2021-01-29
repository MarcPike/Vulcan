﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using LinqConnect template.
// Code is generated on: 9/3/2020 10:11:09 AM
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
    /// There are no comments for Vulcan.IMetal.Context.Company.Term in the schema.
    /// </summary>
    [Table(Name = @"public.terms")]
    public partial class Term : INotifyPropertyChanging, INotifyPropertyChanged
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

        private int? _DueDay;

        private int? _DueMonthEnd;
        #pragma warning restore 0649

        private EntitySet<Company> _Company;

        private EntitySet<SalesHeader> _SalesHeader;

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
        partial void OnDueDayChanging(int? value);
        partial void OnDueDayChanged();
        partial void OnDueMonthEndChanging(int? value);
        partial void OnDueMonthEndChanged();
        #endregion

        public Term()
        {
            this._Company = new EntitySet<Company>(new Action<Company>(this.attach_Company), new Action<Company>(this.detach_Company));
            this._SalesHeader = new EntitySet<SalesHeader>(new Action<SalesHeader>(this.attach_SalesHeader), new Action<SalesHeader>(this.detach_SalesHeader));
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
        /// There are no comments for DueDay in the schema.
        /// </summary>
        [Column(Name = @"due_days", Storage = "_DueDay", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public int? DueDay
        {
            get
            {
                return this._DueDay;
            }
            set
            {
                if (this._DueDay != value)
                {
                    this.OnDueDayChanging(value);
                    this.SendPropertyChanging("DueDay");
                    this._DueDay = value;
                    this.SendPropertyChanged("DueDay");
                    this.OnDueDayChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for DueMonthEnd in the schema.
        /// </summary>
        [Column(Name = @"due_month_ends", Storage = "_DueMonthEnd", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public int? DueMonthEnd
        {
            get
            {
                return this._DueMonthEnd;
            }
            set
            {
                if (this._DueMonthEnd != value)
                {
                    this.OnDueMonthEndChanging(value);
                    this.SendPropertyChanging("DueMonthEnd");
                    this._DueMonthEnd = value;
                    this.SendPropertyChanged("DueMonthEnd");
                    this.OnDueMonthEndChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Company in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="Term_Company", Storage="_Company", ThisKey="Id", OtherKey="TermsId", DeleteRule="NO ACTION")]
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
        /// There are no comments for SalesHeader in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="Term_SalesHeader", Storage="_SalesHeader", ThisKey="Id", OtherKey="TermsId", DeleteRule="NO ACTION")]
        public EntitySet<SalesHeader> SalesHeader
        {
            get
            {
                return this._SalesHeader;
            }
            set
            {
                this._SalesHeader.Assign(value);
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
            entity.Term = this;
        }
    
        private void detach_Company(Company entity)
        {
            this.SendPropertyChanging("Company");
            entity.Term = null;
        }

        private void attach_SalesHeader(SalesHeader entity)
        {
            this.SendPropertyChanging("SalesHeader");
            entity.Term = this;
        }
    
        private void detach_SalesHeader(SalesHeader entity)
        {
            this.SendPropertyChanging("SalesHeader");
            entity.Term = null;
        }
    }

}
