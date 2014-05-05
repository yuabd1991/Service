using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Logic.Models
{
	#region 菜单

	public class Menu
	{
		/// <summary>
		/// ID
		/// </summary>
		//[Key]
		public int ID { get; set; }
		/// <summary>
		/// 菜单名称
		/// </summary>
		public string MenuName { get; set; }
		/// <summary>
		/// 父菜单ID
		/// </summary>
		public int? ParentID { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public string Enable { get; set; }
		/// <summary>
		/// 是否被默认选中
		/// </summary>
		public string Selected { get; set; }
		/// <summary>
		/// 排序字段
		/// </summary>
		public int? OrderIndex { set; get; }
	}

	#endregion

	#region 页面

	public class Page
	{
		public int ID { get; set; }
		public int MenuID { get; set; }
		[MaxLength(100)]
		public string PageName { get; set; }
		public string Enable { get; set; }
		public string PageUrl { get; set; }
		//[MaxLength(1)]
		//public string IsOpen { get; set; }
		public int? OrderIndex { get; set; }
		public string Selected { get; set; }
	}

	#endregion

	#region 文章管理

	#region 评论管理

	public class Comment
	{
		public int ID { get; set; }

		public int? TargetID { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public string Message { get; set; }

		public string IsPublic { get; set; }

		public DateTime? DateCreated { get; set; }
	}

	#endregion

	#region 文章标签

	//public class ArticleTagJoin
	//{
	//    [Key]
	//    public int ID { get; set; }

	//    public int ArticleID { get; set; }

	//    public string Tag { get; set; }

	//    //public virtual Article Article { get; set; }
	//}

	public class ArticleTag
	{
		public int ID { get; set; }

		public string Tag { get; set; }
	}

	#endregion

	#region 文章

	public class Article
	{
		public int ID { get; set; }

		public int? ColumnID { get; set; }

		//public int? CategoryID { get; set; }
		public string ShortTitle { get; set; }

		public int SortOrder { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public string Overview { get; set; }

		public string IsPublic { get; set; }

		public DateTime? UpdateTime { get; set; }

		public string UpdateUser { get; set; }

		public int? PageVisits { get; set; }

		public string Source { get; set; }

		public string Author { get; set; }
		//SEO

		public string PageTitle { get; set; }

		public string MetaDescription { get; set; }

		public string MetaKeywords { get; set; }

		public string Slug { get; set; }

		public string IsDelete { get; set; }
	}

	#endregion

	#region 单页文档

	public class Document
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public string Overview { get; set; }

		public DateTime DateCreated { get; set; }

		public string UpdateUser { get; set; }

		public int? PageVisits { get; set; }

		//SEO
		public string PageTitle { get; set; }

		public string MetaDescription { get; set; }

		public string MetaKeywords { get; set; }

		public string Slug { get; set; }
	}

	#endregion

	#region 图文集

	public class ArticleImage
	{
		public int ID { get; set; }

		public int ColumnID { get; set; }

		public string Title { get; set; }

		public string ShortTitle { get; set; }

		public int SortOrder { get; set; }

		public string Overview { get; set; }

		public string Content { get; set; }

		public string Source { get; set; }

		public string Author { get; set; }

		public int PageVisits { get; set; }

		public DateTime DateCreated { get; set; }

		public string UpdateUser { get; set; }

		public string IsDelete { get; set; }

		//SEO
		public string PageTitle { get; set; }

		public string MetaDescription { get; set; }

		public string MetaKeywords { get; set; }

		public string Slug { get; set; }
	}

	#endregion

	#endregion

	#region 用户管理

	public class User
	{
		public int ID { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public DateTime? DateCreated { get; set; }

		public DateTime? DateLastLogin { get; set; }

		public string IsActive { get; set; }
	}

	public class UserRole
	{
		public int ID { get; set; }

		public string RoleName { get; set; }
	}

	public class UserRoleJoin
	{
		public int ID { get; set; }

		public int UserID { get; set; }

		public int RoleID { get; set; }
	}

	public class UserRolePermission
	{
		public int ID { get; set; }

		public int RoleID { get; set; }

		public int TargetID { get; set; }

		public string Type { get; set; } //Menu or Button or Page

		public string IsManage { get; set; }
	}

	#endregion

	#region 栏目

	public class Column
	{
		public int ID { get; set; }

		public int? ContentTemplateID { get; set; }

		public int? ParentID { get; set; }

		public int? PageID { get; set; }

		public string ColumnName { get; set; }

		public string IsPublic { get; set; }

		public string PageTitle { get; set; }

		public string MetaDescription { get; set; }

		public string MetaKeywords { get; set; }

		public string Slug { get; set; }

		public int? SortOrder { get; set; }

		public string AdminListUrl { get; set; }

		public string WebListUrl { get; set; }

		public string WebDetailUrl { get; set; }

		public int? UpdateUser { get; set; }

		public DateTime? UpdateDate { get; set; }

		public string IsDelete { get; set; }
	}

	#endregion

	#region 内容模板

	public class ContentTemplate
	{
		public int ID { get; set; }

		public string ContentName { get; set; }

		public string Enable { get; set; }

		//public string AdminAddUrl { get; set; }

		//public string AdminEditUrl { get; set; }

		public string AdminListUrl { get; set; }

		//public string AdminDelUrl { get; set; }

		public string WebListUrl { get; set; }

		public string WebDetailUrl { get; set; }

		public DateTime? UpdateDate { get; set; }

		public string UpdateUser { get; set; }
	}

	#endregion

	#region 友情链接

	public class LinkCategory
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public int? SortOrder { get; set; }

		public DateTime? DateCreated { get; set; }

		public string UpdateUser { get; set; }
	}

	public class Links
	{
		public int ID { get; set; }

		public int? LinkCategoryID { get; set; }

		public string LinkUrl { get; set; }

		public int? SortOrder { get; set; }

		public string Name { get; set; }

		public string PictureFile { get; set; }

		public string Description { get; set; }

		public string Contact { get; set; }

		public string Email { get; set; }

		public DateTime? DateCreated { get; set; }

		public string UpdateUser { get; set; }
	}

	#endregion

	#region 图片

	public class Picture
	{
		public int ID { get; set; }

		public int TargetID { get; set; }

		public string Type { get; set; } //图文集? 相册?

		public string IsDefault { get; set; }

		public string PictureUrl { get; set; }
	}

	#endregion

	#region 系统设置

	public class Sys_Config
	{
		public int ID { get; set; }
		/// <summary>
		/// 网站名
		/// </summary>
		public string WebsiteName { get; set; }
		/// <summary>
		/// 页面title
		/// </summary>
		public string PageTitle { get; set; }
		/// <summary>
		/// 站点描述
		/// </summary>
		public string MetaDescription { get; set; }
		/// <summary>
		/// 站点默认关键字
		/// </summary>
		public string MetaKeywords { get; set; }
		/// <summary>
		/// 网站版权信息
		/// </summary>
		public string Copyright { get; set; }
		/// <summary>
		/// 网站备案号
		/// </summary>
		public string ICP { get; set; }
		/// <summary>
		/// 管理员EMAIL
		/// </summary>
		public string AdminEmail { get; set; }
		/// <summary>
		/// smtp服务器
		/// </summary>
		public string SmtpHost { get; set; }
		/// <summary>
		/// smtp端口
		/// </summary>
		public string SmtpPort { get; set; }
		/// <summary>
		/// SMTP服务器的用户邮箱
		/// </summary>
		public string SmtpEmail { get; set; }
		/// <summary>
		/// SMTP服务器的用户帐号
		/// </summary>
		public string SmtpUserAccount { get; set; }
		/// <summary>
		/// SMTP服务器的用户密码
		/// </summary>
		public string SmtpPassword { get; set; }
		/// <summary>
		/// 是否使用伪静态
		/// </summary>
		public string IsStatic { get; set; }
	}

	#endregion

	#region 回收站

	public class RecycleBin
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public string Table { get; set; }

		public int TargetID { get; set; }

		public DateTime UpdateTime { get; set; }

		public string UpdateUser { get; set; }
	}

	#endregion

	#region 行业

	public class Industry
	{
		public int ID { get; set; }

		public string IndustryName { get; set; }

		public string Description { get; set; }

		public int? UserID { get; set; }

		public DateTime AddDate { get; set; }
	}

	#endregion

	#region 企业

	public class Company
	{
		public int ID { get; set; }

		public int UserID { get; set; }

		public string Contact { get; set; }

		public string ContactName { get; set; }

		public string Address { get; set; }

		public string Description { get; set; }

		public string CompanyName { get; set; }

        public string Email { get; set; }

        public string QQ { get; set; }

        public string Website { get; set; }
	}

	#endregion
}
