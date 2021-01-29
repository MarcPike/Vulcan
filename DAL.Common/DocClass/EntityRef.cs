using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Common.DocClass
{
    public class EntityRef : ReferenceObject<Entity>
    {
        public string Name { get; set; }

        public EntityRef() { }

        public EntityRef(Entity e) : base(e)
        {
            Name = e.Name;
        }

        public Entity AsEntity()
        {
            return ToBaseDocument();
        }

    }
}