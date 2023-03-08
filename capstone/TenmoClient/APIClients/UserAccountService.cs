using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

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
                Console.WriteLine($"Status Code: {response.StatusCode} {response.StatusDescription}");
                return 0;
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("An error occured.");
                Console.WriteLine($"Status Code: {response.StatusCode} {response.StatusDescription}");
                return 0;
            }
            return response.Data;
        }
    }
}
