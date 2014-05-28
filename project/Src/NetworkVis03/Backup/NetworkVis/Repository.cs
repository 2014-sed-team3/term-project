using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows;

namespace NetworkVis
{
    class Repository
    {
        public OleDbConnection RepositoryConnection;

        public void Close_Repository()
        {
            try
            {
                RepositoryConnection.Close();

            }

           //Some usual exception handling
            catch (OleDbException e)
            {
                MessageBox.Show(e.Errors[0].Message);
            }
        }

        public void Open_Repository()
        {
            //create the database connection
            string DataBaseRoot = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()).Replace(@"file:\", "") ;

            RepositoryConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DataBaseRoot + "\\Associations.mdb");

            try
            {
                RepositoryConnection.Open();

            }

            //Some usual exception handling
            catch (OleDbException e)
            {
                //MessageBox.Show(e.Errors[0].Message);
            }


        }
    }
}
