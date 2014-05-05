using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.Entities;
using Component;
using Component.Component;
using System.Data;
using Logic.DataAccess;
using Logic.Models;
using System.Web.Security;
using System.Web;

namespace Logic.Services
{
    public class UserLogic : DbAccess
    {
        public UserLogic()
        {
        }

        public List<UserEntity> GetUserListReport(GetReportDataParams param, out int totalCount)
        {
            DataSet ds = MSSqlHelper.GetReportData("UserList", param, out totalCount);
            var dt = ds.Tables[0];
            if (dt == null)
            {
                return new List<UserEntity>();
            }

            var article = (from l in dt.AsEnumerable()
                           select new UserEntity
                           {

                               ID = l.Field<int>("ID"),
                               DateCreated = l.Field<DateTime>("DateCreated"),
                               DateLastLogin = l.Field<DateTime>("DateLastLogin"),
                               IsActive = l.Field<string>("IsActive"),
                               UserName = l.Field<string>("UserName")
                           }).ToList();

            return article;
        }

        #region 角色

        public List<UserRoleEntity> GetUserRoleList(GetReportDataParams param, out int totalCount)
        {
            var list = (from l in _db.UserRoles
                        select new UserRoleEntity
                        {
                            ID = l.ID,
                            RoleName = l.RoleName
                        }).ToList();

            totalCount = list.Count;

            return list;
        }

        public BaseObject InsertUserRole(UserRoleEntity param)
        {
            BaseObject obj = new BaseObject();
            if (_db.UserRoles.Any(m => m.RoleName.Contains(param.RoleName)))
            {
                obj.Tag = -1;
                obj.Message = "该角色名已经存在!";

                return obj;
            }

            var userRole = new UserRole();
            userRole.RoleName = param.RoleName;

            _db.UserRoles.Add(userRole);
            _db.SaveChanges();

            obj.Tag = 1;

            return obj;
        }

        public UserRoleEntity GetUserRoleByID(int id)
        {
            var userRole = _db.UserRoles.Find(id);

            return new UserRoleEntity() { ID = userRole.ID, RoleName = userRole.RoleName };
        }

        public BaseObject UpdateUserRole(UserRoleEntity param)
        {
            BaseObject obj = new BaseObject();

            var userRole = _db.UserRoles.Find(param.ID);
            if (userRole == null)
            {
                obj.Tag = -1;
                obj.Message = "该记录不存在!";
                return obj;
            }

            userRole.RoleName = param.RoleName;

            _db.SaveChanges();

            obj.Tag = 1;

            return obj;
        }

