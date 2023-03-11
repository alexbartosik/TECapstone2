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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Cannot communicate with the server.");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                Console.ResetColor();
                return 0;
            }
            if (!response.IsSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                Console.ResetColor();
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Cannot communicate with the server.");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                Console.ResetColor();
                return new List<User>();
            }
            if (!response.IsSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                Console.ResetColor();
                return new List<User>();
            }

            return response.Data;
        }

        public void SendTEbucks(Transfer transfer)
        {
            RestRequest request = new RestRequest($"{baseUrl}account/send");

            request.AddJsonBody(transfer);

            IRestResponse response = client.Post(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                if(response.ResponseStatus == ResponseStatus.Error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Could not process request. " + response.ErrorMessage);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Cannot communicate with the server.");
                    Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                    Console.ResetColor();
                }
            }
            if (!response.IsSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                Console.ResetColor();
            }
        }

        public void RequestTEbucks(Transfer transfer)
        {
            RestRequest request = new RestRequest($"{baseUrl}account/request");

            request.AddJsonBody(transfer);

            IRestResponse response = client.Post(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Could not process request. " + response.ErrorMessage);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Cannot communicate with the server.");
                    Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                    Console.ResetColor();
                }
            }
            if (!response.IsSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                Console.ResetColor();
            }
        }

        public List<TransferRecord> GetTransferList()
        {
            RestRequest request = new RestRequest($"{baseUrl}account/myTransfers");

            IRestResponse<List<TransferRecord>> response = client.Get<List<TransferRecord>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Could not process request. " + response.ErrorMessage);
                    Console.ResetColor();
                    return new List<TransferRecord>();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Cannot communicate with the server.");
                    Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                    Console.ResetColor();
                    return new List<TransferRecord>();
                }
            }
            if (!response.IsSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                Console.ResetColor();
                return new List<TransferRecord>();
            }

            return response.Data;
        }

        public TransferRecord GetTransferById(int transferId)
        {
            RestRequest request = new RestRequest($"{baseUrl}account/myTransfers/{transferId}");

            IRestResponse<TransferRecord> response = client.Get<TransferRecord>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Could not process request. " + response.ErrorMessage);
                    Console.ResetColor();
                    return new TransferRecord();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Cannot communicate with the server.");
                    Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                    Console.ResetColor();
                    return new TransferRecord();
                }
            }
            if (!response.IsSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                Console.ResetColor();
                return new TransferRecord();
            }

            return response.Data;

        }

        public List<TransferRecord> GetListOfPendingTranfers()
        {
            RestRequest request = new RestRequest($"{baseUrl}account/pending");

            IRestResponse<List<TransferRecord>> response = client.Get<List<TransferRecord>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Could not process request. " + response.ErrorMessage);
                    Console.ResetColor();
                    return new List<TransferRecord>();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Cannot communicate with the server.");
                    Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                    Console.ResetColor();
                    return new List<TransferRecord>();
                }
            }
            if (!response.IsSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                Console.ResetColor();
                return new List<TransferRecord>();
            }

            return response.Data;

        }

        public void RejectTransfer(int transferId)
        {
            RestRequest request = new RestRequest($"{baseUrl}account/reject");
            request.AddJsonBody(transferId);

            IRestResponse response = client.Put(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Could not process request. " + response.ErrorMessage);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Cannot communicate with the server.");
                    Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                    Console.ResetColor();
                }
            }
            if (!response.IsSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                Console.ResetColor();
            }
        }

        public void ApproveTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest($"{baseUrl}account/approve");
            request.AddJsonBody(transfer);

            IRestResponse<Transfer> response = client.Put<Transfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Could not process request. " + response.ErrorMessage);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Cannot communicate with the server.");
                    Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                    Console.ResetColor();
                }
            }
            if (!response.IsSuccessful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.WriteLine($"Status Code: {Convert.ToInt32(response.StatusCode)} {response.StatusDescription}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Transfer Approved!");
            Console.ResetColor();
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

        public void ClearAuthenticator()
        {
            client.Authenticator = null;
        }
    }
}
