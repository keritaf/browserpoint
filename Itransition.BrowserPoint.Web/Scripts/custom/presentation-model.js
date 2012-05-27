/// <reference path="/Scripts/knockout.js"/>

function BlockParameters(options) {
    var self = this;
    self.top = ko.observable(options.top);
    self.left = ko.observable(options.left);
    self.width = ko.observable(options.width);
    self.height = ko.observable(options.height);
    self.rotation = ko.observable(options.rotation);
    self.scaleX = ko.observable(options.scaleX || 1);
    self.scaleY = ko.observable(options.scaleY || 1);
    self.zindex = ko.observable(options.zindex);
}

function TextObject(options) {
    var self = this;

    var text = options.text || "Some text...";
    var block = new BlockParameters(options.block); 
    
    self.text = ko.observable(text);
    self.block = ko.observable(block);
}

function ImageObject(options) {
    var self = this;
    self.url = ko.observable(options.url);
    self.blockParams = ko.observable(options.blockParameters);
}

function PresentationSlide(options) {
    var self = this;

    var texts = options.texts || [];
    var images = options.images || [];
    
    self.texts = ko.observableArray(texts);
    self.images = ko.observableArray(images);
}


function PresentationModel(options) {
    var self = this;

    var authorId = options.authorId;
    if(authorId==null) return null;

    var id = options.id || 0;
    var title = options.title || "My presentation";
    var tags = options.tags || [];
    var slides = options.slides || [];
        
    self.authorId = authorId;
    self.id = id;
    self.title = ko.observable(title);
    self.tags = ko.observableArray(tags);

    self.slides = ko.observableArray(slides);
    self.currentSlideNumber = ko.observable(0);
    
    self.currentSlide = ko.computed(function () {
        return self.slides()[self.currentSlideNumber()];
    });

    // Operations
    self.addSlide = function (ops) {
        self.slides.push(new PresentationSlide(ops));
    };

    self.changeSlide = function (slideId) {
        self.currentSlideNumber = slideId;
    };
    
}

var presentation = new PresentationModel({
    id: 12345,
    title: 'My presentation',
    authorId: 543,
    tags: ['slides', 'kawaii', 'work staff']
});

presentation.slides().push(new PresentationSlide({}));
presentation.slides().push(new PresentationSlide({}));
presentation.slides().push(new PresentationSlide({}));

ko.applyBindings(presentation);