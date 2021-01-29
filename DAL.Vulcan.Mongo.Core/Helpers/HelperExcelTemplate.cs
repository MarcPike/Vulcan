using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public class HelperExcelTemplate : HelperBase, IHelperExcelTemplate
    {
        public HelperExcelTemplate()
        {
        }

        public ExcelTemplateModel GetNewExcelTemplateModel(CrmUser crmUser)
        {
            var newTemplate = new ExcelTemplate()
            {
                Name = string.Empty,
                Team = crmUser.ViewConfig.Team,
                CreatedBySalesPerson = crmUser.AsCrmUserRef(),
            };
            //newTemplate.GetDefaultQuoteValues();
            //newTemplate.GetDefaultQuoteItemValues();
            return new ExcelTemplateModel(newTemplate);
        }

        public ExcelTemplateModel SaveExcelTemplate(ExcelTemplateModel model)
        {
            return ExcelTemplate.SaveExcelTemplate(model);
        }

        public void RemoveExcelTemplate(string templateId)
        {
            ExcelTemplate.Helper.DeleteOne(templateId);
        }

        public ExcelTemplateModel GetExcelTemplateModel(string templateId)
        {
            var template = ExcelTemplate.Helper.FindById(templateId);
            if (template == null) throw new Exception("Template not found");

            return new ExcelTemplateModel(template);
        }

        public List<ExcelTemplateModel> GetTemplatesForTeam(TeamRef team)
        {
            var result = new List<ExcelTemplateModel>();
            var templates = ExcelTemplate.Helper.Find(x => x.Team.Id == team.Id).ToList();
            foreach (var excelTemplate in templates)
            {
                result.Add(new ExcelTemplateModel(excelTemplate));
            }

            return result;
        }

        public IWorkbook GenerateExcelStream(string application, string userId, string quoteId, string templateId)
        {
            var quote = CrmQuote.Helper.FindById(quoteId);
            if (quote == null) throw new Exception("Quote not found");

            var template = ExcelTemplate.Helper.FindById(templateId);
            if (template == null) throw new Exception("Template not found");

            template.Initialize();

            var quoteModel = new QuoteModel(application, userId, quote);

            var memory = new MemoryStream();

            IWorkbook workbook = new XSSFWorkbook();

            IFont font = workbook.CreateFont();
            font.IsBold = true;
            font.FontHeightInPoints = 11;
            //font.Color = 1;
            ICellStyle boldStyle = workbook.CreateCellStyle();
            boldStyle.Alignment = HorizontalAlignment.Center;
            boldStyle.SetFont(font);
            //boldStyle.FillBackgroundColor = 12;


            ICellStyle dataStyle = workbook.CreateCellStyle();
            dataStyle.Alignment = HorizontalAlignment.Center;


            ISheet quoteSheet = workbook.CreateSheet("Quote");
            var itemRow = -1;

            IRow quoteHeadingRow = quoteSheet.CreateRow(++itemRow);
            var quoteHeadings = template.GetQuoteHeadings();
            var cellColumn = -1;
            foreach (var quoteHeading in quoteHeadings)
            {
                cellColumn++;
                var cell = quoteHeadingRow.CreateCell(cellColumn);
                cell.SetCellValue(quoteHeading);
                cell.CellStyle = boldStyle;
            }

            IRow quoteValueRow = quoteSheet.CreateRow(++itemRow);
            var quoteValues = template.GetQuoteValues(quoteModel);
            cellColumn = -1;
            foreach (var quoteValue in quoteValues)
            {
                cellColumn++;
                var cell = quoteValueRow.CreateCell(cellColumn);
                cell.SetCellValue(quoteValue);
                cell.CellStyle = dataStyle;
            }

            for (int i = 0; i <= cellColumn; i++) quoteSheet.AutoSizeColumn(i);

            ISheet quoteItemSheet = workbook.CreateSheet("QuoteItems");
            itemRow = -1;

            IRow quoteItemRow = quoteItemSheet.CreateRow(++itemRow);
            var quoteItemHeadings = template.GetQuoteItemHeadings(quoteModel);
            cellColumn = -1;
            foreach (var quoteItemHeading in quoteItemHeadings)
            {
                cellColumn++;
                var cell = quoteItemRow.CreateCell(cellColumn);
                cell.SetCellValue(quoteItemHeading);
                cell.CellStyle = boldStyle;
            }

            var quoteItems = template.GetQuoteItemValues(quoteModel);
            foreach (var quoteItem in quoteItems)
            {
                quoteItemRow = quoteItemSheet.CreateRow(++itemRow);
                cellColumn = -1;
                foreach (var quoteItemValue in quoteItem.ToList())
                {
                    cellColumn++;
                    var cell = quoteItemRow.CreateCell(cellColumn);
                    cell.SetCellValue(quoteItemValue);
                    cell.CellStyle = dataStyle;
                }
            }

            for (int i = 0; i <= cellColumn; i++)
            {
                quoteItemSheet.AutoSizeColumn(i);
            }

            return workbook;

        }
    }
}
