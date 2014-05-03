using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Component;

namespace EasyUI.Controllers
{
	public class ControlController : BaseController
    {
        //
        // GET: /Control/

        public ActionResult MenuTreeView()
        {
            return PartialView();
        }

		public ActionResult GetMenuTreeJson()
		{
			var menus = new Helpers.SystemHelper().GetTreeMenu();

			//var json = JsonConvert.SerializeObject(menus);

			return Json(menus, JsonRequestBehavior.AllowGet);
		}

    }
}
