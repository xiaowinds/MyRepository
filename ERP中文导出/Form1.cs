using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyERPClassLibrary;
using Newtonsoft.Json;

namespace ERPChineseExport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox1.Text))
            {
                MessageBox.Show("所选目录不存在！");
                return;
            }
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            var ChineseWordPath = Path.Combine(Environment.CurrentDirectory, "ChineseWord.json");
            var strarr = new List<MyObj>();
            var newstrarr = new List<MyObj>();
            if (File.Exists(ChineseWordPath))
            {
                strarr = JsonConvert.DeserializeObject<List<MyObj>>(File.ReadAllText(ChineseWordPath, Encoding.UTF8));
            }

            DirectoryInfo root = new DirectoryInfo(textBox1.Text);
            FileInfo[] files = root.GetFiles("*.asp", checkBox1.Checked? SearchOption.AllDirectories:SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Count(); i++)
            {
                if (files[i].Extension == ".asp")
                {
                    if (files[i].DirectoryName.IndexOf("_vti_cnf") > -1)
                    {
                        continue;
                    }

                    var str = File.ReadAllText(files[i].FullName, EncodingType.GetType(files[i].FullName));

                    // Regex reg = new Regex(@"(?<=\/\*)[.\s\S]*?(?=(\*\/))", RegexOptions.Multiline | RegexOptions.Singleline);

                    var strlist = str.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")
                        .Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    for (int k = 0; k < strlist.Length; k++)
                    {
                        if (strlist[k].ToLower().IndexOf("update ") > -1 || strlist[k].ToLower().IndexOf("delete ") > -1 || strlist[k].ToLower().IndexOf("insert ") > -1 || strlist[k].ToLower().IndexOf("where ") > -1)
                        {
                            continue;
                        }

                        newstrarr.AddRange(Common.GetChinese(strlist[k].Replace("http://","").Replace("https://","")
                            .Split(new string[] { "<!-" }, StringSplitOptions.None)[0]
                            .Split(new string[] { "//" }, StringSplitOptions.None)[0]
                            .Split(new string[] { "*" }, StringSplitOptions.None)[0]
                            .Split(new string[] { "/*" }, StringSplitOptions.None)[0]
                            .Split(new string[] { "<option" }, StringSplitOptions.None)[0]
                            .Split(new string[] { "<OPTION" }, StringSplitOptions.None)[0]
                            .Split(new string[] { " from " }, StringSplitOptions.None)[0]
                            .Split(new string[] { " FROM" }, StringSplitOptions.None)[0],
                            files[i].FullName + " 行号:" + (k + 1)));

                    }
                }
            }
            var list = newstrarr.Distinct(new Compare()).ToList();
            for (int j = 0; j < list.Count(); j++)
            {
                if (strarr.FindAll(q => q.name == list[j].name).Count == 0)
                {
                    var index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = list[j].name.Trim();
                    dataGridView1.Rows[index].Cells[1].Value = string.Join(" | ", list[j].dir);
                }
            }
            strarr.AddRange(list);
            File.WriteAllText(ChineseWordPath, JsonConvert.SerializeObject(strarr.Distinct(new Compare()).ToList()), Encoding.UTF8);

        }

        public class Compare : IEqualityComparer<MyObj>
        {
            public bool Equals(MyObj x, MyObj y)
            {
                if (x.name == y.name)
                {
                    x.dir.AddRange(y.dir);
                    x.dir = x.dir.Distinct().ToList();
                }
                return x.name == y.name;//可以自定义去重规则，此处将name相同的就作为重复记录
            }
            public int GetHashCode(MyObj obj)
            {
                return obj.name.GetHashCode();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            var ChineseWordPath = Path.Combine(Environment.CurrentDirectory, "ChineseWord.json");
            var strarr = new List<MyObj>();
            if (File.Exists(ChineseWordPath))
            {
                strarr = JsonConvert.DeserializeObject<List<MyObj>>(File.ReadAllText(ChineseWordPath, Encoding.UTF8));
            }
            for (int j = 0; j < strarr.Count(); j++)
            {

                var index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = strarr[j].name.Trim();
                dataGridView1.Rows[index].Cells[1].Value = string.Join(" | ", strarr[j].dir);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox1.Text))
            {
                MessageBox.Show("所选目录不存在！");
                return;
            }
            DirectoryInfo root = new DirectoryInfo(textBox1.Text);
            var dirs =  root.GetDirectories("_vti_cnf", SearchOption.AllDirectories);
            if (dirs.Length == 0)
            {
                MessageBox.Show("所选目录未找到_vti_cnf！");
                return;
            }
            for (int i = 0; i < dirs.Length; i++)
            {
                if (Directory.Exists(dirs[i].FullName))
                {
                    dirs[i].Delete(true);
                }
            }
            MessageBox.Show("删除完成");
        }
    }
}