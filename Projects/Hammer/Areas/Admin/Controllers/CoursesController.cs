using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyUI.Areas.Admin.Controllers
{
    public class CoursesController : Controller
    {
        //
        // GET: /Admin/Courses/

        public ActionResult CoursesListView()
        {
            return View();
        }

    }
}
