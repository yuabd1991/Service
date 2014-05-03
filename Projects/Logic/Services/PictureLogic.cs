using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic.DataAccess;
using Entity.Entities;
using Component;
using Logic.Models;

namespace Logic.Services
{
	public class PictureLogic: DbAccess
	{
		public PictureLogic()
		{
		}

		public BaseObject<int> InsertPicture(PictureEntity param)
		{
			var obj = new BaseObject<int>();
			var picture = new Picture();
			picture.IsDefault = "N";
			picture.TargetID = param.TargetID;
			picture.Type = param.Type;

			_db.Pictures.Add(picture);
			_db.SaveChanges();

			obj.Tag = 1;
			obj.Result = picture.ID;

			return obj;
		}
	}
}
