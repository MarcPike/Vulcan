using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.Common.DocClass
{
    public static class InitializerLocation
    {
        public static void Initialize()
        {
            var rep = new CommonRepository<Location>();

            var defaultLocations = new List<Location>();

            var howco = DocClass.Entity.GetRefByName("Howco");
            var edgen = DocClass.Entity.GetRefByName("Edgen Murray");
            var unknown = DocClass.Entity.GetRefByName("<unknown>");

            defaultLocations.Add(new Location()
            {
                Office = "<unknown>",
                Entity = unknown
            });

            defaultLocations.Add(new Location()
            {
                Coid = "INC",
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Navigation",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "Scotland",
                Office = "Drummond",
                Phone = "+44 (0)1294 313131",
                Fax = "+44 (0)1294 313132",
                PhoneTollFree = "+1 800 392 7720",
                MapLocation = new MapLocation(4.6451552, 55.5982534),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "6 Drummond Crescent",
                        AddressLine2 = String.Empty,
                        City = "Irvine",
                        PostalCode = "KA11 SAN",
                        Country = "Scotland",
                        Type = AddressType.Primary,
                    }
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "INC",
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Kiefer",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "INC",
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Emmott Road",
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
                },
                Entity = howco

            });


            defaultLocations.Add(new Location()
            {
                Coid = "INC",
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Duncan",
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
                },
                Entity = howco

            });


            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "Scotland",
                Office = "Aberdeen",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "Scotland",
                Office = "Glasgow",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "Scotland",
                Office = "Cumbernauld",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "Scotland",
                Office = "Irvine",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "Norway",
                Office = "Norway",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "England",
                Office = "Sheffield",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "England",
                Office = "Bredbury",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "INC",
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Telge",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Gray",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "INC",
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Broussard",
                Phone = "+1 985 872 1297",
                Fax = "+1 985 872 1298",
                MapLocation = new MapLocation(991.962415, 30.1608519),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "<Unknown Address>",
                        AddressLine2 = String.Empty,
                        City = "Broussard",
                        StateProvince = "Louisiana",
                        PostalCode = "?????",
                        Country = "United States",
                        Type = AddressType.Primary,
                    }
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "INC",
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Lafayette",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "INC",
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Yorktown",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "INC",
                Branch = "USA",
                Region = "Western Hemisphere",
                Country = "United States",
                Office = "Broken Arrow",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "CAN",
                Branch = "CAN",
                Region = "Western Hemisphere",
                Country = "Canada",
                Office = "Edmonton",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "SIN",
                Branch = "SIN",
                Region = "Middle East Asia Pacific",
                Country = "Singapore",
                Office = "Singapore",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "CHI",
                Branch = "CHI",
                Region = "Middle East Asia Pacific",
                Country = "China",
                Office = "China",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "DUB",
                Branch = "DUB",
                Region = "Middle East Asia Pacific",
                Country = "UAE",
                Office = "Dubai",
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
                },
                Entity = howco

            });

            defaultLocations.Add(new Location()
            {
                Coid = "MSA",
                Branch = "MSA",
                Region = "Middle East Asia Pacific",
                Country = "Malaysia",
                Office = "Malaysia",
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
                },
                Entity = howco


            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "England",
                Office = "EM-Newbridge",
                Phone = "+44 131 333 3333",
                Fax = "+44 131 333 4477",
                MapLocation = new MapLocation(3.4193181, 55.9332385),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "Newbridge Industrial Estate",
                        //AddressLine2 = "",
                        City = "Newbridge",
                        Country = "United Kingdom",
                        PostalCode = "EH28 8PJ",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen

            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "England",
                Office = "EM-Stockton-on-Tees",
                Phone = "+44 1642 704 910",
                Fax = "+44 1642 704 918",
                MapLocation = new MapLocation(1.3215954, 54.5715185),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "Unit 12 & 13 Primrose Hill Industrial Estate, Orde Wingate Way",
                        //AddressLine2 = "",
                        City = "Stockton-on-Tees",
                        Country = "United Kingdom",
                        PostalCode = "TS19 0GA",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen

            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "France",
                Office = "EM-Paris",
                Phone = "+33 1 70 614050",
                Fax = "+33 1 47 224781",
                MapLocation = new MapLocation(1.3215954, 54.5715185),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "11 Rue Louis Philippe",
                        //AddressLine2 = "",
                        City = "Neuilly-sur-Seine",
                        Country = "France",
                        PostalCode = "92200",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen

            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "Norway",
                Office = "EM-Norway",
                Phone = "+47 51 69 52 00",
                Fax = "?",
                MapLocation = new MapLocation(5.5872373, 58.9147394),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "Energivegen 14",
                        //AddressLine2 = "",
                        City = "Tananger",
                        Country = "Norway",
                        PostalCode = "4056",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen

            });

            defaultLocations.Add(new Location()
            {
                Coid = "SIN",
                Branch = "SIN",
                Region = "Middle East Asia Pacific",
                Country = "Singapore",
                Office = "EM-Singapore",
                Phone = "+65 6223 3334",
                Fax = "+65 6223 3336",
                MapLocation = new MapLocation(103.6702683, 1.3130704),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "10 Gul Cir",
                        AddressLine2 = "",
                        City = "Singapore",
                        Country = "Singapore",
                        PostalCode = "629566",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });

            defaultLocations.Add(new Location()
            {
                Coid = "SIN",
                Branch = "SIN",
                Region = "Middle East Asia Pacific",
                Country = "Singapore",
                Office = "EM-Singapore Warehouse 2",
                Phone = "+65 6223 3334",
                Fax = "+65 6223 3336",
                MapLocation = new MapLocation(103.6702683, 1.3130704),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "Cogent 1 Logistics Hub",
                        AddressLine2 = "1 Buroh Cres",
                        City = "Singapore",
                        Country = "Singapore",
                        PostalCode = "627545",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });

            defaultLocations.Add(new Location()
            {
                Coid = "AUS",
                Branch = "AUS",
                Region = "Middle East Asia Pacific",
                Country = "Australia",
                Office = "EM-Brisbane",
                Phone = "+61 7 3257 2274",
                MapLocation = new MapLocation(153.0343753, 27.4598282),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "15C/421 Brunswick St",
                        AddressLine2 = "Fortitude Valley QLD",
                        City = "Brisbane",
                        Country = "Australia",
                        PostalCode = "4006",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });

            defaultLocations.Add(new Location()
            {
                Coid = "IND",
                Branch = "IND",
                Region = "Middle East Asia Pacific",
                Country = "Indonesia",
                Office = "EM-Jakarta",
                Phone = "+62 21 39502272",
                Fax = "+62 21 39502271",
                MapLocation = new MapLocation(106.7874854, 6.1747529),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "NEO SOHO Central Park",
                        AddressLine2 = "Residence Floor 23th unit 2312 Jalan Letjen S. Parman Kav 28. Tanjung Duren, Grogol petamburan",
                        City = "Jakarta",
                        Country = "Indonesia",
                        PostalCode = "11470",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });

            defaultLocations.Add(new Location()
            {
                Coid = "MSA",
                Branch = "MSA",
                Region = "Middle East Asia Pacific",
                Country = "Malaysia",
                Office = "EM-Malasia",
                Phone = "+60 362 114 024",
                Fax = "+60 362 114 026",
                MapLocation = new MapLocation(101.6538766, 3.1628497),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "60, Jalan Sri Hartamas 1",
                        AddressLine2 = "Taman Sri Hartamas",
                        City = "Kuala Lumpur",
                        Country = "Malaysia",
                        PostalCode = "50480",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });


            defaultLocations.Add(new Location()
            {
                Coid = "DUB",
                Branch = "DUB",
                Region = "Middle East Asia Pacific",
                Country = "UAE",
                Office = "EM-Dubai",
                Phone = "+971 4 8834486",
                Fax = "+971 4 8834507",
                MapLocation = new MapLocation(54.9827376, 24.96582),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "P.O. Box 61225",
                        City = "Jebel Ali",
                        Country = "UAE",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "England",
                Office = "HSP-Newbury",
                Phone = "+44 (0) 1635 201 329",
                MapLocation = new MapLocation(-1.2805799, 51.44399),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "Unit 4, Red Shute Hill Industrial Estate",
                        AddressLine2 = "Hermitage",
                        City = "Newbury",
                        Country = "United Kingdom",
                        PostalCode = "RG18 9QL",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "England",
                Office = "HSP-Stockton-on-Tees",
                Phone = "+44 (0) 1642 608 999",
                MapLocation = new MapLocation(-1.3216157, 54.5708781),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "Units 12 & 13, Primrose Hill Industrial Estate Orde Wingate Way",
                        City = "Stockton-on-Tees",
                        Country = "United Kingdom",
                        PostalCode = "TS19 0GA",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });

            defaultLocations.Add(new Location()
            {
                Coid = "EUR",
                Branch = "EUR",
                Region = "Europe",
                Country = "England",
                Office = "HSP-Aberdeen",
                Phone = "+44 (0) 1224 249 900",
                MapLocation = new MapLocation(-2.0681286, 57.119577),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "6 Minto Place",
                        AddressLine2 = "Altens",
                        City = "Aberdeen",
                        Country = "United Kingdom",
                        PostalCode = "AB12 3SN",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });

            defaultLocations.Add(new Location()
            {
                Coid = "DUB",
                Branch = "DUB",
                Region = "Middle East Asia Pacific",
                Country = "UAE",
                Office = "HSP-Dubai",
                Phone = "+971 4 883 4486",
                MapLocation = new MapLocation(54.9827376, 24.96582),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "P.O. Box 61225",
                        City = "Jebel Ali",
                        Country = "UAE",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });

            defaultLocations.Add(new Location()
            {
                Coid = "DUB",
                Branch = "DUB",
                Region = "Middle East Asia Pacific",
                Country = "UAE",
                Office = "HSP-Dubai",
                Phone = "+974 4 495 4609",
                MapLocation = new MapLocation(51.5283519, 25.3191478),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "P.O. Box 24863, Ground Floor, Al Mirqab Tower",
                        City = "Doha",
                        Country = "UAE",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });

            defaultLocations.Add(new Location()
            {
                Coid = "SIN",
                Branch = "SIN",
                Region = "Middle East Asia Pacific",
                Country = "Singapore",
                Office = "HSP-Singapore",
                Phone = "+65 6223 3334", // duplicate phone fax as EM-Singapore
                Fax = "+65 6223 3336",
                MapLocation = new MapLocation(103.6702683, 1.3130704),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "10 Gul Circle",
                        City = "Singapore",
                        Country = "Singapore",
                        PostalCode = "629566",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });

            defaultLocations.Add(new Location()
            {
                Coid = "MSA",
                Branch = "MSA",
                Region = "Middle East Asia Pacific",
                Country = "Malaysia",
                Office = "HSP-Malasia",
                Phone = "+60 362 114 024",
                Fax = "+60 362 114 026",
                MapLocation = new MapLocation(101.6538766, 3.1628497),
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        AddressLine1 = "60, Jalan Sri Hartamas 1",
                        AddressLine2 = "Taman Sri Hartamas",
                        City = "Kuala Lumpur",
                        Country = "Malaysia",
                        PostalCode = "50480",
                        Type = AddressType.Primary,
                    }
                },
                Entity = edgen
            });



            foreach (var defaultLocation in defaultLocations)
            {
                var locationFound = rep.AsQueryable().FirstOrDefault(x =>
                    x.Country == defaultLocation.Country && x.Office == defaultLocation.Office);

                // Correct Coid and Branch
                if (locationFound != null)
                {
                    locationFound.Coid = defaultLocation.Coid;
                    locationFound.Branch = defaultLocation.Branch;
                    rep.Upsert(locationFound);
                }
                else
                {
                    Console.WriteLine($"Added {defaultLocation.Office}");
                    rep.Upsert(defaultLocation);
                }
            }

        }

    }
}