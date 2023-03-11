using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TenmoClient.APIClients;
using TenmoClient.Data;
using TenmoClient.Models;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.ComponentModel;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private readonly UserAccountService accountService = new UserAccountService();

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
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Invalid selection.");
                    Console.WriteLine();
                    Console.ResetColor();
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
                        Console.Clear();
                        string balance = accountService.GetCurrentBalance().ToString("C2");
                        Console.WriteLine($"Your current account balance is: {balance}");
                        Console.WriteLine();
                        break;

                    case "2": // View Past Transfers
                        Console.Clear();
                        ListAllTransfersForCurrentUser();
                        ListTransferInfoById();
                        break;

                    case "3": // View Pending Requests
                        Console.Clear();
                        ListPendingTransfers();
                        ApproveOrRejectTransfer();
                        break;

                    case "4": // Send TE Bucks
                        Console.Clear();
                        SendTEBucks();
                        break;

                    case "5": // Request TE Bucks
                        Console.Clear();
                        RequestTEBucks();
                        break;

                    case "6": // Log in as someone else
                        Console.Clear();
                        authService.ClearAuthenticator();
                        accountService.ClearAuthenticator();
                        return; // Leaves the menu and should return as someone else

                    case "0": // Quit
                        Console.Clear();
                        Console.WriteLine("Goodbye!");
                        quitRequested = true;
                        return;

                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("That doesn't seem like a valid choice.");
                        Console.ResetColor();
                        break;
                    }
            } while (menuSelection != "0");
        }

        private void ApproveOrRejectTransfer()
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine("Please enter transfer ID to approve/reject (0 to cancel): ");
                int transferId = int.Parse(Console.ReadLine());
          
                if (transferId == 0)
                {
                    Console.Clear();
                    return;
                }

                Console.Clear();
                bool transferExists = ListPendingTransfers().Any(t => t.Id == transferId);

                if (!transferExists)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please choose a valid transfer ID from the provided list.");
                    Console.WriteLine();
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("1: Approve");
                    Console.WriteLine("2: Reject");
                    Console.WriteLine("0: Don't approve or reject");
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("Please choose an option: ");
                    int userInput = int.Parse(Console.ReadLine());

                    switch (userInput)
                    {
                        case 0:
                            return;
                        case 1:
                            Transfer approvedTransfer = new Transfer();
                            approvedTransfer.Id = transferId;
                            accountService.ApproveTransfer(approvedTransfer);
                            break;
                        case 2:
                            accountService.RejectTransfer(transferId);
                            Console.Clear();
                            Console.WriteLine("Transfer has been rejected.");
                            break;
                        default:
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("ERROR: Please provide a valid input.");
                            Console.WriteLine();
                            Console.ResetColor();
                            break;
                    }
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please choose a valid transfer ID!");
                Console.ResetColor();
            }
        }

        private List<TransferRecord> ListPendingTransfers()
        {
            List<TransferRecord> pendingTransfers = accountService.GetListOfPendingTranfers();

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Pending Transfers");
            Console.WriteLine("Id".PadRight(10) + "To".PadRight(25) + "Amount");
            Console.WriteLine("--------------------------------------");

            foreach (TransferRecord t in pendingTransfers)
            {
                Console.WriteLine(t.Id.ToString().PadRight(10) + (t.ToName).PadRight(25) + t.Amount.ToString("C2"));
            }

            return pendingTransfers;
        }

        private void ListTransferInfoById()
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine("Please enter transfer ID to view details (0 to cancel): ");
                int inputId = int.Parse(Console.ReadLine());
                if(inputId == 0)
                {
                    Console.Clear();
                    return;
                }
                Console.Clear();
                bool transferIsListed = ListAllTransfersForCurrentUser().Any(t => t.Id == inputId);

                if (!transferIsListed)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Please choose a valid transfer ID from the provided list.");
                    Console.WriteLine();
                    Console.ResetColor();
                }
                else
                {
                    Console.Clear();
                    TransferRecord transfer = accountService.GetTransferById(inputId);
                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine("Transfer Details");
                    Console.WriteLine("--------------------------------------");
                    Console.WriteLine("Id: " + transfer.Id);
                    Console.WriteLine("From: " + transfer.FromName);
                    Console.WriteLine("To: " + transfer.ToName);
                    Console.WriteLine("Type: " + transfer.TypeId);
                    Console.WriteLine("Status: " + transfer.StatusId);
                    Console.WriteLine("Amount: " + transfer.Amount.ToString("C2"));
                }
            }
            catch(FormatException ex)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Please provide a valid input.");
                Console.ResetColor();
            }
        }

        private List<TransferRecord> ListAllTransfersForCurrentUser()
        {
            List<TransferRecord> transfers = accountService.GetTransferList();

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Transfers");
            Console.WriteLine("Id".PadRight(10) + "From/To".PadRight(25) + "Amount");
            Console.WriteLine("--------------------------------------");

            foreach (TransferRecord t in transfers)
            {
                Console.WriteLine(t.Id.ToString().PadRight(10) + (t.TransferDirection + t.FromName + t.ToName).PadRight(25) + t.Amount.ToString("C2"));
            }
            return transfers;
        }
       
        private void SendTEBucks()
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Users");
            Console.WriteLine("Id".PadRight(15) + "Name");
            Console.WriteLine("--------------------------------------");
            List<User> users = accountService.GetUsers();

            foreach (User u in users)
            {
                Console.WriteLine(u.UserId.ToString().PadRight(15) + u.Username);
            }

            try
            {
                SendMoneyByUserId(users);
            }
            catch (FormatException)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Please provide a valid input.");
                Console.ResetColor();
            }
        }

        private void RequestTEBucks()
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Users");
            Console.WriteLine("Id".PadRight(15) + "Name");
            Console.WriteLine("--------------------------------------");
            List<User> users = accountService.GetUsers();

            foreach (User u in users)
            {
                Console.WriteLine(u.UserId.ToString().PadRight(15) + u.Username);
            }

            try
            {
                RequestMoneyByUserId(users);
            }
            catch (FormatException)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Please provide a valid input.");
                Console.ResetColor();
            }
        }

        private void SendMoneyByUserId(List<User> users)
        {
            Console.WriteLine();
            Console.WriteLine("Enter the ID of the user you are sending to (0 to cancel): ");
            int accountToId = int.Parse(Console.ReadLine());

            if (accountToId == 0)
            {
                return;
            }

            bool userExists = users.Any(u => u.UserId == accountToId);

            if (!userExists)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please choose a valid user ID from the provided list.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Enter amount to send: ");
                decimal amountToSend = decimal.Parse(Console.ReadLine());

                if (amountToSend <= 0)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Nice try Matt, you can't send negative money!");
                    Console.ResetColor();
                }
                else if(amountToSend > accountService.GetCurrentBalance())
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Nope. You're 2 broke :(");
                    Console.ResetColor();
                }
                else
                {
                    Transfer receivedTransfer = new Transfer();
                    receivedTransfer.AccountTo = accountToId;
                    receivedTransfer.Amount = amountToSend;

                    accountService.SendTEbucks(receivedTransfer);


                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("TE Bucks sent");
                    Console.ResetColor();
                }
            }
        }

        private void RequestMoneyByUserId(List<User> users)
        {
            Console.WriteLine();
            Console.WriteLine("Enter the ID of the user you are requesting from (0 to cancel): ");
            int accountFromId = int.Parse(Console.ReadLine());

            if (accountFromId == 0)
            {
                return;
            }

            bool userExists = users.Any(u => u.UserId == accountFromId);

            if (!userExists)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Please choose a valid user ID from the provided list.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Enter amount to request: ");
                decimal amountToRequest = decimal.Parse(Console.ReadLine());

                if (amountToRequest <= 0)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Nice try Matt, just send the money instead...");
                    Console.ResetColor();
                }
                else
                {
                    Transfer receivedTransfer = new Transfer();
                    receivedTransfer.AccountFrom = accountFromId;
                    receivedTransfer.Amount = amountToRequest;

                    accountService.RequestTEbucks(receivedTransfer);

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("TE Bucks requested");
                    Console.ResetColor();
                }
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
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Registration successful. You can now log in.");
            Console.WriteLine();
            Console.ResetColor();
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
                    accountService.SetAuthenticationToken(jwt);
                }
            }
        }
    }
}
