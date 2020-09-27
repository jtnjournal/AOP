using System;

namespace AOP
{
    public class LogPaymentDispatcher : IPaymentDispatcher
    {
        private readonly IPaymentDispatcher _decorated;

        public LogPaymentDispatcher(IPaymentDispatcher decorated)
        {
            _decorated = decorated;
        }

        private void Log(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public void AuthorizePayment(Payment item)
        {
            Log(string.Format("In decorator - Before autorize payment for {0} account.", item.AccountID));
            _decorated.AuthorizePayment(item);
            Log(string.Format("In decorator - After autorize payment for {0} account. \n", item.AccountID));
        }

        public void InjectPayment(Payment item)
        {
            Log(string.Format("In decorator - Before make a £{1} payment for {0} account.", item.AccountID,item.Amount));
            _decorated.InjectPayment(item);
            Log(string.Format("In decorator - After make a £{1} payment for {0} account. \n", item.AccountID,item.Amount));
        }

        public void SettlePayment(Payment item)
        {
            Log(string.Format("In decorator - Before settle the payment at {0} account.", item.AccountID));
            _decorated.SettlePayment(item);
            Log(string.Format("In decorator - After settle the payment at {0} account. \n", item.AccountID));
        }

        public Payment LocatePayment(Payment tx)
        {
            Log(string.Format("In decorator - Locate PaymentId {0}.", tx.ID));
            return _decorated.LocatePayment(tx);
        }

        public void ClearPayments()
        {
            _decorated.ClearPayments();
        }
    }
}
