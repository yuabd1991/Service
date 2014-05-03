using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Component
{
	public class DataGridJson
	{
		public DataGridJson(int m_total, object m_rows)
		{
			total = m_total;
			rows = m_rows;
		}

		public DataGridJson(int m_total, object m_rows, object m_footer)
		{
			total = m_total;
			rows = m_rows;
			footer = m_footer;
		}

		public int total { get; set; }
		public object rows { get; set; }
		public object footer { get; set; }
	}
	public class DataGridParameters
	{
		public DataGridParameters()
		{
			page = 1;
			rows = 100;
		}
		public string order { get; set; }
		public int page { get; set; }
		public int rows { get; set; }
		public string sort { get; set; }

		public Dictionary<string, object> GetPageFilter(string filterSql)
		{
			Dictionary<string, object> Filter = new Dictionary<string, object>();
			Filter.Add("pageIndex", page - 1);
			Filter.Add("pageSize", rows);
			Filter.Add("sort", (sort + " " + order).Trim());
			Filter.Add("filterSql", filterSql);

			return Filter;
		}
	}


	public class SeniorParameter
	{
		public string type;
		public string fiter;
		public string op;
		public object value;
	}

	public class DataGridHelper<T>
	{
		public List<T> GetResult(List<T> t, GetReportDataParams param)
		{
			var count = t.Count();
			if (param.PageSize > count)
			{
				return t;
			}
			else
			{
				var tResult = t.Skip((param.PageIndex - 1)*param.PageSize).Take(param.PageSize).ToList();

				return tResult;
			}
		}
	}
}