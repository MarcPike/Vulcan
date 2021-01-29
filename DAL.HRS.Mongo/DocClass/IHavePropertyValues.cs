using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass
{
    public interface IHavePropertyValues
    {
        void LoadPropertyValuesWithThisEntity(EntityRef entity);
    }
}