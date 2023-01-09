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
        private AppScreen screen = new AppScreen();

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

            _ListOfTransactions = new List<Transaction>();
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
                    MakeWithdrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    var internalTransfer = screen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;
                case (int)AppMenu.ViewTransactions:
                    ViewTransaction();
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

            // Store in transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transactionAmount, "");
            // Update Account balance
            selectedAccount.AccountBalance += transactionAmount;

            // Success message
            Utility.PrintMessage($"\nYour deposit of {Utility.FormatAmount(transactionAmount)} was succesful.", true);
        }


        private bool PreviewCountingNotes(int amount)
        {
            int hundredCount = amount / 100;
            int fiftyCount = (amount % 100) / 50;

            Console.WriteLine("\nSummary:");
            Console.WriteLine("----------");
            Console.WriteLine($"{AppScreen.currency}100 X {hundredCount} = {Utility.FormatAmount(hundredCount * 100)}");
            Console.WriteLine($"{AppScreen.currency}50  X {fiftyCount} = {Utility.FormatAmount(fiftyCount * 50)}");
            Console.WriteLine($"Total amount: {Utility.FormatAmount(amount)}");

            int option = Validator.Convert<int>("1 to confirm");
            return option.Equals(1);
        }


        public void MakeWithdrawal()
        {

            var transactionAmount = 0;
            int selectedAmount = AppScreen.SelectAmount();

            if (selectedAmount == -1)
            {
                MakeWithdrawal();
                return;
            }
            else if (selectedAmount == 0)
            {
                transactionAmount = Validator.Convert<int>($"amount {AppScreen.currency}");
            }
            else
            {
                transactionAmount = selectedAmount;
            }

            // Amount validation
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

            // Business Logic Validation
            if (transactionAmount > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"\nTransaction Failed. Your balance is too low to withdraw" +
                    $" {Utility.FormatAmount(transactionAmount)}.", false);
                return;
            }

            // Store in transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, -transactionAmount, "");
            // Update Account balance
            selectedAccount.AccountBalance -= transactionAmount;

            // Success message
            Utility.PrintMessage($"\nYou have successfully withdrawn {Utility.FormatAmount(transactionAmount)}", true);
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


        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if (internalTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("Transfer amount should be more than zero. Try again.", false);
                return;
            }
            if (internalTransfer.TransferAmount > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You don't have enough balance to transfer " +
                    $"{Utility.FormatAmount(internalTransfer.TransferAmount)}.", false);
                return;
            }

            var selectedRecieverAccount = (from userAccount in userAccountList
                                           where userAccount.AccountNumber == internalTransfer.RecipientAccountNumber
                                           select userAccount).FirstOrDefault();
            if (selectedRecieverAccount == null)
            {
                Utility.PrintMessage("Transfer failed. Reciever's account number is invalid.", false);
                return;
            }
            if (selectedRecieverAccount.FullName != internalTransfer.RecipientAccountName)
            {
                Utility.PrintMessage("Transfer failed. Incorrect reciever's account name.", false);
                return;
            }

            // Sender
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount,
                $"Transfered to {selectedRecieverAccount.AccountNumber} ({selectedRecieverAccount.FullName})");
            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;

            // Reciever
            InsertTransaction(selectedRecieverAccount.Id, TransactionType.Transfer, internalTransfer.TransferAmount,
                $"Transfered from {selectedAccount.AccountNumber} ({selectedAccount.FullName})");
            selectedRecieverAccount.AccountBalance += internalTransfer.TransferAmount;

            // Success message
            Utility.PrintMessage($"\nYou have successfully transfered {Utility.FormatAmount(internalTransfer.TransferAmount)}" +
                $"to {internalTransfer.RecipientAccountName}", true);
        }
    }
}
