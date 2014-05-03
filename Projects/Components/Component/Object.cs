using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Component
{
	public class KeyName
	{
		public int ID { set; get; }

		public string Name { set; get; }

		//public string Pic { set; get; }
	}

	public class KeyValue
	{
		public KeyValue()
		{

		}
		public KeyValue(string key, string value)
		{
			Key = key;
			Value = value;
		}
		public string Key { get; set; }
		public string Value { get; set; }
	}
	/// <summary>
	/// int+decimal
	/// </summary>
	public class KeyAmount
	{
		/// <summary>
		///  总数
		/// </summary>
		public int Key { get; set; }
		/// <summary>
		/// decimal amount
		/// </summary>
		public decimal Amount { get; set; }
		public decimal PayAmount { get; set; }
	}
	/// <summary>
	/// int+int
	/// </summary>
	public class KeySNum
	{
		public int Key { get; set; }
		public int SNum { get; set; }
	}
	public class KeyValue<T>
	{
		public string Key { get; set; }
		public string Value { get; set; }
		public T Main { get; set; }
	}
	/// <summary>
	/// string_int
	/// </summary>
	public class KeyStNum
	{
		public string Key { get; set; }
		public int Num { get; set; }
	}
	/// <summary>
	/// 图表数据源
	/// </summary>
	public class JqplotData
	{
		public object Key { get; set; }
		public object Value { get; set; }
		public string TypeName { get; set; }
	}

	public class AddPutDownDetailParams
	{
		public List<int?> OutWareBillIDs { get; set; }
		public string PutDownBillCode { get; set; }
		public int CurrentId { get; set; }
		public string CurrentName { get; set; }
	}
	/// <summary>
	///  简单商品信息
	/// </summary>
	public class SimpleProInfoPara
	{
		public int WareHouseID { get; set; }
		public int ProductID { get; set; }
		public int SkuID { get; set; }
	}
	
	public class GetReportDataParams : ReportDataParamsBase
	{
		/// <summary>
		/// 报表名称
		/// </summary>
		
		public string ReportName { get; set; }
		/// <summary>
		/// 排序字段
		/// </summary>
		
		public string Order { get; set; }
		
		public int UserID { get; set; }

		public GetReportDataParams()
		{
			this.PageSize = 10;
			this.Where = new List<KeyValue>();
		}
	}
	
	public class GetReportExportDataParams : ReportDataParamsBase
	{
		
		public string ReportName { get; set; }
		
		public string Order { get; set; }
		
		public int UserID { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	
	public class ReportDataParamsBase
	{
		/// <summary>
		/// 页面大小
		/// </summary>
		
		public int PageSize { get; set; }
		/// <summary>
		/// 页面索引
		/// </summary>
		
		public int PageIndex { get; set; }
		
		public List<KeyValue> Where { get; set; }
	}


	[Serializable]
	public class GetKPITemplateData
	{
		public List<KPITemplate> KPITemplates { get; set; }
	}
	[Serializable]
	public class KPITemplate
	{
		/// <summary>
		/// 序号
		/// </summary>
		public int Sort { get; set; }
		/// <summary>
		/// 标题
		/// </summary>
		public string KPIName { get; set; }
		/// <summary>
		/// 说明
		/// </summary>
		public string KPIExplain { get; set; }
		/// <summary>
		/// 渠道商
		/// </summary>
		public int ChlBusinessID { get; set; }
		/// <summary>
		/// 总分
		/// </summary>
		public string TotalScore { get; set; }
		/// <summary>
		/// 进度
		/// </summary>
		public string TemPerformance { get; set; }
		/// <summary>
		/// 是否允许编辑
		/// </summary>
		public string IsAllowEdit { get; set; }
		/// <summary>
		/// 单位
		/// </summary>
		public string TemPerformanceUntil { get; set; }
		/// <summary>
		/// 公式
		/// </summary>
		public string Formula { get; set; }
		/// <summary>
		/// 是否启用
		/// </summary>
		public string IsUser { get; set; }

	}

	/// <summary>
	/// 表示包含分页信息的 <see cref="List&lt;T&gt;"/> 对象。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PageList<T>
	{

		public int Result { set; get; }

		public string Message { set; get; }
		/// <summary>
		/// 获取或设置包含的实际的数据集。
		/// </summary>
		public List<T> Data { get; set; }

		/// <summary>
		/// 获取或设置一次查询的总记录数（用于分页）。
		/// </summary>
		public int TotalCount { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		/// <param name="totalCount"></param>
		public PageList(List<T> data, int totalCount = 0, int tag = 1, string message = "")
		{
			this.TotalCount = totalCount;
			this.Data = data;
			this.Result = tag;
			this.Message = message;
		}

	}

	/// <summary>
	/// 
	/// </summary>
	public static class PageListExtension
	{
		/// <summary>
		/// 将 <see cref="IEnumerable&lt;T&gt;"/> 类型的集合转换成 <see cref="PageList&lt;T&gt;"/> 对象。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		public static PageList<T> AsPageList<T>(this IEnumerable<T> data, int totalCount = -1, int result = 1, string message = "")
		{

			var list = data.ToList();
			if (totalCount == -1)
			{
				totalCount = list.Count;
			}

			return new PageList<T>(list, totalCount, result, message);
		}



		///// <summary>
		///// 将 <see cref="IEnumerable&lt;T&gt;"/> 类型的集合转换成 <see cref="PageList&lt;T&gt;"/> 对象。
		///// </summary>
		///// <typeparam name="T"></typeparam>
		///// <param name="data"></param>
		///// <param name="totalCount"></param>
		///// <returns></returns>
		//public static PageList<T> AsPageList<T>(this BaseObject<T> obj, int totalCount = 0)
		//{
		//    return new PageList<T>(obj as BaseObject<T>, totalCount);
		//}


	}
}