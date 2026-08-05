using System;

namespace BackendTest.Application.Exceptions;

public sealed class RequestConflictException(string message) : Exception(message);
