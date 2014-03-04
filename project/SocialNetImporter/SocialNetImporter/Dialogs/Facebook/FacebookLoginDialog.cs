using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{    
    public partial class FacebookLoginDialog : Form
    {
        private String localAccessToken;        
        private FacebookGraphDataProviderDialogBase fcbDialog;
        private string permissionsString;

        public FacebookLoginDialog(FacebookGraphDataProviderDialogBase fcbDialog, string permissionsString)
        {
            InitializeComponent();
            this.fcbDialog = fcbDialog;
            this.permissionsString = permissionsString;
        }             

        public void LogIn()
        {
            webBrowser1.Navigate("https://graph.facebook.com/oauth/authorize?client_id=134453573297344&" +
                                    "redirect_uri=https://www.facebook.com/connect/login_success.html&type=user_agent" +
                                    permissionsString);
            this.ShowDialog();
        }

        public void LogOut()
        {
            webBrowser1.Navigate("http://www.facebook.com/");            
            this.ShowDialog();
        }        

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {           
              
        }

        private void deleteCookies()
        {
            DirectoryInfo folder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
            //Get the list file in temporary
            FileInfo[] files = folder.GetFiles();           
            foreach (FileInfo file in files)
            {                
                try
                {
                    System.IO.File.Delete(file.FullName);
                }
                catch (System.Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }            
            
        }

        private void DisableComponents(FacebookGraphDataProviderDialogBase fcbDialog)
        {
            if (fcbDialog is FacebookDialog)
            {
                FacebookDialog CastedFcbDialog = (FacebookDialog)fcbDialog;
                CastedFcbDialog.btnOK.Enabled = true;
                CastedFcbDialog.dgAttributes.Enabled = false;                
                CastedFcbDialog.chkSelectAll.Enabled = false;
            }
            else if (fcbDialog is FacebookFanPageDialog)
            {
                FacebookFanPageDialog CastedFcbDialog = (FacebookFanPageDialog)fcbDialog;
                CastedFcbDialog.btnOK.Enabled = true;
                CastedFcbDialog.txtPageUsernameID.Enabled = true;
                CastedFcbDialog.dgAttributes.Enabled = false;
                CastedFcbDialog.chkSelectAll.Enabled = false;
                //CastedFcbDialog.chkCoCommenters.Enabled = false;
                //CastedFcbDialog.chkCoLikers.Enabled = false;
                //CastedFcbDialog.nudFirstPosts.Enabled = false;
                CastedFcbDialog.chkStatusUpdates.Enabled = false;
                CastedFcbDialog.chkWallPosts.Enabled = false;
                //CastedFcbDialog.rbDownloadFirstPosts.Enabled = false;
                //CastedFcbDialog.chkUserPost.Enabled = false;
                //CastedFcbDialog.chkPostPost.Enabled = false;
                //CastedFcbDialog.rbDateDownload.Enabled = false;
                //CastedFcbDialog.dtPosts.Enabled = false;
            }
            else if (fcbDialog is FacebookGroupDialog)
            {
                FacebookGroupDialog CastedFcbDialog = (FacebookGroupDialog)fcbDialog;                                
                CastedFcbDialog.dgAttributes.Enabled = false;
                CastedFcbDialog.chkSelectAll.Enabled = false;                
                CastedFcbDialog.chkStatusUpdates.Enabled = false;
                CastedFcbDialog.chkWallPosts.Enabled = false;
                CastedFcbDialog.txtGroupNameID.Enabled = true;
            }
        }

        public String LocalAccessToken
        {
            get { return this.localAccessToken; }
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            //e.Cancel = true;
            //webBrowser1.Navigate(((WebBrowser)sender).Url);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            String stringUrl = webBrowser1.Url.ToString();            
            if (stringUrl.StartsWith("https://www.facebook.com/connect/login_success.html"))
            {                
                int index = stringUrl.IndexOf("=");
                int index2 = stringUrl.IndexOf("&");
                localAccessToken = stringUrl.Substring(index + 1, index2 - index - 1);
                if (fcbDialog != null)
                {
                    DisableComponents(fcbDialog);
                    this.Close();
                }
            }          
        }

        
    }
}
