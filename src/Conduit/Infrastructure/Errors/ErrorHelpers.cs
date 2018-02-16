using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conduit.Infrastructure.Errors
{
    public static class ErrorHelpers
    {
        public static string NotFound(string thing) => $"{thing} not found";
        public static string InUse(string thing) => $"{thing} already in use";

    }
}
