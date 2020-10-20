namespace CvCreator.Api.JsReport
{
    public class HtmlElement
    {
        private readonly ElementStyle style;
        private readonly string text;

        public HtmlElement(ElementStyle style, string text)
        {
            this.style = style;
            this.text = text;
        }

        public string GetElement()
        {
            return "<div style=\"" + style.GetStyle() + "\">" + text + "</div>";
        }
    }
}
