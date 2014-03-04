
using System;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphTooLargeException
//
/// <summary>
/// Represents an exception thrown by <see
/// cref="NodeXLGraphGalleryExporter.ExportToNodeXLGraphGallery" /> when the
/// graph is too large to export.
/// </summary>
//*****************************************************************************

[System.SerializableAttribute()]

public class GraphTooLargeException : Exception
{
    //*************************************************************************
    //  Constructor: GraphTooLargeException()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphTooLargeException" />
    /// class.
    /// </summary>
    //*************************************************************************

    public GraphTooLargeException()
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
