using System;
using TenmoClient.Data;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();

        private bool quitRequested = false;

        public void Start()
        {
            while (!quitRequested)
            {
                // Since the user can fail to log in, loop until they're logged in
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
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            string loginRegister = Console.ReadLine();
            switch (loginRegister)
            {
                case "1":
                    HandleUserLogin();
                    break;
                case "2":
                    HandleUserRegister();
                    break;
                default:
                    Console.WriteLine("Invalid selection.");
                    break;
            }
        }

        private void ShowMainMenu()
        {
            string menuSelection;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                    menuSelection = Console.ReadLine();
                    switch (menuSelection)
                    {
                        case "1": // View Balance
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case "2": // View Past Transfers
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case "3": // View Pending Requests
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case "4": // Send TE Bucks
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case "5": // Request TE Bucks
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case "6": // Log in as someone else
                            authService.ClearAuthenticator();

                            // NOTE: You will need to clear any stored JWTs in other API Clients
                            Console.WriteLine("MAKE SURE YOU SET THE JWT INTO OTHER API CLIENTS HERE");

                            return; // Leaves the menu and should return as someone else

                        case "0": // Quit
                            Console.WriteLine("Goodbye!");
                            quitRequested = true;
                            return;

                        default:
                            Console.WriteLine("That doesn't seem like a valid choice.");
                            break;
                    }
            } while (menuSelection != "0");
        }

        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine();
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
                    Console.WriteLine("YOU'RE DOING NOTHING WITH JWT");
                }
            }
        }
    }
}
