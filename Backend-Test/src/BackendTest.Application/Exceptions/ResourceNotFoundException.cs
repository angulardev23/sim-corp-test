using System;

namespace BackendTest.Application.Exceptions
{
    public sealed class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string resourceName, int id)
            : base($"{resourceName} with id '{id}' does not exist.")
        {
        }
    }
}
