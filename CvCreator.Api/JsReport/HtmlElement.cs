namespace CvCreator.Api.JsReport
{
    public class HtmlElement
    {
        private readonly ElementStyle style;
        private readonly string text;
        private readonly string imagePath;

        public HtmlElement(ElementStyle style, string text, string imagePath )
        {
            this.style = style;
            this.text = text;
            this.imagePath = imagePath;
        }

        public string GetElement()
        {
            return $"<div  style=\"" + style.GetStyle() + "\">"
                + $"<img alt=\"\"  style=\" z-index: -1;position: absolute; top: 0; left: 0; padding: 0; margin-top: 0; vertical-align: middle; max-width: 100%; max-height: 100%; \" src=\"{imagePath}\" />"


                + text + "</div>";
        }
    }
}
