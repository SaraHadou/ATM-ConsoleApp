using ATMApp.UI;

namespace ATMApp.App
{
	class Entry
	{
        static void Main(string[] args)
        {
            while (true)
            {
                AppScreen.Welcome();
                ATMApp atmApp = new ATMApp();
                atmApp.InitializeData();
                atmApp.CheckUserCardNumAndPassword();
                atmApp.Welcome();
            }           
        }
    }
}

