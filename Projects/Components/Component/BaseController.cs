using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Component
{
	/// <summary>
	/// 跳过权限验证标记,不用cookie,ua=xxx
	/// 示例[ValidationAttribute(false)]
	/// </summary>
	public class ValidationAttribute : Attribute
	{
		public bool Validation { get; set; }
		public ValidationAttribute(bool validation)
		{
			Validation = validation;
		}
	}

	public abstract class BaseController : Controller
	{
		private string GetPageTag()
		{
			string path = System.Web.HttpContext.Current.Request.Path;
			path = path.Trim('/');
			return path.Replace("/", ".");
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			//页面标识
			this.ViewBag.PageTag = GetPageTag();
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			bool Validation = true;
			object[] ValidationAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ValidationAttribute), true);
			if (ValidationAttributes.Length > 0)
			{
				ValidationAttribute validationAttribute = (ValidationAttribute)ValidationAttributes[0];
				Validation = validationAttribute.Validation;
			}

			//if (Validation)
			//{
			//    if (!LoginUserManager.IsLogin)
			//    {
			//        if (filterContext.HttpContext.Request.IsAjaxRequest() || filterContext.HttpContext.Request.AppRelativeCurrentExecutionFilePath.IndexOf("/alipay/Return_url") > 0)
			//            filterContext.Result = new EmptyResult();
			//        else
			//            filterContext.Result = new RedirectResult("/Home/NoLogin");
			//    }
			//    else
			//    {
			//        LoginUserManager.CurrentUser.LastActionString = filterContext.Controller.ToString() + "." + filterContext.ActionDescriptor.ActionName;
			//        LoginUserManager.CurrentUser.LastActiveTime = DateTime.Now;
			//    }
			//}
			base.OnActionExecuting(filterContext);
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			//错误号
			string errorNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");

			//ErrorLog log = new ErrorLog();
			//LoginUser user = LoginUserManager.CurrentUser;
			//if (user != null)
			//{
			//    log.UserID = user.UserID;
			//    log.UserName = user.UserName;
			//}
			//else
			//{
			//    log.UserID = 0;
			//    log.UserName = "未登陆";
			//}
			//log.UserIP = Functions.GetIP();
			//log.ErrorCode = errorNo;
			//log.Message = filterContext.Exception.Message.Length > 200 ? filterContext.Exception.Message.Substring(0, 200) : filterContext.Exception.Message;
			//log.ErrorDesc = filterContext.Exception.StackTrace;
			//log.OnAction = ViewBag.PageTag;
			//log.OnPage = Request.RawUrl;
			//new U1City.ECDRP.Component.LogServices.LogClient().AddErrorLogAsync(log);
			//if (filterContext.HttpContext.Request.IsAjaxRequest())
			//{
			//    JsonResult res = new JsonResult();
			//    res.Data = new ExceptionObject() { Tag = -999, Message = filterContext.Exception.Message, ErrorMessage = filterContext.Exception.Message, StackTrace = filterContext.Exception.StackTrace, total = 0, rows = string.Empty };
			//    filterContext.HttpContext.Response.Clear();
			//    filterContext.Result = res;
			//    filterContext.ExceptionHandled = true;
			//}
			//else
			//{
			//    if (Config.ErrorTo404())
			//    {
			//        filterContext.Result = new RedirectResult("/Error");
			//    }
			//    else
			//    {
			//        //跳转 必须使用 
			//        filterContext.ExceptionHandled = true;
			//        Session["ErrorNo"] = errorNo;
			//        Session["ErrorException"] = filterContext.Exception;
			//        filterContext.Result = new RedirectResult("/Home/Error");
			//    }
			//}
			base.OnException(filterContext);
		}

		protected override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			base.OnResultExecuted(filterContext);

		}


		private ActionResult PageLoginRedirect(ActionExecutingContext filterContext)
		{
			return new RedirectResult("/Home/NoLogin");

		}

		/// <summary>
		/// 获得登陆用户信息字符串
		/// </summary>
		/// <returns></returns>
		//private static string GetLoginUserMessage(LoginUser user, string ErrorNo)
		//{
		//    StringBuilder str = new StringBuilder();
		//    if (!string.IsNullOrEmpty(ErrorNo))
		//    {
		//        str.Append("[错误号：" + ErrorNo + "]");
		//    }
		//    if (user != null)
		//    {
		//        str.Append("  [用户ID：" + LoginUserManager.CurrentUser.UserID + "]");
		//        str.Append("  [用户：" + LoginUserManager.CurrentUser.UserName + "]");
		//        str.Append("  [IP：" + LoginUserManager.CurrentUser.LoginIP + "]");
		//        str.Append("  [Action：" + LoginUserManager.CurrentUser.LastActionString + "]");
		//    }
		//    return str.ToString();
		//}
	}
}