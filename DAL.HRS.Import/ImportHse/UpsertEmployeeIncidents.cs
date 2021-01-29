using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.SqlServer;
using DAL.HRS.SqlServer.Model;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using BodyContact = DAL.HRS.Mongo.DocClass.Hse.BodyContact;
using ContactWith = DAL.HRS.Mongo.DocClass.Hse.ContactWith;
using DrugTest = DAL.HRS.Mongo.DocClass.Hse.DrugTest;
using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee;
using EquipmentInvolved = DAL.HRS.Mongo.DocClass.Hse.EquipmentInvolved;
using HrsContext = DAL.HRS.SqlServer.Model.HrsContext;
using MedicalLeave = DAL.HRS.Mongo.DocClass.Hse.MedicalLeave;
using MedicalTreatment = DAL.HRS.Mongo.DocClass.Hse.MedicalTreatment;
using MedicalWorkStatus = DAL.HRS.Mongo.DocClass.Hse.MedicalWorkStatus;
using RootCause = DAL.HRS.Mongo.DocClass.Hse.RootCause;
using NatureOfInjury = DAL.HRS.Mongo.DocClass.Hse.NatureOfInjury;
using Witness = DAL.HRS.Mongo.DocClass.Hse.Witness;


namespace DAL.HRS.Import.ImportHse
{
    [TestFixture()]
    public class UpsertEmployeeIncidents
    {
        private HseHelperMethods _helperMethods = new HseHelperMethods();

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void ReloadJobFactors()
        {
            var rep = new RepositoryBase<EmployeeIncident>();
            var repEmployee = new RepositoryBase<Employee>();
            var incidents = rep.AsQueryable().ToList();
            var encryption = Encryption.NewEncryption;

            var defaultEmployeeRef = repEmployee.AsQueryable()
                .First(x => x.PreferredName == "Denise" && x.LastName == "Walker").AsEmployeeRef();
            var myHrsUser = new RepositoryBase<HrsUser>().AsQueryable().ToList()
                .First(x => x.Employee.PreferredName == "Denise" && x.Employee.LastName == "Walker");

            using (SqlServer.Model.HrsContext context = new HrsContext())
            {
                foreach (var incident in context.Incident.AsNoTracking().Where(x => x.GCRecord == null).ToList())
                {
                    var existingIncident = incidents.FirstOrDefault(x => x.OldHrsId == incident.OID);

                    if (existingIncident == null) continue;

                    if (incident.JobFactors.Any())
                    {
                        if (!existingIncident.JobFactors.Any())
                        {
                            Console.WriteLine("Found one missing job factors");
                        }
                    }



                }



            }
        }

