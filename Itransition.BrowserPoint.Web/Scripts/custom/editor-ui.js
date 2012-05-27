/// <reference path="/Scripts/jquery.js"/>
/// <reference path="/Scripts/jqueryui.js"/>

$(function () {
    var slideAspectRatio = 4 / 3;
    var presentation = {
        id:12345,
        title:'My presentation',
        authorId:543,
        tags:['slides', 'kawaii', 'work staff'],
        theme:'default',
        sizeX:800,
        sizeY:600,
        slides:[
            {
                textObjects:[
                    {
                        text:'Loren ipsum dolor sit amet',
                        type:'slide-text-label',
                        block:{
                            top:10,
                            left:10,
                            width:300,
                            height:200,
                            rotation:45,
                            scaleX:2,
                            scaleY:2
                        }
                    },
                    {
                        text:'test <strong>text test</strong> text',
                        type:'slide-text-title'
                    }
                ]
            }
        ]
    }

    // Window and elements resize handling
    var resizeSlidePreviews = function () {
        $(".slide-preview").each(function () {
            $(this).height($(this).width() / slideAspectRatio);
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
    });

    $('[data-element-draggable]').draggable({
        containment: "#slides-editor-workspace"
    });
});

