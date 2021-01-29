using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Quotes;
using System;
using DAL.iMetal.Core.Helpers;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class CreateNewQuoteTestPieceModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Coid { get; set; } = string.Empty;
        public string Name { get; set; }
        public ProductMaster StartingProduct { get; set; } = new ProductMaster();
        public OrderQuantity OrderQuantity { get; set; } = new OrderQuantity(1,6,"in");

        public QuoteTestPiece GetQuoteTestPiece()
        {
            if (Coid != string.Empty)
            {
                StartingProduct.FactorForKilograms = UomHelper.GetFactorForKilograms(Coid);
                StartingProduct.FactorForLbs = UomHelper.GetFactorForPounds(Coid);
            }

            return new QuoteTestPiece(Id, Coid, StartingProduct, OrderQuantity, Name);
        }

        public CreateNewQuoteTestPieceModel()
        {
        }

        public CreateNewQuoteTestPieceModel(string application, string userId)
        {
            Application = application;
            UserId = userId;
        }

    }
}
