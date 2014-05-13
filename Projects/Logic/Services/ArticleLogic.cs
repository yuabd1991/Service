using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Logic.DataAccess;
using Component;
using Logic.Models;
using Entity.Entities;
using Component.Component;
using Newtonsoft.Json;

namespace Logic.Services
{
	public class ArticleLogic : DbAccess
	{
		public ArticleLogic()
		{
		}

		#region 热门阅读
		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">0默认所有,1月,2周,3日</param>
		/// <returns></returns>
		public List<HomeHotEntity> GetHomeHotList(string type = "0")
		{
			var list = new List<HomeHotEntity>();
			var artilce = (from l in _db.Articles
						   where l.IsDelete != PublicType.Yes
						   select new HomeHotEntity
						   ()
						   {
							   Date = (DateTime)l.UpdateTime,
							   ID = l.ID,
							   Title = l.Title,
							   Type = 1,
							   Count = (int)l.PageVisits
						   }).ToList();
			var image = (from l in _db.ArticleImages
						 where l.IsDelete != PublicType.Yes
						 select new HomeHotEntity()
						 {
							 Type = 2,
							 Title = l.Title,
							 ID = l.ID,
							 Date = l.DateCreated,
							 Count = l.PageVisits
						 }).ToList();

			list.AddRange(artilce);
			list.AddRange(image);
			if (type == "1")
			{
				list = (from l in list
						where l.Date >= DateTime.Now.AddMonths(-1)
						orderby l.Count descending
						select l).Take(9).ToList();
			}
			else if (type == "2")
			{
				list = (from l in list
						where l.Date >= DateTime.Now.AddDays(-7)
						orderby l.Count descending
						select l).Take(9).ToList();
			}
			else if(type == "3")
			{
				list = (from l in list
						where l.Date >= DateTime.Now.AddDays(-1)
						orderby l.Count descending
						select l).Take(9).ToList();
			}
			else
			{
				list = (from l in list
						orderby l.Count descending
						select l).Take(9).ToList();
			}

			return list;
		}

		#endregion

		#region 文章

		public List<ArticleList> GetArticleList(GetReportDataParams param, out int totalCount)
		{
			DataSet ds = MSSqlHelper.GetReportData("ArticleList", param, out totalCount);
			var dt = ds.Tables[0];
			if (dt == null)
			{
				return new List<ArticleList>();
			}

			List<ArticleList> article = (from l in dt.AsEnumerable()
										 select new ArticleList
										 {
											 ID = l.Field<int>("ID"),
											 ColumnName = l.Field<string>("ColumnName"),
											 PageVisits = l.Field<int>("PageVisits"),
											 UpdateTime = l.Field<DateTime>("UpdateTime"),
											 UpdateUser = l.Field<string>("UpdateUser"),
											 ShortTitle = l.Field<string>("ShortTitle"),
											 SortOrder = l.Field<int>("SortOrder"),
											 Title = l.Field<string>("Title")
										 }).ToList();

			return article.ToList();
		}

		public List<ArticleList> GetHomeArticleList(int id)
		{
			GetReportDataParams param = new GetReportDataParams();
			param.Where.Add(new KeyValue() { Key = "ColumnID", Value = id.UString() });
			DataSet ds = MSSqlHelper.GetReportExportData("ArticleList", param.Where, param.Order);
			var dt = ds.Tables[0];
			if (dt == null)
			{
				return new List<ArticleList>();
			}

			List<ArticleList> article = (from l in dt.AsEnumerable()
										 select new ArticleList
										 {
											 ID = l.Field<int>("ID"),
											 ColumnName = l.Field<string>("ColumnName"),
											 PageVisits = l.Field<int>("PageVisits"),
											 UpdateTime = l.Field<DateTime>("UpdateTime"),
											 UpdateUser = l.Field<string>("UpdateUser"),
											 ShortTitle = l.Field<string>("ShortTitle"),
											 SortOrder = l.Field<int>("SortOrder"),
											 Title = l.Field<string>("Title")
										 }).ToList();

			return article.ToList();
		}

