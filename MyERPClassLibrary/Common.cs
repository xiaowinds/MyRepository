using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyERPClassLibrary
{
    public class Common
    {
        public static List<MyObj> GetChinese(string str, string dir)
        {
            List<MyObj> ls = new List<MyObj>();
            Regex reg = new Regex("[\u4e00-\u9fa5；0-9，？。！!?, ()%％/（）~、-]+");
            foreach (Match v in reg.Matches(str))
            {
                var s = v.Value.Trim();
                if (s == "" || "；，？。！!?, ()%％/（）~、-".Contains(s) || new Regex("[\u4e00-\u9fa5]").IsMatch(s) == false)
                {
                    continue;
                }
                if (s.IndexOf(",") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("，") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("(") == s.Length - 1)
                {
                    s = s.Substring(0, s.Length - 1);
                }
                if (s.IndexOf("（") == s.Length - 1)
                {
                    s = s.Substring(0, s.Length - 1);
                }
                if (s.IndexOf(")") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf(")") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf(",") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("，") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("；") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("%") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("％") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("/") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("/") == s.Length - 1)
                {
                    s = s.Substring(0, s.Length - 1);
                }
                if (s.IndexOf("-") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("-") == s.Length - 1)
                {
                    s = s.Substring(0, s.Length - 1);
                }
                if (s.IndexOf("-") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("-") == s.Length - 1)
                {
                    s = s.Substring(0, s.Length - 1);
                }
                if (s.IndexOf("-") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("-") == s.Length - 1)
                {
                    s = s.Substring(0, s.Length - 1);
                }
                if (s.IndexOf("-") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("-") == s.Length - 1)
                {
                    s = s.Substring(0, s.Length - 1);
                }
                if (s.IndexOf("-") == 0)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.IndexOf("-") == s.Length - 1)
                {
                    s = s.Substring(0, s.Length - 1);
                }
                if (ls.FindAll(q => q.name == s).Count == 0)
                {
                    ls.Add(new MyObj { name = s, dir = new List<string> { GetLast2Dir(dir) } });
                }
            }
            return ls;
        }

        public static string GetLast2Dir(string dir) {
            var dirs = new List<string>();
            if (dir.IndexOf("/") > -1)
            {
                dirs = dir.Split(new string[] { "/" }, StringSplitOptions.None).ToList();
            }
            else if(dir.IndexOf("\\") > -1)
            {
                dirs = dir.Split(new string[] { "\\" }, StringSplitOptions.None).ToList();
            }
            else
            {
                dirs.Add(dir);
            }
            if (dirs.Count > 2)
            {
                return string.Join("/", dirs.GetRange(dirs.Count - 3, 3));
            }
            return string.Join("/", dirs);
        }

        public static List<string> GetChinese(string str)
        {
            //str = @"C# Aggh从入 qq门到11精通";
            List<string> ls = new List<string>();
            Regex reg = new Regex("[\u4e00-\u9fa5]+");
            foreach (Match v in reg.Matches(str))
            {
                if (ls.IndexOf(v.Value) == -1)
                {
                    ls.Add(v.Value);
                }
            }
            return ls;
        }
        public static ArrayList getIndexArray(string inputStr, string findStr)
        {
            ArrayList list = new ArrayList();
            int start = 0;
            while (start < inputStr.Length)
            {
                int index = inputStr.IndexOf(findStr, start);
                if (index >= 0)
                {
                    list.Add(index);
                    start = index + findStr.Length;
                }
                else
                {
                    break;
                }
            }
            return list;
      
        }
    }
}
