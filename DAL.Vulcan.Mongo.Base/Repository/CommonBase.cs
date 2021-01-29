using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.Base.Repository
{
    public class CommonRepository<TBaseDocument> : RepositoryBase<TBaseDocument>
        where TBaseDocument : BaseDocument
    {
        public CommonRepository()
        {
            _context = new CommonContext();
        }
    }
}