

using System;
using System.Configuration;
using System.Windows.Forms;
using Smrf.NodeXL.Algorithms;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ClusterUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="ClusterUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="ClusterUserSettings" /> object to the constructor.  If
/// the user edits the object, <see cref="Form.ShowDialog()" /> returns
/// DialogResult.OK.  Otherwise, the object is not modified and <see
/// cref="Form.ShowDialog()" /> returns DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class ClusterUserSettingsDialog : ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: ClusterUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ClusterUserSettingsDialog"
    /// /> class.
    /// </summary>
    ///
    /// <param name="mode">
    /// Indicates the mode in which the dialog is being used.
    /// </param>
    ///
    /// <param name="clusterUserSettings">
    /// The object being edited.
    /// </param>
    //*************************************************************************

    public ClusterUserSettingsDialog
    (
        DialogMode mode,
        ClusterUserSettings clusterUserSettings
    )
    {
        Debug.Assert(clusterUserSettings != null);
        clusterUserSettings.AssertValid();

        m_oClusterUserSettings = clusterUserSettings;

        InitializeComponent();

        if (mode == DialogMode.EditOnly)
        {
            this.Text += " Options";
        }

        // Instantiate an object that saves and retrieves the user settings for
        // this dialog.  Note that the object automatically saves the settings
        // when the form closes.

        m_oClusterUserSettingsDialogUserSettings =
            new ClusterUserSettingsDialogUserSettings(this);

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
        /// The user will edit the cluster settings in this dialog, and then
        /// the caller will group the graph's vertices by cluster after the
        /// dialog closes.

        Normal,

        /// The user will edit the cluster settings in this dialog.  The caller
        /// will NOT group the graph's vertices by cluster after the dialog
        /// closes.

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
        if (bFromControls)
        {
            ClusterAlgorithm eClusterAlgorithm =
                ClusterAlgorithm.ClausetNewmanMoore;

            if (radWakitaTsurumi.Checked)
            {
                eClusterAlgorithm = ClusterAlgorithm.WakitaTsurumi;
            }
            else if (radGirvanNewman.Checked)
            {
                eClusterAlgorithm = ClusterAlgorithm.GirvanNewman;
            }

            m_oClusterUserSettings.ClusterAlgorithm = eClusterAlgorithm;

            m_oClusterUserSettings.PutNeighborlessVerticesInOneCluster =
                chkPutNeighborlessVerticesInOneCluster.Checked;
        }
        else
        {
            switch (m_oClusterUserSettings.ClusterAlgorithm)
            {
                case ClusterAlgorithm.ClausetNewmanMoore:

                    radClausetNewmanMoore.Checked = true;
                    break;

                case ClusterAlgorithm.WakitaTsurumi:

                    radWakitaTsurumi.Checked = true;
                    break;

                case ClusterAlgorithm.GirvanNewman:

                    radGirvanNewman.Checked = true;
                    break;

                default:

                    Debug.Assert(false);
                    break;
            }

            chkPutNeighborlessVerticesInOneCluster.Checked =
                m_oClusterUserSettings.PutNeighborlessVerticesInOneCluster;
        }

        return (true);
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
            this.DialogResult = DialogResult.OK;
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

        Debug.Assert(m_oClusterUserSettings != null);
        Debug.Assert(m_oClusterUserSettingsDialogUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object whose properties are being edited.

    protected ClusterUserSettings m_oClusterUserSettings;

    /// User settings for this dialog.

    protected ClusterUserSettingsDialogUserSettings
        m_oClusterUserSettingsDialogUserSettings;
}


//*****************************************************************************
//  Class: ClusterUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="ClusterUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute("ClusterUserSettingsDialog") ]

public class ClusterUserSettingsDialogUserSettings : FormSettings
{
    //*************************************************************************
    //  Constructor: ClusterUserSettingsDialogUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="ClusterUserSettingsDialogUserSettings" /> class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public ClusterUserSettingsDialogUserSettings
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
