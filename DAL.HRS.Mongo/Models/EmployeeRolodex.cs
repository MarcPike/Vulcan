using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.Models
{

    public class EmployeeRolodex
    {
        public MongoRawQueryHelper<Employee> QueryHelper { get; set; } = new MongoRawQueryHelper<Employee>();
        public List<EmployeeGroup> Rolodex = new List<EmployeeGroup>()
        {
            new EmployeeGroup() {Label = "ABC"},
            new EmployeeGroup() {Label = "DEF"},
            new EmployeeGroup() {Label = "GHI"},
            new EmployeeGroup() {Label = "JKL"},
            new EmployeeGroup() {Label = "MNO"},
            new EmployeeGroup() {Label = "PQRS"},
            new EmployeeGroup() {Label = "TUV"},
            new EmployeeGroup() {Label = "WXYZ"}
        };

        public FilterDefinition<Employee> Filter { get; set; }

        public EmployeeRolodex()
        {
            QueryHelper = new MongoRawQueryHelper<Employee>();
            Filter = QueryHelper.FilterBuilder.Empty;
        }

        public async Task FetchResults()
        {

            foreach (var employeeGroup in Rolodex)
            {
                await employeeGroup.PerformQuery(Filter);
            }


        }
    }

    public class EmployeeGroup
    {
        public string Label { get; set; }
        public List<EmployeeRef> Employees { get; set; } = new List<EmployeeRef>();
        public int Count => Employees.Count;

        public Task PerformQuery(FilterDefinition<Employee> baseMatch)
        {
            var queryHelper = new MongoRawQueryHelper<Employee>();
            List<Task> taskList = new List<Task>();
            var allChars = Label.ToCharArray();
            //var rangeMin = allChars[0].ToString();
            //var rangeMax = allChars[allChars.Length - 1].ToString();

            //return  QueryRange(baseMatch, queryHelper, rangeMin, rangeMax);
            foreach (var c in Label.ToCharArray())
            {
                var task = QueryLetter(baseMatch, queryHelper, c);
                taskList.Add(task);
            }

            return Task.WhenAll(taskList);
        }

        private Task QueryLetter(FilterDefinition<Employee> baseMatch, MongoRawQueryHelper<Employee> queryHelper, char c)
        {
            var filter = baseMatch & queryHelper.FilterBuilder.Where(x => x.LastName.StartsWith(c.ToString()));
            var project = queryHelper.ProjectionBuilder.Expression(x => new EmployeeRef()
            {
                Id = x.Id.ToString(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                Location = x.Location,
                MiddleName = x.MiddleName,
                PayrollId = x.PayrollId,
                PayrollRegion = x.PayrollRegion,
                PreferredName = x.PreferredName
            });
            var sort = queryHelper.SortBuilder.Ascending(x => x.LastName).Ascending(x => x.FirstName);
            return Task.Run(() =>
            {
                Employees.AddRange(queryHelper.FindWithProjection(filter, project, sort).AsReadOnly());
                //Employees.AddRange(queryHelper.FindWithProjection(filter, project, sort));
            });
           
        }

        private Task QueryRange(FilterDefinition<Employee> baseMatch, MongoRawQueryHelper<Employee> queryHelper, string minChar, string maxChar)
        {
            var rangeFilterMin = queryHelper.FilterBuilder.Gte(x => x.LastName.Substring(1, 1), minChar);
            var rangeFilterMax = queryHelper.FilterBuilder.Lte(x => x.LastName.Substring(1, 1), maxChar);
            var filter = baseMatch & rangeFilterMin & rangeFilterMax;
            var project = queryHelper.ProjectionBuilder.Expression(x => new EmployeeRef()
            {
                Id = x.Id.ToString(),
                FirstName = x.FirstName,
                LastName = x.LastName,
                Location = x.Location,
                MiddleName = x.MiddleName,
                PayrollId = x.PayrollId,
                PayrollRegion = x.PayrollRegion,
                PreferredName = x.PreferredName
            });
            var sort = queryHelper.SortBuilder.Ascending(x => x.LastName).Ascending(x => x.FirstName);
            return Task.Run(() =>
            {
                Employees.AddRange(queryHelper.FindWithProjection(filter, project, sort).AsReadOnly());
                //Employees.AddRange(queryHelper.FindWithProjection(filter, project, sort));
            });

        }

    }
}
