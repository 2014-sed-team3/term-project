
using System;
using System.Windows.Forms;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExportedFilesControl
//
/// <summary>
/// Control for selecting the types of files that will be exported.
/// 
/// </summary>
///
/// <remarks>
/// This control is used by several dialogs that export an image of the graph,
/// along with optional NodeXL workbook, workbook options, and GraphML files.
///
/// <para>
/// Set the <see cref="ExportWorkbookAndSettings" />, <see
/// cref="ExportGraphML" />, and <see cref="UseFixedAspectRatio" /> properties
/// after the control is created.  To retrieve the edited properties, call <see
/// cref="Validate" />, and if <see cref="Validate" /> returns true, read the
/// properties.
/// </para>
///
/// <para>
/// This control uses the following keyboard shortcuts: W, G, K, B
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class ExportedFilesControl : UserControl
{
    //*************************************************************************
    //  Constructor: ExportedFilesControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ExportedFilesControl" />
    /// class.
    /// </summary>
    //*************************************************************************

    public ExportedFilesControl()
    {
        InitializeComponent();

        m_bExportWorkbookAndSettings = false;
        m_bExportGraphML = false;
        m_bUseFixedAspectRatio = false;
        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: ExportWorkbookAndSettings
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the NodeXL workbook and a
    /// settings file should be exported.
    /// </summary>
    ///
    /// <value>
    /// true to export the workbook and its settings.
    /// </value>
    //*************************************************************************

    public Boolean
    ExportWorkbookAndSettings
    {
        get
        {
            AssertValid();

            return (m_bExportWorkbookAndSettings);
        }

        set
        {
            m_bExportWorkbookAndSettings = value;
            DoDataExchange(false);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ExportGraphML
    //
    /// <summary>
    /// Gets or sets a flag specifying whether a GraphML file should be
    /// exported.
    /// </summary>
    ///
    /// <value>
    /// true to export the graph's data as GraphML.
    /// </value>
    //*************************************************************************

    public Boolean
    ExportGraphML
    {
        get
        {
            AssertValid();

            return (m_bExportGraphML);
        }

        set
        {
            m_bExportGraphML = value;
            DoDataExchange(false);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: UseFixedAspectRatio
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the graph image should have a
    /// fixed aspect ratio.
    /// </summary>
    ///
    /// <value>
    /// true to use a fixed aspect ratio, false to use the aspect ratio of the
    /// graph pane.
    /// </value>
    //*************************************************************************

    public Boolean
    UseFixedAspectRatio
    {
        get
        {
            AssertValid();

            return (m_bUseFixedAspectRatio);
        }

        set
        {
            m_bUseFixedAspectRatio = value;
            DoDataExchange(false);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: Validate()
    //
    /// <summary>
    /// Validates the user's settings.
    /// </summary>
    ///
    /// <returns>
    /// true if the validation was successful.
    /// </returns>
    ///
    /// <remarks>
    /// If validation fails, an error message is displayed and false is
    /// returned.
    /// </remarks>
    //*************************************************************************

    public new Boolean
    Validate()
    {
        AssertValid();

        return ( DoDataExchange(true) );
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the control's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the control's controls to its fields, false
    /// for the other direction.
    /// </param>
    ///
    /// <returns>
    /// true if the transfer was successful.
    /// </returns>
    //*************************************************************************

    protected Boolean
    DoDataExchange
    (
        Boolean bFromControls
    )
    {
        AssertValid();

        if (bFromControls)
        {
            m_bExportWorkbookAndSettings =
                chkExportWorkbookAndSettings.Checked;

            m_bExportGraphML = chkExportGraphML.Checked;
            m_bUseFixedAspectRatio = radUseFixedAspectRatio.Checked;
        }
        else
        {
            chkExportWorkbookAndSettings.Checked =
                m_bExportWorkbookAndSettings;

            chkExportGraphML.Checked = m_bExportGraphML;

            if (m_bUseFixedAspectRatio)
            {
                radUseFixedAspectRatio.Checked = true;
            }
            else
            {
                radUseGraphPaneAspectRatio.Checked = true;
            }
        }

        return (true);
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
        // m_bExportWorkbookAndSettings
        // m_bExportGraphML
        // m_bUseFixedAspectRatio
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true to export the workbook and its settings.

    protected Boolean m_bExportWorkbookAndSettings;

    /// true to export the graph's data as GraphML.

    protected Boolean m_bExportGraphML;

    /// true to use a fixed aspect ratio, false to use the aspect ratio of the
    /// graph pane.

    protected Boolean m_bUseFixedAspectRatio;
}

}
