using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _client;
        protected readonly string _serviceAddress;

        protected BaseClient(IConfiguration config, string ServiceAddress)
        {
            _serviceAddress = ServiceAddress;

            _client = new HttpClient
            {
                BaseAddress = new Uri(config["ClientAddress"])
            };
            var headers = _client.DefaultRequestHeaders.Accept;
            headers.Clear();
            headers.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        protected async Task<T> GetAsync<T>(string url, CancellationToken cancel = default) where T : new()
        {
            var response = await _client.GetAsync(url, cancel);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<T>(cancel);
            return new T();
        }
        protected T Get<T>(string url) where T : new() => GetAsync<T>(url).Result;
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken cancel = default) where T : new()
        {
            var response = await _client.PutAsJsonAsync<T>(url, item, cancel);
            return response.EnsureSuccessStatusCode();
        }
        protected HttpResponseMessage Put<T>(string url, T item) where T : new() => PutAsync<T>(url, item).Result;
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken cancel = default) where T : new()
        {
            var response = await _client.PostAsJsonAsync<T>(url, item, cancel);
            return response.EnsureSuccessStatusCode();
        }
        protected HttpResponseMessage Post<T>(string url, T item) where T : new() => PostAsync<T>(url, item).Result;
        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancel = default)
            => await _client.DeleteAsync(url, cancel);
        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
    }
}
