$(document).ready(function () {
    $('.btn_li').click(function () {
        this.classList.toggle("li_action");
    });

    $('.btn_login').click(function () {
        var fmlogi = $('.fm_login');
        if (fmlogi.css("display") === "none") {
            fmlogi.css("display", "block");
        }
        else {
            fmlogi.css("display", "none");
        }
    });
    $('.item_option').slideUp();
    $('.can_hv ul').slideUp();
    $(".btnc").click(function () {
        $('.can_hv ul').slideUp();
        $('.item_option').slideUp();
        var panel = $(this).next();
        if (!panel.hasClass("panel")) {
            panel.addClass("panel");
            panel.slideDown();
        }
        else {
            panel.removeClass("panel");
            panel.slideUp();
        }
    });
    $('.btn_collapse').click(function () {
        var hear = $("header");
        hear.toggleClass("slide_col");
        var main = $(".main");
        main.toggleClass("main_actor");
        var hv = $(".can_hv ul");
        hv.toggleClass("ul_col");
        var canhv = $(".can_hv");
        canhv.toggleClass("canhv_col");
        $('.can_hv ul').removeClass("panel");
    });
})

//   main

$(document).ready(function () {
    var fm_data = $(".data_show");
    var fm_detail = $(".detail_fm");
    var fm_edit = $(".edit_fm");
    var page_data = $(".name_page");
    var page_detail = $(".name_page_detail");
    var page_edit = $(".name_page_edit");
    var main = $(".container");


    $(".btn_fm_detail").click(function () {
        fm_data.hide();
        fm_detail.show();
        fm_edit.hide();
        page_data.hide();
        page_detail.show();
        page_edit.hide();
    });
    $(".btn_fm_edit").click(function () {
        fm_data.hide();
        fm_detail.hide();
        fm_edit.show();
        page_data.hide();
        page_detail.hide();
        page_edit.show();
    });
    $(".btn_edit_close").click(function () {
        fm_data.show();
        fm_detail.hide();
        fm_edit.hide();
        page_data.show();
        page_detail.hide();
        page_edit.hide();
    })

    $(".btn_fm_danhsachvb").click(function () {
        main.show();
        fm_data.show();
        fm_detail.hide();
        fm_edit.hide();
        page_data.show();
        page_detail.hide();
        page_edit.hide();
    })

});
