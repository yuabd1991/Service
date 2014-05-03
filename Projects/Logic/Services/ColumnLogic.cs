using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Logic.DataAccess;
using Logic.Models;
using Component;
using Entity.Entities;
using Component.Component;

namespace Logic.Services
{
	public class ColumnLogic: DbAccess
	{
		public ColumnLogic(DataAccess.SiteContext db)
			: base(db)
		{
		}

		#region 栏目管理

		public List<ColumnList> GetColumnList(GetReportDataParams param, out int totalCount)
		{
			DataSet ds = MSSqlHelper.GetReportData("ColumnList", param, out totalCount);
			var dt = ds.Tables[0];
			if (dt == null)
			{
				return new List<ColumnList>();
			}

			var article = (from l in dt.AsEnumerable()
						   select new ColumnList
						   {
							   ColumuName = l.Field<string>("ColumnName"),
							   ID = l.Field<int>("ID"),
							   IsPublic = l.Field<string>("IsPublic"),
							   AdminListUrl = l.Field<string>("AdminListUrl"),
							   ContentTemplateID = l.Field<int>("ContentTemplateID"),
							   ParentID = l.Field<int?>("ParentID"),
							   SortOrder = l.Field<int>("SortOrder"),
							   UpdateDate = l.Field<DateTime>("UpdateDate"),
							   UpdateUser = l.Field<string>("UserName"),
						   }).ToList();

			return article;
		}

		public BaseObject InsertColumn(ColumnEntity param)
		{
			BaseObject obj = new BaseObject();

			if (param == null)
			{
				obj.Tag = -1;
				obj.Message = "参数错误！";
				return obj;
			}

            if (_db.Columns.Any(m => m.ColumnName == param.ColumnName && m.IsDelete != PublicType.Yes))
            {
                obj.Tag = -1;
                obj.Message = "该栏目名称已被使用！";

                return obj;
            }

			if (_db.Connection.State != ConnectionState.Open)
			{
				_db.Connection.Open();
			}

			using (var tran = _db.Connection.BeginTransaction())
			{
				try
				{
					var content = _db.ContentTemplates.Find(param.ContentTemplateID);
					//文章类型、还是图文集、或者相册

					var column = new Column
						{
							//AdminDelUrl = content.AdminDelUrl,
							//AdminEditUrl = content.AdminEditUrl,
							ColumnName = param.ColumnName,
							ParentID = param.ParentID, 
							ContentTemplateID = param.ContentTemplateID,
							IsPublic = param.IsPublic,
							MetaDescription = param.MetaDescription,
							MetaKeywords = param.MetaKeywords,
							PageTitle = param.PageTitle,
							SortOrder = param.SortOrder,
							UpdateDate = DateTime.Now,
							UpdateUser = param.UpdateUser,
							WebDetailUrl = content.WebDetailUrl,
                            WebListUrl = content.WebListUrl,
                            IsDelete = PublicType.No,
                            PageID = param.PageID,
                            Slug = param.Slug
						};

					_db.Columns.Add(column);
					_db.SaveChanges();

					column.AdminListUrl = content.AdminListUrl + "?columnID=" + column.ID;
					//column.AdminAddUrl = content.AdminAddUrl + "?columnID=" + column.ID;

					_db.SaveChanges();

					tran.Commit();
					obj.Tag = 1;
					obj.Message = "新增成功！";
				}
				catch (Exception e)
				{
					tran.Rollback();
					obj.Tag = -2;
					obj.Message = e.Message;
				}
				finally
				{
					tran.Connection.Close();
				}
			}

			return obj;
		}

		public ColumnEntity GetColumnById(int id)
		{
			var column = (from l in _db.Columns
			             where l.ID == id
			             select new ColumnEntity()
				             {
                                 ID = l.ID,
                                 Slug = l.Slug,
                                 IsDelete = PublicType.No,
                                 PageID = l.PageID,
								 AdminListUrl = l.AdminListUrl,
								 ColumnName = l.ColumnName,
								 ContentTemplateID = l.ContentTemplateID,
								 IsPublic = l.IsPublic,
								 MetaDescription = l.MetaDescription,
								 MetaKeywords = l.MetaKeywords,
								 PageTitle = l.PageTitle,
								 SortOrder = l.SortOrder,
								 ParentID = l.ParentID,
								 UpdateDate = l.UpdateDate,
								 UpdateUser = l.UpdateUser,
								 WebDetailUrl = l.WebDetailUrl,
								 WebListUrl = l.WebListUrl
				             }).FirstOrDefault();

			return column;
		}

