using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Component
{
	/// <summary>
	/// 图片类型
	/// </summary>
	public static class PictureType
	{
		public const string ArticleImage = "ArticleImage";

		public const string Product = "Product";

		public const string Photo = "Photo";
	}
	/// <summary>
	/// 内容类型
	/// </summary>
	public static class ContentType
	{
		/// <summary>
		/// 文章
		/// </summary>
		public const int Article = 1;
		/// <summary>
		/// 单页文档
		/// </summary>
		public const int Document = 2;
		/// <summary>
		/// 图文集
		/// </summary>
		public const int ArticleWithImage = 3;
		/// <summary>
		/// 相册
		/// </summary>
        public const int Gallery = 4;
	}

    public static class PublicType
    {
        public const string Yes = "Y";
        public const string No = "N";
    }

	public static class RecycleBinTableName
	{
		public const string Article = "Article";
	}
}
