namespace MoXie
{
    [Serializable]
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

        public List<Question> GetQuestions()
        {
            List<Question> questions = new();
            foreach (Text text in texts)
            {
                questions.AddRange(text.GetQuestions());
            }
            return questions;
        }
    }

    [Serializable]
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

        public List<Question> GetQuestions()
        {
            List<Question> questions = new();
            foreach (TextLine line in lines)
            {
                questions.Add(line.GetQuestion($"《{title}》"));
            }
            return questions;
        }
    }

    [Serializable]
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

        public Question GetQuestion(string pre)
        {
            Question question = new(new QuestionPart[] { new QuestionPart(pre) });
            List<LinePart> parts = this.parts.FindAll(delegate (LinePart part)
            {
                return part.hide;
            });
            int count = parts.Count / 2;
            foreach (LinePart part in this.parts)
            {
                if (part.hide & count > 0 & (parts.Count < 2 | Random.Shared.Next(0, 100) >= 50))
                {
                    _ = question.AddPart(part.part, true);
                    count--;
                }
                else
                {
                    _ = question.AddPart(part.part);
                }
                if (part.hide)
                {
                    parts.Remove(part);
                }
            }
            return question;
        }
    }

    [Serializable]
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
