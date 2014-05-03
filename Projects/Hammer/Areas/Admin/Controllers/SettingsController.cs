using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Component;
using Entity.Entities;

namespace EasyUI.Areas.Admin.Controllers
{
	[Authorize]
    public class SettingsController : BaseController
    {
        //
        // GET: /Admin/Settings/

		public ActionResult SysConfigView()
        {
			var result = new Helpers.SystemHelper().GetSystemConfig();

            return View(result);
        }

		public ActionResult SysConfigJson(SysConfig param)
		{
			//string a = "{\"total\":4, \"rows\":[{\"ID\":4,\"PhotoUrl\":\"201307/18.jpg\",\"ProductCode\":\"anta002\",\"ProductName\":\"安踏网面鞋网鞋anta男鞋正品2013运动鞋夏透气轻便跑鞋安踏跑步鞋\",\"CategoryName\":\"运动鞋\",\"BrandName\":\"安踏\",\"BrandID\":0,\"Specification\":null,\"Price\":0,\"CategoryID\":0,\"AddUserName\":\"Admin\",\"AddTime\":\"\\/Date(1374635450000)\\/\",\"UpdateUserName\":null,\"UpdateTime\":null,\"IsEnabled\":\"U\",\"IsSetSku\":\"Y\",\"MarketPrice\":229.00,\"ChannelPrice\":null,\"StockNum\":0.000,\"LockNumber\":0.000,\"ReserveNumber\":0.000,\"SkuID\":null,\"ProductID\":null,\"SkuCode\":null,\"SkuBarCode\":null,\"Weight\":null,\"Volume\":null,\"CostPrice\":null,\"Unit\":null,\"SpecName\":null,\"WareHouseName\":null,\"ProductCategoryUrl\":null,\"IsBatch\":null,\"SysSkuCode\":null,\"WareHouseID\":0}],\"footer\":null}";

			//return Json(a);
			var result = new Helpers.SystemHelper().SetSystemConfig(param);

			return Json(result);
		}
    }
}
