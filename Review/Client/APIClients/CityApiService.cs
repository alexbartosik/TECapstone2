using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using Client.Models;
using RestSharp.Authenticators;

namespace Client.APIClients
{
    public class CityApiService
    {
        private readonly RestClient client;

        public CityApiService(string base_url)
        {
            this.client = new RestClient(base_url);
        }

        public List<City> GetAllCities()
        {
            RestRequest request = new RestRequest("city");

            IRestResponse<List<City>> response = client.Get<List<City>>(request);

            if(response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine($"Could not communicate with server. Check your internet connection");
                return new List<City>();
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine($"Error on request. Status Code: {response.StatusCode} {response.StatusDescription}");
                return new List<City>();
            }

            return response.Data;
        }

        public void AddCity(City city)
        {
            RestRequest request = new RestRequest("city");
            request.AddJsonBody(city);

            IRestResponse response = client.Post(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    Console.WriteLine("Could not process request: " + response.ErrorMessage);
                }
                else
                {
                    Console.WriteLine($"Could not communicate with server. Check your internet connection");
                }
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine($"Error on request. Status Code: {response.StatusCode} {response.StatusDescription}");
            }
        }

        public void SetAuthenticationToken(string jwt)
        {
            if(jwt == null)
            {
                client.Authenticator = null;
            }
            else
            {
                client.Authenticator = new JwtAuthenticator(jwt);
            }
        }
    }
}
