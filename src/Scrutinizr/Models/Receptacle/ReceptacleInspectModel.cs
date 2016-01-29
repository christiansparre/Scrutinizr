using System;
using System.Collections.Generic;
using Scrutinizr.Models.Database;

namespace Scrutinizr.Models.Receptacle
{
    public class ReceptacleInspectModel
    {
        public Database.Receptacle Receptacle { get; set; }
        public IEnumerable<RequestViewModel> Requests { get; set; }
    }

    public class RequestViewModel
    {
        public string Id { get; set; }
        public Uri Url { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
        public string ReceptacleId { get; set; }
        public DateTime ReceivedAtUtc { get; set; }
        public string Method { get; set; }
    }
}