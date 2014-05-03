using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic.DataAccess
{
	public abstract class DbAccess : IDisposable
	{
		protected SiteContext _db { get; set; }

		public DbAccess(SiteContext db = null)
		{
			if (db == null)
			{
				_db = new SiteContext();
			}
			else
			{
				_db = db;
			}
		}


		#region IDisposable 成员

		public void Dispose()
		{
			_db.Database.Connection.Close();
			_db.Dispose();
			_db = null;
		}

		#endregion
	}
}