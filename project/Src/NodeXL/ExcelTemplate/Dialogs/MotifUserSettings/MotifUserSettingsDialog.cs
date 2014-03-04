
using System;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Smrf.NodeXL.Algorithms;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: MotifUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="MotifUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="MotifUserSettings" /> object to the
/// constructor.  If the user edits the object, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK.  Otherwise, the object
/// is not modified and <see cref="Form.ShowDialog()" /> returns
/// DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class MotifUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: MotifUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="MotifUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="motifUserSettings">
    /// The object being edited.
    /// </param>
    //*************************************************************************

    public MotifUserSettingsDialog
    (
        MotifUserSettings motifUserSettings
    )
    {
        Debug.Assert(motifUserSettings != null);
        motifUserSettings.AssertValid();

        m_oMotifUserSettings = motifUserSettings;

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oMotifUserSettingsDialogUserSettings =
            new MotifUserSettingsDialogUserSettings(this);

        InitializeComponent();
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
            Motifs eMotifsToCalculate = Motifs.None;

            if (chkFan.Checked)
            {
                eMotifsToCalculate |= Motifs.Fan;
            }

            if (chkDConnector.Checked)
            {
                eMotifsToCalculate |= Motifs.DConnector;

                Int32 iDConnectorMinimumAnchorVertices,
                    iDConnectorMaximumAnchorVertices;

                if (
                    !ValidateNumericUpDown(nudDConnectorMinimumAnchorVertices,
                        "the minimum number of anchor vertices",
                        out iDConnectorMinimumAnchorVertices)
                    ||
                    !ValidateNumericUpDown(nudDConnectorMaximumAnchorVertices,
                        "the maximum number of anchor vertices",
                        out iDConnectorMaximumAnchorVertices)
                    )
                {
                    return (false);
                }
                else
                {
                    // Don't require the minimum and maximum to be in the
                    // typical order.

                    m_oMotifUserSettings.DConnectorMinimumAnchorVertices =
                        Math.Min(iDConnectorMinimumAnchorVertices,
                            iDConnectorMaximumAnchorVertices);

                    m_oMotifUserSettings.DConnectorMaximumAnchorVertices =
                        Math.Max(iDConnectorMinimumAnchorVertices,
                            iDConnectorMaximumAnchorVertices);
                }
            }

            if (chkClique.Checked)
            {
                eMotifsToCalculate |= Motifs.Clique;

                Int32 iCliqueMinimumMemberVertices,
                    iCliqueMaximumMemberVertices;

                if (
                    !ValidateNumericUpDown(nudCliqueMinimumMemberVertices,
                        "the minimum number of member vertices",
                        out iCliqueMinimumMemberVertices)
                    ||
                    !ValidateNumericUpDown(nudCliqueMaximumMemberVertices,
                        "the maximum number of member vertices",
                        out iCliqueMaximumMemberVertices)
                    )
                {
                    return (false);
                }
                else
                {
                    // Don't require the minimum and maximum to be in the
                    // typical order.

                    m_oMotifUserSettings.CliqueMinimumMemberVertices =
                        Math.Min(iCliqueMinimumMemberVertices,
                            iCliqueMaximumMemberVertices);

                    m_oMotifUserSettings.CliqueMaximumMemberVertices =
                        Math.Max(iCliqueMinimumMemberVertices,
                            iCliqueMaximumMemberVertices);
                }
            }

            m_oMotifUserSettings.MotifsToCalculate = eMotifsToCalculate;
        }
        else
        {
            chkFan.Checked = ShouldGroupByMotif(Motifs.Fan);
            chkDConnector.Checked = ShouldGroupByMotif(Motifs.DConnector);
            chkClique.Checked = ShouldGroupByMotif(Motifs.Clique);

            nudDConnectorMinimumAnchorVertices.Value =
                m_oMotifUserSettings.DConnectorMinimumAnchorVertices;

            nudDConnectorMaximumAnchorVertices.Value =
                m_oMotifUserSettings.DConnectorMaximumAnchorVertices;

            nudCliqueMinimumMemberVertices.Value =
                m_oMotifUserSettings.CliqueMinimumMemberVertices;

            nudCliqueMaximumMemberVertices.Value =
                m_oMotifUserSettings.CliqueMaximumMemberVertices;

            EnableControls();
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ShouldGroupByMotif()
    //
    /// <summary>
    /// Determines whether a motif should be used when grouping the graph's
    /// vertices by motif.
    /// </summary>
    ///
    /// <param name="eMotif">
    /// The motif to test.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="eMotif" /> should be used.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ShouldGroupByMotif
    (
        Smrf.NodeXL.Algorithms.Motifs eMotif
    )
    {
        return ( (m_oMotifUserSettings.MotifsToCalculate & eMotif) != 0 );
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

        EnableControls(chkDConnector.Checked,
            nudDConnectorMinimumAnchorVertices,
            nudDConnectorMaximumAnchorVertices
            );

        EnableControls(chkClique.Checked,
            nudCliqueMinimumMemberVertices,
            nudCliqueMaximumMemberVertices
    );

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
            if (m_oMotifUserSettings.MotifsToCalculate == Motifs.None)
            {
                this.ShowWarning("No motifs have been selected.");
                return;
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

    public  override void
    AssertValid()
    {
        base.AssertValid();

        Debug.Assert(m_oMotifUserSettings != null);
        Debug.Assert(m_oMotifUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected MotifUserSettings m_oMotifUserSettings;

    /// User settings for this dialog.

    protected MotifUserSettingsDialogUserSettings
        m_oMotifUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: MotifUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see cref="MotifUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("MotifUserSettingsDialog") ]

public class MotifUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: MotifUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="MotifUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public MotifUserSettingsDialogUserSettings
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
