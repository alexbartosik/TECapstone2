﻿using RestSharp;
using RestSharp.Authenticators;
using System;
using TenmoClient.Data;

namespace TenmoClient
{
    /// <summary>
    /// This class must ONLY communicate with the LoginController on the server-side.
    /// </summary>
    public class AuthService
    {
        private const string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();
        
        public bool IsLoggedIn
        {
            get
            {
                return client.Authenticator != null;
            }
        }
        
        public string Token { get; set; }
        
        public void ClearAuthenticator()
        {
            client.Authenticator = null;
            Token = null;
        }
        
        //login endpoints
        public bool Register(LoginUser registerUser)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "login/register");
            request.AddJsonBody(registerUser);
            
            IRestResponse<API_User> response = client.Post<API_User>(request);
            
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return false;
            }
            else if (!response.IsSuccessful)
            {
                if (!string.IsNullOrWhiteSpace(response.Data.Message))
                {
                    Console.WriteLine("An error message was received: " + response.Data.Message);
                }
                else
                {
                    Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
                }
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public API_User Login(LoginUser loginUser)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "login");
            request.AddJsonBody(loginUser);
            
            IRestResponse<API_User> response = client.Post<API_User>(request);
            
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                
                return null;
            }
            else if (!response.IsSuccessful)
            {
                if (!string.IsNullOrWhiteSpace(response.Data.Message))
                {
                    Console.WriteLine("An error message was received: " + response.Data.Message);
                }
                else
                {
                    Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
                }
                return null;
            }
            else
            {
                client.Authenticator = new JwtAuthenticator(response.Data.Token);
                Token = response.Data.Token;
                
                return response.Data;
            }
        }
    }
}



