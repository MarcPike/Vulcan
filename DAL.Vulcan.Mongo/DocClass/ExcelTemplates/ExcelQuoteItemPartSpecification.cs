using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemPartSpecification")]
    public class ExcelQuoteItemPartSpecification : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemPartSpecification()
        {
            FieldName = "PartSpecification";
            Label = "Part Specs";
            Id = "85364220-2CA9-4E02-8429-C377E00FF624";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            return item.PartSpecification;
        }
    }
}