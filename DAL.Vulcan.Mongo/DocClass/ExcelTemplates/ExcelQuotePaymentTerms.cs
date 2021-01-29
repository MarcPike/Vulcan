using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuotePaymentTerms")]
    public class ExcelQuotePaymentTerms : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuotePaymentTerms()
        {
            FieldName = "PaymentTerm";
            Label = "Payment Terms";
            Id = "11BD82D6-4A34-4683-88C4-2E5781804911";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.PaymentTerm;
        }
    }
}