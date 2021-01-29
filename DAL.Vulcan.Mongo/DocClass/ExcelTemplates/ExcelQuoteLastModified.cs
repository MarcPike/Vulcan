using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteLastModified")]
    public class ExcelQuoteLastModified : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteLastModified()
        {
            FieldName = "LastModified";
            Label = "Last Modified";
            Id = "315EA76C-8E23-47B0-8415-73098FD0E725";
        }

        public override string GetValueFor(QuoteModel model)
        {
            var modified = model.ModifiedDateTime;
            return modified.ToString("g");
        }
    }
}