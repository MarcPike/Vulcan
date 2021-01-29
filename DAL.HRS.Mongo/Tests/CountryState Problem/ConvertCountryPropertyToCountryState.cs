using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.HRS.Mongo.Tests.CountryState_Problem
{
    [TestFixture]
    public class ConvertCountryPropertyToCountryState
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void BadCountries()
        {
            var fb = Employee.Helper.FilterBuilder;
            var pb = Employee.Helper.ProjectionBuilder;
            var uniqueCountriesFromEmployee =
                Employee.Helper
                    .Find(x => x.Country != null).Project(x => new {CountryName = x.Country.Code}).ToList().Distinct();
            Console.WriteLine("Missing from my table");
            Console.WriteLine("=====================");
            foreach (var country in uniqueCountriesFromEmployee)
            {
                if (CountryState.Helper.Find(x => x.Country == country.CountryName).FirstOrDefault() == null)
                {
                    Console.WriteLine(country.CountryName);

                }
            }
            Console.WriteLine();
            Console.WriteLine("My countries include");
            Console.WriteLine("====================");

            var allCountriesFilter = CountryState.Helper.FilterBuilder.Empty;
            var myCountriesInclude = CountryState.Helper.Find(allCountriesFilter).Select(x => x.Country).ToList().Distinct();
            foreach (var country in myCountriesInclude)
            {
                Console.WriteLine(country);
            }

        }

        [Test]
        public void LoadFromExistingCountryState()
        {
            foreach (var countryState in CountryState.Helper.GetAll())
            {
                var country = CountryValue.Helper.Find(x => x.CountryName == countryState.Country).FirstOrDefault() ?? new CountryValue()
                {
                    CountryName = countryState.Country,
                    States = new List<StateValue>()
                };
                if (country.States.All(x => x.StateName != countryState.StateProvince))
                {
                    country.States.Add(new StateValue()
                    {
                        StateName = countryState.StateProvince
                    });
                }

                CountryValue.Helper.Upsert(country);
            }
        }

        [Test]
        public void BadCountryStates()
        {
            var badCountries = new List<string>()
            {
                "Australia",
                "Western Australia",
                "UAE",
                "India",
                "France",
                "Germany",
                "Malaysia",
                "Philippines",
                "Sri Lanka",
                "Pakistan",
                "Nepal"
            };

            foreach (var badCountry in badCountries)
            {
                Console.WriteLine($"{badCountry} list of States");
                var states = Employee.Helper
                    .Find(x => x.Country.Code == badCountry)
                    .Project(x => new {x.State}).ToList().Distinct();
                
                foreach (var state in states.Where(x=>!String.IsNullOrEmpty(x.State) && x.State != "Unspecified"))
                {
                    var country = badCountry;
                    if (country == "UAE")
                    {
                        country = "United Arab Emirates";
                    }

                    var countryValue = CountryValue.Helper.Find(x => x.CountryName == country).FirstOrDefault();
                    if (countryValue == null)
                    {
                        countryValue = new CountryValue()
                        {
                            CountryName = country,
                            States = new List<StateValue>()
                        };
                        Console.WriteLine($"Country: {country} (added)");

                    }

                    if (countryValue.States.All(x => x.StateName != state.State))
                    {
                        if ((state.State == "Ajman") && (country != "United Arab Emirates"))
                        {
                            Console.WriteLine($"Skipped [Adman] for {country}");
                        }
                        else
                        {
                            countryValue.States.Add(new StateValue()
                            {
                                StateName = state.State
                            });
                            Console.WriteLine($"\t\tCountry: {country} State: {state.State} (added)");
                        }
                    }

                    CountryValue.Helper.Upsert(countryValue);

                }
            }

        }

        [Test]
        public void LookForCountriesNotInCountryValue()
        {
            var existingCountries = CountryValue.Helper.GetAll().Select(x => x.CountryName).ToList();
            var uniqueCountryCodeValues = Employee.Helper.Find(x => x.Country != null)
                .Project(x => x.Country.Code).ToList().Distinct();
            foreach (var country in uniqueCountryCodeValues)
            {
                if (existingCountries.All(x => x != country))
                {
                    Console.WriteLine($"Missing Country: {country}");
                }
            }
        }

        [Test]
        public void AddMissingCountries()
        {
            var missingCountries = new List<string>()
            {
                "Australia",
                "France",
                "Germany",
                "Nepal"
            };

            foreach (var country in missingCountries)
            {
                CountryValue.Helper.Upsert(new CountryValue()
                {
                    CountryName = country
                });
            }
        }

        [Test]
        public void GetContactCompanyForAllEmployees()
        {
            //var missingContactCountry = Employee.Helper.Find(x => x.ContactCountry == string.Empty).ToList();
            //foreach (var employee in missingContactCountry)
            //{
            //    employee.GetDefaultContactCompany();
            //}


        }
    }
}
