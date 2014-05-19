using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity.Entities;

namespace EasyUI.Controllers
{
    public class CoursesController : Controller
    {
        //
        // GET: /Courses/

        public ActionResult Index()
        {
            var coures = new Helpers.CourseHelper().GetCourseList();
            ViewBag.Course = "active";

            return View(coures);
        }

        public ActionResult Detail(int id)
        {
            var course = new Helpers.CourseHelper().GetCourseByID(id);
            ViewBag.Course = "active";

            return View(course);
        }

        public ActionResult ApplyJson(ApplyCourseEntity param)
        {
            //var 
            return View();
        }

    }
}
