using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;
using Vulcan.IMetal.Helpers;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemCurrency")]
    public class ExcelQuoteItemCurrency : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemCurrency()
        {
            FieldName = "DisplayCurrency";
            Label = "Currency";
            Id = "70B1D04A-B7EA-4DA2-A55C-EDFB73791280";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            return model.DisplayCurrency;
        }
    }

}
