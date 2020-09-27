namespace AOP
{
    /// <summary>
    /// Entity class Extract Only
    /// </summary>
    public class Payment
    {
        public int ID { get; set; }
        public string AccountID { get; set; }
        public bool Authorised { get; set; }
        public bool Settled { get; set; }
        public decimal Amount { get; set; }

        public Payment(int paymentId, string accountID, decimal amount)
        {
            this.ID = paymentId;
            AccountID = accountID;
            Amount = amount;
        }
        public Payment()
        {

        }
    }
}
