using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Entities
{
	public class ColumnEntity
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

	public  class ColumnList
	{
		public int ID { get; set; }

		public int? ContentTemplateID { get; set; }

		public int? ParentID { get; set; }

		public string ColumuName { get; set; }

		public string IsPublic { get; set; }

		public string AdminListUrl { get; set; }

		public string UpdateUser { get; set; }

		public DateTime? UpdateDate { get; set; }

		public int? SortOrder { get; set; }
	}

	public class ContentTemplateEntity
	{
		public int ID { get; set; }

		public string ContentName { get; set; }

		public string Enable { get; set; }

		public string AdminAddUrl { get; set; }

		public string AdminEditUrl { get; set; }

		public string AdminListUrl { get; set; }

		public string AdminDelUrl { get; set; }

		public string WebListUrl { get; set; }

		public string WebDetailUrl { get; set; }

		public DateTime? UpdateDate { get; set; }

		public string UpdateUser { get; set; }

		public int UpdateUserID { get; set; }
	}

    public class SidebarEntity
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public int? ColumnID { get; set; }

        public string Content { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateUser { get; set; }
    }
}
