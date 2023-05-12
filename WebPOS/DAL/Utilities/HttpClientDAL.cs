using DAL.Utilities.Interface;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
//using System.Web.Mvc;

namespace DAL.Utilities
{
    public class HttpClientDAL : IClientApi
    {
        public HttpClient Client
        {
            get; set;
        }
        public string _accessToken { get; set; }

        public HttpClientDAL()
        {
            string ServerAPI = string.Empty;
            ServerAPI = System.Configuration.ConfigurationManager.AppSettings["ServerAPILocal"].ToString();

            Client = new HttpClient();
            Client.BaseAddress = new Uri(ServerAPI);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            Client.Timeout = TimeSpan.FromMinutes(3);
        }

        public HttpResponseMessage GetResponseAPI(string url)
        {
            try
            {
                return Client.GetAsync(url).Result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj)
        {
            try
            {
                return Client.PostAsJsonAsync(url, obj).Result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //public async Task<int> ReadWaitAsStringAsyncAPI(string url, object obj)
        //{
        //    string JsonResult = string.Empty;

        //    try
        //    {
        //        url = "api2/GetUpdateInfoCliente";
        //        //HttpResponseMessage response = _VentasUsuarioBL.ReadAsStringAsyncAPI(url, clientes);
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(url);
        //            HttpResponseMessage responses = await client.PostAsJsonAsync(url, obj);

        //            if (responses.IsSuccessStatusCode)
        //            {
        //                var result = responses.Content.ReadAsAsync<int>();

        //                JsonResult = JsonConvert.SerializeObject(result);
        //                return await result;
        //            }
        //        }
        //        //}
        //        //responses.EnsureSuccessStatusCode();
        //        //var result = response.Content.ReadAsAsync<int>().Result;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

    }
}
