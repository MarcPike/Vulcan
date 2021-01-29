using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using System.Linq;

namespace DAL.HRS.Import.ImportHse
{
    public class HseHelperMethods
    {
        public LocationRef GetLocationForHrsLocation(string locationValueFromHrs)
        {
            if (locationValueFromHrs == "Canada")
            {
                locationValueFromHrs = "Edmonton";
            }

            if (locationValueFromHrs == "Emmott")
            {
                locationValueFromHrs = "Emmott Road";
            }

            if ((locationValueFromHrs == "Lafayette/South Bernard") || (locationValueFromHrs == "Lafayette (Deleted)"))
            {
                locationValueFromHrs = "Lafayette";
            }

            if (locationValueFromHrs == "Houston")
            {
                locationValueFromHrs = "Telge";
            }

            locationValueFromHrs = locationValueFromHrs.TrimEnd();
            var rep = new RepositoryBase<Location>();
            var location = rep.AsQueryable().FirstOrDefault(x => x.Office == locationValueFromHrs);
            if (location == null)
            {
                var payrollRegion =
                    new RepositoryBase<PayrollRegion>().Upsert(new PayrollRegion()
                    {
                        Name = "Unknown - " + locationValueFromHrs
                    });

                location = new Location()
                {
                    Branch = "???",
                    Country = "???",
                    Office = locationValueFromHrs,
                    Region = "???",
                };
                location.PayrollRegions.Add(payrollRegion.AsPayrollRegionRef());
                rep.Upsert(location);

                //throw new Exception($"No Location found for [{locationValueFromHrs}]");
            }

            return location.AsLocationRef();
        }

    }
}
