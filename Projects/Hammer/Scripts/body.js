/*----------------------------------------  总体   -------------------------------------------*/

var F = {};
//调整高度
F.resizeHeight = function (extraHeight) {
	var h = document.body.clientHeight;  //浏览器高度

	if (window.navigator.userAgent.indexOf("MSIE") >= 0) {
		if (navigator.appVersion.split(";")[1].replace(/[ ]/g, "") == "MSIE6.0") {
			h = document.documentElement.clientHeight;
		}
	}
	$("#divMiddle").height(h - extraHeight);
	$("#MiddleModule").height(h - extraHeight - 1); //右边的框框上去7px
	$(".left_body").height(h - extraHeight - 8);
	//$("#LeftModule").height($("#LeftModule").height() - 2);
	//left_body_hide
	$(".left_body_hide").height(h - extraHeight);
	$(".divTrunLeft").css("margin-top", (h - extraHeight) / 2);
	$(".divTrunRight").css("margin-top", (h - extraHeight) / 2);
}

//窗体加载时，做的事情
F.loadEvent = function () {
	//下面的高度 加上 头部的高度宽度。
	var extraHeight = 125;
	//在主页面完成后立马调整。
	F.resizeHeight(extraHeight);
	//窗口事件
	$(window).resize(function () {
		F.resizeHeight(extraHeight);
		//tabs resize
		$("#centerTab").tabs("resize"); //这句在ie6 无效,只变大不变小
	});

	MF.InitTab();
}
//文档加载完成后
$(function () {
	F.loadEvent();
	//overrideAlert.init();
});


/*----------------------------------------  左边   -------------------------------------------*/

var LF = {};
/* 加载菜单组*/
LF.RanderMuneChildren = function (pid) {
	//标记
	var template = "<div class=\"left_head\" onclick=\"LF.MenuEvent(this)\"><div class=\"first\"></div><div class=\"mid\" ><div class=\"ModuleIco\"></div><span id=\"leftTitle\" class=\"leftitle\">@item.MenuName</span><div id=\"IcoUpAndDown\" class=\"HideIcoDown\"></div></div><div class=\"last\"></div></div><div id=\"left_Menu_@item.ID\"  class=\"left_Menu\"></div>";
	var html = "";

	$(".left_body").html("");
	var selectid = null;
	$.each(arrayAllMenus, function (index, value) {
		var arrTemp = value.split("|");
		if (arrTemp[3] == pid) {
			var html = template.replace("@item.MenuName", arrTemp[1]);
			html = html.replace("@item.ID", arrTemp[0]);
			$(".left_body").append(html);
			var res = LF.RanderMuneSubChildren(arrTemp[0]);
			if (res)
				selectid = res;
//			if (arrTemp[1] == "首页管理") {
//				//$('#centerTab div').eq(0).hide();
//				$('#centerTab ul li').hide();
//				//                $('#centerTab .tabs-header').css("background", "none repeat scroll 0 0 #FFFFFF");
//				//                $('#centerTab .tabs').css("border-bottom", "0px solid #BAC0C7");
//				LF.TurnLeft();
//				$('.divTrunRight').hide();
//			} else {
			$('#centerTab div').eq(0).show();
				//                $('#centerTab ul li').show();
				//                $('#centerTab .tabs-header').css("background", "none repeat scroll 0 0 #F6F6F6");
				//                $('#centerTab .tabs').css("border-bottom", "1px solid #BAC0C7");
//			}
		}
	});
	if (selectid)
		LF.MenuEvent($("#left_Menu_" + selectid).prev());
}

/* 加载子菜单*/
LF.RanderMuneSubChildren = function (pid) {
	var template = "<div class=\"left_Item\" id=\"left_Item_@subItem.ID\"><span>@subItem.MenuName</span></div>"
	var scriptTemplate = "<script>$(function () {  $('#left_Item_@subItem.ID').addClass(\"menu_select\");LF.UpdateMiddleByLeft('@subItem.ID');});</script>"
	var res = null;
	$.each(arrayAllMenus, function (index, value) {
		var arrTemp = value.split("|");
		if (arrTemp[3] == pid) {
			var html = template.replace("@subItem.ID", arrTemp[0]);
			html = html.replace("@subItem.MenuName", arrTemp[1]);
			$("#left_Menu_" + pid).append(html);
			if (arrTemp[2] == "Y") {
				var script = scriptTemplate.replace("@subItem.ID", arrTemp[0]);
				script = script.replace("@subItem.ID", arrTemp[0]);
				$("#left_Menu_" + pid).append(script);
				//LF.MenuEvent($("#left_Menu_" + pid).prev());
				res = pid;
			}
		}
	});
	return res;
}


