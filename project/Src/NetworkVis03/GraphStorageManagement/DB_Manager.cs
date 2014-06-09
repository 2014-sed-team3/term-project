using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using Smrf.NodeXL.Core;
namespace GraphStorageManagement
{
    public class DB_Manager
    {
        public MySqlConnection RepositoryConnection;
        private List<string> connectionInfo;
        public DB_Manager(String Database)
        {
            this.connectionInfo = new List<string>();
            this.connectionInfo.Add("localhost");
            this.connectionInfo.Add("enricolu");
            this.connectionInfo.Add("111platform!");
            this.connectionInfo.Add(Database);

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

        public void list_post_schema()
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
        public void list_post_like_schema()
        {
            DataTable table = mysql_query("select * from pages_posts_likes limit 1");
            DataColumnCollection columns = table.Columns;

            // Print the ColumnName and DataType for each column. 
            foreach (DataColumn column in columns)
            {
                Console.WriteLine(column.ColumnName);
                Console.WriteLine(column.DataType);
            }
        }

        public DataTable list_network()
        {
            String sql = "SELECT NetworkID, MAX(count_Edge) as count_Edge, MAX(count_Vertex) as count_Vertex FROM";
            String edge_sql = "SELECT NetworkID, count(EdgeID) as count_Edge, NULL as count_Vertex FROM networkvis.edges GROUP by NetworkID";
            String vertex_sql = "SELECT NetworkID, NULL as count_Edge, count(ID) as count_Vertex FROM networkvis.nodes GROUP by NetworkID";
            String output = String.Format("{0} ({1} UNION ALL {2}) as uni GROUP by NetworkID",sql,edge_sql,vertex_sql);
            return mysql_query(output);
        }

        public void get_network(String ID)
        {
            String edge_sql = String.Format("SELECT * FROM networkvis.edges WHERE NetworkID = {0}", ID);
            String vertex_sql = String.Format("SELECT * FROM networkvis.nodes WHERE NetworkID = {0}", ID);
            /*Get Group information from group table*/
            /*Call DB_Converter to generate graph */
        }

        public void export_to_db(){
            
        }
        public void convert_to_graph()
        {

        }
    }
}
