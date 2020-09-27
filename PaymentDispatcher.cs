using System;
using System.Collections.Generic;


namespace AOP
{
    public class PaymentDispatcher : IPaymentDispatcher
    {
        private readonly DateTime now;
        public readonly Stack<Payment> payments = new Stack<Payment>();

        public PaymentDispatcher()
        {
            this.now = DateTime.Now;
        }
        /// <summary>
        /// Inject a Payment into the payments queue
        /// </summary>
        /// <param name="tx">Payment to inject</param>
        public void InjectPayment(Payment tx)
        {
            //Console.WriteLine("[{0}]: Log Entry ==> Before Inject Payment",
            //                   DateTime.Now.ToString("HH:mm:ss.f"));
            this.payments.Push(tx); // Add the tx to the payments Stack
            Console.WriteLine("[{0}]: Account '{1}' has a payment transaction " +
                              "for £ {2} inserted at {3} dispatcher.",  
                               DateTime.Now.ToString("HH:mm:ss.f"), 
                               this.payments.Peek().AccountID, 
                               this.payments.Peek().Amount, 
                               this.now.ToString("HH:mm:ss"));
            //Console.WriteLine("[{0}]: Log Entry ==> After Inject Payment",
            //                 DateTime.Now.ToString("HH:mm:ss.f"));
        }

        public void AuthorizePayment(Payment tx = null)
        {
            tx = LocatePayment(tx);
            tx.Authorised = true;
            Console.WriteLine("[{0}]: PaymentId = '{1}' has been Authorised for £ {2}", DateTime.Now.ToString("HH:mm:ss.f"), tx.ID, tx.Amount);
        }
        public void SettlePayment(Payment tx = null)
        {
            tx = LocatePayment(tx);
            tx.Settled = true;
            Console.WriteLine("[{0}]:'{1}' Account has been settled.", DateTime.Now.ToString("HH:mm:ss.f"), tx.AccountID);
        }

        public Payment LocatePayment(Payment tx)
        {
            Payment txAffected = tx;
            if (this.payments.Count > 0)
            {
                if (tx == null)
                    txAffected = this.payments.Peek();
                else
                {
                    foreach (Payment item in this.payments)
                        if (item.ID == tx.ID)
                        {
                            txAffected = item;
                            break;
                        }
                }
            }
            else
            {
                throw new ArgumentNullException("Null Payment cannot be used within empty Dispatchers");
            }
            return txAffected;
        }

        public void ClearPayments()
        {
            payments.Clear();
        }
    }
}