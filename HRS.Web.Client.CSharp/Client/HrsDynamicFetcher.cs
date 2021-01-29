using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HRS.Web.Client.CSharp.Referenced_Classes;

namespace HRS.Web.Client.CSharp.Client
{
    public class HrsDynamicFetcher : BaseClient
    {
        private readonly WebApiSource _source;

        public HrsDynamicFetcher(WebApiSource source)
        {
            _source = source;
        }

        public HrsSecurityModel GetHrsSecurityForUser(string activeDirectoryId)
        {
            var client = GetHttpClient(_source);

            var apiCall = $"ExternalApi/GetHrsSecurityForUser/{activeDirectoryId}";

            HttpResponseMessage response = client.GetAsync(apiCall).Result;
            CheckResponseStatus(response);

            // return a single dynamic object
            return ToHrsSecurityModel(response);
        }

        public HseSecurityModel GetHseSecurityForUser(string activeDirectoryId)
        {
            var client = GetHttpClient(_source);

            var apiCall = $"ExternalApi/GetHseSecurityForUser/{activeDirectoryId}";

            HttpResponseMessage response = client.GetAsync(apiCall).Result;
            CheckResponseStatus(response);

            // return a single dynamic object
            return ToHseSecurityModel(response);
        }

        public List<EmployeeDetailsGridModel> GetEmployeeList()
        {
            var client = GetHttpClient(_source);

            var apiCall = $"ExternalApi/GetEmployeeList";

            HttpResponseMessage response = client.GetAsync(apiCall).Result;
            CheckResponseStatus(response);

            return ToEmployeeDetailsGridModels(response);
        }

        public List<LocationModel> GetAllLocations()
        {
            var client = GetHttpClient(_source);

            var apiCall = $"ExternalApi/GetAllLocations";

            HttpResponseMessage response = client.GetAsync(apiCall).Result;
            CheckResponseStatus(response);

            return ToLocationModels(response);
        }


        public List<QngEmployeeBasicDataModel> GetEmployeeDetailsForQng(DateTime dateOf, string activeDirectoryId)
        {
            var client = GetHttpClient(_source);

            var dateOfString = DateStringForHttp(dateOf);
            var apiCall = $"ExternalApi/GetEmployeeDetailsForQng/{dateOfString}/{activeDirectoryId}";

            HttpResponseMessage response = client.GetAsync(apiCall).Result;
            CheckResponseStatus(response);

            return ToQngEmployeeBasicDataModel(response);

        }

        public List<QngEmployeeIncidentVarDataModel> GetEmployeeIncidentsByVarDataField(string varDataField, DateTime minDate, DateTime maxDate, string activeDirectoryId)
        {
            var client = GetHttpClient(_source);
            client.Timeout = new TimeSpan(0,0,2,0);

            var minDateAsString = DateStringForHttp(minDate);
            var maxDateAsString = DateStringForHttp(maxDate);
            var apiCall = $"ExternalApi/GetEmployeeIncidentsByVarDataField/{varDataField}/{minDateAsString}/{maxDateAsString}/{activeDirectoryId}";

            HttpResponseMessage response = client.GetAsync(apiCall).Result;
            CheckResponseStatus(response);

            return ToQngEmployeeIncidentsVarDataModel(response);
        }

        public List<QngEmployeeIncidentModel> GetEmployeeIncidents(DateTime minDate, DateTime maxDate, string activeDirectoryId)
        {
            var client = GetHttpClient(_source);
            client.Timeout = new TimeSpan(0, 0, 2, 0);

            var minDateAsString = DateStringForHttp(minDate);
            var maxDateAsString = DateStringForHttp(maxDate);
            var apiCall = $"ExternalApi/GetEmployeeIncidents/{minDateAsString}/{maxDateAsString}/{activeDirectoryId}";

            HttpResponseMessage response = client.GetAsync(apiCall).Result;
            CheckResponseStatus(response);

            return ToQngEmployeeIncidentsModel(response);
        }


        public List<QngCompensationModel> GetCompensationForQng(DateTime dateOf, string activeDirectoryId, bool isActive)
        {
            var client = GetHttpClient(_source);
            client.Timeout = new TimeSpan(0, 0, 2, 0);

            var dateOfString = DateStringForHttp(dateOf);
            var apiCall = $"ExternalApi/GetCompensationForQng/{dateOfString}/{activeDirectoryId}/{isActive}";

            HttpResponseMessage response = client.GetAsync(apiCall).Result;
            CheckResponseStatus(response);

            return toQngCompensationModels(response);

        }

