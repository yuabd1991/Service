using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic.DataAccess;
using Entity.Entities;
using Logic.Models;

namespace Logic.Services
{
	public class PublicLogic: DbAccess
	{
		public PublicLogic(DataAccess.SiteContext db)
			: base(db)
		{
		}
		#region 回收站
		public void InsertRecycleBin(RecycleBinEntity param)
		{
			try
			{
				var recycbin = new RecycleBin()
				{
					Table = param.Table,
					Name = param.Name,
					TargetID = param.TargetID,
					UpdateTime = param.UpdateTime,
					UpdateUser = param.UpdateUser
				};

				_db.RecycleBins.Add(recycbin);

				_db.SaveChanges();
			}
			catch (Exception e)
			{
				
			}
		}
		#endregion
	}
}
