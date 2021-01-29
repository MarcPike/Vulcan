using DAL.HRS.Mongo.DocClass.Legal;
using DAL.HRS.Mongo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperLegal : IHelperLegal
    {
        public List<RegulatoryIncidentGridModel> GetRegulatoryIncidentsGrid()
        {
            var filter = RegulatoryIncident.Helper.FilterBuilder.Empty;
            var projection = RegulatoryIncident.Helper.ProjectionBuilder.Expression(x =>
                new RegulatoryIncidentGridModel()
                {
                    Id = x.Id,
                    IncidentId = x.IncidentId,
                    IncidentDate = x.IncidentDate,
                    RegulatoryType = x.RegulatoryType,
                    IncidentType = x.IncidentType,
                    Complainant = x.Complainant,
                    Location = x.Location,
                    IncidentStatus = x.Status

                });

            return RegulatoryIncident.Helper.FindWithProjection(filter, projection).ToList();
        }
    }
}
