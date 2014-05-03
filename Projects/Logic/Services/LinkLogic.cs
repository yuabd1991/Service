using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Logic.DataAccess;
using Component;
using Logic.Models;
using Entity.Entities;


namespace Logic.Services
{
	public class LinkLogic : DbAccess
	{
		public LinkLogic()
		{
		}

		#region 友情链接分类

		public List<KeyName> GetLinkCategoryList()
		{
			var list = (from l in _db.LinkCategories
						select new KeyName()
						{
							ID = l.ID,
							Name = l.Name,
						}).ToList();

			return list;
		}

		public BaseObject InsertLinkCategory(LinkCategoryEntity link)
		{
			BaseObject obj = new BaseObject();
			try
			{
				var category = new LinkCategory()
					{
						Name = link.Name,
						SortOrder = link.SortOrder,
						DateCreated = DateTime.Now,
						UpdateUser = link.UpdateUser
					};

				_db.LinkCategories.Add(category);
				_db.SaveChanges();

				obj.Tag = 1;
			}
			catch (Exception)
			{
				obj.Tag = -1;
			}

			return obj;
		}

		public LinkCategoryEntity GetLinkCategory(int id)
		{
			var category = (from l in _db.LinkCategories
							where l.ID == id
							select new LinkCategoryEntity()
								{
									ID = l.ID,
									Name = l.Name,
									SortOrder = l.SortOrder,
									UpdateUser = l.UpdateUser,
									DateTime = l.DateCreated
								}).FirstOrDefault();

			return category;
		}

		public BaseObject UpdateLinkCategory(LinkCategoryEntity category)
		{
			BaseObject obj = new BaseObject();

			var c = _db.LinkCategories.Find(category.ID);

			if (c == null)
			{
				obj.Tag = -1;

				return obj;
			}
			c.Name = category.Name;
			c.SortOrder = category.SortOrder;
			c.DateCreated = DateTime.Now;
			c.UpdateUser = category.UpdateUser;

			_db.SaveChanges();
			obj.Tag = 1;

			return obj;
		}

		public BaseObject DeleteLinkCategory(int id)
		{
			BaseObject obj = new BaseObject();
			var link = _db.LinkCategories.Find(id);
			if (link == null)
			{
				obj.Tag = -1;
				obj.Message = "该记录不存在!";
				return obj;
			}

			_db.LinkCategories.Remove(link);

			obj.Tag = 1;

			return obj;
		}

		public List<LinkCategoryEntity> GetLinkCategories()
		{
			var list = (from l in _db.LinkCategories
						select new LinkCategoryEntity()
							{
								ID = l.ID,
								Name = l.Name,
								SortOrder = l.SortOrder,
								UpdateUser = l.UpdateUser,
								DateTime = l.DateCreated
							}).ToList();

			return list;
		}

		#endregion

		#region 友情链接

		public BaseObject InsertLink(LinksEntity link)
		{
			BaseObject obj = new BaseObject();

			if (_db.Connection.State != ConnectionState.Open)
			{
				_db.Connection.Open();
			}

			using (var tran = _db.Connection.BeginTransaction())
			{
				try
				{
					var lin = new Links()
						{
							Contact = link.Contact,
							Description = link.Description,
							Email = link.Email,
							LinkCategoryID = link.LinkCategoryID,
							LinkUrl = link.LinkUrl,
							Name = link.Name,
							PictureFile = link.PictureFile,
							SortOrder = link.SortOrder,
							DateCreated = DateTime.Now,
							UpdateUser = link.UpdateUser
						};

					_db.Links.Add(lin);
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

		public LinksEntity GetLink(int id)
		{
			var link = (from l in _db.Links
						join m in _db.LinkCategories on l.LinkCategoryID equals m.ID
						where l.ID == id
						select new LinksEntity()
						{
							Contact = l.Contact,
							LinkCategoryID = l.LinkCategoryID,
							Description = l.Description,
							Email = l.Email,
							ID = l.ID,
							LinkCategory = m.Name,
							Name = l.Name,
							LinkUrl = l.LinkUrl,
							PictureFile = l.PictureFile,
							SortOrder = l.SortOrder,
							DateCreated = l.DateCreated,
							UpdateUser = l.UpdateUser
						}).FirstOrDefault();

			return link;
		}

		public BaseObject UpdateLink(LinksEntity link)
		{
			BaseObject obj = new BaseObject();
			var l = _db.Links.Find(link.ID);

			if (l == null)
			{
				obj.Tag = -1;
				obj.Message = "该记录不存在！";
				return obj;
			}

			try
			{
				l.Contact = link.Contact;
				l.Description = link.Description;
				l.Email = link.Email;
				l.Name = link.Name;
				l.LinkUrl = link.LinkUrl;
				l.PictureFile = link.PictureFile;
				l.SortOrder = link.SortOrder;
				l.UpdateUser = link.UpdateUser;
				l.DateCreated = DateTime.Now;
				l.LinkCategoryID = link.LinkCategoryID;

				_db.SaveChanges();

				obj.Tag = 1;
			}
			catch (Exception)
			{
				obj.Tag = -1;
				obj.Message = "修改失败！";
			}

			return obj;
		}

		public BaseObject DeleteLink(int id)
		{
			BaseObject obj = new BaseObject();
			var link = _db.Links.Find(id);
			if (link == null)
			{
				obj.Tag = -1;
				obj.Message = "该记录不存在!";
				return obj;
			}

			_db.Links.Remove(link);
			_db.SaveChanges();

			obj.Tag = 1;

			return obj;
		}

		public List<LinksEntity> GetLinks(GetReportDataParams param, out int totalCount)
		{

			var list = (from l in _db.Links
						join m in _db.LinkCategories on l.LinkCategoryID equals m.ID
						select new LinksEntity()
						{
							Contact = l.Contact,
							Description = l.Description,
							Email = l.Email,
							ID = l.ID,
							LinkCategory = m.Name,
							Name = l.Name,
							LinkUrl = l.LinkUrl,
							PictureFile = l.PictureFile,
							SortOrder = l.SortOrder,
							DateCreated = l.DateCreated,
							UpdateUser = l.UpdateUser
						}).ToList();

			#region 查询条件

			list = param.Where.Where(m => m.Key.ToLower() == "name")
						   .Aggregate(list,
									  (current, column) =>
									  (from l in current where l.Name.Contains(column.Value) select l).ToList());

			list = param.Where.Where(m => m.Key.ToLower() == "startdate")
						   .Aggregate(list,
									  (current, column) =>
									  (from l in current where l.DateCreated >= Convert.ToDateTime(column.Value) select l).ToList());

			list = param.Where.Where(m => m.Key.ToLower() == "enddate")
						   .Aggregate(list,
									  (current, column) =>
									  (from l in current where l.DateCreated <= Convert.ToDateTime(column.Value) select l).ToList());

			#endregion


			totalCount = list.Count();

			return list.ToList();
		}

		public List<LinksEntity> GetLinksForHome()
		{
			var links = from l in _db.Links
						 orderby l.SortOrder
						 select new LinksEntity
						 {
							 Contact = l.Contact,
							 Description = l.Description,
							 Email = l.Email,
							 ID = l.ID,
							 //LinkCategory = l.Name,
							 Name = l.Name,
							 LinkUrl = l.LinkUrl,
							 PictureFile = l.PictureFile,
							 SortOrder = l.SortOrder,
							 DateCreated = l.DateCreated,
							 UpdateUser = l.UpdateUser
						 };

			return links.ToList();
		}

		#endregion

	}
}
