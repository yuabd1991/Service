using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using Component;

namespace EasyUI.Areas.Admin.Controllers
{
	[Authorize]
    public class HomeController : BaseController
    {
        //
        // GET: /Admin/Home/

		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Top()
		{
			var str = new ArrayList();
			var allMenus = new Helpers.SystemHelper().GetAllMenus();
			var allPages = new Helpers.SystemHelper().GetAllPages();

			ViewBag.AllMenus = allMenus;
			ViewBag.AllPages = allPages;
			return View();
		}

		public ActionResult Left()
		{
			return View();
		}

        public ActionResult Middle()
        {
            return View();
        }

    }
}
