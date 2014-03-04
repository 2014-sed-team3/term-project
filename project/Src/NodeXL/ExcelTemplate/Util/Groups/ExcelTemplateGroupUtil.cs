
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExcelTemplateGroupUtil
//
/// <summary>
/// Utility methods for dealing with vertex groups in the ExcelTemplate
/// project.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class ExcelTemplateGroupUtil
{
    //*************************************************************************
    //  Method: GetTopGroups()
    //
    /// <summary>
    /// Gets the graph's top groups, ranked by vertex count.
    /// </summary>
    ///
    /// <param name="groups">
    /// Information about the graph's groups.
    /// </param>
    ///
    /// <param name="maximumGroups">
    /// Maximum number of groups to get, or Int32.MaxValue for all groups.
    /// </param>
    ///
    /// <returns>
    /// A collection of the top groups in <paramref name="groups" />.
    /// </returns>
    //*************************************************************************

    public static IEnumerable<ExcelTemplateGroupInfo>
    GetTopGroups
    (
        GroupInfo [] groups,
        Int32 maximumGroups
    )
    {
        Debug.Assert(groups != null);
        Debug.Assert(maximumGroups > 0);

        return ( 
            (from oGroupInfo in groups
            orderby oGroupInfo.Vertices.Count descending
            select (ExcelTemplateGroupInfo)oGroupInfo).Take(maximumGroups)
            );
    }
}

}
