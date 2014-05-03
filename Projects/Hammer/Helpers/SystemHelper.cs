using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Component;
using Logic.DataAccess;
using Entity.Entities;
using Logic.Services;

namespace EasyUI.Helpers
{
	public class SystemHelper : DbAccess
	{
		public SystemHelper()
		{
		}

		#region Menus

		public List<MenuEntity> GetAllMenus()
		{
			using (MenuLogic logic = new MenuLogic())
			{
				return logic.GetAllMenus();
			}
		}

		public List<MenuEntity> GetAllPages()
		{
			using (MenuLogic logic = new MenuLogic())
			{
				return logic.GetAllPages();
			}
		}

		/// <summary>
		/// 页面获取
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PageEntity GetPageByID(int id)
		{
			using (MenuLogic logic = new MenuLogic())
			{
				return logic.GetPageByID(id);
			}
		}
		/// <summary>
		/// 修改页面
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		public BaseObject EditPage(MenuEntity param)
		{
			using (MenuLogic logic = new MenuLogic())
			{
				return logic.EditPage(param);
			}
		}

		public BaseObject InsertMenu(MenuEntity menu)
		{
			using (MenuLogic logic = new MenuLogic())
			{
				return logic.InsertMenu(menu);
			}
		}

		public MenuEntity GetMenuByID(int id)
		{
			using (MenuLogic logic = new MenuLogic())
			{
				return logic.GetMenuByID(id);
			}
		}

		public BaseObject EditMenu(MenuEntity menu)
		{
			using (MenuLogic logic = new MenuLogic())
			{
				return logic.EditMenu(menu);
			}
		}

		public BaseObject DelMenu(int id)
		{
			using (MenuLogic logic = new MenuLogic())
			{
				return logic.DelMenu(id);
			}
		}

		public List<MenuList> GetMenus(string ids)
		{
			using (MenuLogic logic = new MenuLogic())
			{
				return logic.GetMenus(ids);
			}
		}

		public List<MenuTree> GetTreeMenu()
		{
			using (MenuLogic logic = new MenuLogic())
			{
				return logic.GetTreeMenu();
			}
		}

        public BaseObject DisEnableMenu(int id, string state, string type)
        {
            using (MenuLogic logic = new MenuLogic())
            {
                return logic.DisEnableMenu(id, state, type);
            }
        }

		#endregion

        #region 单页文档

        public BaseObject InsertDocument(DocumentEntity param)
        {
            using (ArticleLogic logic = new ArticleLogic())
            {
                return logic.InsertDocument(param);
            }
        }

        public DocumentEntity GetDocumentByID(int id)
        {
            using (ArticleLogic logic = new ArticleLogic())
            {
                return logic.GetDocumentByID(id);
            }
        }

        public BaseObject UpdateDocument(DocumentEntity param)
        {
            using (ArticleLogic logic = new ArticleLogic())
            {
                return logic.UpdateDocument(param);
            }
        }

        public BaseObject DelDocument(int id)
        {
            using (ArticleLogic logic = new ArticleLogic())
            {
                return logic.DelDocument(id);
            }
        }

        public List<DocumentEntity> GetDocumentList(GetReportDataParams param, out int totalCount)
        {
            using (ArticleLogic logic = new ArticleLogic())
            {
                return logic.GetDocumentList(param, out totalCount);
            }
        }


        #endregion

        #region Article

		public List<ArticleList> GetArticleList(GetReportDataParams param, out int totalCount)
		{
			using (ArticleLogic logic = new ArticleLogic())
			{
				return logic.GetArticleList(param, out totalCount);
			}
		}

        public List<ArticleList> GetHomeArticleList(int id)
        {
            using (ArticleLogic logic = new ArticleLogic())
            {
                return logic.GetHomeArticleList(id);
            }
        }

		public BaseObject InsertArticle(ArticleEntity param)
		{
			using (ArticleLogic logic = new ArticleLogic())
			{
				return logic.InsertArticle(param);
			}
		}

		public ArticleEntity GetArticleByID(int id)
		{
			using (ArticleLogic logic = new ArticleLogic())
			{
				return logic.GetArticleByID(id);
			}
		}

		public BaseObject UpdateArticle(ArticleEntity param)
		{
			using (ArticleLogic logic = new ArticleLogic())
			{
				return logic.UpdateArticle(param);
			}
		}

		public BaseObject DelArticle(int id)
		{
			using (ArticleLogic logic = new ArticleLogic())
			{
				return logic.DelArticle(id);
			}
		}

		public List<HomeHotEntity> GetHomeHotList(string type)
		{
			using (ArticleLogic logic = new ArticleLogic())
			{
				return logic.GetHomeHotList(type);
			}
		}

