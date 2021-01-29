using System.Collections.Generic;
using DAL.Common.DocClass;
using DAL.Common.Models;

namespace DAL.Common.Helper
{
    public interface IHelperCommon
    {
        List<EntityModel> GetEntities();
        List<LocationModel> GetLocations();
        List<PayrollRegionRef> GetPayrollRegionReferences();
        List<PayrollRegionModel> GetPayrollRegionModels();

        List<LocationTimeZoneRef> GetTimeZones();
        List<CountryStateModel> GetCountryStateList();
        CountryListModel GetCountryList();
        CountryListModel SaveCountryList(CountryListModel model);
        KronosLaborLevelModel GetKronosLaborLevels();
        KronosLaborLevelModel SaveKronosLaborLevels(KronosLaborLevelModel model);

        EntityModel SaveEntity(EntityModel model);
        LocationModel SaveLocation(LocationModel model);
        PayrollRegionModel SavePayrollRegion(PayrollRegionModel model);

    }
}