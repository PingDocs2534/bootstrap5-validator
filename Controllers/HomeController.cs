using bootstrap5.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bootstrap5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View(new IndexViewModel());
        }

        public ActionResult Login(IndexViewModel model)
        {
            ModelState.Clear();
            ModelState.AddModelError(nameof(model.UserName), "Invalid UserName");
            return View("Index", model);
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