        [Test]
        public void Execute()
        {
            //FileAttachmentsHrs.RemoveAllDocumentsForModule("Incident Reporting Screen");
            var rep = new RepositoryBase<EmployeeIncident>();
            var repEmployee = new RepositoryBase<Employee>();
            var incidents = rep.AsQueryable().ToList();
            var encryption = Encryption.NewEncryption;

            var defaultEmployeeRef = repEmployee.AsQueryable()
                .First(x => x.PreferredName == "Denise" && x.LastName == "Walker").AsEmployeeRef();
            var myHrsUser = new RepositoryBase<HrsUser>().AsQueryable().ToList()
                    .First(x => x.Employee.PreferredName == "Denise" && x.Employee.LastName == "Walker");

            var onRow = 0;
            var newRows = 0;

            using (SqlServer.Model.HrsContext context = new HrsContext())
            {
                foreach (var incident in context.Incident.AsNoTracking().Where(x=>x.GCRecord == null && x.OID == 2907).ToList())
                {
                    onRow++;
                    var newRow = incidents.FirstOrDefault(x => x.OldHrsId == incident.OID);

                    if (newRow == null)
                    {
                        newRows++;
                        newRow = new EmployeeIncident()
                        {
                            OldHrsId = incident.OID
                        };
                    }
                                 
                    if (incident.Employee != null)
                    {

                        var employeeRef = repEmployee.AsQueryable()
                            .FirstOrDefault(x => x.OldHrsId == (incident.Employee))?.AsEmployeeRef();
                        if (employeeRef != null)
                        {
                            newRow.Employee = employeeRef;
                        }


                    }

                    newRow.IncidentDate = incident.IncidentDate;

                    if (incident.Manager != null)
                    {
                        var employeeRef = repEmployee.AsQueryable()
                            .FirstOrDefault(x => x.OldHrsId == (incident.Manager))?.AsEmployeeRef();
                        if (employeeRef != null)
                        {
                            newRow.Manager = employeeRef;
                        }
                    }

                    if (incident.Location1 != null)
                    {
                        newRow.Location = _helperMethods.GetLocationForHrsLocation(incident.Location1.Name);
                    }

                    newRow.IncidentId = incident.OID;
                    newRow.IncidentType = PropertyBuilder.New("IncidentType", "Type of Incident",
                        incident.IncidentType1?.IncidentTypeName ?? "<unspecified>", "");

                    newRow.Osha = incident.OSHA ?? false;
                    newRow.Riddor = incident.RIDDOR ?? false;
                    newRow.NearMissTypeCode = PropertyBuilder.New("NearMissTypeCode","Type of Near Miss",
                        incident.IncidentNearMissType1?.PrefixForIncidenIdentifier ?? "<unspecified>",
                        incident.IncidentNearMissType1?.Incident_NearMissTypeName ?? "<unspecified>");

                    newRow.NatureOfInjuries = incident.NatureOfInjury.Select(x => new NatureOfInjury()
                    {
                        Type = PropertyBuilder.New("NatureOfInjuryType", "Nature of Injury",
                            x.NatureOfInjuryType1.NatureOfInjuryName, ""),
                        Comments = x.Comment
                    }).ToList();

                    
                    newRow.RootCauses = incident.RootCause.Select(x => new RootCause()
                    {
                        Type = PropertyBuilder.New("RootCauseType", "Root cause of incident",
                            x.RootCauseType1.RootCauseName, ""),
                        Comments = x.Comment
                    }).ToList();

                    newRow.PersonalFactors = incident.PersonalFactors.Select(x => new PersonalFactor()
                    {
                        Type = PropertyBuilder.New("PersonalFactorType", "Personal Factors involved with Incident", x.PersonalFactorType1.PersonalFactorName, ""),
                        Comments = x.Comment

                    }).ToList();


                    newRow.Witnesses = incident.Witness.Select(x => new Witness()
                    {
                        WitnessName = x.WitnessName,
                        WitnessDate = x.WitnessDate

                    }).ToList();

                    newRow.AdditionalComments = incident.AdditionalComments;
                    newRow.PhysicalLocation = incident.PhysicalLocation ?? string.Empty;
                    newRow.IncidentTypeNote = incident.IncidentTypeNote ?? string.Empty;

                    newRow.SeverityTypeCode = PropertyBuilder.New("SeverityTypeCode", "Type of Severity", incident.SeverityType1?.SeverityTypeName ?? "<unspecified>", incident.SeverityType1.SeverityTypeName ?? "<unspecified>");
                    newRow.SeverityTypeNote = incident.SeverityTypeNote ?? string.Empty;

                    newRow.WorkBeingPerformed = incident.WorkBeingPerformed ?? string.Empty;

                    newRow.ImmediateCauses = incident.ImmediateCauses;

                    newRow.CorrectiveAction = incident.CorrectiveAction ?? string.Empty;
                    newRow.PreventativeAction = incident.PreventativeAction ?? string.Empty;
                    newRow.Recommendation = incident.Recommendation ?? string.Empty;

                    if (incident.ApprovedBy != null)
                    {
                        var employeeRef = repEmployee.AsQueryable()
                            .FirstOrDefault(x => x.OldHrsId == (incident.ApprovedBy))?.AsEmployeeRef();
                        if (employeeRef != null)
                        {
                            newRow.ApprovedBy = employeeRef;
                        }

                    }

                    newRow.Investigations = incident.InvestigatedBy.Select(x => new Investigation()
                    {
                        InvestigatedDate = x.InvestigatedDate,
                        InvestigatorName = x.InvestigatorName

                    }).ToList();


                    newRow.ApprovedDate = incident.ApprovedByDate;
                    if (incident.DrugTest != null)
                    {
                        var drugTest = context.DrugTest.FirstOrDefault(x => x.OID == incident.DrugTest);


                        if (newRow.DrugTest != null)
                        {
                            var queryHelperDrugTest = new MongoRawQueryHelper<DrugTest>();
                            var objectId = ObjectId.Parse(newRow.DrugTest.Id);
                            var filter = queryHelperDrugTest.FilterBuilder.Where(x => x.Id == objectId);
                            queryHelperDrugTest.DeleteOne(filter);
                        }

                        if (drugTest != null)
                        {
                            var leaveHistory = new List<IncidentLeave>();
                            if (drugTest.MedicalInfo1?.LeaveHistory != null)
                            {
                                foreach (var leave in drugTest.MedicalInfo1.LeaveHistory)
                                {
                                    leaveHistory.Add(new IncidentLeave()
                                    {
                                        FromDate = leave.FromDate,
                                        ToDate = leave.ToDate,
                                        EligibleMedicalLeave = leave.Eligible_Medical_Leave ?? false,
                                        Notes = leave.Notes,
                                        ReasonCode = PropertyBuilder.CreatePropertyValue("IncidentLeaveReasonCode", "Incident leave reason", leave.ReasonCodes1?.ReasonName ?? "<unspecified>", "").AsPropertyValueRef(),
                                        MedicalLeaveType = PropertyBuilder.CreatePropertyValue("MedicalLeaveType", "Medical leave type", leave.MedicalLeaveType1?.Name ?? "<unspecified>", "").AsPropertyValueRef(),
                                    });
                                }

                            }

                            var newDrugTestMedicalInfo = new DrugTestMedicalInfo()
                            {
                                Comments = drugTest.Comments,
                                LeaveHistory = leaveHistory
                            };

                            

                            var newDrugTest = new DrugTest()
                            {
                                DrugTestType = PropertyBuilder.New("DrugTestType","Type of drug test", drugTest.DrugTestType?.Name ?? "<unspecified>", ""),
                                DrugTestResult = PropertyBuilder.New("DrugTestResult", "Result of drug test", drugTest.DrugTestResult?.Name ?? "<unspecified>", ""),

                                MedicalInfo = newDrugTestMedicalInfo,
                                TestDate = drugTest.TestDate,
                                ResultDate = drugTest.ResultDate,
                                Comments = drugTest.Comments
                            };
                            newDrugTest.SaveToDatabase();
                            newRow.DrugTest = newDrugTest.AsDrugTestRef();
                        }
                    }

                    newRow.AffectedBodyParts.Clear();
                    foreach (var bodyPart in incident.AffectedBodyParts)
                    {
                        newRow.AffectedBodyParts.Add(new AffectedBodyPart()
                        {
                            Type = PropertyBuilder.New("BodyPartType","Body part type",bodyPart.BodyParts1?.BodyPartName ?? "<unspecified>",""),
                            Comments = bodyPart.Comment
                        });
                    }

                    newRow.BodyContacts.Clear();
                    foreach (var bodyContact in incident.BodyContact)
                    {
                        newRow.BodyContacts.Add(new BodyContact()
                        {
                            Type = PropertyBuilder.New("BodyContactType", "Type of body contact", bodyContact.BodyContactType1?.BodyContactName ?? "<unspecified>", ""),
                            Comments = bodyContact.Comment
                        });
                        
                    }

                    newRow.ContactWith.Clear();
                    foreach (var contactWith in incident.ContactWith)
                    {
                        newRow.ContactWith.Add(new ContactWith()
                        {
                            Type = PropertyBuilder.New("ContactWithType", "Contact with type", contactWith.ContactWithType1?.ContactWithName ?? "<unspecified>", ""),
                            Comments = contactWith.Comment

                        });
                    }

                    newRow.EquipmentInvolved.Clear();
                    foreach (var equipment in incident.EquipmentInvolved)
                    {
                        
                        newRow.EquipmentInvolved.Add(new EquipmentInvolved()
                        {
                            EquipmentCode = PropertyBuilder.New("EquipmentCode","Equipment involved code", equipment.EquipmentInvolvedCode1?.EquipmentInvolvedCodeName ?? "<unspecified>",""),
                            EquipmentType = PropertyBuilder.New("EquipmentType", "Equipment involved type", equipment.EquipmentInvolvedType1?.EquipmentInvolvedTypeName ?? "<unspecified>", ""),
                            Comments = equipment.Comment
                        });
                    }

                    newRow.IncidentStatus = PropertyBuilder.New("IncidentStatusType", "Type of incident",
                        incident.Status1?.StatusName ?? "<unspecified>", "");

                    

                    newRow.OneDayNotificationSent = incident.OneDayNotificationSent ?? false;
                    newRow.TenDayNotificationSent = incident.TenDayNotificationSent ?? false;

                    newRow.SubmittedFromWeb = incident.SubmittedFromWeb ?? false;

                    newRow.FirstAidTreatment = PropertyBuilder.New("FirstAidTreatmentType", "First aid treatment type",
                        incident.FirstAidTreatment1?.Name ?? "<no first aid treatment>", "");
                    newRow.FirstAidTreatmentAdministered = incident.FirstAidTreatmentAdministered;

                    newRow.IIQ.EmployeeAuthorized = incident.IIQ_EmployeeAuthorized ?? false;
                    newRow.IIQ.EmployeeAuthorizedComment = incident.IIQ_EmployeeAuthorized_Comment;

                    newRow.IIQ.EquipmentInDangerousPosition = incident.IIQ_EquipmentInDangerousPosition ?? false;
                    newRow.IIQ.EquipmentInDangerousPositionComments = incident.IIQ_EquipmentInDangerousPosition_Comment;

                    newRow.IIQ.EquipmentPreChecksCompleted = incident.IIQ_EquipmentPreChecksCompleted ?? false;
                    newRow.IIQ.EquipmentPreChecksCompletedComments = incident.IIQ_EquipmentPreChecksCompleted_Comment;

                    newRow.IIQ.OthersInAreaWarned = incident.IIQ_OthersInAreaWarned ?? false;
                    newRow.IIQ.OthersInAreaWarnedComments = incident.IIQ_OthersInAreaWarned_Comment;

                    newRow.IIQ.RiskAssessmentAvailable = incident.IIQ_RiskAssessmentAvailable ?? false;
                    newRow.IIQ.RiskAssessmentAvailableComments = incident.IIQ_RiskAssessmentAvailable_Comment;

                    newRow.ImmediateActionTakenNote = incident.ImmediateActionTakenNote;

                    newRow.IIQ.SafeWorkingProceduresFollowed = incident.IIQ_SafeWorkingProceduresFollowed ?? false;
                    newRow.IIQ.SafeWorkingProceduresFollowedComments = incident.IIQ_SafeWorkingProceduresFollowed_Comment;

                    newRow.IIQ.UsingEquipmentCorrectly = incident.IIQ_UsingEquipmentCorrectly ?? false;
                    newRow.IIQ.UsingEquipmentCorrectlyComments = incident.IIQ_UsingEquipmentCorrectly_Comment;

                    newRow.IIQ.UsingRequiredPPE = incident.IIQ_UsingRequiredPPE ?? false;
                    newRow.IIQ.UsingRequiredPPEComments = incident.IIQ_UsingRequiredPPE_Comment;

                    newRow.JobFactors.Clear();
                    foreach (var factor in incident.JobFactors)
                    {
                        newRow.JobFactors.Add(new JobFactor()
                        {
                            Type = PropertyBuilder.New("JobFactorType", "Type of Job Factor",factor.JobFactorType1?.JobFactorName ?? "<unspecified>", ""),
                            Comments = factor.Comment
                        }); 
                    }

                    newRow.LackOfControls.Clear();
                    foreach (var lackOfControl in incident.LackOfControls)
                    {
                        newRow.LackOfControls.Add(new LackOfControl()
                        {
                            Type = PropertyBuilder.New("LackOfControlType","Lack of control type",lackOfControl.LackOfControlType1?.LackOfControlName ?? "<unspecified>",""),
                            Comments = lackOfControl.Comment
                        });
                    }

                    newRow.MedicalLeaves.Clear();
                    foreach (var leave in incident.MedicalLeave)
                    {
                        newRow.MedicalLeaves.Add(new MedicalLeave()
                        {
                            DateOfActualReturn = leave.DateOfActualReturn,
                            DateOfExpectedReturn = leave.DateOfExpectedReturn,
                            DateOfLeave = leave.DateOfLeave,
                            DaysAway = leave.DaysAway,
                            DaysRestrictedWork = leave.DaysRestrictedWork,
                            HoursAway = leave.HoursAway
                        });
                    }

                    newRow.MedicalTreatments.Clear();
                    foreach (var treatment in incident.MedicalTreatment)
                    {
                        newRow.MedicalTreatments.Add(new MedicalTreatment()
                        {
                            DateOfFollowup = treatment.DateOfFollowup,
                            DateOfTreatment = treatment.DateOfTreatment,
                            Doctor = treatment.Doctor,
                            FollowupRequired = treatment.FollowupRequired,
                            TreatmentFacility = treatment.TreatmentFacility
                        });
                    }

                    newRow.MedicalWorkStatus.Clear();
                    foreach (var workStatus in incident.MedicalWorkStatus)
                    {
                        newRow.MedicalWorkStatus.Add(new MedicalWorkStatus()
                        {
                            Comments = workStatus.Comment,
                            Date = workStatus.Date,
                            Type = PropertyBuilder.New("MedicalWorkStatusType","Type of medical work status", workStatus.MedicalWorkStatusType?.Name ?? "<unspecified>","")
                        });
                    }

                    newRow.MedicalNotes = incident.MedicalNotes;

                    newRow.MedicalTreatmentAdministeredType = PropertyBuilder.New("MedicalTreatmentAdministeredType",
                        "Type of medical treatment administered",
                        incident.MedicalTreatmentAdministeredType?.MedicalTreatmentAdministeredTypeName ?? "<no medical treatment administered>", "");

                    var newIncident = rep.Upsert(newRow);

                    // Now for supporting documents

                    var drugTestSupportingDocument = context.DrugTest.FirstOrDefault(x => x.OID == incident.DrugTest);
                    if (drugTestSupportingDocument != null)
                    {
                        ImportSupportingDocumentDrugTestMedicalInfo(drugTestSupportingDocument, encryption, myHrsUser, newRow);
                    }

                    ImportMedicalSupportingDocument(incident, encryption, myHrsUser, newIncident);

                    if (incident.IncidentImage != null)
                    {
                        ImportIncidentImage(incident, encryption, myHrsUser, newIncident);

                    }

                    //if ((onRow % 200) == 0)
                    //{
                    //    var status = $"On row: {onRow} New rows: {newRows}";
                    //}

                }

                Console.WriteLine($"Total rows: {onRow} New Rows: {newRows}");

            }
        }

