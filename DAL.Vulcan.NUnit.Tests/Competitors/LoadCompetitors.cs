using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Competitors
{
    [TestFixture]
    public class LoadCompetitors
    {
        [Test]
        public void Execute()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var competitors = new List<string>()
            {
                "Energy Alloys",
                "Continental Alloys",
                "Castle Metals",
                "Specialty Quality Alloys",
                "Earle M Jorgensen",
                "Sigma Tube & Bar",
                "Oilfield Steel Supply",
                "Ryerson",
                "Fox Metals",
                "Hamilton Metals",
                "Sunbelt",
                "National Specially Alloys",
                "Tube Supply",
                "Voestalipne Specialty Metals",
                "Metaltech",
                "Best Stainless",
                "Thyssenkrupp Materials North America",
                "J&J Bar Plus",
                "Kreher Steel",
                "Houston Special Metals Inc",
                "Trident Metals",
                "Corrosions Materials",
                "Barret Steel",
                "Rolled Alloys",
                "TW Metals",
                "Double Eagle Alloys",
                "Ram Alloys",
                "Texas Honing",
                "Steel Alloys & Services",
                "O’Neal Steel",
                "CD Steel",
                "Bar Source",
                "Legacy Tubular",
                "Unknown"
            };

            var rep = new RepositoryBase<Competitor>();
            foreach (var competitorName in competitors)
            {
                var competitor = rep.AsQueryable().SingleOrDefault(x => x.Name == competitorName);
                if (competitor == null)
                {
                    competitor = new Competitor()
                    {
                        Name = competitorName
                    };
                    competitor.SaveToDatabase();
                    Console.WriteLine(competitor.Name+ " was added");
                }
            }
        }
    }
}