		public BaseObject InsertArticle(ArticleEntity param)
		{
			var obj = new BaseObject();

			if (_db.Connection.State != ConnectionState.Open)
			{
				_db.Connection.Open();
			}

			if (_db.Articles.Any(m => m.IsDelete != PublicType.Yes && m.Title == param.Title))
			{
				obj.Tag = -1;
				obj.Message = "该文章标题已存在！";

				return obj;
			}

			using (var tran = _db.Connection.BeginTransaction())
			{
				try
				{
					var article = new Article()
					{
						ColumnID = param.ColumnID,
						Content = param.Content,
						IsPublic = param.IsPublic,
						MetaDescription = param.MetaDescription,
						MetaKeywords = param.MetaKeywords,
						Overview = param.Overview,
						PageTitle = param.PageTitle,
						PageVisits = 0,
						Slug = param.Slug,
						Title = param.Title,
						UpdateTime = DateTime.Now,
						UpdateUser = param.UpdateUser,
						Source = param.Source,
						SortOrder = param.SortOrder,
						ShortTitle = param.ShortTitle,
						Author = param.Author,
						IsDelete = PublicType.No
					};

					_db.Articles.Add(article);
					_db.SaveChanges();
					tran.Commit();

					obj.Tag = 1;
					obj.Message = "保存成功！";
				}
				catch (Exception e)
				{
					tran.Rollback();
					obj.Tag = -1;
					obj.Message = "保存失败！";
				}
				finally
				{
					tran.Dispose();
				}
			}

			return obj;
		}

		public ArticleEntity GetArticleByID(int id)
		{
			var arts = _db.Articles.Where(m => m.ID == id).ToList();

			if (arts.Count > 0)
			{
				var art = arts.FirstOrDefault();
				art.PageVisits += 1;

				_db.SaveChanges();
			}

			var article = (from l in arts
						   select new ArticleEntity
						   {
							   ColumnID = l.ColumnID,
							   UpdateUser = l.UpdateUser,
							   UpdateTime = l.UpdateTime,
							   Title = l.Title,
							   Slug = l.Slug,
							   PageVisits = l.PageVisits,
							   PageTitle = l.PageTitle,
							   Overview = l.Overview,
							   MetaKeywords = l.MetaKeywords,
							   MetaDescription = l.MetaDescription,
							   IsPublic = l.IsPublic,
							   ID = l.ID,
							   Author = l.Author,
							   ShortTitle = l.ShortTitle,
							   SortOrder = l.SortOrder,
							   Source = l.Source,
							   Content = l.Content
						   }).FirstOrDefault();

			

			return article;
		}

		public BaseObject UpdateArticle(ArticleEntity param)
		{
			var obj = new BaseObject();

			var article = _db.Articles.Find(param.ID);
			if (article == null)
			{
				obj.Tag = -1;
				obj.Message = "记录不存在！";
				return obj;
			}
			if (_db.Articles.Any(m => m.IsDelete != PublicType.Yes && m.Title == param.Title && m.ID != param.ID))
			{
				obj.Tag = -1;
				obj.Message = "该文章标题已存在！";

				return obj;
			}

			article.ColumnID = param.ColumnID;
			article.Content = param.Content;
			article.IsPublic = param.IsPublic;
			article.MetaDescription = param.MetaDescription;
			article.MetaKeywords = param.MetaKeywords;
			article.Overview = param.Overview;
			article.PageTitle = param.PageTitle;
			article.Slug = param.Slug;
			article.Title = param.Title;
			article.UpdateTime = DateTime.Now;
			article.UpdateUser = param.UpdateUser;
			article.Author = param.Author;
			article.ShortTitle = param.ShortTitle;
			article.SortOrder = param.SortOrder;
			article.Source = param.Source;

			_db.Connection.Open();
			using (var tran = _db.Connection.BeginTransaction())
			{
				try
				{
					_db.SaveChanges();

					tran.Commit();
					obj.Tag = 1;
					obj.Message = "保存成功！";
				}
				catch (Exception e)
				{
					tran.Rollback();
					obj.Tag = -1;
					obj.Message = "保存失败！";
				}
				finally
				{
					tran.Dispose();
				}
			}

			return obj;
		}

		public BaseObject DelArticle(int id)
		{
			var obj = new BaseObject();

			var article = _db.Articles.Find(id);

			if (article == null)
			{
				obj.Tag = -1;
				obj.Message = "该记录不存在！";

				return obj;
			}

			_db.Connection.Open();
			using (var tran = _db.Connection.BeginTransaction())
			{
				try
				{
					//_db.Articles.Remove(article);
					article.IsDelete = PublicType.Yes;
					var recycleBin = new RecycleBinEntity()
					{
						Table = RecycleBinTableName.Article,
						UpdateTime = DateTime.Now,
						TargetID = id,
						Name = "文章",
						UpdateUser = "1" //TODO
					};
					new PublicLogic(_db).InsertRecycleBin(recycleBin);

					_db.SaveChanges();


					tran.Commit();
					obj.Tag = 1;
					obj.Message = "删除成功！";
				}
				catch (Exception e)
				{
					tran.Rollback();
					obj.Tag = -1;
					obj.Message = "删除失败！";
				}
				finally
				{
					tran.Dispose();
				}
			}

			return obj;
		}