		#endregion

		#region ArticleImage

		public List<ArticleImageEntity> GetArticleImageList(int id)
		{
			using (ArticleLogic logic = new ArticleLogic())
			{
				GetReportDataParams param = new GetReportDataParams();
				List<KeyValue> where = new List<KeyValue>();
				where.Add(new KeyValue { Key = "ColumnID", Value = id.ToString() });

				param.PageIndex = 1;
				param.PageSize = 100;
				//param.Order = ;
				param.Where = where;

				//return logic.GetArticleImageList(param, out tCount);

				return logic.GetArticleImageList(param);
			}
		}

		public List<ArticleImageEntity> GetArticleImageList(GetReportDataParams param, out int totalCount)
		{
			using (ArticleLogic logic = new ArticleLogic())
			{
				return logic.GetArticleImageList(param, out totalCount);
			}
		}

		//public List<ArticleImageEntity> GetArticleImageList(int id)
		//{
		//    using (ArticleLogic logic = new ArticleLogic())
		//    {
		//        int tCount = 0;
		//        GetReportDataParams param = new GetReportDataParams();
		//        List<KeyValue> where = new List<KeyValue>();
		//        where.Add(new KeyValue { Key = "ColumnID", Value = id.ToString() });

		//        param.PageIndex = 1;
		//        param.PageSize = 100;
		//        //param.Order = ;
		//        param.Where = where;

		//        return logic.GetArticleImageList(param, out tCount);
		//    }
		//}

		public BaseObject InsertArticleImage(ArticleImageEntity param)
		{
			using (ArticleLogic logic = new ArticleLogic())
			{
				return logic.InsertArticleImage(param);
			}
		}

		public ArticleImageEntity GetArticleImageByID(int id)
		{
			using (ArticleLogic logic = new ArticleLogic())
			{
				return logic.GetArticleImageByID(id);
			}
		}

		public BaseObject UpdateArticleImage(ArticleImageEntity param)
		{
			using (ArticleLogic logic = new ArticleLogic())
			{
				return logic.UpdateArticleImage(param);
			}
		}

		public BaseObject DeleteArticleImage(int id)
		{
			using (ArticleLogic logic = new ArticleLogic())
			{
				return logic.DeleteArticleImage(id);
			}
		}

		#endregion

		#region 栏目

		public List<ColumnList> GetColumnList(GetReportDataParams param, out int totalCount)
		{
			using (ColumnLogic logic = new ColumnLogic(_db))
			{
				return logic.GetColumnList(param, out totalCount);
			}
		}

        public List<ColumnEntity> GetMenuHelper()
        {
            using (ColumnLogic logic = new ColumnLogic(_db))
            {
                return logic.GetMenuHelper();
            }
        }

		public BaseObject InsertColumn(ColumnEntity param)
		{
			using (ColumnLogic logic = new ColumnLogic(_db))
			{
				return logic.InsertColumn(param);
			}
		}

		public ColumnEntity GetColumnById(int id)
		{
			using (ColumnLogic logic = new ColumnLogic(_db))
			{
				return logic.GetColumnById(id);
			}
		}

		public BaseObject UpdateColumn(ColumnEntity param)
		{
			using (ColumnLogic logic = new ColumnLogic(_db))
			{
				return logic.UpdateColumn(param);
			}
		}

		public BaseObject DelColumn(string id)
		{
			using (ColumnLogic logic = new ColumnLogic(_db))
			{
				return logic.DelColumn(id);
			}
		}
		/// <summary>
		/// 栏目下拉框
		/// </summary>
		/// <param name="columnID"></param>
		/// <returns></returns>
		public List<KeyName> GetColumnDropList(int? columnID, int tempID)
		{
			using (ColumnLogic logic = new ColumnLogic(_db))
			{
				return logic.GetColumnDropList(columnID, tempID);
			}
		}

		#endregion

		#region 内容模板

		public List<ContentTemplateEntity> GetContentTemplateList(GetReportDataParams param, out int totalCount)
		{
			using (ColumnLogic logic = new ColumnLogic(_db))
			{
				return logic.GetContentTemplateList(param, out totalCount);
			}
		}

		public ContentTemplateEntity GetContentTempById(int id)
		{
			using (ColumnLogic logic = new ColumnLogic(_db))
			{
				return logic.GetContentTempById(id);
			}
		}

		public BaseObject EditContentTemp(ContentTemplateEntity param)
		{
			using (ColumnLogic logic = new ColumnLogic(_db))
			{
				return logic.EditContentTemp(param);
			}
		}

