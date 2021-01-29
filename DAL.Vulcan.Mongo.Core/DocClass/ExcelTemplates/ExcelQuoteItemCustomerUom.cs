using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemCustomerUom")]
    public class ExcelQuoteItemCustomerUom : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemCustomerUom()
        {
            FieldName = "CustomerUom";
            Label = "Customer Unit of Measure";
            Id = "D3BF24F1-7AE7-44AB-9378-2C2751375123";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            return item.CustomerUom.ToString();
        }
    }
}