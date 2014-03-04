
using System;
using System.Web.UI;

namespace Smrf.WebAppLib
{
//*****************************************************************************
//  Class: ControlUtil
//
/// <summary>
/// Utility methods for working with ASP.NET controls.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class ControlUtil
{
    //*************************************************************************
    //  Method: FindRequiredControlRecursive()
    //
    /// <summary>
    /// Recursively searches for a required control within a parent control.
    /// </summary>
    ///
    /// <param name="parentControl">
    /// Parent control to recursively search.
    /// </param>
    ///
    /// <param name="id">
    /// ID of the control to search for.
    /// </param>
    ///
    /// <returns>
    /// The found control.  Never null.
    /// </returns>
    ///
    /// <remarks>
    /// If the control isn't found, an exception is thrown.
    /// </remarks>
    //*************************************************************************

    public static Control
    FindRequiredControlRecursive
    (
        Control parentControl,
        String id
    )
    { 
        Control oFoundControl = FindControlRecursive(parentControl, id);

        if (oFoundControl == null)
        {
            throw new ArgumentException("Control not found: " + id);
        }

        return (oFoundControl);
    } 

    //*************************************************************************
    //  Method: FindControlRecursive()
    //
    /// <summary>
    /// Recursively searches for a specified control within a parent control.
    /// </summary>
    ///
    /// <param name="parentControl">
    /// Parent control to recursively search.
    /// </param>
    ///
    /// <param name="id">
    /// ID of the control to search for.
    /// </param>
    ///
    /// <returns>
    /// The found control, or null if not found.
    /// </returns>
    //*************************************************************************

    public static Control
    FindControlRecursive
    (
        Control parentControl,
        String id
    )
    { 
        foreach (Control oChildControl in parentControl.Controls)
        {
            if (oChildControl.ID == id)
            {
                return (oChildControl);
            }

            Control oFoundControl = FindControlRecursive(oChildControl, id);

            if (oFoundControl != null)
            {
                return (oFoundControl);
            }
        }

        return (null);
    } 
}

}
