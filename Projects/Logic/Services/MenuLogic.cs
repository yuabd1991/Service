using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logic.DataAccess;
using Component;
using Logic.Models;
using System.Data;
using System.Data.SqlClient;
using Entity.Entities;

namespace Logic.Services
{
    public class MenuLogic : DbAccess
    {
        public MenuLogic()
        {

        }

        public List<MenuEntity> GetAllPages(UserAuth list)
        {
            var qu = list.UserRolePermissionEntities.Where(m => m.Type.ToLower() == "page").Select(m => m.TargetID).Distinct().ToArray();
            var query = (from l in _db.Pages
                         where l.Enable == PublicType.Yes && (list.IsManage == PublicType.No ? qu.Contains(l.ID) : true)
                         orderby l.OrderIndex descending
                         select new MenuEntity
                         {
                             Enable = l.Enable,
                             ID = l.ID,
                             MenuName = l.PageName,
                             PageUrl = l.PageUrl,
                             OrderIndex = l.OrderIndex,
                             ParentID = l.MenuID,
                             Selected = l.Selected
                         }).ToList();
            return query;
        }

        public List<MenuEntity> GetAllMenus(UserAuth list)
        {
            var qu = list.UserRolePermissionEntities.Where(m => m.Type.ToLower() == "menu").Select(m => m.TargetID).Distinct().ToArray();

            var query = (from l in _db.Menus
                         where l.Enable == PublicType.Yes && (list.IsManage == PublicType.No ? qu.Contains(l.ID) : true)
                         orderby l.OrderIndex descending
                         select new MenuEntity
                         {
                             Enable = l.Enable,
                             ID = l.ID,
                             MenuName = l.MenuName,
                             OrderIndex = l.OrderIndex,
                             ParentID = l.ParentID,
                             Selected = l.Selected,
                         }).ToList();
            return query;
        }

        public BaseObject InsertMenu(MenuEntity menu)
        {
            BaseObject obj = new BaseObject();
            _db.Connection.Open();
            var con = _db.Connection.BeginTransaction();



            if (menu.Type == "Page")
            {
                var samename = _db.Pages.FirstOrDefault(m => m.PageName == menu.MenuName && m.MenuID == menu.ParentID);
                if (samename != null)
                {
                    obj.Tag = -1;
                    obj.Message = "页面名称已经存在！";
                    return obj;
                }

                var page = new Page()
                {
                    Enable = menu.Enable,
                    Selected = menu.Selected,
                    MenuID = (int)menu.ParentID,
                    OrderIndex = menu.OrderIndex,
                    PageName = menu.MenuName,
                    PageUrl = menu.PageUrl,
                };

                _db.Pages.Add(page);
            }
            else
            {
                var samename = _db.Menus.FirstOrDefault(m => m.MenuName == menu.MenuName && m.ParentID == menu.ParentID);
                if (samename != null)
                {
                    obj.Tag = -1;
                    obj.Message = "菜单名称已经存在！";
                    return obj;
                }

                var u = new Menu()
                {
                    Enable = menu.Enable,
                    MenuName = menu.MenuName,
                    OrderIndex = menu.OrderIndex,
                    ParentID = menu.ParentID,
                    Selected = menu.Selected,
                };

                _db.Menus.Add(u);
            }

            try
            {
                _db.SaveChanges();
                con.Commit();

                obj.Tag = 1;
                obj.Message = "新增成功！";
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = "新增失败！";
                con.Rollback();
                throw e;
            }
            finally
            {
                con.Dispose();
            }

            return obj;
        }

        public MenuEntity GetMenuByID(int id)
        {
            var menu = (from l in _db.Menus
                        where l.ID == id
                        select new MenuEntity
                        {
                            ID = id,
                            Enable = l.Enable,
                            MenuName = l.MenuName,
                            OrderIndex = l.OrderIndex,
                            ParentID = l.ParentID,
                            Selected = l.Selected,
                            Type = "Menu",
                        }).FirstOrDefault();

            return menu;
        }

