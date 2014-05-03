using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Entities
{
	#region 友情链接

	public class LinkCategoryEntity
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public int? SortOrder { get; set; }

		public DateTime? DateTime { get; set; }

		public string UpdateUser { get; set; }
	}

	public class LinksEntity
	{
		public int ID { get; set; }

		public int? LinkCategoryID { get; set; }

		public string LinkCategory { get; set; }

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
}
