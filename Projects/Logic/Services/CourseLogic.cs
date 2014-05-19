using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Component;
using Component.Component;
using Entity.Entities;
using System.Data;
using Logic.DataAccess;
using Logic.Models;

namespace Logic.Services
{
    public class CourseLogic : DbAccess
    {
        #region 课程类型

        public BaseObject InsertCourseType(CourseTypeEntity param)
        {
            var obj = new BaseObject();
            var course = new CourseType();
            course.ParentID = param.ParentID;
            course.SortOrder = param.SortOrder;
            course.TypeName = param.TypeName;
            course.UserID = param.UserID;
            course.AddDate = DateTime.Now;

            _db.CourseTypes.Add(course);

            _db.SaveChanges();

            obj.Tag = 1;

            return obj;
        }

        public CourseTypeEntity GetCourseTypeByID(int id)
        {
            var type = (from l in _db.CourseTypes
                        where l.ID == id
                        select new CourseTypeEntity()
                        {
                            ID = l.ID,
                            ParentID = l.ParentID,
                            SortOrder = l.SortOrder,
                            TypeName = l.TypeName,
                        }).FirstOrDefault();
            return type;
        }

        public BaseObject UpdateCourseType(CourseTypeEntity param)
        {
            var obj = new BaseObject();
            var courseType = _db.CourseTypes.FirstOrDefault(m => m.ID == param.ID);
            if (courseType == null)
            {
                obj.Tag = -1;
                obj.Message = "该记录没找到";
                return obj;
            }

            courseType.SortOrder = param.SortOrder;
            courseType.TypeName = param.TypeName;
            courseType.UserID = param.UserID;
            courseType.AddDate = DateTime.Now;

            _db.SaveChanges();
            obj.Tag = 1;

            return obj;
        }

        public BaseObject DeleteCourseType(int id)
        {
            var obj = new BaseObject();
            var courseType = _db.CourseTypes.FirstOrDefault(m => m.ID == id);
            if (courseType == null)
            {
                obj.Tag = -1;
                obj.Message = "该记录没找到";
                return obj;
            }

            _db.CourseTypes.Remove(courseType);

            _db.SaveChanges();

            obj.Tag = 1;

            return obj;
        }

        /// <summary>
        /// 课程类型列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CourseTypeEntity> GetCourseTypeReport(GetReportDataParams param, out int totalCount)
        {
            DataSet ds = MSSqlHelper.GetReportData("CourseTypeList", param, out totalCount);
            var dt = ds.Tables[0];
            if (dt == null)
                return new List<CourseTypeEntity>();

            var article = (from l in dt.AsEnumerable()
                           select new CourseTypeEntity
                           {

                               ID = l.Field<int>("ID"),
                               ParentID = l.Field<int>("ParentID"),
                               AddDate = l.Field<DateTime>("AddDate"),
                               UserID = l.Field<int>("UserID"),
                               UserName = l.Field<string>("UserName"),
                               SortOrder = l.Field<int>("SortOrder"),
                               TypeName = l.Field<string>("TypeName")
                           }).ToList();

            return article;
        }

        public List<KeyName> GetCourseTypeKeyName()
        {
            var list = (from l in _db.CourseTypes
                        orderby l.SortOrder descending
                        select new KeyName()
                        {
                            ID = l.ID,
                            Name = l.TypeName
                        }).ToList();

            return list;
        }

        #endregion

        #region 课程

        public BaseObject InsertCourse(CourseEntity param)
        {
            var obj = new BaseObject();
            var course = new Course();
            course.AddDate = DateTime.Now;
            course.AddUserID = param.AddUserID;
            course.Contact = param.Contact;
            course.CourseName = param.CourseName;
            course.CourseTypeID = param.CourseTypeID;
            course.StartDate = param.StartDate;
            course.EndDate = param.EndDate;
            course.Amount = param.Amount;
            course.ApplyCount = 0;
            course.CountPeople = param.CountPeople;
            course.IsDelete = PublicType.No;
            course.Visit = 0;
            course.Description = param.Description;
            course.IndustryID = param.IndustryID;
            course.UserID = param.UserID;
            course.State = CourseState.NoAudit;
            course.Address = param.Address;

            _db.Courses.Add(course);

            _db.SaveChanges();

            obj.Tag = 1;

            return obj;
        }

