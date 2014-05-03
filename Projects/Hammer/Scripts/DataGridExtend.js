/// <reference path="../../Resource/Scripts/Jquery.U1Fn.js" />

var DataGridExtend = {};



//初始化列选项
DataGridExtend.InitCols = function (gridId, cookieId) {

    var hideCols = DataGridExtend.getcookie(cookieId).split(',');
    DataGridExtend.setcookie(cookieId, "");
    for (var i = 0; i < hideCols.length; i++) {
        var cols = $('#' + gridId).datagrid('options').columns[0];
        $.each(cols, function (key, val) {
            if (val["field"] == hideCols[i]) {
                $('#' + gridId).datagrid('showColumn', hideCols[i]);
            }
        });
    }
};
DataGridExtend.setcookie = function (id, values) {
    var date = new Date();
    date.setFullYear(2055, 10, 10);
    setCookie(id, values, date, "/", null, false);
};
DataGridExtend.getcookie = function (id) {
    var result = Cookie(id);
    if (result == null) {
        return "";
    }
    return result;
};
DataGridExtend.loadSuccessInit = function (gridId, cookieId) {
    var hideCols = DataGridExtend.getcookie(cookieId).split(',');
    for (var i = 0; i < hideCols.length; i++) {
        var cols = $('#' + gridId).datagrid('options').columns[0];
        $.each(cols, function (key, val) {
            //回调函数有两个参数,第一个是元素索引,第二个为当前值
            if (val["field"] == hideCols[i]) {
                $('#' + gridId).datagrid('hideColumn', hideCols[i]);
            }
        });
    }

    // 列表的回到顶部功能。
    U1.initializePositionControl();
};
DataGridExtend.headerContextMenu = function (e, field, gridId, cookieId) {
    //阻止浏览器的菜单
    e.preventDefault();
    if (!$('#tmenu').length) {
        DataGridExtend.createColumnMenu(gridId, cookieId);
    }
    $('#tmenu').menu('show', {
        left: e.pageX,
        top: e.pageY
    });
};

//对官方的选择列的改进
DataGridExtend.createColumnMenu = function (gridId, cookieId) {
    var tmenu = $('<div id="tmenu" style="width:110px;"></div>').appendTo('body');
    var cols = $('#' + gridId).datagrid('options').columns[0];
    for (var i = 0; i < cols.length; i++) {
        //读取cookie
        var hideCols = DataGridExtend.getcookie(cookieId).split(',');
        var exist = $.inArray(cols[i]["field"], hideCols);
        var divItem = "";
        if (exist >= 0) {
            divItem = '<div id="tmenu_' + cols[i]["field"] + '" iconCls="icon-unchecked" >' + cols[i]["title"] + '</div>'
        } else {
            divItem = '<div id="tmenu_' + cols[i]["field"] + '" iconCls="icon-checked" >' + cols[i]["title"] + '</div>'
        }
        tmenu.append(divItem);
    }
    tmenu.menu({
        onClick: function (item) {
            if (item.iconCls == 'icon-checked') {
                //新增cookie中的元素
                var hideCols = DataGridExtend.getcookie(cookieId);
                var result = hideCols + item.id.replace("tmenu_", "") + ",";
                DataGridExtend.setcookie(cookieId, result);
                $('#' + gridId).datagrid('hideColumn', item.id.replace("tmenu_", ""));
                tmenu.menu('setIcon', {
                    target: item.target,
                    iconCls: 'icon-unchecked'
                });
            } else {
                //删除cookie中的元素
                var hideCols = DataGridExtend.getcookie(cookieId);
                var result = hideCols.replace(item.id.replace("tmenu_", "") + ",", "");
                DataGridExtend.setcookie(cookieId, result);
                $('#' + gridId).datagrid('showColumn', item.id.replace("tmenu_", ""));
                tmenu.menu('setIcon', {
                    target: item.target,
                    iconCls: 'icon-checked'
                });
            }
        }
    });
};

