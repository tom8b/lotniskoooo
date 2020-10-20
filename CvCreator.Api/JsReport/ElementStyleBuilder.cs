namespace CvCreator.Api.JsReport
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

        public ElementStyleBuilder WithBackgroundColor(string color)
        {
            elementStyle.AddStyle("background-color", color);
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
