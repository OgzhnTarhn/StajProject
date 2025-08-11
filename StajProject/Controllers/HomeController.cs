using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StajProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Kullanıcı giriş yapmamışsa Login'e yönlendir
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            // Giriş yapan herkes direkt Trips sayfasına yönlendirilsin
            return RedirectToAction("Index", "Block");
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