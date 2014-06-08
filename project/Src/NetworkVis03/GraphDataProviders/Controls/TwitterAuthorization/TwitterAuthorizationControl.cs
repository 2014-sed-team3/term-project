
using System;
using System.Windows.Forms;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphDataProviders.Twitter
{
//*****************************************************************************
//  Class: TwitterAuthorizationControl
//
/// <summary>
/// UserControl that shows a user's Twitter authorization status and provides
/// some help links concerning whitelisting.
/// </summary>
///
/// <remarks>
/// Get and set the user's Twitter authorization status with the <see
/// cref="Status" /> property.
///
/// <para>
/// This control uses the following keyboard shortcuts: I, V, W.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class TwitterAuthorizationControl : UserControl
{
    //*************************************************************************
    //  Constructor: TwitterAuthorizationControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterAuthorizationControl" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterAuthorizationControl()
    {
        InitializeComponent();

        this.Status =
            TwitterAuthorizationStatus.HasTwitterAccountNotAuthorized;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Status
    //
    /// <summary>
    /// Sets the user's Twitter authorization status.
    /// </summary>
    ///
    /// <value>
    /// The user's Twitter authorization status, as a <see
    /// cref="TwitterAuthorizationStatus" />.
    /// </value>
    //*************************************************************************

    public TwitterAuthorizationStatus
    Status
    {
        set
        {
            // Note that unless the status is HasTwitterAccountAuthorized, the
            // radHasTwitterAccountAuthorized radio button is disabled.  The
            // user can't just declare that he has authorized NodeXL to use
            // his account.  That is determined by the
            // TwitterAuthorizationManager that sets this property.

            Boolean bEnableHasTwitterAccountAuthorized = false;

            switch (value)
            {
                case TwitterAuthorizationStatus.NoTwitterAccount:

                    radNoTwitterAccount.Checked = true;
                    break;

                case TwitterAuthorizationStatus.HasTwitterAccountNotAuthorized:

                    radHasTwitterAccountNotAuthorized.Checked = true;
                    break;

                case TwitterAuthorizationStatus.HasTwitterAccountAuthorized:

                    radHasTwitterAccountAuthorized.Checked = true;
                    bEnableHasTwitterAccountAuthorized = true;
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            radHasTwitterAccountAuthorized.Enabled =
                bEnableHasTwitterAccountAuthorized;

            AssertValid();
        }

        get
        {
            AssertValid();

            if (radNoTwitterAccount.Checked)
            {
                return (TwitterAuthorizationStatus.NoTwitterAccount);
            }

            if (radHasTwitterAccountNotAuthorized.Checked)
            {
                return (
                    TwitterAuthorizationStatus.HasTwitterAccountNotAuthorized);
            }

            return (TwitterAuthorizationStatus.HasTwitterAccountAuthorized);
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
