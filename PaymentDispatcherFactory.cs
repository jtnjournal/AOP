using AOP.FeaturesToggle;

namespace AOP
{
    public class PaymentDispatcherFactory
    {
        public static IPaymentDispatcher Create<T>()
        {
            var ToggleBasicLogging = new BasicLogging();
            var ToggleDynamicLogging = new DynamicProxyLogging();
            var ToggleAuthentication = new Authentication();
            var paymentDispatcher = new PaymentDispatcher();
            if (ToggleDynamicLogging.FeatureEnabled)
            {
                var decoratedDispatcher = (IPaymentDispatcher)new LoggerProxy<IPaymentDispatcher>(paymentDispatcher).GetTransparentProxy();
                if (ToggleAuthentication.FeatureEnabled)
                    decoratedDispatcher = (IPaymentDispatcher)new AuthenticationProxy<IPaymentDispatcher>(decoratedDispatcher).GetTransparentProxy();
                return decoratedDispatcher;
            }
            if (ToggleBasicLogging.FeatureEnabled)
            {
                return new LogPaymentDispatcher(new PaymentDispatcher());
            }
            return paymentDispatcher;
        }
    }
}


