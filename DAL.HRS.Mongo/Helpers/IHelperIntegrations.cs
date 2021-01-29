using System.Collections.Generic;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperIntegrations
    {
        List<KronosExportModel> KronosExport(string entityName);
        List<HalogenExportModel> HalogenExport(string entityName);
    }

}