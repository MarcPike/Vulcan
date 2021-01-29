using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using Vulcan.IMetal.Queries.ProductCodes;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class QuoteQueryProductOptionsHelper
    {
        private readonly List<string> _coidList = new List<string>();
        private readonly List<ProductMasterAdvancedQuery> _allTeamProducts = new List<ProductMasterAdvancedQuery>();
        private readonly List<ProductMasterAdvancedQuery> _allSystemProducts = new List<ProductMasterAdvancedQuery>();
        private readonly List<ProductMaster> _allProductMasters = new List<ProductMaster>();
        //private readonly QuoteQuery _quoteQuery;
        private readonly TeamRef _teamRef;
        
        public List<string> CoidList => _coidList;
        public List<ProductMaster> Products => _allProductMasters;

        public List<string> ProductCategories =>
            _allTeamProducts.Select(x => x.ProductCategory).Distinct().OrderBy(x => x).ToList();

        public List<string> StockGrades =>
            _allTeamProducts.Select(x => x.StockGrade).Distinct().OrderBy(x => x).ToList();

        public List<string> MetalCategories =>
            _allTeamProducts.Select(x => x.MetalCategory).Distinct().OrderBy(x => x).ToList();

        public List<string> ProductTypes =>
            _allTeamProducts.Select(x => x.ProductType).Distinct().OrderBy(x => x).ToList();

        //public List<ProductMasterAdvancedQuery> ProductsFull => _allTeamProducts;

        public QuoteQueryProductOptionsHelper(TeamRef teamRef)
        {
            var repQuoteQuery = new RepositoryBase<QuoteQuery>();
            _teamRef = teamRef;
            ProcessAllTeamProductMaster();
            _allSystemProducts.Clear();
        }

        private void AllSystemFullProducts(string coid)
        {
            if (_coidList.All(x => x != coid))
            {
                _coidList.Add(coid);
                var query = ProductMasterAdvancedQuery.AsQueryable(coid);
                _allSystemProducts.AddRange(query.ToList()); 
            }
        }

        private void ProcessAllTeamProductMaster()
        {
            var repQuote = new RepositoryBase<CrmQuote>();
            foreach (var crmQuote in repQuote.AsQueryable().Where(x=>x.Team.Id == _teamRef.Id).ToList())
            {
                foreach (var crmQuoteItem in crmQuote.Items.Select(x=>x.AsQuoteItem()))
                {
                    var startingProduct = crmQuoteItem.QuotePrice.StartingProduct;
                    var finishedProduct = crmQuoteItem.QuotePrice.FinishedProduct;

                    if (!startingProduct.IsNewProduct)
                    {
                        AddProduct(startingProduct);
                    }

                    if (!finishedProduct.IsNewProduct)
                    {
                        AddProduct(finishedProduct);
                    }
                }
            }

        }

        public void AddProduct(ProductMaster product)
        {
            AllSystemFullProducts(product.Coid);
            //if (product.IsNewProduct)
            //{
            //    //_allTeamProducts.Add(productMasterFound);
            //    _allProductMasters.Add(product);
            //}

            if (!_allTeamProducts.Any(x => x.Coid == product.Coid && x.ProductId == product.ProductId))
            {
                var productMasterFound = _allSystemProducts.FirstOrDefault(x => x.Coid == product.Coid && x.ProductId == product.ProductId);
                if (productMasterFound != null)
                {
                    _allTeamProducts.Add(productMasterFound);
                    _allProductMasters.Add(product);
                }
            }
        }
    }
}