﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyUI.Controllers
{
    public class CoursesController : Controller
    {
        //
        // GET: /Courses/

        public ActionResult Index()
        {
            var coures = new Helpers.CourseHelper().GetCourseList();

            return View(coures);
        }

        public ActionResult Detail(int id)
        {
            var course = new Helpers.CourseHelper().GetCourseByID(id);
            return View(course);
        }

    }
}
