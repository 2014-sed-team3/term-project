﻿
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RunEdgeCommandEventArgs
//
/// <summary>
/// Provides information for an edge command that needs to be run.
/// </summary>
///
/// <remarks>
/// See <see cref="RunCommandEventArgs" /> for information about how NodeXL
/// sends commands from one UI object to another.
/// </remarks>
//*****************************************************************************

public class RunEdgeCommandEventArgs : RunCommandEventArgs
{
    //*************************************************************************
    //  Constructor: RunEdgeCommandEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RunEdgeCommandEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="edgeRowID">
    /// Row ID of the edge that was right-clicked in the edge table in the edge
    /// worksheet, or <see cref="WorksheetContextMenuManager.NoRowID" /> if
    /// an edge wasn't right-clicked.  This is a row ID that is stored in the
    /// worksheet, NOT an IEdge.ID value.
    /// </param>
    ///
    /// <param name="edgeCommand">
    /// Command to run.
    /// </param>
    //*************************************************************************

    public RunEdgeCommandEventArgs
    (
        Int32 edgeRowID,
        WorksheetContextMenuManager.EdgeCommand edgeCommand
    )
    {
        m_iEdgeRowID = edgeRowID;
        m_eEdgeCommand = edgeCommand;

        AssertValid();
    }

    //*************************************************************************
    //  Property: EdgeRowID
    //
    /// <summary>
    /// Gets the row ID of the edge that was right-clicked in the edge table in
    /// the edge worksheet.
    /// </summary>
    ///
    /// <value>
    /// The row ID of the right-clicked edge, or <see
    /// cref="WorksheetContextMenuManager.NoRowID" /> if an edge wasn't right-
    /// clicked.  This is a row ID that is stored in the worksheet, NOT an
    /// IEdge.ID value.
    /// </value>
    //*************************************************************************

    public Int32
    EdgeRowID
    {
        get
        {
            AssertValid();

            return (m_iEdgeRowID);
        }
    }

    //*************************************************************************
    //  Property: EdgeCommand
    //
    /// <summary>
    /// Gets the command to run.
    /// </summary>
    ///
    /// <value>
    /// The command to run.
    /// </value>
    //*************************************************************************

    public WorksheetContextMenuManager.EdgeCommand
    EdgeCommand
    {
        get
        {
            AssertValid();

            return (m_eEdgeCommand);
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

        Debug.Assert(m_iEdgeRowID == WorksheetContextMenuManager.NoRowID ||
            m_iEdgeRowID > 0);

        // m_eEdgeCommand
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Row ID of the edge that was right-clicked in the edge table in the edge
    /// worksheet, or WorksheetContextMenuManager.NoRowID if an edge wasn't
    /// right-clicked.

    protected Int32 m_iEdgeRowID;

    /// The edge command to run.

    protected WorksheetContextMenuManager.EdgeCommand m_eEdgeCommand;
}
}
