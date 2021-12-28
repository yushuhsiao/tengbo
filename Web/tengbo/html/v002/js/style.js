// JavaScript Document
$(document).ready(function(){
	$(".game_menu li").click(function(){
		var num = $(".game_menu li").index(this);
		$(this).addClass("mtab").siblings().removeClass("mtab");
		$(".content-area").eq(num).fadeIn().siblings().hide();
		})
	$(".float_game li").click(function(){
		$(this).addClass("current").siblings().removeClass("current");
		})
	
	$(".month li").click(function(){
		var num = $(".month li").index(this);
		$(this).addClass("cur").siblings().removeClass("cur");
		$(".month_tab").eq(num).fadeIn().siblings().hide();
		})
	
	$(".user_box dd a").click(function(){
		$(".user_box dd a").removeClass();
		$(this).addClass("cur");
		
		})
	
    });
	
	
function myhidden(divid)
{$("[name='divn']").hide();
	$(divid).show();
	
	
	
}
$(function(){
             $(".type").hover(function(){
                 $(this).find(".bg").animate({top:"44px"},200);
             },function(){
                 $(this).find(".bg").animate({top:"280px"},200);
             })
			 
			 $(".type").hover(function(){
                 $(this).find(".bg2").animate({top:"0px"},200);
             },function(){
                 $(this).find(".bg2").animate({top:"83px"},200);
             })
        })
