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
    /// There are no comments for Vulcan.IMetal.Context.PurchaseOrders.PurchaseOrderTotal in the schema.
    /// </summary>
    [Table(Name = @"public.purchase_order_totals")]
    public partial class PurchaseOrderTotal : INotifyPropertyChanging, INotifyPropertyChanged
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

        private System.Nullable<int> _OrderedPiece;

        private System.Nullable<decimal> _OrderedQuantity;

        private System.Nullable<decimal> _OrderedWeight;

        private System.Nullable<int> _DeliveredPiece;

        private System.Nullable<decimal> _DeliveredQuantity;

        private System.Nullable<decimal> _DeliveredWeight;

        private System.Nullable<int> _BalancePiece;

        private System.Nullable<decimal> _BalanceQuantity;

        private System.Nullable<decimal> _BalanceWeight;

        private System.Nullable<decimal> _MaterialValue;

        private System.Nullable<decimal> _TransportValue;

        private System.Nullable<decimal> _ProductionValue;

        private System.Nullable<decimal> _MiscellaneousValue;

        private System.Nullable<decimal> _SurchargeValue;

        private System.Nullable<int> _TransportPiece;

        private System.Nullable<decimal> _TransportQuantity;

        private System.Nullable<decimal> _TransportWeight;

        private System.Nullable<int> _TransientPiece;

        private System.Nullable<decimal> _TransientQuantity;

        private System.Nullable<decimal> _TransientWeight;

        private System.Nullable<decimal> _BaseMaterialValue;

        private System.Nullable<decimal> _BaseTransportValue;

        private System.Nullable<decimal> _BaseProductionValue;

        private System.Nullable<decimal> _BaseMiscellaneousValue;

        private System.Nullable<decimal> _BaseSurchargeValue;
        #pragma warning restore 0649

        private EntitySet<PurchaseOrderItem> _PurchaseOrderItem;
    
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
        partial void OnOrderedPieceChanging(System.Nullable<int> value);
        partial void OnOrderedPieceChanged();
        partial void OnOrderedQuantityChanging(System.Nullable<decimal> value);
        partial void OnOrderedQuantityChanged();
        partial void OnOrderedWeightChanging(System.Nullable<decimal> value);
        partial void OnOrderedWeightChanged();
        partial void OnDeliveredPieceChanging(System.Nullable<int> value);
        partial void OnDeliveredPieceChanged();
        partial void OnDeliveredQuantityChanging(System.Nullable<decimal> value);
        partial void OnDeliveredQuantityChanged();
        partial void OnDeliveredWeightChanging(System.Nullable<decimal> value);
        partial void OnDeliveredWeightChanged();
        partial void OnBalancePieceChanging(System.Nullable<int> value);
        partial void OnBalancePieceChanged();
        partial void OnBalanceQuantityChanging(System.Nullable<decimal> value);
        partial void OnBalanceQuantityChanged();
        partial void OnBalanceWeightChanging(System.Nullable<decimal> value);
        partial void OnBalanceWeightChanged();
        partial void OnMaterialValueChanging(System.Nullable<decimal> value);
        partial void OnMaterialValueChanged();
        partial void OnTransportValueChanging(System.Nullable<decimal> value);
        partial void OnTransportValueChanged();
        partial void OnProductionValueChanging(System.Nullable<decimal> value);
        partial void OnProductionValueChanged();
        partial void OnMiscellaneousValueChanging(System.Nullable<decimal> value);
        partial void OnMiscellaneousValueChanged();
        partial void OnSurchargeValueChanging(System.Nullable<decimal> value);
        partial void OnSurchargeValueChanged();
        partial void OnTransportPieceChanging(System.Nullable<int> value);
        partial void OnTransportPieceChanged();
        partial void OnTransportQuantityChanging(System.Nullable<decimal> value);
        partial void OnTransportQuantityChanged();
        partial void OnTransportWeightChanging(System.Nullable<decimal> value);
        partial void OnTransportWeightChanged();
        partial void OnTransientPieceChanging(System.Nullable<int> value);
        partial void OnTransientPieceChanged();
        partial void OnTransientQuantityChanging(System.Nullable<decimal> value);
        partial void OnTransientQuantityChanged();
        partial void OnTransientWeightChanging(System.Nullable<decimal> value);
        partial void OnTransientWeightChanged();
        partial void OnBaseMaterialValueChanging(System.Nullable<decimal> value);
        partial void OnBaseMaterialValueChanged();
        partial void OnBaseTransportValueChanging(System.Nullable<decimal> value);
        partial void OnBaseTransportValueChanged();
        partial void OnBaseProductionValueChanging(System.Nullable<decimal> value);
        partial void OnBaseProductionValueChanged();
        partial void OnBaseMiscellaneousValueChanging(System.Nullable<decimal> value);
        partial void OnBaseMiscellaneousValueChanged();
        partial void OnBaseSurchargeValueChanging(System.Nullable<decimal> value);
        partial void OnBaseSurchargeValueChanged();
        #endregion

        public PurchaseOrderTotal()
        {
            this._PurchaseOrderItem = new EntitySet<PurchaseOrderItem>(new Action<PurchaseOrderItem>(this.attach_PurchaseOrderItem), new Action<PurchaseOrderItem>(this.detach_PurchaseOrderItem));
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
        /// There are no comments for OrderedPiece in the schema.
        /// </summary>
        [Column(Name = @"ordered_pieces", Storage = "_OrderedPiece", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> OrderedPiece
        {
            get
            {
                return this._OrderedPiece;
            }
            set
            {
                if (this._OrderedPiece != value)
                {
                    this.OnOrderedPieceChanging(value);
                    this.SendPropertyChanging("OrderedPiece");
                    this._OrderedPiece = value;
                    this.SendPropertyChanged("OrderedPiece");
                    this.OnOrderedPieceChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for OrderedQuantity in the schema.
        /// </summary>
        [Column(Name = @"ordered_quantity", Storage = "_OrderedQuantity", DbType = "NUMERIC(12,3)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> OrderedQuantity
        {
            get
            {
                return this._OrderedQuantity;
            }
            set
            {
                if (this._OrderedQuantity != value)
                {
                    this.OnOrderedQuantityChanging(value);
                    this.SendPropertyChanging("OrderedQuantity");
                    this._OrderedQuantity = value;
                    this.SendPropertyChanged("OrderedQuantity");
                    this.OnOrderedQuantityChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for OrderedWeight in the schema.
        /// </summary>
        [Column(Name = @"ordered_weight", Storage = "_OrderedWeight", DbType = "NUMERIC(10,3)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> OrderedWeight
        {
            get
            {
                return this._OrderedWeight;
            }
            set
            {
                if (this._OrderedWeight != value)
                {
                    this.OnOrderedWeightChanging(value);
                    this.SendPropertyChanging("OrderedWeight");
                    this._OrderedWeight = value;
                    this.SendPropertyChanged("OrderedWeight");
                    this.OnOrderedWeightChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for DeliveredPiece in the schema.
        /// </summary>
        [Column(Name = @"delivered_pieces", Storage = "_DeliveredPiece", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> DeliveredPiece
        {
            get
            {
                return this._DeliveredPiece;
            }
            set
            {
                if (this._DeliveredPiece != value)
                {
                    this.OnDeliveredPieceChanging(value);
                    this.SendPropertyChanging("DeliveredPiece");
                    this._DeliveredPiece = value;
                    this.SendPropertyChanged("DeliveredPiece");
                    this.OnDeliveredPieceChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for DeliveredQuantity in the schema.
        /// </summary>
        [Column(Name = @"delivered_quantity", Storage = "_DeliveredQuantity", DbType = "NUMERIC(12,3)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> DeliveredQuantity
        {
            get
            {
                return this._DeliveredQuantity;
            }
            set
            {
                if (this._DeliveredQuantity != value)
                {
                    this.OnDeliveredQuantityChanging(value);
                    this.SendPropertyChanging("DeliveredQuantity");
                    this._DeliveredQuantity = value;
                    this.SendPropertyChanged("DeliveredQuantity");
                    this.OnDeliveredQuantityChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for DeliveredWeight in the schema.
        /// </summary>
        [Column(Name = @"delivered_weight", Storage = "_DeliveredWeight", DbType = "NUMERIC(10,3)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> DeliveredWeight
        {
            get
            {
                return this._DeliveredWeight;
            }
            set
            {
                if (this._DeliveredWeight != value)
                {
                    this.OnDeliveredWeightChanging(value);
                    this.SendPropertyChanging("DeliveredWeight");
                    this._DeliveredWeight = value;
                    this.SendPropertyChanged("DeliveredWeight");
                    this.OnDeliveredWeightChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for BalancePiece in the schema.
        /// </summary>
        [Column(Name = @"balance_pieces", Storage = "_BalancePiece", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> BalancePiece
        {
            get
            {
                return this._BalancePiece;
            }
            set
            {
                if (this._BalancePiece != value)
                {
                    this.OnBalancePieceChanging(value);
                    this.SendPropertyChanging("BalancePiece");
                    this._BalancePiece = value;
                    this.SendPropertyChanged("BalancePiece");
                    this.OnBalancePieceChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for BalanceQuantity in the schema.
        /// </summary>
        [Column(Name = @"balance_quantity", Storage = "_BalanceQuantity", DbType = "NUMERIC(12,3)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> BalanceQuantity
        {
            get
            {
                return this._BalanceQuantity;
            }
            set
            {
                if (this._BalanceQuantity != value)
                {
                    this.OnBalanceQuantityChanging(value);
                    this.SendPropertyChanging("BalanceQuantity");
                    this._BalanceQuantity = value;
                    this.SendPropertyChanged("BalanceQuantity");
                    this.OnBalanceQuantityChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for BalanceWeight in the schema.
        /// </summary>
        [Column(Name = @"balance_weight", Storage = "_BalanceWeight", DbType = "NUMERIC(10,3)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> BalanceWeight
        {
            get
            {
                return this._BalanceWeight;
            }
            set
            {
                if (this._BalanceWeight != value)
                {
                    this.OnBalanceWeightChanging(value);
                    this.SendPropertyChanging("BalanceWeight");
                    this._BalanceWeight = value;
                    this.SendPropertyChanged("BalanceWeight");
                    this.OnBalanceWeightChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for MaterialValue in the schema.
        /// </summary>
        [Column(Name = @"material_value", Storage = "_MaterialValue", DbType = "NUMERIC(12,2)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> MaterialValue
        {
            get
            {
                return this._MaterialValue;
            }
            set
            {
                if (this._MaterialValue != value)
                {
                    this.OnMaterialValueChanging(value);
                    this.SendPropertyChanging("MaterialValue");
                    this._MaterialValue = value;
                    this.SendPropertyChanged("MaterialValue");
                    this.OnMaterialValueChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for TransportValue in the schema.
        /// </summary>
        [Column(Name = @"transport_value", Storage = "_TransportValue", DbType = "NUMERIC(12,2)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> TransportValue
        {
            get
            {
                return this._TransportValue;
            }
            set
            {
                if (this._TransportValue != value)
                {
                    this.OnTransportValueChanging(value);
                    this.SendPropertyChanging("TransportValue");
                    this._TransportValue = value;
                    this.SendPropertyChanged("TransportValue");
                    this.OnTransportValueChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for ProductionValue in the schema.
        /// </summary>
        [Column(Name = @"production_value", Storage = "_ProductionValue", DbType = "NUMERIC(12,2)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> ProductionValue
        {
            get
            {
                return this._ProductionValue;
            }
            set
            {
                if (this._ProductionValue != value)
                {
                    this.OnProductionValueChanging(value);
                    this.SendPropertyChanging("ProductionValue");
                    this._ProductionValue = value;
                    this.SendPropertyChanged("ProductionValue");
                    this.OnProductionValueChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for MiscellaneousValue in the schema.
        /// </summary>
        [Column(Name = @"miscellaneous_value", Storage = "_MiscellaneousValue", DbType = "NUMERIC(12,2)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> MiscellaneousValue
        {
            get
            {
                return this._MiscellaneousValue;
            }
            set
            {
                if (this._MiscellaneousValue != value)
                {
                    this.OnMiscellaneousValueChanging(value);
                    this.SendPropertyChanging("MiscellaneousValue");
                    this._MiscellaneousValue = value;
                    this.SendPropertyChanged("MiscellaneousValue");
                    this.OnMiscellaneousValueChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for SurchargeValue in the schema.
        /// </summary>
        [Column(Name = @"surcharge_value", Storage = "_SurchargeValue", DbType = "NUMERIC(12,2)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> SurchargeValue
        {
            get
            {
                return this._SurchargeValue;
            }
            set
            {
                if (this._SurchargeValue != value)
                {
                    this.OnSurchargeValueChanging(value);
                    this.SendPropertyChanging("SurchargeValue");
                    this._SurchargeValue = value;
                    this.SendPropertyChanged("SurchargeValue");
                    this.OnSurchargeValueChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for TransportPiece in the schema.
        /// </summary>
        [Column(Name = @"transport_pieces", Storage = "_TransportPiece", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> TransportPiece
        {
            get
            {
                return this._TransportPiece;
            }
            set
            {
                if (this._TransportPiece != value)
                {
                    this.OnTransportPieceChanging(value);
                    this.SendPropertyChanging("TransportPiece");
                    this._TransportPiece = value;
                    this.SendPropertyChanged("TransportPiece");
                    this.OnTransportPieceChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for TransportQuantity in the schema.
        /// </summary>
        [Column(Name = @"transport_quantity", Storage = "_TransportQuantity", DbType = "NUMERIC(12,3)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> TransportQuantity
        {
            get
            {
                return this._TransportQuantity;
            }
            set
            {
                if (this._TransportQuantity != value)
                {
                    this.OnTransportQuantityChanging(value);
                    this.SendPropertyChanging("TransportQuantity");
                    this._TransportQuantity = value;
                    this.SendPropertyChanged("TransportQuantity");
                    this.OnTransportQuantityChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for TransportWeight in the schema.
        /// </summary>
        [Column(Name = @"transport_weight", Storage = "_TransportWeight", DbType = "NUMERIC(10,3)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> TransportWeight
        {
            get
            {
                return this._TransportWeight;
            }
            set
            {
                if (this._TransportWeight != value)
                {
                    this.OnTransportWeightChanging(value);
                    this.SendPropertyChanging("TransportWeight");
                    this._TransportWeight = value;
                    this.SendPropertyChanged("TransportWeight");
                    this.OnTransportWeightChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for TransientPiece in the schema.
        /// </summary>
        [Column(Name = @"transient_pieces", Storage = "_TransientPiece", DbType = "INT4", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<int> TransientPiece
        {
            get
            {
                return this._TransientPiece;
            }
            set
            {
                if (this._TransientPiece != value)
                {
                    this.OnTransientPieceChanging(value);
                    this.SendPropertyChanging("TransientPiece");
                    this._TransientPiece = value;
                    this.SendPropertyChanged("TransientPiece");
                    this.OnTransientPieceChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for TransientQuantity in the schema.
        /// </summary>
        [Column(Name = @"transient_quantity", Storage = "_TransientQuantity", DbType = "NUMERIC(12,3)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> TransientQuantity
        {
            get
            {
                return this._TransientQuantity;
            }
            set
            {
                if (this._TransientQuantity != value)
                {
                    this.OnTransientQuantityChanging(value);
                    this.SendPropertyChanging("TransientQuantity");
                    this._TransientQuantity = value;
                    this.SendPropertyChanged("TransientQuantity");
                    this.OnTransientQuantityChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for TransientWeight in the schema.
        /// </summary>
        [Column(Name = @"transient_weight", Storage = "_TransientWeight", DbType = "NUMERIC(10,3)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> TransientWeight
        {
            get
            {
                return this._TransientWeight;
            }
            set
            {
                if (this._TransientWeight != value)
                {
                    this.OnTransientWeightChanging(value);
                    this.SendPropertyChanging("TransientWeight");
                    this._TransientWeight = value;
                    this.SendPropertyChanged("TransientWeight");
                    this.OnTransientWeightChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for BaseMaterialValue in the schema.
        /// </summary>
        [Column(Name = @"base_material_value", Storage = "_BaseMaterialValue", DbType = "NUMERIC(12,2)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> BaseMaterialValue
        {
            get
            {
                return this._BaseMaterialValue;
            }
            set
            {
                if (this._BaseMaterialValue != value)
                {
                    this.OnBaseMaterialValueChanging(value);
                    this.SendPropertyChanging("BaseMaterialValue");
                    this._BaseMaterialValue = value;
                    this.SendPropertyChanged("BaseMaterialValue");
                    this.OnBaseMaterialValueChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for BaseTransportValue in the schema.
        /// </summary>
        [Column(Name = @"base_transport_value", Storage = "_BaseTransportValue", DbType = "NUMERIC(12,2)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> BaseTransportValue
        {
            get
            {
                return this._BaseTransportValue;
            }
            set
            {
                if (this._BaseTransportValue != value)
                {
                    this.OnBaseTransportValueChanging(value);
                    this.SendPropertyChanging("BaseTransportValue");
                    this._BaseTransportValue = value;
                    this.SendPropertyChanged("BaseTransportValue");
                    this.OnBaseTransportValueChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for BaseProductionValue in the schema.
        /// </summary>
        [Column(Name = @"base_production_value", Storage = "_BaseProductionValue", DbType = "NUMERIC(12,2)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> BaseProductionValue
        {
            get
            {
                return this._BaseProductionValue;
            }
            set
            {
                if (this._BaseProductionValue != value)
                {
                    this.OnBaseProductionValueChanging(value);
                    this.SendPropertyChanging("BaseProductionValue");
                    this._BaseProductionValue = value;
                    this.SendPropertyChanged("BaseProductionValue");
                    this.OnBaseProductionValueChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for BaseMiscellaneousValue in the schema.
        /// </summary>
        [Column(Name = @"base_miscellaneous_value", Storage = "_BaseMiscellaneousValue", DbType = "NUMERIC(12,2)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> BaseMiscellaneousValue
        {
            get
            {
                return this._BaseMiscellaneousValue;
            }
            set
            {
                if (this._BaseMiscellaneousValue != value)
                {
                    this.OnBaseMiscellaneousValueChanging(value);
                    this.SendPropertyChanging("BaseMiscellaneousValue");
                    this._BaseMiscellaneousValue = value;
                    this.SendPropertyChanged("BaseMiscellaneousValue");
                    this.OnBaseMiscellaneousValueChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for BaseSurchargeValue in the schema.
        /// </summary>
        [Column(Name = @"base_surcharge_value", Storage = "_BaseSurchargeValue", DbType = "NUMERIC(12,2)", UpdateCheck = UpdateCheck.Never)]
        public System.Nullable<decimal> BaseSurchargeValue
        {
            get
            {
                return this._BaseSurchargeValue;
            }
            set
            {
                if (this._BaseSurchargeValue != value)
                {
                    this.OnBaseSurchargeValueChanging(value);
                    this.SendPropertyChanging("BaseSurchargeValue");
                    this._BaseSurchargeValue = value;
                    this.SendPropertyChanged("BaseSurchargeValue");
                    this.OnBaseSurchargeValueChanged();
                }
            }
        }

    
        /// <summary>
        /// There are no comments for PurchaseOrderItem in the schema.
        /// </summary>
        [Devart.Data.Linq.Mapping.Association(Name="PurchaseOrderTotal_PurchaseOrderItem", Storage="_PurchaseOrderItem", ThisKey="Id", OtherKey="PurchaseOrderTotalsId", DeleteRule="NO ACTION")]
        public EntitySet<PurchaseOrderItem> PurchaseOrderItem
        {
            get
            {
                return this._PurchaseOrderItem;
            }
            set
            {
                this._PurchaseOrderItem.Assign(value);
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

        private void attach_PurchaseOrderItem(PurchaseOrderItem entity)
        {
            this.SendPropertyChanging("PurchaseOrderItem");
            entity.PurchaseOrderTotal = this;
        }
    
        private void detach_PurchaseOrderItem(PurchaseOrderItem entity)
        {
            this.SendPropertyChanging("PurchaseOrderItem");
            entity.PurchaseOrderTotal = null;
        }
    }

}
