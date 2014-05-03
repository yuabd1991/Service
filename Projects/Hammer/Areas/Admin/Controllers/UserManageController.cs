using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Component;
using Entity.Entities;

namespace EasyUI.Areas.Admin.Controllers
{
	[Authorize]
    public class UserManageController : BaseController
    {
        //
        // GET: /Admin/UserManage/

		public ActionResult UserListView()
        {
            return View();
        }

		public ActionResult UserListJson()
		{
			int tCount = 0;
			GetReportDataParams param = new GetReportDataParams();
			List<KeyValue> where = new Functions().GetParam(Request);

			param.PageIndex = string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]);
			param.PageSize = string.IsNullOrEmpty(Request["rows"]) ? 20 : Convert.ToInt32(Request["rows"]);
			param.Order = Request["sort"] == null ? "" : Request["sort"] + " " + Request["order"];
			param.Where = where;

			var list = new Helpers.SystemHelper().GetUserListReport(param, out tCount);
			var json = new DataGridJson(tCount, list);

			return Json(json);
		}

		#region 权限设置

		public ActionResult UserPermissionListView(int id)
		{
			var ids = new Helpers.SystemHelper().GetPermissionList(id);
			ViewBag.IDS = ids;
			return View();
		}

		public ActionResult UserPermissionListJson(string id)
		{
			var list = new Helpers.SystemHelper().GetMenus(id);

			return Json(list);
		}

		#endregion

		#region 角色

		public ActionResult UserRoleListView()
		{
			return View();
		}

		public ActionResult UserRoleListJson()
		{
			int tCount = 0;
			GetReportDataParams param = new GetReportDataParams();
			List<KeyValue> where = new Functions().GetParam(Request);

			param.PageIndex = string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]);
			param.PageSize = string.IsNullOrEmpty(Request["rows"]) ? 20 : Convert.ToInt32(Request["rows"]);
			param.Order = Request["sort"] == null ? "" : Request["sort"] + " " + Request["order"];
			param.Where = where;

			var list = new Helpers.SystemHelper().GetUserRoleList(param, out tCount);
			var json = new DataGridJson(tCount, list);

			return Json(json);
		}

		public ActionResult AddUserRoleView()
		{
			return View();
		}

		public ActionResult AddUserRoleJson(UserRoleEntity param)
		{
			var result = new Helpers.SystemHelper().InsertUserRole(param);

			return Json(result);
		}

		public ActionResult EditUserRoleView(int id)
		{
			var result = new Helpers.SystemHelper().GetUserRoleByID(id);

			return View(result);
		}

		public ActionResult EditUserRoleJson(UserRoleEntity param)
		{
			return Json(new Helpers.SystemHelper().UpdateUserRole(param));
		}

		public ActionResult DelUserRoleJson(int id)
		{
			var result = new Helpers.SystemHelper().DeleteUserRole(id);

			return Json(result);
		}

		#endregion
    }
}
