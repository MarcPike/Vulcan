using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Config;

namespace DAL.Vulcan.Mongo.Models
{
    public class CoidConfigurationModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public string Coid { get; set; }
        public Configuration Configuration { get; set; }

        public CoidConfigurationModel()
        {

        }

        public CoidConfigurationModel(string application, string userId, CoidConfiguration config)
        {
            Id = config.Id.ToString();
            Coid = config.Coid;
            Configuration = config.Configuration;
        }

        public static CoidConfigurationModel GetForCoid(string application, string userId, string coid)
        {
            var rep = new RepositoryBase<CoidConfiguration>();
            var result = rep.AsQueryable().SingleOrDefault(x => x.Coid == coid);
            if (result == null)
            {
                result = new CoidConfiguration(coid, new Configuration());

                rep.Upsert(result);
            }

            return new CoidConfigurationModel(application, userId, result);
        }
    }
}