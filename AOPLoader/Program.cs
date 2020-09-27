using AOP;
using AOP.FeaturesToggle;
using System;
using System.Security.Principal;
using System.Threading;
/// <summary>
/// Main program Extract Only
/// </summary>
namespace AOPLoader
{
    class Program
    {
        static void Main()
        {

            ShowData(PaymentDispatcherFactory.Create<IPaymentDispatcher>());

        }

        private static void ShowData(IPaymentDispatcher PO)
        {
            var tx1 = new Payment { ID = 1, AccountID = "070102 01234567", Amount = 10.30M };
            var tx2 = new Payment { ID = 2, AccountID = "070103 76543219", Amount = 30.10M };
            PO.InjectPayment(tx1);
            PO.InjectPayment(tx2);
            PO.AuthorizePayment(tx1);
            PO.AuthorizePayment(tx2);
            PO.SettlePayment(tx1);
            PO.SettlePayment(tx2);
            PO.ClearPayments();
            #region ...
            Console.WriteLine("\r\nRunning as user");
            Thread.CurrentPrincipal =
              new GenericPrincipal(new GenericIdentity("NormalUser"),
              new string[] { });
            PO.InjectPayment(tx1);
            PO.InjectPayment(tx2);
            PO.AuthorizePayment(tx1);
            PO.AuthorizePayment(tx2);
            PO.SettlePayment(tx1);
            PO.SettlePayment(tx2);
            #endregion
            Console.WriteLine("\r\nAOP app stopping");
            Console.ReadKey();
        }
    }
}