        public BaseObject EditMenu(MenuEntity menu)
        {
            BaseObject obj = new BaseObject();

            _db.Connection.Open();
            using (var con = _db.Connection.BeginTransaction())
            {
                try
                {
                    var m = _db.Menus.FirstOrDefault(q => q.ID == menu.ID);
                    if (m == null)
                    {
                        obj.Tag = -1;
                        obj.Message = "记录不存在！";

                        return obj;
                    }

                    //m.Enable = menu.Enable;
                    m.MenuName = menu.MenuName;
                    m.OrderIndex = menu.OrderIndex;
                    //m.ParentID = menu.ParentID;
                    m.Selected = menu.Selected;
                    //m.SystemID = menu.SystemID;

                    _db.SaveChanges();
                    obj.Tag = 1;
                    con.Commit();
                }
                catch (Exception e)
                {
                    con.Rollback();
                    obj.Tag = -1;
                    obj.Message = e.Message;
                    throw e;
                }
                finally
                {
                    con.Dispose();
                }
            }

            return obj;
        }

        public BaseObject DelMenu(int id)
        {
            BaseObject obj = new BaseObject();
            var menu = _db.Menus.Find(id);

            if (menu == null)
            {
                obj.Tag = -1;
                obj.Message = "记录不存在！";

                return obj;
            }

            try
            {
                _db.Menus.Remove(menu);
                _db.SaveChanges();

                obj.Tag = 1;
            }
            catch (Exception e)
            {
                obj.Tag = -2;
                obj.Message = e.Message;
                throw e;
            }

            return obj;
        }

        public List<MenuList> GetMenus(string ids)
        {
            int? menuID = null;
            int? pageID = null;
            if (!string.IsNullOrEmpty(ids))
            {
                if (ids.IndexOf("menu") > 0) menuID = Convert.ToInt32(ids.Split(':')[0]);
                if (ids.IndexOf("page") > 0) pageID = Convert.ToInt32(ids.Split(':')[0]);
            }

            var list = new List<MenuList>();
            var query1 = new List<MenuList>();
            var query2 = new List<MenuList>();
            var query3 = new List<MenuList>();

            if (string.IsNullOrEmpty(ids))
            {
                list = (from l in _db.Menus
                        where l.ParentID.HasValue ? l.ParentID == 0 : l.ParentID.HasValue == false
                        select new MenuList
                        {
                            ID = l.ID,
                            Enable = l.Enable,
                            MenuName = l.MenuName,
                            OrderIndex = l.OrderIndex,
                            _parentId = l.ParentID,
                            Selected = l.Selected,
                            Type = "Menu"
                        }).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    MenuList menu = list[i];
                    menu.MenuGuid = Guid.NewGuid().ToString();
                    menu.idtype = menu.ID + ":menu";
                    menu.state = "closed";
                }
            }

            if (menuID != null)
            {
                query1 = (from l in _db.Menus
                          where l.ParentID == menuID
                          select new MenuList
                          {
                              ID = l.ID,
                              Enable = l.Enable,
                              MenuName = l.MenuName,
                              OrderIndex = l.OrderIndex,
                              _parentId = l.ParentID,
                              Selected = l.Selected,
                              Type = "Menu"
                          }).ToList();
                for (int i = 0; i < query1.Count; i++)
                {
                    MenuList menu = query1[i];
                    menu.MenuGuid = Guid.NewGuid().ToString();
                    menu.idtype = menu.ID + ":menu";
                    menu.state = "closed";
                }

                query2 = (from l in _db.Pages
                          where l.MenuID == menuID
                          select new MenuList
                          {
                              ID = l.ID,
                              Enable = l.Enable,
                              MenuName = l.PageName,
                              MenuUrl = l.PageUrl,
                              OrderIndex = l.OrderIndex,
                              _parentId = menuID,
                              Selected = l.Selected,
                              Type = "Page"
                          }).ToList();

                for (int i = 0; i < query2.Count; i++)
                {
                    MenuList menu = query2[i];
                    menu.MenuGuid = Guid.NewGuid().ToString();
                    menu.idtype = menu.ID + ":page";
                    menu.state = "";
                }
            }

