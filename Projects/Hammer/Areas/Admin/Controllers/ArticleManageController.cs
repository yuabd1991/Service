using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Component;
using Entity.Entities;
using EasyUI.Helpers;
using Component.Component;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace EasyUI.Areas.Admin.Controllers
{
	[Authorize]
	public class ArticleManageController : BaseController
	{
		//
		// GET: /Admin/ArticleManage/

		#region 文章

		public ActionResult ArticleList(int? columnID = 0)
		{
			ViewBag.CategoryID = columnID;
			ViewBag.Category = new SystemHelper().GetColumnDropList(0, (int)ContentType.Article);

			return View();
		}

		public ActionResult ArticleListJson()
		{
			int tCount = 0;
			GetReportDataParams param = new GetReportDataParams();
			List<KeyValue> where = new Functions().GetParam(Request);

			param.PageIndex = string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]);
			param.PageSize = string.IsNullOrEmpty(Request["rows"]) ? 20 : Convert.ToInt32(Request["rows"]);
			param.Order = Request["sort"] == null ? "" : Request["sort"] + " " + Request["order"];
			param.Where = where;

			var list = new Helpers.SystemHelper().GetArticleList(param, out tCount);
			var json = new DataGridJson(tCount, list);

			return Json(json);
		}

		public ActionResult AddArticle(int? columnID = 0)
		{
			ViewBag.CategoryID = columnID;

			ViewBag.Category = new SystemHelper().GetColumnDropList(columnID, (int)ContentType.Article);

			return View();
		}

		[ValidateInput(false)]
		public ActionResult AddArticleJson(ArticleEntity param)
		{
			param.UpdateUser = User.Identity.Name;
			var result = new Helpers.SystemHelper().InsertArticle(param);

			return Json(result);
		}


		public ActionResult EditArticleView(int id, int? columnID = 0)
		{
			var model = new Helpers.SystemHelper().GetArticleByID(id);

			ViewBag.Category = new SystemHelper().GetColumnDropList(columnID, (int)ContentType.Article);

			return View(model);
		}

		[ValidateInput(false)]
		public ActionResult EditArticleJson(ArticleEntity param)
		{
			param.UpdateUser = User.Identity.Name;
			var result = new Helpers.SystemHelper().UpdateArticle(param);

			return Json(result);
		}

		public ActionResult DeleteArticleJson(int id)
		{
			var result = new Helpers.SystemHelper().DelArticle(id);

			return Json(result);
		}

		#endregion

		#region 图文集

		public ActionResult ImageArticleListView(int? columnID = 0)
		{
			ViewBag.CategoryID = columnID;
			ViewBag.Category = new SystemHelper().GetColumnDropList(0, (int)ContentType.ArticleWithImage);

			return View();
		}

		public ActionResult ImageArticleListJson()
		{
			int tCount = 0;
			GetReportDataParams param = new GetReportDataParams();
			List<KeyValue> where = new Functions().GetParam(Request);

			param.PageIndex = string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]);
			param.PageSize = string.IsNullOrEmpty(Request["rows"]) ? 20 : Convert.ToInt32(Request["rows"]);
			param.Order = Request["sort"] == null ? "" : Request["sort"] + " " + Request["order"];
			param.Where = where;

			var list = new Helpers.SystemHelper().GetArticleImageList(param, out tCount);
			var json = new DataGridJson(tCount, list);

			return Json(json);
		}

		public ActionResult ImageArticleAddView(int? columnID = 0)
		{
			ViewBag.CategoryID = columnID;

			ViewBag.Category = new SystemHelper().GetColumnDropList(columnID, (int)ContentType.ArticleWithImage);

			return View();
		}

		[ValidateInput(false)]
		public ActionResult ImageArticleAddJson(ArticleImageEntity param)
		{
			var result = new Helpers.SystemHelper().InsertArticleImage(param);
			return Json(result);
		}

		public ActionResult ImageArticleEditView(int id)
		{
			var result = new Helpers.SystemHelper().GetArticleImageByID(id);
			ViewBag.Category = new SystemHelper().GetColumnDropList(0, (int)ContentType.ArticleWithImage);

			return View(result);
		}

		[ValidateInput(false)]
		public ActionResult ImageArticleEditJson(ArticleImageEntity param)
		{
			var result = new Helpers.SystemHelper().UpdateArticleImage(param);

			return Json(result);
		}

		public ActionResult DeleteArticleImageJson(int id)
		{
			var result = new Helpers.SystemHelper().DeleteArticleImage(id);

			return Json(result);
		}

		#endregion

		#region 单页文档

		public ActionResult DocumentsListView(int? columnID = 0)
		{
			ViewBag.CategoryID = columnID;
			ViewBag.Category = new SystemHelper().GetColumnDropList(0, (int)ContentType.Document);

			return View();
		}

		public ActionResult DocumentsListJson()
		{
			int tCount = 0;
			GetReportDataParams param = new GetReportDataParams();
			List<KeyValue> where = new Functions().GetParam(Request);

			param.PageIndex = string.IsNullOrEmpty(Request["page"]) ? 1 : Convert.ToInt32(Request["page"]);
			param.PageSize = string.IsNullOrEmpty(Request["rows"]) ? 20 : Convert.ToInt32(Request["rows"]);
			param.Order = Request["sort"] == null ? "" : Request["sort"] + " " + Request["order"];
			param.Where = where;

			var list = new Helpers.SystemHelper().GetDocumentList(param, out tCount);
			var json = new DataGridJson(tCount, list);

			return Json(json);
		}

		public ActionResult AddDocumentView()
		{
			return View();
		}

		[ValidateInput(false)]
		public ActionResult AddDocumentJson(DocumentEntity param)
		{
            param.UpdateUser = Helpers.LoginHelper.UserID;
			var result = new Helpers.SystemHelper().InsertDocument(param);

			return Json(result);
		}

		public ActionResult EditDocumentView(int id)
		{
			var model = new Helpers.SystemHelper().GetDocumentByID(id);

			return View(model);
		}

		[ValidateInput(false)]
		public ActionResult EditDocumentJson(DocumentEntity param)
		{
            param.UpdateUser = Helpers.LoginHelper.UserID;
			var result = new Helpers.SystemHelper().UpdateDocument(param);

			return Json(result);
		}

		#endregion

		public ActionResult UploadExcel()
		{
			return View();
		}

		[HttpPost]
		public ActionResult UploadExcel(HttpPostedFileBase file)
		{
			//string line;
			//string[] arrayLine;

			//if (file.ContentLength > 0)
			//{
				//if (file.ContentType == "application/vnd.ms-excel")
				//{
				//string filename = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1);
				//string filePath = HttpContext.Server.MapPath("/content/" + filename);
				//file.SaveAs(filePath);
			string connectionString = "";
			OleDbConnection Connection = new OleDbConnection();
			try
			{
				string connStr;
				connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString();
				// connStr = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString(); //从web.config配置中读取
				connectionString = connStr;
				//connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Request.PhysicalApplicationPath + connStr;
				// connectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString();
				//
				Connection = new OleDbConnection(connectionString);

				Connection.Open();

				DataSet ds = new DataSet();
				//System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(dbConnStr);
				System.Data.OleDb.OleDbDataAdapter _adapter;
				try
				{
					if (Connection.State != System.Data.ConnectionState.Open)
						Connection.Open();

					_adapter = new System.Data.OleDb.OleDbDataAdapter("select * from News_News", Connection);
					_adapter.Fill(ds);
				}
				finally
				{
					Connection.Close();
				}

				
				//OleDbTransaction myTrans = Connection.BeginTransaction();
				OleDbCommand command = new OleDbCommand("", Connection);
				//using (System.IO.StreamReader streamReader = new System.IO.StreamReader(filePath, Encoding.UTF8))
				//{
				//var ds = 
				foreach(var item in ds.Tables[0].AsEnumerable())
				{
					//0:title, 1:类型, 2:keyword, 3:overview, 4: content, 5:date, 6: vis
					//arrayLine = line.Split(new char[] { ',' });
					//if (item.Field < 7)
					//{
					//    continue;
					//}
					var type = item.Field<int>("News_Class_ID").Uint();
					//精品课程13-9,10  行业资讯14-2,3,7,8,20,29,12,19   学院动态17-4,5,6,30  学员天地15-21,22,23,24
					//灵位视角16-11,27,28  热点18-26   20-13  21-16  22-14  23-17  24-15  25-18

					Insert(type, item);
				}
				//}
				//}
			}
			catch (Exception e)
			{

				throw;
			}
			finally
			{
				Connection.Close();
			}

			//}

			return View();
		}


		public void Insert(int type, DataRow arrayLine)
		{
			var db = new Logic.DataAccess.SiteContext();

			try
			{
				var title = arrayLine.Field<string>("News_Title").UString();
				if (db.Articles.Any(m => m.Title == title) ||
					db.ArticleImages.Any(m => m.Title == title))
				{
					return;
				}
				var columnID = GetColumnID(type);
				if (columnID == 13)
				{
					var image = new Logic.Models.ArticleImage();
					image.Author = "凌薇团队";
					image.ColumnID = columnID;
					image.Content = arrayLine.Field<string>("News_Body").UString();
					image.DateCreated = arrayLine.Field<DateTime>("News_Date");
					image.IsDelete = "N";
					image.MetaDescription = arrayLine.Field<string>("News_Body_Easy").UString();
					image.MetaKeywords = arrayLine.Field<string>("News_KeyWord").UString();
					image.Overview = arrayLine.Field<string>("News_Body_Easy").UString();
					image.PageTitle = arrayLine.Field<string>("News_Title").UString();
					image.PageVisits = arrayLine.Field<int>("News_Click").Uint();
					image.ShortTitle = arrayLine.Field<string>("News_Title").UString();
					image.Slug = arrayLine.Field<string>("News_Title").UString();
					image.SortOrder = 0;
					image.Source = "凌薇团队";
					image.Title = arrayLine.Field<string>("News_Title").UString();
					image.UpdateUser = "admin";

					db.ArticleImages.Add(image);
				}
				else
				{
					var image = new Logic.Models.Article();
					image.Author = "凌薇团队";
					image.ColumnID = columnID;
					image.Content = arrayLine.Field<string>("News_Body").UString();
					image.UpdateTime = arrayLine.Field<DateTime>("News_Date");
					image.IsDelete = "N";
					image.IsPublic = PublicType.Yes;
					image.MetaDescription = arrayLine.Field<string>("News_Body_Easy").UString();
					image.MetaKeywords = arrayLine.Field<string>("News_KeyWord").UString();
					image.Overview = arrayLine.Field<string>("News_Body_Easy").UString();
					image.PageTitle = arrayLine.Field<string>("News_Title").UString();
					image.PageVisits = arrayLine.Field<int>("News_Click").Uint();
					image.ShortTitle = arrayLine.Field<string>("News_Title").UString();
					image.Slug = arrayLine.Field<string>("News_Title").UString();
					image.SortOrder = 0;
					image.Source = "凌薇团队";
					image.Title = arrayLine.Field<string>("News_Title").UString();
					image.UpdateUser = "admin";

					db.Articles.Add(image);
				}

				db.SaveChanges();
			}
			catch (Exception e)
			{
				
			}

		}

		private int GetColumnID(int type)
		{
			var iii = 0;
			if ((new int[] { 2, 3, 7, 8, 20, 29, 12, 19 }).Contains(type))
			{
				iii = 14;
			}
			else if ((new int[] { 9, 10 }).Contains(type))
			{
				iii = 13;
			}
			else if ((new int[] { 4, 5, 6, 30 }).Contains(type))
			{
				iii = 17;
			}
			else if ((new int[] { 21, 22, 23, 24 }).Contains(type))
			{
				iii = 15;
			}
			else if ((new int[] { 11, 27, 28 }).Contains(type))
			{
				iii = 16;
			}
			else if ((new int[] { 26 }).Contains(type))
			{
				iii = 18;
			}
			else if (type == 13)
			{
				iii = 20;
			}
			else if (type == 16)
			{
				iii = 21;
			}
			else if (type == 14)
			{
				iii = 22;
			}
			else if (type == 17)
			{
				iii = 23;
			}
			else if (type == 15)
			{
				iii = 24;
			}
			else if (type == 18)
			{
				iii = 25;
			}

			return iii;
		}
	}
}
