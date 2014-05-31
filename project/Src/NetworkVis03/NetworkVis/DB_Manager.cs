﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace NetworkVis
{
    class DB_Manager
    {
        public MySqlConnection RepositoryConnection;
        private List<string> connectionInfo;
        public DB_Manager()
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
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName; ;// +";CharSet=utf8_general_ci";// +";CharSet=utf8mb4";
            RepositoryConnection = new MySqlConnection(connStr);

            // 連線到資料庫 
            try
            {
                RepositoryConnection.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                RepositoryConnection = null;
                RepositoryConnection.Close();
                Console.WriteLine("錯啦  " + ex.ToString());
            }
        }

        public DataTable mysql_query(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, RepositoryConnection);
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
            cmd.CommandTimeout = 120000;
            DataSet dataset = new DataSet();
            dataAdapter.Fill(dataset);
            return dataset.Tables[0];
        }

        public void list_schema()
        {
            DataTable table = mysql_query("select * from allinone limit 1");
            // Use a DataTable object's DataColumnCollection.
            DataColumnCollection columns = table.Columns;

            // Print the ColumnName and DataType for each column. 
            foreach (DataColumn column in columns)
            {
                Console.WriteLine(column.ColumnName);
                Console.WriteLine(column.DataType);
            }
        }

    }

}
