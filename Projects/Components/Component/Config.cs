using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Component.Component
{
	public static class Config
	{
		public static string ImageRelDirectory = ConfigurationManager.AppSettings["ImageRelDirectory"];
		public static string ImageCacheDirectory = ConfigurationManager.AppSettings["ImageCacheDirectory"];
		public static string ImageSysRelDirectory = ConfigurationManager.AppSettings["ImageSysRelDirectory"];
		public static string ImageWebDir = ConfigurationManager.AppSettings["ImageWebDir"];
	}
}
