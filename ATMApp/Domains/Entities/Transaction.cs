using ATMApp.Domains.Enums;

namespace ATMApp.Domains.Entities
{
	public class Transaction
	{
        public int TransactionId { get; set; }
        public int UserAccountId { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public Decimal TransactionAmount { get; set; }
        public string Description { get; set; }
    }
}

