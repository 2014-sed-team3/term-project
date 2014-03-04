﻿
using System;
using System.IO;
using System.Windows.Forms;
using Smrf.AppLib;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Adapters;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: OpenPajekFileDialog
//
/// <summary>
/// Represents a dialog box for opening a Pajek file.
/// </summary>
///
/// <remarks>
/// Call <see cref="ShowDialogAndOpenPajekFile" /> to allow the user to open
/// a Pajek file from a location of his choice.
/// </remarks>
//*****************************************************************************

public class OpenPajekFileDialog : OpenFileDialog2
{
    //*************************************************************************
    //  Constructor: OpenPajekFileDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenPajekFileDialog" />
    /// class.
    /// </summary>
    //*************************************************************************

    public OpenPajekFileDialog()
    :
    base
    (
        String.Empty,
        String.Empty
    )
    {
        // (Do nothing else.)
    }

    //*************************************************************************
    //  Method: ShowDialogAndOpenPajekFile()
    //
    /// <summary>
    /// Opens a Pajek graph file.
    /// </summary>
    ///
    /// <param name="graph">
    /// Where a new graph gets stored.
    /// </param>
    ///
    /// <returns>
    /// DialogResult.OK if the user selected a file name and a a graph object
    /// was successfully created from the file.
    /// </returns>
    ///
    /// <remarks>
    /// This method allows the user to select a Pajek file name.  It then
    /// opens the file and creates a graph object from it.
    /// </remarks>
    //*************************************************************************

    public DialogResult
    ShowDialogAndOpenPajekFile
    (
        out IGraph graph
    )
    {
        AssertValid();

        // Let the base class do most of the work.  ShowDialogAndOpenObject()
        // calls OpenObject(), which will open the file and create a graph 
        // object from it.

        Object oObject;

        DialogResult oDialogResult = ShowDialogAndOpenObject(out oObject);

        Debug.Assert(oObject == null || oObject is IGraph);
        graph = (IGraph)oObject;

        return (oDialogResult);
    }

    //*************************************************************************
    //  Method: GetDialogTitle()
    //
    /// <summary>
    /// Gets the title to use for the dialog.
    /// </summary>
    //*************************************************************************

    protected override String
    GetDialogTitle()
    {
        AssertValid();

        return (DialogTitle);
    }

    //*************************************************************************
    //  Method: GetFilter()
    //
    /// <summary>
    /// Gets the filter to use for the dialog.
    /// </summary>
    //*************************************************************************

    protected override String
    GetFilter()
    {
        AssertValid();

        return (Filter);
    }

    //*************************************************************************
    //  Method: OpenObject()
    //
    /// <summary>
    /// Opens a graph data file and creates a graph object from it.
    /// </summary>
    ///
    /// <param name="sFileName">
    /// File name to open, including a full path.
    /// </param>
    ///
    /// <param name="oObject">
    /// Where the new graph object get stored.
    /// </param>
    ///
    /// <remarks>
    /// This is called by the base-class ShowDialogAndOpenObject() method.
    /// </remarks>
    //*************************************************************************

    protected override void
    OpenObject
    (
        String sFileName,
        out Object oObject
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sFileName) );
        Debug.Assert( File.Exists(sFileName) );
        AssertValid();

        oObject = null;

        // Use a graph adapter to create a graph from the file.

        IGraphAdapter oPajekGraphAdapter = new PajekGraphAdapter();

        oObject = oPajekGraphAdapter.LoadGraphFromFile(sFileName);
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
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Filter to use for this dialog and the SavePajekFileDialog.

    public const String Filter =
        "Pajek Files (*.net)|*.net";


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Title to use for this dialog.

    protected const String DialogTitle =
        "Import from Pajek File";
}

}
