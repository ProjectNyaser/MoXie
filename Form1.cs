using System.Diagnostics;

namespace MoXie
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "׼����ʼ";
            try
            {
                if (Directory.Exists("temp"))
                {
                    Directory.Delete("temp", true);
                }

                _ = Directory.CreateDirectory("temp\\ppt\\slides");
                File.Copy("data\\pptx.zip", "temp\\pptx.zip");

                label1.Text = "������Ŀ";
                string[] lines = File.ReadAllLines("data\\questions.txt");
                List<Question> questions = new();
                foreach (string line in lines)
                {
                    if (!line.StartsWith(";") && line != "" && line.Contains('_'))
                    {
                        questions.Add(Question.FromString(line));
                    }
                }

                label1.Text = "����pptx";
                string s = QuestionsToString(RandomSortList(questions), 5);
                File.WriteAllText("temp\\ppt\\slides\\slide1.xml", s);

                label1.Text = "���pptx";
                Directory.SetCurrentDirectory("temp");
                Process.Start(new ProcessStartInfo { FileName = "..\\7z.exe", Arguments = "a pptx.zip ppt\\slides\\slide1.xml", CreateNoWindow = true }).WaitForExit();
                File.Move("pptx.zip", "MoXie.pptx");
                Directory.SetCurrentDirectory("..");

                label1.Text = "��pptx";
                Process.Start("explorer.exe", "temp\\MoXie.pptx").WaitForExit();
            }
            catch (Exception ex)
            {
                label1.Text = "����ʧ��";
                _ = MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            label1.Text = "׼������";
        }

        private static string QuestionsToString(List<Question> questions, int count = -1)
        {
            string s = File.ReadAllText("data\\file_pre.xml");
            foreach (Question question in questions)
            {
                if (count == 0)
                {
                    break;
                }
                s += question.ToString();
                count--;
            }
            s += File.ReadAllText("data\\file_end.xml");
            return s;
        }

        public static List<T> RandomSortList<T>(List<T> ListT)
        {
            Random random = new();
            List<T> newList = new();
            foreach (T item in ListT)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }
            return newList;
        }
    }
}