
/// <reference path="../../Scripts/jquery.js" />

var U1 = {};



$.fn.U1CC = function () {
	var str = "<li class=\"U1CC\"><span title=\"关键字\" class=\"title\"><select class=\"u1cc_select\" onchange=\"U1.conditionChange(this)\">";
	var fitst = "";
	$(".ul_hide").find("li").each(function (i, value) {
		var title = $(value).attr("title");
		str = str + "<option value=\"" + title + "\">" + title + "</option>";
		if (fitst == "") {
			fitst = $(value).html();
		}
	});
	str = str + "</select>：</span>\r\n" + fitst + "</li>";

	$(this).children().first().prepend($(str));
	// $("li.U1CC [col]").attr("col-text", "select.u1cc_select");
};

U1.conditionChange = function (obj) {
	var value = $(obj).val();

	var parentLi = $(".ul_hide").find("li[title=" + value + "]");
	var htmlNew = parentLi.html();

	// to fix bug: 6307, yrk.
	// 默认启用。
	var keepValueFromFirstInput = true; //$(".ul_hide").attr("keepVal") == "true";

	try {
		var originalValue = "";
		if (keepValueFromFirstInput) {
			originalValue = $(obj).parent().parent().find("input[type='text']").val();
		}
	} catch (e) {

	}

	$($(".searchBar .U1CC").children()[1]).remove();
	$(".searchBar .U1CC").append(htmlNew);

	try {
		if (originalValue != "") {
			var inputBox = $(".searchBar .U1CC").find("input[type='text']");
			if (inputBox.length == 0) {
				$(inputBox).attr("value", originalValue);
			} else {
				$(inputBox).val(originalValue);
			}
		}
	} catch (e) {

	}
	// $("li.U1CC [col]").attr("col-text", "select.u1cc_select");
};

U1.U1EasyGrid_Defaults_pageSize = 50;
U1.IsEnableEnterKeySearching = true; // 全局控制-列表页 回车搜索。

U1.positionControl = true;
var U1EasyGrid_Defaults =
{
	width: "auto",
	height: "auto",
	striped: true,
	idfield: "ID",
	pageSize: U1.U1EasyGrid_Defaults_pageSize,
	fit: true,
	fitColumns: true,
	pagination: true,
	singleSelect: true,
	ShowTip_href: false, //初始化tip
	nowrap: false,
	togglebtn: true, //搜索框 收缩/展开
	pageList: [10, 20, 50, 100],
	pageTag: pageTag,
	//工具栏
	toolbar: [
            ],
	//选择列
	onHeaderContextMenu: function (e, field) {
		DataGridExtend.headerContextMenu(e, field, $(this).attr('id'), pageTag + '.' + $(this).attr('id'));
	},
	//加载成功的时候，初始化隐藏列
	onLoadSuccess: function (data) {
		DataGridExtend.loadSuccessInit($(this).attr('id'), pageTag + '.' + $(this).attr('id'));

		if (data.Tag)
			alert("请求数据出错了,请刷新页面重新尝试！");

		if ($.data(this, 'datagrid').options.ShowTip_href)
			DataGridExtend.ShowTip_href();
		if (typeof (setToolMsg) != "undefined") {
			setToolMsg(data);
		}
		if (this.onLoadSuccess) {
			this.onLoadSuccess(data);
		}
		//权限控制
		InitPageRightKey(PageBtnList); //最后执行初始化

		// 列表的回到顶部功能。
		//U1.initializePositionControl();
	},
	onLoadError: function () {
	},
	onClickRow: function (rowIndex, rowData) {
		if (this.onLoadSuccess) {
			this.onClickRow(rowIndex, rowData);
		}
	},
	onDblClickRow: function (rowIndex, rowData) {
		if (this.onLoadSuccess) {
			this.onDblClickRow(rowIndex, rowData);
		}
	},
	onSelect: function (rowIndex, rowData) {
		if (this.onLoadSuccess) {
			this.onSelect(rowIndex, rowData);
		}
	},
	onUnselect: function (rowIndex, rowData) {
		if (this.onLoadSuccess) {
			this.onUnselect(rowIndex, rowData);
		}
	},
	onRowContextMenu: function (e, rowIndex, rowData) {
		if (this.onLoadSuccess) {
			this.onRowContextMenu(e, rowIndex, rowData);
		}
	}

}


