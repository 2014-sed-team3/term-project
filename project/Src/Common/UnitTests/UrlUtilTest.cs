using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.AppLib;

namespace Smrf.Common.UnitTests
{
//*****************************************************************************
//  Class: UrlUtilTest
//
/// <summary>
/// his is a test fixture for the <see cref="UrlUtil" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class UrlUtilTest : Object
{
    //*************************************************************************
    //  Constructor: UrlUtilTest()
    //
    /// <summary>
    /// Initializes a new instance of the UrlUtilTest class.
    /// </summary>
    //*************************************************************************

    public UrlUtilTest()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: SetUp()
    //
    /// <summary>
    /// Gets run before each test.
    /// </summary>
    //*************************************************************************

    [TestInitializeAttribute]

    public void
    SetUp()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TearDown()
    //
    /// <summary>
    /// Gets run after each test.
    /// </summary>
    //*************************************************************************

    [TestCleanupAttribute]

    public void
    TearDown()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestExpandUrl()
    //
    /// <summary>
    /// Tests the ExpandUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExpandUrl()
    {
        Assert.AreEqual( "http://en.wikipedia.org/wiki/URL_shortening",
            UrlUtil.ExpandUrl("http://bit.ly/urlwiki") );
    }

    //*************************************************************************
    //  Method: TestExpandUrl2()
    //
    /// <summary>
    /// Tests the ExpandUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExpandUrl2()
    {
        Assert.AreEqual( "http://en.wikipedia.org/wiki/URL_shortening",
            UrlUtil.ExpandUrl("http://tinyurl.com/urlwiki") );
    }

    //*************************************************************************
    //  Method: TestExpandUrl3()
    //
    /// <summary>
    /// Tests the ExpandUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExpandUrl3()
    {
        Assert.AreEqual( "http://en.wikipedia.org/wiki/URL_shortening",
            UrlUtil.ExpandUrl("http://is.gd/urlwiki") );
    }

    //*************************************************************************
    //  Method: TestExpandUrl4()
    //
    /// <summary>
    /// Tests the ExpandUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExpandUrl4()
    {
        Assert.AreEqual( "http://en.wikipedia.org/wiki/URL_shortening",
            UrlUtil.ExpandUrl("http://goo.gl/Gmzqv") );
    }

    //*************************************************************************
    //  Method: TestExpandUrl5()
    //
    /// <summary>
    /// Tests the ExpandUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExpandUrl5()
    {
        // Real page.

        Assert.AreEqual( "http://www.bing.com",
            UrlUtil.ExpandUrl("http://www.bing.com") );
    }

    //*************************************************************************
    //  Method: TestExpandUrl6()
    //
    /// <summary>
    /// Tests the ExpandUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExpandUrl6()
    {
        // No such page.

        Assert.AreEqual( "http://www.ThereIsNoSuchAddressAsThis.com",
            UrlUtil.ExpandUrl("http://www.ThereIsNoSuchAddressAsThis.com") );
    }

    //*************************************************************************
    //  Method: TestExpandUrl7()
    //
    /// <summary>
    /// Tests the ExpandUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExpandUrl7()
    {
        // Real page, slow.

        Assert.AreEqual(
            "https://nodexlgraphgallery.org/Pages/Graph.aspx?graphID=718",
            UrlUtil.ExpandUrl("http://bit.ly/KFlexu") );
    }

    //*************************************************************************
    //  Method: TestExpandUrl8()
    //
    /// <summary>
    /// Tests the ExpandUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExpandUrl8()
    {
        // Repeated calls.

        for (Int32 i = 0; i < 10; i++)
        {
            Assert.AreEqual( "http://en.wikipedia.org/wiki/URL_shortening",
                UrlUtil.ExpandUrl("http://bit.ly/urlwiki") );

            Assert.AreEqual( "http://en.wikipedia.org/wiki/URL_shortening",
                UrlUtil.ExpandUrl("http://tinyurl.com/urlwiki") );

            Assert.AreEqual( "http://en.wikipedia.org/wiki/URL_shortening",
                UrlUtil.ExpandUrl("http://is.gd/urlwiki") );

            Assert.AreEqual( "http://en.wikipedia.org/wiki/URL_shortening",
                UrlUtil.ExpandUrl("http://goo.gl/Gmzqv") );

            Assert.AreEqual( "http://www.bing.com",
                UrlUtil.ExpandUrl("http://www.bing.com") );
        }
    }

    //*************************************************************************
    //  Method: TestExpandUrl9()
    //
    /// <summary>
    /// Tests the ExpandUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExpandUrl9()
    {
        // Real page, Location header is relative URL instead of the required
        // absolute URL.

        const String Url =
            "http://www.marketwatch.com/Story/story/rescue?SourceUrl=http://www.marketwatch.com/story/hispanic-national-bar-association-statement-on-state-of-the-union-address-2012-01-26";

        Assert.AreEqual( Url, UrlUtil.ExpandUrl(Url) );
    }

    //*************************************************************************
    //  Method: TestExpandUrl10()
    //
    /// <summary>
    /// Tests the ExpandUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestExpandUrl10()
    {
        // Bad URL found on Twitter on 5/28/2013.

        const String Url = "twitter.com/nsm";

        Assert.AreEqual( Url, UrlUtil.ExpandUrl(Url) );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks()
    {
        // Simple case.

        Assert.AreEqual(
        
            GetLink("http://www.bing.com"),
            UrlUtil.ReplaceUrlsWithLinks("http://www.bing.com")
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks2()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks2()
    {
        // Terminated with period.

        Assert.AreEqual(
        
            GetLink("http://www.bing.com") + ".",
            UrlUtil.ReplaceUrlsWithLinks("http://www.bing.com.")
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks3()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks3()
    {
        // Terminated with period-space.

        Assert.AreEqual(
        
            GetLink("http://www.bing.com") + ". More",
            UrlUtil.ReplaceUrlsWithLinks("http://www.bing.com. More")
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks4()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks4()
    {
        // Terminated with comma.

        Assert.AreEqual(
            GetLink("http://www.bing.com") + ",",
            UrlUtil.ReplaceUrlsWithLinks("http://www.bing.com,")
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks5()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks5()
    {
        // Terminated with comma-space.

        Assert.AreEqual(
        
            GetLink("http://www.bing.com") + ", ",
            UrlUtil.ReplaceUrlsWithLinks("http://www.bing.com, ")
            );
    }


    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks6()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks6()
    {
        // Preceding text.

        Assert.AreEqual(
        
            "This is the URL: " + GetLink("http://www.bing.com"),

            UrlUtil.ReplaceUrlsWithLinks(
                "This is the URL: http://www.bing.com")
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks7()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks7()
    {
        // Longer URL.

        const String Url = 
            "http://www.nodexlgraphgallery.org/Pages/Graph.aspx?graphID=747";

        Assert.AreEqual(
            GetLink(Url),
            UrlUtil.ReplaceUrlsWithLinks(Url)
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks8()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks8()
    {
        // Longer URL.

        const String Url = 
            "http://www.bing.com/weather/search?q=seattle%20weather&unit="
            + "F&qpvt=seattle+weather&FORM=DTPWEA";

        Assert.AreEqual(
            GetLink(Url),
            UrlUtil.ReplaceUrlsWithLinks(Url)
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks9()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks9()
    {
        // Longer URL.

        const String Url = 
            "http://www.sears.com/viper-tool-storage-26inch-5-drawer-18g-"
            + "steel/p-00935584000P?prdNo=17&blockNo=167&blockType=G167";

        Assert.AreEqual(
            GetLink(Url),
            UrlUtil.ReplaceUrlsWithLinks(Url)
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks10()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks10()
    {
        // Longer URL.

        const String Url = 
            "http://msdn.microsoft.com/en-us/library/bb149081(v=office.12)"
            + ".aspx";

        Assert.AreEqual(
            GetLink(Url),
            UrlUtil.ReplaceUrlsWithLinks(Url)
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks11()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks11()
    {
        // Simple URL.

        const String Url = "http://www.amazon.com/gp/product/B005HXMZ0S/ref=s9_simh_gw_p351_d0_g351_i1?pf_rd_m=ATVPDKIKX0DER&pf_rd_s=center-2&pf_rd_r=1KY9A98NGZAKW5Q6B3N1&pf_rd_t=101&pf_rd_p=470938631&pf_rd_i=507846";

        Assert.AreEqual(
            GetLink(Url),
            UrlUtil.ReplaceUrlsWithLinks(Url)
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks12()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks12()
    {
        // Longer URL.

        const String Url = 
            "https://mail.microsoft.com/owa/auth/logon.aspx?replaceCurrent=1&"
            + "url=https%3a%2f%2fmail.microsoft.com%2fowa%2f";

        Assert.AreEqual(
            GetLink(Url),
            UrlUtil.ReplaceUrlsWithLinks(Url)
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks13()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks13()
    {
        // Longer URL.

        const String Url = 
            "https://nes.ncdc.noaa.gov/pls/prod/f?p=100:1:3485367646946292"
            + "::::P1_ARTICLE_SEARCH:348";

        Assert.AreEqual(
            GetLink(Url),
            UrlUtil.ReplaceUrlsWithLinks(Url)
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks14()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks14()
    {
        // Several URLs.

        const String Url = 
            "URL1 http://url1.com https://url2.com  http://url3.com"
            + "\r\n"
            + "http://url4.com"
            + "\r\n"
            + " URL5 http://url5.com"
            + "\r\n"
            ;

        Assert.AreEqual(

            "URL1 " + GetLink("http://url1.com") + " "
            + GetLink("https://url2.com") + "  " + GetLink("http://url3.com")
            + "\r\n"
            + GetLink("http://url4.com")
            + "\r\n"
            + " URL5 "
            + GetLink("http://url5.com")
            + "\r\n"
            ,

            UrlUtil.ReplaceUrlsWithLinks(Url)
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks15()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks15()
    {
        // URL that starts with "www.", which should be fixed automatically.

        const String Url = 
            "This is the URL: www.yahoo.com, it is.";

        Assert.AreEqual(

            "This is the URL: "
            + "<a href=\"http://www.yahoo.com\">www.yahoo.com</a>, it is.",

            UrlUtil.ReplaceUrlsWithLinks(Url)
            );
    }

    //*************************************************************************
    //  Method: TestReplaceUrlsWithLinks16()
    //
    /// <summary>
    /// Tests the ReplaceUrlsWithLinks() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestReplaceUrlsWithLinks16()
    {
        // URL embedded in text, which should not get a link.

        const String Url = 
            "This is the URL:http://www.yahoo.com, it is.";

        Assert.AreEqual(
            Url,
            UrlUtil.ReplaceUrlsWithLinks(Url)
            );
    }

    //*************************************************************************
    //  Method: TestTryGetDomainFromUrl()
    //
    /// <summary>
    /// Tests the TryGetDomainFromUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetDomainFromUrl()
    {
        String sDomain;

        Assert.IsTrue( UrlUtil.TryGetDomainFromUrl(
            "http://bit.ly/urlwiki", out sDomain) );

        Assert.AreEqual("bit.ly", sDomain);
    }

    //*************************************************************************
    //  Method: TestTryGetDomainFromUrl2()
    //
    /// <summary>
    /// Tests the TryGetDomainFromUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetDomainFromUrl2()
    {
        String sDomain;

        Assert.IsTrue( UrlUtil.TryGetDomainFromUrl(
            "http://www.mail.yahoo.com", out sDomain) );

        Assert.AreEqual("yahoo.com", sDomain);
    }

    //*************************************************************************
    //  Method: TestTryGetDomainFromUrl3()
    //
    /// <summary>
    /// Tests the TryGetDomainFromUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetDomainFromUrl3()
    {
        String sDomain;

        Assert.IsTrue( UrlUtil.TryGetDomainFromUrl(
            "http://nodexl.codeplex.com/discussions", out sDomain) );

        Assert.AreEqual("codeplex.com", sDomain);
    }

    //*************************************************************************
    //  Method: TestTryGetDomainFromUrl4()
    //
    /// <summary>
    /// Tests the TryGetDomainFromUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetDomainFromUrl4()
    {
        String sDomain;

        Assert.IsTrue( UrlUtil.TryGetDomainFromUrl(
            "http://www.nodexlgraphgallery.org/Pages/Graph.aspx?graphID=991",
            out sDomain) );

        Assert.AreEqual("nodexlgraphgallery.org", sDomain);
    }

    //*************************************************************************
    //  Method: TestTryGetDomainFromUrl5()
    //
    /// <summary>
    /// Tests the TryGetDomainFromUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetDomainFromUrl5()
    {
        String sDomain;

        Assert.IsTrue( UrlUtil.TryGetDomainFromUrl(
            "https://91.295.394.23:8787/xy/zk", out sDomain) );

        Assert.AreEqual("91.295.394.23", sDomain);
    }

    //*************************************************************************
    //  Method: TestTryGetDomainFromUrl6()
    //
    /// <summary>
    /// Tests the TryGetDomainFromUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetDomainFromUrl6()
    {
        String sDomain;

        Assert.IsTrue( UrlUtil.TryGetDomainFromUrl(
            "http://www.amazon.com/gp/product/B0051QVESA/ref=s9_pop_gw_g349_ir03/177-5503097-9707547?pf_rd_m=ATVPDKIKX0DER&pf_rd_s=center", out sDomain) );

        Assert.AreEqual("amazon.com", sDomain);
    }

    //*************************************************************************
    //  Method: TestTryGetDomainFromUrl7()
    //
    /// <summary>
    /// Tests the TryGetDomainFromUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetDomainFromUrl7()
    {
        String sDomain;

        Assert.IsTrue( UrlUtil.TryGetDomainFromUrl(
            "http://www.ubnt.com/airmax#picostationm", out sDomain) );

        Assert.AreEqual("ubnt.com", sDomain);
    }

    //*************************************************************************
    //  Method: TestTryGetDomainFromUrl8()
    //
    /// <summary>
    /// Tests the TryGetDomainFromUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetDomainFromUrl8()
    {
        String sDomain;

        Assert.IsTrue( UrlUtil.TryGetDomainFromUrl(
            "http://a.b.c.d.e.com/xyz?a=b", out sDomain) );

        Assert.AreEqual("e.com", sDomain);
    }

    //*************************************************************************
    //  Method: TestTryGetDomainFromUrlBad()
    //
    /// <summary>
    /// Tests the TryGetDomainFromUrl() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetDomainFromUrlBad()
    {
        String sDomain;

        Assert.IsFalse( UrlUtil.TryGetDomainFromUrl(
            "abcdefg", out sDomain) );
    }

    //*************************************************************************
    //  Method: GetLink()
    //
    /// <summary>
    /// Gets a link for an URL.
    /// </summary>
    ///
    /// <param name="sUrl">
    /// The URL.
    /// </param>
    ///
    /// <returns>
    /// The link for the URL.
    /// </returns>
    //*************************************************************************

    protected String
    GetLink
    (
        String sUrl
    )
    {
        return ( String.Format(

            "<a href=\"{0}\">{0}</a>"
            ,
            sUrl
            ) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
