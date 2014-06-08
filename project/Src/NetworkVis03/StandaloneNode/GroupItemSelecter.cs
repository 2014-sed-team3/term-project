using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StandaloneNode
{
    public partial class GroupItemSelecter : Form
    {
        public GroupItemSelecter()
        {
            InitializeComponent();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                //listBoxShoppingCart.Items.Add(checkedListBoxProducts.Items[e.Index]);
            }
            else if (e.NewValue == CheckState.Unchecked)
            {

            }
                //listBoxShoppingCart.Items.Remove(checkedListBoxProducts.Items[e.Index]);
        }
    }
}
