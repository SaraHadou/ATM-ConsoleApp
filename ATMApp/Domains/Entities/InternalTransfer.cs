using System;
namespace ATMApp.Domains.Entities
{
	public class InternalTransfer
	{	
        public Decimal TransferAmount { get; set; }
		public long RecipientAccountNumber { get; set; }
		public string RecipientAccountName { get; set; }		
	}
}

