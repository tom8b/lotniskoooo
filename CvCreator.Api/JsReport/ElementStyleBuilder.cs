﻿namespace CvCreator.Api.JsReport
{
    public class ElementStyleBuilder
    {
        private ElementStyle elementStyle;

        public ElementStyleBuilder(int xPosition, int yPosition, int width, int height)
        {
            elementStyle = new ElementStyle();
            elementStyle.AddStyle("position", "absolute");
            elementStyle.AddStyle("left", xPosition);
            elementStyle.AddStyle("top", yPosition);
            elementStyle.AddStyle("width", width);
            elementStyle.AddStyle("height", height);
        }

        public ElementStyleBuilder(double xPosition, double yPosition, string width, string height)
        {
            elementStyle = new ElementStyle();
            elementStyle.AddStyle("position", "absolute");
            elementStyle.AddStyle("left", xPosition);
            elementStyle.AddStyle("top", yPosition);
            elementStyle.AddStyle("width", width);
            elementStyle.AddStyle("height", height);
        }

        public ElementStyleBuilder(int xPosition, int yPosition, string width, string height)
        {
            elementStyle = new ElementStyle();
            elementStyle.AddStyle("position", "absolute");
            elementStyle.AddStyle("left", xPosition);
            elementStyle.AddStyle("top", yPosition);
            elementStyle.AddStyle("width", width);
            elementStyle.AddStyle("height", height);
        }

        public ElementStyleBuilder WithBackgroundColor(string color)
        {
            elementStyle.AddStyle("background-color", color);
            return this;
        }


        public ElementStyleBuilder WithZIndex(int zIndex)
        {
            elementStyle.AddStyle("z-index", zIndex);
            return this;
        }

        public ElementStyleBuilder WithFontSize(string fontSize)
        {
            elementStyle.AddStyle("font-size", fontSize);
            return this;
        }

        public ElementStyleBuilder WithBackgroundImage(string path)
        {
            elementStyle.AddStyle("background-image", path);
            return this;
        }

        public ElementStyle Build()
        {
            var style = elementStyle;
            elementStyle = new ElementStyle();
            return style;
        }
    }
}