DataGridExtend.toDecimal = function (x) {
    var f_x = parseFloat(x);
    if (isNaN(f_x)) {
        return "0";
    }
    var f_x = Math.round(x * 100) / 100;
    return f_x;
};
DataGridExtend.toDate = function (input) {
    if (input == null) {
        return "";
    }
    var dateTime = eval("new " + input.replace("/", "").replace("/", ""));
    return dateTime.toLocaleDateString();
};
DataGridExtend.toDatetime = function (input) {
    if (input == null) {
        return "";
    }
    var dateTime = eval("new " + input.replace("/", "").replace("/", ""));
    return dateTime.toLocaleDateString() + " " + dateTime.toLocaleTimeString();
};
DataGridExtend.toNull = function (input, format) {
    if (format == null || format == "undefined" || format == "null") {
        format = "";
    }
    if (input == null || input == "undefined" || input == "null") {
        return format;
    }
    return input;
};
//搜索统一方法
DataGridExtend.searchGrid = function (gridId, queryParams, url) {
    if (queryParams != null) {
        $('#' + gridId).datagrid('options').queryParams = queryParams;
    }
    if (url != null) {
        $('#' + gridId).datagrid('options').url = getParUrl(url);
    }
    //全局变量，判断是否刷新到第一页
    if (grid_search_rload)
        $("#" + gridId).datagrid('load');
    else
        $("#" + gridId).datagrid('reload');
    grid_search_rload = false;
};
DataGridExtend.decimalFormat = function (val, row, rowIndex) {
    return DataGridExtend.toDecimal(val);
};
DataGridExtend.formatTime = function (val, row, rowIndex) {
    return DataGridExtend.toDate(val);
};
DataGridExtend.datetimeFormat = function (val, row, rowIndex) {
    return DataGridExtend.toDatetime(val);
};

DataGridExtend.formatTime = function (val, row, rowIndex) {
    if (val == null) {
        return "";
    }
    var dateTime = eval("new " + val.replace("/", "").replace("/", ""));
    return dateTime.format("yyyy-MM-dd hh:mm:ss");
};

DataGridExtend.formatTimeWith = function (val, formatter) {
    if (val == null) {
        return "";
    }
    var dateTime = eval("new " + val.replace("/", "").replace("/", ""));
    return dateTime.format(formatter);
};

// 对Date的扩展，将 Date 转化为指定格式的String 
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18
Date.prototype.format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1,                 //月份 
        "d+": this.getDate(),                    //日 
        "h+": this.getHours(),                   //小时 
        "m+": this.getMinutes(),                 //分 
        "s+": this.getSeconds(),                 //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds()             //毫秒 
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
return fmt;
};

DataGridExtend.formatMoneyStyle = function (val, row, rowIndex) {
    var html = "0.00";
    if (val != null && val != undefined && val != "null") {
        html = $.U1Fn.moneyFormat(val.toString());
    }
    return html;
};

//自动完成 IsDirectSale
function init_autocomplete(id, valueid, url, Params, successHandle) {
    var P = Params || {};
    var em = $("#" + id);
    var acOptions =
        {
            width: 157,
            minChars: 1,
            scroll: true,
            scrollHeight: 220,
            dataType: 'json',
            extraParams: P,
            matchCase: true,
            parse: function (data) {
                var parsed = [];
                for (var i = 0; i < data.length; i++) {
                    parsed[parsed.length] = {
                        data: data[i],
                        //返回的json数据：{["Name":"a","ID":"a"}]}
                        value: data[i].ID,
                        result: data[i].Name
                    };
                }
                return parsed;
            },
            formatItem: function (item) {
                return item.Name;
            }
        };

    $(em).autocomplete(url, acOptions).result(function (event, data, formatted) {
        if (data)
            $("#" + valueid).val(data.ID);
        else
            $("#" + valueid).val("");
        if (successHandle) {
            if (data)
                successHandle(data.ID);
            else
                successHandle("");
        }
    });
    $(em).change(function () { $("#" + id).search(); });

    $(em).bind("input.autocomplete", function () {
        $(this).trigger('keydown.autocomplete');
    });
}
//检验值
function getBussinessValue(id) {
    $("#" + id).search();
}

//-------------------------------------tip 提示---------------------------------------------------

// class='grid_Tip' rel='url' 
DataGridExtend.ShowTip_Url = function () {
    $('.grid_Tip').cluetip({
        cluetipClass: 'rounded',
        arrows: true,
        dropShadow: false,
        hoverIntent: false,
        //sticky: true,
        mouseOutClose: true,
        showTitle: false
        //closePosition: 'title',
        //closeText: '<img src="/Resource/Themes/icons/cross.png" alt="close" />'
    });
}
// class='grid_Tip'  rel='#id' 
DataGridExtend.ShowTip_href = function () {
    //fix bug 6199 to avoid error when the function "cluetip" is not avaiable(not add reference to jquery.cluetip.all.js to page).
    try {
        $('.grid_Tip').cluetip({
            cluetipClass: 'rounded',
            cursor: 'pointer',
            arrows: true,
            dropShadow: false,
            hoverIntent: false,
            local: true,
            sticky: false, //出现关闭按钮
            mouseOutClose: true,
            showTitle: false,
            width: 580
            //closePosition: 'title',
            //closeText: '<img src="/Resource/Themes/icons/cross.png" alt="close" />'
        });
    } catch (e) {

    }

}