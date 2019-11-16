// JavaScript Document
 $("document").ready(function($){
    var nav = $('#banner');

    $(window).scroll(function () {
        if ($(this).scrollTop() > 200) {
			$("#banner").css("display", "block");
            nav.addClass("f-nav");
        } else {
			$("#banner").css("display", "block");
            nav.removeClass("f-nav");
        }
    });
});