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

    $('[data-element-draggable]').draggable({
        containment: "parent" //"#slides-editor-workspace"
    });
});


var presentation = new PresentationModel({
    id: 12345,
    title: 'My presentation',
    authorId: 543,
    tags: ['slides', 'kawaii', 'work staff']
});

var blockParams = {
    top: 10,
    left: 10,
    width: 300,
    height: 200,
    rotation: 45,
    scaleX: 2,
    scaleY: 2
};

var text1 = new TextObject({ text: 'Loren ipsum dolor sit amet', block: blockParams });
presentation.addSlide({ texts: [text1] });

ko.applyBindings(presentation);


