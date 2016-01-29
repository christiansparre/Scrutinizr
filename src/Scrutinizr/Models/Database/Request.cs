using System;
using System.Collections.Generic;

namespace Scrutinizr.Models.Database
{
    public class Request
    {
        public Request()
        {
            Id = Guid.NewGuid().ToString("N");
        }
        public string Id { get; set; }
        public string Url { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public byte[] Body { get; set; }
        public string ReceptacleId { get; set; }
        public DateTime ReceivedAtUtc { get; set; }
        public string Method { get; set; }
    }
}