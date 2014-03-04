
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RunEditableCommandEventArgs
//
/// <summary>
/// Base class for classes that provide information for a command that needs to
/// be run after the user optionally edits user settings for the command.
/// </summary>
///
/// <remarks>
/// See <see cref="RunCommandEventArgs" /> for information about how NodeXL
/// sends commands from one UI object to another.
/// </remarks>
//*****************************************************************************

public class RunEditableCommandEventArgs : RunCommandEventArgs
{
    //*************************************************************************
    //  Constructor: RunEditableCommandEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RunEditableCommandEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="editUserSettings">
    /// true to allow the user to edit the user settings for the command before
    /// the command is run.
    /// </param>
    //*************************************************************************

    public RunEditableCommandEventArgs
    (
        Boolean editUserSettings
    )
    {
        m_bEditUserSettings = editUserSettings;
        m_bCommandSuccessfullyRun = false;

        AssertValid();
    }

    //*************************************************************************
    //  Property: EditUserSettings
    //
    /// <summary>
    /// Gets a flag indicating whether the user should be allowed to edit the
    /// user settings for the command.
    /// </summary>
    ///
    /// <value>
    /// true to allow the user to edit the user settings for the command before
    /// the command is run.
    /// </value>
    //*************************************************************************

    public Boolean
    EditUserSettings
    {
        get
        {
            AssertValid();

            return (m_bEditUserSettings);
        }
    }

    //*************************************************************************
    //  Property: CommandSuccessfullyRun
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the command was successfully
    /// run.
    /// </summary>
    ///
    /// <value>
    /// true if the command was successfully run.
    /// </value>
    //*************************************************************************

    public Boolean
    CommandSuccessfullyRun
    {
        get
        {
            AssertValid();

            return (m_bCommandSuccessfullyRun);
        }

        set
        {
            m_bCommandSuccessfullyRun = value;

            AssertValid();
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")] 

    public override void
    AssertValid()
    {
        base.AssertValid();

        // m_eEditUserSettings
        // m_bCommandSuccessfullyRun
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// true to allow the user to edit the user settings for the command before
    /// the command is run.

    protected Boolean m_bEditUserSettings;

    /// true if the command was successfully run.

    protected Boolean m_bCommandSuccessfullyRun;
}

}
