using System;
using System.Runtime.Serialization;

namespace IdentittVault.System
{
    [Serializable]
    public abstract class IdentittVaultException : Exception
    {
        public IdentittVaultExceptionType ExceptionType { get; private set; }
        public int StatusCode { get; private set; }

        public IdentittVaultException(IdentittVaultExceptionType exceptionType, int statusCode, string message) : base(message)
        {
            this.ExceptionType = exceptionType;
            this.StatusCode = statusCode;
        }

        public IdentittVaultException(IdentittVaultExceptionType exceptionType, int statusCode, string message, Exception innerException) : base(message, innerException)
        {
            this.ExceptionType = exceptionType;
            this.StatusCode = statusCode;
        }

        protected IdentittVaultException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}