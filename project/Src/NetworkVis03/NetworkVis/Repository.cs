using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data.OleDb;
using System.Windows;
using MySql.Data.MySqlClient;

namespace NetworkVis
{
    class Repository
    {
        //public OleDbConnection RepositoryConnection;
        public MySqlConnection RepositoryConnection;
        private List<string> connectionInfo;


        //public Repository()
        //{
        //    this.connectionInfo = connectionInfo;
        //    this.connectionInfo = new List<string>();
        //    this.connectionInfo.Add("localhost");
        //    this.connectionInfo.Add("enricolu");
        //    this.connectionInfo.Add("111platform!");
        //    this.connectionInfo.Add("networkvis");
        //}
        public bool checkDBExist()
        {
            this.connectionInfo = new List<string>();
            this.connectionInfo.Add("localhost");
            this.connectionInfo.Add("enricolu");
            this.connectionInfo.Add("111platform!");
            this.connectionInfo.Add("networkvis");

            string dbHost = connectionInfo.ElementAt<string>(0);
            string dbUser = connectionInfo.ElementAt<string>(1);
            string dbPass = connectionInfo.ElementAt<string>(2);
            string dbName = connectionInfo.ElementAt<string>(3);
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass;// +";CharSet=utf8_general_ci";// +";CharSet=utf8mb4";
            RepositoryConnection = new MySqlConnection(connStr);

            // 連線到資料庫 
            try
            {
                RepositoryConnection.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                RepositoryConnection = null;
                return false;
            }
            catch
            {
                return false;
            }
            try
            {
                RepositoryConnection.Close();
            }
            catch { }
            return true;
        }
        public void Close_Repository()
        {
           // try
           // {
           //     RepositoryConnection.Close();

           // }

           ////Some usual exception handling
           // catch (OleDbException e)
           // {
           //     MessageBox.Show(e.Errors[0].Message);
           // }
            try
            {
                RepositoryConnection.Close();
            }
            catch (Exception closeException)
            {
                Console.WriteLine(closeException.ToString());
            }
        }

        public void Open_Repository()
        {
            ////create the database connection
            //string DataBaseRoot = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()).Replace(@"file:\", "") ;

            //RepositoryConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DataBaseRoot + "\\Associations.mdb");

            //try
            //{
            //    RepositoryConnection.Open();

            //}

            ////Some usual exception handling
            //catch (OleDbException e)
            //{
            //    //MessageBox.Show(e.Errors[0].Message);
            //}
            this.connectionInfo = new List<string>();
            this.connectionInfo.Add("localhost");
            this.connectionInfo.Add("enricolu");
            this.connectionInfo.Add("111platform!");
            this.connectionInfo.Add("networkvis");

            string dbHost = connectionInfo.ElementAt<string>(0);
            string dbUser = connectionInfo.ElementAt<string>(1);
            string dbPass = connectionInfo.ElementAt<string>(2);
            string dbName = connectionInfo.ElementAt<string>(3);
            //Console.WriteLine(dbHost + "-" + dbUser);
            // 如果有特殊的編碼在database後面請加上;CharSet=編碼, utf8請使用utf8_general_ci 
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;// +";CharSet=utf8_general_ci"; ;// ";CharSet=utf8mb4;";
            RepositoryConnection = new MySqlConnection(connStr);

            // 連線到資料庫 
            try
            {
                RepositoryConnection.Open();
                //return "";
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                RepositoryConnection = null;
                //switch (ex.Number)
                //{
                //    case 0:
                //        //return "無法連線。";
                //    case 1045:
                //        //return "使用者帳號或密碼錯誤，請再試一次。";
                //    case 1049:
                //        //return dbName + "不存在，請再試一次。";
                //    default:
                //        //return "發生不明連線錯誤。";
                //}
            }
        }
        public MySqlDataReader mysql_query(string sql)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, RepositoryConnection);
                cmd.CommandTimeout = 120000;
                MySqlDataReader myData = cmd.ExecuteReader();
                return myData;
            }
            catch (Exception ex)
            {
                //mydata.AppendText( sql + "\r\nError " + ex.Number + " : " + ex.Message);
                Console.WriteLine("錯啦  " + ex.ToString());
            }
            return null;
        }
    }
}