$.fn.U1EasyGrid = function (gridOptions) {

	var p = $.extend({}, U1EasyGrid_Defaults, gridOptions || {});
	p.pageSize = U1.pagerSizeGet();

	$grid = $(this);

	p.url = getParUrl(p.url);

	$(this).datagrid(p);

	//搜素栏
	var searchBar = $(".searchBar");
	if (searchBar != null && searchBar.length > 0) {
		var html = "<div class=\"searchBarContainer\" style='overflow: hidden;' ><table cellpadding=\"0\" cellspacing=\"0\" ><tr><td class=\"searchBarBody\"></td><td class='searchBarBody-col' style=\"width: 120px;vertical-align:top;\"><a id=\"sb\" iconCls=\"icon-search\" plain=\"false\" href=\"javascript:void(0)\" onclick=\"U1.__doSearch()\">搜索</a></td></tr></table><div class=\"togglebtn togglebtn-down\" title='收起'></div></div>";
		html += "<div id=\"mm\" style=\"width:100px;display:none;\"></div>";
		$(".datagrid-toolbar").append(html);
		$(".searchBarBody").append(searchBar);
		var splitbuttonEnable = false;
		/*
		if (searchBar.attr("searchsetting") == "true") {
		$("#mm").append("<div iconCls=\"icon-setting\" onclick=\"U1.searchSet()\">搜索设置</div>");
		$("#mm").append("<div class=\"menu-sep\"></div>");
		splitbuttonEnable = true;
		//搜索设置
		if (typeof (searchNameList) != "undefined") {
		searchNameList(pageTag);
		}

		}*/

		if (splitbuttonEnable) {
			$("#sb").splitbutton({
				menu: '#mm'
			});
		} else {
			$("#sb").linkbutton();
		}
		searchBar.show();
	}
	//messageBar
	$(".datagrid-toolbar").append($(".messageBar"));
	//工具栏
	$(".datagrid-toolbar").append($(".toolBar"));

	var grid_id = $(this).attr("id");
	$(window).resize(function () {
		$("#" + grid_id).datagrid("resize");
	});

	if (p.togglebtn) {
		//搜索框 收缩/展开
		$(".togglebtn").css({ cursor: "hand", cursor: "pointer" })
        .live("click", function () {
        	var btn = $(this);
        	var searchbox = $(".searchBarContainer");
        	if (btn.hasClass("togglebtn-down")) {
        		btn.removeClass("togglebtn-down").attr("title", "收起");
        		if (searchbox.height() > 35)
        			searchbox.height(24);
        	}
        	else {
        		btn.addClass("togglebtn-down").attr("title", "展开");
        		searchbox.height("auto");
        	}
        	$("#" + grid_id).datagrid("resize");
        });
	} else {
		$(".togglebtn").hide();
	}

	if (U1.IsEnableEnterKeySearching) {
		// to fix bug 5125
		$(".searchBar :input[type=text]").live("keydown", function (e) {
			if (e.keyCode == 13) {
				Search();
			}
		});
	}

	$("#" + grid_id).datagrid("resize");

	var pager = $(this).datagrid('getPager');
	if (pager) {
		$(pager).pagination({
			onChangePageSize: function (pageSize) {
				U1.pagerSet(pageSize);
			}

		});
	}

};

U1.pagerSizeGet = function () {
	var cookieid = "U1city.Ecdrp.Pager";
	var pageStr = DataGridExtend.getcookie(cookieid);
	var pageSiteList = pageStr.split(';');
	for (var i = 0; i < pageSiteList.length; i++) {
		var itemList = pageSiteList[i].split('=');
		if (itemList[0] == pageTag) {
			return itemList[1];
		}
	}
	return U1.U1EasyGrid_Defaults_pageSize;

}

U1.pagerSet = function (pageSize) {

	var cookieid = "U1city.Ecdrp.Pager";
	var pageStr = DataGridExtend.getcookie(cookieid);
	var pageSiteList = pageStr.split(';');
	for (var i = 0; i < pageSiteList.length; i++) {
		var itemList = pageSiteList[i].split('=');
		if (itemList[0] == pageTag) {
			pageStr = pageStr.replace(pageSiteList[i] + ";", "");
		}
	}
	if (U1.U1EasyGrid_Defaults_pageSize != pageSize) {
		var pageStr = pageTag + "=" + pageSize.toString() + ";" + pageStr;
	}
	DataGridExtend.setcookie(cookieid, pageStr);

}