		public BaseObject UpdateColumn(ColumnEntity param)
		{
			BaseObject obj = new BaseObject();

            if (_db.Columns.Any(m => m.ColumnName == param.ColumnName && m.ID != param.ID && m.IsDelete != PublicType.Yes))
            {
                obj.Tag = -1;
                obj.Message = "该栏目名称已被使用！";

                return obj;
            }

			if (_db.Connection.State != ConnectionState.Open)
			{
				_db.Connection.Open();
			}

			using (var tran = _db.Connection.BeginTransaction())
			{
				try
				{
                    var content = _db.ContentTemplates.Find(param.ContentTemplateID);

					var dbColumn = _db.Columns.Find(param.ID);

                    dbColumn.AdminListUrl = content.AdminListUrl + "?columnID=" + param.ID;
					dbColumn.ColumnName = param.ColumnName;
					dbColumn.ContentTemplateID = param.ContentTemplateID;
					dbColumn.IsPublic = param.IsPublic;
					dbColumn.MetaDescription = param.MetaDescription;
					dbColumn.MetaKeywords = param.MetaKeywords;
					dbColumn.PageTitle = param.PageTitle;
					dbColumn.SortOrder = param.SortOrder;
					dbColumn.ParentID = param.ParentID;
					dbColumn.UpdateDate = DateTime.Now;
					dbColumn.UpdateUser = param.UpdateUser;
					dbColumn.WebDetailUrl = content.WebDetailUrl;
                    dbColumn.WebListUrl = content.WebListUrl;
                    dbColumn.PageID = param.PageID;
                    dbColumn.Slug = param.Slug;

					_db.SaveChanges();
					tran.Commit();
					obj.Tag = 1;
					obj.Message = "保存成功！";
				}
				catch (Exception e)
				{
					obj.Tag = -1;
					obj.Message = e.Message;
					tran.Rollback();
                    throw new Exception(e.Message);
				}
				finally
				{
					_db.Connection.Close();
				}
			}

			return obj;
		}

		public BaseObject DelColumn(string id)
		{
			var obj = new BaseObject();

            var ids = id.Split(';');

			if (_db.Connection.State != ConnectionState.Open)
			{
				_db.Connection.Open();
			}
			using (var tran = _db.Connection.BeginTransaction())
			{
				try
				{
                    foreach (var item in ids)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            var i = Convert.ToInt32(item);

                            var column = _db.Columns.FirstOrDefault(m => m.ID == i);

                            if (column == null)
                            {
                                continue;
                            }
                            column.IsDelete = PublicType.Yes;
                        }
                    }

					_db.SaveChanges();
					tran.Commit();
					obj.Tag = 1;
					obj.Message = "删除成功";
				}
				catch (Exception e)
				{
					tran.Rollback();
					obj.Tag = -1;
					obj.Message = e.Message;
					throw;
				}
				finally
				{
					_db.Connection.Close();
				}
			}

