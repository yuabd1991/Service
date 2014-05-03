using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic.DataAccess
{
	public class Connection
	{
		public static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SiteContext"].ConnectionString;
	}
}