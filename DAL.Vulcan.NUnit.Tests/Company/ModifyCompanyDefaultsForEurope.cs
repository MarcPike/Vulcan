using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Company
{
    //[TestFixture()]
    //public class ModifyCompanyDefaultsForEurope
    //{
    //    [Test]
    //    public void Execute()
    //    {
    //        EnvironmentSettings.CurrentEnvironment = Environment.Development;

    //        var rep = new RepositoryBase<Mongo.DocClass.Companies.CompanyDefaults>();
    //        var companyDefaultsForEUR = 
    //            rep.AsQueryable().Where(x => x.Coid == "EUR" && x.CustomerUom == CustomerUom.Inches).ToList();
    //        foreach (var companyDefault in companyDefaultsForEUR)
    //        {
    //            companyDefault.CustomerUom = CustomerUom.PerPiece;
    //            rep.Upsert(companyDefault);
    //        }
    //    }

    //    [Test]
    //    public void RemoveDuplicates()
    //    {
    //        EnvironmentSettings.CurrentEnvironment = Environment.Development;

    //        var rep = new RepositoryBase<CompanyDefaults>();
    //        var defaultsWithNoCoid = rep.AsQueryable().Where(x => x.Coid == null).ToList();
    //        foreach (var companyDefault in defaultsWithNoCoid)
    //        {
    //            rep.RemoveOne(companyDefault);
    //        }

    //        var companyDefaults = rep.AsQueryable().ToList();
    //        foreach (var companyDefault in companyDefaults)
    //        {
    //            var coid = companyDefault.Coid;



    //        }

    //    }

    //}
}