        public CourseEntity GetCourseByID(int id)
        {
            var type = (from l in _db.Courses
                        where l.ID == id
                        select new CourseEntity()
                        {
                            ID = l.ID,
                            AddDate = l.AddDate,
                            AddUserID = l.AddUserID,
                            Contact = l.Contact,
                            CourseName = l.CourseName,
                            CourseTypeID = l.CourseTypeID,
                            StartDate = l.StartDate,
                            Description = l.Description,
                            IndustryID = l.IndustryID,
                            UserID = l.UserID,
                            Amount = l.Amount,
                            ApplyCount = l.ApplyCount,
                            CountPeople = l.CountPeople,
                            EndDate = l.EndDate,
                            Address = l.Address,
                            PictureFile = _db.Pictures.Where(m => m.TargetID == l.ID && m.Type == PictureType.CourseImage).OrderBy(m => m.IsDefault).Select(m => m.PictureUrl).FirstOrDefault()
                        }).FirstOrDefault();
            return type;
        }

        public BaseObject UpdateCourse(CourseEntity param)
        {
            var obj = new BaseObject();
            var courseType = _db.Courses.FirstOrDefault(m => m.ID == param.ID);
            if (courseType == null)
            {
                obj.Tag = -1;
                obj.Message = "该记录没找到";
                return obj;
            }

            courseType.AddUserID = param.AddUserID;
            courseType.Contact = param.Contact;
            courseType.CourseName = param.CourseName;
            courseType.CourseTypeID = param.CourseTypeID;
            courseType.StartDate = param.StartDate;
            courseType.Description = param.Description;
            courseType.IndustryID = param.IndustryID;
            courseType.UserID = param.UserID;
            courseType.AddDate = DateTime.Now;
            courseType.AddUserID = param.AddUserID;
            courseType.Amount = param.Amount;
            courseType.CountPeople = param.CountPeople;
            courseType.EndDate = param.EndDate;
            courseType.Address = param.Address;

            _db.SaveChanges();
            obj.Tag = 1;

            return obj;
        }

        public BaseObject DeleteCourse(int id)
        {
            var obj = new BaseObject();
            var courseType = _db.Courses.FirstOrDefault(m => m.ID == id);
            if (courseType == null)
            {
                obj.Tag = -1;
                obj.Message = "该记录没找到";
                return obj;
            }

            _db.Courses.Remove(courseType);

            _db.SaveChanges();

            obj.Tag = 1;

            return obj;
        }

        /// <summary>
        /// 课程列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CourseEntity> GetCourseReport(GetReportDataParams param, out int totalCount)
        {
            DataSet ds = MSSqlHelper.GetReportData("CourseList", param, out totalCount);
            var dt = ds.Tables[0];
            if (dt == null)
                return new List<CourseEntity>();

            var article = (from l in dt.AsEnumerable()
                           select new CourseEntity
                           {

                               ID = l.Field<int>("ID"),
                               AddUserName = l.Field<string>("UserName"),
                               Contact = l.Field<string>("Contact"),
                               AddDate = l.Field<DateTime>("AddDate"),
                               StartDate = l.Field<DateTime?>("StartDate"),
                               CourseName = l.Field<string>("CourseName"),
                               Industry = l.Field<string>("Industry"),
                               CourseType = l.Field<string>("CourseType"),
                               ApplyCount = l.Field<int?>("ApplyCount"),
                               State = l.Field<int>("State")
                           }).ToList();

            return article;
        }

        public List<CourseEntity> GetCourseList()
        {
            var list = (from l in _db.Courses
                        orderby l.StartDate
                        select new CourseEntity()
                        {
                            ID = l.ID,
                            ApplyCount = l.ApplyCount,
                            StartDate = l.StartDate,
                            CourseName = l.CourseName,
                            Address = l.Address,
                            Amount = l.Amount,
                            PictureFile = _db.Pictures.Where(m => m.TargetID == l.ID && m.Type == PictureType.CourseImage).OrderBy(m => m.IsDefault).Select(m => m.PictureUrl).FirstOrDefault()
                        }).ToList();

            return list;
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id">课程ID</param>
        /// <param name="auditID">审核ID</param>
        /// <returns></returns>
        public BaseObject AuditCourse(int id, int auditID)
        {
            var obj = new BaseObject();
            var course = _db.Courses.FirstOrDefault(m => m.ID == id);

            if (course == null)
            {
                obj.Tag = -1;
                obj.Message = "该记录未找到，请联系管理员";
                return obj;
            }

            course.State = auditID;
            _db.SaveChanges();

            obj.Tag = 1;

            return obj;
        }
        #endregion
    }
}
