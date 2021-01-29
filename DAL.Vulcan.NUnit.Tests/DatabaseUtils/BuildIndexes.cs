using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Indexes;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.DatabaseUtils
{
    [TestFixture]
    public class BuildIndexes
    {
        [Test]
        public void CreateAllIndexes()
        {
            var builder = new IndexBuilder();
            builder.Execute();
        }

        [Test]
        public void BuildQuoteIndexes()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var builder = new IndexBuilder();
            builder.BuildQuoteIndexes();
        }

        [Test]
        public void BuildCrmUserTokenIndexes()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var builder = new IndexBuilder();
            builder.BuildCrmUserTokenIndexes();
        }
    }
}
