using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public partial class SearchResultsComboBox : UserControl
    {
        private const string sLikeText = " like this";
        private const string sTalkingText = " talking about this";
        private const string sMembersText = " members";
        private string m_sResultID;
        private GraphDataProviderDialogBase m_oFBFanPageGroupDialog;

        public SearchResultsComboBox
        (
            string sPageGroupID,
            string sImageURL,
            string sTitle,
            string sDescription,
            string sLikes, 
            string sTalking,
            GraphDataProviderDialogBase oFBFanPageDialog
        )
        {
            InitializeComponent();

            pbProfilePicture.LoadAsync(sImageURL);
            lblTitle.Text = sTitle;
            lblDescription.Text = sDescription;
            lblLikes.Text = sLikes + sLikeText;
            lblTalking.Text = sTalking + sTalkingText;
            lblMembers.Text = "";
            m_sResultID = sPageGroupID;
            m_oFBFanPageGroupDialog = oFBFanPageDialog;
        }

        public SearchResultsComboBox
        (
            string sPageGroupID,
            string sImageURL,
            string sTitle,
            string sDescription,
            string sMembers,
            GraphDataProviderDialogBase oFBGroupDialog
        )
        {            
            InitializeComponent();

            pbProfilePicture.LoadAsync(sImageURL);
            lblTitle.Text = sTitle;
            lblDescription.Text = sDescription;
            lblLikes.Text = "";
            lblTalking.Text = "";
            lblMembers.Text = "";
            m_sResultID = sPageGroupID;
            m_oFBFanPageGroupDialog = oFBGroupDialog;
        }

        public string ResultID
        {
            get { return m_sResultID; }
        }        

        private void SearchResultsComboBox_MouseLeave(object sender, EventArgs e)
        {
            if (!this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
            {
                this.BackColor = Color.White;
            }
        }

        private void SearchResultsComboBox_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.SteelBlue;
            Parent.Focus();
        }

        private void pbProfilePicture_Click(object sender, EventArgs e)
        {
            if (m_oFBFanPageGroupDialog is FacebookFanPageDialog)
            {
                ((FacebookFanPageDialog)m_oFBFanPageGroupDialog).SetSelectedPageID(m_sResultID);
            }
            else if (m_oFBFanPageGroupDialog is FacebookGroupDialog)
            {
                ((FacebookGroupDialog)m_oFBFanPageGroupDialog).SetSelectedGroupID(m_sResultID);
            }
        }
    }
}
