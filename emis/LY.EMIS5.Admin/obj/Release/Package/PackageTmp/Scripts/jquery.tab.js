$(function () {
	/******************³äÖµÒ³Ãæ***********************/
    $("#czw>li").each(function (index, domElem) {
        $(this).click(function () {
            var $this = $(this);
            var aa = $this.find("a");
            if ($this.hasClass("czbg")) {
                return;
            } else {
                $this.addClass("czbg").siblings('li').removeClass("czbg");
                aa.addClass("baise").parent().siblings('li').find("a").removeClass("baise");
                $("div.chongz").eq(index).siblings("div.chongz").hide().end().show();
				$("div.jstab").eq(index).siblings("div.jstab").hide().end().show();
            }
        });
    });
	/******************µ¼º½*****************************/
        $("#navi>li").hover(function () {		
            if ($(this).find("a").hasClass("nav_h")) {
				$(this).find("ul").show();
				$(this).find(".nav_h").css({"color":"#f18b00"});
            }
        }, function() {
			$(this).find("ul").hide();
			$(this).find(".nav_h").css({"color":"#fff"});
		});
	//ÓÒ±ßÏÔÊ¾ Òþ²Ø
	$(".nav").hover(function(){
		$("#navi").show();
		$(this).css({width:"62px"});	
	},function(){
		$(this).css({width:"10px"});
		$("#navi").hide();
		
	});
	//×ó±ß
	$(".massages_jt").click(function(){
		if($(".massages_sfx").css("display")=="block")
		{
			$(".massages_sfx").hide();
			$(this).find("img").attr("src","/content/images/massages_jt.jpg");
		}
		else
		{
			$(".massages_sfx").show();
			$(this).find("img").attr("src", "/content/images/massages_jt1.jpg");
		}	
	 });
});


