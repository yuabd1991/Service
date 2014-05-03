using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Entities
{
	public class ArticleList
	{
		public int ID { get; set; }

		public string ColumnName { get; set; }

		public string Title { get; set; }

		public string ShortTitle { get; set; }

		public int SortOrder { get; set; }

		public string Overview { get; set; }

		public DateTime? UpdateTime { get; set; }

		public string UpdateUser { get; set; }

		public int? PageVisits { get; set; }

        public string Description { get; set; }

		public string IsPublic { get; set; }
	}

	public class ArticleEntity
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
	}

    public class DocumentEntity
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

	public class ArticleImageEntity
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

		public string Photo { get; set; }

        public List<PictureEntity> Photos { get; set; }
	}

	public class HomeHotEntity
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public DateTime Date { get; set; }

		public int Type { get; set; }

		public int Count { get; set; }
	}
}
