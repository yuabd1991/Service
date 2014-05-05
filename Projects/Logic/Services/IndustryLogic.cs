using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Component;
using Logic.Models;
using Logic.DataAccess;
using Entity.Entities;
using System.Data;
using Component.Component;

namespace Logic.Services
{
    public class IndustryLogic : DbAccess
    {
        /// <summary>
        /// 添加行业
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseObject InsertIndustry(IndustryEntity param)
        {
            var obj = new BaseObject();
            try
            {
                var industry = new Industry();
                industry.AddDate = DateTime.Now;
                industry.Description = param.Description;
                industry.IndustryName = param.IndustryName;
                industry.UserID = param.UserID;

                _db.Industries.Add(industry);
                _db.SaveChanges();

                obj.Tag = 1;
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = e.Message;
            }

            return obj;
        }

        /// <summary>
        /// 获取行业
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IndustryEntity GetIndustryByID(int id)
        {
            var ind = (from l in _db.Industries
                       where l.ID == id
                       select new IndustryEntity()
                       {
                           AddDate = l.AddDate,
                           Description = l.Description,
                           ID = l.ID,
                           IndustryName = l.IndustryName,
                           UserID = l.UserID
                       }).FirstOrDefault();

            return ind;
        }

        /// <summary>
        /// 更新行业信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public BaseObject UpdateIndustry(IndustryEntity param)
        {
            var obj = new BaseObject();
            try
            {
                var industry = _db.Industries.FirstOrDefault(m => m.ID == param.ID);
                if (industry == null)
                {
                    obj.Tag = -1;
                    obj.Message = "更新失败！";
                    return obj;
                }

                industry.AddDate = DateTime.Now;
                industry.Description = param.Description;
                industry.IndustryName = param.IndustryName;
                industry.UserID = param.UserID;

                _db.SaveChanges();
                obj.Tag = 1;
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = e.Message;
            }

            return obj;
        }

        public BaseObject DeleteIndustry(int id)
        {
            var obj = new BaseObject();

            try
            {
                var industry = _db.Industries.FirstOrDefault(m => m.ID == id);
                if (industry == null)
                {
                    obj.Tag = -1;
                    obj.Message = "删除失败！";

                    return obj;
                }

                _db.Industries.Remove(industry);
                _db.SaveChanges();
                obj.Tag = 1;
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = e.Message;
            }

            return obj;
        }

        public List<IndustryEntity> GetIndustryListReport(GetReportDataParams param, out int totalCount)
        {
            DataSet ds = MSSqlHelper.GetReportData("IndustryList", param, out totalCount);
            var dt = ds.Tables[0];
            if (dt == null)
            {
                return new List<IndustryEntity>();
            }

            var article = (from l in dt.AsEnumerable()
                           select new IndustryEntity
                        {
                            ID = l.Field<int>("ID"),
                            AddDate = l.Field<DateTime>("AddDate"),
                            UserID = l.Field<int?>("UserID"),
                            UserName = l.Field<string>("UserName"),
                            IndustryName = l.Field<string>("IndustryName"),
                            Description = l.Field<string>("Description")
                        }).ToList();

            return article.ToList();
        }

        public List<IndustryEntity> GetIndustryList()
        {
            return (from l in _db.Industries
                    select new IndustryEntity()
                    {
                        AddDate = l.AddDate,
                        Description = l.Description,
                        UserID = l.UserID,
                        IndustryName = l.IndustryName,
                        ID = l.ID
                    }).ToList();
        }
    }
}
