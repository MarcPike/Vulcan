using System;
using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.AuditTrails;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeAuditTrailHistoryModel
    {
        public EmployeeAuditTrailHistoryModel()
        {
        }

        public EmployeeAuditTrailHistoryModel(DateTime fromDate, string module, HrsUser hrsUser,
            List<string> empIdList = null)
        {
            if (empIdList == null)
            {
                var role = SecurityRoleHelper.GetHrsSecurityRole(hrsUser);
                var employee = hrsUser.Employee.AsEmployee();

                var empFilter = Employee.Helper.FilterBuilder.Empty;

                if (role.DirectReportsOnly)
                {
                    empFilter = EmployeeDirectReportsFilterGenerator.GetDirectReportsOnlyFilter(Employee.Helper, role,
                        employee, empFilter);
                }
                else
                {
                    var locations = hrsUser.HrsSecurity.Locations;
                    empFilter = EmployeeLocationsFilterGenerator.GetLocationsFilter(Employee.Helper, locations,
                        empFilter);
                }

                var projectionEmployeeId = Employee.Helper.ProjectionBuilder.Expression(x => x.Id.ToString());


                empIdList = Employee.Helper.FindWithProjection(empFilter, projectionEmployeeId).ToList();

                if (empIdList.All(x => x != employee.Id.ToString())) empIdList.Add(employee.Id.ToString());
            }
            
            EmployeeAuditTrail.CalculateRequired();

            List<EmployeeAuditTrail> audits;

            if (module == "EmployeeDetails")
            {
                //audits =
                //    EmployeeAuditTrail.Helper.Find(x => x.UpdatedAt >= fromDate &&
                //                                        x.AuditTrail.EmployeeDetailsChanges.ValueChanges.Any(v =>
                //                                            EmployeeAuditTrailFlatModel.LimitEmployeeDetailFieldsTo
                //                                                .Contains(v.FieldName))).ToList();

                var queryHelper = new MongoRawQueryHelper<EmployeeAuditTrail>();
                var filterAuditTrail = queryHelper.FilterBuilder.In(x => x.AuditTrail.Employee.Id, empIdList);
                var filter = queryHelper.FilterBuilder.Where(x => x.AuditTrail.UpdatedAt >= fromDate &&
                                                                  x.AuditTrail.EmployeeDetailsChanges.ValueChanges.Any(
                                                                      v =>
                                                                          EmployeeAuditTrailFlatModel
                                                                              .LimitEmployeeDetailFieldsTo
                                                                              .Contains(v.FieldName)));

                var project = queryHelper.ProjectionBuilder.Expression(x => new EmployeeAuditTrail
                {
                    AuditTrail = x.AuditTrail,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedAt = x.UpdatedAt
                });

                audits = queryHelper.FindWithProjection(filter & filterAuditTrail, project).ToList();
            }
            else if (module == "Benefits")
            {
                audits =
                    EmployeeAuditTrail.Helper.Find(x => x.UpdatedAt >= fromDate &&
                                                        (x.AuditTrail.BenefitsChanges.ValueChanges.Count > 0
                                                         || x.AuditTrail.BenefitsChanges.ListChanges.Count > 0))
                        .ToList();
            }
            else if (module == "Compensation")
            {
                audits =
                    EmployeeAuditTrail.Helper.Find(x => x.UpdatedAt >= fromDate &&
                                                        (x.AuditTrail.CompensationChanges.ValueChanges.Count > 0
                                                         || x.AuditTrail.CompensationChanges.ListChanges.Count > 0))
                        .ToList();
            }
            else if (module == "Discipline")
            {
                audits =
                    EmployeeAuditTrail.Helper.Find(x => x.UpdatedAt >= fromDate &&
                                                        (x.AuditTrail.DisciplineChanges.ValueChanges.Count > 0
                                                         || x.AuditTrail.DisciplineChanges.ListChanges.Count > 0))
                        .ToList();
            }
            else if (module == "Performance")
            {
                audits =
                    EmployeeAuditTrail.Helper.Find(x => x.UpdatedAt >= fromDate &&
                                                        (x.AuditTrail.PerformanceChanges.ValueChanges.Count > 0
                                                         || x.AuditTrail.PerformanceChanges.ListChanges.Count > 0))
                        .ToList();
            }
            else
            {
                throw new Exception(
                    "Module must be Benefits | Compensation | Discipline | EmployeeDetails | Performance");
            }


            foreach (var audit in audits)
            {
                var newRows = EmployeeAuditTrailFlatModel.FlattenEmployeeAuditTrail(audit.AuditTrail, module);
                if (newRows.Any()) AuditTrailHistory.AddRange(newRows);
            }
        }

        //public DateTime FromDate { get; set; }
        public List<EmployeeAuditTrailFlatModel> AuditTrailHistory { get; set; } =
            new List<EmployeeAuditTrailFlatModel>();
    }
}