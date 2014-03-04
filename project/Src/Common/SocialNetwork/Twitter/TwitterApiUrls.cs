
using System;

namespace Smrf.SocialNetworkLib.Twitter
{
//*****************************************************************************
//  Class: TwitterApiUrls
//
/// <summary>
/// Provides URLs for the Twitter API.
/// </summary>
//*****************************************************************************

public static class TwitterApiUrls
{
    /// <summary>
    /// REST API.
    /// </summary>

    public static readonly String Rest = "https://api.twitter.com/1.1/";

    /// <summary>
    /// Search API.
    /// </summary>

    public static readonly String Search =
        "https://api.twitter.com/1.1/search/tweets.json";

    /// <summary>
    /// OAuth API.
    /// </summary>

    public const string OAuth = "https://api.twitter.com/oauth/";

    /// <summary>
    /// Format pattern for the URL of the Web page for a Twitter user.  The {0}
    /// argument must be replaced with a Twitter screen name.
    /// </summary>

    public static readonly String UserWebPageUrlPattern =
        "https://twitter.com/{0}";

    /// <summary>
    /// Format pattern for the URL of the Web page for a Twitter status.  The
    /// {0} argument must be replaced with a Twitter screen name and the {1}
    /// argument must be replaced with a status ID.
    /// </summary>

    public static readonly String StatusWebPageUrlPattern =
        "https://twitter.com/#!/{0}/status/{1}";
}

}
