using ATMApp.Domains.Entities;

namespace ATMApp.UI
{
	public class AppScreen
	{

        internal const string currency = "$";
        

		internal static void Welcome()
        {
            Console.Clear();
            Console.Title = "My ATM APP";
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("\n\n------------------------- Welcome to Our ATM App -----------------------\n\n");
            Console.WriteLine("Please Insert your Card");
            Console.WriteLine("Note: Actual ATM will accept only physical cards, " +
                "read the card number and validate it.");

            Utility.PressEnterToContinue();
        }


        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();

            tempUserAccount.CardNumber = Validator.Convert<long>("your card Numer");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Please, Enter your card PIN."));

            return tempUserAccount;
        }


        internal static void LoginProgress()
        {
            Console.WriteLine("\nChecking Card Number and PIN...");
            Utility.PrintDotAnimation();
        }


        internal static void PrintLockScreen()
        {
            Console.Clear();
            Utility.PrintMessage("Your account is locked. Please visit the nearest branch " +
                "to unlock your account. Thanks You.", true);
            Utility.PressEnterToContinue();
            Environment.Exit(1);
        }


        internal static void WelcomeCustomer(string fullName)
        { 
            Console.WriteLine($"Welcome back, {fullName}");
            Utility.PressEnterToContinue();
        }


        internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("--------My ATM APP Menu--------");
            Console.WriteLine(":                             :");
            Console.WriteLine("1. Acount Balance             :");
            Console.WriteLine("2. Cash Deposit               :");
            Console.WriteLine("3. Withdrawal                 :");
            Console.WriteLine("4. Transfer                   :");
            Console.WriteLine("5. Transactions               :");
            Console.WriteLine("6. Logout                     :");
            Console.WriteLine("-------------------------------");
        }


        internal static void LogoutProgress()
        {
            Console.WriteLine("Thank You for Banking with us.");
            Utility.PrintDotAnimation();
            Console.Clear();
        }


        internal static int SelectAmount()
        {
            Console.WriteLine("");
            Console.WriteLine("1. {0} 50      5. {0} 500  ", currency);
            Console.WriteLine("2. {0} 100     6. {0} 1,000", currency);
            Console.WriteLine("3. {0} 200     7. {0} 2,000", currency);
            Console.WriteLine("4. {0} 400     8. {0} 4,000", currency);
            Console.WriteLine("0. Other                   ");
            Console.WriteLine("");

            int selectedAmount = Validator.Convert<int>("option:");

            switch (selectedAmount)
            {
                case 1:
                    return 50;
                    break;
                case 2:
                    return 100;
                    break;
                case 3:
                    return 200;
                    break;
                case 4:
                    return 400;
                    break;
                case 5:
                    return 500;
                    break;
                case 6:
                    return 1000;
                    break;
                case 7:
                    return 2000;
                    break;
                case 8:
                    return 4000;
                    break;
                case 0:
                    return 0;
                    break;
                default:
                    Utility.PrintMessage("Invalid input. Try again");
                    return -1;
                    break;
            }                
        }


        internal InternalTransfer InternalTransferForm()
        {
            var internalTransfer = new InternalTransfer();
            internalTransfer.RecipientAccountNumber = Validator.Convert<long>("recipient's account number:"); ;
            internalTransfer.RecipientAccountName = Utility.GetUserInput("recipient's name:");
            internalTransfer.TransferAmount = Validator.Convert<decimal>($"amount {currency}:");
            return internalTransfer;
        }

    }
}

