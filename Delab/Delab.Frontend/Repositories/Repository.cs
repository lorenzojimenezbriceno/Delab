using System.Text;
using System.Text.Json;

namespace Delab.Frontend.Repositories
{
    public class Repository : IRepository
    {
        /*
         * cliente HTTP
         */

        private readonly HttpClient _httpClient;

        /*
         * Constructor
         */

        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /*
         * Get
         */

        public async Task<HttpResponseWrapper<object>> GetAsync(string url)
        {
            var responseHTTP = await _httpClient.GetAsync(url);
            return new HttpResponseWrapper<object>(null, !responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
        {
            var responseHTTP = await _httpClient.GetAsync(url);
            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswerAsync<T>(responseHTTP);
                return new HttpResponseWrapper<T>(response, false, responseHTTP);
            }

            return new HttpResponseWrapper<T>(default, true, responseHTTP);
        }

        /*
         * Post
         */

        public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHTTP = await _httpClient.PostAsync(url, messageContent);

            return new HttpResponseWrapper<object>(null, !responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

        public async Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHTTP = await _httpClient.PostAsync(url, messageContent);

            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswerAsync<TResponse>(responseHTTP);
                return new HttpResponseWrapper<TResponse>(response, false, responseHTTP);
            }

            return new HttpResponseWrapper<TResponse>(default, !responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

        /*
         * Put
         */

        public async Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHTTP = await _httpClient.PutAsync(url, messageContent);

            return new HttpResponseWrapper<object>(null, !responseHTTP.IsSuccessStatusCode, responseHTTP);

        }

        public async Task<HttpResponseWrapper<TResponse>> PutAsync<T, TResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHTTP = await _httpClient.PutAsync(url, messageContent);

            if (responseHTTP.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswerAsync<TResponse>(responseHTTP);
                return new HttpResponseWrapper<TResponse>(response, false, responseHTTP);
            }

            return new HttpResponseWrapper<TResponse>(default, true, responseHTTP);
        }

        /*
         * Delete
         */
        
        public async Task<HttpResponseWrapper<object>> DeleteAsync(string url)
        {
            var responseHTTP = await _httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, !responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

        /*
         * Helper
         * 
         * Se utiliza para convertir el json recibido para que no sea capital case sino camel case (regular no como en C#)
         */

        private async Task<TResponse?> UnserializeAnswerAsync<TResponse>(HttpResponseMessage responseHTTP)
        {
            var response = await responseHTTP.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
        }
    }
}
