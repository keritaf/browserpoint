/// <reference path="/Scripts/jquery.js"/>
/// <reference path="/Scripts/jqueryui.js"/>

$(function () {

   // Window and elements resize handling
    var resizeSlidePreviews = function () {
        $(".slide-preview").each(function () {
            $(this).height($(this).width() / 1.33);
        });
    };
    $('#slides-editor-previews').resize(resizeSlidePreviews);
    resizeSlidePreviews();

    var resizeHandler = function () {
        var editorWindow = $("#slides-editor-window");
        editorWindow.height($(window).height() - editorWindow.position().top);
    };
    $(window).resize(resizeHandler);
    $(window).resize();

    // Slide selection
    $("#slides-editor-previews").on("click", ".slide-preview", function (e) {
        $("#slides-editor-previews .slide-preview").removeClass("selected");
        $(this).addClass("selected");
        // change slide
    });

    $('.draggable-and-resizible').draggable({
        containment: "parent" 
    }).resizable({
        containment: "parent"
    });

    
});


