using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Component;

namespace Component
{
	public class Functions
	{
		private static Random Ran { get; set; }

		public static string GetRandomName(int max = 999)
		{
			return DateTime.Now.ToString("yyyyMMddhhmmss") + new Random().Next(max).ToString();
		}

		/// <summary>
		/// 获得IP
		/// </summary>
		/// <returns></returns>
		public static string GetIP()
		{
			string ip = string.Empty;
			if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
			{
				ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
			}
			else
			{
				ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
			}
			return ip;
		}

		/// <summary>
		/// 获得浏览器信息
		/// </summary>
		/// <returns></returns>
		public static string GetBrowser()
		{

			return HttpContext.Current.Request.Browser.Version.ToString();
		}

		/// <summary>
		/// 根据框架中的url 获取当前系统。
		/// </summary>
		//public static string CurrentSite
		//{
		//    get
		//    {

		//        string requestStr = HttpContext.Current.Request.RawUrl.Trim('/').Split('/')[0];
		//        return requestStr;
		//    }
		//}

		/// <summary>
		/// 检查调用
		/// </summary>
		/// <param name="site"></param>
		/// <returns></returns>
		//public static bool CheckSite(EcdrpSite site)
		//{
		//    //if (Functions.CurrentSite.ToLower() != site.ToString().ToLower())
		//    //{
		//    //    throw new Exception("您调用的方式不对，没有权限调用其他端的服务。");
		//    //}
		//    return true;
		//}

		/// <summary>
		/// 给url 添加参数
		/// </summary>
		/// <param name="url"></param>
		/// <param name="par"></param>
		/// <returns></returns>
		public static string AddUrlPar(string url, string par)
		{
			if (string.IsNullOrEmpty(url))
			{
				return "";
			}

			if (url.IndexOf("?") == -1)
			{
				url = url + "?" + par;
			}
			else
			{
				var paraString = url.Split('?')[1];
				var baseUrl = url.Split('?')[0];
				url = baseUrl + "?" + paraString + "&" + par;
			}

			return url;
		}


		/// <summary>
		/// 获取枚举Description
		/// </summary>
		/// <param name="object"></param>
		/// <returns></returns>
		//public static string GetEnumDescription<TEnum>(object value)
		//{

		//    Type enumType = typeof(TEnum);



		//    if (!enumType.IsEnum)
		//    {
		//        throw new ArgumentException("enumItem requires a Enum ");
		//    }



		//    var name = Enum.GetName(enumType, Convert.ToInt32(value));
		//    if (name == null)
		//        return string.Empty;



		//    object[] objs = enumType.GetField(name).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);



		//    if (objs == null || objs.Length == 0)
		//    {
		//        return string.Empty;
		//    }
		//    else
		//    {
		//        DescriptionAttribute attr = objs[0] as DescriptionAttribute;



		//        return attr.Description;
		//    }
		//}
		/// <summary>
		/// 获取参数
		/// </summary>
		/// <param name="Request"></param>
		/// <returns></returns>
		public List<KeyValue> GetParam(HttpRequestBase Request)
		{
			List<KeyValue> where = new List<KeyValue>();

			//IReportDataParamsExtension.handleOperatorPermission(where);

			foreach (var item in Request.Form)
			{
				var str = item.ToString();
				if (str.ToLower() == "page" || str.ToLower() == "rows" || str.ToLower() == "order")
					continue;
				if (!string.IsNullOrEmpty(Request[str]))
				{
					where.Add(new KeyValue { Key = str, Value = Request[str].Trim() });
				}
			}

			return where;
		}
		/// <summary>
		/// 获取EXCEL传过来的参数
		/// </summary>
		/// <param name="Request"></param>
		/// <returns></returns>
		//public static List<KeyValue> GetExcelParam(HttpRequestBase Request)
		//{
		//    List<KeyValue> where = new List<KeyValue>();
		//    IReportDataParamsExtension.handleOperatorPermission(where);
		//    foreach (var item in Request.QueryString)
		//    {
		//        var str = item.ToString();
		//        if (str.ToLower() == "ua")
		//            continue;
		//        if (str.ToLower() == "chlbusinessid")
		//        {
		//            if (where.Where(q => q.Key.ToLower() == "chlbusinessid").Count() == 0)
		//            {
		//                if (!string.IsNullOrEmpty(Request[str]) && Convert.ToInt32(Request[str]) > 0)
		//                {
		//                    where.Add(new KeyValue { Key = str, Value = Request[str].Trim() });
		//                }
		//            }
		//        }
		//        else
		//        {
		//            if (!string.IsNullOrEmpty(Request[str]))
		//            {
		//                where.Add(new KeyValue { Key = str, Value = Request[str].Trim() });
		//            }
		//        }
		//    }

		//    return where;

		//}

		/// <summary>
		/// 数字类型转金钱字符串。用户界面显示。截断办法。
		/// </summary>
		/// <param name="number">数字类型 eg：8569853259.5623</param>
		/// <returns>eg：8,569,853,259.56 </returns>
		public static string MoneyFormat(dynamic number)
		{
			if (number == null)
				return "";
			return Convert.ToDecimal(number).ToString("N");
		}

		/// <summary>
		/// 数字类型转金钱字符串。用户界面显示。截断办法。
		/// </summary>
		/// <param name="number">数字类型 eg：8569853259.5623</param>
		/// <returns>eg：8,569,853,259.56 </returns>
		public static string NumberFormat(dynamic number)
		{
			if (number == null)
				return "";
			return Convert.ToDecimal(number).ToString("0.00");
		}

		public static string NumberFormat(dynamic number, string df)
		{
			if (number == null)
				return df;
			return Convert.ToDecimal(number).ToString("0.00");
		}
		/// <summary>
		/// 时间格式化
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string DateFormat(DateTime? date)
		{
			if (date == null)
				return string.Empty;

			return ((DateTime)date).ToString("yyyy-MM-dd");
		}

		public static string IntFormat(dynamic number)
		{
			if (number == null)
				return "0";
			if (Convert.ToInt32(number) == number)
			{
				return Convert.ToInt32(number).ToString("0");
			}
			else
			{
				return Convert.ToDecimal(number).ToString("0.00");
			}
		}

	}
}