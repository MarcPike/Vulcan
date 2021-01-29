using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Test;
using MongoDB.Bson;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.CompanyDefaults
{
    [TestFixture]
    public class RemoveDuplicates
    {
        private struct Keepers
        {
            public string CompanyId { get; set; }
            public ObjectId? CompanyDefaultId { get; set; }
        }

        private List<Keepers> _keepers = new List<Keepers>();

        private RepositoryBase<Mongo.DocClass.Companies.CompanyDefaults> _repository;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            _repository = new RepositoryBase<Mongo.DocClass.Companies.CompanyDefaults>();
        }

        [Test]
        public void Execute()
        {

            var uniqueCompanies = _repository.AsQueryable()
                .Select(x => x.CompanyId)
                .Distinct()
                .ToList();

            foreach (var uniqueCompany in uniqueCompanies)
            {
                _keepers.Add(new Keepers()
                {
                    CompanyId = uniqueCompany,
                    CompanyDefaultId = _repository.AsQueryable().First(x=>x.CompanyId == uniqueCompany).Id
                });
            }


            foreach (var keeper in _keepers)
            {
                var removeDefaults = _repository.AsQueryable()
                    .Where(x => x.CompanyId == keeper.CompanyId && x.Id != keeper.CompanyDefaultId).ToList();
                foreach (var removeDefault in removeDefaults)
                {
                    _repository.RemoveOne(removeDefault);
                    //Console.WriteLine(ObjectDumper.Dump(removeDefault));
                }
            }

        }
    }
}
