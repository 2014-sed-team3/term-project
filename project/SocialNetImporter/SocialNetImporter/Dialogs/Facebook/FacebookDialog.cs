using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Smrf.SocialNetworkLib;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public partial class FacebookDialog : FacebookGraphDataProviderDialogBase
    {
        public FacebookDialog()
            :
            base(new FacebookUserNetworkAnalyzer())
        {
            InitializeComponent();
            addAttributes();
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

            try
            {
                s_accessToken = o_fcbLoginDialog.LocalAccessToken;

                List<NetworkType> oEdgeType = new List<NetworkType>();

                if (chkUsername.Checked)
                {
                    oEdgeType.Add(NetworkType.TimelineUserTagged);
                }
                if (chkComment.Checked)
                {
                    oEdgeType.Add(NetworkType.TimelineUserComments);
                }
                if (chkLike.Checked)
                {
                    oEdgeType.Add(NetworkType.TimelineUserLikes);
                }
                if (chkAuthor.Checked)
                {
                    oEdgeType.Add(NetworkType.TimelinePostAuthors);
                }

                ((FacebookUserNetworkAnalyzer)m_oHttpNetworkAnalyzer).
                GetNetworkAsync(s_accessToken, oEdgeType, rbFromPost.Checked,
                                 rbBetween.Checked, chkMyTimeline.Checked, chkMyFriendTimeline.Checked,
                                 (int)nuFromPost.Value, (int)nuToPost.Value, dtStartDate.Value,
                                 dtEndDate.Value, chkLimit.Checked, (int)nuLimit.Value,
                                 chkTooltips.Checked, chkIncludeMe.Checked, attributes);
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

        /// Flickr Web page for requesting an API key.

        protected const String RequestFlickrApiKeyUrl =
            "http://www.flickr.com/services/api/misc.api_keys.html";


        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        // These are static so that the dialog's controls will retain their values
        // between dialog invocations.  Most NodeXL dialogs persist control values
        // via ApplicationSettingsBase, but this plugin does not have access to
        // that and so it resorts to static fields.

        /// Tag to get the related tags for.  Can be empty but not null.

        protected static String m_sTag = "sociology";

        /// Network level to include.

        //protected static NetworkLevel m_eNetworkLevel = NetworkLevel.OnePointFive;

        /// true to include a sample thumbnail for each tag.

        protected static Boolean m_bIncludeSampleThumbnails = false;

        public String s_accessToken;
        
        private FacebookLoginDialog o_fcbLoginDialog;

        //private Dictionary<Attribute, bool> attributes = new Dictionary<Attribute,bool>() 
        //{
        //    {new Attribute("Name","name"),true},
        //    {new Attribute("First Name","first_name"),true},
        //    {new Attribute("Middle Name","middle_name"),true},
        //    {new Attribute("Last Name","last_name"),true},
        //    {new Attribute("Hometown","hometown_location"),true},
        //    {new Attribute("Current Location","current_location"),true},
        //    {new Attribute("Birthday","birthday"),true},
        //    {new Attribute("Picture","pic_small"),true},
        //    {new Attribute("Profile Update Time","profile_update_time"),false},
        //    {new Attribute("Timezone","timezone"),true},
        //    {new Attribute("Religion","religion"),false},
        //    {new Attribute("Sex","sex"),true},
        //    {new Attribute("Relationship","relationship_status"),true},
        //    {new Attribute("Political Views","political"),false},
        //    {new Attribute("Activities","activities"),false},
        //    {new Attribute("Interests","interests"),false},
        //    {new Attribute("Music","music"),false},
        //    {new Attribute("TV","tv"),false},
        //    {new Attribute("Movies","movies"),false},
        //    {new Attribute("Books","books"),false},
        //    {new Attribute("Quotes","quotes"),false},
        //    {new Attribute("About Me","about_me"),true},            
        //    {new Attribute("Online Presence","online_presence"),true},
        //    {new Attribute("Locale","locale"),true},
        //    {new Attribute("Website","website"),false}                      
        //};

        private AttributesDictionary<bool> attributes = new AttributesDictionary<bool>()
        {
            {true},
            {true},
            {true},
            {true},
            {true},
            {true},
            {true},
            {true},
            {false},
            {true},
            {false},
            {true},
            {true},
            {false},
            {false},
            {false},
            {false},
            {false},
            {false},
            {false},
            {false},
            {true},            
            {true},
            {true},
            {false}        
        };

        private Dictionary<string, string> attributeFriendsPermissionMapping = new Dictionary<string, string>()
        {
            {"birthday", "friends_birthday"},
            {"hometown_location","friends_hometown"},
            {"current_location","friends_location"},
            {"religion", "friends_religion_politics"},
            {"relationship_status", "friends_relationships"},
            {"political","friends_religion_politics"},
            {"activities","friends_activities"},
            {"interests","friends_interests"},
            {"music","friends_likes"},
            {"tv","friends_likes"},
            {"movies","friends_likes"},
            {"books","friends_likes"},
            {"quotes","friends_about_me"},
            {"about_me","friends_about_me"},
            {"status","friends_status"},
            {"online_presence","friends_online_presence"},
            {"website","friends_website"},            
        };

        private Dictionary<string, string> attributeUserPermissionMapping = new Dictionary<string, string>()
        {
            {"birthday", "user_birthday"},
            {"hometown_location","user_hometown"},
            {"current_location","user_location"},
            {"religion", "user_religion_politics"},
            {"relationship_status", "user_relationships"},
            {"political","user_religion_politics"},
            {"activities","user_activities"},
            {"interests","user_interests"},
            {"music","user_likes"},
            {"tv","user_likes"},
            {"movies","user_likes"},
            {"books","user_likes"},
            {"quotes","user_about_me"},
            {"about_me","user_about_me"},
            {"status","user_status"},
            {"online_presence","user_online_presence"},
            {"website","user_website"},            
        };

        private void addAttributes()
        {
            int i = 0;
            dgAttributes.Rows.Add(attributes.Count);            
            foreach (KeyValuePair<AttributeUtils.Attribute, bool> kvp in attributes)
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
            o_fcbLoginDialog = new FacebookLoginDialog(this,createRequiredPermissionsString());
            o_fcbLoginDialog.LogIn();
        
        }        
        

        private void PrintAttributes()
        {
            string text = "";
            foreach (KeyValuePair<AttributeUtils.Attribute, bool> kvp in attributes)
            {
                text += kvp.Key.name + "=" + kvp.Value.ToString() + "\n";
            }

            this.ShowInformation(text);            
        }

        private void readAttributes()
        {
            foreach (DataGridViewRow row in dgAttributes.Rows)
            {
                attributes[row.Cells[2].Value.ToString()] = (Boolean)row.Cells[1].Value;
            }            
        }

        private string createRequiredPermissionsString()
        {
            string permissionsString = "&scope=";

            if (chkMyTimeline.Checked)
            {
                foreach (KeyValuePair<AttributeUtils.Attribute, bool> kvp in attributes)
                {
                    if (kvp.Value && attributeFriendsPermissionMapping.ContainsKey(kvp.Key.value)
                        && attributeUserPermissionMapping.ContainsKey(kvp.Key.value))
                    {
                        permissionsString += attributeFriendsPermissionMapping[kvp.Key.value] + ","
                                            + attributeUserPermissionMapping[kvp.Key.value] + ",";
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<AttributeUtils.Attribute, bool> kvp in attributes)
                {
                    if (kvp.Value && attributeFriendsPermissionMapping.ContainsKey(kvp.Key.value))
                    {
                        permissionsString += attributeFriendsPermissionMapping[kvp.Key.value] + ",";                                            
                    }
                }
            }

            permissionsString += "read_stream,";

            return permissionsString.Remove(permissionsString.Length - 1);            
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgAttributes.Rows)
            {
                row.Cells[1].Value = ((CheckBox)sender).Checked;
            }
        }

        private void FacebookDialog_Load(object sender, EventArgs e)
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
                    rect.Location.Y + Math.Abs((rect.Height - chkSelectAll.Height)/2));

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

        public FacebookLoginDialog FacebookLoginDialog
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public FacebookUserNetworkAnalyzer sdcsd
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        private void chkSelectAllEdges_CheckedChanged(object sender, EventArgs e)
        {
            chkAuthor.Checked = chkSelectAllEdges.Checked;
            chkUsername.Checked = chkSelectAllEdges.Checked;
            chkComment.Checked = chkSelectAllEdges.Checked;
            chkLike.Checked = chkSelectAllEdges.Checked;
        }

        private void nuFromPost_ValueChanged(object sender, EventArgs e)
        {
            nuToPost.Minimum = nuFromPost.Value;
            nuToPost.Value = nuFromPost.Value + 2;
        }
    
    }
}
