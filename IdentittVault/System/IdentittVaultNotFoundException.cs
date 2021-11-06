using Microsoft.AspNetCore.Http;
using System;
using System.Runtime.Serialization;

namespace IdentittVault.System
{
    [Serializable]
    public class IdentittVaultNotFoundException : IdentittVaultException
    {
        private const IdentittVaultExceptionType EXCEPTION_TYPE = IdentittVaultExceptionType.NOT_FOUND;

        private const int STATUS_CODE = StatusCodes.Status404NotFound;
        private const string DEFAULT_MESSAGE = "Not found requested resource.";

        public IdentittVaultNotFoundException(): base(EXCEPTION_TYPE, STATUS_CODE, DEFAULT_MESSAGE)
        {
        }

        public IdentittVaultNotFoundException(string message) : base(EXCEPTION_TYPE, STATUS_CODE, message)
        {
        }

        public IdentittVaultNotFoundException(string message, Exception innerException) : base(EXCEPTION_TYPE, STATUS_CODE, message, innerException)
        {
        }

        protected IdentittVaultNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}