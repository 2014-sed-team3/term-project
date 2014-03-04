
using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: StandardSettingsFileLockedException
//
/// <summary>
/// Represents an exception thrown by <see
/// cref="NodeXLApplicationSettingsBase" /> when the standard settings file is
/// locked.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class StandardSettingsFileLockedException : Exception
{
    //*************************************************************************
    //  Constructor: StandardSettingsFileLockedException()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="StandardSettingsFileLockedException" /> class.
    /// </summary>
    //*************************************************************************

    public StandardSettingsFileLockedException()
    {
        // (Do nothing.)
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
