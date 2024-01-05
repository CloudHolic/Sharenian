using System;

namespace Sharenian.Exceptions;

public class ApiFailedException : Exception
{
    public ApiFailedException()
    {
    }

    public ApiFailedException(string message) : base(message)
    {
    }

    public ApiFailedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
