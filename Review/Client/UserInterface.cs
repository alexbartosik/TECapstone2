using Client.APIClients;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using TenmoClient.Data;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private readonly CityApiService cityService = new CityApiService("https://localhost:44315/");

        private bool quitRequested = false;

        public void Start()
        {
            while (!quitRequested)
            {
                while (!authService.IsLoggedIn)
                {
                    ShowLogInMenu();
                }

                // If we got here, then the user is logged in. Go ahead and show the main menu
                ShowMainMenu();
            }
        }

        private void ShowLogInMenu()
        {
            Console.WriteLine("Welcome to the TE City Manager!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (loginRegister == 1)
            {
                HandleUserLogin();
            }
            else if (loginRegister == 2)
            {
                HandleUserRegister();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void ShowMainMenu()
        {
            int menuSelection;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to the TE City Manager! Please make a selection: ");
                Console.WriteLine("1: View all cities");
                Console.WriteLine("2: Add a new city");
                Console.WriteLine("3: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else
                {
                    switch (menuSelection)
                    {
                        case 1: // View All Cities
                            ListCities();
                            break;

                        case 2: // Add a New City
                            AddCity();

                            break;

                        case 3: // Log in as someone else

                            authService.ClearAuthenticator();

                            // NOTE: You will need to clear any stored JWTs in other API Clients
                            Console.WriteLine("NOT IMPLEMENTED!");

                            return; // Leaves the menu and should return as someone else

                        case 0: // Quit
                            Console.WriteLine("Goodbye!");
                            quitRequested = true;
                            return;

                        default:
                            Console.WriteLine("That doesn't seem like a valid choice.");
                            break;
                    }
                }
            } while (menuSelection != 0);
        }

        private void ListCities()
        {
            List<City> allCities = cityService.GetAllCities();
            foreach (City c in allCities)
            {
                Console.WriteLine($"{c.Name}, {c.StateAbbreviation}");
            }
        }

        private void AddCity()
        {
            try
            {
                City newCity = new City();
                Console.WriteLine();
                Console.WriteLine("What is the name of the new city?");
                newCity.Name = Console.ReadLine();

                Console.WriteLine();
                Console.WriteLine("What is the state abbreviation?");
                newCity.StateAbbreviation = Console.ReadLine();

                Console.WriteLine();
                Console.WriteLine("What is the population?");
                newCity.Population = int.Parse(Console.ReadLine());

                Console.WriteLine();
                Console.WriteLine("What is the area?");
                newCity.Area = decimal.Parse(Console.ReadLine());

                cityService.AddCity(newCity);
                Console.WriteLine("City added");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
        }

        private void HandleUserLogin()
        {
            while (!authService.IsLoggedIn) //will keep looping until user is logged in
            {
                LoginUser loginUser = consoleService.PromptForLogin();

                // Log the user in
                API_User authenticatedUser = authService.Login(loginUser);

                if (authenticatedUser != null)
                {
                    string jwt = authenticatedUser.Token;

                    // TODO: Do something with this JWT.
                    cityService.SetAuthenticationToken(jwt);
                }
            }
        }
    }
}
