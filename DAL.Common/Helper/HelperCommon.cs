using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.Common.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;

namespace DAL.Common.Helper
{
    public class HelperCommon : IHelperCommon
    {
        private CommonMongoRawQueryHelper<Entity> _queryEntities = new CommonMongoRawQueryHelper<Entity>();
        private CommonMongoRawQueryHelper<Location> _queryLocation = new CommonMongoRawQueryHelper<Location>();

        public List<EntityModel> GetEntities()
        {
            var filter = _queryEntities.FilterBuilder.Empty;
            var project = _queryEntities.ProjectionBuilder.Expression(x => new EntityModel(x));
            return _queryEntities.FindWithProjection(filter, project);
        }

        public List<LocationModel> GetLocations()
        {
            var filter = _queryLocation.FilterBuilder.Empty;
            var project = _queryLocation.ProjectionBuilder.Expression(x => new LocationModel(x));
            return _queryLocation.FindWithProjection(filter, project);
        }

        public List<PayrollRegionModel> GetPayrollRegionModels()
        {
            var payrollRegions = PayrollRegion.Helper.GetAll().OrderBy(x => x.Name);
            return payrollRegions.Select(x => new PayrollRegionModel(x)).ToList();
        }

        public List<PayrollRegionRef> GetPayrollRegionReferences()
        {
            var payrollRegions = PayrollRegion.Helper.GetAll().OrderBy(x => x.Name);
            return payrollRegions.Select(x => x.AsPayrollRegionRef()).ToList();
        }

        public List<LocationTimeZoneRef> GetTimeZones()
        {
            var timeZones = LocationTimeZone.Helper.GetAll();
            return timeZones.Select(x => new LocationTimeZoneRef(x)).ToList();
        }

        public List<CountryStateModel> GetCountryStateList()
        {
            return CountryStateModel.GetCountryStateModel();
        }

        public CountryListModel GetCountryList()
        {
            return CountryListModel.GetModel();
        }

        public CountryListModel SaveCountryList(CountryListModel model)
        {
            return CountryListModel.SaveModel(model);
        }


        public KronosLaborLevelModel GetKronosLaborLevels()
        {
            var filter = KronosLabelLevel.Helper.FilterBuilder.Empty;
            var kronosLaborLevel = KronosLabelLevel.Helper.Find(filter).FirstOrDefault();
            if (kronosLaborLevel == null)
            {
                kronosLaborLevel = new KronosLabelLevel();
                KronosLabelLevel.Helper.Upsert(kronosLaborLevel);
            }

            return new KronosLaborLevelModel()
            {
                Values = kronosLaborLevel.Values
            };
        }

        public KronosLaborLevelModel SaveKronosLaborLevels(KronosLaborLevelModel model)
        {
            var filter = KronosLabelLevel.Helper.FilterBuilder.Empty;
            var kronosLaborLevel = KronosLabelLevel.Helper.Find(filter).FirstOrDefault();
            if (kronosLaborLevel == null)
            {
                kronosLaborLevel = new KronosLabelLevel();
            }

            kronosLaborLevel.Values = model.Values;
            KronosLabelLevel.Helper.Upsert(kronosLaborLevel);

            return new KronosLaborLevelModel()
            {
                Values = kronosLaborLevel.Values
            };
        }

        public EntityModel SaveEntity(EntityModel model)
        {
            var entity = _queryEntities.FindById(model.Id);
            if (entity == null)
            {
                entity = new Entity()
                {
                    Id = ObjectId.Parse(model.Id)
                };
                entity.Name = model.Name;
                entity.Locations = model.Locations;
            }

            _queryEntities.Upsert(entity);
            return new EntityModel(entity);
        }

        public LocationModel SaveLocation(LocationModel model)
        {
            var location = _queryLocation.FindById(model.Id);
            if (location == null)
            {
                location = new Location()
                {
                    Id = ObjectId.Parse(model.Id)
                };
                location.Coid = model.Coid;
                location.Branch = model.Branch;
                location.Region = model.Region;
                location.Country = model.Country;
                location.Office = model.Office;
                location.Phone = model.Phone;
                location.Fax = model.Fax;
                location.PhoneTollFree = model.PhoneTollFree;
                location.Addresses = model.Addresses;
                location.MapLocation = model.MapLocation;
                location.Entity = model.Entity;
                location.KronosLaborLevel = model.KronosLaborLevel;
                location.TimeZone = model.TimeZone.AsLocationTimeZone().AsLocationTimeZoneRef();
                location.Locale = model.Locale;
            }
            else
            {
                location.KronosLaborLevel = model.KronosLaborLevel;
                location.TimeZone = model.TimeZone.AsLocationTimeZone().AsLocationTimeZoneRef();
                location.Locale = model.Locale;
                location.DefaultCurrency = model.DefaultCurrency;
            }

            _queryLocation.Upsert(location);
            return new LocationModel(location);
        }

        public PayrollRegionModel SavePayrollRegion(PayrollRegionModel model)
        {
            var payrollRegion = PayrollRegion.Helper.FindById(model.Id);
            if (payrollRegion == null) throw new Exception("Payroll region not found");

            payrollRegion.Name = model.Name;
            PayrollRegion.Helper.Upsert(payrollRegion);

            return new PayrollRegionModel(payrollRegion);
        }
    }
}
