﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Smrf.NodeXL.Core;

namespace GraphStorageManagement
{
    public partial class NetworkID : Form
    {
        public event EventHandler<IgraphGenerateEvent> GraphGenerated;
        public NetworkID(DB_Manager db, EventHandler<IgraphGenerateEvent> _e)
        {
            InitializeComponent();
            loadNetwork(listView1, db.list_network());
            GraphGenerated = _e;
            db.close();
        }

        private void loadNetwork(ListView view, DataTable result)
        {
            ListViewItem[] item_list = new ListViewItem[result.Rows.Count];
            for (int i=0; i< result.Rows.Count; i++)
            {
                DataRow row = result.Rows[i];
                ListViewItem newitem = new ListViewItem(row[0].ToString(), 0);
                for (int k = 1; k < result.Columns.Count; k++)
                {
                    newitem.SubItems.Add(row[k].ToString());
                }
                item_list[i] = newitem;
            }
            view.Items.AddRange(item_list);
            view.EndUpdate();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*Do Nothing*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Console.WriteLine(listView1.SelectedItems[0].Text);
                String ID = listView1.SelectedItems[0].Text;
                DB_Manager dbm = new DB_Manager("networkvis");
                this.Close();
                GraphGenerated(this, new IgraphGenerateEvent(dbm.get_network(ID)));
                
                /*TODO: Make query to generate graph*/
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            /*Do Nothing*/
        }
    }
}