        [Test]
        public void GetImmediateCauses()
        {
            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            using (SqlServer.Model.HrsContext context = new HrsContext())
            {
                foreach (var incident in context.Incident.AsNoTracking().Where(x=> x.ImmediateCauses != null && x.ImmediateCauses != string.Empty).ToList())
                {
                    var filter = queryHelper.FilterBuilder.Where(x => x.OldHrsId == incident.OID);
                    var employeeIncident = queryHelper.Find(filter).FirstOrDefault();
                    if (employeeIncident != null)
                    {
                        employeeIncident.ImmediateCauses = incident.ImmediateCauses;
                        queryHelper.Upsert(employeeIncident);
                    }
                }
            }
        }

            private static void ImportIncidentImage(Incident incident, Encryption encryption, HrsUser myHrsUser,
    EmployeeIncident newIncident)
        {
            //return;
            if (incident.IncidentImage != null)
            {
                var queryHelper = new MongoRawQueryHelper<SupportingDocument>();
                var filter = queryHelper.FilterBuilder.Where(x =>
                    x.BaseDocumentId == newIncident.Id && x.FileName == $"Incident_{newIncident.Id.ToString()}_Image.jpg");
                if (queryHelper.Find(filter).Any())
                {
                    return;
                }

                if (incident.IncidentImage == null) return;

                byte[] fileBytes;
                try
                {
                    fileBytes = encryption.Decrypt<byte[]>(incident.IncidentImage);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                if (fileBytes == null || fileBytes.Length == 0) return;


                var comments = string.Empty;
                var fileName = $"Incident_{newIncident.Id.ToString()}_Image.jpg";
                var documentDate = DateTime.Now;
                var documentType = "IncidentImage";
                var userId = myHrsUser.UserId;

                try
                {
                    var mongoContext = (Vulcan.Mongo.Base.Context.HrsContext)queryHelper.Context;
                    FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate,
                        newIncident,
                        "Incident Reporting Screen", documentType, userId, comments, newIncident.OldHrsId, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType}");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void ImportMedicalSupportingDocument(Incident incident, Encryption encryption, HrsUser myHrsUser,
            EmployeeIncident newIncident)
        {
            foreach (var doc in incident.MedicalSupportingDocument)
            {
                var queryHelper = new MongoRawQueryHelper<SupportingDocument>();
                var filter = queryHelper.FilterBuilder.Where(x =>
                    x.BaseDocumentId == newIncident.Id && x.FileName == doc.MyFileData.FileName);
                if (queryHelper.Find(filter).Any())
                {
                    continue;
                }



                byte[] fileBytes;
                if (doc.MyFileData == null) continue;
                if (doc.IncidentSupportingDocumentType == null) continue;


                try
                {
                    fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }

                if (fileBytes == null || fileBytes.Length == 0) continue;


                var comments = doc.Comments ?? string.Empty;
                var fileName = doc.MyFileData.FileName;
                var documentDate = doc.DocumentDate ?? DateTime.Now;
                var documentType = doc.IncidentSupportingDocumentType.Name.Replace("/", "-");
                var userId = myHrsUser.UserId;


                try
                {
                    var mongoContext = (Vulcan.Mongo.Base.Context.HrsContext) queryHelper.Context;
                    FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate,
                        newIncident,
                        "Incident Reporting Screen", documentType, userId, comments, newIncident.OldHrsId, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType}");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void ImportSupportingDocumentDrugTestMedicalInfo(SqlServer.Model.DrugTest drugTest, Encryption encryption,
            HrsUser myHrsUser, EmployeeIncident newRow)
        {
            foreach (var doc in drugTest.MedicalInfo1.MedicalInfoSupportingDocument)
            {
                var queryHelper = new MongoRawQueryHelper<SupportingDocument>();
                var filter = queryHelper.FilterBuilder.Where(x =>
                    x.BaseDocumentId == newRow.Id && x.FileName == doc.MyFileData.FileName);
                if (queryHelper.Find(filter).Any())
                {
                    continue;
                }

                byte[] fileBytes;
                if (doc.MyFileData == null) continue;
                if (doc.MedicalInfoSupportingDocumentType == null) continue;


                try
                {
                    fileBytes = encryption.Decrypt<byte[]>(doc.MyFileData.Content);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }

                if (fileBytes == null || fileBytes.Length == 0) continue;


                var comments = doc.Comments ?? string.Empty;
                var fileName = doc.MyFileData.FileName;
                var documentDate = doc.DocumentDate ?? DateTime.Now;
                var documentType = doc.MedicalInfoSupportingDocumentType.Name.Replace("/", "-");
                var userId = myHrsUser.UserId;

                try
                {
                    var mongoContext = (Vulcan.Mongo.Base.Context.HrsContext) queryHelper.Context;
                    FileAttachmentsHrs.SaveFileAttachment(mongoContext, fileBytes, fileName, documentDate,
                        newRow,
                        "Incident Reporting Screen", documentType, userId, comments, newRow.OldHrsId, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Failed! FileName: {fileName} Size: {fileBytes.Length} DocType: {documentType}");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}