		#endregion

		#region 图文集

		public List<ArticleImageEntity> GetArticleImageList(GetReportDataParams param)
		{
			DataSet ds = MSSqlHelper.GetReportExportData("ArticleImageList", param.Where, param.Order);
			var dt = ds.Tables[0];
			if (dt == null)
			{
				return new List<ArticleImageEntity>();
			}

			var article = (from l in dt.AsEnumerable()
						   select new ArticleImageEntity
						   {
							   Author = l.Field<string>("Author"),
							   UpdateUser = l.Field<string>("UpdateUser"),
							   Title = l.Field<string>("Title"),
							   Source = l.Field<string>("Source"),
							   SortOrder = l.Field<int>("SortOrder"),
							   Slug = l.Field<string>("Slug"),
							   ShortTitle = l.Field<string>("ShortTitle"),
							   PageVisits = l.Field<int>("PageVisits"),
							   PageTitle = l.Field<string>("PageTitle"),
							   MetaKeywords = l.Field<string>("MetaKeywords"),
							   MetaDescription = l.Field<string>("MetaDescription"),
							   DateCreated = l.Field<DateTime>("DateCreated"),
							   //Content = l.Field<string>("Content"),
							   //IsDelete = l.IsDelete,
							   Overview = l.Field<string>("Overview"),
							   Photo = l.Field<string>("Photo"),
							   ColumnID = l.Field<int>("ColumnID"),
							   ID = l.Field<int>("ID")
						   }).ToList();

			return article.ToList();
		}

		public List<ArticleImageEntity> GetArticleImageList(GetReportDataParams param, out int totalCount)
		{
			DataSet ds = MSSqlHelper.GetReportData("ArticleImageList", param, out totalCount);
			var dt = ds.Tables[0];
			if (dt == null)
			{
				return new List<ArticleImageEntity>();
			}

			var article = (from l in dt.AsEnumerable()
						   select new ArticleImageEntity
						   {
							   Author = l.Field<string>("Author"),
							   UpdateUser = l.Field<string>("UserName"),
							   Title = l.Field<string>("Title"),
							   Source = l.Field<string>("Source"),
							   SortOrder = l.Field<int>("SortOrder"),
							   Slug = l.Field<string>("Slug"),
							   ShortTitle = l.Field<string>("ShortTitle"),
							   PageVisits = l.Field<int>("PageVisits"),
							   PageTitle = l.Field<string>("PageTitle"),
							   MetaKeywords = l.Field<string>("MetaKeywords"),
							   MetaDescription = l.Field<string>("MetaDescription"),
							   DateCreated = l.Field<DateTime>("DateCreated"),
							   //Content = l.Field<string>("Content"),
							   //IsDelete = l.IsDelete,
							   Overview = l.Field<string>("Overview"),
							   Photo = l.Field<string>("Photo"),
							   ColumnID = l.Field<int>("ColumnID"),
							   ID = l.Field<int>("ID")
						   }).ToList();

			//article.ForEach(m => m.Photo =
			//    _db.Pictures.Where(l => l.TargetID == m.ID && l.Type == PictureType.ArticleImage
			//        && l.IsDefault == PublicType.Yes).Select(n => n.PictureUrl).FirstOrDefault());

			return article.ToList();
		}

		private class Photo
		{
			public int ID { get; set; }

			public string Url { get; set; }

			public string IsDefault { get; set; }

			public string PictureUrl { get; set; }
		}

		private BaseObject HandlePicture(ArticleImageEntity param, int id)
		{
			BaseObject obj = new BaseObject();
			var photo = JsonConvert.DeserializeObject<List<Photo>>(param.Photo);

			//TODO

			try
			{
				var ids = photo.Select(q => q.ID).ToList();
				var phos = _db.Pictures.Where(m => m.TargetID == id && !ids.Contains(m.ID) && m.Type == PictureType.ArticleImage).ToList();

				foreach (var item in phos)
				{
					var pho = _db.Pictures.Find(item.ID);
					if (pho == null)
					{
						continue;
					}
					_db.Pictures.Remove(pho);
				}
				_db.SaveChanges();

				foreach (var item in photo)
				{
					var pho = _db.Pictures.Find(item.ID);
					if (pho == null)
					{
						continue;
					}
					pho.IsDefault = item.IsDefault;
					pho.Type = PictureType.ArticleImage;
					pho.TargetID = id;
					pho.PictureUrl = item.PictureUrl;
				}

				_db.SaveChanges();

				obj.Tag = 1;
			}
			catch (Exception e)
			{
				obj.Tag = -1;
			}
			return obj;
		}

