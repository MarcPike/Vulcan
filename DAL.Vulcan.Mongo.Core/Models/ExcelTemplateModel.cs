using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class ExcelTemplateModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public TeamRef Team { get; set; }
        public CrmUserRef CreatedBySalesPerson { get; set; }
        public List<ExcelTemplateColumnModel> QuoteValues { get; set; } = new List<ExcelTemplateColumnModel>();
        public List<ExcelTemplateColumnModel> QuoteItemValues { get; set; } = new List<ExcelTemplateColumnModel>();

        public List<ExcelTemplateQuoteColumnBase> UnusedQuoteValues { get; set; } = new List<ExcelTemplateQuoteColumnBase>();
        public List<ExcelTemplateQuoteItemColumnBase> UnusedQuoteItemValues { get; set; } = new List<ExcelTemplateQuoteItemColumnBase>();


        public ExcelTemplateModel()
        {
            
        }

        public ExcelTemplateModel(ExcelTemplate template)
        {
            Id = template.Id.ToString();
            Name = template.Name;
            Team = template.Team;
            CreatedBySalesPerson = template.CreatedBySalesPerson;
            foreach (var quoteValue in template.QuoteValues)
            {
                QuoteValues.Add(new ExcelTemplateColumnModel()
                {
                    Id = quoteValue.Id,
                    FieldName = quoteValue.FieldName,
                    Label = quoteValue.Label
                });
            }
            foreach (var column in template.QuoteDefaultColumns().Where(column => QuoteValues.All(x => x.Id != column.Id)))
            {
                UnusedQuoteValues.Add(column);
            }

            foreach (var itemValue in template.QuoteItemValues)
            {
                QuoteItemValues.Add(new ExcelTemplateColumnModel()
                {
                    Id = itemValue.Id,
                    FieldName = itemValue.FieldName,
                    Label = itemValue.Label
                });
            }
            foreach (var column in template.QuoteItemDefaultColumns().Where(column => QuoteItemValues.All(x => x.Id != column.Id)))
            {
                UnusedQuoteItemValues.Add(column);
            }


        }
    }

    public class ExcelTemplateColumnModel
    {
        public string Id { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;


    }
}
