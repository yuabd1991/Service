/*----------------------------------------  总体   -------------------------------------------*/
//文档加载完成后
$(function () {
	//下面的高度 加上 头部的高度宽度。
	var extraHeight = 94;
	//在主页面完成后立马调整。
	resizeHeight();
	//窗口事件
	$(window).load(function () {
		//tabs resize
		$("#centerTab").tabs("resize");
	});
	$(window).resize(function () {
		resizeHeight();
		//tabs resize
		$("#centerTab").tabs("resize");
	});
	function resizeHeight() {
		var h = document.body.clientHeight;  //浏览器高度
		$("#divMiddle").height(h - extraHeight);
		$("#MiddleModule").height(h - extraHeight - 1); //右边的框框上去7px
		$(".left_body").height(h - extraHeight);
		//left_body_hide
		$(".left_body_hide").height(h - extraHeight);
		$(".divTrunLeft").css("margin-top", (h - extraHeight) / 2);
		$(".divTrunRight").css("margin-top", (h - extraHeight) / 2);
	}
});
/*----------------------------------------  左边   -------------------------------------------*/
/*点击头部，改变左边菜单*/
function UpdateLeftByTop(id) {
	$("#divMiddle").mask("加载中...");
	$.post("../Default/Left", { id: id }, function (data) {
		//根据回传的数据填充LEFT_BODY
		$('#LeftModule').html(data);
		$("#divMiddle").unmask();
		//调整加载出的DIV高度
		$(".left_body").height($("#divMiddle").height());
		//重新绑定function事件        
		$("div.left_head").click(function () {
			MenuEvent(this);
		});
	});
};
/*点击左边菜单，改变中间内容*/
function UpdateMiddleByLeft(id) {
	$.post("../Default/Middle", { id: id }, function (data) {
		$('#MiddleModule').html(data);
		LoadMiddle();
	});
};
$(function () {
	//头部按钮选中
	$('#TopMenu .menu_button').click(function () {
		$('#TopMenu>div').removeClass("TopBar-select");
		$(this).parent().addClass("TopBar-select");
		UpdateLeftByTop(this.id);
	});
	//左边按钮选中
	var $menu = $('#LeftModule');
	$menu.delegate(".left_Item", "click", function () {
		$menu.find(".left_Item").removeClass("menu_select");
		$(this).addClass("menu_select");
		UpdateMiddleByLeft(this.id);
	});
});
/* 左边隐藏 */
function TurnLeft() {
	$('.left_body').hide();
	$('.divTrunLeft').hide();
	$('.divTrunRight').show();
	$("#MiddleModule").css("margin-left", "6px");
	//tabs resize
	$("#centerTab").tabs("resize");
};
/* 左边显示 */
function TurnRight() {
	$('.left_body').show();
	$('.divTrunLeft').show();
	$('.divTrunRight').hide();
	$("#MiddleModule").css("margin-left", "163px");
	//tabs resize
	$("#centerTab").tabs("resize");
};
/* 菜单 */
$("div.left_head").click(function () {
	MenuEvent(this);
});
function MenuEvent(obj) {
	var ico = $(obj).find("#IcoUpAndDown");
	var cls = ico.attr("class");
	if (cls == "HideIcoUp") {
		ico.attr("class", "HideIcoDown");
		$(obj).next().hide();
	}
	else {
		ico.attr("class", "HideIcoUp");
		$(obj).next().show();
	}
};
/*----------------------------------------  middle 页面   -------------------------------------------*/
//获取子项
function GetPageItem(title) {
	var result = "";
	$.each(arrayPages, function (index, value) {
		if (value.split("|")[0] == title) {
			result = value;
		}
	});
	return result;
};
//获取页面默认打开的TAB
function SetPageOpened(title) {
	for (var i = 0; i < arrayPages.length; i++) {
		if (arrayPages[i].split("|")[0] == title) {
			arrayPages[i] = arrayPages[i].split("|")[0] + "|" + arrayPages[i].split("|")[1] + "|" + arrayPages[i].split("|")[2] + "|1";
		}
	}
};
//模板    初始化的     
//var template = "<iframe id=\"MainFrame\" name=\"MainFrame\"  scrolling=\"no\"  frameborder=\"0\" src=\"[src]\"   style=\"width:100%;height:100%;\"></iframe>";

var template = "<div class=\"contentDiv\">[content]</div>"

function LoadMiddle() {
	$('#centerTab').tabs({
		border: false,
		fit: true,
		onContextMenu: function (e, title) {
			e.preventDefault();
			$('#tab_menu > div').show();
			if (!GetPageItem(title) == "") {
				$("#closeItem").hide();
				$("#closeOthers").hide();
			}
			$('#tab_menu').menu(
                    {
                    	onClick: function (item) {
                    		if (item.id == "refresh") {
                    			var tab = $('#centerTab').tabs("getTab", title);
                    			var options = tab.panel("options");
                    			var html = $.ajax({
                    				url: options.href,
                    				async: false
                    			}).responseText;
                    			$('#centerTab').tabs('update', { tab: tab, options: { content: html} });

                    		} else if (item.id == "closeItem") {
                    			$('#centerTab').tabs("close", title)
                    		}
                    		else if (item.id == "closeOthers") {
                    			closeOtherTabs(title);
                    		} else if (item.id == "addBookmark") {
                    			AddMark(title);
                    		}
                    	}
                    });
			$('#tab_menu').menu("show", {
				left: e.clientX,
				top: e.clientY
			});
		}

	});
	//默认选中      
	var selectedTitle = "";
	//显示tab
	$.each(arrayPages, function (index, value) {
		var title = value.split("|")[0];
		var url = value.split("|")[1];
		var content = template;
		if (value.split("|")[2] == "1") {
			selectedTitle = value.split("|")[0];
		}
		OpenTabPage(title, url, false, false);
	});

	//alert(3);
	//默认选中tab
	$('#centerTab').tabs("select", selectedTitle);
};

