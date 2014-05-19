using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Component
{
    /// <summary>
    /// 用户类型
    /// </summary>
    public static class UserType
    {
        public const string Personal = "Personal";

        public const string Company = "Company";
    }
	/// <summary>
	/// 图片类型
	/// </summary>
	public static class PictureType
	{
		public const string ArticleImage = "ArticleImage";

		public const string Product = "Product";

		public const string Photo = "Photo";

        public const string CourseImage = "CourseImage";
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
    /// <summary>
    /// 课程状态
    /// </summary>
    public static class CourseState
    {
        /// <summary>
        /// 未审核
        /// </summary>
        public const int NoAudit = 0;
        /// <summary>
        /// 审核通过
        /// </summary>
        public const int Pass = 1;
        /// <summary>
        /// 审核不通过
        /// </summary>
        public const int UnPass = 2;
        /// <summary>
        /// 报名未开始
        /// </summary>
        public const int NoStart = 3;
        /// <summary>
        /// 报名进行中
        /// </summary>
        public const int Process = 4;
        /// <summary>
        /// 报名结束
        /// </summary>
        public const int End = 5;
    }
}
