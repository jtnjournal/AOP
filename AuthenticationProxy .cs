using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Threading;

namespace AOP
{
    class AuthenticationProxy<T>: RealProxy
    {
        private readonly T anyClassorInterface;
        public AuthenticationProxy(T classorInterface)
          : base(typeof(T))
        {
            anyClassorInterface = classorInterface;
        }
        private void Log(string msg, object arg = null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg, arg);
            Console.ResetColor();
        }
        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;
            bool userIsAuthenticated = false;

            foreach (object obj in methodInfo.GetCustomAttributes(false))
                if (obj is IsAuthenticatedAttribute attribute)
                {
                    userIsAuthenticated = attribute.IsAuthenticated;
                }

            if (userIsAuthenticated)
            {
                if (Thread.CurrentPrincipal.IsInRole("ADMIN"))
                {
                    try
                    {
                        Log("Admin rights granted - You can execute '{0}' ",
                          methodCall.MethodName);
                        var result = methodInfo.Invoke(anyClassorInterface, methodCall.InArgs);
                        return new ReturnMessage(result, null, 0,
                          methodCall.LogicalCallContext, methodCall);
                    }
                    catch (Exception e)
                    {
                        Log(string.Format(
                          "Admin rights granted - Exception {0} executing '{1}'", e,
                          methodCall.MethodName));
                        return new ReturnMessage(e, methodCall);
                    }
                }
                else
                {
                    Log("Admin rights NOT granted - You can't execute '{0}' ",
                    methodCall.MethodName);
                    return new ReturnMessage(null, null, 0,
                    methodCall.LogicalCallContext, methodCall);
                }
            }
            else
            {
                try
                {
                    Log("User not authenticated - You can execute '{0}' ",
                      methodCall.MethodName);
                    var result = methodInfo.Invoke(anyClassorInterface, methodCall.InArgs);
                    return new ReturnMessage(result, null, 0,
                      methodCall.LogicalCallContext, methodCall);
                }
                catch (Exception e)
                {
                    Log(string.Format(
                      "User not authenticated - Exception {0} executing '{1}'", e,
                      methodCall.MethodName));
                    return new ReturnMessage(e, methodCall);
                }

            }
        }
        
    }
}
