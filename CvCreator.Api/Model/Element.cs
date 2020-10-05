using System;

namespace CvCreator.Api.Model
{
    public class Element
    {
        public Guid Id { get; set; }
        public Position Position { get; set; }
        public Size Size { get; set; }
        public Content Content { get; set; }
        public bool UserFillsOut { get; set; }
    }

    public class Content
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }

    public class Size
    {
        public Guid Id { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
    }

    public class Position
    {
        public Guid Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
