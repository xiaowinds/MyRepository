using MyERPClassLibrary;
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
using System.Text.RegularExpressions;

namespace ERP中文导入到SQL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox1.Text))
            {
                string str = File.ReadAllText(textBox1.Text, EncodingType.GetType(openFileDialog1.FileName));
                var strlist = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < strlist.Count(); i++)
                {
                    var s = strlist[i];
                    if (s.Trim()!="" && s.IndexOf("\t")>-1 )
                    {
                        var s2 = s.Split(new string[] { "\t" }, StringSplitOptions.None);
                        var sql1 = string.Format("select 1 from a_trans where name_cn = '{0}'", s2[0]);
                        if (DBHelper.GetSingle(sql1)==null)
                        {
                            var sql2 = string.Format("insert into a_trans ( name_cn, path)values('{0}','{1}')", s2[0], s2[1]);
                            DBHelper.Exec(sql2);
                        }
                    }
                }
                MessageBox.Show("导入成功！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox2.Text))
            {
                string str = File.ReadAllText(textBox2.Text, EncodingType.GetType(openFileDialog1.FileName));
                var strlist = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < strlist.Count(); i++)
                {
                    var s = strlist[i];
                    if (s.Trim() != "" && s.IndexOf("\t") > -1)
                    {
                        var s2 = s.Split(new string[] { "\t" }, StringSplitOptions.None);
                        var sql1 = string.Format("select name_en from a_trans where name_cn = '{0}'", s2[0]);
                        var rs = DBHelper.GetSingle(sql1);
                        if (rs != null)
                        {
                            var sql2 = string.Format("update a_trans set name_en=N'{0}', name_es=N'{1}', name_vn=N'{2}', flg='{4}'  where name_cn='{3}'", s2[1], s2[2], s2[3], s2[0], checkBox4.Checked ? 0 : 1);
                            DBHelper.Exec(sql2);
                        }
                    }
                }
                MessageBox.Show("导入成功！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = openFileDialog1.FileName;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                var reg = new Regex("[\u4e00-\u9fa5？]+__1");
                string str = File.ReadAllText(textBox3.Text, EncodingType.GetType(openFileDialog1.FileName));
                var str_en = str;
                var str_es = str;
                var str_vn = str;
                var arr = reg.Matches(str);
                foreach (Match v in arr)
                {
                    var txt = v.Value.Replace("__1", "");
                    var sql = "select name_en,name_es,name_vn from a_trans where name_cn='" + txt + "'";
                    var dt = DBHelper.GetMany(sql);
                    if (dt.Rows.Count > 0)
                    {
                        var txt_en = dt.Rows[0]["name_en"].ToString();
                        var txt_es = dt.Rows[0]["name_es"].ToString();
                        var txt_vn = dt.Rows[0]["name_vn"].ToString();
                        str_en = str_en.Replace(v.Value, txt_en);
                        str_es = str_es.Replace(v.Value, txt_es);
                        str_vn = str_vn.Replace(v.Value, txt_vn);
                    }
                }
                File.WriteAllText(openFileDialog1.FileName + ".en.asp", str_en, Encoding.UTF8);
                File.WriteAllText(openFileDialog1.FileName + ".es.asp", str_es, Encoding.UTF8);
                File.WriteAllText(openFileDialog1.FileName + ".vn.asp", str_vn, Encoding.UTF8);
                MessageBox.Show("修改成功！");
            }
            catch (Exception ex)
            {

                MessageBox.Show("修改失败！" + ex.Message);
            }
        }
    }
}
