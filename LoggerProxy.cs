using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace AOP
{
    class LoggerProxy<T>: RealProxy
    {
        private readonly T anyClassorInterface;
        public LoggerProxy(T classOrInterface)
          : base(typeof(T))
        {
            anyClassorInterface = classOrInterface;
        }
        private void Log(string msg, object arg = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg, arg);
            Console.ResetColor();
        }
        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;
            bool isLogging = false;
            foreach (object obj in methodInfo.GetCustomAttributes(true))
                if (obj is IsLoggingAttribute attribute)
                {
                    isLogging = attribute.IsLogging;
                }
 
            ReturnMessage retVal = new ReturnMessage(null, null, 0, methodCall.LogicalCallContext, methodCall);

            if (isLogging)
            {
                Log("In AOP Proxy - Before executing '{0}'",
                methodCall.MethodName);
                try
                {
                    var result = methodInfo.Invoke(anyClassorInterface, methodCall.InArgs);
                    Log("In AOP Proxy - After executing '{0}' ",
                    methodCall.MethodName);
                    retVal = new ReturnMessage(result, null, 0,
                    methodCall.LogicalCallContext, methodCall);
                }
                catch (Exception e)
                {
                    Log(string.Format(
                    "In AOP Proxy- Exception {0} executing '{1}'", e,
                    methodCall.MethodName));
                    retVal = new ReturnMessage(e, methodCall);
                }
            }
            return retVal;
        }
    }
}
