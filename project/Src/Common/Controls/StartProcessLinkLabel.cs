
using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: StartProcessLinkLabel
//
/// <summary>
/// Represents a LinkLabel that starts a specified process when clicked.
/// </summary>
///
/// <remarks>
/// Clicking a link in the LinkLabel causes <see
/// cref="Process.Start(String)" /> to be called with the text stored in the
/// LinkLabel's Tag property.  If the Tag is not set to a String, the click
/// does nothing.
/// </remarks>
//*****************************************************************************

public class StartProcessLinkLabel : LinkLabel
{
    //*************************************************************************
    //  Constructor: StartProcessLinkLabel()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="StartProcessLinkLabel" />
    /// class.
    /// </summary>
    //*************************************************************************

    public StartProcessLinkLabel()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: OnLinkClicked()
    //
    /// <summary>
    /// Handles the LinkClicked event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnLinkClicked
    (
        LinkLabelLinkClickedEventArgs e
    )
    {
        AssertValid();

        base.OnLinkClicked(e);

        if ( Tag is String && !String.IsNullOrEmpty( (String)Tag ) )
        {
            Process.Start( (String)Tag );
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
