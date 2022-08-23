namespace MoXie
{
    internal class TextBook
    {
        public List<Text> texts;

        public TextBook(List<Text>? texts = null)
        {
            this.texts = texts is null ? new() : texts;
        }

        public TextBook FromString(string[] lines)
        {
            Text text = new();
            foreach (string line in lines)
            {
                if (line.StartsWith("="))
                {
                    text.title = line[1..];
                }
                else if (line.StartsWith("-"))
                {
                    text.anthor = line[1..];
                }
                else if (line.StartsWith("+"))
                {
                    text.lines.Add(new TextLine().FromString(line[1..]));
                }
                else if (line.StartsWith("_"))
                {
                    texts.Add(text);
                    text = new Text();
                }
            }
            return this;
        }
    }

    internal class Text
    {
        public string title;
        public string anthor;
        public List<TextLine> lines;

        public Text(string title = "", string anthor = "", List<TextLine>? lines = null)
        {
            this.title = title;
            this.anthor = anthor;
            this.lines = lines is null ? new() : lines;
        }
    }

    internal class TextLine
    {
        public List<LinePart> parts;

        public TextLine(List<LinePart>? parts = null)
        {
            this.parts = parts is null ? new() : parts;
        }

        public TextLine FromString(string s)
        {
            char[] chars = s.ToCharArray();
            LinePart part = new();
            foreach (char c in chars)
            {
                if (c == '[')
                {
                    if (part.part != "")
                    {
                        part.hide = false;
                        parts.Add(part);
                        part = new();
                    }
                }
                else if (c == ']')
                {
                    if (part.part != "")
                    {
                        part.hide = true;
                        parts.Add(part);
                        part = new();
                    }
                }
                else
                {
                    part.part += c;
                }
            }
            parts.Add(part);
            return this;
        }
    }

    internal class LinePart
    {
        public string part;
        public bool hide;

        public LinePart(string part = "", bool hide = false)
        {
            this.part = part;
            this.hide = hide;
        }
    }
}
