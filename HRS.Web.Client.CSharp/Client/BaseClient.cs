using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HRS.Web.Client.CSharp.Client
{
    public enum WebApiSource
    {
        Development,
        Production,
        Local,
        QngProd
    }

    public class BaseClient
    {
        static HttpClient client = new HttpClient();
        public static readonly string HRS_DEV = $"http://s-us-hrsdev01.howcogroup.com:80/";
        public static readonly string HRS_PROD = $"http://s-us-hrs01.howcogroup.com:80/";
        public static readonly string HRS_LOCAL = $"http://192.168.102.223:5001";

        public static readonly string HRS_QNGPROD = $"http://s-us-hrsqng01.howcogroup.com:80/";

        public HttpClient GetHttpClient(WebApiSource source)
        {
            return CreateHttpClient(source);
        }

        private static HttpClient CreateHttpClient(WebApiSource source)
        {
            var baseAddress = HRS_DEV;
            switch (source)
            {
                case WebApiSource.Development:
                {
                    baseAddress = HRS_DEV; 
                    break;
                    }
                case WebApiSource.Production:
                {
                    baseAddress = HRS_PROD;
                    break;
                }
                case WebApiSource.Local:
                {
                    baseAddress = HRS_LOCAL;
                    break;
                }
                case WebApiSource.QngProd:
                {
                    baseAddress = HRS_QNGPROD;
                    break;
                }
            }

            client = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
            
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

    }
}
