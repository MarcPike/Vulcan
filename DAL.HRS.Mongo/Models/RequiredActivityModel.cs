using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using System;
using DAL.HRS.Mongo.DocClass.MedicalExams;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.HRS.Mongo.Models
{
    public class RequiredActivityModel 
    {
        RequiredActivityType type = RequiredActivityType.Unknown;
        RequiredActivityStatus requiredActivityStatus = RequiredActivityStatus.Unknown;
        

        public EmployeeRef Employee { get; set; }

        //[JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        //[BsonRepresentation(BsonType.String)]         // Mongo
        //public RequiredActivityType Type { get; set; } = RequiredActivityType.Unknown;

        public string Id { get; set; }

        //[JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        //[BsonRepresentation(BsonType.String)]         // Mongo
        //public RequiredActivityStatus Status { get; set; }

        public PropertyValueRef ActivityType { get; set; }
        public TrainingCourseRef TrainingCourse { get; set; }
        public string Description { get; set; }
        public DateTime? CompletionDeadline { get; set; }
        public DateTime? RevisedCompletionDeadline { get; set; }
        public DateTime? DateCompleted { get; set; }
        public PropertyValueRef CompleteStatus { get; set; }
        public Guid MedicalExamId { get; set; } 
        public Guid VerificationDocumentId { get; set; } = Guid.Empty;

        public string Comments { get; set; }
        public bool HasSupportingDocs { get; set; }
        public bool IsDirty { get; set; } = false;

        public int DaysRemaining
        {
            get
            {
                if ((CompletionDeadline != null) && (CompletionDeadline > DateTime.Now))
                {
                    return (int) (CompletionDeadline - DateTime.Now).Value.TotalDays;
                }
                else
                {
                    return 0;
                }
            }
        }


        [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        [BsonRepresentation(BsonType.String)]         // Mongo
        public RequiredActivityType Type
        {
            get
            {
                if (type == RequiredActivityType.Unknown && ActivityType != null)
                {
                    switch (ActivityType.Code)
                    {
                        case "Training":
                            type = RequiredActivityType.Training;
                            break;
                        case "90-Day Review":
                            type = RequiredActivityType.NinetyDayReview;
                            break;
                        case "Goals and Objectives":
                            type = RequiredActivityType.GoalsAndObjectives;
                            break;
                        case "On-the-Job Training":
                            type = RequiredActivityType.OnTheJobTraining;
                            break;
                        case "Other":
                            type = RequiredActivityType.Other;
                            break;
                        default:
                            break;

                    }

                }

                return type;
            }
            set
            {
                type = value;
            }
        }

        public string TypeForUI
        {
            get
            {
               
                    switch (Type)
                    {

                        case RequiredActivityType.Training:
                            return "Training";
                           
                        case RequiredActivityType.NinetyDayReview:
                            return "90-Day Review";
                            
                        case RequiredActivityType.GoalsAndObjectives:
                            return "Goals And Objectives";
                            
                        case RequiredActivityType.OnTheJobTraining:
                             return "On-the-Job Training";

                        case RequiredActivityType.Other:
                            return "Other";
                            

                        default:
                            break;

                    }



                return Type.ToString();
            }
            
        }

        [JsonConverter(typeof(StringEnumConverter))]  // JSON.Net
        [BsonRepresentation(BsonType.String)]         // Mongo
        public RequiredActivityStatus Status
        {
            get
            {
                if (requiredActivityStatus == RequiredActivityStatus.Unknown && CompleteStatus != null)
                {
                    switch (CompleteStatus.Code)
                    {
                        case "Complete":
                            requiredActivityStatus = RequiredActivityStatus.Completed;
                            break;
                        case "Follow-Up Required":
                            requiredActivityStatus = RequiredActivityStatus.Pending;
                            break;
                        case "Incomplete":
                            requiredActivityStatus = RequiredActivityStatus.Incomplete;
                            break;
                        case "Unknown":
                            requiredActivityStatus = RequiredActivityStatus.Unknown;
                            break;
                        case "Dismissed":
                            requiredActivityStatus = RequiredActivityStatus.Dismissed;
                            break;

                        default:
                            break;

                    }

                }

                return requiredActivityStatus;
            }
            set
            {
                requiredActivityStatus = value;
            }
        }


        public RequiredActivityModel()
        {
        }

        public RequiredActivityModel(RequiredActivity r)
        {
            Employee = r.Employee;
            Id = r.Id.ToString();
            ActivityType = r.ActivityType;
            TrainingCourse = r.TrainingCourse;
            Description = r.Description;
            CompletionDeadline = r.CompletionDeadline;
            RevisedCompletionDeadline = r.RevisedCompletionDeadline;
            CompleteStatus = r.CompleteStatus;
            Comments = r.Comments;
            HasSupportingDocs = r.HasSupportingDocs;
            DateCompleted = r.DateCompleted;

            MedicalExamId = r.MedicalExamId;

            VerificationDocumentId = r.VerificationDocumentId;

            Status = r.Status;
            Type = r.Type;
        }

    }
}
