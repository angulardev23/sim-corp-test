using System;

namespace BackendTest.Application.Exceptions
{
    public sealed class RequestConflictException : Exception
    {
        public RequestConflictException(string message) : base(message)
        {
        }
    }
}
