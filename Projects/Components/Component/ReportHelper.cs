using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace Component.Component
{
	public class ReportHelp
	{

		public ReportHelp()
		{

		}
		/// <summary>
		/// 报表统计
		/// </summary>
		/// <param name="reportName"> 对应的名称 </param>
		/// <param name="systemID"> 系统ID</param>
		/// <param name="where"> 查询条件 </param>
		/// <param name="type"> 报表路径 </param>
		/// <param name="Totalstartsql"> Sql头(select order ,count(*) as total from ) </param>
		/// <param name="Totalendsql"> Sql(group by order)</param>
		/// <returns></returns>
		public static DataSet GetReportTotal(string reportName, List<KeyValue> where, int systemID,
			ReportConnectionType type, bool optimize = false)
		{
			if (where != null)
			{
				foreach (var item in where)
				{
					item.Value = item.Value.Trim();
				}
			}
			//switch (ConfigContext.SqlAss)
			//{
			//    case SqlEnum.MSSql:
			//        return MSSqlHelper.GetReportTotal(reportName, where, systemID, type);
			//    case SqlEnum.MYSql:
			//        {
			//            DataSet ds = MySqlHelper.GetReportTotal(reportName, where, systemID, type, optimize);
			//            if (ds.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).Count() == 0)
			//                return ds;
			//            DataSet resDs = new DataSet();
			//            resDs.Tables.Add(ds.Tables[0].Clone());
			//            resDs.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).ToList().ForEach(q => q.DataType = typeof(int));
			//            foreach (var item in ds.Tables[0].Rows)
			//            {
			//                resDs.Tables[0].ImportRow(item as DataRow);
			//            }
			//            return resDs;
			//        }
			//    default:
			//        return MSSqlHelper.GetReportTotal(reportName, where, systemID, type);
			//}

			DataSet ds = MySqlHelper.GetReportTotal(reportName, where, type, optimize);
			if (ds.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).Count() == 0)
				return ds;
			DataSet resDs = new DataSet();
			resDs.Tables.Add(ds.Tables[0].Clone());
			resDs.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).ToList().ForEach(q => q.DataType = typeof(int));
			foreach (var item in ds.Tables[0].Rows)
			{
				resDs.Tables[0].ImportRow(item as DataRow);
			}
			return resDs;

		}
		/// <summary>
		/// 前??台???需??要?a点??击??排?序??的??时???候??调???用??
		/// </summary>
		/// <param name="reportName"> XML对?应?|的??名?称? </param>
		/// <param name="where"> 查??询??条??件t </param>
		/// <param name="pageSize"> 页?3面?打???小? </param>
		/// <param name="pageIndex"> 页?3面?索??引?y </param>
		/// <param name="order"></param>
		/// <param name="systemID"> 系??统?3ID</param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		public static DataSet GetReportData(string reportName, GetReportDataParams param,
			int systemID, ReportConnectionType type, out int totalCount, bool optimize = false)
		{
			if (param.Where != null)
			{
				foreach (var item in param.Where)
				{
					item.Value = item.Value.Trim();
				}
			}
			DataSet ds;
			//switch (ConfigContext.SqlAss)
			//{
			//    case SqlEnum.MSSql:
			//        ds = MSSqlHelper.GetReportData(reportName, param, systemID, type, out totalCount);
			//        break;
			//    case SqlEnum.MYSql:
			//        ds = MySqlHelper.GetReportData(reportName, param, systemID, type, out totalCount, optimize);
			//        if (ds.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).Count() == 0)
			//        {
			//            break;
			//        }
			//        DataSet resDs = new DataSet();
			//        resDs.Tables.Add(ds.Tables[0].Clone());
			//        resDs.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).ToList().ForEach(q => q.DataType = typeof(int));
			//        foreach (var item in ds.Tables[0].Rows)
			//        {
			//            resDs.Tables[0].ImportRow(item as DataRow);
			//        }
			//        ds = resDs;
			//        break;
			//    default:
			//        ds = MSSqlHelper.GetReportData(reportName, param, systemID, type, out totalCount);
			//        break;
			//}
			ds = MySqlHelper.GetReportData(reportName, param, systemID, type, out totalCount, optimize);
			if (ds.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).Count() == 0)
			{
				return ds;
			}
			DataSet resDs = new DataSet();
			resDs.Tables.Add(ds.Tables[0].Clone());
			resDs.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).ToList().ForEach(q => q.DataType = typeof(int));
			foreach (var item in ds.Tables[0].Rows)
			{
				resDs.Tables[0].ImportRow(item as DataRow);
			}
			ds = resDs;

			return ds;

		}

		private static string ForSql(string sql, string order, int start, int end)
		{
			sql = @" SELECT * FROM ( SELECT Row_Number() OVER ({0}) row, * from ( select * FROM (" + sql + " )  tt) t ) item "
					 + " WHERE item.row BETWEEN " + start + " AND " + end + " ";

			sql = string.Format(sql, order);
			return sql;

		}
		/// <summary>
		/// 导??出?报???表???
		/// </summary>
		/// <param name="reportSql"> 导??出?报???表???的??SQL </param>
		/// <param name="where"> 过y滤?条??件t </param>
		/// <param name="order"> 排?序??字??段? </param>
		/// <returns></returns>
		public static DataSet GetReportExportData(string reportName, List<KeyValue> where, int systemID, string order, ReportConnectionType type)
		{
			//switch (ConfigContext.SqlAss)
			//{
			//    case SqlEnum.MSSql:
			//        return MSSqlHelper.GetReportExportData(reportName, where, systemID, order, type);
			//    case SqlEnum.MYSql:
			//        {
			//            DataSet ds = MySqlHelper.GetReportExportData(reportName, where, systemID, order, type);
			//            if (ds.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).Count() == 0)
			//                return ds;
			//            DataSet resDs = new DataSet();
			//            resDs.Tables.Add(ds.Tables[0].Clone());
			//            resDs.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).ToList().ForEach(q => q.DataType = typeof(int));
			//            foreach (var item in ds.Tables[0].Rows)
			//            {
			//                resDs.Tables[0].ImportRow(item as DataRow);
			//            }
			//            return resDs;
			//        }
			//    default:
			//        return MSSqlHelper.GetReportExportData(reportName, where, systemID, order, type);
			//}

			DataSet ds = MySqlHelper.GetReportExportData(reportName, where, systemID, type);
			if (ds.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).Count() == 0)
				return ds;
			DataSet resDs = new DataSet();
			resDs.Tables.Add(ds.Tables[0].Clone());
			resDs.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).ToList().ForEach(q => q.DataType = typeof(int));
			foreach (var item in ds.Tables[0].Rows)
			{
				resDs.Tables[0].ImportRow(item as DataRow);
			}
			return resDs;
		}
		/// <summary>
		/// 导??出?报???表???
		/// </summary>
		/// <param name="reportSql"> 导??出?报???表???的??SQL </param>
		/// <param name="where"> 过y滤?条??件t </param>
		/// <param name="order"> 排?序??字??段? </param>
		/// <returns></returns>
		public static DataSet GetReportExportData(string reportName, List<KeyValue> where,
			int systemID, ReportConnectionType type)
		{
			if (where != null)
			{
				foreach (var item in where)
				{
					item.Value = item.Value.Trim();
				}
			}
			//switch (ConfigContext.SqlAss)
			//{
			//    case SqlEnum.MSSql:
			//        return MSSqlHelper.GetReportExportData(reportName, where, systemID, "", type);
			//    case SqlEnum.MYSql:
			//        {
			//            DataSet ds = MySqlHelper.GetReportExportData(reportName, where, systemID, "", type);
			//            if (ds.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).Count() == 0)
			//                return ds;
			//            DataSet resDs = new DataSet();
			//            resDs.Tables.Add(ds.Tables[0].Clone());
			//            resDs.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).ToList().ForEach(q => q.DataType = typeof(int));
			//            foreach (var item in ds.Tables[0].Rows)
			//            {
			//                resDs.Tables[0].ImportRow(item as DataRow);
			//            }
			//            return resDs;
			//        }
			//    default:
			//        return MSSqlHelper.GetReportExportData(reportName, where, systemID, "", type);
			//}

			DataSet ds = MySqlHelper.GetReportExportData(reportName, where, systemID, type);
			if (ds.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).Count() == 0)
				return ds;
			DataSet resDs = new DataSet();
			resDs.Tables.Add(ds.Tables[0].Clone());
			resDs.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).ToList().ForEach(q => q.DataType = typeof(int));
			foreach (var item in ds.Tables[0].Rows)
			{
				resDs.Tables[0].ImportRow(item as DataRow);
			}
			return resDs;
		}
		/// <summary>
		/// 报???表???管??理???查??询??语??句?
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <param name="type"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		public static DataSet GetReportData(string sql, int pageSize, int pageIndex, string Order,
			ReportConnectionType type, out int totalCount)
		{
			//switch (ConfigContext.SqlAss)
			//{
			//    case SqlEnum.MSSql:
			//        return MSSqlHelper.GetReportData(sql, pageSize, pageIndex, Order, type, out totalCount);
			//    case SqlEnum.MYSql:
			//        {
			//            DataSet ds = MySqlHelper.GetReportData(sql, pageSize, pageIndex, Order, type, out totalCount);
			//            if (ds == null || ds.Tables.Count == 0)
			//                return null;
			//            if (ds.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).Count() == 0)
			//                return ds;
			//            DataSet resDs = new DataSet();
			//            resDs.Tables.Add(ds.Tables[0].Clone());
			//            resDs.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).ToList().ForEach(q => q.DataType = typeof(int));
			//            foreach (var item in ds.Tables[0].Rows)
			//            {
			//                resDs.Tables[0].ImportRow(item as DataRow);
			//            }
			//            return resDs;
			//        }
			//    default:
			//        return MSSqlHelper.GetReportData(sql, pageSize, pageIndex, Order, type, out totalCount);
			//}

			DataSet ds = MySqlHelper.GetReportData(sql, pageSize, pageIndex, Order, type, out totalCount);
			if (ds == null || ds.Tables.Count == 0)
				return null;
			if (ds.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).Count() == 0)
				return ds;
			DataSet resDs = new DataSet();
			resDs.Tables.Add(ds.Tables[0].Clone());
			resDs.Tables[0].Columns.Cast<DataColumn>().Where(q => q.DataType == typeof(Int64)).ToList().ForEach(q => q.DataType = typeof(int));
			foreach (var item in ds.Tables[0].Rows)
			{
				resDs.Tables[0].ImportRow(item as DataRow);
			}
			return resDs;
		}

		public DataTable GetDataTable(string sqlQuery)
		{
			DataSet ds = new DataSet();

			//switch (ConfigContext.SqlAss)
			//{
			//    case SqlEnum.MSSql:
			//        SqlConnection conn = new SqlConnection(ConfigContext.reportConnectString);
			//        try
			//        {
			//            string sql = " SELECT  TOP 0 * FROM (" + sqlQuery + ") AS a";
			//            SqlCommand cmd = new SqlCommand(sql, conn);
			//            SqlDataAdapter apd = new SqlDataAdapter(cmd);
			//            apd.Fill(ds);
			//            conn.Close();
			//        }
			//        catch (Exception)
			//        {
			//            if (conn != null)
			//                conn.Close();
			//        }
			//        break;
			//    case SqlEnum.MYSql:
			//        MySqlConnection mconn = new MySqlConnection(ConfigContext.reportConnectString);
			//        try
			//        {
			//            string sql = " SELECT * FROM (" + sqlQuery + ") AS a limit 0";
			//            MySqlCommand cmd = new MySqlCommand(sql, mconn);
			//            MySqlDataAdapter apd = new MySqlDataAdapter(cmd);
			//            apd.Fill(ds);
			//            mconn.Close();
			//        }
			//        catch (Exception)
			//        {
			//            if (mconn != null)
			//                mconn.Close();
			//        }
			//        break;
			//}

			MySqlConnection mconn = new MySqlConnection(ConfigContext.ReportConnectString);
			try
			{
				string sql = " SELECT * FROM (" + sqlQuery + ") AS a limit 0";
				MySqlCommand cmd = new MySqlCommand(sql, mconn);
				MySqlDataAdapter apd = new MySqlDataAdapter(cmd);
				apd.Fill(ds);
				mconn.Close();
			}
			catch (Exception)
			{
				if (mconn != null)
					mconn.Close();
			}
			return ds.Tables[0];
		}
		//sql语??句?判D断?是??否??正y确???
		public bool SqlIsError(string sqlQuery)
		{
			var res = false;
			//switch (ConfigContext.SqlAss)
			//{
			//    case SqlEnum.MSSql:
			//        SqlConnection conn = new SqlConnection(ConfigContext.reportConnectString);
			//        try
			//        {
			//            string sql = " SELECT  TOP 0 * FROM (" + sqlQuery + ") AS a";
			//            SqlCommand cmd = new SqlCommand(sql, conn);
			//            conn.Open();
			//            cmd.ExecuteScalar();
			//            conn.Close();
			//            res = true;
			//        }
			//        catch (Exception)
			//        {
			//            if (conn != null)
			//                conn.Close();
			//            res = false;
			//        }
			//        break;
			//    case SqlEnum.MYSql:
			//        MySqlConnection mconn = new MySqlConnection(ConfigContext.reportConnectString);
			//        try
			//        {
			//            string sql = " SELECT  * FROM (" + sqlQuery + ") AS a limit 0";
			//            MySqlCommand cmd = new MySqlCommand(sql, mconn);
			//            mconn.Open();
			//            cmd.ExecuteScalar();
			//            mconn.Close();
			//            res = true;
			//        }
			//        catch (Exception)
			//        {
			//            if (mconn != null)
			//                mconn.Close();
			//            res = false;
			//        }
			//        break;
			//}

			MySqlConnection mconn = new MySqlConnection(ConfigContext.ReportConnectString);
			try
			{
				string sql = " SELECT  * FROM (" + sqlQuery + ") AS a limit 0";
				MySqlCommand cmd = new MySqlCommand(sql, mconn);
				mconn.Open();
				cmd.ExecuteScalar();
				mconn.Close();
				res = true;
			}
			catch (Exception)
			{
				if (mconn != null)
					mconn.Close();
				res = false;
			}
			return res;
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="where"></param>
		/// <param name="diff"></param>
		/// <returns></returns>
		public static DataSet GetSerialData(List<KeyValue> where, int diff, int step = 1)
		{
			if (where != null)
			{
				foreach (var item in where)
				{
					item.Value = item.Value.Trim();
				}
			}
			string strSql = string.Empty;
			switch (diff)
			{
				case 0:
					strSql = "UPDATE sys_Serial SET CurrentValue=CurrentValue+" + step + ",UpdateTime={0} WHERE RelationTable=@Type and RelationID=@relationid ";
					break;
				default:
					strSql = "UPDATE sys_Serial SET CurrentValue=" + step + ",UpdateTime={0} WHERE RelationTable=@Type and RelationID=@relationid";
					break;
			}
			//switch (ConfigContext.SqlAss)
			//{
			//    case SqlEnum.MSSql:
			//        strSql = string.Format(strSql, "GETDATE()");
			//        return MSSqlHelper.GetSerial(strSql, where);
			//    case SqlEnum.MYSql:
			//        strSql = string.Format(strSql, "NOW()");
			//        return MySqlHelper.GetSerial(strSql, where);
			//    default:
			//        strSql = string.Format(strSql, "GETDATE()");
			//        return MSSqlHelper.GetSerial(strSql, where);
			//}

			strSql = string.Format(strSql, "NOW()");
			return MySqlHelper.GetSerial(strSql, where);
		}
	}


	public enum ReportConnectionType
	{
		Business = 1, 
		Log = 2
	}

	public class XMLID
	{
		public static int SuperAdmin = 1;  //超级管理员
		public static int Admin = 2;   //管理员
		public static int Log = 4;   //日志 备用
	}
}
