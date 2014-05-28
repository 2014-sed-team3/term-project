using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.OleDb;

namespace NetworkVis
{
    /// <summary>
    /// Interaction logic for NetProperty.xaml
    /// </summary>
    public partial class NetProperty : Window
    {
        public NetProperty()
        {
            InitializeComponent();
        }

        private void FireConnection(object sender, RoutedEventArgs e)
        {
            string cs="";
            cs = ConnectString.Text;
            //create the database connection
            

            try
            {
                OleDbConnection TestConn = new OleDbConnection(cs);
                TestConn.Open();
                TestConnection.Background = new SolidColorBrush(Colors.Green);
                TestConnection.Foreground = new SolidColorBrush(Colors.White);
                TestConnection.Content = "Connection Successful!";
            }

            //Some usual exception handling
            catch (OleDbException eFailConnect)
            {
                MessageBox.Show(eFailConnect.Message);
                TestConnection.Background = new SolidColorBrush(Colors.Red);
                TestConnection.Foreground = new SolidColorBrush(Colors.White);
                TestConnection.Content = "Connection Failed!";
            }
        }

        private void LoadNodes(object sender, RoutedEventArgs e)
        {
            string cs = "";
            string SQL = "";
            string fieldname = "";
            int looper;
            cs = ConnectString.Text;
            SQL = NodeSQL.Text;
           
            // Clear the drop downs...
            NodeName.Items.Clear();
            NodeID.Items.Clear();
            NodeLabel.Items.Clear();
            NodeValue.Items.Clear();
            NodeType.Items.Clear();

            // Add Null to all the drop downs
            ComboBoxItem ni = new ComboBoxItem();
            ni.Content = "(null)";
            NodeName.Items.Add(ni);
            ComboBoxItem ni2 = new ComboBoxItem();
            ni2.Content = "(null)";
            NodeID.Items.Add(ni2);
            ComboBoxItem ni3 = new ComboBoxItem();
            ni3.Content = "(null)";
            NodeLabel.Items.Add(ni3);
            ComboBoxItem ni4 = new ComboBoxItem();
            ni4.Content = "(null)";
            NodeValue.Items.Add(ni4);
            ComboBoxItem ni5 = new ComboBoxItem();
            ni5.Content = "(null)";
            NodeType.Items.Add(ni5);

            //create the database connection
            
            try
            {
                OleDbConnection TestConn = new OleDbConnection(cs);
                TestConn.Open();
                TestConnection.Background = new SolidColorBrush(Colors.Green);
                TestConnection.Foreground = new SolidColorBrush(Colors.White);
                TestConnection.Content = "Connection Successful!";
                OleDbCommand aCommand = new OleDbCommand(SQL, TestConn);
            
                //create the datareader object to connect to table
                OleDbDataReader aReader = aCommand.ExecuteReader();

                //Iterate throuth the database
                for (looper=0;looper < aReader.FieldCount;looper++)
                {
                    fieldname = aReader.GetName(looper);
                    ComboBoxItem additem = new ComboBoxItem();
                    additem.Content = fieldname;
                    NodeName.Items.Add(additem);
                    ComboBoxItem ni_2 = new ComboBoxItem();
                    ni_2.Content = fieldname;
                    NodeID.Items.Add(ni_2);
                    ComboBoxItem ni_3 = new ComboBoxItem();
                    ni_3.Content = fieldname;
                    NodeLabel.Items.Add(ni_3);
                    ComboBoxItem ni_4 = new ComboBoxItem();
                    ni_4.Content = fieldname;
                    NodeValue.Items.Add(ni_4);
                    ComboBoxItem ni_5 = new ComboBoxItem();
                    ni_5.Content = fieldname;
                    NodeType.Items.Add(ni_5);
                }
                    
                
            }

            //Some usual exception handling
            catch (OleDbException eFailNodeConnect)
            {
                MessageBox.Show(eFailNodeConnect.Message);
                TestConnection.Background = new SolidColorBrush(Colors.Red);
                TestConnection.Foreground = new SolidColorBrush(Colors.White);
                TestConnection.Content = "Connection Failed!";
            }
        }

        private void LoadEdges(object sender, RoutedEventArgs e)
        {
            string cs = "";
            string SQL = "";
            string fieldname = "";
            int looper;
            cs = ConnectString.Text;
            SQL = EdgeSQL.Text;

            // Clear the drop downs...
            EdgeID.Items.Clear();
            EdgeName.Items.Clear();
            FromEdge.Items.Clear();
            ToEdge.Items.Clear();
            EdgeType.Items.Clear();
            
            // Add Null to all the drop downs
            ComboBoxItem ni = new ComboBoxItem();
            ni.Content = "(null)";
            EdgeID.Items.Add(ni);
            ComboBoxItem ni2 = new ComboBoxItem();
            ni2.Content = "(null)";
            EdgeName.Items.Add(ni2);
            ComboBoxItem ni3 = new ComboBoxItem();
            ni3.Content = "(null)";
            EdgeType.Items.Add(ni3);
            ComboBoxItem ni4 = new ComboBoxItem();
            ni4.Content = "(null)";
            FromEdge.Items.Add(ni4);
            ComboBoxItem ni5 = new ComboBoxItem();
            ni5.Content = "(null)";
            ToEdge.Items.Add(ni5);

            //create the database connection

            try
            {
                OleDbConnection TestConn = new OleDbConnection(cs);
                TestConn.Open();
                TestConnection.Background = new SolidColorBrush(Colors.Green);
                TestConnection.Foreground = new SolidColorBrush(Colors.White);
                TestConnection.Content = "Connection Successful!";
                OleDbCommand aCommand = new OleDbCommand(SQL, TestConn);

                //create the datareader object to connect to table
                OleDbDataReader aReader = aCommand.ExecuteReader();

                //Iterate throuth the database
                for (looper = 0; looper < aReader.FieldCount; looper++)
                {
                    fieldname = aReader.GetName(looper);
                    ComboBoxItem additem = new ComboBoxItem();
                    additem.Content = fieldname;
                    EdgeID.Items.Add(additem);
                    ComboBoxItem ni_2 = new ComboBoxItem();
                    ni_2.Content = fieldname;
                    EdgeName.Items.Add(ni_2);
                    ComboBoxItem ni_3 = new ComboBoxItem();
                    ni_3.Content = fieldname;
                    EdgeType.Items.Add(ni_3);
                    ComboBoxItem ni_4 = new ComboBoxItem();
                    ni_4.Content = fieldname;
                    FromEdge.Items.Add(ni_4);
                    ComboBoxItem ni_5 = new ComboBoxItem();
                    ni_5.Content = fieldname;
                    ToEdge.Items.Add(ni_5);
                }


            }

            //Some usual exception handling
            catch (OleDbException eFailEdgeConnect)
            {
                MessageBox.Show(eFailEdgeConnect.Message);
                TestConnection.Background = new SolidColorBrush(Colors.Red);
                TestConnection.Foreground = new SolidColorBrush(Colors.White);
                TestConnection.Content = "Connection Failed!";
            }

        }
    }
}
