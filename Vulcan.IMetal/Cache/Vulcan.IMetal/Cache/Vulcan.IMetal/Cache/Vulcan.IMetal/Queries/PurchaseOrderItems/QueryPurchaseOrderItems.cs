using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.SearchResults;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.ViewFilterObjects;
using Vulcan.IMetal.Context.PurchaseOrders;
using Devart.Data.Linq;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    /// <summary>
    /// TODO: Finalize Incoming Inventory Search
    /// </summary>
    public class QueryPurchaseOrderItems: QueryBase<PurchaseOrderItemResult>
    {
        public string QueryCoid { get; set; }
        public PurchaseOrdersContext Context;

        public FilterCreateDate FilterCreateDate { get; set; } = new FilterCreateDate();

        public FilterCategoryCode FilterCategoryCode { get; set; } = new FilterCategoryCode();
        public FilterCategoryCodeDescription FilterCategoryCodeDescription { get; set; } = new FilterCategoryCodeDescription();

        public FilterProductGrade FilterProductGrade { get; set; } = new FilterProductGrade();
        public FilterProductCondition FilterProductCondition { get; set; } = new FilterProductCondition();
        public FilterProductCategory FilterProductCategory { get; set; } = new FilterProductCategory();

        public FilterProductSize FilterProductSize { get; set; } = new FilterProductSize();

        public FilterProductCode FilterProductCode { get; set; } = new FilterProductCode();
        public FilterProductDescription FilterProductDescription { get; set; } = new FilterProductDescription();

        public FilterMetalCategory FilterMetalCategory { get; set; } = new FilterMetalCategory();
        public FilterMetalType FilterMetalType { get; set; } = new FilterMetalType();
        public FilterStockType FilterStockType { get; set; } = new FilterStockType();

        public FilterAvailablePieces FilterAvailablePieces { get; set; } = new FilterAvailablePieces();
        public FilterAvailableLength FilterAvailableLength { get; set; } = new FilterAvailableLength();
        public FilterAvailableWeight FilterAvailableWeight { get; set; } = new FilterAvailableWeight();

        public FilterOutsideDiameter FilterOutsideDiameter { get; set; } = new FilterOutsideDiameter();
        public FilterInsideDiameter FilterInsideDiameter { get; set; } = new FilterInsideDiameter();
        public FilterWidth FilterWidth { get; set; } = new FilterWidth();
        public FilterLength FilterLength { get; set; } = new FilterLength();
        public FilterThick FilterThick { get; set; } = new FilterThick();
        public FilterDensity FilterDensity { get; set; } = new FilterDensity();

        public FilterMaterialTypeDescription FilterMaterialTypeDescription { get; set; } = new FilterMaterialTypeDescription();
        public FilterMaterialType FilterMaterialType { get; set; } = new FilterMaterialType();

        public FilterDueDate FilterDueDate { get; set; } = new FilterDueDate();

        public override List<PurchaseOrderItemResult> Execute()
        {

            var purchaseOrders = Context.PurchaseOrderItem
                .LoadWith(x => x.Product)
                .LoadWith(x => x.PurchaseStatusCode)
                .LoadWith(x => x.PurchaseOrderHeader_PurchaseHeaderId)
                .LoadWith(x => x.InboundAllocation)
                .LoadWith(x => x.Company_BoughtForCustomerId)
                .LoadWith(x => x.Product.ProductCategory)
                .LoadWith(x => x.Product.StockCast)
                .LoadWith(x => x.Product.StockGrade_GradeId)
                .LoadWith(x => x.Product.ProductCategory.StockAnalysisCode_Analysis1Id)
                .LoadWith(x => x.Product.ProductCategory.StockAnalysisCode_Analysis2Id)
                .LoadWith(x => x.Product.ProductCategory.ProductControl)
                .LoadWith(x => x.PartNumberSpecification).AsQueryable();

            purchaseOrders = FilterCreateDate.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterDueDate.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterProductSize.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterProductCategory.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterProductCondition.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterProductGrade.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterProductDescription.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterProductCode.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterCategoryCode.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterCategoryCodeDescription.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterAvailablePieces.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterAvailableLength.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterAvailableWeight.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterMetalCategory.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterMetalType.ApplyFilter(purchaseOrders);

            purchaseOrders = FilterStockType.ApplyFilter(purchaseOrders);

            var postResult = GetResultsFromDatabase(purchaseOrders);

            postResult = PostFilterDimensions(postResult.AsQueryable());

            return postResult.Select(x => new PurchaseOrderItemResult(Context,Coid,x)).ToList();
        }

        private bool DimensionsUsed()
        {
            return FilterLength.IsActive || FilterWidth.IsActive || FilterInsideDiameter.IsActive || FilterOutsideDiameter.IsActive || FilterDensity.IsActive || FilterThick.IsActive;
        }

        private List<PurchaseOrderItem> PostFilterDimensions(IQueryable<PurchaseOrderItem> postResult)
        {
            postResult = postResult.AsQueryable();
            if (DimensionsUsed())
            {
                postResult = FilterLength.ApplyFilter(postResult);
                postResult = FilterWidth.ApplyFilter(postResult);
                postResult = FilterInsideDiameter.ApplyFilter(postResult);
                postResult = FilterOutsideDiameter.ApplyFilter(postResult);
                postResult = FilterDensity.ApplyFilter(postResult);
                postResult = FilterThick.ApplyFilter(postResult);
            }
            return postResult.ToList();
        }

        private List<PurchaseOrderItem> GetResultsFromDatabase(IQueryable<PurchaseOrderItem> stockItems)
        {
            // Dimensions have to be calculated after database call
            // because the iMetal developers made it so damn hard!!!
            var postResult = stockItems.ToList();
            foreach (var item in postResult)
            {
                item.Coid = Coid;
                item.InitializeDimensions(Context);
            }
            return postResult;
        }


        public QueryPurchaseOrderItems(string coid) : base(coid)
        {
            Context = ContextFactory.GetPurchaseOrdersContextForCoid(coid);
        }
    }
}
