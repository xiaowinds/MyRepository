using MyERPClassLibrary;
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

namespace ERP中文语言修改
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(textBox1.Text))
                {
                    MessageBox.Show("所选目录不存在！");
                    return;
                }
                var lang = "";
                if (radioButton1.Checked)
                {
                    lang = "name_en";
                }
                else if (radioButton2.Checked)
                {
                    lang = "name_es";
                }
                else if (radioButton3.Checked)
                {
                    lang = "name_vn";
                }
                DirectoryInfo root = new DirectoryInfo(textBox1.Text);
                FileInfo[] files = root.GetFiles("*.asp", checkBox1.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                for (int i = 0; i < files.Count(); i++)
                {
                    if (files[i].Extension == ".asp")
                    {
                        if (files[i].DirectoryName.IndexOf("_vti_cnf") > -1 || files[i].DirectoryName.IndexOf("trans") > -1)
                        {
                            continue;
                        }
                        var haschange = false;

                        var str = File.ReadAllText(files[i].FullName, EncodingType.GetType(files[i].FullName));

                        if (str.IndexOf("Response.CodePage") == -1)
                        {
                            var reg = new Regex("%>");
                            str = reg.Replace(str, " CODEPAGE=\"65001\"%>\r\n<% Response.CodePage=65001%>\r\n<% Response.Charset=\"UTF-8\" %>", 1);
                            haschange = true;
                        }
                        if (str.IndexOf("../../../../global.asa") == -1)
                        {
                            str = str.Replace("../../global.asa", "../../../../global.asa");
                            haschange = true;
                        }
                        if (str.IndexOf("../../../../_ScriptLibrary") == -1)
                        {
                            str = str.Replace("../../_ScriptLibrary", "../../../../_ScriptLibrary");
                            haschange = true;
                        }
                        var strlist = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        for (int k = 0; k < strlist.Length; k++)
                        {
                            if (strlist[k].ToLower().IndexOf("\" values") > -1 || strlist[k].ToLower().IndexOf("update ") > -1 || strlist[k].ToLower().IndexOf("delete ") > -1 || strlist[k].ToLower().IndexOf("insert ") > -1)
                            {
                                continue;
                            }
                            var chs = Common.GetChinese(strlist[k].Replace("http://", "").Replace("https://", "")
                                .Split(new string[] { "<!-" }, StringSplitOptions.None)[0]
                                .Split(new string[] { "//" }, StringSplitOptions.None)[0]
                                .Split(new string[] { "*" }, StringSplitOptions.None)[0]
                                .Split(new string[] { "/*" }, StringSplitOptions.None)[0]
                                .Split(new string[] { "<option" }, StringSplitOptions.None)[0]
                                .Split(new string[] { "<OPTION" }, StringSplitOptions.None)[0]
                                .Split(new string[] { " from " }, StringSplitOptions.None)[0]
                                .Split(new string[] { " FROM" }, StringSplitOptions.None)[0],
                                files[i].FullName + " 行号:" + (k + 1));
                            if (chs.Count > 0)
                            {
                                haschange = true;
                                for (int j = 0; j < chs.Count; j++)
                                {
                                    var getlang = DBHelper.GetSingle(string.Format("select {0} from a_trans where name_cn='{1}'", lang, chs[j].name));
                                    if (getlang != null && getlang.ToString() != "")
                                    {
                                        strlist[k] = strlist[k].Replace(chs[j].name, getlang.ToString());
                                    }

                                }
                            }
                        }
                        if (haschange)
                        {
                            try
                            {
                                if (File.GetAttributes(files[i].FullName).ToString().IndexOf("ReadOnly") != -1)
                                {
                                    File.SetAttributes(files[i].FullName, FileAttributes.Normal);
                                }
                                File.WriteAllText(files[i].FullName, string.Join("\r\n", strlist), Encoding.UTF8);

                            }
                            catch (Exception exe)
                            {
                                label1.Text += exe.Message + "\r\n";
                            }
                        }
                    }

                }

                MessageBox.Show("Succsee!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("fail" + ex.Message);

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(textBox2.Text))
                {
                    MessageBox.Show("所选目录不存在！");
                    return;
                }
                DirectoryInfo root = new DirectoryInfo(textBox2.Text);
                FileInfo[] files = root.GetFiles("*.asp", SearchOption.AllDirectories);
                for (int i = 0; i < files.Count(); i++)
                {
                    if (files[i].Extension == ".asp")
                    {
                        if (files[i].DirectoryName.IndexOf("_vti_cnf") > -1 || files[i].DirectoryName.IndexOf("trans") > -1)
                        {
                            continue;
                        }
                        var haschange = false;

                        var str = File.ReadAllText(files[i].FullName, EncodingType.GetType(files[i].FullName));
                        if (str.ToLower().IndexOf(textBox3.Text.ToLower()) == -1)
                        {
                            continue;
                        }
                        if (str.IndexOf("Response.CodePage") == -1)
                        {
                            var reg = new Regex("%>");
                            str = reg.Replace(str, " CODEPAGE=\"65001\"%>\r\n<% Response.CodePage=65001%>\r\n<% Response.Charset=\"UTF-8\" %>", 1);
                            haschange = true;
                        }
                        if (str.IndexOf("../../../../global.asa") == -1)
                        {
                            str = str.Replace("../../global.asa", "../../../../global.asa");
                            haschange = true;
                        }
                        if (str.IndexOf("../../../../_ScriptLibrary") == -1)
                        {
                            str = str.Replace("../../_ScriptLibrary", "../../../../_ScriptLibrary");
                            haschange = true;
                        }
                        var strlist = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        var strlist_en = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        var strlist_es = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        var strlist_vn = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        for (int k = 0; k < strlist.Length; k++)
                        {
                            if (strlist[k].ToLower().IndexOf("\" values") > -1 || strlist[k].ToLower().IndexOf("update ") > -1 || strlist[k].ToLower().IndexOf("delete ") > -1 || strlist[k].ToLower().IndexOf("insert ") > -1)
                            {
                                continue;
                            }
                            var chs = Common.GetChinese(strlist[k].Replace("http://", "").Replace("https://", "").Replace("\t"," ")
                                .Split(new string[] { "<!-" }, StringSplitOptions.None)[0]
                                .Split(new string[] { "//" }, StringSplitOptions.None)[0]
                                .Split(new string[] { "*" }, StringSplitOptions.None)[0]
                                .Split(new string[] { "/*" }, StringSplitOptions.None)[0]
                                .Split(new string[] { "<option" }, StringSplitOptions.None)[0]
                                .Split(new string[] { "<OPTION" }, StringSplitOptions.None)[0]
                                .Split(new string[] { " from " }, StringSplitOptions.None)[0]
                                .Split(new string[] { " FROM" }, StringSplitOptions.None)[0],
                                files[i].FullName + " 行号:" + (k + 1));
                            if (chs.Count > 0)
                            {
                                chs = chs.OrderByDescending(q => q.name.Length).ToList();
                                haschange = true;
                                for (int j = 0; j < chs.Count; j++)
                                {
                                    var dt = DBHelper.GetMany(string.Format("select {0},{1},{2} from a_trans where name_cn='{3}'", "name_en","name_es","name_vn", chs[j].name));
                                    if (dt.Rows.Count > 0)
                                    {
                                        strlist_en[k] = strlist_en[k].Replace(chs[j].name, dt.Rows[0]["name_en"].ToString());
                                        strlist_es[k] = strlist_es[k].Replace(chs[j].name, dt.Rows[0]["name_es"].ToString());
                                        strlist_vn[k] = strlist_vn[k].Replace(chs[j].name, dt.Rows[0]["name_vn"].ToString());
                                    }

                                }
                            }
                        }
                        if (haschange)
                        {
                            try
                            {
                                if (File.GetAttributes(files[i].FullName).ToString().IndexOf("ReadOnly") != -1)
                                {
                                    File.SetAttributes(files[i].FullName, FileAttributes.Normal);
                                }
                                var path_en = "/trans/EN";
                                var path_es = "/trans/ES";
                                var path_vn = "/trans/VN";
                                if (files[i].FullName.IndexOf("\\") > -1)
                                {
                                    path_en = "\\trans\\EN";
                                    path_es = "\\trans\\ES";
                                    path_vn = "\\trans\\VN";
                                }
                                var path = textBox2.Text;
                                if (textBox2.Text.LastIndexOf("/") == textBox2.Text.Length - 1 || textBox2.Text.LastIndexOf("\\") == textBox2.Text.Length - 1)
                                {
                                    path = textBox2.Text.Substring(0, textBox2.Text.Length - 1);
                                }
                                if (!Directory.Exists(Path.GetDirectoryName(files[i].FullName.Replace(path, path + path_en))))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(files[i].FullName.Replace(path, path + path_en)));
                                }
                                if (!Directory.Exists(Path.GetDirectoryName(files[i].FullName.Replace(path, path + path_es))))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(files[i].FullName.Replace(path, path + path_es)));
                                }
                                if (!Directory.Exists(Path.GetDirectoryName(files[i].FullName.Replace(path, path + path_vn))))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(files[i].FullName.Replace(path, path + path_vn)));
                                }
                                File.WriteAllText(files[i].FullName.Replace(path, path + path_en), string.Join("\r\n", strlist_en), Encoding.UTF8);
                                File.WriteAllText(files[i].FullName.Replace(path, path + path_es), string.Join("\r\n", strlist_es), Encoding.UTF8);
                                File.WriteAllText(files[i].FullName.Replace(path, path + path_vn), string.Join("\r\n", strlist_vn), Encoding.UTF8);

                            }
                            catch (Exception exe)
                            {
                                label1.Text += exe.Message + "\r\n";
                            }
                        }
                    }

                }

                MessageBox.Show("Succsee!");

            }
            catch (Exception ex)
            {
                MessageBox.Show("fail" + ex.Message);

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = openFileDialog1.FileName;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox4.Text))
            {
                string str = File.ReadAllText(textBox4.Text, EncodingType.GetType(openFileDialog1.FileName));
                var strlist = str.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < strlist.Count(); i++)
                {
                    
                    var s = strlist[i];
                    if (string.IsNullOrEmpty(s))
                    {
                        continue;
                    }
                    var s2 = s.Replace(@"C:\Users\zhangyaof\Desktop\qingmao\ERP原件", @"C:\Users\zhangyaof\Desktop\qingmao\ERP原件副本").Replace(@"\\qmrpt02\qingmao_test", @"C:\Users\zhangyaof\Desktop\qingmao\ERP原件副本");
                    if (!Directory.Exists(Path.GetDirectoryName(s2))) {
                        Directory.CreateDirectory(Path.GetDirectoryName(s2));
                    }
                    File.Copy(s, s2,true);

                }
                MessageBox.Show("复制成功！");

            }
        }
    }
}