using Microsoft.AspNetCore.Http;
using System;
using System.Runtime.Serialization;

namespace IdentittVault.System
{
    [Serializable]
    public class IdentittVaultBadRequestException : IdentittVaultException
    {
        private const IdentittVaultExceptionType EXCEPTION_TYPE = IdentittVaultExceptionType.BAD_REQUEST;

        private const int STATUS_CODE = StatusCodes.Status400BadRequest;
        private const string DEFAULT_MESSAGE = "The request has some errors.";

        public IdentittVaultBadRequestException(): base(EXCEPTION_TYPE, STATUS_CODE, DEFAULT_MESSAGE)
        {
        }

        public IdentittVaultBadRequestException(string message) : base(EXCEPTION_TYPE, STATUS_CODE, message)
        {
        }

        public IdentittVaultBadRequestException(string message, Exception innerException) : base(EXCEPTION_TYPE, STATUS_CODE, message, innerException)
        {
        }

        protected IdentittVaultBadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}