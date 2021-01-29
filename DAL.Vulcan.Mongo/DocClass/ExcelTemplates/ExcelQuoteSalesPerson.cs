using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteSalesPerson")]
    public class ExcelQuoteSalesPerson: ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteSalesPerson()
        {
            FieldName = "SalesPerson";
            Label = "Sales Person";
            Id = "9806F225-5044-4E00-9143-D917BCF4BB8C";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.SalesPerson.FullName;
        }
    }
}