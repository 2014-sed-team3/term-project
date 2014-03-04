

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.NodeXL.ExcelTemplatePlugIns;
using Smrf.NodeXL.GraphDataProviders.Twitter;
using Smrf.NodeXL.GraphDataProviders.Flickr;
using Smrf.NodeXL.GraphDataProviders.YouTube;
using Smrf.NodeXL.GraphDataProviders.GraphServer;

namespace Smrf.NodeXL.TestGraphDataProviders
{
/// <summary>
/// </summary>

public partial class MainForm : Form
{
    /// <summary>
    /// </summary>

    public MainForm()
    {
        InitializeComponent();
    }

    private void btnTwitterUser_Click(object sender, EventArgs e)
    {
        GetGraphData( new TwitterUserNetworkGraphDataProvider() );
    }

    private void btnTwitterSearch_Click(object sender, EventArgs e)
    {
        GetGraphData( new TwitterSearchNetworkGraphDataProvider() );
    }

    private void btnTwitterList_Click(object sender, EventArgs e)
    {
        GetGraphData( new TwitterListNetworkGraphDataProvider() );
    }

    private void btnYouTubeUsers_Click(object sender, EventArgs e)
    {
        GetGraphData( new YouTubeUserNetworkGraphDataProvider() );
    }

    private void btnYouTubeVideos_Click(object sender, EventArgs e)
    {
        GetGraphData( new YouTubeVideoNetworkGraphDataProvider() );
    }

    private void btnFlickrUsers_Click(object sender, EventArgs e)
    {
        GetGraphData( new FlickrUserNetworkGraphDataProvider() );
    }

    private void btnFlickrRelatedTags_Click(object sender, EventArgs e)
    {
        GetGraphData( new FlickrRelatedTagNetworkGraphDataProvider() );
    }

    private void btnGraphServer_Click(object sender, EventArgs e)
    {
        GetGraphData( new GraphServerNetworkGraphDataProvider() );
    }

    private void GetGraphData(IGraphDataProvider2 oGraphDataProvider)
    {
        wbWebBrowser.GoHome();

        String sPathToTemporaryFile;

        if ( !oGraphDataProvider.TryGetGraphDataAsTemporaryFile(
            out sPathToTemporaryFile) )
        {
            return;
        }

        File.Copy(sPathToTemporaryFile, TempXmlFileName, true);
        File.Delete(sPathToTemporaryFile);

        wbWebBrowser.Navigate(TempXmlFileName);
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        File.Delete(TempXmlFileName);
    }

    private String
    TempXmlFileName
    {
        get
        {
            String sAssemblyPath = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().CodeBase);

            if ( sAssemblyPath.StartsWith("file:") )
            {
                sAssemblyPath = sAssemblyPath.Substring(6);
            }

            return ( Path.Combine(sAssemblyPath, "TempGetGraphData.xml") );
        }
    }
}
}
