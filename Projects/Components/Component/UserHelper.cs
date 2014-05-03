using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Component
{
	public class UserHelper
	{
		public static List<LoginUserInfo> _loginUserInfo { get; set; }

		public List<LoginUserInfo> LoginUserInfo
		{
			get
			{
				if (_loginUserInfo == null)
				{
					return _loginUserInfo;
				}
				else
				{
					return _loginUserInfo;
				}
			}
		}

		public BaseObject Login()
		{
			var obj = new BaseObject();

			return obj;
		}
	}
}
