using DAL.Vulcan.Mongo.Base.Core.Context;
using DAL.Vulcan.Mongo.Base.Core.DocClass;

namespace DAL.Vulcan.Mongo.Base.Core.Repository
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