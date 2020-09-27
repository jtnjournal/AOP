using System.Collections.Generic;

namespace AOP
{
    public interface IPaymentDispatcher
    {
        [IsLogging(true)]
        [IsAuthenticated(true)]
        void AuthorizePayment(Payment tx);
        [IsLogging(true)]
        [IsAuthenticated(false)]
        void InjectPayment(Payment tx);
        [IsLogging(false)]
        [IsAuthenticated(true)]
        void SettlePayment(Payment tx);
        [IsLogging(false)]
        [IsAuthenticated(false)]
        Payment LocatePayment(Payment tx);
        void ClearPayments();
    }
}