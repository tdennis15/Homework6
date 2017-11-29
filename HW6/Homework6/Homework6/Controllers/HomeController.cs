using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework6.Models;

namespace Homework6.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Creating a context instance in so that we can access the database inside our controller
        /// </summary>
        private AdventureWorksContext db = new AdventureWorksContext();


        /// <summary>
        /// Standard index action handler. This will handle the landing page and the home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}