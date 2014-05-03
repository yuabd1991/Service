using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Component.Component
{
	public class CReport
	{
		//获取连接串
		internal static string GetConnection()
		{
			return ConfigContext.ReportConnectString;
			//switch (type)
			//{
			//    case ReportConnectionType.Business:
			//        //if (ConfigContext.SqlAss == SqlEnum.MSSql)
			//        return ConfigContext.ReportConnectString;
			//    //else if (ConfigContext.SqlAss == SqlEnum.MYSql)
			//    // return ConfigContext.mySqlReportConnectString;
			//    //return ConfigContext.mySqlReportConnectString;
			//    //case ReportConnectionType.Log:
			//    // return ConfigContext.logReportConnectString;
			//    default:
			//        return ConfigContext.ReportConnectString;
			//}
		}

		//获取报表路径
		internal static string GetReportPath()
		{
			string reportPath = string.Format(ConfigContext.AdminReportPath, System.Configuration.ConfigurationSettings.AppSettings["DBType"].ToString());
			//switch (systemID)
			//{
			//    case 1:

			//        reportPath = string.Format(ConfigContext.AdminReportPath, System.Configuration.ConfigurationSettings.AppSettings["DBType"].ToString());
			//        // reportPath = @"E:\EC-DRP\trunk\Services\U1City.EC-DRPService\Host\Web\bin\Report\AdminReport.xml";
			//        break;
			//}

			return reportPath;
		}
		//type 0 mySql type 1 MsSql
		internal static string GetTotalSql(string reportPath, string reportName, List<KeyValue> where, int type = 0)
		{
			var xDoc = XDocument.Load(reportPath); //加载XML报表参数

			//读取XML获取参数字段信息
			var queryXml = (from q in xDoc.Descendants("report")
							where q.Attribute("id").Value.ToLower() == reportName.ToLower()
							select q).AsQueryable();
			var querySql = queryXml.Elements("sql").FirstOrDefault(); //查出SQL语句
			string totalSql = queryXml.Elements("formatsql").FirstOrDefault().Value; //格式化SQL语句
			#region 语句最后的查询条件
			if (querySql == null) //如果没配置则返回NULL
				return null;

			string uperSql = querySql.Value;

			#region 子查询的查询条件
			var childAttr = queryXml.Elements("childDynamic");

			foreach (var _child in childAttr)
			{
				int cIndex = 1;
				var child = _child.Elements("isNotEmpty");
				if (child == null)
					continue;
				var childEle = _child.Elements("isNotEmpty").ToList();

				//StringBuilder para = new StringBuilder();
				string c_Param = HandleParam(childEle, where, cIndex, type);
				string childEnd = _child.Element("childEnd") == null ? "" : _child.Element("childEnd").Value;
				if (c_Param.Length == 0)
				{
					uperSql = uperSql.Replace(_child.Attribute("property").Value, childEnd);
					Regex.Replace(uperSql, _child.Attribute("property").Value, childEnd, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
					continue;
				}
				uperSql = Regex.Replace(uperSql, _child.Attribute("property").Value, c_Param.Insert(0, _child.Attribute("prepend").Value + " ") + childEnd, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				//uperSql = uperSql.Replace(_child.Attribute("property").Value.ToLower(), c_Param.Insert(0, _child.Attribute("prepend").Value + " ") + childEnd);

			}

			#endregion

			StringBuilder sql = new StringBuilder();
			var attr = queryXml.Elements("dynamic"); //得到设置类型的查询参数 (获取查询条件)
			sql.Append(uperSql);
			if (attr != null && attr.Elements("isNotEmpty").Count() != 0)
			{
				var paraEle = attr.Elements("isNotEmpty").ToList();
				int index = 1;

				string strPara = HandleParam(paraEle, where, index, type); //where 参数


				if (!string.IsNullOrEmpty(strPara))
				{
					sql.Append(strPara.Insert(0, attr.Attributes("prepend").Select(q => q.Value).FirstOrDefault() + " "));
				}
			}
			var endSql = queryXml.Elements("endSql");
			if (endSql.FirstOrDefault() != null)
			{
				sql.Append(endSql.Select(q => q.Value).FirstOrDefault() + " ");
			}
			#endregion
			return string.Format(totalSql, sql.ToString()); //得到查询的SQL语句,去掉XML里面的多余空格 
		}

		internal static string GetSql(string reportPath, string reportName, List<KeyValue> where, ref string sqlCount, int type = 0)
		{

			var xDoc = XDocument.Load(reportPath); //加载XML报表参数

			//读取XML获取参数字段信息
			var queryXml = (from q in xDoc.Descendants("report")
							where q.Attribute("id").Value.ToLower() == reportName.ToLower()
							select q).AsQueryable();
			var querySql = queryXml.Elements("sql").FirstOrDefault(); //查出SQL语句
			var querySqlCount = queryXml.Elements("sqlCount").FirstOrDefault(); //查出SQL计算总数语句

			#region 语句最后的查询条件
			if (querySql == null) //如果没配置则返回NULL
				return null;

			string uperSql = querySql.Value;
			sqlCount = string.Empty;
			if (querySqlCount != null) //计算总数SQL语句
			{
				sqlCount = querySqlCount.Value;
			}
			#region 子查询的查询条件

			var childAttr = queryXml.Elements("childDynamic");

			foreach (var _child in childAttr)
			{
				int cIndex = 1;
				var child = _child.Elements("isNotEmpty");
				if (child == null)
					continue;
				var childEle = _child.Elements("isNotEmpty").ToList();

				//StringBuilder para = new StringBuilder();
				string c_Param = HandleParam(childEle, where, cIndex, type);
				string childEnd = _child.Element("childEnd") == null ? "" : _child.Element("childEnd").Value;
				if (c_Param.Length == 0)
				{
					uperSql = uperSql.Replace(_child.Attribute("property").Value, childEnd);
					Regex.Replace(uperSql, _child.Attribute("property").Value, childEnd,
					System.Text.RegularExpressions.RegexOptions.IgnoreCase);
					if (!string.IsNullOrEmpty(sqlCount))
					{
						sqlCount = sqlCount.Replace(_child.Attribute("property").Value, childEnd);
						Regex.Replace(sqlCount, _child.Attribute("property").Value, childEnd,
						System.Text.RegularExpressions.RegexOptions.IgnoreCase);
					}

					continue;
				}
				uperSql = Regex.Replace(uperSql, _child.Attribute("property").Value,
				c_Param.Insert(0, _child.Attribute("prepend").Value + " ") + childEnd,
				System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				if (!string.IsNullOrEmpty(sqlCount))
				{
					sqlCount = Regex.Replace(sqlCount, _child.Attribute("property").Value,
					c_Param.Insert(0, _child.Attribute("prepend").Value + " ") + childEnd,
					System.Text.RegularExpressions.RegexOptions.IgnoreCase);
				}

			}

			#endregion

			StringBuilder sql = new StringBuilder();
			StringBuilder strSqlCount = new StringBuilder();
			var attr = queryXml.Elements("dynamic"); //得到设置类型的查询参数 (获取查询条件)
			sql.Append(uperSql);
			strSqlCount.Append(sqlCount);
			if (attr != null && attr.Elements("isNotEmpty").Count() != 0)
			{
				var paraEle = attr.Elements("isNotEmpty").ToList();
				int index = 1;
				string strPara = HandleParam(paraEle, where, index, type); //where 参数
				if (!string.IsNullOrEmpty(strPara))
				{
					sql.Append(strPara.Insert(0, attr.Attributes("prepend").Select(q => q.Value).FirstOrDefault() + " "));
					if (!string.IsNullOrEmpty(sqlCount))
					{
						sqlCount += " " + strPara.Insert(0, attr.Attributes("prepend").Select(q => q.Value).FirstOrDefault() + " ");
					}
				}
			}
			var endSql = queryXml.Elements("endSql");
			if (endSql.FirstOrDefault() != null)
			{
				string tempEndSql = endSql.Select(q => q.Value).FirstOrDefault().ToLower();
				string groupByPattern = @"group\s+by";
				string orderByPattern = @"order\s+by\s+.+";
				string periodPattern = @"\s+[a-zA-Z]+\.";//匹配a.ID

				if (!Regex.IsMatch(tempEndSql, @"order\s+by"))//不包含order by
				{
					sql.Append(tempEndSql + " ");
				}
				else if (Regex.IsMatch(tempEndSql, groupByPattern))//包含order by 并且包含group by 
				{
					tempEndSql = Regex.Replace(tempEndSql, orderByPattern, string.Empty);
					sql.Append(tempEndSql + " ");
				}
				else//包含order by 但不包含group by
				{
					tempEndSql = Regex.Replace(tempEndSql, periodPattern, " ");
					sql.Append("~" + tempEndSql + " ");
				}
			}

			#endregion

			return sql.ToString(); //得到查询的SQL语句,去掉XML里面的多余空格

		}

		//处理所有参数以及Where的值 (type:0 mySql type 1 msSql)
		private static string HandleParam(List<XElement> childEle, List<KeyValue> where, int index, int SqlType = 0)
		{
			StringBuilder para = new StringBuilder();
			foreach (var ele in childEle)
			{
				if (ele.Attribute("value") != null && ele.Attribute("value").Value.ToLower() == "fixed")
				{
					para.Append(" " + GetStr(index, ele.Value, ele.Attribute("prepend").Value));
					index++;
					continue;
				}
				var ctn = where.Where(q => q.Key.ToLower() == ele.Attribute("property").Value.ToLower()).FirstOrDefault(); //判断查询条件是否存在
				if (ctn == null)
					continue;
				string type = "";
				if (ele.Attribute("type").Value != null)
				{
					type = ele.Attribute("type").Value.ToLower();
				}
				//in条件特殊处理
				if (type == "in")
				{
					string[] inValue = ctn.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(n => n).ToArray();
					string strValue = string.Join(",", inValue);

					List<string> p = new List<string>();
					for (int i = 0; i < inValue.Count(); i++)
					{
						where.Add(new KeyValue { Key = ctn.Key + i, Value = inValue[i] });
						p.Add("@" + ctn.Key + i);
					}

					if (index == 1)
					{
						para.Append(" " + ele.Value.ToLower().Replace("@" + ctn.Key.ToLower(), string.Join(",", p)));
					}
					else
					{
						para.Append(" " + ele.Attribute("prepend").Value);
						para.Append(" " + ele.Value.ToLower().Replace("@" + ctn.Key.ToLower(), string.Join(",", p)));
					}
					where.Remove(ctn);
					index++;
					continue;
				}

				else if (type == "like")
				{
					if (ctn.Value.Contains("%") || ctn.Value.Contains("_"))
					{
						ctn.Value = ctn.Value.Replace("%", @"\%").Replace("_", @"\_");
					}
					para.Append(" " + GetStr(index, ele.Value, ele.Attribute("prepend").Value));

					index++;
					continue;
				}
				else if (type == "limit")
				{
					string[] inValue = ctn.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(n => n).ToArray();
					string strValue = string.Join(",", inValue);
					para.Append(" " + ele.Value.ToLower().Replace("@" + ctn.Key.ToLower(), strValue.Trim(',')));
					continue;
				}
				else if (type == "begintime")
				{
					if (!string.IsNullOrEmpty(ctn.Value))
					{
						string strValue = Convert.ToDateTime(ctn.Value).ToShortDateString();
						para.Append(" " + ele.Value.ToLower().Replace("@" + ctn.Key.ToLower(), strValue.Trim(',')));
						continue;
					}
				}
				else if (type == "endtime")
				{
					if (!string.IsNullOrEmpty(ctn.Value))
					{
						string strValue = Convert.ToDateTime(ctn.Value).AddDays(1).AddSeconds(-1).ToString();
						para.Append(" " + ele.Value.ToLower().Replace("@" + ctn.Key.ToLower(), strValue.Trim(',')));
						continue;
					}

				}
				para.Append(" " + GetStr(index, ele.Value, ele.Attribute("prepend").Value));
				index++;
			}
			return para.ToString();
		}

		//type sql的查询条件，index，where 当1的时候紧跟where后面 strIn in的时候值带入SQL语句,XML中的语句,prepend 语句中的关键字（and ,or ）
		private static string GetStr(int index, string eleStr, string prepend)
		{
			string para = string.Empty;
			if (index == 1)
			{
				return eleStr;
			}
			else
			{
				return prepend + " " + eleStr;
			}
		}
		//获取COUNT的SQL语句
		internal static string GetStrCount(string sql)
		{
			//匹配SELECT
			// var selectArray = Regex.Matches(sql, @"^\sSelect\s+", RegexOptions.IgnoreCase);
			sql = " " + sql.Replace("\n", " ").Replace("\r", " ");
			List<int> selectList = new List<int>();
			List<int> fromList = new List<int>();
			string sqlUpper = sql.ToUpper();
			GetIndex(sqlUpper, 0, 0, selectList, fromList);
			int index = fromList.FirstOrDefault();
			List<int> list = new List<int>();
			list.Add(selectList[0]);
			selectList.Remove(selectList[0]);
			int id = 100;
			while (list.Count > 0)
			{
				id--;
				if (id < 0)
					break;
				if (selectList.Count == 0 && list.Count > 1)
				{
					list.Remove(list.Last());
					fromList.Remove(fromList[0]);
				}
				else if (selectList.Count == 0 && list.Count == 1)
				{
					index = fromList[0];
					list.Clear();
					break;
				}
				else
				{
					if (selectList[0] > fromList[0]) //选择的位置>from的位置 //from 入栈
					{
						if (list.Count == 1) //只有一个SELECT 则找到匹配的from
						{
							index = fromList[0];
							list.Clear();
							break;
						}
						else //否则把select 与from 弹出list
						{
							list.Remove(list.Last());
							fromList.Remove(fromList[0]);
						}
					}
					else
					{
						list.Add(selectList[0]);
						selectList.Remove(selectList[0]); //移掉入栈的数据
					}
				}

			}
			if (sqlUpper.IndexOf("GROUP") > 0 || sqlUpper.IndexOf("DISTINCT") > 0)
			{
				return "Select Count(*) from ( " + sql + ") ttCount";
			}
			else
			{
				return " Select Count(*) " + sql.Substring(index);
			}
		}

		private static void GetIndex(string sql, int indexF, int indexS, List<int> selectList, List<int> fromList)
		{

			// CompareInfo Compare = CultureInfo.InvariantCulture.CompareInfo;
			//var indexSelect = Compare.IndexOf(sql, " SELECT ", indexS, CompareOptions.IgnoreCase);
			//var indexFrom = Compare.IndexOf(sql, " FROM ", indexF, CompareOptions.IgnoreCase);
			var indexSelect = sql.IndexOf("SELECT ", indexS);
			var indexFrom = sql.IndexOf("FROM ", indexF);
			if (indexSelect > 0)
			{
				selectList.Add(indexSelect);
				indexS = indexSelect + 8;
			}
			if (indexFrom > 0)
			{
				fromList.Add(indexFrom);
				indexF = indexFrom + 8;
			}
			if (indexSelect == -1 && indexFrom == -1)
				return;

			GetIndex(sql, indexF, indexS, selectList, fromList);
		}


	}
}
