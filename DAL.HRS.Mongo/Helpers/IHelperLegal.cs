using DAL.HRS.Mongo.Models;
using System.Collections.Generic;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperLegal
    {
        List<RegulatoryIncidentGridModel> GetRegulatoryIncidentsGrid();
    }
}