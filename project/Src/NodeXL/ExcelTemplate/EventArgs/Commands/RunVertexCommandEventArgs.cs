
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RunVertexCommandEventArgs
//
/// <summary>
/// Provides information for a vertex command that needs to be run.
/// </summary>
///
/// <remarks>
/// See <see cref="RunCommandEventArgs" /> for information about how NodeXL
/// sends commands from one UI object to another.
/// </remarks>
//*****************************************************************************

public class RunVertexCommandEventArgs : RunCommandEventArgs
{
    //*************************************************************************
    //  Constructor: RunVertexCommandEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RunVertexCommandEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="vertexRowID">
    /// Row ID of the vertex that was right-clicked in the vertex table in the
    /// vertex worksheet, or <see cref="WorksheetContextMenuManager.NoRowID" />
    /// if a vertex wasn't right-clicked.  This is a row ID that is stored in
    /// the worksheet, NOT an IEdge.ID value.
    /// </param>
    ///
    /// <param name="vertexCommand">
    /// Command to run.
    /// </param>
    //*************************************************************************

    public RunVertexCommandEventArgs
    (
        Int32 vertexRowID,
        WorksheetContextMenuManager.VertexCommand vertexCommand
    )
    {
        m_iVertexRowID = vertexRowID;
        m_eVertexCommand = vertexCommand;

        AssertValid();
    }

    //*************************************************************************
    //  Property: VertexRowID
    //
    /// <summary>
    /// Gets the row ID of the vertex that was right-clicked in the vertex
    /// table in the vertex worksheet.
    /// </summary>
    ///
    /// <value>
    /// The row ID of the right-clicked vertex, or <see
    /// cref="WorksheetContextMenuManager.NoRowID" /> if a vertex wasn't right-
    /// clicked.  This is a row ID that is stored in the worksheet, NOT an
    /// IEdge.ID value.
    /// </value>
    //*************************************************************************

    public Int32
    VertexRowID
    {
        get
        {
            AssertValid();

            return (m_iVertexRowID);
        }
    }

    //*************************************************************************
    //  Property: VertexCommand
    //
    /// <summary>
    /// Gets the command to run.
    /// </summary>
    ///
    /// <value>
    /// The command to run.
    /// </value>
    //*************************************************************************

    public WorksheetContextMenuManager.VertexCommand
    VertexCommand
    {
        get
        {
            AssertValid();

            return (m_eVertexCommand);
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

        Debug.Assert(m_iVertexRowID == WorksheetContextMenuManager.NoRowID ||
            m_iVertexRowID > 0);

        // m_eVertexCommand
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Row ID of the vertex that was right-clicked in the vertex table in the
    /// vertex worksheet, or WorksheetContextMenuManager.NoRowID if a vertex
    /// wasn't right-clicked.

    protected Int32 m_iVertexRowID;

    /// The command to run.

    protected WorksheetContextMenuManager.VertexCommand m_eVertexCommand;
}

}
