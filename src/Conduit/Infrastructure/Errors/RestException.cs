using System;
using System.Net;

namespace Conduit.Infrastructure.Errors
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }

        public object Errors { get; }

        public HttpStatusCode Code { get; }
    }
}
