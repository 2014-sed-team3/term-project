
using System;
using System.Net;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: UrlUtil
//
/// <summary>
/// Utility methods for working with URLs.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class UrlUtil : Object
{
    //*************************************************************************
    //  Method: EncodeUrlParameter()
    //
    /// <summary>
    /// Encodes an URL parameter by converting it to UTF-8 and then
    /// URL-encoding the UTF-8.
    /// </summary>
    ///
    /// <param name="urlParameter">
    /// The URL parameter to be encoded.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// The encoded parameter.
    /// </returns>
    //*************************************************************************

    public static String
    EncodeUrlParameter
    (
        String urlParameter
    )
    {
        Debug.Assert(urlParameter != null);

        return ( HttpUtility.UrlEncode( Encoding.UTF8.GetBytes(
            urlParameter.ToCharArray() ) ) );
    }

    //*************************************************************************
    //  Method: ExpandUrl()
    //
    /// <summary>
    /// Expands an URL that might be a shortened URL.
    /// </summary>
    ///
    /// <param name="url">
    /// The URL to expand.  Can't be null or empty.  Sample:
    /// "http://bit.ly/urlwiki".
    /// </param>
    ///
    /// <returns>
    /// The expanded URL if <paramref name="url" /> is a shortened URL, or
    /// <paramref name="url" /> if it is not.  Sample:
    /// "http://en.wikipedia.org/wiki/URL_shortening".
    /// </returns>
    ///
    /// <remarks>
    /// This method checks for only one level of indirection.  If the URL has
    /// been shortened more than once, only the first expansion is performed.
    ///
    /// <para>
    /// If the URL cannot be reached for any reason, <paramref name="url" /> is
    /// returned.  This method catches all such exceptions.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static String
    ExpandUrl
    (
        String url
    )
    {
        Debug.Assert(url != null);

        HttpWebResponse oResponse = null;

        try
        {
            HttpWebRequest oRequest =
                (HttpWebRequest)HttpWebRequest.Create(url);

            oRequest.AllowAutoRedirect = false;
            oRequest.UserAgent = "SMRF URL Expander";
            oRequest.Timeout = 1000 * ExpandUrlTimeoutSeconds;

            oResponse = (HttpWebResponse)oRequest.GetResponse();
            Int32 iStatusCode = (Int32)oResponse.StatusCode;

            if (iStatusCode >= 300 && iStatusCode <= 399)
            {
                String sLocation = oResponse.Headers["Location"];

                if ( !String.IsNullOrEmpty(sLocation) )
                {
                    return ( MakeLocationAbsolute(sLocation, url) );
                }
            }
        }
        catch (WebException)
        {
        }
        catch (UriFormatException)
        {
        }
        finally
        {
            if (oResponse != null)
            {
                oResponse.Close();
            }
        }

        return (url);
    }

    //*************************************************************************
    //  Method: ReplaceUrlsWithLinks()
    //
    /// <summary>
    /// Replaces each URL found in a string with a link.
    /// </summary>
    ///
    /// <param name="text">
    /// The string.
    /// </param>
    ///
    /// <returns>
    /// The string with replaced URLs.
    /// </returns>
    //*************************************************************************

    public static String
    ReplaceUrlsWithLinks
    (
        String text
    )
    {
        Debug.Assert(text != null);

        // Don't try to use a regular expression to match URLs.  There are many
        // such solutions on the Web, but they tend to be very complex,
        // untested, and buggy.  Some are susceptible to the denial-of-service
        // attack described at
        // http://msdn.microsoft.com/en-us/magazine/ff646973.aspx.
        //
        // Instead, use Regex to split the text into tokens on whitespace
        // boundaries, then check whether each token appears to be an URL.

        const String Pattern = @"(\s+)";
        Regex oRegex = new Regex(Pattern);
        StringBuilder oTextWithLinks = new StringBuilder();

        foreach ( String sToken in oRegex.Split(text) )
        {
            Boolean bStartsWithWww = sToken.StartsWith("www.");

            if (
                bStartsWithWww
                ||
                sToken.StartsWith("http://")
                ||
                sToken.StartsWith("https://")
                )
            {
                String sUrl = sToken;
                Int32 iUrlLength = sUrl.Length;
                Char cLastChar = sUrl[iUrlLength - 1];
                Boolean bLastCharIsPunctuation = Char.IsPunctuation(cLastChar);

                if (bLastCharIsPunctuation)
                {
                    // The URL is at the end of a sentence.  Don't include the
                    // punctuation in the link.  Instead, append it after the
                    // link.

                    sUrl = sUrl.Substring(0, iUrlLength - 1);
                }

                // If the URL starts with http or https, the link should look
                // like this:
                //
                //     <a href="http://www.site.com">http://www.site.com</a>
                //
                // If the URL starts with www, insert an "http://" in the href
                // attribute so the link looks like this:
                //
                //     <a href="http://www.site.com">www.site.com</a>
                //

                oTextWithLinks.AppendFormat(
                    "<a href=\"{0}{1}\">{1}</a>"
                    ,
                    bStartsWithWww ? "http://" : String.Empty,
                    sUrl
                    );

                if (bLastCharIsPunctuation)
                {
                    oTextWithLinks.Append(cLastChar);
                }
            }
            else
            {
                oTextWithLinks.Append(sToken);
            }
        }

        return ( oTextWithLinks.ToString() );
    }

    //*************************************************************************
    //  Method: TryGetDomainFromUrl()
    //
    /// <summary>
    /// Attempts to extract the domain from an URL, where "domain" is defined
    /// below.
    /// </summary>
    ///
    /// <param name="url">
    /// The URL.  Can't be null or empty.  Sample:
    /// "http://www.mail.yahoo.com/xyz?a=b".
    /// </param>
    ///
    /// <param name="domain">
    /// Where the domain gets stored if true is returned.  This includes the
    /// top-level domain and the first subdomain, but not any additional
    /// subdomains.  Sample: "yahoo.com".
    /// </param>
    ///
    /// <returns>
    /// true if successful, false if <paramref name="url"/> is not a valid URL.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetDomainFromUrl
    (
        String url,
        out String domain
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(url) );

        domain = null;
        Uri oUri;

        try
        {
            // Sample: http://www.mail.yahoo.com/xyz?a=b

            oUri = new Uri(url);
        }
        catch (UriFormatException)
        {
            return (false);
        }

        // Sample: www.mail.yahoo.com

        String sHost = oUri.Host;

        // There doesn't seem to be anything in the URI class to parse the
        // host, so do it by brute force.

        String[] asHostComponents = sHost.Split(new char[] {'.'},
            StringSplitOptions.RemoveEmptyEntries);

        Int32 iHostComponents = asHostComponents.Length;

        if ( iHostComponents <= 1 || HostIsIPAddress(sHost) )
        {
            domain = sHost;
        }
        else
        {
            // Sample: yahoo.com

            domain = String.Format(
                "{0}.{1}"
                ,
                asHostComponents[iHostComponents - 2],
                asHostComponents[iHostComponents - 1]
                );
        }

        return (true);
    }

    //*************************************************************************
    //  Method: EscapeSpacesInUrl()
    //
    /// <summary>
    /// Escapes any illegal spaces found in an URL.
    /// </summary>
    ///
    /// <param name="url">
    /// The URL.
    /// </param>
    ///
    /// <returns>
    /// The URL with escaped spaces.
    /// </returns>
    //*************************************************************************

    public static String
    EscapeSpacesInUrl
    (
        String url
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(url) );

        return ( url.Replace(" ", "%20") );
    }

    //*************************************************************************
    //  Method: MakeLocationAbsolute()
    //
    /// <summary>
    /// Makes the URL specified in a Location header absolute if it is
    /// relative.
    /// </summary>
    ///
    /// <param name="sLocation">
    /// The Location header in an HTTP redirect response.
    /// </param>
    ///
    /// <param name="sOriginalUrl">
    /// The original URL that resulted in the redirect response.
    /// </param>
    ///
    /// <returns>
    /// The Location if it's absolute; the Location appended to the base of the
    /// original URL if that is a legal URL; the original URL otherwise.
    /// </returns>
    ///
    /// <remarks>
    /// The Location header is supposed to be absolute, but browsers tolerate
    /// relative values.
    /// </remarks>
    //*************************************************************************

    private static String
    MakeLocationAbsolute
    (
        String sLocation,
        String sOriginalUrl
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sLocation) );
        Debug.Assert( !String.IsNullOrEmpty(sOriginalUrl) );

        String sReturnValue;

        try
        {
            Uri oLocationUri = new Uri(sLocation, UriKind.RelativeOrAbsolute);

            if (oLocationUri.IsAbsoluteUri)
            {
                sReturnValue = sLocation;
            }
            else
            {
                Uri oOriginalUri = new Uri(sOriginalUrl);

                Uri oBaseUri = new Uri(
                    oOriginalUri.GetLeftPart(UriPartial.Path) );

                Uri oNewUri = new Uri(oBaseUri, sLocation);
                sReturnValue = oNewUri.ToString();
            }
        }
        catch (UriFormatException)
        {
            sReturnValue = sOriginalUrl;
        }

        return (sReturnValue);
    }

    //*************************************************************************
    //  Method: HostIsIPAddress()
    //
    /// <summary>
    /// Determines whether the host part of an URL is a numerical IP address.
    /// </summary>
    ///
    /// <param name="host">
    /// The host part of an URL.  Can't be null or empty.  Sample:
    /// "www.mail.yahoo.com".
    /// </param>
    ///
    /// <returns>
    /// true if the host is a numerical IP address.
    /// </returns>
    //*************************************************************************

    private static Boolean
    HostIsIPAddress
    (
        String host
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(host) );

        // I doubt that this handles all IP address formats.  What is a better
        // technique?

        foreach (Char cChar in host)
        {
            if ( cChar != '.' && !Char.IsDigit(cChar) )
            {
                return (false);
            }
        }

        return (true);
    }


    //*************************************************************************
    //  Constants
    //*************************************************************************

    /// Timeout for the <see cref="ExpandUrl" /> method, in seconds.
    ///
    /// Shortened URL services such as bit.ly are generally fast, so a long
    /// timeout isn't necessary.  If ExpandUrl() is given an URL that isn't
    /// shortened and the URL's page is slow, ExpandUrl() will time out and
    /// return the URL itself.  That is the correct behavior.

    private const Int32 ExpandUrlTimeoutSeconds = 5;
}

}
