using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Route("/error")]
        public IActionResult Error()
        {
            // Handle error here
            return View();
        }
    }
}