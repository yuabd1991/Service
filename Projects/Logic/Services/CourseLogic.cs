using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic.DataAccess;
using Component;
using Entity.Entities;
using Logic.Models;
using System.Data;
using Component.Component;

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
                            TypeName = l.TypeName
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
            course.Date = param.Date;
            course.Description = param.Description;
            course.IndustryID = param.IndustryID;
            course.UserID = param.UserID;

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
                            Date = l.Date,
                            Description = l.Description,
                            IndustryID = l.IndustryID,
                            UserID = l.UserID
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
            courseType.Date = param.Date;
            courseType.Description = param.Description;
            courseType.IndustryID = param.IndustryID;
            courseType.UserID = param.UserID;
            courseType.AddDate = DateTime.Now;

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
                               Date = l.Field<string>("Date"),
                               CourseName = l.Field<string>("CourseName"),
                               Industry = l.Field<string>("Industry"),
                               CourseType = l.Field<string>("CourseType")
                           }).ToList();

            return article;
        }

        #endregion
    }
}
