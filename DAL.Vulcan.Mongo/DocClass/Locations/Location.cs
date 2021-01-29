using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Currency;

namespace DAL.Vulcan.Mongo.DocClass.Locations
{
    public class Location: BaseDocument
    {
        public static MongoRawQueryHelper<Location> Helper = new MongoRawQueryHelper<Location>();
        public string Branch { get; set; } // eur usa
        public string Region { get; set; } // meap, Europe, Western
        public string Country { get; set; } // country UAE, Scotland, USA, Norway
        public string Office { get; set; } // Telge
        public CurrencyType DefaultCurrency { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string PhoneTollFree { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public ReferenceList<Team,TeamRef> Teams { get; set; } = new ReferenceList<Team, TeamRef>();

        public MapLocation MapLocation { get; set; }

        public string GetCoid()
        {
            return Branch == "USA" ? "INC" : Branch;
        }

        public static void GenerateDefaults()
        {
            var rep = new RepositoryBase<Location>();
            var currencyRep = new RepositoryBase<CurrencyType>();

            var defaultLocations = new List<Location>();

            defaultLocations.Add(new Location()
            {
                Branch = "EUR",
                Region = "Europe",
                Country = "Norway",
                Office = "Norway",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "NOK"),
                Phone = "+47 51 97 17 70",
                Fax = "+47 51 97 17 71",
                MapLocation = new MapLocation(5.828566, 58.780690),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "Fladene 7",
                        AddressLine2 = "4332 Figgjo, Norway",
                        City = "Figgjo",
                        PostalCode = "4332",
                        Country = "Norway",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Kiefer",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "USD"),
                Phone = "+1 918 259 2143",
                Fax = "+1 918 259 2084",
                PhoneTollFree = "+1 800 392 7720",
                MapLocation = new MapLocation(96.0488903, 35.9467664),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "14963 S 49th W Ave",
                        AddressLine2 = String.Empty,
                        City = "Kiefer",
                        StateProvince = "OK",
                        PostalCode = "74041",
                        Country = "United States",
                        Type = AddressType.Primary,
                    }
                }

            });



