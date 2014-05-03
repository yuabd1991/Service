using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Entities
{
	public class MenuList
	{
		public int ID { get; set; }
		/// <summary>
		/// 菜单名称
		/// </summary>
		public string MenuName { get; set; }
		/// <summary>
		/// 父菜单ID
		/// </summary>
		//public int? ParentID { get; set; }
		/// <summary>
		/// 状态
		/// </summary>
		public string Enable { get; set; }
		/// <summary>
		/// 菜单URL
		/// </summary>
		public string MenuUrl { get; set; }
		/// <summary>
		/// 是否被默认选中
		/// </summary>
		public string Selected { get; set; }

		public string Type { get; set; }

		public int? _parentId { get; set; }

		public string MenuGuid { get; set; }

		public string state { get; set; }

		public string idtype { get; set; }

		public int? SystemID { set; get; }
		/// <summary>
		/// 排序字段
		/// </summary>
		public int? OrderIndex { set; get; }
	}

	public class MenuTree
	{
		public int id { get; set; }
		/// <summary>
		/// 菜单名称
		/// </summary>
		public string name { get; set; }
		/// <summary>
		/// 父菜单ID
		/// </summary>
		public int? pId { get; set; }

		public string type { get; set; }

		public List<MenuTree> children { get; set; }
	}

	public class MenuEntity
	{
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

		public string PageUrl { get; set; }

		public string Type { get; set; }
		/// <summary>
		/// 是否被默认选中
		/// </summary>
		public string Selected { get; set; }
		/// <summary>
		/// 排序字段
		/// </summary>
		public int? OrderIndex { set; get; }
	}

	public class PageEntity
	{
		public int ID { get; set; }
		public int MenuID { get; set; }
		public string PageName { get; set; }
		public string Enable { get; set; }
		public string PageUrl { get; set; }
		//[MaxLength(1)]
		//public string IsOpen { get; set; }
		public int? OrderIndex { get; set; }
		public string Selected { get; set; }
	}
}
