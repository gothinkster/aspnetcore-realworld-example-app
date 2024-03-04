using System;
using System.Net;

namespace Conduit.Infrastructure.Errors;

public class RestException(HttpStatusCode code, object? errors = null) : Exception
{
    public object? Errors { get; } = errors;

    public HttpStatusCode Code { get; } = code;
}
