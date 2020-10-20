using System.Collections.Generic;

namespace CvCreator.Api.JsReport
{
    public class ElementStyle
    {
        private Dictionary<string, string> styles = new Dictionary<string, string>();

        public void AddStyle(string key, string value)
        {
            styles.Add(key, value);
        }

        public void AddStyle(string key, int value)
        {
            styles.Add(key, value.ToString());
        }

        public string GetStyle()
        {
            string result = string.Empty;

            foreach (var style in styles)
            {
                result += style.Key + ":" + style.Value + ";";
            }

            return result;
        }
    }
}
