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

    /// <summary>
    /// There are no comments for Vulcan.IMetal.Context.Orders.SalesStatusCode in the schema.
    /// </summary>
    [Table(Name = @"public.sales_status_codes")]
    public partial class SalesStatusCode : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);
        #pragma warning disable 0649

        private int _Id;

        private System.Nullable<int> _Version;

        private System.Nullable<System.DateTime> _Cdate;

        private System.Nullable<System.DateTime> _Mdate;

        private System.Nullable<int> _CuserId;

        private System.Nullable<int> _MuserId;

        private string _Status;

        private string _Code;

        private string _Description;

        private System.Nullable<int> _InternalStatusId;

        private System.Nullable<int> _Sequence;

        private string _AdditionalDescription1;

        private string _AdditionalDescription2;
        #pragma warning restore 0649

        private EntitySet<SalesItem> _SalesItem;

        private EntitySet<SalesHeader> _SalesHeader;
    
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
        partial void OnMdateChanging(System.Nullable<System.DateTime> value);
        partial void OnMdateChanged();
        partial void OnCuserIdChanging(System.Nullable<int> value);
        partial void OnCuserIdChanged();
        partial void OnMuserIdChanging(System.Nullable<int> value);
        partial void OnMuserIdChanged();
        partial void OnStatusChanging(string value);
        partial void OnStatusChanged();
        partial void OnCodeChanging(string value);
        partial void OnCodeChanged();
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        partial void OnInternalStatusIdChanging(System.Nullable<int> value);
        partial void OnInternalStatusIdChanged();
        partial void OnSequenceChanging(System.Nullable<int> value);
        partial void OnSequenceChanged();
        partial void OnAdditionalDescription1Changing(string value);
        partial void OnAdditionalDescription1Changed();
        partial void OnAdditionalDescription2Changing(string value);
        partial void OnAdditionalDescription2Changed();
        #endregion

        public SalesStatusCode()
        {
            this._SalesItem = new EntitySet<SalesItem>(new Action<SalesItem>(this.attach_SalesItem), new Action<SalesItem>(this.detach_SalesItem));
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
        /// There are no comments for Code in the schema.
        /// </summary>
        [Column(Name = @"code", Storage = "_Code", DbType = "VARCHAR(3)", UpdateCheck = UpdateCheck.Never)]
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
        /// There are no comments for InternalStatusId in the schema.
        /// </summary>
        [Column(Name = @"internal_status_id", Storage = "_InternalStatusId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> InternalStatusId
        {
            get
            {
                return this._InternalStatusId;
            }
            set
            {
                if (this._InternalStatusId != value)
                {
                    this.OnInternalStatusIdChanging(value);
                    this.SendPropertyChanging("InternalStatusId");
                    this._InternalStatusId = value;
                    this.SendPropertyChanged("InternalStatusId");
                    this.OnInternalStatusIdChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Sequence in the schema.
        /// </summary>
        [Column(Name = @"""sequence""", Storage = "_Sequence", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> Sequence
        {
            get
            {
                return this._Sequence;
            }
            set
            {
                if (this._Sequence != value)
                {
                    this.OnSequenceChanging(value);
                    this.SendPropertyChanging("Sequence");
                    this._Sequence = value;
                    this.SendPropertyChanged("Sequence");
                    this.OnSequenceChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for AdditionalDescription1 in the schema.
        /// </summary>
        [Column(Name = @"additional_description1", Storage = "_AdditionalDescription1", DbType = "VARCHAR(255)", UpdateCheck = UpdateCheck.Never)]
        public string AdditionalDescription1
        {
            get
            {
                return this._AdditionalDescription1;
            }
            set
            {
                if (this._AdditionalDescription1 != value)
                {
                    this.OnAdditionalDescription1Changing(value);
                    this.SendPropertyChanging("AdditionalDescription1");
                    this._AdditionalDescription1 = value;
                    this.SendPropertyChanged("AdditionalDescription1");
                    this.OnAdditionalDescription1Changed();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for AdditionalDescription2 in the schema.
        /// </summary>
        [Column(Name = @"additional_description2", Storage = "_AdditionalDescription2", DbType = "VARCHAR(255)", UpdateCheck = UpdateCheck.Never)]
        public string AdditionalDescription2
        {
            get
            {
                return this._AdditionalDescription2;
            }
            set
            {
                if (this._AdditionalDescription2 != value)
                {
                    this.OnAdditionalDescription2Changing(value);
                    this.SendPropertyChanging("AdditionalDescription2");
                    this._AdditionalDescription2 = value;
                    this.SendPropertyChanged("AdditionalDescription2");
                    this.OnAdditionalDescription2Changed();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for SalesItem in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="SalesStatusCode_SalesItem", Storage="_SalesItem", ThisKey="Id", OtherKey="StatusId", DeleteRule="NO ACTION")]
        public EntitySet<SalesItem> SalesItem
        {
            get
            {
                return this._SalesItem;
            }
            set
            {
                this._SalesItem.Assign(value);
            }
        }

    
        /// <summary>
        /// There are no comments for SalesHeader in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="SalesStatusCode_SalesHeader", Storage="_SalesHeader", ThisKey="Id", OtherKey="StatusId", DeleteRule="NO ACTION")]
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

        private void attach_SalesItem(SalesItem entity)
        {
            this.SendPropertyChanging("SalesItem");
            entity.SalesStatusCode = this;
        }
    
        private void detach_SalesItem(SalesItem entity)
        {
            this.SendPropertyChanging("SalesItem");
            entity.SalesStatusCode = null;
        }

        private void attach_SalesHeader(SalesHeader entity)
        {
            this.SendPropertyChanging("SalesHeader");
            entity.SalesStatusCode = this;
        }
    
        private void detach_SalesHeader(SalesHeader entity)
        {
            this.SendPropertyChanging("SalesHeader");
            entity.SalesStatusCode = null;
        }
    }

}
