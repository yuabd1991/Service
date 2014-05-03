$(document).ready(function () {
	//主菜单
	$("ul#menu>li:has(dt)").hover(
	  function () {
	  	$(this).children('a').addClass('red').end().find('dd').fadeIn(400);
	  },
	  function () {
	  	$(this).children('a').removeClass('red').end().find('dd').fadeOut(400);
	  }
	);

	//首页幻灯片 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
//	var sWidth = $("#focus").width(); //获取焦点图的宽度（显示面积）
//	var len = $("#focus ul li").length; //获取焦点图个数
//	var index = 0;
//	var picTimer;

	//以下代码添加数字按钮和按钮后的半透明条，还有上一页、下一页两个按钮
//	var btn = "<div class='btnBg'></div><div class='btn'>";
//	for (var i = 0; i < len; i++) { btn += "<span></span>"; }
//	btn += "</div>";
//	$("#focus").append(btn);
//	$("#focus .btnBg").css("opacity", 0.5);

	//为小按钮添加鼠标滑入事件，以显示相应的内容
//	$("#focus .btn span").css("opacity", 0.4).mouseenter(function () {
//		index = $("#focus .btn span").index(this);
//		showPics(index);
//	}).eq(0).trigger("mouseenter");
	//本例为左右滚动，即所有li元素都是在同一排向左浮动，所以这里需要计算出外围ul元素的宽度
//	$("#focus ul").css("width", sWidth * (len));
//	//鼠标滑上焦点图时停止自动播放，滑出时开始自动播放
//	$("#focus").hover(function () { clearInterval(picTimer); }, function () {
//		picTimer = setInterval(function () { showPics(index); index++; if (index == len) { index = 0; } }, 4000); //此4000代表自动播放的间隔，单位：毫秒
//	}).trigger("mouseleave");

	//显示图片函数，根据接收的index值显示相应的内容
//	function showPics(index) { //普通切换
//		var nowLeft = -index * sWidth; //根据index值计算ul元素的left值
//		$("#focus ul").stop(true, false).animate({ "left": nowLeft }, 300); //通过animate()调整ul元素滚动到计算出的position
//		//$("#focus .btn span").removeClass("on").eq(index).addClass("on"); //为当前的按钮切换到选中的效果
//		$("#focus .btn span").stop(true, false).animate({ "opacity": "0.4" }, 300).eq(index).stop(true, false).animate({ "opacity": "1" }, 300); //为当前的按钮切换到选中的效果
//	}
	//首页幻灯片 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------


	//热门、就业、提升 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	var msn = msn || {};
	msn.hp = msn.hp || {};
	msn.hp.tab = {
		t: null,
		delayTime: 150,
		fx: true,
		tab: function (b) {
			$(b).siblings().removeClass("on");
			$(b).addClass("on");
			var c = $(b).parents(".tab").find("div.t");
			var a = c.eq($(b).index());
			c.addClass("none");
			a.removeClass("none");
			if (this.fx) {
				if ($(b).parent().hasClass("nofx")) {
					return
				}
				$(b).parent().siblings(".animate").width($(b).outerWidth() - 2).animate({
					left: $(b).position().left
				},
            "slow")
			}
		},
		delayTab: function (b, a) {
			clearTimeout(b.t);
			this.t = setTimeout(function () {
				b.tab(a)
			},
        this.delayTime)
		},
		init: function () {
			var a = this;
			a.animate();
			if (window.Touch) {
				$(".tab .main_title>ul>li[class!='on']>a").click(function () {
					return false
				})
			}
			$(".tab .main_title>ul>li,.tab>ul.hotread_menu>li").hover(function () {
				a.delayTab(a, this)
			},
        function () {
        	clearTimeout(a.t)
        })
		},
		animate: function () {
			if (!this.fx) {
				return
			}
			$(".tab .main_title>ul").each(function () {
				if (!$(this).hasClass("nofx")) {
					$(this).addClass("fx")
				}
			});
			$(".tab .main_title").each(function (a, b) {
				if ($(this).find("ul").hasClass("nofx")) {
					return
				}
				$(b).append("<div class='animate' ></div>");
				$(b).find(".animate").width($(b).find("ul>li.on").outerWidth() - 2).css("left", $(b).find("ul>li.on").position().left)
			})
		}
	};

	var a = msn.hp;
	a.tab.init();
	//热门、就业、提升 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------

	//首页优惠活动 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	$(".fd_list dt a").mousemove(function () {
		$(".fd_list dd").css("display", "none");
		$(".fd_list dt").removeClass("open");
		$(this).parent("dt").next("dd").css("display", "block");
		$(this).parent("dt").addClass("open");
		return false;
	})
	//首页优惠活动 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------

	//专业技巧 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	$("#zyjq").organicTabs({ "speed": 200 });
	//专业技巧 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------


	//摄影专业 化妆专业 数码专业 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	$("#warpsub").organicTabs({ "speed": 100 });
	//摄影专业 化妆专业 数码专业 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------

	//热点排行 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	$("#rdph").organicTabs({ "speed": 100 });
	//热点排行 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------


	//全部课程和购课流程 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	$("#Allkc").organicTabs({ "speed": 200 });
	//全部课程和购课流程 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------

	//联系我们内页 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	$("#acontact").organicTabs({ "speed": 200 });
	//联系我们内页 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------

	//名师讲堂 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	$("#msjt").organicTabs({ "speed": 200 });
	//名师讲堂 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------

	//招生信息 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	$("#zsjz").organicTabs({ "speed": 200 });
	//招生信息 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------

	//课程阶段 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	$("#jd").organicTabs({ "speed": 200 });
	//课程阶段 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------

	//课程阶段 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	$("#SchoolOverview").organicTabs({ "speed": 200 });
	//课程阶段 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------

	//课程阶段 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	$("#SchoolE").organicTabs({ "speed": 200 });
	//课程阶段 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------

	//当前位置下方图片轮换 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	//$('#change_1 .a_bigImg').soChange();
	//当前位置下方图片轮换 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------


	//返回首页 {{{{{{{{{{{{{{{{{{{{{{---------------------------------------
	$(window).scroll(function () { var top = $(window).scrollTop(); if (top > 500) { $("#scrolltop").fadeIn(); } else { $("#scrolltop").fadeOut(); } }); //点击返回头部效果
	$("#scrolltop").click(function () { $("html,body").animate({ scrollTop: 0 }); });
	//返回首页 }}}}}}}}}}}}}}}}}}}}}}}}}}-----------------------------------

	//摄影专业 - 化妆名师
	//$(".hzms li .img img,.hzzp li .img img").mouseover(function(){
	//    $(this).css({opacity: 0.5});
	//    $(this).animate({'opacity': 1}, "slow");//接触时显示颜色的速度如：＂normal＂＂fast＂＂slow＂毫秒（比如 1500）
	//  });
	//  $(".hzms li .img img,.hzzp li .img img").mouseout(function(){
	//    $(this).css({opacity: 1});
	//    $(this).animate({'opacity': 0.5},"slow");//接触时显示颜色的速度如：＂normal＂＂fast＂＂slow＂毫秒（比如 1500）
	//  });

});