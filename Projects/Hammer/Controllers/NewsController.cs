using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyUI.Controllers
{
    public class NewsController : Controller
    {
        //
        // GET: /News/

        public ActionResult Index(int? id)
        {
            var imageArt = new Helpers.SystemHelper().GetArticleImageList(id);

            return View(imageArt);
        }

        public ActionResult Detail(int id)
        {
            var image = new Helpers.SystemHelper().GetArticleImageByID(id);

            return View(image);
        }
    }
}
