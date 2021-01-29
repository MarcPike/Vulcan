using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.QNG;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.QNG_Export
{
    [TestFixture]
    public class AddQngExportLogRow
    {
        [Test]
        public void Execute()
        {
            EnvironmentSettings.Database = MongoDatabase.VulcanCrm;
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            
            var rep = new RepositoryBase<QngExportLog>();
            var row = new QngExportLog()
            {
                ExportTime = DateTime.Now.AddMinutes(-90),
                Finished = DateTime.Now.AddMinutes(-30),
                RowsExported = 34156+544,
                RowsSkipped = 0,
            };
            rep.Upsert(row);
        }
    }
}
