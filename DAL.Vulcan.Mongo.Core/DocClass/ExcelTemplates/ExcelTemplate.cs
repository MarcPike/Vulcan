using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Queries;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonIgnoreExtraElements]
    public class ExcelTemplate : BaseDocument
    {
        public static MongoRawQueryHelper<ExcelTemplate> Helper = new MongoRawQueryHelper<ExcelTemplate>();

        public string Name { get; set; }
        public TeamRef Team { get; set; }
        public CrmUserRef CreatedBySalesPerson { get; set; }
        public List<ExcelTemplateQuoteColumnBase> QuoteValues { get; set; } = new List<ExcelTemplateQuoteColumnBase>();
        public List<ExcelTemplateQuoteItemColumnBase> QuoteItemValues { get; set; } = new List<ExcelTemplateQuoteItemColumnBase>();

        public void Initialize()
        {
            var quoteColumns = QuoteDefaultColumns();
            var updatedQuoteColumnList = new List<ExcelTemplateQuoteColumnBase>();
            foreach (var quoteColumn in QuoteValues)
            {
                var correctColumnAndClassDerived = quoteColumns.Single(x => x.Id == quoteColumn.Id);
                correctColumnAndClassDerived.Label = quoteColumn.Label;
                updatedQuoteColumnList.Add(correctColumnAndClassDerived);
            }

            QuoteValues = updatedQuoteColumnList;

            var quoteItemColumns = QuoteItemDefaultColumns();
            var updatedQuoteItemColumnList = new List<ExcelTemplateQuoteItemColumnBase>();
            foreach (var quoteItemColumn in QuoteItemValues)
            {
                var correctColumnAndClassDerived = quoteItemColumns.Single(x => x.Id == quoteItemColumn.Id);
                correctColumnAndClassDerived.Label = quoteItemColumn.Label;
                updatedQuoteItemColumnList.Add(correctColumnAndClassDerived);
            }

            QuoteItemValues = updatedQuoteItemColumnList;

        }

        public static ExcelTemplateModel SaveExcelTemplate(ExcelTemplateModel model)
        {
            var template = Helper.FindById(model.Id) ?? new ExcelTemplate()
            {
                Id = ObjectId.Parse(model.Id),
                Team = model.Team,
                CreatedBySalesPerson = model.CreatedBySalesPerson,
            };
            template.Name = model.Name;


            template.QuoteValues.Clear();
            template.QuoteItemValues.Clear();

            var allQuoteColumns = template.QuoteDefaultColumns();
            var allQuoteItemColumns = template.QuoteItemDefaultColumns();

            foreach (var excelTemplateColumnModel in model.QuoteValues)
            {
                var quoteColumn = allQuoteColumns.
                    FirstOrDefault(x => x.Id == excelTemplateColumnModel.Id);
                if (quoteColumn == null) throw new Exception($"Support for FieldName: {excelTemplateColumnModel.FieldName} is not supported");

                quoteColumn.Label = excelTemplateColumnModel.Label;
                template.QuoteValues.Add(quoteColumn);
            }

            foreach (var excelTemplateColumnModel in model.QuoteItemValues)
            {
                var quoteItemColumn = allQuoteItemColumns.
                    FirstOrDefault(x => x.Id == excelTemplateColumnModel.Id);
                if (quoteItemColumn == null) throw new Exception($"Support for FieldName: {excelTemplateColumnModel.FieldName} is not supported");

                quoteItemColumn.Label = excelTemplateColumnModel.Label;

                template.QuoteItemValues.Add(quoteItemColumn);
            }

            Helper.Upsert(template);
            return new ExcelTemplateModel(template);
        }

        public ExcelTemplateQuoteColumnBase GetQuoteColumnForFieldName(string fieldName)
        {
            return QuoteValues.FirstOrDefault(x => x.FieldName == fieldName);
        }

        public ExcelTemplateQuoteItemColumnBase GetQuoteItemColumnForFieldName(string fieldName)
        {
            return QuoteItemValues.FirstOrDefault(x => x.FieldName == fieldName);
        }


        public ExcelTemplateQuoteColumnBase GetQuoteColumnForId(string id)
        {
            return QuoteValues.FirstOrDefault(x => x.Id == id);
        }

        public ExcelTemplateQuoteItemColumnBase GetQuoteItemColumnForId(string id)
        {
            return QuoteItemValues.FirstOrDefault(x => x.Id == id);
        }

        public void SetQuoteColumn(ExcelTemplateQuoteColumnBase column)
        {
            var columnFound = QuoteValues.FirstOrDefault(x => x.Id == column.Id);
            if (columnFound != null)
            {
                columnFound = column;
                Helper.Upsert(this);
                return;
            }
            throw new Exception($"This template no longer has a Quote Column definition for {column.FieldName}");
        }

        public void SetQuoteItemColumn(ExcelTemplateQuoteItemColumnBase column)
        {
            var columnFound = QuoteItemValues.FirstOrDefault(x => x.Id == column.Id);
            if (columnFound != null)
            {
                columnFound = column;
                Helper.Upsert(this);
                return;
            }
            throw new Exception($"This template no longer has a Quote Column definition for {column.FieldName}");
        }


        public void RemoveQuoteColumn(string id)
        {
            var rowFound = QuoteValues.FirstOrDefault(x => x.Id == id);
            if (rowFound != null)
            {
                QuoteValues.Remove(rowFound);
                Helper.Upsert(this);
            }
        }

        public void RemoveQuoteItemColumn(string id)
        {
            var rowFound = QuoteItemValues.FirstOrDefault(x => x.Id == id);
            if (rowFound != null)
            {
                QuoteItemValues.Remove(rowFound);
                Helper.Upsert(this);
            }
        }


        public void UpdateQuoteValues(List<ExcelTemplateQuoteColumnBase> modelValues)
        {
            QuoteValues = modelValues;
            Helper.Upsert(this);
        }

        public void UpdateQuoteItemValues(List<ExcelTemplateQuoteItemColumnBase> modelValues)
        {
            QuoteItemValues = modelValues;
            Helper.Upsert(this);
        }

        public ExcelTemplate()
        {
            //GetDefaultQuoteValues();
            //GetDefaultQuoteItemValues();
        }

        public List<string> GetQuoteHeadings()
        {
            var result = new List<string>();
            foreach (var col in QuoteValues)
            {
                result.Add(col.Label);
            }

            return result;
        }

        public List<string> GetQuoteItemHeadings(QuoteModel model)
        {
            var result = new List<string>();
            foreach (var col in QuoteItemValues)
            {
                if (col.MultipleColumns)
                {
                    var labelsForcolumns = col.GetLabelsFor(model);
                    if (labelsForcolumns.Any()) result.AddRange(labelsForcolumns);
                }
                else
                {
                    result.Add(col.Label);
                }
            }

            return result;
        }


        public List<string> GetQuoteValues(QuoteModel model)
        {
            var result = new List<string>();
            foreach (var col in QuoteValues)
            {
                result.Add(col.GetValueFor(model));
            }

            return result;
        }

        public List<List<string>> GetQuoteItemValues(QuoteModel model)
        {
            var results = new List<List<string>>();
                for (int i = 0; i < model.Items.Count; i++)
                {
                    var result = new List<string>();
                    foreach (var col in QuoteItemValues)
                    {
                        if (col.MultipleColumns)
                        {
                            var valuesForColumns = col.GetValuesFor(model, i);
                            if (valuesForColumns.Any()) result.AddRange(valuesForColumns);
                        }
                        else
                        {
                            result.Add(col.GetValueFor(model,i));
                        }
                    }
                    results.Add(result);
                }

            return results;
        }

        public void GetDefaultQuoteValues()
        {
            QuoteValues.Clear();
            QuoteValues.AddRange(QuoteDefaultColumns());
        }

        public void GetDefaultQuoteItemValues()
        {
            QuoteItemValues.Clear();
            QuoteItemValues.AddRange(QuoteItemDefaultColumns());
        }


        public List<ExcelTemplateQuoteColumnBase> QuoteDefaultColumns()
        {
            var result = new List<ExcelTemplateQuoteColumnBase>
            {
                new ExcelQuoteQuoteId(),
                new ExcelQuoteRevision(),
                new ExcelQuoteRfqNumber(),
                new ExcelQuotePoNumber(),
                new ExcelQuoteCompanyCode(),
                new ExcelQuoteCompanyName(),
                new ExcelQuoteProspectName(),
                new ExcelQuoteShipToName(),
                new ExcelQuoteShipToAddress(),
                new ExcelQuoteOrderDate(),
                new ExcelQuoteValidity(),
                new ExcelQuoteValidityDate(),
                new ExcelQuoteSubmitDate(),
                new ExcelQuoteSubmitTime(),
                new ExcelQuoteLastModified(),
                new ExcelQuoteSalesPerson(),
                new ExcelQuoteContactName(),
                new ExcelQuotePaymentTerms(),
                new ExcelQuoteFreightTerms(),
                new ExcelQuoteCustomerNotes(),
                new ExcelQuoteTotalWeightKilograms(),
                new ExcelQuoteTotalWeightPounds(),
                new ExcelQuoteDisplayCurrency(),
                new ExcelQuoteOrderTotal()
            };
            return result;
        }

        public List<ExcelTemplateQuoteItemColumnBase> QuoteItemDefaultColumns()
        {
            var result = new List<ExcelTemplateQuoteItemColumnBase>
            {
                new ExcelQuoteItemLineType(),
                new ExcelQuoteItemLineItemNumber(),
                new ExcelQuoteItemPieces(),
                new ExcelQuoteItemCurrency(),
                new ExcelQuoteItemQuantity(),
                new ExcelQuoteItemCustomerUom(),
                new ExcelQuoteItemUnitPrice(),
                new ExcelQuoteItemOrderUnit(),
                new ExcelQuoteItemTotal(),
                new ExcelQuoteItemLeadTime(),
                new ExcelQuoteItemPartNumber(),
                new ExcelQuoteItemPartSpecification(),
                new ExcelQuoteItemCustomerNotes(),
                new ExcelQuoteItemStartingProduct(),
                new ExcelQuoteItemFinishedProduct(),
                new ExcelQuoteItemOemType(),
                new ExcelQuoteItemLastModified(),
                new ExcelQuoteItemProductionSteps(),
                new ExcelQuoteItemTestPieces()
            };
            return result;
        }

    }
}
