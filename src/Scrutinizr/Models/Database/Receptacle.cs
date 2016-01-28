using System;

namespace Scrutinizr.Models.Database
{
    public class Receptacle
    {
        public Receptacle()
        {
            Id = Guid.NewGuid().ToString("N");
            CreatedUtc = DateTime.UtcNow;
        }

        public DateTime CreatedUtc { get; set; }

        public string Id { get; set; }
    }
}