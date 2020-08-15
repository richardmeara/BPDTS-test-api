using BPDTS_Test.API.Models;
using BPDTS_Test.API.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BPDTS_Test.API.Services
{
    public class BPDTSTestAppService : IBPDTSTestAppService
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _apiUri;

        public BPDTSTestAppService(IConfiguration config, IHttpClientFactory clientFactory)
        {
            _config = config;
            _clientFactory = clientFactory;
            _apiUri = _config["TestApi:Endpoint"];
        }

        public async Task<List<User>> GetUsersByCity(string city)
        {
            var responseMessage = await HttpGet($"{_apiUri}/city/{city}/users");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseObject = await responseMessage.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(responseObject);
                return users;
            }
            return null;
        }

        public async Task<User> GetUser(string id)
        {
            var responseMessage = await HttpGet($"{_apiUri}/user/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseObject = await responseMessage.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(responseObject);
                return user;
            }
            return null;
        }

        public async Task<List<User>> GetUsers()
        {
            var responseMessage = await HttpGet($"{_apiUri}/users");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseObject = await responseMessage.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(responseObject);
                return users;
            }
            return null;
        }

        #region "Base Methods"

        public async Task<HttpResponseMessage> HttpGet(string uri)
        {
            HttpClient client = _clientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(double.Parse(_config["TestApi:TimeoutLength"]));
            HttpResponseMessage responseMessage = await client.GetAsync(uri);
            return responseMessage;
        }

        #endregion "Base Methods"
    }
}