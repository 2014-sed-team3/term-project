
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: ExcelHiddenColumns
//
/// <summary>
/// Retains a list of hidden columns.
/// </summary>
///
/// <remarks>
/// An <see cref="ExcelHiddenColumns" /> object is returned by <see
/// cref="ExcelColumnHider.ShowHiddenColumns" /> and passed to <see
/// cref="ExcelColumnHider.RestoreHiddenColumns" />.
/// </remarks>
//*****************************************************************************

public class ExcelHiddenColumns : LinkedList<String>
{
    //*************************************************************************
    //  Constructor: ExcelHiddenColumns()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ExcelHiddenColumns" />
    /// class.
    /// </summary>
    //*************************************************************************

    public ExcelHiddenColumns()
    {
        // (Do nothing.)

        AssertValid();
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
