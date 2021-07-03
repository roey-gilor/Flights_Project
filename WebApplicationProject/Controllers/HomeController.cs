using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationProject.Models;

namespace WebApplicationProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            //return View();
            var res = System.IO.File.ReadAllText("wwwroot\\FlightPages\\loginPage.html");
            return Content(res, "text/html");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult AcceptPageGET()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><body>");
            foreach (var key in Request.Query.Keys)
            {
                sb.Append($"<h1>{key} : {Request.Query[key]} </h1>");
            }
            sb.Append("</html>");
            return Content(sb.ToString());

        }

        //[HttpPost]
        public ActionResult AcceptPagePOST()
        {
            if (Request.Form["password"] != Request.Form["username"])
            {
                var res = System.IO.File.ReadAllText("wwwroot\\FlightPages\\loginPage.html");
                return Content(res, "text/html");
            }
            else
            {
                var res = System.IO.File.ReadAllText("wwwroot\\FlightPages\\AirlinePage.html");
                return Content(res, "text/html");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
