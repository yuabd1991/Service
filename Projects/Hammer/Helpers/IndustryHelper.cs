using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Component;
using Entity.Entities;
using Logic.Services;

namespace EasyUI.Helpers
{
    public class IndustryHelper
    {
        /// <summary>
        /// 添加行业
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseObject InsertIndustry(IndustryEntity param)
        {
            using (IndustryLogic logic = new IndustryLogic())
            {
                return logic.InsertIndustry(param);
            }
        }
        /// <summary>
        /// 获取行业
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IndustryEntity GetIndustryByID(int id)
        {
            using (IndustryLogic logic = new IndustryLogic())
            {
                return logic.GetIndustryByID(id);
            }
        }
        /// <summary>
        /// 更新行业信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseObject UpdateIndustry(IndustryEntity param)
        {
            using (IndustryLogic logic = new IndustryLogic())
            {
                return logic.UpdateIndustry(param);
            }
        }

        public BaseObject DeleteIndustry(int id)
        {
            using (IndustryLogic logic = new IndustryLogic())
            {
                return logic.DeleteIndustry(id);
            }
        }

        public List<IndustryEntity> GetIndustryListReport(GetReportDataParams param, out int totalCount)
        {
            using (IndustryLogic logic = new IndustryLogic())
            {
                return logic.GetIndustryListReport(param, out totalCount);
            }
        }

        public List<IndustryEntity> GetIndustryList()
        {
            using (IndustryLogic logic = new IndustryLogic())
            {
                return logic.GetIndustryList();
            }
        }
    }
}