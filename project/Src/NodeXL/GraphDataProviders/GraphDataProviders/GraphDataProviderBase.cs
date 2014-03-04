﻿

using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.NodeXL.ExcelTemplatePlugIns;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: GraphDataProviderBase
//
/// <summary>
/// Base class for graph data providers.
/// </summary>
//*****************************************************************************

public abstract class GraphDataProviderBase : Object, IGraphDataProvider2
{
   //*************************************************************************
    //  Constructor: GraphDataProviderBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphDataProviderBase" />
    /// class.
    /// </summary>
    ///
    /// <param name="name">
    /// The name of the data provider.
    /// </param>
    ///
    /// <param name="description">
    /// A description of the data provider.
    /// </param>
    //*************************************************************************

    public GraphDataProviderBase
    (
        String name,
        String description
    )
    {
        m_sName = name;
        m_sDescription = description;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Name
    //
    /// <summary>
    /// Gets the name of the data provider.
    /// </summary>
    ///
    /// <value>
    /// See the <see cref="IGraphDataProvider" /> topic for details.
    /// </value>
    //*************************************************************************

    public String
    Name
    {
        get
        {
            AssertValid();

            return (m_sName);
        }
    }

    //*************************************************************************
    //  Property: Description
    //
    /// <summary>
    /// Gets a description of the data provider.
    /// </summary>
    ///
    /// <value>
    /// See the <see cref="IGraphDataProvider" /> topic for details.
    /// </value>
    //*************************************************************************

    public String
    Description
    {
        get
        {
            AssertValid();

            return (m_sDescription);
        }
    }

    //*************************************************************************
    //  Method: TryGetGraphDataAsTemporaryFile()
    //
    /// <summary>
    /// Attempts to get graph data to import into the NodeXL Excel Template.
    /// </summary>
    ///
    /// <param name="pathToTemporaryFile">
    /// If true is returned, this gets set to the full path of a temporary file
    /// created by this method.  The method's caller will read the temporary
    /// file and then delete it.
    /// </param>
    ///
    /// <returns>
    /// true if the graph data was obtained and stored in a temporary file
    /// created by this method, false if the graph data was not obtained.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryGetGraphDataAsTemporaryFile
    (
        out String pathToTemporaryFile
    )
    {
        AssertValid();

        pathToTemporaryFile = null;

        // Always start with a new dialog.  This avoids problems with reusing
        // the dialog's BackgroundWorker object, which may still be cancelling
        // an asynchronous operation.

        GraphDataProviderDialogBase oGraphDataProviderDialogBase =
            CreateDialog();

        oGraphDataProviderDialogBase.Text = "Import from " + m_sName;

        if (oGraphDataProviderDialogBase.ShowDialog() == DialogResult.OK)
        {
            pathToTemporaryFile = FileUtil.GetPathToUseForTemporaryFile();

            oGraphDataProviderDialogBase.Results.Save(pathToTemporaryFile);
            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: CreateDialog()
    //
    /// <summary>
    /// Creates a dialog for getting graph data.
    /// </summary>
    ///
    /// <returns>
    /// A dialog derived from GraphDataProviderDialogBase.
    /// </returns>
    //*************************************************************************

    protected abstract GraphDataProviderDialogBase
    CreateDialog();


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        Debug.Assert( !String.IsNullOrEmpty(m_sName) );
        Debug.Assert( !String.IsNullOrEmpty(m_sDescription) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The name of the data provider.

    protected String m_sName;

    /// A description of the data provider.

    protected String m_sDescription;
}

}
