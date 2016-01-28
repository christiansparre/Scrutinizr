using System.Linq;
using LiteDB;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Scrutinizr.Models;
using Scrutinizr.Models.Database;
using Scrutinizr.Models.Home;

namespace Scrutinizr.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(IConfiguration config) : base(config){}

        [HttpGet("")]
        public ActionResult Index()
        {
            var coll = Database.GetCollection<Receptacle>("Receptacles");

            var receptacles = coll.FindAll().OrderByDescending(d => d.CreatedUtc).Take(100).ToList();

            return View(new HomeIndexModel { Receptacles = receptacles });
        }


        
    }

    public class BaseController : Controller
    {
        public BaseController(IConfiguration config)
        {
            Database = new LiteDatabase(config["Database"]);
        }

        public LiteDatabase Database { get; private set; }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Database.Dispose();

            base.OnActionExecuted(context);
        }
    }
}