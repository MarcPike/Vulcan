using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Quotes;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class QuoteTestPiece
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Coid { get; set; }
        public string Name { get; set; } = string.Empty;
        public OrderQuantity OrderQuantity { get; set; } = new OrderQuantity(0,0,"in");
        public ProductMaster StartingProduct { get; set; }

        public QuoteTestPiece()
        {
        }

        public QuoteTestPiece(Guid id, string coid, ProductMaster product, OrderQuantity orderQuantity, string name)
        {
            Id = id;
            StartingProduct = product;
            OrderQuantity = orderQuantity;
            Coid = coid;
            Name = name;
        }

        public RequiredQuantity RequiredQuantity => OrderQuantity.GetRequiredQuantity(Coid, StartingProduct.TheoWeight);
    }
}
