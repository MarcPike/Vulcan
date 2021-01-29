using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemLineType")]
    public class ExcelQuoteItemLineType : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemLineType()
        {
            FieldName = "LineType";
            Label = "Line Item Type";
            Id = "5AD32411-6F2B-41FF-AF83-5BA8070351CC";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            if (item.IsQuickQuoteItem) return "QuickQuote";
            if (item.IsMachinedPart) return "MachinedPart";
            if (item.IsCrozCalc) return "MachineCalc";
            return "Normal";
        }
    }
}