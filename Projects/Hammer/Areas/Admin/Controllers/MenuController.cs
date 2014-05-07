using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logic;
using Component;
using Entity.Entities;

namespace EasyUI.Areas.Admin.Controllers
{
	[Authorize]
	public class MenuController : BaseController
    {
        //
        // GET: /Admin/Menu/

        public ActionResult MenuList()
        {
			//var menus = logic.GetMenus();

			return View();
        }

		public ActionResult MenuJosn(string id)
		{
			var list = new Helpers.SystemHelper().GetMenus(id);

			return Json(list);
		}

		public ActionResult AddMenu()
		{
			return View();
		}

		public ActionResult AddMenuJson(MenuEntity menu)
		{
			menu.Enable = PublicType.Yes;
			var result = new Helpers.SystemHelper().InsertMenu(menu);

			return Json(result);
		}

		public ActionResult EditMenu(int id, string type)
		{
			var page = new PageEntity();
			var menu = new MenuEntity();
			if (type.ToLower() == "page")
			{
				page = new Helpers.SystemHelper().GetPageByID(id);
			}
			else
			{
				menu = new Helpers.SystemHelper().GetMenuByID(id);
			}

			ViewBag.Page = page;
			ViewBag.Menu = menu;
			ViewBag.Type = type;

			return View();
		}

		public ActionResult EditMenuJson(MenuEntity menu)
		{
			var result = new BaseObject();
            if (menu.Type.ToLower() == "page")
			{
				result = new Helpers.SystemHelper().EditPage(menu);
			}
			else
			{
				result = new Helpers.SystemHelper().EditMenu(menu);
			}

			return Json(result);
		}

		public ActionResult EditPage(int id)
		{
			var page = new Helpers.SystemHelper().GetPageByID(id);

			return View();
		}

		public ActionResult EditPageJson(MenuEntity param)
		{
			var result = new Helpers.SystemHelper().EditPage(param);

			return Json(result);
		}

        public ActionResult DeleteMenu(int id, string state, string type)
        {
            var result = new Helpers.SystemHelper().DisEnableMenu(id, state, type);
            return Json(result);
        }
    }
}
