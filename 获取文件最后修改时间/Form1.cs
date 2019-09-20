using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 获取文件最后修改时间
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

            DirectoryInfo root = new DirectoryInfo(textBox1.Text);
            FileInfo[] files = root.GetFiles("*.asp",  SearchOption.AllDirectories  );
            for (int i = 0; i < files.Count(); i++)
            {
                if (files[i].DirectoryName.IndexOf("_vti_cnf") > -1 || files[i].DirectoryName.ToLower().IndexOf("trans") > -1)
                {
                    continue;
                }
                var index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = files[i].FullName;
                dataGridView1.Rows[index].Cells[1].Value = files[i].LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;

            }
        }
    }
}
