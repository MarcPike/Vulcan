using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteCustomerNotes")]
    public class ExcelQuoteCustomerNotes : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteCustomerNotes()
        {
            FieldName = "CustomerNotes";
            Label = "Customer Notes";
            Id = "4C59FE15-5434-4E82-A713-C8E44A99E93B";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.CustomerNotes;
        }
    }
}