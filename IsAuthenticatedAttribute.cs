using System;

namespace AOP
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class IsAuthenticatedAttribute : Attribute
    {
        private readonly bool isAdminRequired;
        public IsAuthenticatedAttribute(bool adminRequired)
        {
            this.isAdminRequired = adminRequired;
        }

        public virtual bool IsAuthenticated { get { return isAdminRequired; } }

    }
}