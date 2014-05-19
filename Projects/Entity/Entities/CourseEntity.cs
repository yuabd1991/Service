using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Entities
{
    public class CourseEntity
    {
        public int ID { get; set; }

        public string Industry { get; set; }

        public string PictureFile { get; set; }

        public int IndustryID { get; set; }

        public int UserID { get; set; }

        public string CourseName { get; set; }

        public string Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Contact { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        public string IsDelete { get; set; }

        public string AddUserName { get; set; }

        public int AddUserID { get; set; }

        public DateTime AddDate { get; set; }

        public string CourseType { get; set; }

        public int CourseTypeID { get; set; }

        /// <summary>
        /// 课程费用
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 可报名人数
        /// </summary>
        public int? CountPeople { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int? Visit { get; set; }
        /// <summary>
        /// 报名人数
        /// </summary>
        public int? ApplyCount { get; set; }

        public string Photo { get; set; }

        public List<PictureEntity> Photos { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
    }

    public class CourseTypeEntity
    {
        public int ID { get; set; }

        public string TypeName { get; set; }

        public int SortOrder { get; set; }

        public int ParentID { get; set; }

        public string UserName { get; set; }

        public int UserID { get; set; }

        public DateTime AddDate { get; set; }
    }

    public class ApplyCourseEntity
    {
        public int ID { get; set; }
        /// <summary>
        /// 报名的用户
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 课程ID
        /// </summary>
        public int? CourseID { get; set; }
        /// <summary>
        /// 报名人姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 报名时间
        /// </summary>
        public DateTime? AddDate { get; set; }
    }
}