/*点击头部，改变左边菜单*/
LF.UpdateLeftByTop = function (id) {
	LF.RanderMuneChildren(id);
};
/*点击左边菜单，改变中间内容*/
LF.UpdateMiddleByLeft = function (id) {
	MF.InitPageArray(id);
	MF.LoadMiddle();
};
LF.loadEvent = function () {
	//头部按钮选中
	$('#TopMenu .menu_button').click(function () {
		$('#TopMenu>div').removeClass("TopBar-select");
		$(this).parent().addClass("TopBar-select");
		LF.TurnRight();
		LF.UpdateLeftByTop(this.id);
	});
	//左边按钮选中
	var $menu = $('#LeftModule');
	$menu.delegate(".left_Item", "click", function () {
		$menu.find(".left_Item").removeClass("menu_select");
		$(this).addClass("menu_select");
		var id = this.id.replace("left_Item_", "");
		LF.UpdateMiddleByLeft(id);
	});

}

/* 左边隐藏 */
LF.TurnLeft = function () {
	$('.left_body').hide();
	$('.divTrunLeft').hide();
	$('.divTrunRight').show();
	$("#MiddleModule").css("margin-left", "0px");
	//tabs resize
	$("#centerTab").tabs("resize");
};
/* 左边显示 */
LF.TurnRight = function () {
	$('.left_body').show();
	$('.divTrunLeft').show();
	$('.divTrunRight').hide();
	$("#MiddleModule").css("margin-left", "157px");
	//tabs resize
	$("#centerTab").tabs("resize");
};

LF.MenuEvent = function (obj) {
	var ico = $(obj).find("#IcoUpAndDown");
	var cls = ico.attr("class");
	var heads = $(".left_body .left_head");
	heads.next().hide();
	heads.find("#IcoUpAndDown").attr("class", "HideIcoDown");
	if (cls == "HideIcoUp") {
		ico.attr("class", "HideIcoDown");
		$(obj).next().hide();
	}
	else {
		ico.attr("class", "HideIcoUp");
		$(obj).next().show();
		//var menu = $(obj).next();
		//menu.height($('.left_body').innerHeight() - (heads.outerHeight() + 6) * heads.length - 14).show();
	}
};

$(function () {
	LF.loadEvent();
});



/*----------------------------------------  middle 页面   -------------------------------------------*/


var MF = {};
//初始化页面数组
MF.InitPageArray = function (mid) {
    arrayPages = []; //清空数组
    arrayPages.length = 0;
    $.each(arrayAllPages, function (index, value) {
        var itemTemp = value.split("|");
        if (itemTemp[3] == mid) {
            var select = 0;
            if (itemTemp[2] == "Y") {
                select = 1;
            }
            arrayPages.push(itemTemp[1] + "|" + itemTemp[4] + "|" + select + "|0");
        }
    });
}

