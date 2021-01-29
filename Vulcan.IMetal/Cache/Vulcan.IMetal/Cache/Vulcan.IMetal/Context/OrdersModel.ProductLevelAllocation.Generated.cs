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
    /// There are no comments for Vulcan.IMetal.Context.Orders.ProductLevelAllocation in the schema.
    /// </summary>
    [Table(Name = @"public.product_level_allocation")]
    public partial class ProductLevelAllocation : INotifyPropertyChanging, INotifyPropertyChanged
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

        private string _AllocationType;

        private System.Nullable<int> _SalesItemId;

        private System.Nullable<int> _ProcessRequestId;

        private System.Nullable<int> _ProcessGroupId;

        private System.Nullable<int> _CacheOfProductIdForSearching;

        private System.Nullable<decimal> _CacheOfDim1ForSearching;

        private System.Nullable<decimal> _CacheOfDim2ForSearching;

        private System.Nullable<decimal> _CacheOfDim3ForSearching;

        private System.Nullable<decimal> _CacheOfDim4ForSearching;

        private System.Nullable<decimal> _CacheOfDim5ForSearching;

        private System.Nullable<int> _CacheOfBranchIdForSearching;

        private System.Nullable<System.DateTime> _PrintedDate;

        private System.Nullable<bool> _StockAllocated = false;

        private System.Nullable<bool> _ReadyStockAllocated = false;
        #pragma warning restore 0649

        private EntityRef<SalesItem> _SalesItem_SalesItemId;

        private EntitySet<SalesItem> _SalesItem_ProductLevelAllocationId;
    
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
        partial void OnAllocationTypeChanging(string value);
        partial void OnAllocationTypeChanged();
        partial void OnSalesItemIdChanging(System.Nullable<int> value);
        partial void OnSalesItemIdChanged();
        partial void OnProcessRequestIdChanging(System.Nullable<int> value);
        partial void OnProcessRequestIdChanged();
        partial void OnProcessGroupIdChanging(System.Nullable<int> value);
        partial void OnProcessGroupIdChanged();
        partial void OnCacheOfProductIdForSearchingChanging(System.Nullable<int> value);
        partial void OnCacheOfProductIdForSearchingChanged();
        partial void OnCacheOfDim1ForSearchingChanging(System.Nullable<decimal> value);
        partial void OnCacheOfDim1ForSearchingChanged();
        partial void OnCacheOfDim2ForSearchingChanging(System.Nullable<decimal> value);
        partial void OnCacheOfDim2ForSearchingChanged();
        partial void OnCacheOfDim3ForSearchingChanging(System.Nullable<decimal> value);
        partial void OnCacheOfDim3ForSearchingChanged();
        partial void OnCacheOfDim4ForSearchingChanging(System.Nullable<decimal> value);
        partial void OnCacheOfDim4ForSearchingChanged();
        partial void OnCacheOfDim5ForSearchingChanging(System.Nullable<decimal> value);
        partial void OnCacheOfDim5ForSearchingChanged();
        partial void OnCacheOfBranchIdForSearchingChanging(System.Nullable<int> value);
        partial void OnCacheOfBranchIdForSearchingChanged();
        partial void OnPrintedDateChanging(System.Nullable<System.DateTime> value);
        partial void OnPrintedDateChanged();
        partial void OnStockAllocatedChanging(System.Nullable<bool> value);
        partial void OnStockAllocatedChanged();
        partial void OnReadyStockAllocatedChanging(System.Nullable<bool> value);
        partial void OnReadyStockAllocatedChanged();
        #endregion

        public ProductLevelAllocation()
        {
            this._SalesItem_SalesItemId  = default(EntityRef<SalesItem>);
            this._SalesItem_ProductLevelAllocationId = new EntitySet<SalesItem>(new Action<SalesItem>(this.attach_SalesItem_ProductLevelAllocationId), new Action<SalesItem>(this.detach_SalesItem_ProductLevelAllocationId));
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
        /// There are no comments for AllocationType in the schema.
        /// </summary>
        [Column(Name = @"allocation_type", Storage = "_AllocationType", DbType = "CHAR(1)", UpdateCheck = UpdateCheck.Never)]
        public string AllocationType
        {
            get
            {
                return this._AllocationType;
            }
            set
            {
                if (this._AllocationType != value)
                {
                    this.OnAllocationTypeChanging(value);
                    this.SendPropertyChanging("AllocationType");
                    this._AllocationType = value;
                    this.SendPropertyChanged("AllocationType");
                    this.OnAllocationTypeChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for SalesItemId in the schema.
        /// </summary>
        [Column(Name = @"sales_item_id", Storage = "_SalesItemId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> SalesItemId
        {
            get
            {
                return this._SalesItemId;
            }
            set
            {
                if (this._SalesItemId != value)
                {
                    if (this._SalesItem_SalesItemId.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }

                    this.OnSalesItemIdChanging(value);
                    this.SendPropertyChanging("SalesItemId");
                    this._SalesItemId = value;
                    this.SendPropertyChanged("SalesItemId");
                    this.OnSalesItemIdChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for ProcessRequestId in the schema.
        /// </summary>
        [Column(Name = @"process_request_id", Storage = "_ProcessRequestId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> ProcessRequestId
        {
            get
            {
                return this._ProcessRequestId;
            }
            set
            {
                if (this._ProcessRequestId != value)
                {
                    this.OnProcessRequestIdChanging(value);
                    this.SendPropertyChanging("ProcessRequestId");
                    this._ProcessRequestId = value;
                    this.SendPropertyChanged("ProcessRequestId");
                    this.OnProcessRequestIdChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for ProcessGroupId in the schema.
        /// </summary>
        [Column(Name = @"process_group_id", Storage = "_ProcessGroupId", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> ProcessGroupId
        {
            get
            {
                return this._ProcessGroupId;
            }
            set
            {
                if (this._ProcessGroupId != value)
                {
                    this.OnProcessGroupIdChanging(value);
                    this.SendPropertyChanging("ProcessGroupId");
                    this._ProcessGroupId = value;
                    this.SendPropertyChanged("ProcessGroupId");
                    this.OnProcessGroupIdChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for CacheOfProductIdForSearching in the schema.
        /// </summary>
        [Column(Name = @"cache_of_product_id_for_searching", Storage = "_CacheOfProductIdForSearching", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> CacheOfProductIdForSearching
        {
            get
            {
                return this._CacheOfProductIdForSearching;
            }
            set
            {
                if (this._CacheOfProductIdForSearching != value)
                {
                    this.OnCacheOfProductIdForSearchingChanging(value);
                    this.SendPropertyChanging("CacheOfProductIdForSearching");
                    this._CacheOfProductIdForSearching = value;
                    this.SendPropertyChanged("CacheOfProductIdForSearching");
                    this.OnCacheOfProductIdForSearchingChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for CacheOfDim1ForSearching in the schema.
        /// </summary>
        [Column(Name = @"cache_of_dim1_for_searching", Storage = "_CacheOfDim1ForSearching", DbType = "NUMERIC(9,4)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> CacheOfDim1ForSearching
        {
            get
            {
                return this._CacheOfDim1ForSearching;
            }
            set
            {
                if (this._CacheOfDim1ForSearching != value)
                {
                    this.OnCacheOfDim1ForSearchingChanging(value);
                    this.SendPropertyChanging("CacheOfDim1ForSearching");
                    this._CacheOfDim1ForSearching = value;
                    this.SendPropertyChanged("CacheOfDim1ForSearching");
                    this.OnCacheOfDim1ForSearchingChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for CacheOfDim2ForSearching in the schema.
        /// </summary>
        [Column(Name = @"cache_of_dim2_for_searching", Storage = "_CacheOfDim2ForSearching", DbType = "NUMERIC(9,4)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> CacheOfDim2ForSearching
        {
            get
            {
                return this._CacheOfDim2ForSearching;
            }
            set
            {
                if (this._CacheOfDim2ForSearching != value)
                {
                    this.OnCacheOfDim2ForSearchingChanging(value);
                    this.SendPropertyChanging("CacheOfDim2ForSearching");
                    this._CacheOfDim2ForSearching = value;
                    this.SendPropertyChanged("CacheOfDim2ForSearching");
                    this.OnCacheOfDim2ForSearchingChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for CacheOfDim3ForSearching in the schema.
        /// </summary>
        [Column(Name = @"cache_of_dim3_for_searching", Storage = "_CacheOfDim3ForSearching", DbType = "NUMERIC(9,4)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> CacheOfDim3ForSearching
        {
            get
            {
                return this._CacheOfDim3ForSearching;
            }
            set
            {
                if (this._CacheOfDim3ForSearching != value)
                {
                    this.OnCacheOfDim3ForSearchingChanging(value);
                    this.SendPropertyChanging("CacheOfDim3ForSearching");
                    this._CacheOfDim3ForSearching = value;
                    this.SendPropertyChanged("CacheOfDim3ForSearching");
                    this.OnCacheOfDim3ForSearchingChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for CacheOfDim4ForSearching in the schema.
        /// </summary>
        [Column(Name = @"cache_of_dim4_for_searching", Storage = "_CacheOfDim4ForSearching", DbType = "NUMERIC(9,4)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> CacheOfDim4ForSearching
        {
            get
            {
                return this._CacheOfDim4ForSearching;
            }
            set
            {
                if (this._CacheOfDim4ForSearching != value)
                {
                    this.OnCacheOfDim4ForSearchingChanging(value);
                    this.SendPropertyChanging("CacheOfDim4ForSearching");
                    this._CacheOfDim4ForSearching = value;
                    this.SendPropertyChanged("CacheOfDim4ForSearching");
                    this.OnCacheOfDim4ForSearchingChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for CacheOfDim5ForSearching in the schema.
        /// </summary>
        [Column(Name = @"cache_of_dim5_for_searching", Storage = "_CacheOfDim5ForSearching", DbType = "NUMERIC(9,4)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> CacheOfDim5ForSearching
        {
            get
            {
                return this._CacheOfDim5ForSearching;
            }
            set
            {
                if (this._CacheOfDim5ForSearching != value)
                {
                    this.OnCacheOfDim5ForSearchingChanging(value);
                    this.SendPropertyChanging("CacheOfDim5ForSearching");
                    this._CacheOfDim5ForSearching = value;
                    this.SendPropertyChanged("CacheOfDim5ForSearching");
                    this.OnCacheOfDim5ForSearchingChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for CacheOfBranchIdForSearching in the schema.
        /// </summary>
        [Column(Name = @"cache_of_branch_id_for_searching", Storage = "_CacheOfBranchIdForSearching", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> CacheOfBranchIdForSearching
        {
            get
            {
                return this._CacheOfBranchIdForSearching;
            }
            set
            {
                if (this._CacheOfBranchIdForSearching != value)
                {
                    this.OnCacheOfBranchIdForSearchingChanging(value);
                    this.SendPropertyChanging("CacheOfBranchIdForSearching");
                    this._CacheOfBranchIdForSearching = value;
                    this.SendPropertyChanged("CacheOfBranchIdForSearching");
                    this.OnCacheOfBranchIdForSearchingChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for PrintedDate in the schema.
        /// </summary>
        [Column(Name = @"printed_date", Storage = "_PrintedDate", DbType = "DATE", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<System.DateTime> PrintedDate
        {
            get
            {
                return this._PrintedDate;
            }
            set
            {
                if (this._PrintedDate != value)
                {
                    this.OnPrintedDateChanging(value);
                    this.SendPropertyChanging("PrintedDate");
                    this._PrintedDate = value;
                    this.SendPropertyChanged("PrintedDate");
                    this.OnPrintedDateChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for StockAllocated in the schema.
        /// </summary>
        [Column(Name = @"stock_allocated", Storage = "_StockAllocated", DbType = "BOOL", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<bool> StockAllocated
        {
            get
            {
                return this._StockAllocated;
            }
            set
            {
                if (this._StockAllocated != value)
                {
                    this.OnStockAllocatedChanging(value);
                    this.SendPropertyChanging("StockAllocated");
                    this._StockAllocated = value;
                    this.SendPropertyChanged("StockAllocated");
                    this.OnStockAllocatedChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for ReadyStockAllocated in the schema.
        /// </summary>
        [Column(Name = @"ready_stock_allocated", Storage = "_ReadyStockAllocated", DbType = "BOOL", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<bool> ReadyStockAllocated
        {
            get
            {
                return this._ReadyStockAllocated;
            }
            set
            {
                if (this._ReadyStockAllocated != value)
                {
                    this.OnReadyStockAllocatedChanging(value);
                    this.SendPropertyChanging("ReadyStockAllocated");
                    this._ReadyStockAllocated = value;
                    this.SendPropertyChanged("ReadyStockAllocated");
                    this.OnReadyStockAllocatedChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for SalesItem_SalesItemId in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="SalesItem_ProductLevelAllocation", Storage="_SalesItem_SalesItemId", ThisKey="SalesItemId", OtherKey="Id", IsForeignKey=true)]
        public SalesItem SalesItem_SalesItemId
        {
            get
            {
                return this._SalesItem_SalesItemId.Entity;
            }
            set
            {
                SalesItem previousValue = this._SalesItem_SalesItemId.Entity;
                if ((previousValue != value) || (this._SalesItem_SalesItemId.HasLoadedOrAssignedValue == false))
                {
                    this.SendPropertyChanging("SalesItem_SalesItemId");
                    if (previousValue != null)
                    {
                        this._SalesItem_SalesItemId.Entity = null;
                        previousValue.ProductLevelAllocation_SalesItemId.Remove(this);
                    }
                    this._SalesItem_SalesItemId.Entity = value;
                    if (value != null)
                    {
                        this._SalesItemId = value.Id;
                        value.ProductLevelAllocation_SalesItemId.Add(this);
                    }
                    else
                    {
                        this._SalesItemId = default(System.Nullable<int>);
                    }
                    this.SendPropertyChanged("SalesItem_SalesItemId");
                }
            }
        }

    
        /// <summary>
        /// There are no comments for SalesItem_ProductLevelAllocationId in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="ProductLevelAllocation_SalesItem", Storage="_SalesItem_ProductLevelAllocationId", ThisKey="Id", OtherKey="ProductLevelAllocationId", DeleteRule="NO ACTION")]
        public EntitySet<SalesItem> SalesItem_ProductLevelAllocationId
        {
            get
            {
                return this._SalesItem_ProductLevelAllocationId;
            }
            set
            {
                this._SalesItem_ProductLevelAllocationId.Assign(value);
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

        private void attach_SalesItem_ProductLevelAllocationId(SalesItem entity)
        {
            this.SendPropertyChanging("SalesItem_ProductLevelAllocationId");
            entity.ProductLevelAllocation_ProductLevelAllocationId = this;
        }
    
        private void detach_SalesItem_ProductLevelAllocationId(SalesItem entity)
        {
            this.SendPropertyChanging("SalesItem_ProductLevelAllocationId");
            entity.ProductLevelAllocation_ProductLevelAllocationId = null;
        }
    }

}