using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smrf.SocialNetworkLib
{
    public enum
    NetworkType
    {
        /// <summary>
        /// Download User-User network of a Fan Page based on comments.
        /// </summary>

        UserUserComments = 0,

        /// <summary>
        /// Download User-User network of a Fan Page based on likes.
        /// </summary>

        UserUserLikes = 1,

        /// <summary>
        /// Download User-Post network of a Fan Page based on comments.
        /// </summary>

        UserPostComments = 2,

        /// <summary>
        /// Download User-Post network of a Fan Page based on likes.
        /// </summary>

        UserPostLikes = 3,

        /// <summary>
        /// Download Post-Post network of a Fan Page based on comments.
        /// </summary>

        PostPostComments = 4,

        /// <summary>
        /// Download Post-Post network of a Fan Page based on likes.
        /// </summary>

        PostPostLikes = 5,

        /// <summary>
        /// Download User-User timeline network based on user tagged.
        /// </summary>

        TimelineUserTagged = 6,

        /// <summary>
        /// Download Post-Post network of a Fan Page based on likes.
        /// </summary>

        TimelineUserComments = 7,

        /// <summary>
        /// Download Post-Post network of a Fan Page based on likes.
        /// </summary>

        TimelineUserLikes = 8,

        /// <summary>
        /// Download Post-Post network of a Fan Page based on likes.
        /// </summary>
        
        TimelinePostAuthors = 9,

    }
}