//获取子项
MF.GetPageItem = function (title) {
    var result = "";
    $.each(arrayPages, function (index, value) {
        if (value.split("|")[0] == title) {
            result = value;
        }
    });
    return result;
};
//获取页面默认打开的TAB
MF.SetPageOpened = function (title) {
    for (var i = 0; i < arrayPages.length; i++) {
        if (arrayPages[i].split("|")[0] == title) {
            arrayPages[i] = arrayPages[i].split("|")[0] + "|" + arrayPages[i].split("|")[1] + "|" + arrayPages[i].split("|")[2] + "|1";
        }
    }
};
//模板    初始化的
MF.template = function () {
    return "<iframe id=\"MainFrame\" name=\"MainFrame\"  scrolling=\"auto\"  frameborder=\"0\" src=\"[src]\"   style=\"width:100%;height:100%; padding:0px; margin:0px;\"></iframe>";
}
var Load_PageID;
MF.InitTab = function () {

    $('#centerTab').tabs({
        border: false,
        fit: true,
        onSelect: function (title) {
            var item = MF.GetPageItem(title);
            //空判断
            if (item == "") {
                Load_PageID = title; //打开的页面
            }
        },
        onContextMenu: function (e, title) {
            e.preventDefault();
            $('#tab_menu > div').show();
            if (!MF.GetPageItem(title) == "") {
                $("#closeItem").hide();
                $("#closeOthers").hide();
            }
            var tab = $('#centerTab').tabs("getTab", title);
            if (tab.panel("options").tab.find(".icon-lock-tab").length > 0) {
                $('#tab_menu > #lockTab').hide();
            } else {
                $('#tab_menu > #unlockTab').hide();
            }
            $('#tab_menu').menu(
                    {
                        onClick: function (item) {
                            if (item.id == "refresh") {
                                var m_Iframe = $('#centerTab').tabs("getTab", title).find("iframe");
                                var url = m_Iframe.attr("src");
                                m_Iframe.attr("src", url);
                            } else if (item.id == "closeItem") {
                                MF.closeTab(title);
                            }
                            else if (item.id == "closeOthers") {
                                MF.closeOtherTabs(title);
                            } 
                            else if (item.id == "lockTab") {
                                MF.lockTab(title);
                            }
                            else if (item.id == "unlockTab") {
                                MF.unlockTab(title);
                            }
                        }
                    });
            $('#tab_menu').menu("show", {
                left: e.clientX,
                top: e.clientY
            });
        },
        onBeforeClose: function (title) {
            var tab = $('#centerTab').tabs("getTab", title);
            if (tab != null) {
                tab.find("iframe").attr("src", "");
                tab = null;
            }
        }

    });
}

MF.lockTab = function (title) {
    var tab = $('#centerTab').tabs("getTab", title);
    tab.panel("options").tab.find(".tabs-icon").addClass("icon-lock-tab");
    tab.panel("options").tab.find(".tabs-title").addClass("tabs-with-icon");
}
MF.unlockTab = function (title) {
    var tab = $('#centerTab').tabs("getTab", title);
    tab.panel("options").tab.find(".tabs-icon").removeClass("icon-lock-tab");
    tab.panel("options").tab.find(".tabs-title").removeClass("tabs-with-icon");
}

MF.LoadMiddle = function () {
    //Page排序
    MF.isSortTabs = true;
    //清空所有tab
    MF.closeAllTabs();
    //默认选中      
    var selectedTitle = "";
    //显示tab
    $.each(arrayPages, function (index, value) {
        var title = value.split("|")[0];
        var content = MF.template();
        var isSelect = false;
        if (value.split("|")[2] == "1") {
            selectedTitle = value.split("|")[0];
            var url = value.split("|")[1];
            content = content.replace("[src]", url); //默认打开的页面
            MF.SetPageOpened(title);
            isSelect = true;
        }
        else {
            content = content.replace("[src]", "");
        }
        content = content.replace("[src]", "");
        MF.OpenTabPage(title, content, isSelect, false);

    });

    //绑定click事件
    $('.tabs li').click(function () {
        var title = $(this).find(".tabs-title").html();
        var item = MF.GetPageItem(title);
        //空判断
        if (item == "") {
            return;
        }
        var url = item.split("|")[1];
        var isopen = item.split("|")[3];
        if (isopen == 0) {
            $('#centerTab').tabs("getTab", title).find("iframe").attr("src", url);
            MF.SetPageOpened(title);
        } else if (isopen == 1 && Load_PageID != item.split("|")[0]) {
            var iframe = $('#centerTab').tabs("getTab", title).find("iframe").get(0);
            if (iframe) {
                if (iframe.contentWindow.Search) {
                    iframe.contentWindow.Search();
                } else if (iframe.contentWindow.Reflesh) {
                    iframe.contentWindow.Reflesh();
                }
            }
        }
        Load_PageID = item.split("|")[0];
    });
    //默认选中tab
    $('#centerTab').tabs("select", selectedTitle);
};

//选中tab
MF.select = function (title) {
    $('#centerTab').tabs("select", title);
}

