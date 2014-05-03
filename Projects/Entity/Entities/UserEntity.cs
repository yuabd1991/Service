using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Entities
{
	public class UserEntity
	{
		public int ID { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public DateTime? DateCreated { get; set; }

		public DateTime? DateLastLogin { get; set; }

		public string IsActive { get; set; }
	}

	public class UserRoleEntity
	{
		public int ID { get; set; }

		public string RoleName { get; set; }
	}

	public class UserRoleJoinEntity
	{
		public int ID { get; set; }

		public int UserID { get; set; }

		public int RoleID { get; set; }
	}

	public class UserRolePermissionEntity
	{
		public int ID { get; set; }

		public int RoleID { get; set; }

		public int TargetID { get; set; }

		public string Type { get; set; } //Menu or Button or Page

		public string IsManage { get; set; }
	}
}
