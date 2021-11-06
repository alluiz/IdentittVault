using Microsoft.AspNetCore.Http;
using System;
using System.Runtime.Serialization;

namespace IdentittVault.System
{
    [Serializable]
    public class IdentittVaultConflictException : IdentittVaultException
    {
        private const IdentittVaultExceptionType EXCEPTION_TYPE = IdentittVaultExceptionType.CONFLICT;

        private const int STATUS_CODE = StatusCodes.Status409Conflict;
        private const string DEFAULT_MESSAGE = "The request has one or more conflicts.";

        public IdentittVaultConflictException(): base(EXCEPTION_TYPE, STATUS_CODE, DEFAULT_MESSAGE)
        {
        }

        public IdentittVaultConflictException(string message) : base(EXCEPTION_TYPE, STATUS_CODE, message)
        {
        }

        public IdentittVaultConflictException(string message, Exception innerException) : base(EXCEPTION_TYPE, STATUS_CODE, message, innerException)
        {
        }

        protected IdentittVaultConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}