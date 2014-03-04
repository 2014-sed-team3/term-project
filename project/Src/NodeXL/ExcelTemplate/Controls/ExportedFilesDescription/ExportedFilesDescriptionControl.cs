
using System;
using System.Windows.Forms;
using System.Diagnostics;
using Smrf.NodeXL.Algorithms;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExportedFilesDescriptionControl
//
/// <summary>
/// Control for providing a title and description for files that will be
/// exported.
/// 
/// </summary>
///
/// <remarks>
/// This control is used by several dialogs that export an image of the graph,
/// along with optional NodeXL workbook, workbook options, and GraphML files.
///
/// <para>
/// You must set the <see cref="Workbook" /> property after the control is
/// created.
/// </para>
///
/// <para>
/// Also set the <see cref="Title" /> and <see cref="Description" /> properties
/// after the control is created.  To retrieve the edited properties, call <see
/// cref="Validate" />, and if <see cref="Validate" /> returns true, read the
/// properties.
/// </para>
///
/// <para>
/// This control uses the following keyboard shortcuts: S and whatever is set
/// by the <see cref="TitleLabel" /> and <see cref="DescriptionLabel" />
/// properties.
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class ExportedFilesDescriptionControl : UserControl
{
    //*************************************************************************
    //  Constructor: ExportedFilesDescriptionControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ExportedFilesDescriptionControl" /> class.
    /// </summary>
    //*************************************************************************

    public ExportedFilesDescriptionControl()
    {
        InitializeComponent();

        m_oWorkbook = null;
        m_sTitle = String.Empty;
        m_sDescription = String.Empty;
        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: Workbook
    //
    /// <summary>
    /// Sets the workbook object.
    /// </summary>
    ///
    /// <value>
    /// The workbook containing the graph data.
    /// </value>
    ///
    /// <remarks>
    /// You must set this property after the control is created.
    /// </remarks>
    //*************************************************************************

    public Microsoft.Office.Interop.Excel.Workbook
    Workbook
    {
        set
        {
            m_oWorkbook = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Title
    //
    /// <summary>
    /// Gets or sets the title for the files.
    /// </summary>
    ///
    /// <value>
    /// The title for the files.
    /// </value>
    //*************************************************************************

    public String
    Title
    {
        get
        {
            AssertValid();

            return (m_sTitle);
        }

        set
        {
            m_sTitle = value;
            DoDataExchange(false);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Description
    //
    /// <summary>
    /// Gets or sets the description for the files.
    /// </summary>
    ///
    /// <value>
    /// The description for the files.
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

        set
        {
            m_sDescription = value;
            DoDataExchange(false);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: TitleLabel
    //
    /// <summary>
    /// Gets or sets the label for the title.
    /// </summary>
    ///
    /// <value>
    /// The label for the title, with a colon.
    /// </value>
    //*************************************************************************

    public String
    TitleLabel
    {
        get
        {
            AssertValid();

            return (lblTitle.Text);
        }

        set
        {
            lblTitle.Text = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DescriptionLabel
    //
    /// <summary>
    /// Gets or sets the label for the description.
    /// </summary>
    ///
    /// <value>
    /// The label for the description, with a colon.
    /// </value>
    //*************************************************************************

    public String
    DescriptionLabel
    {
        get
        {
            AssertValid();

            return (lblDescription.Text);
        }

        set
        {
            lblDescription.Text = value;

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
            String sTitle;

            if (
                !FormUtil.ValidateRequiredTextBox(txbTitle,
                
                    String.Format(
                        "Enter a {0}."
                        ,
                        lblTitle.Text.ToLower().Replace("&", String.Empty)
                            .Replace(":", String.Empty)
                        ),

                    out sTitle)
                )
            {
                return (false);
            }

            m_sTitle = sTitle;
            m_sDescription = txbDescription.Text.Trim();
        }
        else
        {
            txbTitle.Text = m_sTitle;
            txbDescription.Text = m_sDescription;
        }

        return (true);
    }

    //*************************************************************************
    //  Method: btnInsertGraphSummary_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnInsertGraphSummary button.
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
    btnInsertGraphSummary_Click
    (
        object sender,
        EventArgs e
    )
    {
        Debug.Assert(m_oWorkbook != null);
        AssertValid();

        String sGraphSummary;

        if ( GraphSummarizer.TrySummarizeGraph(m_oWorkbook,
            out sGraphSummary) )
        {
            txbDescription.SelectedText = sGraphSummary;
        }
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
        // m_oWorkbook
        // m_sTitle
        // m_sDescription
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Workbook containing the graph data.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// The title for the files.

    protected String m_sTitle;

    /// The description for the files.

    protected String m_sDescription;
}

}
