using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyUI.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /About/

        public ActionResult Index()
        {
            ViewBag.About = "active";
            var doc = new Helpers.SystemHelper().GetDocumentByID(1);

            return View(doc);
        }

        public ActionResult Service()
        {
            ViewBag.About = "active";
            var doc = new Helpers.SystemHelper().GetDocumentByID(2);

            return View(doc);
        }

        public ActionResult Opera()
        {
            ViewBag.About = "active";
            var doc = new Helpers.SystemHelper().GetDocumentByID(3);

            return View(doc);
        }

    }
}