            if (pageID != null)
            {
                query3 = (from l in _db.Pages
                          where l.MenuID == menuID
                          select new MenuList
                          {
                              ID = l.ID,
                              Enable = "Y",
                              MenuName = l.PageName,
                              MenuUrl = l.PageUrl,
                              OrderIndex = l.OrderIndex,
                              Selected = l.Selected,
                              Type = "Page",
                          }).ToList();

                for (int i = 0; i < query3.Count; i++)
                {
                    MenuList menu = query3[i];
                    menu.MenuGuid = Guid.NewGuid().ToString();
                    menu.idtype = menu.ID + ":page";
                    menu.state = "";
                }
            }

            list.AddRange(query1);
            list.AddRange(query2);
            list.AddRange(query3);

            return list;
        }

        public List<MenuTree> HandleSubMenu(int? parentID)
        {
            var rtnList = new List<MenuTree>();
            var menulist = _db.Menus.Where(m => m.ParentID == parentID).OrderByDescending(m => m.OrderIndex).ToList();

            foreach (var item in menulist)
            {
                if (parentID != null && item.ParentID.Equals(parentID))
                {
                    var menu = new MenuTree()
                    {
                        id = item.ID,
                        pId = parentID,
                        name = item.MenuName,
                        type = "Menu",
                        children = HandleSubMenu(item.ID)
                    };
                    rtnList.Add(menu);
                }
            }

            return rtnList;
        }

        public List<MenuTree> GetTreeMenu()
        {
            var menus = new List<MenuTree>();
            var menulist = _db.Menus.Where(m => m.ParentID == 0 || m.ParentID == null).OrderByDescending(m => m.OrderIndex).ToList();
            foreach (var item in menulist)
            {
                var menu = new MenuTree()
                {
                    id = item.ID,
                    pId = item.ParentID,
                    name = item.MenuName,
                    type = "Menu",
                    children = HandleSubMenu(item.ID)
                };

                menus.Add(menu);
            }

            return menus;
        }

        public BaseObject DisEnableMenu(int id, string state, string type)
        {
            var obj = new BaseObject();
            try
            {
                var newState = state == PublicType.Yes ? PublicType.No : PublicType.Yes;
                if (type.ToLower() == "page")
                {

                    var page = _db.Pages.FirstOrDefault(m => m.ID == id);
                    if (page == null)
                    {
                        obj.Tag = -1;
                        obj.Message = "操作失败!";
                    }
                    page.Enable = newState;
                }
                else if (type.ToLower() == "menu")
                {
                    var menu = _db.Menus.FirstOrDefault(m => m.ID == id);
                    if (menu == null)
                    {
                        obj.Tag = -1;
                        obj.Message = "操作失败!";
                    }
                    menu.Enable = newState;
                }

                _db.SaveChanges();
                obj.Tag = 1;
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = "内部错误!, 错误信息: " + e.Message;
            }

            return obj;
        }
        /// <summary>
        /// 页面获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PageEntity GetPageByID(int id)
        {
            var page = (from l in _db.Pages
                        where l.ID == id
                        select new PageEntity()
                        {
                            Enable = l.Enable,
                            ID = l.ID,
                            MenuID = l.MenuID,
                            OrderIndex = l.OrderIndex,
                            PageName = l.PageName,
                            PageUrl = l.PageUrl,
                            Selected = l.Selected
                        }).FirstOrDefault();

            return page;
        }
        /// <summary>
        /// 修改页面
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseObject EditPage(MenuEntity param)
        {
            var obj = new BaseObject();
            var page = _db.Pages.FirstOrDefault(m => m.ID == param.ID);

            if (page == null)
            {
                obj.Tag = -1;
                return obj;
            }
            page.OrderIndex = param.OrderIndex;
            page.PageName = param.MenuName;
            page.PageUrl = param.PageUrl;
            page.Selected = param.Selected;

            _db.SaveChanges();

            obj.Tag = 1;

            return obj;
        }
    }
}