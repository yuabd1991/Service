

/// <reference path="../../Resource/Scripts/jquery.js" />
/// <reference path="../../Resource/Scripts/jquery.easyui.js" />

function guidGenerator() {
	var S4 = function () {
		return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
	};
	return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}


//文件下载
function ExeclDownload(url, data) {
	url = getParUrl(url);
	var myForm = document.createElement("form");
	myForm.method = "post";
	myForm.action = url + "&did=" + guidGenerator();
	myForm.target = "_blank";
	if ($.isArray(data)) {
		$.each(data, function (i, n) {
			var myInput = document.createElement("input");
			myInput.setAttribute("type", "text");
			myInput.setAttribute("name", n.name);
			myInput.setAttribute("value", n.value);
			myForm.appendChild(myInput);
		});
	} else {
		$.each(data, function (i, n) {
			var myInput = document.createElement("input");
			myInput.setAttribute("type", "text");
			myInput.setAttribute("name", i);
			myInput.setAttribute("value", n == undefined ? "" : n);
			myForm.appendChild(myInput);
		});
	}
	document.body.appendChild(myForm);
	myForm.submit();
	document.body.removeChild(myForm);
}


function getGridRowString(grid, datarow) {
	var opts = $.data($("#" + grid).get(0), 'datagrid').options; //获得配置
	var row = JSON.parse(JSON.stringify(datarow));
	var fcol = new Array();
	for (var i = 0; i < opts.columns.length; i++) {
		var cols = opts.columns[i];
		for (var j = 0; j < cols.length; j++) {
			var col = cols[j];
			if (col.formatter != null && !col.notFmt) {
				fcol.push(col);
			}
		}
	}
	//格式
	for (var i = 0; i < fcol.length; i++) {
		for (var j = 0; j < row.length; j++) {
			var name = fcol[i].field;
			row[j][name] = $("<span>" + fcol[i].formatter(row[j][name], row[j]) + "</span>").text();
		}
	}
	return JSON.stringify(row);
}



function getParUrl(url, par) {
	if (url.indexOf("?") == -1) {
		if (par) {
			url = url + "?" + par;
		}
		else {
			url = url;
		}

	}
	else {
		var baseUrl = url.split('?')[0];
		var paraString = url.split('?')[1];
		if (paraString.indexOf("ua=") == -1) {
			if (par) {
				url = baseUrl + "?" + paraString + "&" + par;
			}
			else {
				//				url = baseUrl + "?" + paraString + "&ua=" + parent.active_UserName();
				url = baseUrl + "?" + paraString;
			}

		}
	}
	return url;
}

//获取url中的参数
//function getQueryString(url,name) {
//    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
//    var r = url.substr(1).match(reg);
//    if (r != null) return unescape(r[2]); return null;
//}


var request =
{
	QueryString: function (val) {
		var uri = window.location.search;
		var re = new RegExp("" + val + "=([^&?]*)", "ig");
		return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
	}
};


var PW = {};
PW.win = parent.PW_father;
var PWW = {};
PWW.win = parent.PW_father_father;

/* 弹出窗口 */


PW.getWindow = function (url, title, width, height) {
	//this.win.showUrlWindow(url, title, width, height);
	url = getParUrl(url);
	_Win.getWindow({ url: url, title: title, width: width, height: height });
};

PW.getConfigWindow = function (Config) {
	//Config.close   Function 回调函数
	//this.win.showUrlWindow(url, title, width, height);
	Config.url = getParUrl(Config.url);
	_Win.getWindow({ url: Config.url, title: Config.title, width: Config.width, height: Config.height, close: Config.close });
};

/* 关闭弹出窗口 */
PW.closeWindow = function () {
	//this.win.closeWindow();
	_Win.close();
};

/*在新窗体中弹出页面*/
function openWinPage(url, openType) {
	var dom = document.createElement('a');
	dom.setAttribute('href', url);
	dom.setAttribute('target', openType);
	document.body.appendChild(dom);
	dom.click();
	document.body.removeChild(dom);
}


/* 获取弹出窗口或者tab容器 的文档对象 */
function getCD() {
	return parent.getSelectPageDoc()
};
/* 获取弹出窗口或者tab容器 的Iframe对象 */
function getIframe() {
	return parent.getSelectPageIframe();
}
/* 获取tab 的文档对象 */
function getCDByTitle(title) {
	return parent.getSelectPageDocByTitle(title);
};
/* 获取tab 的Iframe对象 */
function getIframeByTitle(title) {
	return parent.getSelectPageIframeByTitle(title);
}
/* 在tab中打开新页面,并延迟加载，当点击页签的时候，才加载 */
function addTab(title, url) {
	url = getParUrl(url);
	parent.MF.AddTab(title, url);
}

/* 在tab中打开新页面 selected:是否默认选中它 默认选中 modify xunm 2013-05-23 20:00*/
function openPage(title, url, selected) {
	url = getParUrl(url);

	var template = parent.MF.template();
	var content = template.replace("[src]", url);
	if (selected == undefined || selected == null || selected.length <= 0) {
		selected = true;
	}
	parent.MF.OpenTabPage(title, content, selected, true);
};


