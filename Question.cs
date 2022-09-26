namespace MoXie
{
    [Serializable]
    internal class Question
    {
        public QuestionPart[] Parts;

        public Question()
        {
            Parts = Array.Empty<QuestionPart>();
        }

        public Question(QuestionPart[] parts)
        {
            Parts = parts;
        }

        public Question AddPart(QuestionPart part)
        {
            List<QuestionPart> parts = Parts.ToList();
            parts.Add(part);
            Parts = parts.ToArray();
            return this;
        }

        public Question AddPart(string part, bool hide = false)
        {
            List<QuestionPart> parts = Parts.ToList();
            parts.Add(new QuestionPart(part, hide));
            Parts = parts.ToArray();
            return this;
        }

        public static Question FromString(string s)
        {
            Question question = new();
            char[] chars = s.ToCharArray();
            string part = "";
            bool hide = false;
            foreach (char c in chars)
            {
                if (c == '[')
                {
                    _ = question.AddPart(part, hide);
                    part = "";
                    hide = true;
                }
                else if (c == ']')
                {
                    _ = question.AddPart(part, hide);
                    part = "";
                    hide = false;
                }
                else
                {
                    part += c;
                }
            }
            _ = question.AddPart(part, hide);
            return question;
        }

        public override string ToString()
        {
            string question = File.ReadAllText("data\\line_pre.xml");
            foreach (QuestionPart part in Parts)
            {
                question += part.ToString();
            }
            question += File.ReadAllText("data\\line_end.xml");
            return question;
        }
    }

    [Serializable]
    internal class QuestionPart
    {
        public string Part;
        public bool Hide;

        public QuestionPart(string part, bool hide = false)
        {
            Part = part;
            Hide = hide;
        }

        public override string ToString()
        {
            return $"{(Hide ? File.ReadAllText("data\\part_pre_hide.xml") : File.ReadAllText("data\\part_pre.xml"))}{Part}{File.ReadAllText("data\\part_end.xml")}";
        }
    }
}