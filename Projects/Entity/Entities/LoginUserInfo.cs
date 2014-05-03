using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Entities
{
	public class LoginUserInfo
	{
		public int ID { get; set; }

		public string UserName { get; set; }

		public DateTime? AddTime { get; set; }


	}

	public class UserLoginEntity
	{
		public string UserName { get; set; }

		public string Password { get; set; }

		public string ValidateCode { get; set; }
	}
}
