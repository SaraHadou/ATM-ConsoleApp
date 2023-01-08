namespace ATMApp.Domains.Interfaces
{
	public interface IUserAccountActions
	{
		void CheckBalance();
		void PlaceDeposit();
		void MakeWithdrawal();
	}
}

