using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Component;
using Logic.Services;
using Component.Component;
using Entity.Entities;

namespace EasyUI.Helpers
{
    public class LoginHelper
    {
        public LoginHelper()
        {
        }
        //登录
        public BaseObject Login(string userName, string clearPassword, bool rememberMe)
        {
            using (UserLogic logic = new UserLogic())
            {
                return logic.Login(userName, clearPassword, rememberMe);
            }
        }

        public BaseObject Register(RegisterUser param)
        {
            using (UserLogic logic = new UserLogic())
            {
                return logic.Register(param);
            }
        }

        public static int UserID
        {
            get
            {
                int userID = (HttpContext.Current.User.Identity.Name).Uint();

                return userID;
            }
        }

        public static string _userName;
        public static string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_userName))
                {
                    return HttpContext.Current.Session["UserName"].UString();
                }

                return _userName;
            }
        }

        public static bool IsManage
        {
            get
            {
                using (UserLogic logic = new UserLogic())
                {
                    var auth = logic.GetPermissionList(UserID);
                    if (auth.IsManage == PublicType.Yes)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}