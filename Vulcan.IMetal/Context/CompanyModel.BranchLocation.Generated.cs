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
    /// There are no comments for Vulcan.IMetal.Context.Company.BranchLocation in the schema.
    /// </summary>
    [Table(Name = @"public.branch_locations")]
    public partial class BranchLocation : INotifyPropertyChanging, INotifyPropertyChanged
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

        private int? _BranchId;

        private string _Location;
        #pragma warning restore 0649

        private EntityRef<Branch> _Branch;
    
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
        partial void OnBranchIdChanging(int? value);
        partial void OnBranchIdChanged();
        partial void OnLocationChanging(string value);
        partial void OnLocationChanged();
        #endregion

        public BranchLocation()
        {
            this._Branch  = default(EntityRef<Branch>);
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
        /// There are no comments for BranchId in the schema.
        /// </summary>
        [Column(Name = @"branch_id", Storage = "_BranchId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public int? BranchId
        {
            get
            {
                return this._BranchId;
            }
            set
            {
                if (this._BranchId != value)
                {
                    if (this._Branch.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }

                    this.OnBranchIdChanging(value);
                    this.SendPropertyChanging("BranchId");
                    this._BranchId = value;
                    this.SendPropertyChanged("BranchId");
                    this.OnBranchIdChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Location in the schema.
        /// </summary>
        [Column(Name = @"""location""", Storage = "_Location", DbType = "VARCHAR(16)", UpdateCheck = UpdateCheck.Never)]
        public string Location
        {
            get
            {
                return this._Location;
            }
            set
            {
                if (this._Location != value)
                {
                    this.OnLocationChanging(value);
                    this.SendPropertyChanging("Location");
                    this._Location = value;
                    this.SendPropertyChanged("Location");
                    this.OnLocationChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for Branch in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="Branch_BranchLocation", Storage="_Branch", ThisKey="BranchId", OtherKey="Id", IsForeignKey=true)]
        public Branch Branch
        {
            get
            {
                return this._Branch.Entity;
            }
            set
            {
                Branch previousValue = this._Branch.Entity;
                if ((previousValue != value) || (this._Branch.HasLoadedOrAssignedValue == false))
                {
                    this.SendPropertyChanging("Branch");
                    if (previousValue != null)
                    {
                        this._Branch.Entity = null;
                        previousValue.BranchLocation.Remove(this);
                    }
                    this._Branch.Entity = value;
                    if (value != null)
                    {
                        this._BranchId = value.Id;
                        value.BranchLocation.Add(this);
                    }
                    else
                    {
                        this._BranchId = default(int?);
                    }
                    this.SendPropertyChanged("Branch");
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
    }

}
