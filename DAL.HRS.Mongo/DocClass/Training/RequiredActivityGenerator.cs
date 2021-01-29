using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.MedicalExams;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using EmployeeMedicalExam = DAL.HRS.Mongo.DocClass.MedicalExams.EmployeeMedicalExam;


namespace DAL.HRS.Mongo.DocClass.Training
{
   public class RequiredActivityGenerator
    {
        private MongoRawQueryHelper<RequiredActivity> _queryHelper = new MongoRawQueryHelper<RequiredActivity>();
        private List<PropertyValueRef> _requiredActivityCompleteStatus = new List<PropertyValueRef>();

        public RequiredActivityGenerator()
        {
            InitializeCompleteStatusProperty();
        }

        private void InitializeCompleteStatusProperty()
        {
            var queryHelper = new MongoRawQueryHelper<PropertyValue>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Type == "RequiredActivityCompleteStatus");
            _requiredActivityCompleteStatus = queryHelper.Find(filter).ToList().Select(x => x.AsPropertyValueRef()).ToList();
        }

        public RequiredActivity ForTraining(Employee.Employee employee, TrainingEvent trainingEvent)
        {
            var type = RequiredActivityType.Training;
            var course = trainingEvent.TrainingCourse;
            if (trainingEvent.TrainingCourse.AsTrainingCourse().Certification != null)
            {
                type = RequiredActivityType.TrainingCertification;
            }

            var entityId = employee.Entity.Id;
            var completeStatus = GetCompletionStatus(trainingEvent.EndDate, entityId);

            var ra = new RequiredActivity()
            {
                Type = type,
                Employee = employee.AsEmployeeRef(),
                CompletionDeadline = trainingEvent.EndDate,
                TrainingCourse = course,
                CompleteStatus = completeStatus
            };
            _queryHelper.Upsert(ra);

            return ra;
        }

        public RequiredActivity ForMedicalExam(Employee.Employee employee, EmployeeMedicalExam exam)
        {
            var entityId = employee.Entity.Id;
            var completeStatus = GetCompletionStatus(exam.Completed, entityId);

            var ra = new RequiredActivity()
            {
                Type = RequiredActivityType.MedicalExam,
                Employee = employee.AsEmployeeRef(),
                CompletionDeadline = exam.DueDate,
                DateCompleted = exam.Completed,
                MedicalExamId = exam.Id,
                CompleteStatus = completeStatus
            };
            _queryHelper.Upsert(ra);

            exam.RequiredActivity = ra.AsRequiredActivityRef();
            return ra;

        }

        public RequiredActivity ForVerificationDocument(Employee.Employee employee, EmployeeVerification document, DateTime dueDate)
        {
            var entityId = employee.Entity.Id;
            var completeStatus = GetCompletionStatus(dueDate, entityId);

            var ra = new RequiredActivity()
            {
                Type = RequiredActivityType.VerificationDocument,
                Employee = employee.AsEmployeeRef(),
                CompletionDeadline = dueDate,
                VerificationDocumentId = document.Id,
                CompleteStatus = completeStatus
            };
            _queryHelper.Upsert(ra);

            document.RequiredActivity = ra.AsRequiredActivityRef();

            return ra;


        }

        private PropertyValueRef GetCompletionStatus(DateTime? endDate, string entityId)
        {
            var completeStatus = _requiredActivityCompleteStatus.First(x => x.Code == "Incomplete" && x.Entity.Id == entityId);
            if (endDate != null && endDate <= DateTime.Now)
            {
                completeStatus = _requiredActivityCompleteStatus.First(x => x.Code == "Complete");
            }

            return completeStatus;
        }

    }
}
