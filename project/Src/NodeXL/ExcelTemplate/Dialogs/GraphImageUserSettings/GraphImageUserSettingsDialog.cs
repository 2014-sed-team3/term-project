

using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GraphImageUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="GraphImageUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="GraphImageUserSettings" /> object to the constructor.  If
/// the user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class GraphImageUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: GraphImageUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphImageUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="graphImageUserSettings">
    /// The object being edited.
    /// </param>
    ///
    /// <param name="nodeXLControlSizePx">
    /// The size of the NodeXLControl, in pixels.
    /// </param>
    //*************************************************************************

    public GraphImageUserSettingsDialog
    (
        GraphImageUserSettings graphImageUserSettings,
        Size nodeXLControlSizePx
    )
    {
        Debug.Assert(graphImageUserSettings != null);
        graphImageUserSettings.AssertValid();
        Debug.Assert(nodeXLControlSizePx.Width >= 0);
        Debug.Assert(nodeXLControlSizePx.Height >= 0);

        InitializeComponent();

        m_oGraphImageUserSettings = graphImageUserSettings;
        m_oNodeXLControlSizePx = nodeXLControlSizePx;
        m_bCalculatingHeightOrWidth = false;

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oGraphImageUserSettingsDialogUserSettings =
            new GraphImageUserSettingsDialogUserSettings(this);

        m_oHeaderFooterFont = m_oGraphImageUserSettings.HeaderFooterFont;

        lblControlWidth.Text =
            nodeXLControlSizePx.Width.ToString(ExcelTemplateForm.Int32Format);

        lblControlHeight.Text =
            nodeXLControlSizePx.Height.ToString(ExcelTemplateForm.Int32Format);

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
            Boolean bUseControlSize = radUseControlSize.Checked;
            Int32 iWidth = 0;
            Int32 iHeight = 0;

            if (!bUseControlSize)
            {
                if (
                    !ValidateWidth(out iWidth)
                    ||
                    !ValidateHeight(out iHeight)
                    )
                {
                    return (false);
                }
            }

            if ( !usrHeaderFooter.Validate() )
            {
                return (false);
            }

            if (!bUseControlSize)
            {
                m_oGraphImageUserSettings.ImageSize =
                    new Size(iWidth, iHeight);
            }

            m_oGraphImageUserSettings.UseControlSize = bUseControlSize;

            m_oGraphImageUserSettings.IncludeHeader =
                usrHeaderFooter.IncludeHeader;

            m_oGraphImageUserSettings.HeaderText = usrHeaderFooter.HeaderText;

            m_oGraphImageUserSettings.IncludeFooter =
                usrHeaderFooter.IncludeFooter;

            m_oGraphImageUserSettings.FooterText = usrHeaderFooter.FooterText;

            m_oGraphImageUserSettings.HeaderFooterFont = m_oHeaderFooterFont;
        }
        else
        {
            radThisSize.Checked = !(radUseControlSize.Checked =
                m_oGraphImageUserSettings.UseControlSize);

            Size oImageSize = m_oGraphImageUserSettings.ImageSize;
            nudWidth.Value = oImageSize.Width;
            nudHeight.Value = oImageSize.Height;

            usrHeaderFooter.IncludeHeader =
                m_oGraphImageUserSettings.IncludeHeader;

            usrHeaderFooter.HeaderText = m_oGraphImageUserSettings.HeaderText;

            usrHeaderFooter.IncludeFooter =
                m_oGraphImageUserSettings.IncludeFooter;

            usrHeaderFooter.FooterText = m_oGraphImageUserSettings.FooterText;

            m_oHeaderFooterFont = m_oGraphImageUserSettings.HeaderFooterFont;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ValidateWidth()
    //
    /// <summary>
    /// Validates the nudWidth control.
    /// </summary>
    ///
    /// <param name="iWidth">
    /// Where the validated width gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if validation passes.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ValidateWidth
    (
        out Int32 iWidth
    )
    {
        return ( ValidateNumericUpDown(nudWidth, "a width", out iWidth) );
    }

    //*************************************************************************
    //  Method: ValidateHeight()
    //
    /// <summary>
    /// Validates the nudHeight control.
    /// </summary>
    ///
    /// <param name="iHeight">
    /// Where the validated height gets stored.
    /// </param>
    ///
    /// <returns>
    /// true if validation passes.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ValidateHeight
    (
        out Int32 iHeight
    )
    {
        return ( ValidateNumericUpDown(nudHeight, "a height", out iHeight) );
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

        grpThisSize.Enabled =
            !(grpUseControlSize.Enabled = radUseControlSize.Checked);
    }

    //*************************************************************************
    //  Method: CalculateWidthOrHeight()
    //
    /// <summary>
    /// Calculates and sets the width (or height) of the image when the user
    /// enters a height (or width) and wants to preserve the aspect ratio.
    /// </summary>
    ///
    /// <param name="bCalculateWidth">
    /// true to calculate the width, false to calculate the height.
    /// </param>
    ///
    /// <remarks>
    /// If the height (or width) is invalid, the width (or height) is not set.
    /// </remarks>
    //*************************************************************************

    protected void
    CalculateWidthOrHeight
    (
        Boolean bCalculateWidth
    )
    {
        AssertValid();

        m_bCalculatingHeightOrWidth = true;

        Double dNodeXLControlAspectRatio =
            (Double)m_oNodeXLControlSizePx.Width /
            (Double)m_oNodeXLControlSizePx.Height;

        Int32 iOtherDimension;

        if ( bCalculateWidth ? ValidateHeight(out iOtherDimension) :
            ValidateWidth(out iOtherDimension) )
        {
            Int32 iCalculatedDimension;
            NumericUpDown oCalculatedNumericUpDown;

            if (bCalculateWidth)
            {
                iCalculatedDimension = (Int32)
                    ( (Double)iOtherDimension * dNodeXLControlAspectRatio );

                oCalculatedNumericUpDown = nudWidth;
            }
            else
            {
                iCalculatedDimension = (Int32)
                    ( (Double)iOtherDimension / dNodeXLControlAspectRatio );

                oCalculatedNumericUpDown = nudHeight;
            }

            if (iCalculatedDimension >= oCalculatedNumericUpDown.Minimum &&
                iCalculatedDimension <= oCalculatedNumericUpDown.Maximum)
            {
                oCalculatedNumericUpDown.Value = iCalculatedDimension;
            }
            else
            {
                oCalculatedNumericUpDown.Text = String.Empty;
            }
        }

        m_bCalculatingHeightOrWidth = false;
    }

    //*************************************************************************
    //  Method: OnWidthOrHeightValueChanged()
    //
    /// <summary>
    /// Handles the ValueChanged event on the nudWidth and nudHeight
    /// NumericUpDown controls.
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
    OnWidthOrHeightValueChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (!m_bCalculatingHeightOrWidth && chkPreserveAspectRatio.Checked)
        {
            CalculateWidthOrHeight(sender == nudHeight);
        }
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
    //  Method: chkPreserveAspectRatio_Click()
    //
    /// <summary>
    /// Handles the Click event on the chkPreserveAspectRatio CheckBox.
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
    chkPreserveAspectRatio_CheckedChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        if (chkPreserveAspectRatio.Checked)
        {
            CalculateWidthOrHeight(false);
        }
    }

    //*************************************************************************
    //  Method: btnHeaderFooterFont_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnHeaderFooterFont button.
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
    btnHeaderFooterFont_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        EditFont(ref m_oHeaderFooterFont);
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
        if ( DoDataExchange(true) )
        {
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

        Debug.Assert(m_oGraphImageUserSettings != null);
        Debug.Assert(m_oGraphImageUserSettingsDialogUserSettings != null);
        Debug.Assert(m_oHeaderFooterFont != null);
        // m_oNodeXLControlSizePx
        // m_bCalculatingHeightOrWidth
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected GraphImageUserSettings m_oGraphImageUserSettings;

    /// User settings for this dialog.

    protected GraphImageUserSettingsDialogUserSettings
        m_oGraphImageUserSettingsDialogUserSettings;

    /// HeaderFooterFont properties of m_oGraphImageUserSettings.  These get
    /// edited by a FontDialog.

    protected Font m_oHeaderFooterFont;

    /// The size of the NodeXLControl, in pixels.

    protected Size m_oNodeXLControlSizePx;

    /// true if the height or width is being calculated.

    protected Boolean m_bCalculatingHeightOrWidth;
}


//*****************************************************************************
//  Class: GraphImageUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="GraphImageUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("GraphImageUserSettingsDialog3") ]

public class GraphImageUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: GraphImageUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphImageUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public GraphImageUserSettingsDialogUserSettings
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
