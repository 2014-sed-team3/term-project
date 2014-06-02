

using System;
using System.Configuration;
using System.Windows.Forms;
using Smrf.NodeXL.Layouts;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: LayoutUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="LayoutUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="LayoutUserSettings" /> object to the constructor.  If the
/// user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class LayoutUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: LayoutUserSettingsDialog()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="LayoutUserSettingsDialog" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LayoutUserSettingsDialog" /> class with a LayoutUserSettings
    /// object.
    /// </summary>
    ///
    /// <param name="layoutUserSettings">
    /// The object being edited.
    /// </param>
    //*************************************************************************

    public LayoutUserSettingsDialog
    (
        LayoutUserSettings layoutUserSettings
    )
    : this()
    {
        Debug.Assert(layoutUserSettings != null);
        layoutUserSettings.AssertValid();

        m_oLayoutUserSettings = layoutUserSettings;

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oLayoutUserSettingsDialogUserSettings =
            new LayoutUserSettingsDialogUserSettings(this);

        cbxBoxLayoutAlgorithm.PopulateWithEnumValues(
            typeof(BoxLayoutAlgorithm), true);

        cbxIntergroupEdgeStyle.PopulateWithEnumValues(
            typeof(IntergroupEdgeStyle), true);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: LayoutUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LayoutUserSettingsDialog" /> class for the Visual Studio
    /// designer.
    /// </summary>
    ///
    /// <remarks>
    /// Do not use this constructor.  It is for use by the Visual Studio
    /// designer only.
    /// </remarks>
    //*************************************************************************

    public LayoutUserSettingsDialog()
    {
        InitializeComponent();

        // AssertValid();
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
            Int32 iMargin, iFruchtermanReingoldIterations;
            Single fFruchtermanReingoldC;

            if (
                !ValidateNumericUpDown(nudMargin,
                    "a graph margin", out iMargin)
                ||
                !ValidateNumericUpDown(nudFruchtermanReingoldC,
                    "a strength", out fFruchtermanReingoldC)
                ||
                !ValidateNumericUpDown(nudFruchtermanReingoldIterations,
                    "an iteration count", out iFruchtermanReingoldIterations)
                )
            {
                return (false);
            }

            if (radLayoutStyleNormal.Checked)
            {
                m_oLayoutUserSettings.LayoutStyle = LayoutStyle.Normal;
            }
            else if (radLayoutStyleUseGroups.Checked)
            {
                Int32 iGroupRectanglePenWidth;

                if (!ValidateNumericUpDown(nudGroupRectanglePenWidth,
                        "a box outline width", out iGroupRectanglePenWidth))
                {
                    return (false);
                }

                m_oLayoutUserSettings.LayoutStyle = LayoutStyle.UseGroups;

                m_oLayoutUserSettings.GroupRectanglePenWidth =
                    (Double)nudGroupRectanglePenWidth.Value;

                m_oLayoutUserSettings.BoxLayoutAlgorithm =
                    (BoxLayoutAlgorithm)
                    cbxBoxLayoutAlgorithm.SelectedValue;

                m_oLayoutUserSettings.IntergroupEdgeStyle =
                    (IntergroupEdgeStyle)
                    cbxIntergroupEdgeStyle.SelectedValue;

                m_oLayoutUserSettings.ImproveLayoutOfGroups =
                    chkImproveLayoutOfGroups.Checked;
            }
            else
            {
                Int32 iMaximumVerticesPerBin, iBinLength;

                if (
                    !ValidateNumericUpDown(nudMaximumVerticesPerBin,
                        "a maximum component size", out iMaximumVerticesPerBin)
                    ||
                    !ValidateNumericUpDown(nudBinLength,
                        "a box size", out iBinLength)
                    )
                {
                    return (false);
                }

                m_oLayoutUserSettings.LayoutStyle = LayoutStyle.UseBinning;

                m_oLayoutUserSettings.MaximumVerticesPerBin =
                    iMaximumVerticesPerBin;

                m_oLayoutUserSettings.BinLength = iBinLength;
            }

            m_oLayoutUserSettings.Margin = iMargin;
            m_oLayoutUserSettings.FruchtermanReingoldC = fFruchtermanReingoldC;

            m_oLayoutUserSettings.FruchtermanReingoldIterations =
                iFruchtermanReingoldIterations;
        }
        else
        {
            nudMargin.Value = m_oLayoutUserSettings.Margin;

            switch (m_oLayoutUserSettings.LayoutStyle)
            {
                case LayoutStyle.Normal:

                    radLayoutStyleNormal.Checked = true;
                    break;

                case LayoutStyle.UseGroups:

                    radLayoutStyleUseGroups.Checked = true;
                    break;

                case LayoutStyle.UseBinning:

                    radLayoutStyleUseBinning.Checked = true;
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            nudGroupRectanglePenWidth.Value =
                       (Decimal)m_oLayoutUserSettings.GroupRectanglePenWidth;
            
            cbxBoxLayoutAlgorithm.SelectedValue =
                m_oLayoutUserSettings.BoxLayoutAlgorithm;

            cbxIntergroupEdgeStyle.SelectedValue =
                m_oLayoutUserSettings.IntergroupEdgeStyle;

            chkImproveLayoutOfGroups.Checked =
                m_oLayoutUserSettings.ImproveLayoutOfGroups;

            nudMaximumVerticesPerBin.Value =
                m_oLayoutUserSettings.MaximumVerticesPerBin;

            nudBinLength.Value = m_oLayoutUserSettings.BinLength;

            nudFruchtermanReingoldC.Value =
                (Decimal)m_oLayoutUserSettings.FruchtermanReingoldC;

            nudFruchtermanReingoldIterations.Value =
                m_oLayoutUserSettings.FruchtermanReingoldIterations;

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

        pnlLayoutStyleUseGroups.Enabled = radLayoutStyleUseGroups.Checked;
        pnlLayoutStyleUseBinning.Enabled = radLayoutStyleUseBinning.Checked;
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
    //  Method: btnResetAll_Click()
    //
    /// <summary>
    /// Handles the Click event on the btnResetAll button.
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
    btnResetAll_Click
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        m_oLayoutUserSettings.Reset();
        DoDataExchange(false);
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

        Debug.Assert(m_oLayoutUserSettings != null);
        Debug.Assert(m_oLayoutUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected LayoutUserSettings m_oLayoutUserSettings;

    /// User settings for this dialog.

    protected LayoutUserSettingsDialogUserSettings
        m_oLayoutUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: LayoutUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="LayoutUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("LayoutUserSettingsDialog4") ]

public class LayoutUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: LayoutUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="LayoutUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public LayoutUserSettingsDialogUserSettings
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
