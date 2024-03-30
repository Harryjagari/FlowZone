using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlowZone.Helpers
{
    public static class HttpClientHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<HttpResponseMessage> GetAsync<T>(string requestUri)
        {
            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HTTP GET request: " + ex.Message);
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            try
            {
                var response = await _httpClient.PostAsync(requestUri, content);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HTTP POST request: " + ex.Message);
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public static async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            try
            {
                var response = await _httpClient.PutAsync(requestUri, content);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HTTP PUT request: " + ex.Message);
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(requestUri);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HTTP DELETE request: " + ex.Message);
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
