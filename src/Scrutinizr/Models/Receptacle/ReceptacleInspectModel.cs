using System.Collections.Generic;
using Scrutinizr.Models.Database;

namespace Scrutinizr.Models.Receptacle
{
    public class ReceptacleInspectModel
    {
        public Database.Receptacle Receptacle { get; set; }
        public List<Request> Requests { get; set; }
    }
}