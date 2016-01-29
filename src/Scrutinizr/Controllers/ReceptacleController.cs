using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Extensions;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            return View(new ReceptacleInspectModel { Receptacle = receptacle, Requests = requests.Select(s => new RequestViewModel { Id = s.Id, ReceptacleId = s.ReceptacleId, Headers = s.Headers, Method = s.Method, ReceivedAtUtc = s.ReceivedAtUtc, Url = new Uri(s.Url), Body = GetBodyAsString(s) }).ToList() });
        }

        private string GetBodyAsString(Request request)
        {
            if (request.Body == null)
            {
                return string.Empty;
            }

            if (request.Headers.ContainsKey("Content-Type"))
            {
                var contentType = request.Headers["Content-Type"];
                if (contentType == "text/plain" || contentType == "application/json")
                {
                    var bodyAsString = Encoding.UTF8.GetString(request.Body);

                    if (contentType == "application/json")
                    {
                        try
                        {
                            return $"<pre>{JObject.Parse(bodyAsString).ToString(Formatting.Indented)}</pre>";
                        }
                        catch (Exception ex)
                        {
                            return $"Invalid JSON: {ex.Message}";
                        }

                    }

                    return bodyAsString;
                }
                if (contentType == "application/x-www-form-urlencoded")
                {
                    var bodyAsString = Encoding.UTF8.GetString(request.Body);

                    return $"<pre>{string.Join("<br />", bodyAsString.Split('&'))}</pre>";
                }
            }

            return "Unsupported Content-Type";
        }

        [Route("receptacle/{id}/{*url}", Order = 2)]
        public async Task<ActionResult> Receive(string id, string url)
        {
            var request = new Request
            {
                ReceptacleId = id,
                ReceivedAtUtc = DateTime.UtcNow,
                Url = Request.GetDisplayUrl(),
                Headers = Request.Headers.ToDictionary(d => d.Key, d => d.Value.ToString()),
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