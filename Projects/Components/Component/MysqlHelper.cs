using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Component.Component
{
	public class MySqlHelper
	{
		public static DataSet GetReportData(string reportName, GetReportDataParams param, int systemID, ReportConnectionType type, out int totalCount, bool optimize = false)
		{
			int pageSize = param.PageSize;
			int pageIndex = param.PageIndex;
			string order = param.Order;
			List<KeyValue> where = param.Where;
			totalCount = 0;
			if (pageIndex < 1)  //不?能??出?现?索??引?y页?3小?于???1的??情??况?，??否??则??查??询??语??句?报???错???
				pageIndex = 1;
			if (where == null)
				where = new List<KeyValue>();

			string reportPath = CReport.GetReportPath(); //报???表???路??径?

			if (string.IsNullOrEmpty(reportPath))
				return new DataSet();
			string sql = CReport.GetSql(reportPath, reportName, where); //获?取??要?a查??询??的??SQL语??句? 及??其? 参?数?y    
			if (string.IsNullOrEmpty(sql))
				return new DataSet();
			else
				sql = sql.Trim();
			string conString = CReport.GetConnection(type); //获取SQL连接串

			if (string.IsNullOrEmpty(conString))
				return new DataSet();
			string rowOrder = "";
			if (!string.IsNullOrEmpty(order))
			{
				if (order.ToUpper() == "NEWID()")
				{ //取??随?机??行D数?y
					order = "RAND()";
				}

				rowOrder = "order by " + order + "";
			}
			int start = pageSize * (pageIndex - 1);
			//    int end = pageSize * pageIndex;

			var matchs = Regex.Matches(sql, @"\s+order\s+", RegexOptions.IgnoreCase); //检查语句中是否含有order by
			string strCount = sql;
			if (matchs.Count > 1)
			{
				strCount = sql.Substring(0, matchs[matchs.Count - 1].Index);
				if (string.IsNullOrEmpty(order))
				{
					rowOrder = sql.Substring(matchs[matchs.Count - 1].Index);
				}
			}
			else if (matchs.Count == 1)
			{
				strCount = sql.Substring(0, matchs[0].Index);
				if (string.IsNullOrEmpty(order))
				{
					rowOrder = sql.Substring(matchs[0].Index);
				}
			}

			sql = string.Format(strCount + " " + rowOrder);
			if (!optimize) //是否自定义分页
			{
				sql += string.Format(" limit {0},{1}", start, pageSize);
				sql = sql.Insert(sql.ToLower().IndexOf("select") + 6, " SQL_CALC_FOUND_ROWS ");
				sql = sql + ";" + "select FOUND_ROWS()";
			}
			else
			{
				sql = sql.Replace("@PagerLimit", string.Format(" limit {0},{1}", start, pageSize)); //针对特殊的分页，limit不是在sql的最后面的情况
			}

			var Rundate = DateTime.Now;
			int RunTime = 0;
			using (MySqlConnection conn = new MySqlConnection(conString))
			{
				MySqlCommand cmd = new MySqlCommand(sql, conn);
				//where 替??换?
				foreach (var data in where)
				{
					cmd.Parameters.Add(new MySqlParameter("@" + data.Key, data.Value));
				}
				DataSet ds = new DataSet();
				MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
				adp.Fill(ds);
				totalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
				RunTime = (int)DateTime.Now.Subtract(Rundate).TotalMilliseconds;
				//记录日志
				//if (RunTime > 2000)
					//Clients.LogClient.AddApplicationStartLogAsync(new U1City.ECDRP.Component.LogServices.ApplicationStartLog() { LogType = 1, Message = reportName, StartTime = DateTime.Now, RunTime = RunTime, SystemID = systemID });
				return ds;
			}

		}

		public static DataSet GetReportData(string sql, int pageSize, int pageIndex, string Order, ReportConnectionType type,
			out int totalCount)
		{
			totalCount = 0;
			if (pageIndex < 1)  //不?能??出?现?索??引?y页?3小?于???1的??情??况?，??否??则??查??询??语??句?报???错???
				return new DataSet();
			if (sql == null)
				return null;
			sql = sql.Trim();
			string conString = CReport.GetConnection(type); //获?取??SQL连??接??串??

			if (string.IsNullOrEmpty(conString))
				return new DataSet();
			var order = Order;
			string rowOrder = "";
			if (!string.IsNullOrEmpty(order))
			{
				if (order.ToUpper() == "NEWID()")
				{ //取??随?机??行D数?y
					order = "RAND()";
				}
				rowOrder = "order by " + order + "";
			}
			int start = pageSize * (pageIndex - 1);
			int end = pageSize * pageIndex;


			var match = Regex.Match(sql, @"\s+order\s+", RegexOptions.IgnoreCase); //检??查??语??句?中D是??否??含?有?Dorder by
			string strCount = sql;
			sql = string.Format(strCount + " " + rowOrder + " limit {0},{1}", start, pageSize);
			strCount = "select count(0) from (" + strCount + ") item ";
			sql = sql + ";" + strCount;
			try
			{
				using (MySqlConnection conn = new MySqlConnection(conString))
				{
					MySqlCommand cmd = new MySqlCommand(sql, conn);
					DataSet ds = new DataSet();
					MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
					adp.Fill(ds);
					totalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
					return ds;
				}
			}
			catch (Exception)
			{
				return null;
			}
		}
		/// <summary>
		/// 导??出?报???表???
		/// </summary>
		/// <param name="reportSql"> 导??出?报???表???的??SQL </param>
		/// <param name="where"> 过y滤?条??件t </param>
		/// <param name="order"> 排?序??字??段? </param>
		/// <returns></returns>
		public static DataSet GetReportExportData(string reportName, List<KeyValue> where, int systemID,
			ReportConnectionType type)
		{
			if (where == null)
				where = new List<KeyValue>();
			string reportPath = CReport.GetReportPath(systemID); //报表路??径?


			if (string.IsNullOrEmpty(reportPath))
				return new DataSet();
			string reportSql = CReport.GetSql(reportPath, reportName, where); //获?取??要?a查??询??的??SQL语??句? 及??其? 参?数?y
			if (string.IsNullOrEmpty(reportSql))
				return new DataSet();
			string conString = CReport.GetConnection(type); //获?取??SQL连??接??串??
			if (string.IsNullOrEmpty(conString))
				return new DataSet();
			string sql = reportSql;
			//int count = CheckExportCount(sql, conString, where, "");
			DataSet ds = new DataSet();
			//for (int i = 0; i < Math.Ceiling((double)count / 9000); i++)
			//{
			//    var sqlItem = string.Format(sql + " limit {0},{1}", i * 9000, 9000);
			//    if (ds.Tables.Count == 0)
			//    {
			//        ds = ExcuteMyDs(sqlItem, where, conString);
			//    }
			//    else
			//    {
			//        DataTable dt = ExcuteMyDs(sqlItem, where, conString).Tables[0];
			//        ds.Tables[0].Merge(dt);
			//    }
			//}
			//if (count == 0)
			//{
			ds = ExcuteMyDs(sql, where, conString);
			//}
			//MonitorManager.SetReturnValue(string.Format("Excel导??出?{0}条??", count));
			return ds;

		}
		//判D断?导??出?的??行D数?y
		private static int CheckExportCount(string sql, string conString, List<KeyValue> where, string order)
		{
			string rowOrder = string.Empty;
			var matchs = Regex.Matches(sql, @"\s+order\s+", RegexOptions.IgnoreCase); //检??查??语??句?中D是??否??含?有?Dorder by
			string strCount = sql;
			if (matchs.Count > 1)
			{
				strCount = sql.Substring(0, matchs[matchs.Count - 1].Index);
				if (string.IsNullOrEmpty(order))
				{
					rowOrder = sql.Substring(matchs[matchs.Count - 1].Index);
				}
			}
			else if (matchs.Count == 1)
			{
				strCount = sql.Substring(0, matchs[0].Index);
				if (string.IsNullOrEmpty(order))
				{
					rowOrder = sql.Substring(matchs[0].Index);
				}
			}
			strCount = strCount + " limit 0";
			strCount = strCount.Insert(strCount.ToLower().IndexOf("select") + 6, " SQL_CALC_FOUND_ROWS ");
			strCount = strCount + ";" + "select FOUND_ROWS()";


			//    strCount = CReport.GetStrCount(strCount);

			DataSet ds = ExcuteMyDs(strCount, where, conString);
			if (ds == null)
			{
				return -1;
			}
			return Convert.ToInt32(ds.Tables[1].Rows[0][0]);
		}
		/// <summary>
		/// 报???表???统?3计?
		/// </summary>
		/// <param name="reportName"> 对?应?|的??名?称? </param>
		/// <param name="systemID"> 系??统?3ID</param>
		/// <param name="where"> 查??询??条??件t </param>
		/// <param name="type"> 报???表???路??径? </param>
		/// <param name="Totalstartsql"> Sql头???(select order ,count(*) as total from ) </param>
		/// <param name="Totalendsql"> Sql(group by order)</param>
		/// <returns></returns>
		public static DataSet GetReportTotal(string reportName, List<KeyValue> where, int systemID, ReportConnectionType type, bool optimize = false)
		{
			string reportPath = CReport.GetReportPath(systemID); //报???表???路??径?
			if (string.IsNullOrEmpty(reportPath))
				return new DataSet();
			string sql = CReport.GetTotalSql(reportPath, reportName, where).Trim(); //获?取??要?a查??询??的??SQL语??句? 及??其? 参?数?y
			if (string.IsNullOrEmpty(sql))
				return new DataSet();
			string conString = CReport.GetConnection(type); //获取SQL连接串
			if (string.IsNullOrEmpty(conString))
				return new DataSet();
			DataSet ds = new DataSet();
			using (MySqlConnection conn = new MySqlConnection(conString))
			{
				MySqlCommand cmd = new MySqlCommand(sql, conn);
				//where 替换
				foreach (var data in where)
				{
					cmd.Parameters.Add(new MySqlParameter("@" + data.Key, data.Value));
				}

				MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
				adp.Fill(ds);
				return ds;
			}

		}

		internal static DataSet ExcuteMyDs(string sql, List<KeyValue> where, string conString)
		{
			DataSet ds = new DataSet();
			using (MySqlConnection conn = new MySqlConnection(conString))
			{
				MySqlCommand cmd = new MySqlCommand(sql, conn);
				//where 替换
				foreach (var data in where)
				{
					cmd.Parameters.Add(new MySqlParameter("@" + data.Key, data.Value));
				}

				MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
				adp.Fill(ds);

				conn.Close();
				conn.Dispose();
				return ds;
			}
		}

		//获取流水号数据
		public static DataSet GetSerial(string strSql, List<KeyValue> where)
		{
			MySqlConnection con = new MySqlConnection(ConfigContext.ReportConnectString);
			con.Open();
			//启动一个事务。
			MySqlTransaction myTran = con.BeginTransaction();
			//为事务创建一个命令，注意我们执行双条命令，第一次执行当然成功。我们再执行一次，失败。
			//第三次我们改其中一个命令，另一个不改，这时候事务会报错，这就是事务机制。
			MySqlCommand myCom = new MySqlCommand();
			myCom.Connection = con;
			myCom.Transaction = myTran;
			try
			{
				myCom.CommandText = strSql;
				//where 替换
				foreach (var data in where)
				{
					myCom.Parameters.Add(new MySqlParameter("@" + data.Key, data.Value));
				}
				myCom.ExecuteNonQuery();
				myCom.Parameters.Clear();
				myCom.CommandText = "SELECT  * FROM Sys_Serial WHERE RelationTable=@Type AND RelationID=@Relationid limit 1";
				//where 替换
				foreach (var data in where)
				{
					myCom.Parameters.Add(new MySqlParameter("@" + data.Key, data.Value));
				}
				DataSet ds = new DataSet();
				MySqlDataAdapter adp = new MySqlDataAdapter(myCom);
				adp.Fill(ds);
				myTran.Commit();
				return ds;
			}
			catch (Exception Ex)
			{
				myTran.Rollback();
				//创建并且返回异常的错误信息
				throw new Exception(Ex.Message);
			}
			finally
			{
				con.Close();
			}
		}

	}
}
