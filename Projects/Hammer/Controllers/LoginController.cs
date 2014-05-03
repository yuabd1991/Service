using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyUI.Models;
using System.Web.Security;
using EasyUI.Helpers;

namespace EasyUI.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public ActionResult Login(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				var loginMessage = new LoginHelper().Login(model.UserID, model.Password, model.RememberMe);

				if (loginMessage.Tag > 0)
				{
					if (returnUrl != null)
						return Redirect(returnUrl.ToString());
					else
						return RedirectToAction("Index", "Home", new { area = "Admin" });
				}
				else
				{
					ViewBag.LoginError = loginMessage.Message;
					return View("Index", model);
				}
			}
			else
			{
				return View("Index", model);
			}
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			Session.Abandon();

			return Redirect("/");
		}

    }
}
