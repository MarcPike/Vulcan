using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Models;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperExcelTemplate
    {
        IWorkbook GenerateExcelStream(string application, string userId, string quoteId, string templateId);
        ExcelTemplateModel GetNewExcelTemplateModel(CrmUser crmUser);
        List<ExcelTemplateModel> GetTemplatesForTeam(TeamRef team);
        ExcelTemplateModel SaveExcelTemplate(ExcelTemplateModel model);
        void RemoveExcelTemplate(string templateId);
        ExcelTemplateModel GetExcelTemplateModel(string templateId);
    }
}