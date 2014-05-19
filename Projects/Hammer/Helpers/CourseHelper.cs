using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity.Entities;
using Component;
using Logic.Services;

namespace EasyUI.Helpers
{
    public class CourseHelper
    {
        public CourseHelper()
        {
        }

        #region 课程类型

        /// <summary>
        /// 课程类型列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CourseTypeEntity> GetCourseTypeReport(GetReportDataParams param, out int totalCount)
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.GetCourseTypeReport(param, out totalCount);
            }
        }

        public BaseObject DeleteCourseType(int id)
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.DeleteCourseType(id);
            }
        }

        public BaseObject UpdateCourseType(CourseTypeEntity param)
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.UpdateCourseType(param);
            }
        }

        public CourseTypeEntity GetCourseTypeByID(int id)
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.GetCourseTypeByID(id);
            }
        }

        public BaseObject InsertCourseType(CourseTypeEntity param)
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.InsertCourseType(param);
            }
        }

        public List<KeyName> GetCourseTypeKeyName()
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.GetCourseTypeKeyName();
            }
        }

        #endregion

        #region 课程

        /// <summary>
        /// 课程列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CourseEntity> GetCourseReport(GetReportDataParams param, out int totalCount)
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.GetCourseReport(param, out totalCount);
            }

        }

        public BaseObject DeleteCourse(int id)
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.DeleteCourse(id);
            }
        }

        public BaseObject UpdateCourse(CourseEntity param)
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.UpdateCourse(param);
            }
        }

        public CourseEntity GetCourseByID(int id)
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.GetCourseByID(id);
            }
        }

        public BaseObject InsertCourse(CourseEntity param)
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.InsertCourse(param);
            }
        }

        public List<CourseEntity> GetCourseList()
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.GetCourseList();
            }
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id">课程ID</param>
        /// <param name="auditID">审核ID</param>
        /// <returns></returns>
        public BaseObject AuditCourse(int id, int auditID)
        {
            using (CourseLogic logic = new CourseLogic())
            {
                return logic.AuditCourse(id, auditID);
            }
        }
        #endregion
    }
}