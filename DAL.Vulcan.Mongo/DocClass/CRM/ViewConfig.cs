using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class ViewConfig
    {
        [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        [BsonRepresentation(BsonType.String)]
        public ViewType ViewType { get; set; } = ViewType.MyStuff;

        public bool ShowMyStuff { get; set; } = false;

        public string ViewTypeName => ViewType.ToString();
        public TeamRef Team { get; set; }

        public List<CompanyRef> SelectedAlliances { get; set; } = new List<CompanyRef>();
        public List<CompanyRef> SelectedNonAlliances { get; set; } = new List<CompanyRef>();

        public string DefaultCurrency
        {
            get
            {
                if (Team == null) return string.Empty;

                var team = Team.AsTeam();
                return team.DefaultCurrency ?? "USD";
            }
        }

        public string InventoryCoid
        {
            get
            {
                if (Team == null) return String.Empty;
                var team = Team.AsTeam();
                var location = team.Location.AsLocation();
                return location.GetCoid();

            }
        }
        private void Reset()
        {
            //Team = null;
        }

        public void SetViewMyStuff()
        {
            Reset();
            ViewType = ViewType.MyStuff;
        }

        public void SetViewTeam(Team team)
        {
            Reset();
            ViewType = ViewType.Team;
            Team = team.AsTeamRef();
        }

        public void GetSelectedCompanies(UserRef userRef)
        {
            if (Team == null) return;

            var team = Team.AsTeam();

            //SelectedAlliances = team.Alliances;
            //SelectedNonAlliances = team.NonAlliances;


            //var companyViewSelections = TeamUserCompanyViewSelections.GetTeamUserCompanyViewSelections(userRef, Team);
            //SelectedAlliances = companyViewSelections.Alliances.Where(x => x.Selected).Select(x=>x.Company).ToList();
            //SelectedNonAlliances = companyViewSelections.NonAlliances.Where(x => x.Selected).Select(x => x.Company).ToList();

            //foreach (var companyRef in SelectedAlliances.Where(x=>x.Coid == null).ToList())
            //{
            //    var company = companyRef.AsCompany();
            //    companyRef.Coid = company.Location.GetCoid();
            //}
            //foreach (var companyRef in SelectedNonAlliances.Where(x => x.Coid == null).ToList())
            //{
            //    var company = companyRef.AsCompany();
            //    companyRef.Coid = company.Location.GetCoid();
            //}
        }

        public void SetViewTeam(TeamRef teamRef)
        {
            Reset();
            ViewType = ViewType.Team;
            Team = teamRef;
        }

    }
}