U1.searchSet = function () {
	var searchBar = $(".searchBar");
	if (searchBar == null || searchBar == 'undefined') {
		return;
	}

	var titles = "";
	if ($(".searchBar ul li.U1CC").length > 0) {
		titles = titles + "关键字：";
	}
	if ($(".categoryli").length > 0) {
		titles = titles + "分类：";
	}
	var data = $(".searchBar ul li:not(.U1CC)");
	data.each(function (i, val) {
		titles = titles + $(val).find(".title").html();
	});

	art.dialog.data('titles', titles);

	PW.getWindow('../../Control/SearchBarSettingView?pageTag=' + pageTag, '搜索设置', 550, 280);

};

var grid_search_rload = false; //全局变量，判断是否刷新到第一页

U1.__doSearch = function () {
	grid_search_rload = true;
	var validMsg = checkSearchCondition($(".searchBar")); // 搜索区验证。
	var valid = validMsg != null && validMsg.length == 0;
	// 执行搜索
	if (valid) {
		Search();
	}
	else {
		var msg = "<strong>无法进行搜索！</strong><br/><br/>" + validMsg.join('<br/>');

		$.messager.alert('搜索', msg, null, null, false, true);
	}
};

function checkSearchCondition(area) {
	var validMessage = new Array();
	var validResult = true;
	// 验证结束日期（时间）必须大于等于起始日期（时间）。
	var begintTimeCol = null;
	var endTimeCol = null;
	var subjectTitle = "";
	if (area != null && area != undefined) {
		begintTimeCol = $(area).find(".col-begintime");
	}
	if (begintTimeCol != null && begintTimeCol != undefined && begintTimeCol.length > 0) {

		$(begintTimeCol).each(function () {
			begintTimeCol = $(this);
			var containerParent = $(this).parent();

			if (containerParent != null && containerParent != undefined) {
				subjectTitle = containerParent.find(".title").text();

				if (subjectTitle == null || $.trim(subjectTitle).length == 0) {
					subjectTitle = containerParent.find("span:first").text();
				}

				if (subjectTitle == null || $.trim(subjectTitle).length == 0) {
					subjectTitle = containerParent.text();
				}
			}

			subjectTitle = subjectTitle.replace("至", "").replace("：", "").replace('*', '')
                    .replace(':', '');
			var compareTimeTo = begintTimeCol.attr("compareTimeTo");
			if (compareTimeTo != null && compareTimeTo != "") {
				endTimeCol = $(compareTimeTo);
			}
			else {
				endTimeCol = $(containerParent).find(".col-endtime");
			}

			var beginTimeVal = $(begintTimeCol).val();
			var endTimeVal = "";
			if (endTimeCol != null && endTimeCol != undefined) {
				endTimeVal = $(endTimeCol).val();
			}

			if (($.trim(beginTimeVal).length == 0) || ($.trim(endTimeVal).length == 0)) {
			} else {
				// compare time.
				var m_beginDate = new Date(Date.parse(delimiterConvert(beginTimeVal)));
				var m_endDate = new Date(Date.parse(delimiterConvert(endTimeVal)));

				var isPassed = m_beginDate > m_endDate;
				if (isPassed == true) {
					var tag = subjectTitle.indexOf('时间') > 0 ? "时间" : "日期";

					validMessage.push("必需满足【" + subjectTitle + "】的 '结束" + tag + "' ≥ '起始" + tag + "'。");
					validResult = false;
				}
			}
		});
	}

	// end.
	return validMessage;
}

function delimiterConvert(val) {
	return val.replace('-', '/').replace('-', '/')
}

U1.initializePositionControl = function () {
	// 控制是否使用位置控制（回到顶部）。
	if (U1.positionControl) {
		//添加回到顶部的控制区域。
		$(".datagrid-view").remove(".PagePostionController")
        .append($("<div id='PagePostionController1' class='PagePostionController'></div>")
         .click(function () {
         	$(".datagrid-body").scrollTop(0);
         })
        );
		$(".datagrid-body").scroll(function () {
			var nScrollheight = $(this)[0].scrollHeight;
			var nClientHeight = $(this)[0].clientHeight;
			var nDivHight = $(this).height();
			var nScrollTop = $(this)[0].scrollTop;

			if (nScrollheight > nClientHeight) {
				var sHeight = nDivHight + nScrollTop;
				if (sHeight >= nScrollheight ||
                    (nScrollTop >= 500) // 或者超过 500 高度的时候也显示
                ) {
					$("#PagePostionController1").show();
				} else {
					$("#PagePostionController1").hide();
				}
			}
		});
	}
}