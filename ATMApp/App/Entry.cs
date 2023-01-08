using ATMApp.UI;

namespace ATMApp.App
{
	class Entry
	{
        static void Main(string[] args)
        {
            ATMApp atmApp = new ATMApp();
            atmApp.InitializeData();
            atmApp.Run();
        }
    }
}

