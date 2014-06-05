
using System;

namespace LayoutControls.Exceptions
{
//*****************************************************************************
//  Class: ProjectInformation
//
/// <summary>
/// Contains general information about the project.
/// </summary>
//*****************************************************************************

public static class ProjectInformation
{
    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Name of the Excel Template application's template subfolder within the
    /// deployment folder.

    public const String ExcelTemplateSubfolder = @"DeployedTemplate";

    /// Name of the Excel Template application's template.

    public const String ExcelTemplateName = "NodeXLGraph.xltx";

    /// URL or the project's home page.

    public const String HomePageUrl = "http://www.codeplex.com/NodeXL";

    /// URL of the project's discussion page.

    public const String DiscussionPageUrl =
        "http://www.codeplex.com/NodeXL/Thread/List.aspx";

    /// URL of the page from which the latest release of the application can be
    /// downloaded.

    public const String DownloadPageUrl =
        "http://www.codeplex.com/NodeXL/Release/ProjectReleases.aspx";

    /// URL of the project's registration page.

    public const String RegistrationUrl =
        "https://spreadsheets.google.com/viewform?formkey="
            + "dGo5QWQ2d1liVURfZVlIWjItZVBreUE6MQ&ifq";

    /// URL of the Social Media Research Foundation site.

    public const String SocialMediaResearchFoundationUrl =
        "http://www.smrfoundation.org";

    /// <summary>
    /// URL of the donation page for NodeXL.
    ///
    /// <remarks>
    /// As of January 2012, this is duplicated as hard-coded HTML in the
    /// ExcelTemplate's splash screen.  If the URL is changed here, it must be
    /// changed there as well.
    /// </remarks>
    ///
    /// </summary>

    public const String DonateUrl =
        "http://www.smrfoundation.org/donation-guidance-how-to-support-the-"
        + "social-media-research-foundation/";

    /// URL of the NodeXL Graph Gallery website.

    public const String NodeXLGraphGalleryUrl =
        "http://www.nodexlgraphgallery.org";
     
    /// URL of the "create account" page on the NodeXL Graph Gallery website.

    public const String NodeXLGraphGalleryCreateAccountUrl =
        "http://www.nodexlgraphgallery.org/Pages/CreateAccount.aspx";

    /// URL of the page for downloading the Exchange graph data provider.

    public const String ExchangeGraphDataProviderUrl =
        "http://exchangespigot.codeplex.com/";
     
    /// URL of the page for downloading the MediaWiki graph data provider.

    public const String MediaWikiGraphDataProviderUrl =
        "http://wikiimporter.codeplex.com/";
     
    /// URL of the page for downloading the ONA Surveys graph data provider.

    public const String OnaSurveysGraphDataProviderUrl =
        "https://www.s2.onasurveys.com/help/nodexl.php";
     
    /// URL of the page for downloading the Social Network graph data provider.

    public const String SocialNetworkGraphDataProviderUrl =
        "http://socialnetimporter.codeplex.com/";
     
    /// URL of the page for downloading the Voson graph data provider.

    public const String VosonGraphDataProviderUrl =
        "http://voson.anu.edu.au/node/13#VOSON-NodeXL";
     
    /// URL of the WCF service for exporting graphs to the NodeXL Graph Gallery
    /// website.

    public const String NodeXLGraphGalleryWcfServiceUrl =

        #if true  // Real server.

        "https://www.nodexlgraphgallery.org/WcfService/"
            + "NodeXLGraphGalleryService.svc";

        #else  // For development.

        "https://TCDesktop/NodeXLGraphGalleryService/"
            + "NodeXLGraphGalleryService.svc";

        #endif

    /// URL of the page that lists the project's team members

    public const String NodeXLTeamMembersUrl =
        "http://www.smrfoundation.org/about-us/";
}

}
