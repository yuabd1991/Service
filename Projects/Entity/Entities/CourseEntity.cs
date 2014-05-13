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

        public string Date { get; set; }

        public string Contact { get; set; }

        public string AddUserName { get; set; }

        public int AddUserID { get; set; }

        public DateTime AddDate { get; set; }

        public string CourseType { get; set; }

        public int CourseTypeID { get; set; }

        public string Photo { get; set; }

        public List<PictureEntity> Photos { get; set; }
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
}