			return obj;
		}

        public List<KeyName> GetColumnDropList(int? columnID, int tempID)
        {
            var list = new List<KeyName>();

            list = (from l in _db.Columns
                    orderby l.SortOrder
					where l.IsDelete != PublicType.Yes && l.ContentTemplateID == tempID
                    select new KeyName()
                        {
                            ID = l.ID,
                            Name = l.ColumnName
                        }).ToList();

            if (columnID != 0)
            {
                list = list.Where(m => m.ID == columnID).ToList();
            }

            return list;
        }

        public List<ColumnEntity> GetMenuHelper()
        {
            var list = from l in _db.Columns
                       where l.IsPublic == PublicType.Yes && l.IsDelete != PublicType.Yes
					   orderby l.SortOrder
                       select new ColumnEntity()
                       {
                           ColumnName = l.ColumnName,
                           ContentTemplateID = l.ContentTemplateID,
                           ID = l.ID,
                           PageID = l.PageID,
                           WebDetailUrl = l.WebDetailUrl,
                           WebListUrl = l.WebListUrl
                       };

            return list.ToList();
        }

		#endregion

		#region 内容模板

		public List<ContentTemplateEntity> GetContentTemplateList(GetReportDataParams param, out int totalCount)
		{
			var columns = (from l in _db.ContentTemplates
                           select new ContentTemplateEntity
				               {
					               ID = l.ID,
					               UpdateUser = "",
					               ContentName = l.ContentName,
					               Enable = l.Enable,
					               UpdateDate = DateTime.Now
				               }).ToList();

			//查询条件
			columns = param.Where.Where(column => column.Key == "ContentName")
							   .Aggregate(columns, (current, column) => (from l in current
																		 where l.ContentName.Contains(column.Value)
																		 select l).ToList());
			//总记录
			totalCount = columns.Count();
			//分页取值
            var col = new DataGridHelper<ContentTemplateEntity>().GetResult(columns, param);

			return col;
		}

        public ContentTemplateEntity GetContentTempById(int id)
		{
			var content = (from l in _db.ContentTemplates
			              where l.ID == id
			              select new ContentTemplateEntity
				              {
					              //AdminAddUrl = l.AdminAddUrl,
					              //AdminDelUrl = l.AdminDelUrl,
					              //AdminEditUrl = l.AdminEditUrl,
					              AdminListUrl = l.AdminListUrl,
					              ContentName = l.ContentName,
					              Enable = l.Enable,
					              UpdateDate = l.UpdateDate,
					              ID = l.ID,
					              WebDetailUrl = l.WebDetailUrl,
					              WebListUrl = l.WebListUrl
				              }).FirstOrDefault();

			return content;
		}

        public BaseObject EditContentTemp(ContentTemplateEntity param)
		{
			BaseObject obj = new BaseObject();
			var content = _db.ContentTemplates.Find(param.ID);
			if (content == null)
			{
				obj.Tag = -1;
				obj.Message = "该记录不存在！";
			}

			if (_db.Connection.State != ConnectionState.Open)
			{
				_db.Connection.Open();
			}
			using (var tran = _db.Connection.BeginTransaction())
			{
				try
				{
					//content.AdminAddUrl = param.AdminAddUrl;
					//content.AdminDelUrl = param.AdminDelUrl;
					//content.AdminEditUrl = param.AdminEditUrl;
					content.AdminListUrl = param.AdminListUrl;
					content.ContentName = param.ContentName;
					content.UpdateDate = DateTime.Now;
					content.UpdateUser = param.UpdateUser;
					content.WebDetailUrl = param.WebDetailUrl;
					content.WebListUrl = param.WebListUrl;
					content.Enable = param.Enable;

					_db.SaveChanges();
					tran.Commit();
					obj.Tag = 1;
				}
				catch (Exception e)
				{
					tran.Rollback();
					obj.Tag = -2;
					obj.Message = e.Message;
				}
				finally
				{
					_db.Connection.Close();
				}
			}

			return obj;
		}

		public List<KeyName> GetContentTempList()
		{
			var contents = (from l in _db.ContentTemplates
			               where l.Enable == "Y"
			               select new KeyName()
				               {
					               ID = l.ID,
					               Name = l.ContentName
				               }).ToList();
			return contents;
		}

		#endregion

        #region 侧边栏管理

        public List<SidebarEntity> GetSidebarList(GetReportDataParams param, out int totalCount)
        {
			DataSet ds = MSSqlHelper.GetReportData("SidebarList", param, out totalCount);
            var dt = ds.Tables[0];
            if (dt == null)
            {
                return new List<SidebarEntity>();
            }

            var article = (from l in dt.AsEnumerable()
                           select new SidebarEntity
                           {
                               UpdateUser = l.Field<string>("UpdateUser"),
                               Title = l.Field<string>("Title"),
                               Type = l.Field<string>("Type"),
                               UpdateDate = l.Field<DateTime>("UpdateDate"),
                               ColumnID = l.Field<int>("ColumnID"),
                               ID = l.Field<int>("ID")
                           }).ToList();

            return article.ToList();
        }

		//public BaseObject InsertSidebar(SidebarEntity param)
		//{
		//    var obj = new BaseObject();
		//    try
		//    {
		//        var sidebar = new Sidebar();
		//        sidebar.ColumnID = param.ColumnID;
		//        sidebar.Content = param.Content;
		//        sidebar.Title = param.Title;
		//        sidebar.Type = param.Type;
		//        sidebar.UpdateDate = param.UpdateDate;
		//        sidebar.UpdateUser = param.UpdateUser;

		//        _db.Sidebars.Add(sidebar);
		//        _db.SaveChanges();
		//        obj.Tag = 1;
		//    }
		//    catch (Exception e)
		//    {
		//        obj.Tag = 1;
		//        obj.Message = e.Message;
		//    }

		//    return obj;
		//}

		//public SidebarEntity GetSidebarByID(int id)
		//{
		//    var side = (from l in _db.Sidebars
		//               where l.ID == id
		//               select new SidebarEntity
		//               {
		//                   ColumnID = l.ColumnID,
		//                   Content = l.Content,
		//                   UpdateUser = l.UpdateUser,
		//                   UpdateDate = l.UpdateDate,
		//                   Type = l.Type,
		//                   Title = l.Title,
		//                   ID = l.ID
		//               }).FirstOrDefault();

		//    return side;
		//}

		//public BaseObject UpdateSidebar(SidebarEntity param)
		//{
		//    var obj = new BaseObject();

		//    var sidebar = _db.Sidebars.Find(param.ID);
		//    if (sidebar == null)
		//    {
		//        obj.Tag = -1;
		//        obj.Message = "记录不存在!";
		//        return obj;
		//    }
		//    try
		//    {
		//        sidebar.ColumnID = param.ColumnID;
		//        sidebar.Content = param.Content;
		//        sidebar.Title = param.Title;
		//        sidebar.Type = param.Type;
		//        sidebar.UpdateDate = param.UpdateDate;
		//        sidebar.UpdateUser = param.UpdateUser;

		//        _db.SaveChanges();
		//        obj.Tag = 1;
		//    }
		//    catch (Exception e)
		//    {
		//        obj.Tag = -2;
		//        obj.Message = e.Message;
		//    }

		//    return obj;
		//}

		//public BaseObject DeleteSidebar(int id)
		//{
		//    var obj = new BaseObject();
		//    var sidebar = _db.Sidebars.Find(id);

		//    if (sidebar == null)
		//    {
		//        obj.Tag = -1;
		//        obj.Message = "该记录不存在!";
		//        return obj;
		//    }

		//    _db.Sidebars.Remove(sidebar);
		//    _db.SaveChanges();
		//    obj.Tag = 1;

		//    return obj;
		//}

        #endregion
	}
}
