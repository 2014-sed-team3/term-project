

using System;
using System.Configuration;
using System.Windows.Forms;
using System.ComponentModel;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ReadabilityMetricsDialog
//
/// <summary>
/// Calculates the readability metrics specified by the user.
/// </summary>
///
/// <remarks>
/// This is a modeless dialog that should be opened with <see
/// cref="Form.Show(IWin32Window)" />.  It handles all the details involved in
/// editing the user's selected readability metrics, calculating them on
/// demand, and recalculating them when vertices are moved or the graph is laid
/// out again.
/// </remarks>
//*****************************************************************************

public partial class ReadabilityMetricsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ReadabilityMetricsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ReadabilityMetricsDialog" /> class.
    /// </summary>
    ///
    /// <param name="nodeXLControl">
    /// NodeXLControl containing the graph.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    public ReadabilityMetricsDialog
    (
        NodeXLControl nodeXLControl,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        InitializeComponent();

        m_oNodeXLControl = nodeXLControl;
        m_oWorkbook = workbook;
        m_oReadabilityMetricUserSettings = new ReadabilityMetricUserSettings();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oReadabilityMetricsDialogUserSettings =
            new ReadabilityMetricsDialogUserSettings(this);

        DoDataExchange(false);

        m_oNodeXLControl.VerticesMoved +=
            new VerticesMovedEventHandler(this.OnVerticesMoved);

        m_oNodeXLControl.GraphLaidOut +=
            new AsyncCompletedEventHandler(this.OnGraphLaidOut);

        AssertValid();
    }

    //*************************************************************************
    //  Method: DoDataExchange()
    //
    /// <summary>
    /// Transfers data between the dialog's fields and its controls.
    /// </summary>
    ///
    /// <param name="bFromControls">
    /// true to transfer data from the dialog's controls to its fields, false
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
            ReadabilityMetrics eReadabilityMetricsToCalculate =
                ReadabilityMetrics.None;

            // An ReadabilityMetrics flag is stored in the Tag of each CheckBox.

            foreach ( CheckBox oCheckBox in GetReadabilityMetricCheckBoxes() )
            {
                if (oCheckBox.Checked)
                {
                    eReadabilityMetricsToCalculate |=
                        (ReadabilityMetrics)oCheckBox.Tag;
                }
            }

            m_oReadabilityMetricUserSettings.ReadabilityMetricsToCalculate =
                eReadabilityMetricsToCalculate;
        }
        else
        {
            foreach ( CheckBox oCheckBox in GetReadabilityMetricCheckBoxes() )
            {
                oCheckBox.Checked = m_oReadabilityMetricUserSettings.
                    ShouldCalculateReadabilityMetrics(
                        (ReadabilityMetrics)oCheckBox.Tag);
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: CalculateReadabilityMetrics()
    //
    /// <summary>
    /// Calculates the selected readability metrics.
    /// </summary>
    ///
    /// <param name="bEmptySelectionIsError">
    /// If true and no metrics are selected, an error message is shown before
    /// false is returned.
    /// </param>
    ///
    /// <remarks>
    /// true if the metrics were calculated.
    /// </remarks>
    //*************************************************************************

    protected Boolean
    CalculateReadabilityMetrics
    (
        Boolean bEmptySelectionIsError
    )
    {
        AssertValid();

        if ( !DoDataExchange(true) )
        {
            return (false);
        }

        if (m_oReadabilityMetricUserSettings.ReadabilityMetricsToCalculate ==
            ReadabilityMetrics.None)
        {
            if (bEmptySelectionIsError)
            {
                this.ShowInformation("No metrics have been selected.");
            }

            return (false);
        }

        // ReadabilityMetricCalcuator2 reads the ReadabilityMetricUserSettings
        // settings, so save them.

        m_oReadabilityMetricUserSettings.Save();

        CalculateGraphMetricsDialog oCalculateGraphMetricsDialog =
            new CalculateGraphMetricsDialog(
                m_oNodeXLControl.Graph, m_oWorkbook,

                new IGraphMetricCalculator2 [] {
                    new ReadabilityMetricCalculator2( new System.Windows.Size(
                        m_oNodeXLControl.ActualWidth,
                        m_oNodeXLControl.ActualHeight) )
                    },

                "Calculating Readability Metrics",
                false
                );

        oCalculateGraphMetricsDialog.ShowDialog();

        return (true);
    }

    //*************************************************************************
    //  Method: RecalculateReadabilityMetrics()
    //
    /// <summary>
    /// Recalculates the selected readability metrics if appropriate.
    /// </summary>
    //*************************************************************************

    protected void
    RecalculateReadabilityMetrics()
    {
        AssertValid();

        if (chkRecalculate.Enabled && chkRecalculate.Checked)
        {
            CalculateReadabilityMetrics(false);
        }
    }

    //*************************************************************************
    //  Method: GetReadabilityMetricCheckBoxes()
    //
    /// <summary>
    /// Gets an array of CheckBox controls that represent readability metrics.
    /// </summary>
    ///
    /// <returns>
    /// An array of CheckBox controls.
    /// </returns>
    //*************************************************************************

    protected CheckBox[]
    GetReadabilityMetricCheckBoxes()
    {
        AssertValid();

        return ( new CheckBox[] {
            chkPerEdgeEdgeCrossings,
            chkPerVertexEdgeCrossings,
            chkPerVertexVertexOverlap,
            chkOverallEdgeCrossings,
            chkOverallVertexOverlap,
            chkGraphRectangleCoverage,
            } );
    }

    //*************************************************************************
    //  Method: OnCheckOrUncheckAll()
    //
    /// <summary>
    /// Handles the Click event on the btnCheckAll and btnUncheckAll buttons.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    OnCheckOrUncheckAll
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        CheckCheckBoxes( sender == btnCheckAll,
            GetReadabilityMetricCheckBoxes() );
    }

    //*************************************************************************
    //  Method: OnVerticesMoved()
    //
    /// <summary>
    /// Handles the VerticesMoved event on the NodeXLControl.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected void
    OnVerticesMoved
    (
        object sender,
        VerticesMovedEventArgs e
    )
    {
        AssertValid();

        RecalculateReadabilityMetrics();
    }

    //*************************************************************************
    //  Method: OnGraphLaidOut()
    //
    /// <summary>
    /// Handles the GraphLaidOut event on the NodeXLControl.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    OnGraphLaidOut
    (
        object sender,
        AsyncCompletedEventArgs e
    )
    {
        AssertValid();

        RecalculateReadabilityMetrics();
    }

    //*************************************************************************
    //  Method: btnCalculateNow_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnCalculateNow button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnCalculateNow_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if ( CalculateReadabilityMetrics(true) )
        {
            // Recalculations aren't enabled until the first time the complete
            // readability metrics are calculated.

            chkRecalculate.Enabled = true;
        }
    }

    //*************************************************************************
    //  Method: btnClose_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnClose button.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    btnClose_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if ( DoDataExchange(true) )
        {
            this.Close();
        }
    }

    //*************************************************************************
    //  Method: ReadabilityMetricsDialog_FormClosing()
    //
    /// <summary>
    /// Handles the FormClosing event.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    private void
    ReadabilityMetricsDialog_FormClosing
    (
        object sender,
        FormClosingEventArgs e
    )
    {
        AssertValid();

        m_oNodeXLControl.VerticesMoved -=
            new VerticesMovedEventHandler(this.OnVerticesMoved);

        m_oNodeXLControl.GraphLaidOut -=
            new AsyncCompletedEventHandler(this.OnGraphLaidOut);

        m_oReadabilityMetricUserSettings.Save();
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

        Debug.Assert(m_oNodeXLControl != null);
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oReadabilityMetricUserSettings != null);
        Debug.Assert(m_oReadabilityMetricsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// NodeXLControl containing the graph.

    protected NodeXLControl m_oNodeXLControl;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// Object whose properties are being edited.

    protected ReadabilityMetricUserSettings m_oReadabilityMetricUserSettings;

    /// User settings for this dialog.

    protected ReadabilityMetricsDialogUserSettings
        m_oReadabilityMetricsDialogUserSettings;
}


//*****************************************************************************
//  Class: ReadabilityMetricsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="ReadabilityMetricsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ReadabilityMetricsDialog") ]

public class ReadabilityMetricsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ReadabilityMetricsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ReadabilityMetricsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ReadabilityMetricsDialogUserSettings
    (
        Form oForm
    )
    : base (oForm, true)
    {
        Debug.Assert(oForm != null);

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

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}
}
