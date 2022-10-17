using System;
using System.Net;
using RestSharp;

namespace CreateUsersNeowit
{
    public class ApiClient
    {
        private static string _baseUrl;

        public ApiClient()
        {
            _baseUrl = "https://app.neowit.io/api/";
            var credential = new Token
            {
                AccessToken = null
            };
        }

        public async Task<string> GetBearerToken(User user)
        {
            Console.WriteLine("Getting Token");
            var client = new RestClient("https://app.neowit.io/api/");
            var request = new RestRequest("auth/v1/login", Method.Post);
            request.AddJsonBody(user);

            var response = await client.PostAsync<Token>(request);
            return response.AccessToken;

        }

        public async Task<User> PostDeviceRequest(string resource, User jsonBody, string token)
        {
            var client = new RestClient(_baseUrl);
            
            var request = new RestRequest(resource, Method.Post);
            request.AddJsonBody<User>(jsonBody);
            request.AddHeader("Authorization", "Bearer " + token);

            var response = await client.PostAsync<User>(request);
            //var response = await client.ExecutePostAsync<Device>(request);


            return response;
        }
    }
}