		public BaseObject InsertArticleImage(ArticleImageEntity param)
		{
			BaseObject obj = new BaseObject();
			if (_db.ArticleImages.Any(m => m.Title == param.Title))
			{
				obj.Tag = -2;
				obj.Message = "该标题已经存在!";
				return obj;
			}
			try
			{
				var image = new ArticleImage()
				{
					Author = param.Author,
					Content = param.Content,
					DateCreated = DateTime.Now,
					IsDelete = PublicType.No,
					MetaDescription = param.MetaDescription,
					PageTitle = param.PageTitle,
					MetaKeywords = param.MetaKeywords,
					PageVisits = param.PageVisits,
					ShortTitle = param.ShortTitle,
					Slug = param.Slug,
					SortOrder = param.SortOrder,
					Source = param.Source,
					Title = param.Title,
					UpdateUser = param.UpdateUser,
					Overview = param.Overview,
					ColumnID = param.ColumnID
				};

				_db.ArticleImages.Add(image);

				_db.SaveChanges();

				obj.Tag = 1;

				if (HandlePicture(param, image.ID).Tag != 1)
				{
					obj.Tag = -2;
				}
			}
			catch (Exception e)
			{
				obj.Tag = -1;
			}

			return obj;
		}

		public ArticleImageEntity GetArticleImageByID(int id)
		{
			var im = _db.ArticleImages.Where(m => m.ID == id).ToList();
			if (im.Count > 0)
			{
				var i = im.FirstOrDefault();
				i.PageVisits += 1;

				_db.SaveChanges();
			}

			var image = (from l in im
						 select new ArticleImageEntity
						 {
							 Author = l.Author,
							 UpdateUser = l.UpdateUser,
							 Title = l.Title,
							 Source = l.Source,
							 SortOrder = l.SortOrder,
							 Slug = l.Slug,
							 ShortTitle = l.ShortTitle,
							 PageVisits = l.PageVisits,
							 Overview = l.Overview,
							 ColumnID = l.ColumnID,
							 PageTitle = l.PageTitle,
							 MetaKeywords = l.MetaKeywords,
							 MetaDescription = l.MetaDescription,
							 DateCreated = l.DateCreated,
							 Content = l.Content,
							 IsDelete = l.IsDelete,
							 ID = l.ID
						 }).FirstOrDefault();

			if (image == null)
			{
				return image;
			}
			var photos = (from l in _db.Pictures
						  where l.TargetID == image.ID && l.Type == PictureType.ArticleImage
						  orderby l.IsDefault descending
						  select new PictureEntity()
						  {
							  ID = l.ID,
							  IsDefault = l.IsDefault,
							  TargetID = l.TargetID,
							  Type = l.Type,
							  PictureUrl = l.PictureUrl
						  }).ToList();

			var html = "";
			foreach (var item in photos)
			{
				html += ";" + item.ID + "," + item.IsDefault + "," + item.PictureUrl;
			}
			image.Photo = html.Trim(';');
			image.Photos = photos;

			return image;
		}

		public BaseObject UpdateArticleImage(ArticleImageEntity param)
		{
			BaseObject obj = new BaseObject();

			try
			{
				var image = _db.ArticleImages.Find(param.ID);
				if (image == null)
				{
					obj.Tag = -1;
					obj.Message = "该记录不存在!";
					return obj;
				}

				image.Author = param.Author;
				image.Content = param.Content;
				image.DateCreated = DateTime.Now;
				image.MetaDescription = param.MetaDescription;
				image.MetaKeywords = param.MetaKeywords;
				image.PageTitle = param.PageTitle;
				//image.PageVisits = param.PageVisits;
				image.ShortTitle = param.ShortTitle;
				image.Slug = param.Slug;
				image.Overview = param.Overview;
				image.SortOrder = param.SortOrder;
				image.ColumnID = param.ColumnID;
				image.Source = param.Source;
				image.Title = param.Title;
				image.UpdateUser = param.UpdateUser;

				_db.SaveChanges();

                obj.Tag = 1;

				if (HandlePicture(param, image.ID).Tag != 1)
				{
					obj.Tag = -1;
					return obj;
				}
			}
			catch (Exception e)
			{
				obj.Tag = -1;
			}

			return obj;
		}

