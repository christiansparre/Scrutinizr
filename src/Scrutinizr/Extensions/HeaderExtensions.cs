using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scrutinizr.Models.Database;

namespace Scrutinizr.Extensions
{
    public static class HeaderExtensions
    {
        public static string Get(this IEnumerable<Header> headers, string key, string @default = "")
        {
            return headers.FirstOrDefault(a => a.Key.ToLowerInvariant() == key.ToLowerInvariant())?.Value ?? @default;
        }
    }
}