/* 在tab中打开新页面 */
MF.OpenTabPage = function (title, content, selected, closable) {
    for (var i = 0; i < $('#centerTab').tabs("tabs").length; i++) {
        var tabTitle = $('#centerTab').tabs("tabs")[i].panel("options").title;
        if (title == tabTitle) {
            $('#centerTab').tabs("select", title);
            var parentDiv = $('#centerTab').tabs("tabs")[i].find("iframe").parent();
            parentDiv.empty();
            parentDiv.append(content);
            return;
        }
    }
    $('#centerTab').tabs('add', {
        title: title,
        content: content,
        closable: closable,
        fit: true,
        selected: selected
        // remove refresh icon.
        //        tools: [{
        //            iconCls: 'icon-mini-refresh',
        //            handler: function () {
        //                var m_Iframe = $('#centerTab').tabs("getTab", title).find("iframe");
        //                var url = m_Iframe.attr("src");
        //                m_Iframe.attr("src", url);
        //            }
        //        }]
    });

};


/* 在tab中打开新页面,并延迟加载，当点击页签的时候，才加载 */
MF.AddTab = function (title, url, closable) {
    var content = MF.template();
    content = content.replace("[src]", "");
    $('#centerTab').tabs('add', {
        title: title,
        content: content,
        closable: typeof (closable) == "undefine" ? true : closable,
        fit: true,
        selected: false,
        cacheurl: url,
        OnTabClick: function (val) {
            var iframe = $('#centerTab').tabs("getTab", title).find("iframe");
            if (iframe && iframe.attr("src") == "") {
                $('#centerTab').tabs("getTab", title).find("iframe").attr("src", url);
            }
        }
        // remove refresh icon.
        //        ,
        //        tools: [{
        //            iconCls: 'icon-mini-refresh',
        //            handler: function () {
        //                var m_Iframe = $('#centerTab').tabs("getTab", title).find("iframe");
        //                var url = m_Iframe.attr("src");
        //                m_Iframe.attr("src", url);
        //            }
        //        }]
    });

};

//关闭当前tab
MF.closeSelectTab = function () {
    MF.closeTab($('#centerTab').tabs("getSelected").panel('options').title);
}
//修改当前TAB
MF.updateTab = function (url, title) {
    var tab = $('#centerTab').tabs('getSelected');
    //    $('#centerTab').tabs('update', {
    //        tab: tab,
    //        options: {
    //            title: title,
    //            href: url
    //        }
    //    });
    $('#centerTab .tabs-selected').find(".tabs-title").html(title);
    tab.find("iframe").attr("src", url);
};


//关闭其它标签页，默认tab除外
MF.closeOtherTabs = function (title) {
    var del = new Array();
    $.each($('#centerTab').tabs("tabs"), function (i, tab) {
        var tabTitle = tab.panel("options").title;
        if (tabTitle != title && MF.GetPageItem(tabTitle) == "") {
            del.push(tabTitle);
        }
    });
    $.each(del, function (i, title) {
        MF.closeTab(title);
    });
}

//关闭全部tab
MF.closeAllTabs = function () {
    var del = new Array();
    $.each($('#centerTab').tabs("tabs"), function (i, tab) {
        if (tab.panel("options").tab.find(".icon-lock-tab").length == 0) {
            var tabTitle = tab.panel("options").title;
            del.push(tabTitle);
        }

    });
    $.each(del, function (i, title) {
        MF.closeTab(title);
    });
}
//根据标题关闭tab
MF.closeTab = function (title) {
    $('#centerTab').tabs("close", title);
    var tab = $('#centerTab').tabs("getTab", title);
    if (tab != null) {
        tab.find("iframe").attr("src", "");
        tab = null;
    }
}

//根据标题刷新tab
MF.ReloadTab = function (title) {
    MF.closeSelectTab();
    if (title) {
        var tab = $('#centerTab').tabs("getTab", title);
        if (tab) {
            var currentIframe = tab.find("iframe").get(0);
            if (currentIframe) {
                if (currentIframe.contentWindow.Reflesh) {//如果有Reflesh方法就刷新
                    currentIframe.contentWindow.Reflesh();
                    $('#centerTab').tabs("select", title);
                }
            }
        }
    }
}

//获得当前Tab标题
MF.GetSelectedTabTitle = function () {
    return $('#centerTab').tabs("getSelected").panel('options').title;
}


