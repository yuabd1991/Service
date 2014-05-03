using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Entities
{
	#region 系统设置

	public class SysConfig
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

	#region 图片

	public class PictureEntity
	{
		public int ID { get; set; }

		public int TargetID { get; set; }

		public string Type { get; set; } //图文集? 相册?

        public string PictureUrl { get; set; }

		public string IsDefault { get; set; }
	}

	#endregion

	#region 回收站

	public class RecycleBinEntity
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public string Table { get; set; }

		public int TargetID { get; set; }

		public DateTime UpdateTime { get; set; }

		public string UpdateUser { get; set; }
	}

	#endregion
}
