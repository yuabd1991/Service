using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace Component.Component
{
	public class MSSqlHelper
	{
		public static DataSet GetReportData(string reportName, GetReportDataParams param, out int totalCount, bool optimize = false)
		{
			int pageSize = param.PageSize;
			int pageIndex = param.PageIndex;
			string order = param.Order;
			List<KeyValue> where = param.Where;
			totalCount = 0;
			if (pageIndex < 1) //不能出现索引页小于1的情况，否则查询语句报错
				return new DataSet();
			if (where == null)
				where = new List<KeyValue>();

			string reportPath = CReport.GetReportPath(); //报表路径

			if (string.IsNullOrEmpty(reportPath))
				return new DataSet();
			string sqlCount = "";
			string sql = CReport.GetSql(reportPath, reportName, where, ref sqlCount, 1); //获取要查询的SQL语句 及其 参数
			string[] sqlArray = sql.Split('~');
			string orderBy = string.Empty;
			if (sqlArray.Length > 1)
			{
				orderBy = sqlArray[1];
			}
			sql = sqlArray[0];

			if (string.IsNullOrEmpty(sql))
				return new DataSet();
			else
				sql = sql.Trim();
			string conString = CReport.GetConnection(); //获取SQL连接串

			if (string.IsNullOrEmpty(conString))
				return new DataSet();
			string rowOrder = "";
			if (!string.IsNullOrEmpty(order))
			{
				rowOrder = "order by " + order + "";
			}
			else
			{
				rowOrder = string.IsNullOrEmpty(orderBy) ? "order by (select 0)" : orderBy;
			}
			int start = pageSize * (pageIndex - 1) + 1;
			int end = pageSize * pageIndex;


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
			//int firstSelectIndex = Regex.Matches(strCount, @"select", RegexOptions.IgnoreCase)[0].Index + 7;
			//sql = strCount.Insert(firstSelectIndex, " Row_Number() OVER ({0}) row ,");
			//sql = string.Format(sql, rowOrder);

			sql = string.Format(";with cte as (select Row_Number() OVER ({3}) row , * from ({0})item) select * from cte WHERE cte.row BETWEEN {1} AND {2}", sql, start, end, rowOrder);

			if (string.IsNullOrEmpty(sqlCount))
			{
				sqlCount = "select count(0) from (" + strCount + ") item ";
			}


			sql = sql + ";" + sqlCount;
			var Rundate = DateTime.Now;
			//int RunTime = 0;
			using (SqlConnection conn = new SqlConnection(conString))
			{
				SqlCommand cmd = new SqlCommand(sql, conn);
				//where 替换
				foreach (var data in where)
				{
					cmd.Parameters.Add(new SqlParameter("@" + data.Key, data.Value));
				}
				DataSet ds = new DataSet();

				SqlDataAdapter adp = new SqlDataAdapter(cmd);

				adp.Fill(ds);
				if (ds.Tables.Count > 1)
				{
					totalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
				}
				else
				{
					totalCount = 0;
				}

				conn.Close();
				//RunTime = (int)DateTime.Now.Subtract(Rundate).TotalMilliseconds;
				//记录日志
				//if (RunTime > 2000)
				//	Clients.LogClient.AddApplicationStartLogAsync(new U1City.ECDRP.Component.LogServices.ApplicationStartLog() { LogType = 1, Message = reportName, StartTime = DateTime.Now, RunTime = RunTime, SystemID = systemID });
				return ds;

			}

		}
		/// <summary>
		/// 报表统计
		/// </summary>
		/// <param name="reportName">对应的名称</param>
		/// <param name="systemID">系统ID</param>
		/// <param name="where">查询条件</param>
		/// <param name="type">报表路径</param>
		/// <param name="Totalstartsql">Sql头(select order ,count(*) as total from )</param>
		/// <param name="Totalendsql">Sql(group by order)</param>
		/// <returns></returns>
		public static DataSet GetReportTotal(string reportName, List<KeyValue> where)
		{
			string reportPath = CReport.GetReportPath(); //报表路径

			if (string.IsNullOrEmpty(reportPath))
				return new DataSet();

			string sql = CReport.GetTotalSql(reportPath, reportName, where, 1).Trim(); //获取要查询的SQL语句 及其 参数

			if (string.IsNullOrEmpty(sql))
				return new DataSet();

			string conString = CReport.GetConnection(); //获取SQL连接串

			if (string.IsNullOrEmpty(conString))
				return new DataSet();


			using (SqlConnection conn = new SqlConnection(conString))
			{

				SqlCommand cmd = new SqlCommand(sql, conn);
				//where 替换
				foreach (var data in where)
				{
					cmd.Parameters.Add(new SqlParameter("@" + data.Key, data.Value));
				}
				DataSet ds = new DataSet();
				SqlDataAdapter adp = new SqlDataAdapter(cmd);
				adp.Fill(ds);
				return ds;
			}

		}
		/// <summary>
		/// 导出报表
		/// </summary>
		/// <param name="reportSql">导出报表的SQL</param>
		/// <param name="where">过滤条件</param>
		/// <param name="order">排序字段</param>
		/// <returns></returns>
		public static DataSet GetReportExportData(string reportName, List<KeyValue> where, string order)
		{
			if (where == null)
				where = new List<KeyValue>();

			string reportPath = CReport.GetReportPath(); //报表路径
			if (string.IsNullOrEmpty(reportPath))
				return new DataSet();
			string sqlCount = "";
			string reportSql = CReport.GetSql(reportPath, reportName, where, ref sqlCount, 1).Replace("~", string.Empty); //获取要查询的SQL语句 及其 参数

			if (string.IsNullOrEmpty(reportSql))
				return new DataSet();
			string conString = CReport.GetConnection(); //获取SQL连接串

			if (string.IsNullOrEmpty(conString))
				return new DataSet();
			string sql = reportSql;
			int count = CheckExportCount(sql, conString, where, "");
			DataSet ds = new DataSet();

			ds = ExcuteMSDs(sql, where, conString);

			//MonitorManager.SetReturnValue(string.Format("Excel导出{0}条", count));
			return ds;
		}

		internal static DataSet ExcuteMSDs(string sql, List<KeyValue> where, string conString)
		{
			DataSet ds = new DataSet();
			using (SqlConnection conn = new SqlConnection(conString))
			{
				SqlCommand cmd = new SqlCommand(sql, conn);
				//where 替换
				foreach (var data in where)
				{
					cmd.Parameters.Add(new SqlParameter("@" + data.Key, data.Value));
				}

				SqlDataAdapter adp = new SqlDataAdapter(cmd);
				adp.Fill(ds);

				conn.Close();
				conn.Dispose();
				return ds;
			}
		}

		//判断导出的行数
		private static int CheckExportCount(string sql, string conString, List<KeyValue> where, string order)
		{
			string rowOrder = string.Empty;
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
			strCount = CReport.GetStrCount(strCount);
			DataSet ds = new DataSet();
			using (SqlConnection conn = new SqlConnection(conString))
			{
				SqlCommand cmd = new SqlCommand(sql, conn);
				//where 替换
				foreach (var data in where)
				{
					cmd.Parameters.Add(new SqlParameter("@" + data.Key, data.Value));
				}
				SqlDataAdapter adapter = new SqlDataAdapter(cmd);
				adapter.Fill(ds);

			}
			if (ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
			{
				return -1;
			}
			return ds.Tables[0].Rows.Count.Uint();
		}


		public static DataSet GetReportData(string sql, int pageSize, int pageIndex, string Order, out int totalCount)
		{
			string order = null;
			totalCount = 0;
			if (pageIndex < 1) //不能出现索引页小于1的情况，否则查询语句报错
				return new DataSet();

			string conString = CReport.GetConnection(); //获取SQL连接串

			if (string.IsNullOrEmpty(conString))
				return new DataSet();
			string rowOrder = Order;
			if (!string.IsNullOrEmpty(order))
			{
				rowOrder = "order by " + order + "";
			}
			else
			{
				rowOrder = "order by (select 0)";
			}
			int start = pageSize * (pageIndex - 1) + 1;
			int end = pageSize * pageIndex;


			var match = Regex.Match(sql, @"\s+order\s+", RegexOptions.IgnoreCase); //检查语句中是否含有order by
			string strCount = sql;
			if (match.Success) //有order by 则舍去order by 
			{
				strCount = sql.Substring(0, match.Index);
				if (string.IsNullOrEmpty(order))
				{
					rowOrder = sql.Substring(match.Index);
				}
			}

			sql = @" SELECT * FROM ( SELECT Row_Number() OVER ({0}) row, * from ( select * FROM (" + strCount + " ) tt) t ) item "
			+ " WHERE item.row BETWEEN " + start + " AND " + end + " ";

			sql = string.Format(sql, rowOrder);

			strCount = "select count(0) from (" + strCount + ") item ";

			sql = sql + ";" + strCount;
			try
			{
				using (SqlConnection conn = new SqlConnection(conString))
				{
					SqlCommand cmd = new SqlCommand(sql, conn);
					DataSet ds = new DataSet();

					SqlDataAdapter adp = new SqlDataAdapter(cmd);

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
		//获取流水号数据
		public static DataSet GetSerial(string strSql, List<KeyValue> where)
		{
			SqlConnection con = new SqlConnection(ConfigContext.ReportConnectString);

			con.Open();
			//启动一个事务。
			SqlTransaction myTran = con.BeginTransaction();
			//为事务创建一个命令，注意我们执行双条命令，第一次执行当然成功。我们再执行一次，失败。
			//第三次我们改其中一个命令，另一个不改，这时候事务会报错，这就是事务机制。
			SqlCommand myCom = new SqlCommand();
			myCom.Connection = con;
			myCom.Transaction = myTran;
			try
			{
				myCom.CommandText = strSql;
				//where 替换
				foreach (var data in where)
				{
					myCom.Parameters.Add(new SqlParameter("@" + data.Key, data.Value));
				}
				myCom.ExecuteNonQuery();
				myCom.Parameters.Clear();
				myCom.CommandText = "SELECT top 1 * FROM Sys_Serial WHERE RelationTable=@Type AND RelationID=@Relationid";
				//where 替换
				foreach (var data in where)
				{
					myCom.Parameters.Add(new SqlParameter("@" + data.Key, data.Value));
				}
				DataSet ds = new DataSet();
				SqlDataAdapter adp = new SqlDataAdapter(myCom);
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