		public List<KeyName> GetContentComboList()
		{
			using (ColumnLogic logic = new ColumnLogic(_db))
			{
				return logic.GetContentTempList();
			}
		}
		#endregion

		#region Links

		#region 分类

		public List<KeyName> GetLinkCategoryList()
		{
			using (LinkLogic logic = new LinkLogic())
			{
				return logic.GetLinkCategoryList();
			}
		}

		public BaseObject InsertLinkCategory(LinkCategoryEntity link)
		{
			using (LinkLogic logic = new LinkLogic())
			{
				return logic.InsertLinkCategory(link);
			}
		}

		public LinkCategoryEntity GetLinkCategory(int id)
		{
			using (LinkLogic logic = new LinkLogic())
			{
				return logic.GetLinkCategory(id);
			}
		}

		public BaseObject UpdateLinkCategory(LinkCategoryEntity category)
		{
			using (LinkLogic logic = new LinkLogic())
			{
				return logic.UpdateLinkCategory(category);
			}
		}

		public BaseObject DeleteLinkCategory(int id)
		{
			using (LinkLogic logic = new LinkLogic())
			{
				return logic.DeleteLinkCategory(id);
			}
		}

		public List<LinkCategoryEntity> GetLinkCategories()
		{
			using (LinkLogic logic = new LinkLogic())
			{
				return logic.GetLinkCategories();
			}
		}

		#endregion

		#region 友情链接

		public BaseObject InsertLink(LinksEntity link)
		{
			using (LinkLogic logic = new LinkLogic())
			{
				return logic.InsertLink(link);
			}
		}

		public LinksEntity GetLink(int id)
		{
			using (LinkLogic logic = new LinkLogic())
			{
				return logic.GetLink(id);
			}
		}

		public BaseObject UpdateLink(LinksEntity link)
		{
			using (LinkLogic logic = new LinkLogic())
			{
				return logic.UpdateLink(link);
			}
		}

		public BaseObject DeleteLink(int id)
		{
			using (LinkLogic logic = new LinkLogic())
			{
				return logic.DeleteLink(id);
			}
		}

        public List<LinksEntity> GetLinks(GetReportDataParams param, out int totalCount)
		{
			using (LinkLogic logic = new LinkLogic())
			{
				return logic.GetLinks(param, out totalCount);
			}
		}

		public List<LinksEntity> GetLinksForHome()
		{
			using (LinkLogic logic = new LinkLogic())
			{
				return logic.GetLinksForHome();
			}
		}

		#endregion

		#endregion

		#region 系统设置

		public SysConfig GetSystemConfig()
		{
			using (SysConfigLogic logic = new SysConfigLogic())
			{
				return logic.GetSystemConfig();
			}
		}

		public BaseObject SetSystemConfig(SysConfig param)
		{
			using (SysConfigLogic logic = new SysConfigLogic())
			{
				return logic.SetSystemConfig(param);
			}
		}

		#endregion

		#region 图片

		public BaseObject<int> InsertPicture(PictureEntity param)
		{
			using (PictureLogic logic = new PictureLogic())
			{
				return logic.InsertPicture(param);
			}
		}

		#endregion

		#region 用户管理

		public List<UserEntity> GetUserListReport(GetReportDataParams param, out int totalCount)
		{
			using (UserLogic logic = new UserLogic())
			{
				return logic.GetUserListReport(param, out totalCount);
			}
		}

		#region 角色

		public List<UserRoleEntity> GetUserRoleList(GetReportDataParams param, out int totalCount)
		{
			using (UserLogic logic = new UserLogic())
			{
				return logic.GetUserRoleList(param, out totalCount);
			}
		}

		public BaseObject InsertUserRole(UserRoleEntity param)
		{
			using (UserLogic logic = new UserLogic())
			{
				return logic.InsertUserRole(param);
			}
		}

		public UserRoleEntity GetUserRoleByID(int id)
		{
			using (UserLogic logic = new UserLogic())
			{
				return logic.GetUserRoleByID(id);
			}
		}

		public BaseObject UpdateUserRole(UserRoleEntity param)
		{
			using (UserLogic logic = new UserLogic())
			{
				return logic.UpdateUserRole(param);
			}
		}

		public BaseObject DeleteUserRole(int id)
		{
			using (UserLogic logic = new UserLogic())
			{
				return logic.DeleteUserRole(id);
			}
		}

		#endregion

		#region 权限

		public string GetPermissionList(int id)
		{
			using (UserLogic logic = new UserLogic())
			{
				return logic.GetPermissionList(id);
			}
		}

		#endregion

		#endregion
	}
}