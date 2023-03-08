using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;

namespace TenmoClient.APIClients
{
    public class UserAccountService
    {
        private const string baseUrl = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public decimal GetCurrentBalance()
        {
            RestRequest request = new RestRequest($"{baseUrl}account/balance");

            IRestResponse<decimal> response = client.Get<decimal>(request);

            if(response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occured communicating with the server.");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                return 0;
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("An error occured.");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                return 0;
            }
            return response.Data;
        }

        public List<User> GetUsers()
        {
            RestRequest request = new RestRequest($"{baseUrl}account/users");

            IRestResponse<List<User>> response = client.Get<List<User>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occured communicating with the server.");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                return new List<User>();
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("An error occured.");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                return new List<User>();
            }

            return response.Data;
        }

        public void TransferTEbucks(Transfer transfer)
        {
            RestRequest request = new RestRequest($"{baseUrl}account/transfer");

            request.AddJsonBody(transfer);

            IRestResponse response = client.Post(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                if(response.ResponseStatus == ResponseStatus.Error)
                {
                    Console.WriteLine("Could not process request: " + response.ErrorMessage);
                }
                else
                {
                    Console.WriteLine("An error occured communicating with the server.");
                    Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                }
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("An error occured.");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
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
