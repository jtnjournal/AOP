using System;

namespace AOP
{

    // This attributes are to be used by Reflection Mechanisms
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]

    class IsLoggingAttribute : Attribute
    {
        private readonly bool isLogging;
        public IsLoggingAttribute(bool logged)
        {
            this.isLogging = logged;
        }

        public virtual bool IsLogging { get { return isLogging; } }

    }
}
