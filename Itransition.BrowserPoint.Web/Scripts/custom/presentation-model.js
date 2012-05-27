/// <reference path="/Scripts/knockout.js"/>


function BlockParameters(options) {
    var self = this;
    self.top = ko.observable(options.top);
    self.left = ko.observable(options.left);
    self.width = ko.observable(options.width);
    self.height = ko.observable(options.height);
    self.rotation = ko.observable(options.rotation);
    self.zindex = ko.observable(options.zindex);

    self.scaledTop = function () {
        return self.top() / presentation.scaled();
    };

    self.scaledLeft = function () {
        return self.left() / presentation.scaled();
    };

    self.scaledWidth = function () {
        return self.width() / presentation.scaled();
    };

    self.scaledHeight = function () {
        return self.height() / presentation.scaled();
    };

    self.previewTop = function () {
        return self.top() / presentation.scalePreview();
    };

    self.previewLeft = function () {
        return self.left() / presentation.scalePreview();
    };

    self.previewWidth = function () {
        return self.width() / presentation.scalePreview();
    };

    self.previewHeight = function () {
        return self.height() / presentation.scalePreview();
    };
    
    
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

    var scale = ko.observable(1);
    var scalePreview =  ko.observable(0.2);
    
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
