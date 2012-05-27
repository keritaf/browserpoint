/// <reference path="/Scripts/knockout.js"/>

function BlockParameters() {
    var self = this;
    self.top = ko.observable(0);
    self.left = ko.observable(0);
    self.width = ko.observable(0);
    self.height = ko.observable(0);
    self.rotation = ko.observable(0);
    self.scaleX = ko.observable(1);
    self.scaleY = ko.observable(1);
    self.zindex = ko.observable(0);
}

function TextObject() {
    var self = this;
    self.text = ko.observable('');
    self.type = ko.observable('');
    self.blockParams = ko.observable(new BlockParameters());
}

function ImageObject() {
    var self = this;
    self.url = ko.observable('');
    self.blockParams = ko.observable(new BlockParameters());
}

function PresentationSlideTheme() {
    var self = this;
    self.type = ko.observable('');
    self.typeName = ko.observable('Default');
    self.staticTexts = ko.observable([]);
    self.staticImages = ko.observable([]);
}

function PresentationSlide() {
    var self = this;
    self.type = ko.observable('');
    self.texts = ko.observableArray([]);
    self.images = ko.observableArray([]);
}

function PresentationTheme() {
    var self = this;
    self.id = 0;
    self.name = ko.observable('Unnamed theme');
    self.author = ko.observable('');
    self.description = ko.observable('');
    self.sizeX = ko.observable(800);
    self.sizeY = ko.observable(600);
    self.aspectRatio = ko.computed(function () {
        return self.sizeX / self.sizeY;
    });
    self.slideThemes = ko.observableArray([]);
}

function PresentationModel() {
    var self = this;

    self.id = 0;
    self.title = ko.observable('My presentation');
    self.authorId = 0;
    self.tags = ko.observableArray(['slides', 'kawaii', 'work staff']);
    self.theme = ko.observable('default');
    self.themes = [];

    self.slides = ko.observableArray([]);
    self.currentSlideNumber = ko.observable(0);
    self.currentSlide = ko.computed(function () {
        return self.slides[self.currentSlideNumber];
    });
}
model = new PresentationModel();

ko.applyBindings(model);


