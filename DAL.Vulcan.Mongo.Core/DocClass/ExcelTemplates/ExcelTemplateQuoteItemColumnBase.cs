using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelTemplateQuoteItemColumnBase")]
    public class ExcelTemplateQuoteItemColumnBase
    {
        public string Id { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;

        public bool MultipleColumns { get; set; } = false;

        public virtual string GetValueFor(QuoteModel quoteModel, int index)
        {
            if (MultipleColumns) throw new Exception("This Column only supports multiple columns");
            return string.Empty;
        }

        public virtual List<string> GetValuesFor(QuoteModel quoteModel, int index)
        {
            if (!MultipleColumns) throw new Exception("This Column only supports a single column");
            return new List<string>();
        }

        public virtual List<string> GetLabelsFor(QuoteModel model)
        {
            if (!MultipleColumns) throw new Exception("This Column only supports a single column");
            return new List<string>();
        }

        protected static QuoteItemModel GetQuoteItemModel(QuoteModel model, int index)
        {
            if ((index < 0) || (index > model.Items.Count - 1)) throw new Exception("Invalid Index for GetQuoteItemModel()");
            return model.Items[index];
        }

    }
}