using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LSScraper.Web.Models;
using LSScraper.LSapi;

namespace LSScraper.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<Post> posts = Api.ScrapeLS();
            return View(posts);
        }
    }
}
