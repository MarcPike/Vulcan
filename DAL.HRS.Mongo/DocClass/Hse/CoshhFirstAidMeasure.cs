using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using System.Linq;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class CoshhFirstAidMeasure : BaseDocument
    {
        public string Action { get; set; }
        public PropertyValueRef ExposureType { get; set; }

        public CoshhFirstAidMeasureRef AsCoshhFirstAidMeasureRef()
        {
            return new CoshhFirstAidMeasureRef(this);
        }

        public static CoshhFirstAidMeasureRef CreateAndReturnRef(RepositoryBase<CoshhFirstAidMeasure> rep, string action, PropertyValueRef exposureType)
        {
            var firstAidMeasure = rep.AsQueryable().FirstOrDefault(x => x.Action == action && x.ExposureType.Code == exposureType.Code);
            if (firstAidMeasure == null)
            {
                firstAidMeasure = new CoshhFirstAidMeasure()
                {
                    Action = action,
                    ExposureType = exposureType
                };
                rep.Upsert(firstAidMeasure);
            }

            return firstAidMeasure.AsCoshhFirstAidMeasureRef();
        }

    }

    public class CoshhFirstAidMeasureRef : ReferenceObject<CoshhFirstAidMeasure>
    {
        public string Action { get; set; }
        public PropertyValueRef ExposureType { get; set; }

        public CoshhFirstAidMeasureRef()
        {
        }

        public CoshhFirstAidMeasureRef(CoshhFirstAidMeasure meas) : base(meas)
        {
            Action = meas.Action;
            ExposureType = meas.ExposureType;
        }

        public CoshhFirstAidMeasure AsCoshhFirstAidMeasure()
        {
            return ToBaseDocument();
        }
    }
}
