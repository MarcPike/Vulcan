using System.Collections.Generic;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperTraining
    {
        TrainingCourseDeActivateListModel GetTrainingCourseDeActivateListModel(HrsUserRef hrsUser);
        List<TrainingCourseDeActivateLog> DeActivateTrainingCourses(TrainingCourseDeActivateListModel model);
        TrainingCourseDeActivateLog DeActivateTrainingCourse(string trainingCourseId, HrsUserRef hrsUserRef);
        List<TrainingCourseDeActivateLog> GetDeactivatedTrainingCoursesHistory();
        List<TrainingCourseModel> GetTrainingCourses();
        List<TrainingCourseModel> GetTrainingCoursesForLocation(string locationId);

        List<TrainingEventGridModel> GetAllTrainingEvents(HrsUser hrsUser);
        List<TrainingEventModel> GetTrainingEventsForLocation(string locationId);

        List<EmployeeTrainingEventGridModel> GetAllTrainingEventsForEmployeesGrid(HrsUser hrsUser);
        //List<TrainingEventModel> GetAllTrainingEvents();
        List<TrainingEventModel> GetTrainingEventsForTrainingCourse(string trainingCourseId);
        List<TrainingEventModel> GetTrainingEventsForEmployee(string employeeId);
        TrainingCourseModel GetNewTrainingCourseModel();
        TrainingCourseModel SaveTrainingCourse(TrainingCourseModel model);
        TrainingEventModel GetNewTrainingEventModel();
        TrainingEventModel GetTrainingEvent(string trainingEventId);
        TrainingEventModel SaveTrainingEvent(TrainingEventModel model);
        void RemoveTrainingEvent(string trainingEventId);
        TrainingAttendeeModel GetNewTrainingAttendeeModel(string employeeId);

        List<PropertyValueModel> GetGroupClassifications(string entityId);
        List<PropertyValueModel> GetGroupCourseTypes(string entityId);
        List<PropertyValueModel> GetVenueTypes(string entityId);

        List<RequiredActivityModel> GetRequiredActivitiesForEmployee(string employeeId);

        List<PropertyValueModel> GetRequiredActivityTypes(string entityId);
        List<PropertyValueModel> GetRequiredActivityCompleteStatusTypes(string entityId);
        RequiredActivityModel GetRequiredActivityModel(string requiredActivityId);

        RequiredActivityModel GetNewRequiredActivityModelForEmployee(string employeeId);
        RequiredActivityModel SaveRequiredActivity(RequiredActivityModel model);
        void RemoveRequiredActivity(string requiredActivityId);
        List<TrainingEventSupportingDocumentNestedModel> GetTrainingEventSupportingDocumentsNested();
        List<TrainingEventSupportingDocumentFlatModel> GetTrainingEventSupportingDocumentsFlat();
        List<TrainingCourseRef> GetTrainingCourseReferences();
        List<TrainingCourseRef> GetTrainingCourseReferencesForLocation(string locationId);
        TrainingCourseModel GetTrainingCourse(string courseId);
        JobTitleCourseCopyModel GetModelForJobTitleCourseCopy(string sourceId, string targetId);
        void CopyJobTitleCourses(JobTitleCourseCopyModel model);
        JobTitleModel CopyAllJobTitleCourses(string sourceId, string targetId);
        List<TrainingEventAttendee>GetAllExpiringEvents(List<string> empIdList);
    }
}