            defaultLocations.Add(new Location()
            {
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Navigation",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "USD"),
                Phone = "+1 281 649 8800",
                Fax = "+1 281 649 8903",
                PhoneTollFree = "+1 800 392 7720",
                MapLocation = new MapLocation(95.315555, 29.7481755),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "6023 Navigation Blvd",
                        AddressLine2 = String.Empty,
                        City = "Houston",
                        StateProvince = "Texas",
                        PostalCode = "77011",
                        Country = "United States",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Emmott Road",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "USD"),
                Phone = "+1 281 649 8800",
                Fax = "+1 281 649 8903",
                PhoneTollFree = "+1 800 392 7720",
                MapLocation = new MapLocation(95.5378828, 29.875064),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "9100 Emmott Road",
                        AddressLine2 = String.Empty,
                        City = "Houston",
                        StateProvince = "Texas",
                        PostalCode = "77040",
                        Country = "United States",
                        Type = AddressType.Primary,
                    }
                }

            });


            defaultLocations.Add(new Location()
            {
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Duncan",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "USD"),
                Phone = "+1 580 251 4614",
                Fax = "",
                PhoneTollFree = "",
                MapLocation = new MapLocation(97.9444753, 34.4876073),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "100 East Halliburton Blvd",
                        AddressLine2 = String.Empty,
                        City = "Duncan",
                        StateProvince = "Oklahoma",
                        PostalCode = "73536",
                        Country = "United States",
                        Type = AddressType.Primary,
                    }
                }

            });


            defaultLocations.Add(new Location()
            {
                Branch = "EUR",
                Region = "Europe",
                Country = "Scotland",
                Office = "Aberdeen",
                DefaultCurrency = currencyRep.AsQueryable().First(x=>x.Code == "GBP"),
                Phone = "+44 (0)1236 454111",
                Fax = "+44 (0)1236 454222",
                MapLocation = new MapLocation(2.099075, 57.149651),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "PO Box 10154",
                        AddressLine2 = "Woodside Rd, Bridge of Don",
                        City = "Aberdeen",
                        PostalCode = "AB23 8FR",
                        Country = "Scotland",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "EUR",
                Region = "Europe",
                Country = "Scotland",
                Office = "Glasgow",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "GBP"),
                Phone = "+44(0) 141 353 7800",
                Fax = "+44(0) 141 353 7801",
                MapLocation = new MapLocation(4.2518, 55.8642), 
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "2nd Floor Fountain House",
                        AddressLine2 = "1-3 Woodside Crescent",
                        City = "Glasgow",
                        PostalCode = "G3 7UL",
                        Country = "Scotland",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "EUR",
                Region = "Europe",
                Country = "Scotland",
                Office = "Cumbernauld",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "GBP"),
                Phone = "+44 (0)1236 454111",
                Fax = "+44 (0)1236 454222",
                MapLocation = new MapLocation(3.9925, 55.9457),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "3 Blairlinn Road",
                        AddressLine2 = String.Empty,
                        City = "Cumbernauld",
                        PostalCode = "G67 2TF",
                        Country = "Scotland",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "EUR",
                Region = "Europe",
                Country = "Scotland",
                Office = "Irvine",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "GBP"),
                Phone = "+44 (0)1294 313131",
                Fax = "+44 (0)1294 313132",
                MapLocation = new MapLocation(4.6696, 55.6116),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "Drybridge Park, Shewalton Road",
                        AddressLine2 = String.Empty,
                        City = "Irvine",
                        PostalCode = "KA11 5AL",
                        Country = "Scotland",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "EUR",
                Region = "Europe",
                Country = "Norway",
                Office = "Norway",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "GBP"),
                Phone = "+47 51 97 17 70",
                Fax = "+47 51 97 17 71",
                MapLocation = new MapLocation(5.8154, 58.7857),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "Fladene 7",
                        AddressLine2 = String.Empty,
                        City = "Figgjo",
                        PostalCode = "4332",
                        Country = "Norway",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "EUR",
                Region = "Europe",
                Country = "England",
                Office = "Sheffield",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "GBP"),
                Phone = "+44(0) 114 244 6711",
                Fax = "+44(0) 114 244 7469",
                MapLocation = new MapLocation(1.4701, 53.3811),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "211 Carbrook Street",
                        AddressLine2 = String.Empty,
                        City = "Sheffield",
                        PostalCode = "S9 2JN",
                        Country = "England",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "EUR",
                Region = "Europe",
                Country = "England",
                Office = "Bredbury",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "GBP"),
                Phone = "+44 (0) 161 430 3173",
                Fax = "+44 (0) 161 430 8643",
                MapLocation = new MapLocation(2.1138, 53.4236),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "Cromwell Road",
                        AddressLine2 = "SK6 2RH",
                        City = "Stockport",
                        PostalCode = "SK6 2RH",
                        Country = "England",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Telge",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "USD"),
                Phone = "+1 281 649 8800",
                Fax = "+1 281 649 8903",
                PhoneTollFree = "+1 800 392 7720",
                MapLocation = new MapLocation(95.3698, 29.7604),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "9611 Telge Road",
                        AddressLine2 = String.Empty,
                        City = "Houston",
                        StateProvince = "Texas",
                        PostalCode = "77095",
                        Country = "United States",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Gray",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "USD"),
                Phone = "+1 985 872 1297",
                Fax = "+1 985 872 1298",
                MapLocation = new MapLocation(90.7865, 29.6977),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "3639 West Park Ave.",
                        AddressLine2 = String.Empty,
                        City = "Gray",
                        StateProvince = "Louisiana",
                        PostalCode = "70359",
                        Country = "United States",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Lafayette",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "USD"),
                Phone = "+1 985 746 4192",
                Fax = "+1 337 364 2479",
                MapLocation = new MapLocation(92.0198, 30.2241),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "218 Rue Beauregard Suite G",
                        AddressLine2 = String.Empty,
                        City = "Lafayette",
                        StateProvince = "Louisiana",
                        PostalCode = "70508",
                        Country = "United States",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Yorktown",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "USD"),
                Phone = "+1 985 746 4192",
                Fax = "+1 337 364 2479",
                MapLocation = new MapLocation(97.5028, 28.9811),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "218 Rue Beauregard Suite G",
                        AddressLine2 = String.Empty,
                        City = "Lafayette",
                        StateProvince = "Louisiana",
                        PostalCode = "70508",
                        Country = "United States",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Broken Arrow",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "USD"),
                Phone = "+1 918 259 2143",
                Fax = "+1 918 259 2084",
                MapLocation = new MapLocation(95.7975, 36.0609),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "3000A North Hemlock Circle",
                        AddressLine2 = String.Empty,
                        City = "Broken Arrow",
                        StateProvince = "Oklahoma",
                        PostalCode = "74012",
                        Country = "United States",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "Canada",
                Office = "Edmonton",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "CAD"),
                Phone = "+1 780 439 6746",
                Fax = "+1 780 439 6807",
                MapLocation = new MapLocation(113.4909, 53.5444),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "7504 – 52 Street",
                        AddressLine2 = String.Empty,
                        City = "Edmonton",
                        StateProvince = "Edmonton",
                        PostalCode = "T6B 2G3",
                        Country = "Canada",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "SIN",
                Region = "Middle East Asia Pacific",
                Country = "Singapore",
                Office = "Singapore",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "USD"),
                Phone = "+65 6861 3885",
                Fax = "+65 6861 4827",
                MapLocation = new MapLocation(371296.71750706, 142760.53087404),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "21 Tuas South Street 5",
                        AddressLine2 = String.Empty,
                        PostalCode = "637384",
                        Country = "Singapore",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "CHI",
                Region = "Middle East Asia Pacific",
                Country = "China",
                Office = "China",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "CNY"),
                Phone = "+86 21 5786 8003/8278/6856",
                Fax = "+86 21 5786 8308",
                MapLocation = new MapLocation(121.4737, 31.2304),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "888 Zhongqiang Rd,Maogang Industrial Park",
                        AddressLine2 = "Songjiang District",
                        PostalCode = "201607, P.R.China",
                        City = "Shanghai City",
                        Country = "China",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "DUB",
                Region = "Middle East Asia Pacific",
                Country = "UAE",
                Office = "Dubai",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "USD"),
                Phone = "+971 (0)6 5139500",
                Fax = "+971 (0)6 5139555",
                MapLocation = new MapLocation(55.4209, 25.3463),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "PO Box 50273",
                        AddressLine2 = "Hamriyah Free Zone",
                        City = "Sharjah",
                        Country = "UAE",
                        Type = AddressType.Primary,
                    }
                }

            });

            defaultLocations.Add(new Location()
            {
                Branch = "MSA",
                Region = "Middle East Asia Pacific",
                Country = "Malaysia",
                Office = "Malaysia",
                DefaultCurrency = currencyRep.AsQueryable().First(x => x.Code == "USD"),
                Phone = "+65 6861 3885",
                MapLocation = new MapLocation(103.7618, 1.4854),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "No12, Jalan Laman Setia 7/5, Laman",
                        AddressLine2 = "Setia, 81550 Johor Bahru",
                        City = "Johor Darul Takzim",
                        Country = "Malaysia",
                        Type = AddressType.Primary,
                    }
                }

            });

            foreach (var defaultLocation in defaultLocations)
            {
                if (
                    !rep.AsQueryable()
                        .Any(
                            x =>
                                x.Country == defaultLocation.Country &&
                                x.Office == defaultLocation.Office))
                {
                    Console.WriteLine($"Added {defaultLocation.Office}");
                    rep.Upsert(defaultLocation);
                }
            }

        }

        public LocationRef AsLocationRef()
        {
            return new LocationRef(this);
        }
    }
}
