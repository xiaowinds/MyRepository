
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;


namespace MyERPClassLibrary
{
    /// <summary> 
    /// 数据库访问类 
    /// </summary>
    public static class DBHelper
    {
        private static readonly string ConnString = "server=qmdb02;database=qingmao20190724;User ID=packing_sa;pwd=12345678;";

        /// <summary> 
        /// 无参增删改操作 
        /// </summary> 
        /// <param name="sql"></param> 
        /// <returns></returns>
        public static int Exec(string sql)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        /// <summary> 
        /// 无参单值查询 
        /// </summary> 
        /// <param name="sql"></param> 
        /// <returns></returns>
        public static object GetSingle(string sql)
        {
            object obj = null;
            using (SqlConnection con = new SqlConnection(ConnString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                obj = cmd.ExecuteScalar();
            }
            return obj;
        }

        /// <summary> 
        /// 无参多值查询 
        /// </summary> 
        /// <param name="sql"></param> 
        /// <returns></returns>
        public static DataTable GetMany(string sql)
        {
            SqlConnection con = new SqlConnection(ConnString);
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }

        /// <summary> 
        /// 有参增删改操作 
        /// </summary> 
        /// <param name="sql"></param> 
        /// <returns></returns>
        public static int Exec(string sql, params SqlParameter[] para)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(ConnString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddRange(para);
                i = cmd.ExecuteNonQuery();
            }
            return i;
        }

        /// <summary> 
        /// 有参单值查询 
        /// </summary> 
        /// <param name="sql"></param> 
        /// <returns></returns>
        public static object GetSingle(string sql, params SqlParameter[] para)
        {
            object obj = null;
            using (SqlConnection con = new SqlConnection(ConnString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddRange(para);
                obj = cmd.ExecuteScalar();
            }
            return obj;
        }

        /// <summary> 
        /// 有参多值查询 
        /// </summary> 
        /// <param name="sql"></param> 
        /// <returns></returns>
        public static DataTable GetMany(string sql, params SqlParameter[] para)
        {
            SqlConnection con = new SqlConnection(ConnString);
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            adp.SelectCommand.Parameters.AddRange(para);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            return ds.Tables[0];
        }
    }
}