        public List<QngTrainingInfoModel> GetTrainingInfo(DateTime minDate, DateTime maxDate, string activeDirectoryId)
        {
            var client = GetHttpClient(_source);
            client.Timeout = new TimeSpan(0, 0, 10, 0);

            var minDateAsString = DateStringForHttp(minDate);
            var maxDateAsString = DateStringForHttp(maxDate);
            var apiCall = $"ExternalApi/GetTrainingInfo/{minDateAsString}/{maxDateAsString}/{activeDirectoryId}";

            HttpResponseMessage response = client.GetAsync(apiCall).Result;
            CheckResponseStatus(response);

            return ToQngTrainingInfoModel(response);
        }


        #region Private Methods (Do not change)

        private static string DateStringForHttp(DateTime dateOf)
        {
            return $"{dateOf.Month}-{dateOf.Day}-{dateOf.Year}";
        }

        private static dynamic ToDynamicObject(HttpResponseMessage response)
        {
            return (dynamic)JObject.Parse(response.Content.ReadAsStringAsync().Result);
        }

        private static List<dynamic> ToDynamicList(HttpResponseMessage response)
        {
            return JArray.Parse(response.Content.ReadAsStringAsync().Result).Select(d => (dynamic)d).ToList();
        }

        private static List<QngEmployeeBasicDataModel> ToQngEmployeeBasicDataModel(HttpResponseMessage response)
        {
            var result = new List<QngEmployeeBasicDataModel>();
            var array = JArray.Parse(response.Content.ReadAsStringAsync().Result);
            foreach (var value in array)
            {
                result.Add(value.ToObject<QngEmployeeBasicDataModel>());
            }

            return result;
        }

        private static List<QngEmployeeIncidentVarDataModel> ToQngEmployeeIncidentsVarDataModel(HttpResponseMessage response)
        {
            var result = new List<QngEmployeeIncidentVarDataModel>();
            var array = JArray.Parse(response.Content.ReadAsStringAsync().Result);
            foreach (var value in array)
            {
                result.Add(value.ToObject<QngEmployeeIncidentVarDataModel>());
            }

            return result;
        }

        private static List<QngEmployeeIncidentModel> ToQngEmployeeIncidentsModel(HttpResponseMessage response)
        {
            var result = new List<QngEmployeeIncidentModel>();
            var array = JArray.Parse(response.Content.ReadAsStringAsync().Result);
            foreach (var value in array)
            {
                result.Add(value.ToObject<QngEmployeeIncidentModel>());
            }

            return result;
        }


        private static List<QngCompensationModel> toQngCompensationModels(HttpResponseMessage response)
        {
            var result = new List<QngCompensationModel>();
            var array = JArray.Parse(response.Content.ReadAsStringAsync().Result);
            foreach (var value in array)
            {
                result.Add(value.ToObject<QngCompensationModel>());
            }

            return result;
        }

        private static List<QngTrainingInfoModel> ToQngTrainingInfoModel(HttpResponseMessage response)
        {
            var result = new List<QngTrainingInfoModel>();
            var array = JArray.Parse(response.Content.ReadAsStringAsync().Result);
            foreach (var value in array)
            {
                result.Add(value.ToObject<QngTrainingInfoModel>());
            }
            return result;
        }


        private static List<LocationModel> ToLocationModels(HttpResponseMessage response)
        {
            var result = new List<LocationModel>();
            var array = JArray.Parse(response.Content.ReadAsStringAsync().Result);
            foreach (var value in array)
            {
                result.Add(value.ToObject<LocationModel>());
            }

            return result;
        }

        private static List<EmployeeDetailsGridModel> ToEmployeeDetailsGridModels(HttpResponseMessage response)
        {
            var result = new List<EmployeeDetailsGridModel>();
            var array = JArray.Parse(response.Content.ReadAsStringAsync().Result);
            foreach (var value in array)
            {
                result.Add(value.ToObject<EmployeeDetailsGridModel>());
            }

            return result;
        }


        private static HrsSecurityModel ToHrsSecurityModel(HttpResponseMessage response)
        {
            var jObject = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            return jObject.ToObject<HrsSecurityModel>();
        }

        private static HseSecurityModel ToHseSecurityModel(HttpResponseMessage response)
        {
            var jObject = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            return jObject.ToObject<HseSecurityModel>();
        }


        private static void CheckResponseStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                throw new Exception($"{response.StatusCode.ToString()} - {response.Content.ReadAsStringAsync().Result}");
            }
        }

        #endregion
    }
}
