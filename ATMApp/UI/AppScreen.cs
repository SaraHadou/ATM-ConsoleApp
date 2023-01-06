using ATMApp.Domains.Entities;

namespace ATMApp.UI
{
	public static class AppScreen
	{
		internal static void Welcome()
        {
            Console.Clear();
            Console.Title = "My ATM APP";
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("\n\n--------------Welcome to Our ATM App--------------\n\n");

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

    }
}

