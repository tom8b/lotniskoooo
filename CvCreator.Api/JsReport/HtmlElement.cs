namespace CvCreator.Api.JsReport
{
    public class HtmlElement
    {
        private readonly ElementStyle style;
        private readonly string text;
        private readonly string imagePath;
        private readonly int zIndex;

        public HtmlElement(ElementStyle style, string text, string imagePath, int zIndex )
        {
            this.style = style;
            this.text = text;
            this.imagePath = imagePath;
            this.zIndex = zIndex;
        }

        public string GetElement()
        {
            return $"<div  style=\"" + style.GetStyle() + "\">"
                + $"<img alt=\"\"  style=\" z-index: {zIndex-1};position: absolute; top: 0; left: 0; padding: 0; margin-top: 0; vertical-align: middle; max-width: 100%; max-height: 100%; \" src=\"{imagePath}\" />"


                + text + "</div>";
        }
    }
}
