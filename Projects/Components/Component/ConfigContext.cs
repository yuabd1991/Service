using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;

namespace Component.Component
{
	public class ConfigContext
	{
		public static string SuperAdminReportPath = HttpContext.Current.Server.MapPath("~/bin/Report/SuperAdminReport.xml");
		public static string AdminReportPath = "";

		public static string ReportConnectString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SiteContext"].ConnectionString;
	}
}
