using ATMApp.Domains.Entities;
using ATMApp.Domains.Interfaces;
using ATMApp.UI;

namespace ATMApp
{
    public class ATMApp : IUserLogin
    {

        private List<UserAccount> userAccountList;
        private UserAccount selectedAccount;


        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {
                new UserAccount { Id = 1, FullName = "John Den",   AccountNumber = 37368176381763, CardNumber = 123456, CardPin = 123456, AccountBalance = 40000.00m, IsLocked = false },
                new UserAccount { Id = 2, FullName = "Sara clark", AccountNumber = 87683798237890, CardNumber = 123123, CardPin = 123123, AccountBalance = 50000.00m, IsLocked = false },
                new UserAccount { Id = 3, FullName = "David Ma.",  AccountNumber = 80980980121210, CardNumber = 111222, CardPin = 111222, AccountBalance = 20000.00m, IsLocked = true  },
            };
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


        public void Welcome()
        {
            Console.WriteLine($"Welcome back, {selectedAccount.FullName}");
        }

    }
}