/* 在tab中打开新页面 */
function OpenTabPage(title, url, selected, closable) {
	for (var i = 0; i < $('#centerTab').tabs("tabs").length; i++) {
		var tabTitle = $('#centerTab').tabs("tabs")[i].panel("options").title;
		if (title == tabTitle) {
			$('#centerTab').tabs("select", title);
			var parentDiv = $('#centerTab').tabs("tabs")[i].find("iframe").parent(); //.attr("src", url)
			parentDiv.empty();
			parentDiv.append(content);
			return;
		}
	}
	$('#centerTab').tabs('add', {
		title: title,
		//content: content,
		href: url,
		closable: closable,
		fit: true,
		selected: selected
	});
	//调整样式，去除滚动条
	// $("iframe[name=MainFrame]").parent().css("overflow", "hidden");
};
//关闭其它标签页，默认tab除外
function closeOtherTabs(title) {
	var del = new Array();
	$.each($('#centerTab').tabs("tabs"), function (i, tab) {
		var tabTitle = tab.panel("options").title;
		if (tabTitle != title && GetPageItem(tabTitle) == "") {
			del.push(tabTitle);
		}
	});
	$.each(del, function (i, title) {
		$('#centerTab').tabs("close", title)
	});
};
//tab新增书签
function AddMark(title) {
	var item = GetPageItem(title);
	var url = item.split("|")[1];
	$.post("/DefaultAjax/AddMark", { name: title, url: url },
    function (res) {
    	if (res > 0) {
    		//加载书签
    		LoadMark();
    		alert("新增书签成功！");
    	} else
    		alert("新增书签失败！");
    }, "text");
};

/*----------------------------------------  Top页面  -------------------------------------------*/
//显示书签列表
$(function () {
	var ishover = false;
	var mishover = false;
	$("#mark").hover(function () {
		ishover = true;
		$(".mark_menu").css("left", $(this).offset().left - $(".mark_menu").outerWidth() + $(this).outerWidth());
		$(".mark_menu").css("top", $(this).offset().top + $(this).outerHeight());
		$(".mark_menu").slideDown();
	}, function () {
		ishover = false;
		setTimeout(function () {
			if (ishover == false && mishover == false)
				$(".mark_menu").hide();
		}, 100);
	});
	$(".mark_menu").hover(function () {
		mishover = true;
	}, function () {
		mishover = false;
		setTimeout(function () {
			if (ishover == false && mishover == false)
				$(".mark_menu").hide();
		}, 100);
	});
	//加载书签
	LoadMark();
});
//删除书签
function DeleteMark(e, id) {
	$.post("/Default/DeleteMark", { id: id },
    function (res) {
    	if (res) {
    		$(e).parent(".mark_List_item").remove();
    		if ($(".mark_List > .mark_List_item").length == 0) { $(".mark_List").html("<span style='color:Black;margin-left:5px;'>您还没有新增书签!</span>"); }
    	}
    }, "text");
}
//加载书签
function LoadMark() {
	$.post("/Default/GetMarkList", {},
    function (res) {
    	$(".mark_List").empty();
    	$.each(res, function (i, item) {
    		$(".mark_List").append("<div class=\"mark_List_item\"><a href='javascript:void(0);' onclick=\"AddTabByMark('" + item.MarkName + "','" + item.MarkUrl + "')\" >" + item.MarkName + "</a><div class=\"ico\" title='删除书签' onclick=\"DeleteMark(this,'" + item.ID + "')\"></div></div>");
    	});
    	if (res.length == 0) { $(".mark_List").html("<span style='color:Black;margin-left:5px;'>您还没有新增书签!</span>"); }
    }, "json");
}
//在tab加载书签页面
function AddTabByMark(title, url) {
	var temp = template;
	var content = temp.replace("[src]", url);
	OpenTabPage(title, content, true, true);
}
/*----------------------------------------  弹出窗  -------------------------------------------*/

//弹出div窗口
function showWindow(ID, url, title, width, height, pars) {
	var html = $.ajax({
		data: pars,
		url: url,
		async: false
	}).responseText;

	$("body").append("<div id='" + ID + "'></div>");

	$('#' + ID).html(html);

	$('#' + ID).window({
		title: title,
		width: width,
		modal: true,
		shadow: false,
		closed: false,
		height: height,
		minimizable: false,
		maximizable: false,
		onClose: function () {
			$('#' + ID).parent('.panel.window').next(".window-mask").remove();
			$('#' + ID).parent('.panel.window').remove();
		}
	});
};


//关闭窗口
function closeWindow(ID) {
	$('#' + ID).window('close');
};
