using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.SqlServer.Model;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee;
using HrsContext = DAL.HRS.SqlServer.Model.HrsContext;

namespace DAL.HRS.Import.ImportHse
{
    [TestFixture()]
    public class UpsertEnvAspects
    {
        private HseHelperMethods _helperMethods = new HseHelperMethods();

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var rep = new RepositoryBase<EnvAspectCriteriaScore>();
            var repEmployee = new RepositoryBase<Employee>();
            using (SqlServer.Model.HrsContext context = new HrsContext())
            {
                foreach (var score in context.EnvironmentalAspectCriteriaScore)
                {
                    var newRow = rep.AsQueryable().FirstOrDefault(x => x.OldHrsId == score.OID) ??
                                 new EnvAspectCriteriaScore()
                                 {
                                     OldHrsId = score.OID
                                 };
                    //newRow.

                    newRow.WorkAreas.ExteriorArea = score.AssignedWorkAreaExteriorArea ?? false;
                    newRow.WorkAreas.Fabrication = score.AssignedWorkAreaFabrication ?? false;
                    newRow.WorkAreas.HeatTreatment = score.AssignedWorkAreaHeatTreatment ?? false;
                    newRow.WorkAreas.MachineShop = score.AssignedWorkAreaMachineShop ?? false;
                    newRow.WorkAreas.Office = score.AssignedWorkAreaOffice ?? false;
                    newRow.WorkAreas.PaintShop = score.AssignedWorkAreaPaintShop ?? false;
                    newRow.WorkAreas.SawShop = score.AssignedWorkAreaSawShop ?? false;
                    newRow.WorkAreas.ShotBlasting = score.AssignedWorkAreaShotBlasting ?? false;
                    newRow.WorkAreas.TestHouse = score.AssignedWorkAreaTestHouse ?? false;
                    newRow.Comments = score.Comments;
                    newRow.AspectCode.Description = score.EnvironmentalAspectAspectCode.Description;
                    newRow.AspectCode.Type = PropertyBuilder.New("EnvironmentAspectCodeType",
                        "Type of Aspect Code", score.EnvironmentalAspectAspectCode.EnvironmentalAspectAspectType1.Name, ""); 
                    newRow.AspectCode.OpportunityType = PropertyBuilder.New("EnvironmentAspectCodeOpportunityType",
                        "Type of Opportunity", score.EnvironmentalAspectAspectCode.EnvironmentalAspectOccurrenceOpportunityType1.Name, "");
                    newRow.AspectCode.Location =
                        _helperMethods.GetLocationForHrsLocation(score.EnvironmentalAspectAspectCode.Location1.Name);
                    newRow.AspectCode.Index = score.EnvironmentalAspectAspectCode.Index ?? 0;
                    newRow.AspectType.Activity = score.EnvironmentalAspectTypeActivity;
                    newRow.AspectType.Description = score.EnvironmentalAspectTypeDescription;
                    newRow.AspectType.Impacts = score.EnvironmentalAspectTypeImpacts;
                    newRow.AspectType.Other = score.EnvironmentalAspectTypeOther;
                    GetLocations(newRow, score);
                    GetReviews(newRow, score);
                    GetScores(newRow, score);
                    GetHistory(newRow, score);
                    rep.Upsert(newRow);
                }
            }

        }

        private static void GetHistory(EnvAspectCriteriaScore newRow, EnvironmentalAspectCriteriaScore score)
        {
            newRow.History.Clear();
            foreach (var history in score.EnvironmentalAspectScoreHistory)
            {
                newRow.History.Add(new AspectScoreHistory()
                {
                    DateUpdated = history.DateUpdated,
                    Reason = history.Reason,
                    WorkAreas = new AssignedWorkAreas()
                    {
                        ExteriorArea = history.EnvironmentalAspectCriteriaScore1.AssignedWorkAreaExteriorArea ?? false,
                        Fabrication = history.EnvironmentalAspectCriteriaScore1.AssignedWorkAreaFabrication ?? false,
                        HeatTreatment = history.EnvironmentalAspectCriteriaScore1.AssignedWorkAreaHeatTreatment ?? false,
                        MachineShop = history.EnvironmentalAspectCriteriaScore1.AssignedWorkAreaMachineShop ?? false,
                        Office = history.EnvironmentalAspectCriteriaScore1.AssignedWorkAreaOffice ?? false,
                        PaintShop = history.EnvironmentalAspectCriteriaScore1.AssignedWorkAreaPaintShop ?? false,
                        SawShop = history.EnvironmentalAspectCriteriaScore1.AssignedWorkAreaSawShop ?? false,
                        ShotBlasting = history.EnvironmentalAspectCriteriaScore1.AssignedWorkAreaShotBlasting ?? false,
                        TestHouse = history.EnvironmentalAspectCriteriaScore1.AssignedWorkAreaTestHouse ?? false,
                    },
                    Score = history.Score ?? 0,
                });
            }
        }

        private static void GetScores(EnvAspectCriteriaScore newRow, EnvironmentalAspectCriteriaScore score)
        {
            newRow.Scores.Clear();
            foreach (var thisScore in score.EnvironmentalAspectScore)
            {
                newRow.Scores.Add(new AspectScore()
                {
                    Score = thisScore.Score ?? 0,
                    CriteriaType = new CriteriaType()
                    {
                        Name = thisScore.EnvironmentalAspectCriteriaType1.Name,
                        Order = thisScore.EnvironmentalAspectCriteriaType1.Order
                    },
                    LastUpdated = thisScore.LastUpdated,
                    Reason = thisScore.Reason
                });
            }
        }

        private static void GetReviews(EnvAspectCriteriaScore newRow, EnvironmentalAspectCriteriaScore score)
        {
            newRow.Reviews.Clear();
            foreach (var review in score.EnvironmentalAspectReview)
            {
                newRow.Reviews.Add(new Review()
                {
                    Comment = review.Comment,
                    CurrentReviewDate = review.CurrentReviewDate,
                    NextReviewDate = review.NextReviewDate,
                    ReviewedBy = review.ReviewedBy
                });
            }
        }

        private void GetLocations(EnvAspectCriteriaScore newRow, EnvironmentalAspectCriteriaScore score)
        {
            newRow.Locations.Clear();
            foreach (var location in score.EnvironmentalAspectCriteriaScore_Location)
            {
                newRow.Locations.Add(_helperMethods.GetLocationForHrsLocation(location.Location.Name));
            }
        }
    }

}
