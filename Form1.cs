using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
#pragma warning disable SYSLIB0011 // 类型或成员已过时

namespace MoXie
{
    public partial class Form1 : Form
    {
        private readonly string path = "";

        public Form1(string[] args)
        {
            InitializeComponent();
            foreach (string arg in args)
            {
                path = arg != "" && File.Exists(arg) ? arg : "";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            label1.Text = "准备开始";
            try
            {
                List<Question> questions = GetQuestions();

                label1.Text = "保存题目";
                Stream stream = new FileStream($"questions_{DateTime.Now.Ticks}.bin", FileMode.Create, FileAccess.Write, FileShare.None);
                new BinaryFormatter().Serialize(stream, questions);
                stream.Close();

                OpenInPowerPoint(questions);
            }
            catch (Exception ex)
            {
                label1.Text = "生成失败";
                _ = MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            label1.Text = "准备就绪";
        }

        private List<Question> GetQuestions()
        {
            label1.Text = "生成题目";
            TextBook textBook = new TextBook().FromString(File.ReadAllLines("data\\textbook.txt"));
            List<Question> questions = ReadQuestions("data\\questions.txt");
            questions.AddRange(textBook.GetQuestions());

            label1.Text = "打乱题目";
            for (int i = 0; i < 5; i++)
            {
                questions = RandomSortList(questions);
            }

            label1.Text = "随机抽题";
            #region 方法1(弃用)
            /*
            int[] indexs = new int[5];
            for (int i = 0; i < 5; i++)
            {
                indexs[i] = Random.Shared.Next(questions.Count - 1);
                if (i > 0 && indexs[i - 1] == indexs[i])
                {
                    i--;
                }
            }
            List<Question> questions1 = new();
            foreach (int i in indexs)
            {
                questions1.Add(questions[i]);
            }
            questions = question1;
            */
            #endregion
            #region 方法2
            for (int i = 0; questions.Count > 5; i++)
            {
                _ = questions.Remove(questions[Random.Shared.Next(0, questions.Count - 1)]);
            }
            #endregion

            return questions;
        }

        private void OpenInPowerPoint(List<Question> questions)
        {
            label1.Text = "清空缓存";
            if (Directory.Exists("temp"))
            {
                Directory.Delete("temp", true);
            }
            _ = Directory.CreateDirectory("temp\\ppt\\slides");

            label1.Text = "复制pptx";
            File.Copy("data\\pptx.zip", "temp\\pptx.zip");

            label1.Text = "生成slide";
            string s = QuestionsToString(questions);
            File.WriteAllText("temp\\ppt\\slides\\slide1.xml", s);

            label1.Text = "打包pptx";
            Directory.SetCurrentDirectory("temp");
            Process? process = Process.Start(new ProcessStartInfo { FileName = "..\\7z.exe", Arguments = "a pptx.zip ppt\\slides\\slide1.xml", CreateNoWindow = true });
            if (process is null)
            {
                throw new Exception("Can not start 7z.exe");
            }
            else
            {
                process.WaitForExit();
            }
            File.Move("pptx.zip", "MoXie.pptx");
            Directory.SetCurrentDirectory("..");

            label1.Text = "打开pptx";
            Process.Start("explorer.exe", "temp\\MoXie.pptx").WaitForExit();
        }

        private static List<Question> ReadQuestions(string path)
        {
            string[] lines = File.ReadAllLines(path);
            List<Question> questions = new();
            foreach (string line in lines)
            {
                if (!line.StartsWith(";") && line != "" && line.Contains('_'))
                {
                    questions.Add(Question.FromString(line));
                }
            }
            return questions;
        }

        private static string QuestionsToString(List<Question> questions)
        {
            string s = File.ReadAllText("data\\file_pre.xml");
            foreach (Question question in questions)
            {
                s += question.ToString();
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

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (path != "")
            {
                button1.Dispose();
                groupBox1.Dispose();
                Refresh();
                try
                {
                    label1.Text = "正在读取";
                    Stream stream = File.OpenRead(path);
                    List<Question> questions = (List<Question>)new BinaryFormatter().Deserialize(stream);
                    OpenInPowerPoint(questions);
                }
                catch { }
                Environment.Exit(0);
            }
        }
    }
}