/*----------------------------------------  Top页面  -------------------------------------------*/
var TF = {};
var DeleteMarks = new Array();
var marksPageIndex = 0;
//初始化事件
$(function () {
//    TF.InitLoadMark();
    TF.InitActionLog();
});
//tab新增书签
//TF.AddMark = function (title, url) {
//    //var item = MF.GetPageItem(title);
//    //var url = item.split("|")[1];
//    //杰 2013年5月23日10:09:25 bug
//    $.post("/Default/AddMark?ua=" + active_UserName(), { name: title, url: url },
//    function (res) {
//        if (res > 0) {
//            //加载书签
//            TF.LoadMark();
//            alert("新增书签成功！");
//        } else if (res == -1) {
//            alert("该书签已存在！");
//        }
//        else
//            alert("新增书签失败！");
//    }, "text");
//};

//TF.InitLoadMark = function () {
//    var ishover = false;
//    var mishover = false;
//    $("#mark").hover(function() {
//        ishover = true;
//        $(".mark_menu").css("left", $(this).offset().left - $(".mark_menu").outerWidth() + $(this).outerWidth());
//        $(".mark_menu").css("top", $(this).offset().top + $(this).outerHeight());
//        $(".mark_menu").slideDown();
//    }, function() {
//        ishover = false;
//        setTimeout(function() {
//            if (ishover == false && mishover == false)
//                $(".mark_menu").hide();
//        }, 100);
//    });
//    $(".mark_menu").hover(function() {
//        mishover = true;
//    }, function() {
//        mishover = false;
//        setTimeout(function() {
//            if (ishover == false && mishover == false)
//                $(".mark_menu").hide();
//        }, 100);
//    });
//    //加载书签
//    TF.LoadMark();
//};
////删除书签
//TF.DeleteMark = function(e, id, title, url) {
//    $.post("/Default/DeleteMark?ua=" + active_UserName(), { id: id },
//        function(res) {
//            if (res) {
//                var str = title + "|" + url;
//                DeleteMarks.push(str);
//                $("#markCancel").show();
//                $(e).parent(".mark_List_item").remove();
//                if ($(".mark_List > .mark_List_item").length == 0) {
//                    $(".mark_List").html("<span style='color:Black;margin-left:5px;'>您还没有新增书签!</span>");
//                }
//                marksPageIndex = 0;
//                //书签分页
//                TF.MarkPage();
//            }
//        }, "text");
//};
////加载书签
//TF.LoadMark = function() {
//    $.post("/Default/GetMarkList?ua=" + active_UserName(), {},
//        function(res) {
//            $(".mark_List").empty();
//            $.each(res, function(i, item) {
//                $(".mark_List").append("<div class=\"mark_List_item\"><div class=\"ico\" title='删除书签' onclick=\"TF.DeleteMark(this,'" + item.ID + "','" + item.MarkName + "','" + item.MarkUrl + "')\"></div><a href='javascript:void(0);' onclick=\"TF.AddTabByMark('" + item.MarkName + "','" + item.MarkUrl + "')\" >" + item.MarkName + "</a></div>");
//            });
//            if (res.length == 0) {
//                $(".mark_List").html("<span style='color:Black;margin-left:5px;'>您还没有新增书签!</span>");
//            }
//            marksPageIndex = 0;
//            //书签分页
//            TF.MarkPage();
//        }, "json");
//};
//书签分页 由0开始
TF.MarkPage = function() {
    var pagesize = 10;
    var count = pagesize * marksPageIndex;
    if (marksPageIndex < 0) {
        marksPageIndex = 0;
    }
    var objs = $(".mark_List > .mark_List_item");
    var marksPageCount = parseInt(objs.length / pagesize);
    if (objs.length <= pagesize) {
        marksPageCount = 1;
    } else {
        marksPageCount = marksPageCount + 1;
    }
    if ((marksPageIndex + 1) > marksPageCount) {
        marksPageIndex = marksPageIndex - 1;
        return;
    }
    var j = 1;
    $.each(objs, function(i, item) {
        if (i < count) {
            $(item).hide();
        } else {
            if (j <= pagesize) {
                $(item).show();
            } else {
                $(item).hide();
            }
            j++;
        }
    });
};
//上一页
TF.UpPage = function() {
    marksPageIndex--;
    if (marksPageIndex < 0) {
        marksPageIndex = 0;
    }
    TF.MarkPage();
};
//下一页一页
TF.DownPage = function() {
    marksPageIndex++;
    TF.MarkPage();
};
//在tab加载书签页面
TF.AddTabByMark = function(title, url) {
    var temp = MF.template();
    var content = temp.replace("[src]", url);
    MF.OpenTabPage(title, content, true, true);
};
//返回上一步操作
//TF.CancelDelete = function() {
//    if (DeleteMarks != undefined && DeleteMarks.length > 0) {
//        var str = DeleteMarks[DeleteMarks.length - 1];
//        if (str.length <= 0) {
//            return;
//        }
//        var title = str.split('|')[0];
//        var url = str.split('|')[1];
//        DeleteMarks.pop();
//        $.post("/Default/AddMark?ua=" + active_UserName(), { name: title, url: url },
//            function(res) {
//                if (res > 0) {
//                    //加载书签
//                    TF.LoadMark();
//                } else {
//                    alert("返回上一步操作失败！");
//                }
//            }, "text");
//    }
//    if (DeleteMarks.length <= 0) {
//        $("#markCancel").hide();
//    }
//};
//显示关键操作记录
TF.InitActionLog = function() {
    var ishover = false;
    var mishover = false;
    $("#actionLog").hover(function() {
        ishover = true;
        $(".actionLog_menu").css("left", $(this).offset().left - $(".actionLog_menu").outerWidth() + $(this).outerWidth());
        $(".actionLog_menu").css("top", $(this).offset().top + $(this).outerHeight());
        $(".actionLog_menu").slideDown();
    }, function() {
        ishover = false;
        setTimeout(function() {
            if (ishover == false && mishover == false)
                $(".actionLog_menu").hide();
        }, 100);
    });
    $(".actionLog_menu").hover(function() {
        mishover = true;
    }, function() {
        mishover = false;
        setTimeout(function() {
            if (ishover == false && mishover == false)
                $(".actionLog_menu").hide();
        }, 100);
    });
};
//配置回收站点击事件
TF.LoadRecycl = function() {
    var temp = MF.template();
    var url = "../../Admin/RecycleBin/RecycleBinListView?ua=admin";
    var content = temp.replace("[src]", url);
    MF.OpenTabPage("回收站", content, true, true);
};
TF.LoadIndex = function (site) {
    var url = "/admin/home";
//    if (site == "Admin") {
//        url = "/Admin/Index/IndexView?ua=" + active_UserName();
//    } else if (site == "Channel") {
//        url = "/Channel/Index/IndexView?ua=" + active_UserName();
//    } else if (site == "Wms") {
//        url = "/Wms/Index/IndexView?ua=" + active_UserName();
//    }
    var temp = MF.template();
    var content = temp.replace("[src]", url);
    MF.OpenTabPage("", content, true, true);
    $('#centerTab ul li').hide();
    LF.TurnLeft();
    $('.divTrunRight').hide();
}
TF.LoadSetUp = function (site) {
    var url = "/admin/settings/userProfile";
//    if (site == "Admin") {
//        url = "/Admin/ModifyInformation/ModifyMaterialView?ua=" + active_UserName();
//    } else if (site == "Channel") {
//        url = "/Channel/ModifyInformation/ModifyMaterialView?ua=" + active_UserName();
//    } else if (site == "Wms") {
//        url = "/Wms/WareHouse/WmsManageUserView?ua=" + active_UserName();
//    }
    var temp = MF.template();
    var content = temp.replace("[src]", url);
    MF.OpenTabPage("修改资料", content, true, true);
}


