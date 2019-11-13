using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Interfaces.Api;

namespace WebStore.Clients.Value
{
    public class ValuesClient : BaseClient, IValueService
    {
        public ValuesClient(IConfiguration config):base(config,"api/values")
        {

        }
        public async Task<HttpStatusCode> DeletAsync(int id)
        {
            var response = await _client.DeleteAsync($"{_serviceAddress}/{id}");
            return response.StatusCode;
        }

        public HttpStatusCode Delete(int id)
        {
            return DeletAsync(id).Result;
        }

        public IEnumerable<string> Get()
        {
            return GetAsync().Result;
        }

        public string Get(int id)
        {
            return GetAsync(id).Result;
        }

        public async Task<IEnumerable<string>> GetAsync()
        {
            var response = await _client.GetAsync(_serviceAddress);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<string>>();
            return Array.Empty<string>();
        }

        public async Task<string> GetAsync(int id)
        {
            var response = await _client.GetAsync($"{_serviceAddress}/{id}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<string>();
            return string.Empty;
        }

        public Uri Post(string value)
        {
            return PostAsync(value).Result;
        }

        public async Task<Uri> PostAsync(string value)
        {
            var response = await _client.PostAsJsonAsync(_serviceAddress, value);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public HttpStatusCode Put(int id, string value)
        {
            return PutAsync(id, value).Result;
        }

        public async Task<HttpStatusCode> PutAsync(int id, string value)
        {
            var response = await _client.PutAsJsonAsync($"{_serviceAddress}/{id}", value);
            return response.EnsureSuccessStatusCode().StatusCode;
        }
    }
}
