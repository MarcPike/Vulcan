using System;
using Devart.Data.Linq;
using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.StockItems;
using Vulcan.IMetal.Results;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.StockItems
{
    public class QueryStockItems: QueryBase<StockItemResult>
    {
        

        public string QueryCoid { get; set; }
        public StockItemsContext Context;
        //public CompiledQueryCache Cache =
        //    CompiledQueryCache.RegisterDataContext(typeof(StockItemsContext));

        public int ProductId { get; set; } = int.MinValue;
        public FilterCreateDate CreateDate { get; set; } = new FilterCreateDate();
        public FilterMetalCategory MetalCategory { get; set; } = new FilterMetalCategory();
        public FilterMetalType MetalType { get; set; } = new FilterMetalType();
        public FilterStockType StockType { get; set; } = new FilterStockType();
        public FilterNotes Notes { get; set; } = new FilterNotes();
        public FilterLocation Location { get; set; } = new FilterLocation();
        public FilterWarehouseName WarehouseName { get; set; } = new FilterWarehouseName();
        public FilterWarehouseCode WarehouseCode { get; set; } = new FilterWarehouseCode();
        public FilterProductCode ProductCode { get; set; } = new FilterProductCode();
        public FilterProductSize ProductSize { get; set; } = new FilterProductSize();
        public FilterTagNumber TagNumber { get; set; } = new FilterTagNumber();
        public FilterHeatNumber HeatNumber { get; set; } = new FilterHeatNumber();
        public FilterAvailablePieces AvailablePieces { get; set; } = new FilterAvailablePieces();
        public FilterAvailableWeight AvailableWeight { get; set; } = new FilterAvailableWeight();
        public FilterAvailableLength AvailableLength { get; set; } = new FilterAvailableLength();
        public FilterOutsideDiameter OutsideDiameter { get; set; } = new FilterOutsideDiameter();
        public FilterInsideDiameter InsideDiameter { get; set; } = new FilterInsideDiameter();
        public FilterWidth Width { get; set; } = new FilterWidth();
        public FilterLength Length { get; set; } = new FilterLength();
        public FilterThick Thick { get; set; } = new FilterThick();
        public FilterDensity Density { get; set; } = new FilterDensity();
        public FilterProductGrade ProductGrade { get; set; } = new FilterProductGrade();
        public FilterProductCondition ProductCondition { get; set; } = new FilterProductCondition();
        public FilterProductCategory ProductCategory { get; set; } = new FilterProductCategory();
        public FilterMill Mill { get; set; } = new FilterMill();
        public FilterMaterialTypeDescription MaterialTypeDescription { get; set; } = new FilterMaterialTypeDescription();
        public FilterMaterialType MaterialType { get; set; } = new FilterMaterialType();

        public QueryStockItems(string coid) : base(coid)
        {
            Context = ContextFactory.GetStockItemsContextForCoid(coid);
        }

        public override List<StockItemResult> Execute()
        {

            var stockItems = Context.StockItem.
            LoadWith(x => x.Product).
            LoadWith(x => x.StockCast).
            LoadWith(x => x.StockCast.Mill).
            LoadWith(x => x.Product.ProductCategory).
            LoadWith(x => x.Note).
            LoadWith(x => x.Product.StockGrade_GradeId).
            LoadWith(x => x.Warehouse).
            LoadWith(x => Context.UnitsOfMeasure).
            LoadWith(x => x.Product.ProductCategory.StockAnalysisCode_Analysis1Id).
            LoadWith(x => x.Product.ProductCategory.StockAnalysisCode_Analysis2Id).
            LoadWith(x => x.Product.ProductCategory.ProductControl);

            if (ProductId > int.MinValue)
                    stockItems = stockItems.Where(x => x.ProductId == ProductId);

                stockItems = WarehouseName.ApplyFilter(stockItems);

                stockItems = WarehouseCode.ApplyFilter(stockItems);

                stockItems = ProductCode.ApplyFilter(stockItems);

                // Hardcode FilterAvailablePieces for performance
                if (!AvailablePieces.MinUsed)
                    AvailablePieces.Min(1);

                stockItems = CreateDate.ApplyFilter(stockItems);

                stockItems = MetalCategory.ApplyFilter(stockItems);

                stockItems = MetalType.ApplyFilter(stockItems);

                stockItems = StockType.ApplyFilter(stockItems);

                stockItems = Notes.ApplyFilter(stockItems);

                stockItems = Location.ApplyFilter(stockItems);


                stockItems = ProductSize.ApplyFilter(stockItems);

                stockItems = TagNumber.ApplyFilter(stockItems);

                stockItems = HeatNumber.ApplyFilter(stockItems);

                stockItems = ProductGrade.ApplyFilter(stockItems);

                stockItems = ProductCondition.ApplyFilter(stockItems);

                stockItems = ProductCategory.ApplyFilter(stockItems);

                stockItems = Mill.ApplyFilter(stockItems);

                stockItems = MaterialTypeDescription.ApplyFilter(stockItems);

                stockItems = MaterialType.ApplyFilter(stockItems);

                stockItems = AvailablePieces.ApplyFilter(stockItems);

                stockItems = AvailableWeight.ApplyFilter(stockItems);

                stockItems = AvailableLength.ApplyFilter(stockItems);

                var postResult = GetResultsFromDatabase(stockItems);

                postResult = PostFilterDimensions(postResult.AsQueryable());

                return postResult.Select(x => new StockItemResult(x)).ToList();

        }

        private List<Context.StockItems.StockItem> PostFilterDimensions(IQueryable<Context.StockItems.StockItem> postResult)
        {
            postResult = postResult.AsQueryable();
            if (DimensionsUsed())
            {
                postResult = Length.ApplyFilter(postResult);
                postResult = Width.ApplyFilter(postResult);
                postResult = InsideDiameter.ApplyFilter(postResult);
                postResult = OutsideDiameter.ApplyFilter(postResult);
                postResult = Density.ApplyFilter(postResult);
                postResult = Thick.ApplyFilter(postResult);
            }
            return postResult.ToList();
        }


        private List<Context.StockItems.StockItem> GetResultsFromDatabase(IQueryable<Context.StockItems.StockItem> stockItems)
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



        private bool DimensionsUsed()
        {
            return Length.IsActive || Width.IsActive || InsideDiameter.IsActive || OutsideDiameter.IsActive || Density.IsActive || Thick.IsActive;
        }

        public QueryStockItems() : base("INC")
        {

        }

    }

}