/*----------------------------------------  弹出窗  -------------------------------------------*/
/*
var PW_father = {};
var PW_father_father = {};

//弹出div窗口
PW_father.showWindow = function (title, width, height) {
$('#index-window').window({
title: title,
width: width,
modal: true,
shadow: true,
closed: false,
height: height,
minimizable: false,
maximizable: false,
onClose: function () { $('#WindowFrame').attr("src", ""); }
});
};

//弹出div窗口
PW_father.showModalWindow = function (title, width, height, modal, url, iconCls) {
$('#index-window').attr("iconCls", iconCls);
$('#index-window').window({
title: title,
width: width,
modal: modal,
shadow: true,
closed: false,
height: height,

minimizable: false,
maximizable: false,
onClose: function () { $('#WindowFrame').attr("src", ""); }
});
if (url != null) {
$('#WindowFrame').attr("src", url);
}
};

//弹出div窗口，并把url加载到窗口
PW_father.showUrlWindow = function (url, title, width, height) {
this.showWindow(title, width, height);
$('#WindowFrame').attr("src", url);
};


//关闭窗口
PW_father.closeWindow = function () {
$('#index-window').window('close');
};

*/

//获取选中的页面doc
function getSelectPageDoc() {
    var currentIframe = $('#centerTab').tabs("getSelected").find("iframe").get(0);
    currentIframe.scrolling = "auto";
    var doc = currentIframe.contentWindow;
    return doc;
};

