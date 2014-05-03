using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Component;
using Entity.Entities;
using EasyUI.Helpers;

namespace EasyUI.Areas.Admin.Controllers
{
	[Authorize]
    public class ColumnManageController : BaseController
    {
        //
        // GET: /Admin/ColumnManage/

		#region 内容模板

		public ActionResult GetContentTemplateView()
		{
			return View();
		}

	    public ActionResult ContentTempJson()
	    {
		    int tCount = 0;
			GetReportDataParams param = new GetReportDataParams();
			List<KeyValue> where = new Functions().GetParam(Request);
			//where.Add(new KeyValue() { Key = "ChlBusinessID", Value = new UserHelper().CurrentChlBussinessID.ToString() });

			param.PageIndex = string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]);
			param.PageSize = string.IsNullOrEmpty(Request["rows"]) ? 20 : Convert.ToInt32(Request["rows"]);
			param.Order = Request["sort"] == null ? "" : Request["sort"] + " " + Request["order"];
			param.Where = where;

			var list = new Helpers.SystemHelper().GetContentTemplateList(param, out tCount);
			var json = new DataGridJson(tCount, list);

			return Json(json);
	    }

		public ActionResult EditContentTempView(int id)
		{
			ViewBag.Content = new Helpers.SystemHelper().GetContentTempById(id);

			return View();
		}

        public ActionResult EditContentTempJson(ContentTemplateEntity param)
		{
			var result = new Helpers.SystemHelper().EditContentTemp(param);

			return Json(result);
		}

		#endregion

        public ActionResult ColumnListView()
        {
            return View();
        }

	    public ActionResult ColumnListJson()
	    {
			int tCount = 0;
			GetReportDataParams param = new GetReportDataParams();
			List<KeyValue> where = new Functions().GetParam(Request);
			//where.Add(new KeyValue() { Key = "ChlBusinessID", Value = new UserHelper().CurrentChlBussinessID.ToString() });

			param.PageIndex = string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]);
			param.PageSize = string.IsNullOrEmpty(Request["rows"]) ? 20 : Convert.ToInt32(Request["rows"]);
			param.Order = Request["sort"] == null ? "" : Request["sort"] + " " + Request["order"];
			param.Where = where;

			var list = new Helpers.SystemHelper().GetColumnList(param, out tCount);
			var json = new DataGridJson(tCount, list);

			return Json(json);
	    }

		public ActionResult AddColumnView()
		{
			ViewBag.Content = new Helpers.SystemHelper().GetContentComboList();

			return View();
		}

		public ActionResult AddColumnJson(ColumnEntity param)
		{
			var result = new SystemHelper().InsertColumn(param);

			return Json(result);
		}

        public ActionResult EditColumnView(int id)
        {
            ViewBag.Content = new Helpers.SystemHelper().GetContentComboList();
            var model = new Helpers.SystemHelper().GetColumnById(id);

            return View(model);
        }

        public ActionResult EditColumnJson(ColumnEntity param)
        {
            var result = new SystemHelper().UpdateColumn(param);

            return Json(result);
        }

        public ActionResult DelColumnJson(string id)
        {
            var result = new SystemHelper().DelColumn(id);

            return Json(result);
        }
    }
}
