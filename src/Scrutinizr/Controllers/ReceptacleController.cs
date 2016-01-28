using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Extensions;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Scrutinizr.Models;
using Scrutinizr.Models.Database;
using Scrutinizr.Models.Receptacle;

namespace Scrutinizr.Controllers
{
    public class ReceptacleController : BaseController
    {
        public ReceptacleController(IConfiguration config) : base(config)
        {
        }

        [HttpPost("receptacles")]
        public ActionResult Create()
        {
            var coll = Database.GetCollection<Receptacle>("Receptacles");

            var receptacle = new Receptacle();
            coll.Insert(receptacle);

            return RedirectToAction("Inspect", new { id = receptacle.Id });
        }

        [HttpGet("receptacle/inspect/{id}", Order = 1)]
        public ActionResult Inspect(string id)
        {
            var coll = Database.GetCollection<Receptacle>("Receptacles");
            var requestsCollection = Database.GetCollection<Request>("Requests");

            var receptacle = coll.FindById(id);
            var requests = requestsCollection.Find(f => f.ReceptacleId == id).OrderByDescending(o => o.ReceivedAtUtc).Take(100).ToList();

            return View(new ReceptacleInspectModel { Receptacle = receptacle, Requests = requests });
        }

        [Route("receptacle/{id}/{*url}", Order = 2)]
        public async Task<ActionResult> Receive(string id, string url)
        {
            var request = new Request
            {
                ReceptacleId = id,
                ReceivedAtUtc = DateTime.UtcNow,
                Url = Request.GetDisplayUrl(),
                Headers = Request.Headers.Select(s => new Header { Key = s.Key, Value = s.Value.ToString() }).ToList(),
                Method = Request.Method,
            };

            var contentLength = Request.ContentLength ?? 0;

            if (contentLength > 0 && Request.ContentType != null)
            {
                var bodyBuffer = new byte[contentLength];
                await Request.Body.ReadAsync(bodyBuffer, 0, bodyBuffer.Length);
                request.Body = bodyBuffer;
            }

            var coll = Database.GetCollection<Request>("Requests");

            coll.Insert(request);

            return Ok();
        }
    }
}