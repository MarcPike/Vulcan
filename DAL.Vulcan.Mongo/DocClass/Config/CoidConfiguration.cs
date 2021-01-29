using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.DocClass.Config
{
    public class CoidConfiguration : BaseDocument
    {
        public string Coid { get; set; }
        public Configuration Configuration { get; set; } = new Configuration();

        public CoidConfiguration()
        {

        }

        public CoidConfiguration(string coid, Configuration configuration)
        {
            Coid = coid;
            Configuration = configuration;
        }
    }
}