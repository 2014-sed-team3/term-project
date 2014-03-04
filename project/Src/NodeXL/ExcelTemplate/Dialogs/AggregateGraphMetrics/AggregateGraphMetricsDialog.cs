

using System;
using System.Configuration;
using System.ComponentModel;
using System.Windows.Forms;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AggregateGraphMetricsDialog
//
/// <summary>
/// Aggregates the overall metrics from a folder full of NodeXL workbooks into
/// a new Excel workbook.
/// </summary>
///
/// <remarks>
/// Call <see cref="Form.ShowDialog()" /> to run the dialog.  All aggregation
/// is performed within the dialog.
/// </remarks>
//*****************************************************************************

public partial class AggregateGraphMetricsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: AggregateGraphMetricsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AggregateGraphMetricsDialog" /> class.
    /// </summary>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph data.
    /// </param>
    //*************************************************************************

    public AggregateGraphMetricsDialog
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        InitializeComponent();

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oAggregateGraphMetricsDialogUserSettings =
            new AggregateGraphMetricsDialogUserSettings(this);

        m_oWorkbook = workbook;

        m_oGraphMetricsAggregator = new GraphMetricsAggregator();

        m_oGraphMetricsAggregator.AggregationProgressChanged +=
            new ProgressChangedEventHandler(
                GraphMetricsAggregator_AggregationProgressChanged);

        m_oGraphMetricsAggregator.AggregationCompleted +=
            new RunWorkerCompletedEventHandler(
                GraphMetricsAggregator_AggregationCompleted);

        DoDataExchange(false);

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
        if (bFromControls)
        {
            if ( !usrSourceFolder.Validate() )
            {
                return (false);
            }

            m_oAggregateGraphMetricsDialogUserSettings.SourceFolderPath =
                usrSourceFolder.FolderPath;
        }
        else
        {
            usrSourceFolder.FolderPath =
                m_oAggregateGraphMetricsDialogUserSettings.SourceFolderPath;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: EnableControls()
    //
    /// <summary>
    /// Enables or disables the dialog's controls.
    /// </summary>
    //*************************************************************************

    protected void
    EnableControls()
    {
        AssertValid();

        pnlEnable.Enabled = btnOK.Enabled = !m_oGraphMetricsAggregator.IsBusy;
    }

    //*************************************************************************
    //  Method: OnClosed()
    //
    /// <summary>
    /// Handles the Closed event.
    /// </summary>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnClosed
    (
        EventArgs e
    )
    {
        AssertValid();

        if (m_oGraphMetricsAggregator.IsBusy)
        {
            // Let the background thread cancel its task, but don't try to
            // notify this dialog.

            m_oGraphMetricsAggregator.AggregationProgressChanged -=
                new ProgressChangedEventHandler(
                    GraphMetricsAggregator_AggregationProgressChanged);

            m_oGraphMetricsAggregator.AggregationCompleted -=
                new RunWorkerCompletedEventHandler(
                    GraphMetricsAggregator_AggregationCompleted);

            m_oGraphMetricsAggregator.CancelAsync();
        }
    }

    //*************************************************************************
    //  Method: btnOK_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOK button.
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
    btnOK_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (!m_oGraphMetricsAggregator.IsBusy)
        {
            if ( DoDataExchange(true) )
            {
                m_oGraphMetricsAggregator.AggregateGraphMetricsAsync(

                    m_oAggregateGraphMetricsDialogUserSettings.
                        SourceFolderPath,

                    m_oWorkbook
                    );

                EnableControls();
            }
        }
    }

    //*************************************************************************
    //  Method: GraphMetricsAggregator_AggregationProgressChanged()
    //
    /// <summary>
    /// Handles the AggregationProgressChanged event on the
    /// GraphMetricsAggregator object.
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
    GraphMetricsAggregator_AggregationProgressChanged
    (
        object sender,
        ProgressChangedEventArgs e
    )
    {
        Debug.Assert(e.UserState is String);
        AssertValid();

        slStatusLabel.Text = (String)e.UserState;
    }

    //*************************************************************************
    //  Method: GraphMetricsAggregator_AggregationCompleted()
    //
    /// <summary>
    /// Handles the AggregationCompleted event on the GraphMetricsAggregator
    /// object.
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
    GraphMetricsAggregator_AggregationCompleted
    (
        object sender,
        RunWorkerCompletedEventArgs e
    )
    {
        AssertValid();

        EnableControls();

        if (e.Cancelled)
        {
        }
        else if (e.Error != null)
        {
            ErrorUtil.OnException(e.Error);
            slStatusLabel.Text = String.Empty;
        }
        else
        {
            // The final status message is a summary of what was done.  Show it
            // in a message box in addition to the StatusLabel.

            this.ShowInformation(slStatusLabel.Text);
            this.Close();
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

        Debug.Assert(m_oAggregateGraphMetricsDialogUserSettings != null);
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oGraphMetricsAggregator != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected AggregateGraphMetricsDialogUserSettings
        m_oAggregateGraphMetricsDialogUserSettings;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// Object that does most of the work.

    protected GraphMetricsAggregator m_oGraphMetricsAggregator;
}


//*****************************************************************************
//  Class: AggregateGraphMetricsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="AggregateGraphMetricsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("AggregateGraphMetricsDialog") ]

public class AggregateGraphMetricsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: AggregateGraphMetricsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AggregateGraphMetricsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public AggregateGraphMetricsDialogUserSettings
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
    //  Property: SourceFolderPath
    //
    /// <summary>
    /// Gets or sets the path of the source folder.
    /// </summary>
    ///
    /// <value>
    /// The path of the source folder.  The default is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    SourceFolderPath
    {
        get
        {
            AssertValid();

            return ( (String)this[SourceFolderPathKey] );
        }

        set
        {
            this[SourceFolderPathKey] = value;

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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Name of the settings key for the SourceFolderPath property.

    protected const String SourceFolderPathKey = "SourceFolderPath";
}
}