		public BaseObject DeleteArticleImage(int id)
		{
			var obj = new BaseObject();

			var image = _db.ArticleImages.Find(id);
			if (image == null)
			{
				obj.Tag = -1;
				obj.Message = "该记录不存在!";
				return obj;
			}
			_db.ArticleImages.Remove(image);

			_db.SaveChanges();
			obj.Tag = 1;

			return obj;
		}

		#endregion

		#region 单页文档

		public BaseObject InsertDocument(DocumentEntity param)
		{
			var obj = new BaseObject();

			if (_db.Documents.Any(m => m.Title == param.Title))
			{
				obj.Tag = -1;
				obj.Message = "该页面标题已存在！";

				return obj;
			}
			var article = new Document()
			{
				Content = param.Content,
				MetaDescription = param.MetaDescription,
				MetaKeywords = param.MetaKeywords,
				Overview = param.Overview,
				PageTitle = param.PageTitle,
				PageVisits = 0,
				Slug = param.Slug,
				DateCreated = DateTime.Now,
				UpdateUser = param.UpdateUser,
				Title = param.Title
			};

			_db.Documents.Add(article);
			_db.SaveChanges();

			obj.Tag = 1;
			obj.Message = "保存成功！";


			return obj;
		}

		public DocumentEntity GetDocumentByID(int id)
		{
			var dc = _db.Documents.Where(m => m.ID == id).ToList();
			if (dc.Count > 0)
			{
				var d = dc.FirstOrDefault();
				d.PageVisits += 1;

				_db.SaveChanges();
			}
			var doc = (from l in dc
					   select new DocumentEntity
					   {
						   Content = l.Content,
						   DateCreated = l.DateCreated,
						   ID = l.ID,
						   MetaDescription = l.MetaDescription,
						   MetaKeywords = l.MetaKeywords,
						   Overview = l.Overview,
						   PageTitle = l.PageTitle,
						   PageVisits = l.PageVisits,
						   Slug = l.Slug,
						   Title = l.Title,
						   UpdateUser = l.UpdateUser
					   }).FirstOrDefault();

			return doc;
		}

		public BaseObject UpdateDocument(DocumentEntity param)
		{
			BaseObject obj = new BaseObject();

			var doc = _db.Documents.Find(param.ID);

			if (doc == null)
			{
				obj.Tag = -1;
				obj.Message = "该文档不存在！";
				return obj;
			}

			doc.Content = param.Content;
			doc.DateCreated = DateTime.Now;
			doc.MetaDescription = param.MetaDescription;
			doc.MetaKeywords = param.MetaKeywords;
			doc.Overview = param.Overview;
			doc.PageTitle = param.PageTitle;
			//doc.PageVisits = param.PageVisits; 
			//doc.Slug = param.Slug;
			doc.Title = param.Title;
			doc.UpdateUser = param.UpdateUser;

			_db.SaveChanges();

			obj.Tag = 1;

			return obj;
		}

		public BaseObject DelDocument(int id)
		{
			var obj = new BaseObject();

			var doc = _db.Documents.Find(id);

			if (doc == null)
			{
				obj.Tag = -1;
				obj.Message = "该记录不存在！";

				return obj;
			}

			_db.Documents.Remove(doc);

			_db.SaveChanges();
			obj.Tag = 1;

			return obj;
		}

		public List<DocumentEntity> GetDocumentList(GetReportDataParams param, out int totalCount)
		{
			DataSet ds = MSSqlHelper.GetReportData("DocumentsList", param, out totalCount);
			var dt = ds.Tables[0];
			if (dt == null)
			{
				return new List<DocumentEntity>();
			}

			var article = (from l in dt.AsEnumerable()
						   select new DocumentEntity
						   {
							   Content = l.Field<string>("Content"),
							   ID = l.Field<int>("ID"),
							   MetaDescription = l.Field<string>("MetaDescription"),
							   MetaKeywords = l.Field<string>("MetaKeywords"),
							   Overview = l.Field<string>("Overview"),
							   PageTitle = l.Field<string>("PageTitle"),
							   PageVisits = l.Field<int?>("PageVisits"),
							   Slug = l.Field<string>("Slug"),
							   Title = l.Field<string>("Title"),
							   UpdateUser = l.Field<string>("UpdateUser"),
							   DateCreated = l.Field<DateTime>("DateCreated")
						   }).ToList();

			return article;
		}

		#endregion
	}
}
