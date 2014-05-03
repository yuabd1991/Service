using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Component;
using EasyUI.Models;
using Entity.Entities;

namespace EasyUI.Controllers
{
	public class HomeController : Controller
	{

		//
		// GET: /Home/

		//public ActionResult Test()
		//{
		//    var 
		//    return View();
		//}

		public ActionResult Index()
		{
			ViewBag.Home = "home-page";
			return View();
		}
	}
}
