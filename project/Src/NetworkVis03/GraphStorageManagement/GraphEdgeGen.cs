using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GraphStorageManagement
{
    public class GraphEdgeGen
    {
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
        public static DataTable EdgeGen(DB_Manager dbm, DataTable Vertex, String first, String second)
        {
            DataTable group = dbm.mysql_query(String.Format("SELECT {0},{1} FROM networkvis.allinone GROUP BY {2};", first, second, first));
            
            DataTable result = new DataTable();
            result.Columns.Add("FromID", typeof(int));
            result.Columns.Add("ToID", typeof(int));
            for (int id = 0; id < group.Rows.Count;id++)
            {
                DataRow row = group.Rows[id];
                String Content = row[second].ToString();
                for (int id2 = 0; id2 < id; id2++)
                {
                    DataRow data = group.Rows[id2];
                    //Console.WriteLine(data[second].ToString() + " " + Content);
                    if (Content.Equals(data[second].ToString()))
                    {
                        DataRow edge = result.NewRow();
                        edge["FromID"] = id;
                        edge["ToID"] = id2;
                        result.Rows.Add(edge);
                        
                        break;
                    }
                }
            }
            PrintTableOrView(result, "test");
            return result;
        }
    }
}
