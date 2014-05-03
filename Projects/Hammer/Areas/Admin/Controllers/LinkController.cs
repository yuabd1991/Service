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
	public class LinkController : BaseController
    {
        //
        // GET: /Admin/Link/

        public ActionResult LinksView()
        {
            return View();
        }

		public ActionResult LinkListJson()
		{
			int tCount = 0;
			GetReportDataParams param = new GetReportDataParams();
			List<KeyValue> where = new Functions().GetParam(Request);
			//where.Add(new KeyValue() { Key = "ChlBusinessID", Value = new UserHelper().CurrentChlBussinessID.ToString() });

			param.PageIndex = string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]);
			param.PageSize = string.IsNullOrEmpty(Request["rows"]) ? 20 : Convert.ToInt32(Request["rows"]);
			param.Order = Request["sort"] == null ? "" : Request["sort"] + " " + Request["order"];
			param.Where = where;

            var list = new Helpers.SystemHelper().GetLinks(param, out tCount);
			var json = new DataGridJson(tCount, list);

			return Json(json);
		}

		public ActionResult AddLinkView()
		{
			ViewBag.List = new Helpers.SystemHelper().GetLinkCategoryList();

			return View();
		}

		public ActionResult AddLinkJson(LinksEntity param)
		{
			var list = new Helpers.SystemHelper().InsertLink(param);

			return Json(list);
		}

		public ActionResult EditLinkView(int id)
		{
			var link = new Helpers.SystemHelper().GetLink(id);
			ViewBag.List = new Helpers.SystemHelper().GetLinkCategoryList();

			return View(link);
		}

		public ActionResult EditLinkJson(LinksEntity param)
		{
			var list = new Helpers.SystemHelper().UpdateLink(param);

			return Json(list);
		}

		public ActionResult DelLinkJson(int id)
		{
			var result = new Helpers.SystemHelper().DeleteLink(id);

			return Json(result);
		}

		public ActionResult LinkCategoriesView()
		{
			return View();
		}

		public ActionResult LinkCategoriesJson()
		{
			int tCount = 0;
			GetReportDataParams param = new GetReportDataParams();
			List<KeyValue> where = new Functions().GetParam(Request);

			param.PageIndex = string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]);
			param.PageSize = string.IsNullOrEmpty(Request["rows"]) ? 20 : Convert.ToInt32(Request["rows"]);
			param.Order = Request["sort"] == null ? "" : Request["sort"] + " " + Request["order"];
			param.Where = where;

			var list = new Helpers.SystemHelper().GetLinkCategories();
			var json = new DataGridJson(tCount, list);

			return Json(json);
		}

		public ActionResult AddLinkCategoryView()
		{
			return View();
		}

		public ActionResult AddLinkCategoryJson(LinkCategoryEntity param)
		{
			var list = new Helpers.SystemHelper().InsertLinkCategory(param);

			return Json(list);
		}

		public ActionResult EditLinkCategoryView(int id)
		{
			var result = new Helpers.SystemHelper().GetLinkCategory(id);

			return View(result);
		}

		public ActionResult EditLinkCategoryJson(LinkCategoryEntity param)
		{
			var list = new Helpers.SystemHelper().UpdateLinkCategory(param);

			return Json(list);
		}

		public ActionResult DelLinkCategoryJson(int id)
		{
			var result = new Helpers.SystemHelper().DeleteLinkCategory(id);

			return Json(result);
		}
    }
}
