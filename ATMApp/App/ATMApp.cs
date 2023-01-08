using ATMApp.Domains.Entities;
using ATMApp.Domains.Enums;
using ATMApp.Domains.Interfaces;
using ATMApp.UI;

namespace ATMApp
{
    public class ATMApp : IUserLogin, IUserAccountActions, ITransaction
    {

        private List<UserAccount> userAccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _ListOfTransactions;


        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {
                new UserAccount { Id = 1, FullName = "John Den", AccountNumber = 37368176381763,
                    CardNumber = 123456, CardPin = 123456, AccountBalance = 40000.00m, IsLocked = false },
                new UserAccount { Id = 2, FullName = "Sara clark", AccountNumber = 87683798237890,
                    CardNumber = 123123, CardPin = 123123, AccountBalance = 50000.00m, IsLocked = false },
                new UserAccount { Id = 3, FullName = "David Ma.", AccountNumber = 80980980121210,
                    CardNumber = 111222, CardPin = 111222, AccountBalance = 20000.00m, IsLocked = true  },
            };
        }


        public void Run()
        {
            AppScreen.Welcome();
            CheckUserCardNumAndPassword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            AppScreen.DisplayAppMenu();
            ProcessMenuOption();
        }


        public void CheckUserCardNumAndPassword()
        {
            bool isValidLogin = false;
            while (isValidLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach (UserAccount account in userAccountList)
                {
                    selectedAccount = account;
                    if (inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.TotalLogin++;
                        if (inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;
                            if (selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                            {
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isValidLogin = true;
                                break;
                            }
                        }
                    }
                    if (isValidLogin == false)
                    {
                        Utility.PrintMessage("\nInvalid Card Number or PIN.", false);
                        selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                        if (selectedAccount.IsLocked)
                        {
                            AppScreen.PrintLockScreen();
                        }
                    }
                    Console.Clear();
                }
            }
        }


        public void ProcessMenuOption()
        {
            switch (Validator.Convert<int>("an option:"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    Console.WriteLine("Making withdrawal...");
                    break;
                case (int)AppMenu.InternalTransfer:
                    Console.WriteLine("Making internal transfer...");
                    break;
                case (int)AppMenu.ViewTransactions:
                    Console.WriteLine("Viewing transactions...");
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utility.PrintMessage("You have successfully logged out." +
                        " Please recieve your card.", true);
                    Run();
                    break;
                default:
                    Utility.PrintMessage("Invalid option.", false);
                    break;
            }
        }


        public void CheckBalance()
        {
            Utility.PrintMessage($"Your account balance is: {Utility.FormatAmount(selectedAccount.AccountBalance)}");
        }


        public void PlaceDeposit()
        {
            Console.WriteLine("Only multiples of 50 or 100 dollars allowed.");
            int transactionAmount = Validator.Convert<int>($"amount {AppScreen.currency}");

            // Checking and counting
            Console.WriteLine("\nPLEASE WAIT WHILE YOUR TRANSACTION IS PROCESSING!");
            Utility.PrintDotAnimation();

            if (transactionAmount <= 0)
            {
                Utility.PrintMessage("\nTransaction Failed. Amount needs to be greater than zero. Try again.", false);
                return;
            }
            if (transactionAmount % 50 != 0)
            {
                Utility.PrintMessage("\nTransaction Failed. Amount should be in multiples of 50 or 100. Try again.", false);
                return;
            }
            if (PreviewCountingNotes(transactionAmount) == false)
            {
                Utility.PrintMessage("\nTransaction Failed. You have cancelled your action.", false);
                return;
            }

            // Update Account balance
            selectedAccount.AccountBalance += transactionAmount;

            InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transactionAmount, "");
                
            // Print succesful message
            Utility.PrintMessage($"\nYour deposit of {Utility.FormatAmount(transactionAmount)} was succesful.", true);
        }


        private bool PreviewCountingNotes(int amount)
        {
            int hundredCount = amount / 100;
            int fiftyCount = (amount % 100) / 50;

            Console.WriteLine("\nSummary:");
            Console.WriteLine("----------");
            Console.WriteLine($"{AppScreen.currency}100 X {hundredCount} = {Utility.FormatAmount(hundredCount * 100)}" );
            Console.WriteLine($"{AppScreen.currency}50  X {fiftyCount}   = {Utility.FormatAmount(fiftyCount   *  50)}");
            Console.WriteLine($"Total amount: {Utility.FormatAmount(amount)}");

            int option = Validator.Convert<int>("1 to confirm");
            return option.Equals(1);
        }


        public void MakeWithdrawal()
        {
            throw new NotImplementedException();
        }


        public void InsertTransaction(int _UserAccountId, TransactionType _type, decimal _amount, string _desc)
        {
            var transaction = new Transaction()
            {
                TransactionId = Utility.GetTransactionId(),
                UserAccountId = _UserAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _type,
                TransactionAmount = _amount,
                Description = _desc
            };

            _ListOfTransactions.Add(transaction);
        }


        public void ViewTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
