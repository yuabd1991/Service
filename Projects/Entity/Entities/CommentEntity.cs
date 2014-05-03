using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Entity.Entities
{
	public class CommentEntity
	{
		public int ID { get; set; }

		public int? TargetID { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public string Message { get; set; }

		public string IsPublic { get; set; }

		public DateTime? DateCreated { get; set; }

		[NotMapped]
		public string GravatarHash { get { return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Name.Trim().ToLower(), "MD5").ToLower(); } }
	}
}
