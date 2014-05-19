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
    public class CoursesController : Controller
    {
        //
        // GET: /Admin/Courses/
        
        #region 课程

        public ActionResult CoursesListView()
        {
            ViewBag.Type = new Helpers.CourseHelper().GetCourseTypeKeyName();

            return View();
        }

        public ActionResult AuditListView()
        {
            ViewBag.Type = new Helpers.CourseHelper().GetCourseTypeKeyName();

            return View();
        }

        public ActionResult AuditCourseJson(int id, int auditID)
        {
            var result = new Helpers.CourseHelper().AuditCourse(id, auditID);

            return Json(result);
        }

        public ActionResult CourseListJson()
        {
            int tCount = 0;
            GetReportDataParams param = new GetReportDataParams();
            List<KeyValue> where = new Functions().GetParam(Request);
            if (!LoginHelper.IsManage)
            {
                where.Add(new KeyValue() { Key = "UserID", Value = LoginHelper.UserID.ToString() });
            }

            param.PageIndex = string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]);
            param.PageSize = string.IsNullOrEmpty(Request["rows"]) ? 20 : Convert.ToInt32(Request["rows"]);
            param.Order = Request["sort"] == null ? "" : Request["sort"] + " " + Request["order"];
            param.Where = where;

            var list = new Helpers.CourseHelper().GetCourseReport(param, out tCount);
            var json = new DataGridJson(tCount, list);

            return Json(json);
        }

        public ActionResult AddCourseView()
        {
            ViewBag.Type = new Helpers.CourseHelper().GetCourseTypeKeyName();
            ViewBag.Industry = new Helpers.IndustryHelper().GetIndustryKeyName();

            return View();
        }

        public ActionResult AddCourseJson(CourseEntity param)
        {
            param.AddUserID = Helpers.LoginHelper.UserID;
            param.UserID = Helpers.LoginHelper.UserID;
            
            var result = new Helpers.CourseHelper().InsertCourse(param);
            return Json(result);
        }

        public ActionResult EditCourseView(int id)
        {
            ViewBag.Type = new Helpers.CourseHelper().GetCourseTypeKeyName();
            ViewBag.Industry = new Helpers.IndustryHelper().GetIndustryKeyName();
            var course = new Helpers.CourseHelper().GetCourseByID(id);

            return View(course);
        }

        public ActionResult EditCourseJson(CourseEntity param)
        {
            param.AddUserID = Helpers.LoginHelper.UserID;
            param.UserID = Helpers.LoginHelper.UserID;

            var result = new Helpers.CourseHelper().UpdateCourse(param);
            return Json(result);
        }

        public ActionResult DelCourseJson(int id)
        {
            var result = new Helpers.CourseHelper().DeleteCourse(id);

            return Json(result);
        }

        #endregion

        #region 课程类型

        public ActionResult CourseTypeListView()
        {
            return View();
        }

        public ActionResult CourseTypeListJson()
        {
            int tCount = 0;
            GetReportDataParams param = new GetReportDataParams();
            List<KeyValue> where = new Functions().GetParam(Request);
            //where.Add(new KeyValue() { Key = "ChlBusinessID", Value = new UserHelper().CurrentChlBussinessID.ToString() });

            param.PageIndex = string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]);
            param.PageSize = string.IsNullOrEmpty(Request["rows"]) ? 20 : Convert.ToInt32(Request["rows"]);
            param.Order = Request["sort"] == null ? "" : Request["sort"] + " " + Request["order"];
            param.Where = where;

            var list = new Helpers.CourseHelper().GetCourseTypeReport(param, out tCount);
            var json = new DataGridJson(tCount, list);

            return Json(json);
        }

        public ActionResult AddCourseTypeView()
        {
            return View();
        }

        public ActionResult AddCourseTypeJson(CourseTypeEntity param)
        {
            param.UserID = Helpers.LoginHelper.UserID;
            var result = new Helpers.CourseHelper().InsertCourseType(param);

            return Json(result);
        }

        public ActionResult EditCourseTypeView(int id)
        {
            var course = new Helpers.CourseHelper().GetCourseTypeByID(id);

            return View(course);
        }

        public ActionResult EditCourseTypeJson(CourseTypeEntity param)
        {
            param.UserID = Helpers.LoginHelper.UserID;
            var result = new Helpers.CourseHelper().InsertCourseType(param);

            return Json(result);
        }

        public ActionResult DelCourseTypeJson(int id)
        {
            var result = new Helpers.CourseHelper().DeleteCourseType(id);

            return Json(result);
        }

        #endregion

    }
}
