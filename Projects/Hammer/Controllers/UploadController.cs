using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Component;

namespace EasyUI.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /Upload/

		public ActionResult UploadImage()
		{
			HttpPostedFileBase file = Request.Files["Filedata"];
			
			var param = new Entity.Entities.PictureEntity();
			//param.Type = Request["Type"];
			//param.TargetID = Convert.ToInt32(Request["ID"]);
			
			var picture = new Helpers.SystemHelper().InsertPicture(param);

			var guid = Guid.NewGuid();

            string saveUrl = DateTime.Now.ToString("yyyyMM") + @"\" + picture.Result + ".jpg";

            string url = System.Web.HttpContext.Current.Server.MapPath("/Content/Upload/images/" + saveUrl);

			string directory = Path.GetDirectoryName(url);
			if (directory != null && !Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			if (file != null) file.SaveAs(url);

			file = null;

			return Json(new { queueID = 1, ID = picture.Result, imgUrl = saveUrl.Replace(@"\", "/") });
		}


		/// <summary>
		/// 通用文件上传
		/// </summary>
		/// <returns></returns>
		public ActionResult UploadFile()
		{
			BaseObject res = new BaseObject();
			res.Tag = -1;
			res.Message = "请选择上传文件！";

			HttpPostedFileBase file = Request.Files["Filedata"];
			if (file != null)
			{
				try
				{
					if (file != null && file.ContentLength > 0)
					{
						string filename = file.FileName;//上传的文件路径
						string saveUrl = "/Content/Images/";//文件保存路径 例如：
						string destination = HttpContext.Server.MapPath("~/");
						string resUrl = saveUrl + DateTime.Now.ToString("yyyyMM");
						//判断是否全路径
						string name = filename;
						if (filename.IndexOf(@"\") != -1)
						{
							name = filename.Substring(filename.LastIndexOf(@"\"));
						}
						//string FileType = name.Substring(name.LastIndexOf(".") + 1);//文件类型   
						resUrl += DateTime.Now.ToString("hhmmss") + name;
						string saveFile = Path.Combine(destination, resUrl);
						string directory = Path.GetDirectoryName(saveFile);
						if (!Directory.Exists(directory))
						{
							Directory.CreateDirectory(directory);
						}
						file.SaveAs(saveFile);

						res.Tag = 1;
					}
				}
				catch (Exception ex)
				{
					res.Tag = -1;
					res.Message = "上传异常！";
					throw new Exception(ex.Message);
				}
			}
			return Json(res);
		}

		//public ActionResult UploadPicture(HttpPostedFileBase filedata)
		//{
		//    //xheditorModel model = new xheditorModel();

		//    if (filedata.ContentLength > 0)
		//    {
		//        var guid = Guid.NewGuid();
		//        var fileName = guid + ".jpg";
		//        //Activity.Helpers.IO.UploadImageFile(filedata.InputStream, "/Content/images", fileName, 720);

		//        model.msg = "/Content/images/" + fileName;
		//    }

		//    //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
		//    //return this.Content(javaScriptSerializer.Serialize(model));
		//}

    }
}