//获得选项卡中的Iframe 
function getSelectPageIframe() {
    var currentIframe = $('#centerTab').tabs("getSelected").find("iframe").get(0);
    return currentIframe;
};
//获取选中的页面doc
function getSelectPageDocByTitle(title) {
    var tab = $('#centerTab').tabs("getTab", title);
    if ($.isEmptyObject(tab)) {
        return null;
    }
    var currentIframe = $(tab).find("iframe").get(0);
    currentIframe.scrolling = "auto";
    var doc = currentIframe.contentWindow;
    return doc;
};

//获得选项卡中的Iframe 
function getSelectPageIframeByTitle(title) {
    var tab = $('#centerTab').tabs("getTab", title);
    if ($.isEmptyObject(tab)) {
        return null;
    }
    var currentIframe = $(tab).find("iframe").get(0);
    return currentIframe;
};

//通过索引获得tab的CD
function getTabDocByIndex(index) {
    var tabTitle = $('#centerTab').tabs("tabs")[index].panel("options").title;
    var tab = $('#centerTab').tabs("getTab", tabTitle);
    if ($.isEmptyObject(tab)) {
        return null;
    }
    var currentIframe = $(tab).find("iframe").get(0);
    return currentIframe.contentWindow;
}


//-----------------------------------------------------------------窗口---------------------------------------------------
/*
//弹出第二个窗口
PW_father_father.showWindow = function (title, width, height) {
$('#up-window').window({
title: title,
width: width,
modal: true,
shadow: true,
closed: false,
height: height,
minimizable: false,
maximizable: false,
onClose: function () { $('#UpWindowFrame').attr("src", ""); }
});
};

//弹出第二个窗口窗口，并把url加载到窗口
PW_father_father.showUrlWindow = function (url, title, width, height) {
this.showWindow(title, width, height);
$('#UpWindowFrame').attr("src", url);
};

//关闭第二个窗口
PW_father_father.closeWindow = function () {
$('#up-window').window('close');
};

//第一个弹出窗口doc
PW_father_father.getWindowDoc = function () {
return $('#WindowFrame').get(0).contentWindow;
}*/

/*-------------------------------------------重载alert，子Iframe内Window.js的alert也会调用该alert方法-------------------------------------------*/
//var overrideAlert = {
//    init: function() {
//        window.alert = function(msg) {
//            $("body").mask(msg);
//            $(".loadmask-msg div").css({ fontSize: "12px", background: "#FBFBFB", padding: "5px 25px" });
//            $(".actionLog_List").prepend("<div>" + msg + "</div>");
//            $(".actionLog_List div").css("color", "#000");
//            if ($(".actionLog_List").html()=="") $("#actionLog_None").show();
//            else $("#actionLog_None").hide();
//            $(".actionLog_List div:first-child").css("color", "red");
//            setTimeout('overrideAlert.hideAlert()', 1200);
//        };
//    },
//    hideAlert: function() {
//        $(".loadmask-msg").animate({
//            left: $("#actionLog").position().left,
//            top: $("#actionLog").position().top,
//            width: 1,
//            height: 1
//        }, 300);
//        setTimeout('$("body").unmask(); overrideAlert.promot();', 300);
//    },
//    promot: function() {
//        $("#actionLog span").removeClass("caozuojilu").addClass("caozuojilu2");
//        setTimeout('$("#actionLog span").removeClass("caozuojilu2").addClass("caozuojilu");', 800);
//    },
//};
