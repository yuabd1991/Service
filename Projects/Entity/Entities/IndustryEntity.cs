using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.Entities
{
    public class IndustryEntity
    {
        public int ID { get; set; }

        public string IndustryName { get; set; }

        public string Description { get; set; }

        public int? UserID { get; set; }

        public string UserName { get; set; }

        public DateTime AddDate { get; set; }
    }
}
