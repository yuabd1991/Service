using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity.Entities;
using EasyUI.Helpers;

namespace EasyUI.Controllers
{
    public class RegisterController : Controller
    {
        //
        // GET: /Register/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Company()
        {
            return View();
        }

        public ActionResult Post(RegisterUser param)
        {
            var result = new LoginHelper().Register(param);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
