using System.Text;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteShipToAddress")]
    public class ExcelQuoteShipToAddress: ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteShipToAddress()
        {
            FieldName = "ShipToAddress";
            Label = "Ship To Address";
            Id = "45ED4DDE-F359-48F5-A560-14ED63DCBC68";
        }

        public override string GetValueFor(QuoteModel model)
        {
            if (model.ShipToAddress == null) return string.Empty;

            var shipToAddress = new StringBuilder();

            if (model.ShipToAddress.AddressLine1 != null)
            {
                shipToAddress.AppendLine(model.ShipToAddress.AddressLine1);
            }
            if (model.ShipToAddress.AddressLine2 != null)
            {
                shipToAddress.AppendLine(model.ShipToAddress.AddressLine2);
            }

            if ((model.ShipToAddress.City != null) && (model.ShipToAddress.StateProvince != null) &&
                (model.ShipToAddress.PostalCode != null))
            {
                shipToAddress.AppendLine(
                    $"{model.ShipToAddress.City}, {model.ShipToAddress.StateProvince} {model.ShipToAddress.PostalCode}");
            }

            return shipToAddress.ToString();
        }
    }
}