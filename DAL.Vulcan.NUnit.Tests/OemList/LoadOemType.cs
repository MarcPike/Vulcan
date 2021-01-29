using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using Microsoft.Exchange.WebServices.Autodiscover;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.OemList
{
    [TestFixture]
    public class LoadOemType
    {
        [Test]
        public void LoadOemTypesFromProductionIntoDevelopment()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var oemTypes = new RepositoryBase<OemType>().AsQueryable().Select(x=>x.Name).ToList();
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var rep = new RepositoryBase<OemType>();
            foreach (var oemType in oemTypes)
            {
                var oemFound = rep.AsQueryable().FirstOrDefault(x => x.Name == oemType);
                if (oemFound == null)
                {
                    oemFound = new OemType() {Name = oemType};
                    rep.Upsert(oemFound);
                    Console.WriteLine($"{oemType} was added.");
                }
            }
        }

        [Test]
        public void Execute()
        {
            var oemTypes = new List<string>()
            {
                "ACCESS ESP",
                "AKER",
                "ANADARKO",
                "ARCHER",
                "AWC FRAC VALVES",
                "AXON",
                "BASINTEK",
                "BHGE",
                "BLACKHAWK",
                "BMP",
                "CACTUS WELLHEAD",
                "CHEVRON",
                "CNPC",
                "D&L OIL TOOLS",
                "DAVIS",
                "DOWNHOLE AND DESIGN",
                "DOWNHOLE TECHNOLOGY",
                "Downing Wellhead",
                "DOWNING WELLHEAD EQUIP",
                "DRAW WORKS LP",
                "Dril-Quip",
                "DYNA-DRILL",
                "ELMAR",
                "ENCORE",
                "EOG RESOURCES",
                "EXPRO",
                "EXXON",
                "FAITH",
                "FLOWERVE",
                "TFMC",
                "TFMC-CONTRACT",
                "FREDERICK'S",
                "GE PRESSURE CONTROL",
                "GE VETCO GRAY",
                "GE WOOD GROUP",
                "GEODIAMOND",
                "GULF COAST MFG",
                "HAL",
                "HENWAY",
                "HESS",
                "HMC",
                "HUNTING ENERGY SERVICES",
                "IAL",
                "I-CORP,INC",
                "K&J",
                "KINETEC",
                "LORD CORPORATION",
                "M&J Manufacturing",
                "NATIONAL OILWELL",
                "OIL STATES",
                "PACKARD INTL",
                "PACKERS PLUS",
                "PGI",
                "SLB",
                "SEABOARD INTERNATIONAL INC",
                "SPERRY SUN",
                "STELLAR TECH",
                "SUPERIOR ENERGY SERVICES",
                "SYNTEX SUPER MATERIAL INC",
                "TIW CORPORATION",
                "TRYCO MACHINE WORKS INC.",
                "TURBO DRILL INDUSTRIES",
                "ULTERRA",
                "VANOIL COMPLETION SYSTEMS",
                "WEATHERFORD",
                "WENZEL",
                "WOM"
            };

            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<OemType>();

            foreach (var oemType in rep.AsQueryable().ToList())
            {
                rep.RemoveOne(oemType);
            }

            foreach (var oemType in oemTypes)
            {
                rep.Upsert(new OemType() {Name = oemType});
            }
        }

        [Test]
        public void AddForum()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<OemType>();

            var value = new OemType()
            {
                Name = "FORUM"
            };

            rep.Upsert(value);

        }

        [Test]
        public void AddTIC()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<OemType>();

            var value = new OemType()
            {
                Name = "TIC"
            };

            rep.Upsert(value);

        }


        [Test]
        public void AddSlbCameron()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<OemType>();

            var value = new OemType()
            {
                Name = "SLB CAMERON"
            };

            rep.Upsert(value);

        }

        [Test]
        public void AddUnknown()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var rep = new RepositoryBase<OemType>();

            var value = new OemType()
            {
                Name = "Unknown"
            };

            rep.Upsert(value);

        }

        [Test]
        public void GetAllOemTypes()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<OemType>();

            var oemTypes = rep.AsQueryable().Select(x => x.Name).OrderBy(x => x).ToList();
            foreach (var oemType in oemTypes)
            {
                Console.WriteLine(oemType);
            }

        }
        /*
         * 
                Andrew Clark
                1:39 AM (6 hours ago)
                to me

                Morning Marc,

                Apologies for the delay.

                From Sheffield they've requested...
                TFMC

                OSS/Cameron

                Aker

                GE

                Delta



                From Cumbernauld we would like...

                Plexus
                TFMC Plexus
                Tendeka
                Enpro

                Thank you!

                Cheers,
                Andy
         */
        [Test]
        public void AddOemTypesForUK()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<OemType>();
            var oemTypes = new List<string>()
            {
                "TFMC",
                "OSS/Cameron",
                "Aker",
                "GE",
                "Delta",
                "Plexus",
                "TFMC Plexus",
                "Tendeka",
                "Enpro"
            };

            foreach (var oemType in oemTypes)
            {
                var oemFound = rep.AsQueryable().SingleOrDefault(x => x.Name == oemType);
                if (oemFound == null)
                {
                    Console.WriteLine($"Added {oemType}");
                    rep.Upsert(new OemType()
                    {
                        Name = oemType
                    });
                }
            }
        }
    }
}
