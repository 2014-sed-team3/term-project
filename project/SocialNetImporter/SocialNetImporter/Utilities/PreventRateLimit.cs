using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smrf.SocialNetworkLib
{
    public enum
    PreventRateLimit
    {
        /// <summary>
        /// Logout/Login from Facebook to get another access_token
        /// and continue to query Facebook
        /// </summary>
        LogoutLogin,
        /// <summary>
        /// Make a single call per second to avoid hitting the rate limit
        /// </summary>
        OneCallPerSecond
    }
}
