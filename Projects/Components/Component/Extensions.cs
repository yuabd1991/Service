using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Component.Component
{
	public static class Extensions
	{
		public static int Uint(this object obj)
		{
			int i = 0;
			if (obj != null && CheckIsInt(obj))
			{
				i = Convert.ToInt32(obj);
			}

			return i;
		}

		public static bool CheckIsInt(this object obj)
		{
			bool i = true;
			try
			{
				Convert.ToInt32(obj);
			}
			catch (Exception e)
			{
				i = false;
			}

			return i;
		}

		public static string UString(this object obj)
		{
			var str = "";
			if (obj != null)
			{
				str = obj.ToString();
			}
			return str;
		}
	}
}
