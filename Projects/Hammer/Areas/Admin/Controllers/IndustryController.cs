using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Component;
using Entity.Entities;
using Component.Component;

namespace EasyUI.Areas.Admin.Controllers
{
    [Authorize]
    public class IndustryController : Controller
    {
        //
        // GET: /Admin/Industry/

        public ActionResult IndustryListView()
        {
            return View();
        }

        public ActionResult IndustryListJson()
        {
            int tCount = 0;
            GetReportDataParams param = new GetReportDataParams();
            List<KeyValue> where = new Functions().GetParam(Request);

            param.PageIndex = string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]);
            param.PageSize = string.IsNullOrEmpty(Request["rows"]) ? 20 : Convert.ToInt32(Request["rows"]);
            param.Order = Request["sort"] == null ? "" : Request["sort"] + " " + Request["order"];
            param.Where = where;

            var list = new Helpers.IndustryHelper().GetIndustryListReport(param, out tCount);
            var json = new DataGridJson(tCount, list);

            return Json(json);
        }

        public ActionResult AddIndustryView()
        {

            return View();
        }

        public ActionResult AddIndustryJson(IndustryEntity param)
        {
            param.UserID = User.Identity.Name.Uint();
            var result = new Helpers.IndustryHelper().InsertIndustry(param);
            return Json(result);
        }

        public ActionResult EditIndustryView(int id)
        {
            var industry = new Helpers.IndustryHelper().GetIndustryByID(id);

            return View(industry);
        }

        public ActionResult EditIndustryJson(IndustryEntity param)
        {
            param.UserID = User.Identity.Name.Uint();
            var result = new Helpers.IndustryHelper().UpdateIndustry(param);
            return Json(result);
        }

        public ActionResult DelIndustryJson(int id)
        {
            var result = new Helpers.IndustryHelper().DeleteIndustry(id);

            return Json(result);
        }

    }
}