        public BaseObject DeleteUserRole(int id)
        {
            BaseObject obj = new BaseObject();

            var userRole = _db.UserRoles.Find(id);
            if (userRole == null)
            {
                obj.Tag = -1;
                obj.Message = "该记录不存在!";
                return obj;
            }

            _db.UserRoles.Remove(userRole);

            _db.SaveChanges();

            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 权限设置

        public List<UserRolePermissionEntity> GetPermissionList(int id)
        {
            var list = (from l in _db.UserRolePermissions
                        join u in _db.UserRoleJoins on l.RoleID equals u.RoleID
                        join m in _db.Users on u.UserID equals m.ID
                        //join m in _db.Users on l.RoleID equals m.
                        where m.ID == id
                        select new UserRolePermissionEntity
                        {
                            TargetID = l.TargetID,
                            Type = l.Type,
                            ID = l.ID,
                            IsManage = l.IsManage,
                            RoleID = l.RoleID
                        }).ToList();
            //var ll = new List<string>();
            //string str = "";
            //foreach (var item in list)
            //{
            //    str += item.TargetID.ToString() + "," + item.Type + ";";
            //}

            //string str = string.Join(";", ll);

            return list;
        }

        public string GetPermissionListForUpdate(int id)
        {
            var list = (from l in _db.UserRolePermissions
                        where l.RoleID == id
                        select new UserRolePermissionEntity
                        {
                            TargetID = l.TargetID,
                            Type = l.Type,
                            ID = l.ID,
                            IsManage = l.IsManage,
                            RoleID = l.RoleID
                        }).ToList();
            //var ll = new List<string>();
            string str = "";
            foreach (var item in list)
            {
                str += item.TargetID.ToString() + "," + item.Type + ";";
            }

            //string str = string.Join(";", ll);

            return str;
        }

        #endregion

        #region 登录

        public BaseObject Login(string userName, string clearPassword, bool rememberMe)
        {
            BaseObject obj = new BaseObject();

            var user = _db.Users.FirstOrDefault(m => m.UserName == userName);
            if (user == null)
            {
                obj.Tag = -1;
                obj.Message = "用户名或密码错误!";

                return obj;
            }
            if (UserAuthenticated(user.ID, clearPassword))
            {
                //var user = GetUser(userID);

                string roles = GetUserRoles(user.ID);

                GenerateAuthenticationTicket(user.ID, rememberMe, roles);
                HttpContext.Current.Session["UserName"] = user.UserName;
                //HttpContext.Current.Response.Cookies.Add(cookies);

                user.DateLastLogin = DateTime.Now;

                _db.SaveChanges();

                obj.Tag = 1;
            }
            else
            {
                obj.Tag = -1;
                obj.Message = "用户名或密码错误!";
            }

            return obj;
        }

        public BaseObject Register(User user)
        {
            BaseObject obj = new BaseObject();

            if (_db.Users.Any(m => m.UserName == user.UserName))
            {
                obj.Tag = -1;
                obj.Message = "用户名已经存在! ";

                return obj;
            }

            var roles = new string[] { "2" };

            InsertUser(user, roles);

            _db.SaveChanges();

            obj.Tag = 1;

            return obj;
        }

        public BaseObject EditProfile(User user)
        {
            var obj = new BaseObject();

            var u = GetUser(user.ID);
            if (u == null)
            {
                obj.Tag = -1;
                obj.Message = "用户不存在！";

                return obj;
            }

            if (_db.Users.Any(m => m.UserName.Contains(user.UserName)))
            {
                obj.Tag = -1;
                obj.Message = "用户名已存在!";
            }

            u.UserName = user.UserName;

            if (!string.IsNullOrEmpty(user.Password))
            {
                var newPassword = EncryptPassword(user.Password);
                u.Password = newPassword;
            }

            _db.SaveChanges();
            obj.Tag = 1;

            return obj;
        }

        private bool UserAuthenticated(int userID, string clearPassword)
        {
            string encryptedPassword = EncryptPassword(clearPassword);

            var r = GetUser(userID, encryptedPassword);

            return r != null;
        }

        public BaseObject ChangePassword(int userID, string clearNewPassword)
        {
            var obj = new BaseObject();

            var user = GetUser(userID);

            if (user == null)
            {
                obj.Tag = -1;
                obj.Message = "未登陆或用户不存在！";

                return obj;
            }

            user.Password = EncryptPassword(clearNewPassword);
            obj.Tag = 1;

            return obj;
        }

        public string EncryptPassword(string clearPassword)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(clearPassword, "MD5");
        }

        private static void GenerateAuthenticationTicket(int userID, bool rememberMe, string roles)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
               1, // Ticket version
               userID.ToString(), // Username associated with ticket
               DateTime.Now, // Date/time issued
               DateTime.Now.AddDays(30), // Date/time to expire
               rememberMe, // persistent user cookie
               roles, // User-data, in this case the roles
               FormsAuthentication.FormsCookiePath);// Path cookie valid for Encrypt the cookie using the machine key for secure transport

            string hash = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

            cookie.Expires = DateTime.Now.AddHours(12);

            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

            // Add the cookie to the list for outgoing response
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        // Repository functions

        public User GetUser(int userID)
        {
            return _db.Users.Find(userID);
        }

        public string GetUserRoles(int userID)
        {
            var roleList = "";
            var roles = from u in _db.UserRoleJoins where u.UserID == userID select u;

            foreach (var item in roles)
            {
                roleList += item.RoleID + ",";
            }

            if (!string.IsNullOrEmpty(roleList))
                roleList = roleList.Substring(0, roleList.Length - 1);	// remove last comma (,) symbol

            return roleList;
        }

        public User GetUser(int userID, string encryptedPassword)
        {
            return _db.Users.SingleOrDefault(u => u.ID == userID && u.Password == encryptedPassword);
        }

        public void InsertUser(User user, string[] roles)
        {
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.DateCreated = DateTime.Now;
                user.DateLastLogin = DateTime.Now;
                user.Password = EncryptPassword(user.Password);

                _db.Users.Add(user);

                _db.SaveChanges();

                foreach (var item in roles)
                {
                    var roleID = Convert.ToInt32(item);

                    UserRoleJoin userRoleJoin = new UserRoleJoin();

                    userRoleJoin.RoleID = roleID;
                    userRoleJoin.UserID = user.ID;
                    _db.UserRoleJoins.Add(userRoleJoin);
                }
            }
        }

        #endregion
    }
}
