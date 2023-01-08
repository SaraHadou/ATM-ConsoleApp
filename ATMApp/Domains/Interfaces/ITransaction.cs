using System;
using ATMApp.Domains.Enums;

namespace ATMApp.Domains.Interfaces
{
	public interface ITransaction
	{
		void InsertTransaction(int _UserAccountId, TransactionType _type, Decimal _amount, string _desc);
		void ViewTransaction();
	}
}

