using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Facebook;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public partial class FacebookGroupDialog : FacebookGraphDataProviderDialogBase
    {
        public FacebookGroupDialog()
            :
            base(new FacebookGroupNetworkAnalyzer())
        {
            InitializeComponent();
            addAttributes();
            txtGroupNameID.Text = m_sGroup;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AssertValid();
            

        }


        //*************************************************************************
        //  Property: ToolStripStatusLabel
        //
        /// <summary>
        /// Gets the dialog's ToolStripStatusLabel control.
        /// </summary>
        ///
        /// <value>
        /// The dialog's ToolStripStatusLabel control, or null if the dialog
        /// doesn't have one.  The default is null.
        /// </value>
        ///
        /// <remarks>
        /// If the derived dialog overrides this property and returns a non-null
        /// ToolStripStatusLabel control, the control's text will automatically get
        /// updated when the HttpNetworkAnalyzer fires a ProgressChanged event.
        /// </remarks>
        //*************************************************************************

        protected override ToolStripStatusLabel
        ToolStripStatusLabel
        {
            get
            {
                AssertValid();

                return (this.slStatusLabel);
            }
        }

        //*************************************************************************
        //  Method: DoDataExchange()
        //
        /// <summary>
        /// Transfers data between the dialog's fields and its controls.
        /// </summary>
        ///
        /// <param name="bFromControls">
        /// true to transfer data from the dialog's controls to its fields, false
        /// for the other direction.
        /// </param>
        ///
        /// <returns>
        /// true if the transfer was successful.
        /// </returns>
        //*************************************************************************

        protected override Boolean
        DoDataExchange
        (
            Boolean bFromControls
        )
        {
            if (bFromControls)
            {
                // Validate the controls.

                

            
            }
            else
            {
           
            }

            return (true);
        }

        //*************************************************************************
        //  Method: StartAnalysis()
        //
        /// <summary>
        /// Starts the Flickr analysis.
        /// </summary>
        ///
        /// <remarks>
        /// It's assumed that DoDataExchange(true) was called and succeeded.
        /// </remarks>
        //*************************************************************************

        protected override void
        StartAnalysis()
        {
            AssertValid();

            m_oGraphMLXmlDocument = null;

            Debug.Assert(m_oHttpNetworkAnalyzer is
                FacebookGroupNetworkAnalyzer);            

            try
            {
                s_accessToken = o_fcbLoginDialog.LocalAccessToken;
                List<FacebookGroupNetworkAnalyzer.NetworkType> netTypes = new List<FacebookGroupNetworkAnalyzer.NetworkType>();
                if (chkUserUserComments.Checked)
                {
                    netTypes.Add(FacebookGroupNetworkAnalyzer.NetworkType.UserUserComments);
                }
                if (chkUserUserLikes.Checked)
                {
                    netTypes.Add(FacebookGroupNetworkAnalyzer.NetworkType.UserUserLikes);
                }
                if (chkUserPostLikes.Checked)
                {
                    netTypes.Add(FacebookGroupNetworkAnalyzer.NetworkType.UserPostLikes);
                }
                if (chkUserPostComments.Checked)
                {
                    netTypes.Add(FacebookGroupNetworkAnalyzer.NetworkType.UserPostComments);
                }
                if (chkPostPostLikes.Checked)
                {
                    netTypes.Add(FacebookGroupNetworkAnalyzer.NetworkType.PostPostLikes);
                }
                if (chkPostPostComments.Checked)
                {
                    netTypes.Add(FacebookGroupNetworkAnalyzer.NetworkType.PostPostComments);
                }

                ((FacebookGroupNetworkAnalyzer)m_oHttpNetworkAnalyzer).
                    GetNetworkAsync(s_accessToken, txtGroupNameID.Text,
                    netTypes, rbDownloadFromPost.Checked ? (int)nudFromPost.Value : 0,
                    rbDownloadFromPost.Checked?(int)nuToPost.Value:0,
                    UserAttributes.attributes,
                    chkStatusUpdates.Checked, chkWallPosts.Checked, false,
                    rbDateDownload.Checked ? dtStartDate.Value : dtStartDate.MinDate,
                    rbDateDownload.Checked ? dtEndDate.Value : dtStartDate.MinDate);                
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show(e.Message);            
            }
        
        
        
        }

        //*************************************************************************
        //  Method: EnableControls()
        //
        /// <summary>
        /// Enables or disables the dialog's controls.
        /// </summary>
        //*************************************************************************

        protected override void
        EnableControls()
        {
            AssertValid();

            Boolean bIsBusy = m_oHttpNetworkAnalyzer.IsBusy;

            //EnableControls(!bIsBusy, pnlUserInputs);
            //btnOK.Enabled = !bIsBusy;
            //this.UseWaitCursor = bIsBusy;
        }

        //*************************************************************************
        //  Method: OnEmptyGraph()
        //
        /// <summary>
        /// Handles the case where a graph was successfully obtained by is empty.
        /// </summary>
        //*************************************************************************

        protected override void
        OnEmptyGraph()
        {
            AssertValid();

            //this.ShowInformation("That tag has no related tags.");
            //txbTag.Focus();
        }

        //*************************************************************************
        //  Method: btnOK_Click()
        //
        /// <summary>
        /// Handles the Click event on the btnOK button.
        /// </summary>
        ///
        /// <param name="sender">
        /// Standard event argument.
        /// </param>
        ///
        /// <param name="e">
        /// Standard event argument.
        /// </param>
        //*************************************************************************

        protected void
        btnOK_Click
        (
            object sender,
            EventArgs e
        )
        {
            AssertValid();

            if (!chkUserUserComments.Checked &&
                !chkUserUserLikes.Checked &&
                !chkUserPostLikes.Checked &&
                !chkUserPostComments.Checked &&
                !chkPostPostLikes.Checked &&
                !chkPostPostComments.Checked)
            {                
                ShowWarning("At least one network type should be selected!");
                return;
            }            
            if (rbDateDownload.Checked &&
                (dtEndDate.Value.Date < dtStartDate.Value.Date))
            {                
                ShowWarning("End date value should be greater than start date value!");
                return;
            }
            dtStartDate.Value = dtStartDate.Value.Date;
            dtEndDate.Value = dtEndDate.Value.Date.AddSeconds(86399);
            OnOKClick();
        }


        //*************************************************************************
        //  Method: AssertValid()
        //
        /// <summary>
        /// Asserts if the object is in an invalid state.  Debug-only.
        /// </summary>
        //*************************************************************************

        // [Conditional("DEBUG")]

        public override void
        AssertValid()
        {
            base.AssertValid();

            //Debug.Assert(m_sTag != null);
            // m_eNetworkLevel
            // m_bIncludeSampleThumbnails
        }


        //*************************************************************************
        //  Protected constants
        //*************************************************************************

        

        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        // These are static so that the dialog's controls will retain their values
        // between dialog invocations.  Most NodeXL dialogs persist control values
        // via ApplicationSettingsBase, but this plugin does not have access to
        // that and so it resorts to static fields.

        protected static string m_sGroup = "";

        /// Network level to include.

        //protected static NetworkLevel m_eNetworkLevel = NetworkLevel.OnePointFive;

        /// true to include a sample thumbnail for each tag.

        
        public String s_accessToken;
        private FacebookLoginDialog o_fcbLoginDialog;
        private FacebookAPI fb;

        

        private void addAttributes()
        {
            int i = 0;
            dgAttributes.Rows.Add(UserAttributes.attributes.Count);
            foreach (KeyValuePair<Attribute, bool> kvp in UserAttributes.attributes)
            {
                dgAttributes.Rows[i].Cells[0].Value = kvp.Key.name;
                dgAttributes.Rows[i].Cells[1].Value = kvp.Value;
                dgAttributes.Rows[i].Cells[2].Value = kvp.Key.value;
                i++;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {            
            readAttributes();
            o_fcbLoginDialog = new FacebookLoginDialog(this, 
                                    UserAttributes.createRequiredPermissionsString(
                                                    chkStatusUpdates.Checked, chkWallPosts.Checked,true));
            o_fcbLoginDialog.LogIn();        
        }

        


        private void PrintAttributes()
        {
            string text = "";
            foreach (KeyValuePair<Attribute, bool> kvp in UserAttributes.attributes)
            {
                text += kvp.Key.name + "=" + kvp.Value.ToString() + "\n";
            }

            this.ShowInformation(text);
        }

        private void readAttributes()
        {
            foreach (DataGridViewRow row in dgAttributes.Rows)
            {
                UserAttributes.attributes[new Attribute(row.Cells[0].Value.ToString(),
                row.Cells[2].Value.ToString())] = (Boolean)row.Cells[1].Value;
            }
            //Always get the name because it is needed to build the vertex list
            UserAttributes.attributes[new Attribute("Name", "name")] = true;            
        }

        

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgAttributes.Rows)
            {
                row.Cells[1].Value = ((CheckBox)sender).Checked;
            }
        }

        private void FacebookFanPageDialog_Load(object sender, EventArgs e)
        {
            dgAttributes.Columns[1].Width =
                TextRenderer.MeasureText(dgAttributes.Columns[1].HeaderText,
                dgAttributes.Columns[1].HeaderCell.Style.Font).Width + 25;
            //Get the column header cell bounds

            Rectangle rect =
                this.dgAttributes.GetCellDisplayRectangle(1, -1, true);

            //Change the location of the CheckBox to make it stay on the header

            chkSelectAll.Location =
                new Point(rect.Location.X + rect.Width - 20,
                    rect.Location.Y + Math.Abs((rect.Height - chkSelectAll.Height) / 2));

            chkSelectAll.CheckedChanged += new EventHandler(chkSelectAll_CheckedChanged);

            //Add the CheckBox into the DataGridView

            this.dgAttributes.Controls.Add(chkSelectAll);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (o_fcbLoginDialog == null)
                o_fcbLoginDialog = new FacebookLoginDialog(this, "");

            o_fcbLoginDialog.LogOut();

        }

        private void ChkCheckedChanged(object sender, EventArgs e)
        {
            if (!btnOK.Enabled)
            {
                if (chkUserUserComments.Checked ||
                    chkUserUserLikes.Checked ||
                    chkUserPostLikes.Checked ||
                    chkUserPostComments.Checked)
                {
                    dgAttributes.Enabled = true;
                    chkSelectAll.Enabled = true;
                    chkStatusUpdates.Enabled = true;
                    chkWallPosts.Enabled = true;
                }
                else if (chkPostPostLikes.Checked ||
                        chkPostPostComments.Checked)
                {
                    dgAttributes.Enabled = false;
                    chkSelectAll.Enabled = false;
                    chkStatusUpdates.Enabled = false;
                    chkWallPosts.Enabled = false;
                }
            }            
        }       

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ShowInformation("Most of the posts made by others rather than the page owner "
                            + "contain irrelevant information to the fan page and therefore "
                            + "you may not be interested in analyzing them. However you can "
                            + "choose to analyze them or not by checking/unchecking this option.");
        }

        public void SetSelectedGroupID(string sGroupID)
        {
            txtGroupNameID.Text = sGroupID;
            this.pnResults.Visible = false;
            this.btnOK.Enabled = true;
        }        

        private void SearchAndAddToResultsPanel
        (
            string sGroupName
        )
        {
            
        }

        private JSONObject[] Search(string sGroupName)
        {
            List<JSONObject> oBothResults = new List<JSONObject>();
            JSONObject[] oArrayResults;
            oBothResults.AddRange(SearchMyGroups(sGroupName));
            oBothResults.AddRange(SearchPublic(sGroupName));
            oArrayResults = oBothResults.ToArray();

            bgLoadResults.ReportProgress(100, oArrayResults);

            return oArrayResults;
        }

        private JSONObject[] SearchPublic(string sGroupName)
        {
            JSONObject result;
            if (fb == null)
            {
                if (String.IsNullOrEmpty(s_accessToken))
                {
                    fb = new FacebookAPI();
                }
                else
                {
                    fb = new FacebookAPI(s_accessToken);
                }
            }

            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("fields", "icon,name");
            args.Add("q", sGroupName);
            args.Add("type", "group");
            args.Add("limit", "15");

            result = fb.Get("search", args);

            return result.Dictionary["data"].Array;
        }

        private JSONObject[] SearchMyGroups(string sGroupName)
        {
            s_accessToken = o_fcbLoginDialog.LocalAccessToken;
            if (fb == null)
            {
                if (String.IsNullOrEmpty(s_accessToken))
                {
                    fb = new FacebookAPI();
                }
                else
                {
                    fb = new FacebookAPI(s_accessToken);
                }
            }

            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("fields", "icon,name");

            JSONObject[] result = fb.Get("me/groups", args).Dictionary["data"].Array.Where(x => x.Dictionary["name"].String.IndexOf(sGroupName,StringComparison.OrdinalIgnoreCase)>=0).ToArray();            
            return result;
        }

        private void AddToResultsPanel
        (
            JSONObject[] oResults
        )
        {
            if (oResults != null && oResults.Length>0)
            {
                string sPageGroupID;
                string sImageURL;
                string sTitle;
                string sDescription;
                string sMembers;

                flpResults.Controls.Clear();
                foreach (JSONObject ob in oResults)
                {
                    GetKeyValues(ob, out sPageGroupID, out sImageURL, out sTitle, out sDescription, out sMembers);
                    flpResults.Controls.Add(new SearchResultsComboBox(
                        sPageGroupID, sImageURL,
                        sTitle,sDescription,
                        sMembers,this));
                }
                pnResults.Visible = true;
                piLoading.SendToBack();
                piLoading.Stop();
            }
        }

        private void GetKeyValues
        (
            JSONObject ob,
            out string sPageGroupID,
            out string sImageURL,
            out string sTitle,
            out string sDescription,
            out string sMembers
        )
        {
            sPageGroupID = "";
            sImageURL = "https://www.facebook.com/images/icons/group-types/large/default.png";
            sTitle = "";
            sDescription = "Group";
            sMembers = "";

            if (ob.Dictionary.ContainsKey("id"))
            {
                sPageGroupID = ob.Dictionary["id"].String;
            }            
            if (ob.Dictionary.ContainsKey("name"))
            {
                sTitle = ob.Dictionary["name"].String;
            }            
        }

        private void flpResults_Leave(object sender, EventArgs e)
        {
            pnResults.Visible = false;
            
        }

        private void flpResults_Enter(object sender, EventArgs e)
        {
            flpResults.Focus();
        }

        private void flpResults_MouseHover(object sender, EventArgs e)
        {
            flpResults.Focus();
        }

        private void FacebookFanPageDialog_MouseClick(object sender, MouseEventArgs e)
        {
            pnResults.Visible = false;
        }

        private void bgLoadResults_DoWork(object sender, DoWorkEventArgs e)
        {
            Search((string)e.Argument);
        }

        private void bgLoadResults_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            JSONObject[] result = (JSONObject[])e.UserState;
            AddToResultsPanel(result);
        }

        private void bgLoadResults_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void txtGroupNameID_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtGroupNameID.Text.Length >= 3)
            {
                pnResults.Visible = true;
                piLoading.Start();
                piLoading.BringToFront();
                if (!bgLoadResults.IsBusy)
                {
                    bgLoadResults.RunWorkerAsync(txtGroupNameID.Text);
                }
            }
            else
            {
                pnResults.Visible = false;
            }
        }

        private void nudFromPost_ValueChanged(object sender, EventArgs e)
        {
            nuToPost.Minimum = nudFromPost.Value;
            nuToPost.Value = nudFromPost.Value + 2;
        }        
       
    }
}
