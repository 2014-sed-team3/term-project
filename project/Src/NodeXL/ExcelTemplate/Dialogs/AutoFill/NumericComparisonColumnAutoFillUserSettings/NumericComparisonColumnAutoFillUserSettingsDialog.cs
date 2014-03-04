
using System;
using System.Configuration;
using System.Windows.Forms;
using Smrf.AppLib;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NumericComparisonColumnAutoFillUserSettingsDialog
//
/// <summary>
/// Edits a <see cref="NumericComparisonColumnAutoFillUserSettings" /> object.
/// </summary>
///
/// <remarks>
/// Pass a <see cref="NumericComparisonColumnAutoFillUserSettings" /> object to
/// the constructor.  If the user edits the object, <see
/// cref="Form.ShowDialog()" /> returns DialogResult.OK.  Otherwise, the object
/// is not modified and <see cref="Form.ShowDialog()" /> returns
/// DialogResult.Cancel.
/// </remarks>
//*****************************************************************************

public partial class NumericComparisonColumnAutoFillUserSettingsDialog :
    ExcelTemplateForm
{
    //*************************************************************************
    //  Constructor: NumericComparisonColumnAutoFillUserSettingsDialog()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NumericComparisonColumnAutoFillUserSettingsDialog" /> class.
    /// </summary>
    ///
    /// <param name="numericComparisonColumnAutoFillUserSettings">
    /// Object to edit.
    /// </param>
    ///
    /// <param name="populateComboBox">
    /// Method that populates a ComboBox with strings.
    /// </param>
    ///
    /// <param name="columnDescription">
    /// Description of the column that is being autofilled.  Sample: "Edge
    /// Visibility".
    /// </param>
    //*************************************************************************

    public NumericComparisonColumnAutoFillUserSettingsDialog
    (
        NumericComparisonColumnAutoFillUserSettings
            numericComparisonColumnAutoFillUserSettings,

        ComboBoxPopulator populateComboBox,
        String columnDescription
    )
    {
        Debug.Assert(numericComparisonColumnAutoFillUserSettings != null);
        Debug.Assert(populateComboBox != null);
        Debug.Assert( !String.IsNullOrEmpty(columnDescription) );

        InitializeComponent();

        m_oNumericComparisonColumnAutoFillUserSettings =
            numericComparisonColumnAutoFillUserSettings;

        this.Text = String.Format(this.Text, columnDescription);

        cbxComparisonOperator.PopulateWithObjectsAndText(
            ComparisonOperator.LessThan, "Less than",
            ComparisonOperator.LessThanOrEqual, "Less than or equal to",
            ComparisonOperator.Equal, "Equal to",
            ComparisonOperator.NotEqual, "Not equal to",
            ComparisonOperator.GreaterThan, "Greater than",
            ComparisonOperator.GreaterThanOrEqual, "Greater than or equal to"
            );

        String sColumnDescriptionLower = columnDescription.ToLower();

        lblDestination1.Text = String.Format(lblDestination1.Text,
            sColumnDescriptionLower);

        chkDestination2.Text = String.Format(chkDestination2.Text,
            sColumnDescriptionLower);

        populateComboBox(cbxDestination1);
        populateComboBox(cbxDestination2);

        // Instantiate an object that saves and retrieves the position of this
        // dialog.  Note that the object automatically saves the settings when
        // the form closes.

        m_oNumericComparisonColumnAutoFillUserSettingsDialogUserSettings =
            new NumericComparisonColumnAutoFillUserSettingsDialogUserSettings(
                this);

        DoDataExchange(false);

        AssertValid();
    }

    //*************************************************************************
    //  Delegate: ComboBoxPopulator()
    //
    /// <summary>
    /// Represents a method that populates a ComboBox with strings.
    /// </summary>
    ///
    /// <param name="comboBoxToPopulate">
    /// Object to edit.
    /// </param>
    ///
    /// <remarks>
    /// The method must populate <paramref name="comboBoxToPopulate" /> with 
    /// one or more strings.  The string the user selects in the ComboBox is
    /// what gets stored in <see
    /// cref="NumericComparisonColumnAutoFillUserSettings.DestinationString1"
    /// /> or <see
    /// cref="NumericComparisonColumnAutoFillUserSettings.DestinationString2"
    /// />.
    /// </remarks>
    //*************************************************************************

    public delegate void
    ComboBoxPopulator
    (
        ComboBoxPlus comboBoxToPopulate
    );

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
            Double dSourceNumberToCompareTo;

            if ( !this.ValidateDoubleTextBox(txbSourceNumber,
                Double.MinValue, Double.MaxValue, "Enter a number.",
                out dSourceNumberToCompareTo) )
            {
                return (false);
            }

            m_oNumericComparisonColumnAutoFillUserSettings.ComparisonOperator
                = (ComparisonOperator)cbxComparisonOperator.SelectedValue;

            m_oNumericComparisonColumnAutoFillUserSettings.
                SourceNumberToCompareTo = dSourceNumberToCompareTo;

            m_oNumericComparisonColumnAutoFillUserSettings.
                DestinationString1 = cbxDestination1.Text;

            m_oNumericComparisonColumnAutoFillUserSettings.
                DestinationString2 = (chkDestination2.Checked ? 
                cbxDestination2.Text : null);
        }
        else
        {
            cbxComparisonOperator.SelectedValue =
                m_oNumericComparisonColumnAutoFillUserSettings.
                    ComparisonOperator;

            txbSourceNumber.Text =
                m_oNumericComparisonColumnAutoFillUserSettings.
                    SourceNumberToCompareTo.ToString();

            cbxDestination1.Text =
                m_oNumericComparisonColumnAutoFillUserSettings.
                    DestinationString1;

            String sDestinationString2 =
                m_oNumericComparisonColumnAutoFillUserSettings.
                    DestinationString2;

            Boolean bSetDestinationString2 = (sDestinationString2 != null);
            chkDestination2.Checked = bSetDestinationString2;

            if (bSetDestinationString2)
            {
                cbxDestination2.Text = sDestinationString2;
            }

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

        cbxDestination2.Enabled = chkDestination2.Checked;
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

        Debug.Assert(
            m_oNumericComparisonColumnAutoFillUserSettingsDialogUserSettings
                != null);

        Debug.Assert(m_oNumericComparisonColumnAutoFillUserSettings != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// User settings for this dialog.

    protected NumericComparisonColumnAutoFillUserSettingsDialogUserSettings
        m_oNumericComparisonColumnAutoFillUserSettingsDialogUserSettings;

    /// Object being edited.

    protected NumericComparisonColumnAutoFillUserSettings
        m_oNumericComparisonColumnAutoFillUserSettings;
}


//*****************************************************************************
//  Class: NumericComparisonColumnAutoFillUserSettingsDialogUserSettings
//
/// <summary>
/// Stores the user's settings for the <see
/// cref="NumericComparisonColumnAutoFillUserSettingsDialog" />.
/// </summary>
///
/// <remarks>
/// The user settings include the form size and location.
/// </remarks>
//*****************************************************************************

[ SettingsGroupNameAttribute(
    "NumericComparisonColumnAutoFillUserSettingsDialog2") ]

public class NumericComparisonColumnAutoFillUserSettingsDialogUserSettings :
    FormSettings
{
    //*************************************************************************
    //  Constructor: NumericComparisonColumnAutoFillUserSettingsDialog
    //     UserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NumericComparisonColumnAutoFillUserSettingsDialogUserSettings" />
    /// class.
    /// </summary>
    ///
    /// <param name="oForm">
    /// The form to save settings for.
    /// </param>
    //*************************************************************************

    public NumericComparisonColumnAutoFillUserSettingsDialogUserSettings
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
