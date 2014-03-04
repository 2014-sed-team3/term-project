
using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: HeaderFooterControl
//
/// <summary>
/// Control for getting header and footer text.
/// </summary>
///
/// <remarks>
/// Set the <see cref="IncludeHeader" />, <see cref="HeaderText" />, <see
/// cref="IncludeFooter" />, and <see cref="FooterText" /> properties after the
/// control is created.  To retrieve the edited properties, call <see
/// cref="Validate" />, and if <see cref="Validate" /> returns true, read the
/// properties.
///
/// <para>
/// This control uses the following keyboard shortcuts: I, F
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class HeaderFooterControl : UserControl
{
    //*************************************************************************
    //  Constructor: HeaderFooterControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderFooterControl" />
    /// class.
    /// </summary>
    //*************************************************************************

    public HeaderFooterControl()
    {
        InitializeComponent();

        m_bIncludeHeader = false;
        m_sHeaderText = String.Empty;
        m_bIncludeFooter = false;
        m_sFooterText = String.Empty;

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Property: IncludeHeader
    //
    /// <summary>
    /// Gets or sets a flag indicating whether a header should be included.
    /// </summary>
    ///
    /// <value>
    /// true to include a header.  The default value is false.
    /// </value>
    //*************************************************************************

    public Boolean
    IncludeHeader
    {
        get
        {
            AssertValid();

            return (m_bIncludeHeader);
        }

        set
        {
            m_bIncludeHeader = value;
            DoDataExchange(false);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: HeaderText
    //
    /// <summary>
    /// Gets or sets the header text.
    /// </summary>
    ///
    /// <value>
    /// The header text.  Can be empty but not null.  The default value is
    /// String.Empty.
    /// </value>
    //*************************************************************************

    public String
    HeaderText
    {
        get
        {
            AssertValid();

            return (m_sHeaderText);
        }

        set
        {
            m_sHeaderText = value;
            DoDataExchange(false);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: IncludeFooter
    //
    /// <summary>
    /// Gets or sets a flag indicating whether a footer should be included.
    /// </summary>
    ///
    /// <value>
    /// true to include a footer.  The default value is false.
    /// </value>
    //*************************************************************************

    public Boolean
    IncludeFooter
    {
        get
        {
            AssertValid();

            return (m_bIncludeFooter);
        }

        set
        {
            m_bIncludeFooter = value;
            DoDataExchange(false);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FooterText
    //
    /// <summary>
    /// Gets or sets the footer text.
    /// </summary>
    ///
    /// <value>
    /// The footer text.  Can be empty but not null.  The default value is
    /// String.Empty.
    /// </value>
    //*************************************************************************

    public String
    FooterText
    {
        get
        {
            AssertValid();

            return (m_sFooterText);
        }

        set
        {
            m_sFooterText = value;
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
            m_bIncludeHeader = chkIncludeHeader.Checked;
            m_sHeaderText = txbHeaderText.Text;

            m_bIncludeFooter = chkIncludeFooter.Checked;
            m_sFooterText = txbFooterText.Text;
        }
        else
        {
            chkIncludeHeader.Checked = m_bIncludeHeader;
            txbHeaderText.Text = m_sHeaderText;

            chkIncludeFooter.Checked = m_bIncludeFooter;
            txbFooterText.Text = m_sFooterText;

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

        txbHeaderText.Enabled = chkIncludeHeader.Checked;
        txbFooterText.Enabled = chkIncludeFooter.Checked;
    }

    //*************************************************************************
    //  Method: OnEventThatRequiresControlEnabling()
    //
    /// <summary>
    /// Handles any event that should changed the enabled state of the dialog's
    /// controls.
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
    OnEventThatRequiresControlEnabling
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EnableControls();
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
        // m_bIncludeHeader
        Debug.Assert(m_sHeaderText != null);
        // m_bIncludeFooter
        Debug.Assert(m_sFooterText != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true to include a header.

    protected Boolean m_bIncludeHeader;

    /// The header text.  Can't be null.

    protected String m_sHeaderText;

    /// true to include a footer.

    protected Boolean m_bIncludeFooter;

    /// The footer text.  Can't be null.

    protected String m_sFooterText;
}

}