//关闭当前tab
function closeSelectTab(title) {
	setTimeout(function () {
		parent.MF.ReloadTab(decodeURI(title));
	}, 10);
};
//关闭指定tab
function closeTab(title) {
	parent.MF.closeTab(title);
};

//通过索引获得tab的CD
function getCDByIndex(index) {
	return parent.getTabDocByIndex(index);
}

//获得当前tab标题
function getTabTitle() {
	return encodeURI(parent.MF.GetSelectedTabTitle());
}

//修改当前TAB
function updateTab(url, title) {
	url = getParUrl(url);
	parent.MF.updateTab(url, title);
};

//选中tab
function selectTab(title) {
	parent.MF.select(title);
}
//重新刷新页面
var refreshPage = function () {
	location.href = location.href;
};

//导出excel
var exportExcel = function (url) {
	location.href = getParUrl(url);
};

/*----------------------------------------  图片   -------------------------------------------*/

var Img = {};
Img.formatImageUrl = function (imgUrl, width, height, style) {
	if (imgUrl) {
		if (imgUrl.indexOf("http://") >= 0) {
			return imgUrl;
		}
		var array = imgUrl.split('.');
		var result = "<img src='../../Content/upload/images/"
                    + array[0] + ".jpg' width='" + width + "' height='" + height + "' style='" + style + "'/>";
		return result;

	} else {
        return "../../Content/upload/images/NoImage.jpg";
	}

};
Img.formatOriginalImageUrl = function (imgUrl) {
	var array = imgUrl.split('.');
	var result = "../../Content/upload/images/" + array[0] + ".jpg";
	return result;
};


/*------------------------------------------判断验证-----------------------------------------------*/
//整数
function isNum(s) {
	var r = /^\d+$/;
	return r.test(s);
}
//价格
function isCurrency(s) {
	return (/^\d+(\.\d+)?$/).test(s);
}

//权限判断
function Permission(BtnList, key) {
	for (var i = 0; i < BtnList.length; i++) {
		if (key == BtnList[i])
			return true;
	}
	return false;
}


/*----------------------------------------------新弹出窗口---------------------------------------*/
var _Win = {};
//弹出窗口
_Win.getWindow = function (config) {
	config = config || {};
	config.id = "artWin";
	// 合并默认配置
	var url = config.url;
	//标记
	if (config.width > document.body.clientWidth)
		config.width = (document.body.clientWidth + 200) * 0.9;
	if (config.height > document.body.clientHeight)
		config.height = (document.body.clientHeight + 200) * 0.85;
	var defaults = $.artDialog.defaults;
	for (var i in defaults) {
		if (config[i] === undefined) config[i] = defaults[i];
	};
	if (url)
		$.dialog.open(url, config, false);
};

//关闭所有弹出窗口
_Win.closeAll = function () {
	var list = $.dialog.list;
	for (var i in list) {
		list[i].close();
	};
};
//根据id关闭窗口 如果不传递id，更具默认id
_Win.close = function () {
	setTimeout(function () {
		$.dialog.close();
	}, 50);
	//$.dialog.close();
};
/*----------------------------------------------消息提醒---------------------------------------*/

_Win.notice = function (options) {
	var opt = options || {},
        api, aConfig, hide, wrap, top,
        duration = 800;

	var config = {
		id: 'Notice',
		left: '100%',
		top: '100%',
		fixed: true,
		drag: false,
		resize: false,
		follow: null,
		lock: false,
		init: function (here) {
			api = this;
			aConfig = api.config;
			wrap = api.DOM.wrap;
			top = parseInt(wrap[0].style.top);
			hide = top + wrap[0].offsetHeight;

			wrap.css('top', hide + 'px')
                .animate({ top: top + 'px' }, duration, function () {
                	opt.init && opt.init.call(api, here);
                });
		},
		close: function (here) {
			wrap.animate({ top: hide + 'px' }, duration, function () {
				opt.close && opt.close.call(this, here);
				aConfig.close = $.noop;
				api.close();
			});

			return false;
		}
	};

	for (var i in opt) {
		if (config[i] === undefined) config[i] = opt[i];
	};

	return $.dialog(config);
};

var notice = function (config) {
	_Win.notice({
		title: config.title,
		width: 220, // 必须指定一个像素宽度值或者百分比，否则浏览器窗口改变可能导致artDialog收缩
		content: config.content,
		icon: config.icon,
		time: 2
	});
};


/*----------------------------------------------按钮权限---------------------------------------*/
var InitPageRightKey = function (userPageKeys) {
	if (userPageKeys.length == 0 && RoldeBtnCount == "0")
		return;
	$("body a[btnKey],:button[btnKey]").each(function (i, item) {
		var val = $(item).attr("btnKey");
		if (!isCon(userPageKeys, val)) {
			$(item).attr("disabled", "disabled").removeAttr("onclick").addClass("btn_disable");
		}
	});
};

var isCon = function (arr, val) {
	for (var i = 0; i < arr.length; i++) {
		if (val.indexOf(arr[i]) != -1)
			return true;
	}
	return false;
};

/*-------------------------------------------重载alert，调用父窗口内Defalut.js的alert方法-------------------------------------------*/
window.alert = function (msg) {
	parent.window.alert(msg);
};
