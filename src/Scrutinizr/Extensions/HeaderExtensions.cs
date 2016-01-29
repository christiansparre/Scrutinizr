using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scrutinizr.Models.Database;

namespace Scrutinizr.Extensions
{
    public static class HeaderExtensions
    {
        public static string Get(this Dictionary<string, string> headers, string key, string @default = "")
        {
            return headers.ContainsKey(key) ? headers[key] : @default;
        }
    }
}