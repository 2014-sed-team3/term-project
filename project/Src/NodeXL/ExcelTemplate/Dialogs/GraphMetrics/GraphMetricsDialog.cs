
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Windows.Forms;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphMetricsDialog
//
/// <summary>
/// Edits the user's graph metric settings and optionally calculates the graph
/// metrics.
/// </summary>
///
/// <remarks>
/// This dialog lets the user select from a set of graph metrics.  It saves the
/// user's selection in a <see cref="GraphMetricUserSettings" /> object.  If
/// the <see cref="DialogMode" /> argument to the constructor is <see
/// cref="DialogMode.Normal" />, it also calculates the selected metrics.
///
/// <para>
/// If the user edits and saves graph metrics, <see cref="Form.ShowDialog()" />
/// returns DialogResult.OK.  Otherwise, it returns DialogResult.Cancel.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class GraphMetricsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: GraphMetricsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GraphMetricsDialog" />
    /// class.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the dialog is being used.
    /// </param>
    ///
    /// <param name="workbook">
    /// Workbook containing the graph contents.
    /// </param>
    //*************************************************************************

    public GraphMetricsDialog
    (
        DialogMode mode,
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        InitializeComponent();

        m_eMode = mode;
        m_oWorkbook = workbook;
        m_oGraphMetricUserSettings = new GraphMetricUserSettings();

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oGraphMetricsDialogUserSettings =
            new GraphMetricsDialogUserSettings(this);

        if (m_eMode == DialogMode.EditOnly)
        {
            this.Text += " Options";
            btnOK.Text = "OK";
        }

        clbGraphMetrics.Items.AddRange(
            GraphMetricInformation.GetAllGraphMetricInformation() );

        clbGraphMetrics.SetSelected(0, true);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Enum: DialogMode
    //
    /// <summary>
    /// Indicates the mode in which the dialog is being used.
    /// </summary>
    //*************************************************************************

    public enum
    DialogMode
    {
        /// The user can edit the dialog settings and then calculate metrics.

        Normal,

        /// The user can edit the dialog settings but cannot calculate metrics.

        EditOnly,
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
            GraphMetrics eGraphMetricsToCalculate = GraphMetrics.None;

            foreach (Object oCheckedItem in clbGraphMetrics.CheckedItems)
            {
                eGraphMetricsToCalculate |=
                    ItemToGraphMetricInformation(oCheckedItem).GraphMetric;
            }

            m_oGraphMetricUserSettings.GraphMetricsToCalculate =
                eGraphMetricsToCalculate;
        }
        else
        {
            CheckedListBox.ObjectCollection oItems = clbGraphMetrics.Items;
            Int32 iItems = oItems.Count;

            for (Int32 i = 0; i < iItems; i++)
            {
                clbGraphMetrics.SetItemChecked(i, 
                    m_oGraphMetricUserSettings.ShouldCalculateGraphMetrics(
                    ItemToGraphMetricInformation( oItems[i] ).GraphMetric) );
            }
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetSelectedGraphMetricInformation()
    //
    /// <summary>
    /// Gets the selected GraphMetricInformation object in the clbGraphMetrics
    /// CheckedListBox, if there is one.
    /// </summary>
    ///
    /// <param name="oSelectedGraphMetricInformation">
    /// Where the selected GraphMetricInformation object gets stored if true is
    /// returned.
    /// </param>
    ///
    /// <returns>
    /// true if an object was selected.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryGetSelectedGraphMetricInformation
    (
        out GraphMetricInformation oSelectedGraphMetricInformation
    )
    {
        AssertValid();

        Int32 iSelectedIndex = clbGraphMetrics.SelectedIndex;

        if (iSelectedIndex >= 0)
        {
            oSelectedGraphMetricInformation = ItemToGraphMetricInformation(
                clbGraphMetrics.Items[iSelectedIndex] );

            return (true);
        }

        oSelectedGraphMetricInformation = null;
        return (false);
    }

    //*************************************************************************
    //  Method: ItemToGraphMetricInformation()
    //
    /// <summary>
    /// Casts an item from the clbGraphMetrics CheckedListBox to a
    /// GraphMetricInformation object.
    /// </summary>
    ///
    /// <param name="oCheckedListBoxItem">
    /// An item from the clbGraphMetrics CheckedListBox.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="oCheckedListBoxItem" /> cast to a
    /// GraphMetricInformation object.
    /// </returns>
    //*************************************************************************

    protected GraphMetricInformation
    ItemToGraphMetricInformation
    (
        Object oCheckedListBoxItem
    )
    {
        Debug.Assert(oCheckedListBoxItem != null);
        AssertValid();

        Debug.Assert(oCheckedListBoxItem is GraphMetricInformation);

        return ( (GraphMetricInformation)oCheckedListBoxItem );
    }

    //*************************************************************************
    //  Method: EditWordMetricUserSettings()
    //
    /// <summary>
    /// Opens a dialog for editing word metric user settings.
    /// </summary>
    //*************************************************************************

    protected void
    EditWordMetricUserSettings()
    {
        AssertValid();

        WordMetricUserSettings oWordMetricUserSettings =
            m_oGraphMetricUserSettings.WordMetricUserSettings;

        if ( ( new WordMetricUserSettingsDialog(
            oWordMetricUserSettings, m_oWorkbook) ).ShowDialog() ==
            DialogResult.OK )
        {
            m_oGraphMetricUserSettings.WordMetricUserSettings =
                oWordMetricUserSettings;
        }
    }

    //*************************************************************************
    //  Method: EditTopNByMetricUserSettings()
    //
    /// <summary>
    /// Opens a dialog for editing top-N-by metric user settings.
    /// </summary>
    //*************************************************************************

    protected void
    EditTopNByMetricUserSettings()
    {
        AssertValid();

        List<TopNByMetricUserSettings> oTopNByMetricUserSettings =
            m_oGraphMetricUserSettings.TopNByMetricsToCalculate;

        if ( ( new TopNByMetricUserSettingsListDialog(
            oTopNByMetricUserSettings, m_oWorkbook) ).ShowDialog() ==
            DialogResult.OK )
        {
            m_oGraphMetricUserSettings.TopNByMetricsToCalculate =
                oTopNByMetricUserSettings;
        }
    }

    //*************************************************************************
    //  Method: CalculateTopNByMetrics()
    //
    /// <summary>
    /// Calculates top-N-by metrics.
    /// </summary>
    ///
    /// <returns>
    /// true if the metrics were successfully calculated.
    /// </returns>
    //*************************************************************************

    protected Boolean
    CalculateTopNByMetrics()
    {
        AssertValid();

        TopNByMetricCalculator2 oTopNByMetricCalculator2 =
            new TopNByMetricCalculator2();

        CalculateGraphMetricsDialog oCalculateGraphMetricsDialog =
            new CalculateGraphMetricsDialog(null, m_oWorkbook,
                new IGraphMetricCalculator2 [] {oTopNByMetricCalculator2},
                null, true);
                
        return (oCalculateGraphMetricsDialog.ShowDialog() == DialogResult.OK);
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

    private void
    OnCheckOrUncheckAll
    (
        object sender,
        System.EventArgs e
    )
    {
        AssertValid();

        clbGraphMetrics.SetAllItemsChecked(sender == btnCheckAll);
    }

    //*************************************************************************
    //  Method: clbGraphMetrics_SelectedIndexChanged()
    //
    /// <summary>
    /// Handles the SelectedIndexChanged event on the clbGraphMetrics
    /// CheckedListBox.
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
    clbGraphMetrics_SelectedIndexChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        GraphMetricInformation oSelectedGraphMetricInformation;

        if ( TryGetSelectedGraphMetricInformation(
            out oSelectedGraphMetricInformation) )
        {
            // Insert the description, which is in HTML format, into a web
            // page, then insert the web page into the wbDescription
            // WebBrowser.

            wbDescription.DocumentText = String.Format(

                @"<html>
                <head>
                    <style>
                        BODY
                        {{
                            border: 1px solid black;
                        }}

                        BODY, TABLE
                        {{
                            font-family: arial;
                            font-size: 9.0pt;
                        }}

                        TABLE
                        {{
                            border: 1px solid black;
                            border-collapse: collapse;
                        }}

                        TD, TH
                        {{
                            border: 1px solid black;
                            padding: 0.3em;
                            vertical-align: top;
                        }}

                        .divCaption
                        {{
                            border: 1px solid gray;
                            padding: 0.1em;
                            color: HighlightText;
                            background: Highlight;
                        }}

                        .divQuote, .divUrl
                        {{
                            margin-left: 2em;
                        }}

                        .spnOptions
                        {{
                            font-weight: bold;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""divCaption"">{0}</div>
                    {1}
                </body>
                </html>"
                ,
                oSelectedGraphMetricInformation.Name,
                oSelectedGraphMetricInformation.DescriptionAsHtml
                );

            btnOptions.Enabled =
                oSelectedGraphMetricInformation.HasUserSettings;
        }
    }

    //*************************************************************************
    //  Method: btnOptions_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnOptions button.
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
    btnOptions_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        GraphMetricInformation oGraphMetricInformation;

        if ( TryGetSelectedGraphMetricInformation(
            out oGraphMetricInformation) )
        {
            switch (oGraphMetricInformation.GraphMetric)
            {
                case GraphMetrics.Words:

                    EditWordMetricUserSettings();
                    break;

                case GraphMetrics.TopNBy:

                    EditTopNByMetricUserSettings();
                    break;

                default:

                    break;
            }
        }
    }

    //*************************************************************************
    //  Method: wbDescription_Navigating()
    //
    /// <summary>
    /// Handles the Navigating event on the wbDescription WebBrowser.
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
    wbDescription_Navigating
    (
        object sender,
        WebBrowserNavigatingEventArgs e
    )
    {
        // AssertValid();

        String sUrl = e.Url.ToString();

        // This event is fired with an URL of "about:blank" when this dialog
        // sets the WebBrowser's DocumentText property.  Ignore the event in
        // that case.

        if (sUrl.IndexOf("about:blank") == -1)
        {
            // By default, clicking an URL in the description displayed in the
            // WebBrowser control uses Internet Explorer to open the URL.  Use
            // the user's default browser instead.

            e.Cancel = true;
            Process.Start(sUrl);
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
        System.EventArgs e
    )
    {
        if ( !DoDataExchange(true) )
        {
            return;
        }

        if (m_oGraphMetricUserSettings.GraphMetricsToCalculate ==
            GraphMetrics.None)
        {
            this.ShowWarning("No metrics have been selected.");
            return;
        }

        // The IGraphMetricCalculator2 implementations read the 
        // GraphMetricUserSettings settings, so save them.

        m_oGraphMetricUserSettings.Save();

        if (m_eMode == DialogMode.EditOnly)
        {
            DialogResult = DialogResult.OK;
            this.Close();
            return;
        }

        // The CalculateGraphMetricsDialog does all the work.  Use the
        // constructor overload that uses a default list of graph metric
        // calculators.

        CalculateGraphMetricsDialog oCalculateGraphMetricsDialog =
            new CalculateGraphMetricsDialog(null, m_oWorkbook);

        if (oCalculateGraphMetricsDialog.ShowDialog() == DialogResult.OK)
        {
            if ( m_oGraphMetricUserSettings.ShouldCalculateGraphMetrics(
                GraphMetrics.TopNBy) )
            {
                // The CalculateGraphMetricsDialog does not calculate top-N-by
                // metrics by default, because those metrics can't always be
                // calculated without first calculating other metrics and
                // writing them to the workbook.  Now that the other metrics
                // have been calculated and written to the workbook, calculate
                // the top-N-by metrics.

                if ( !CalculateTopNByMetrics() )
                {
                    return;
                }
            }

            DialogResult = DialogResult.OK;
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

        // m_eMode
        Debug.Assert(m_oWorkbook != null);
        Debug.Assert(m_oGraphMetricUserSettings != null);
        Debug.Assert(m_oGraphMetricsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Indicates the mode in which the dialog is being used.

    protected DialogMode m_eMode;

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// Object whose properties are being edited.

    protected GraphMetricUserSettings m_oGraphMetricUserSettings;

    /// User settings for this dialog.

    protected GraphMetricsDialogUserSettings m_oGraphMetricsDialogUserSettings;
}


//*****************************************************************************
//  Class: GraphMetricsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="GraphMetricsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphMetricsDialog6") ]

public class GraphMetricsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GraphMetricsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphMetricsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public GraphMetricsDialogUserSettings
    (
        Form oForm
    )
    : base (oForm, false)
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
