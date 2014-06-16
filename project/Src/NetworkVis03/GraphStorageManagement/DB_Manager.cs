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
        public DB_setting setting;
        public MySqlConnection RepositoryConnection;
        private List<string> connectionInfo;
        private String Database;
        public DB_Manager(String _Database)
        {
            this.Database = _Database;
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
                RepositoryConnection.Close();
                RepositoryConnection = null;
                Console.WriteLine("錯啦  " + ex.ToString());
            }
            setting = new DB_setting();
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
        private int count_column_repeatness(String ColumnName, String Table)
        {
            DataTable res = mysql_query(String.Format("SELECT count(DISTINCT {0}) FROM {1}.{2};", ColumnName, Database, Table));
            return int.Parse(res.Rows[0][0].ToString());
        }

        private bool filter_vertex_by_usability(String ColumnName, String Table)
        {
            if (count_column_repeatness(ColumnName, Table) < 5)
                return false;
            return true;
        }
        private void filter_vertex(DataColumnCollection columns, DataTable table, String TableName)
        {
            int i = 0;
            String[] bit_filter = new String[columns.Count];
            foreach (DataColumn column in columns)
            {
                if (!filter_vertex_by_usability(column.ColumnName, TableName))
                    bit_filter[i++] = column.ColumnName;
            }
            int end = i;
            for (int k = 0; k < end; k++)
            {
                Console.WriteLine(bit_filter[k]);
                table.Columns.Remove(bit_filter[k]);
            }
        }

        public String[] list_post_schema()
        {
            DataTable table = mysql_query("select * from allinone limit 1");
            // Use a DataTable object's DataColumnCollection.
            DataColumnCollection columns = table.Columns;

            // Print the ColumnName and DataType for each column. 
            filter_vertex(columns,table, "allinone");
            int i = 0;
            String[] result = new String[columns.Count];
            foreach (DataColumn column in columns)
            {
                result[i++] = column.ColumnName;
               // Console.WriteLine(column.DataType);
            }
            return result;
        }
        public void list_post_like_schema()
        {
            DataTable table = mysql_query("select * from pages_posts_likes limit 1");
            DataColumnCollection columns = table.Columns;
            filter_vertex(columns, table, "pages_posts_likes");
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


        public Graph get_network(String ID)
        {
            String edge_sql = String.Format("SELECT * FROM networkvis.edges WHERE NetworkID = \"{0}\"", ID);
            String vertex_sql = String.Format("SELECT * FROM networkvis.nodes WHERE NetworkID = \"{0}\"", ID);
            
            DataTable edge_table = mysql_query(edge_sql);
            DataTable vertex_table = mysql_query(vertex_sql);
            DB_Converter conv = new DB_Converter();
            return conv.convert_to_graph(vertex_table, edge_table);
            /*Get Group information from group table*/
            /*Call DB_Converter to generate graph */
        }

        public Graph get_network(String NetWorkID, int option)
        {
            String edgeCol = setting.edgeCol;
            String vertexCol = setting.vertexCol;
            String vertex_sql = String.Format("SELECT DISTINCT {0} FROM networkvis.allinone ", vertexCol);
            DataTable vertex_table = mysql_query(vertex_sql);
            vertex_table.Columns.Add("ID", typeof(int));
            vertex_table.Columns[vertexCol].ColumnName = "NodeName";
            int i = 0;
            foreach (DataRow row in vertex_table.Rows)
            {
                row["ID"] = i++;
            }
            DataTable edge_table = GraphEdgeGen.EdgeGen(this, vertex_table, vertexCol, edgeCol);
           // export_to_db(vertex_table, NetWorkID, "nodes");
           // export_to_db(edge_table, NetWorkID, "edges");
           // /*Use setting to generate sql and get DataTable*/
            DB_Converter conv = new DB_Converter();
            return conv.convert_to_graph(vertex_table,edge_table);
        }

        private static void PrintTableOrView(DataTable table, string label)
        {
            System.IO.StringWriter sw;
            string output;

            Console.WriteLine(label);

            // Loop through each row in the table.
            foreach (DataRow row in table.Rows)
            {
                sw = new System.IO.StringWriter();
                // Loop through each column.
                foreach (DataColumn col in table.Columns)
                {
                    // Output the value of each column's data.
                    sw.Write(row[col].ToString() + ", ");
                }
                output = sw.ToString();
                // Trim off the trailing ", ", so the output looks correct.
                if (output.Length > 2)
                {
                    output = output.Substring(0, output.Length - 2);
                }
                // Display the row in the console window.
                Console.WriteLine(output);
            } //
            Console.WriteLine();
        }

        public void export_to_db(DataTable table, String NetworkID, String label){
            string sQuery = String.Format("Select * from networkvis.{0} WHERE NetWorkID =\"{1}\"",label,NetworkID);

            MySqlDataAdapter myDA = new MySqlDataAdapter(sQuery, RepositoryConnection);
            MySqlCommandBuilder cmb=new MySqlCommandBuilder(myDA);

            DataTable MyDT = new DataTable();// <- My DataTable
            myDA.Fill(MyDT);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow data = table.Rows[i];
                DataRow new_data = MyDT.NewRow();
                for (int k = 0; k < table.Columns.Count; k++)
                {
                    if (new_data[k].GetType() == typeof(Int32))
                    {
                        new_data[k] = int.Parse(data[k].ToString());
                    }else
                       new_data[k] = data[k];
                }
                MyDT.Rows.Add(new_data);
            }
            //PrintTableOrView(MyDT, "test");
            
            //Add new rows or delete/update existing one
             //and update the DataTable using 

            myDA.Update(MyDT);
        }

        public void close()
        {
            RepositoryConnection.Close();
        }
    }
}
