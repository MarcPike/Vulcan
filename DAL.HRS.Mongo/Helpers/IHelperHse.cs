using System.Collections.Generic;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperHse
    {
        List<HseObservationModel> GetAllHseObservations();

        List<BbsObserverRef> GetActiveObserverRefs();
        List<BbsObserverModel> GetObserverModels();
        BbsObserverModel SaveBbsObserver(BbsObserverModel model);
        void RemoveBbsObserver(string id);

        List<BbsDepartmentModel> GetAllBbsDepartmentModels();
        List<BbsTaskModel> GetAllBbsTaskModels();
        List<BbsObservationGridModel> GetBbsObservationGrid();
        List<BbsObservationModel> GetAllBbsObservationModels();
        BbsObservationModel GetBbsObservation(string id);
        BbsObservationItem GetNewObservationItem();
        List<BbsDepartmentRef> GetAllBbsDepartmentRefs();
        List<BbsTaskRef> GetAllBbsTaskRefs();

        List<BbsDepartmentSubCategoryRef> GetAllBbsDepartmentSubCategoryRefs(string departmentId);
        List<BbsTaskSubCategoryRef> GetAllBbsTaskSubCategoryRefs(string taskId);

        BbsDepartmentModel GetNewBbsDepartmentModel();
        BbsTaskModel GetNewBbsTaskModel();
        BbsObservationModel GetNewBbsObservationModel();

        BbsDepartmentModel SaveBbsDepartment(BbsDepartmentModel model);
        BbsTaskModel SaveBbsTask(BbsTaskModel model);
        BbsObservationModel SaveBbsObservation(BbsObservationModel model, HrsUserRef modifiedByUser);

        List<IncidentSeverityModel> GetIncidentSeverityByLocation();
        List<IncidentsYearToYearModel> GetIncidentsYearToYear();

        void RemoveBbsDepartment(string id);
        void RemoveBbsTask(string id);
        void RemoveBbsObservation(string id);

    }
}