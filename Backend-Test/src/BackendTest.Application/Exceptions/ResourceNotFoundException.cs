using System;

namespace BackendTest.Application.Exceptions;

public sealed class ResourceNotFoundException(string resourceName, int id)
    : Exception($"{resourceName} with id '{id}' does not exist.");
