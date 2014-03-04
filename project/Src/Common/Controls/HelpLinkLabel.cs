
using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: HelpLinkLabel
//
/// <summary>
/// Represents a LinkLabel that displays its Tag text in a message box when
/// clicked.
/// </summary>
///
/// <remarks>
/// Clicking the LinkLabel opens an "information" message box that shows the
/// text stored in the LinkLabel's Tag property.  If the Tag property is not
/// set to a String, the click does nothing.
/// </remarks>
//*****************************************************************************

public class HelpLinkLabel : LinkLabel
{
    //*************************************************************************
    //  Constructor: HelpLinkLabel()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="HelpLinkLabel" /> class.
    /// </summary>
    //*************************************************************************

    public HelpLinkLabel()
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
            FormUtil.ShowInformation( (String)Tag );
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
