using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GraphStorageManagement
{
    public partial class AskNetWorkID : Form
    {
        public event EventHandler<statusEventHandler> inputGet;
        public AskNetWorkID(EventHandler<statusEventHandler> _inputGet)
        {
            InitializeComponent();
            inputGet = _inputGet;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DB_Manager db = new DB_Manager("networkvis");
            DataTable dt = db.mysql_query(String.Format("SELECT DISTINCT NetworkID FROM networkvis.nodes WHERE NetworkID = '{0}'", textBox1.Text));
            
            if (dt.Rows.Count > 0)
            {
                return;
            }
            else
            {
                this.Close();
                inputGet(this, new statusEventHandler(1, textBox1.Text));
            }
        }
    }
}
