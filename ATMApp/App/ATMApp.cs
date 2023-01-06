using System;

namespace ATMApp
{
    class ATMApp
    {
        static void Main(string[] args)
        {
            // Clear the console
            Console.Clear();
            // Set the title of the console
            Console.Title = "My ATM APP";
            // Set the text color to white
            Console.ForegroundColor = ConsoleColor.White;

            // Set the Welcoming Message
            Console.WriteLine("\n\n--------------Welcome to Our ATM App--------------\n\n");
            // Prompt the user to insert his card
            Console.WriteLine("Please Insert your Card");
            Console.WriteLine("Note: Actual ATM will accept only physical cards, " +
                "read the card number and validate it.");


            Console.WriteLine("\n\nPlease Enter to continue...\n");
            Console.ReadLine();
        }
    }
}
