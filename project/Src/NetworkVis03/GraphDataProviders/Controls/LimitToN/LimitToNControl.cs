

using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Smrf.NodeXL.GraphDataProviders
{
//*****************************************************************************
//  Class: LimitToNControl
//
/// <summary>
/// UserControl for selecting a maximum number of objects to include.
/// </summary>
///
/// <remarks>
/// The control contains a CheckBox and a NumericUpDown control.  If the
/// checkbox is checked, the user can enter a value into the NumericUpDown
/// control that is between 10 and <see cref="MaximumN" />.  The value is
/// available via the <see cref="N" /> property.
///
/// <para>
/// Unchecking the checkbox indicates "no limit," in which case the user cannot
/// enter a value and <see cref="N" /> is always Int32.MaxValue.
/// </para>
/// 
/// <para>
/// By default, the "objects" are people.  If the control is being used to
/// select a maximum number of something besides people, set the <see
/// cref="ObjectName" /> property.
/// </para>
///
/// <para>
/// This control uses the following keyboard shortcut: T
/// </para>
///
/// </remarks>
//*****************************************************************************

public partial class LimitToNControl : UserControl
{
    //*************************************************************************
    //  Constructor: LimitToNControl()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="LimitToNControl" /> class.
    /// </summary>
    //*************************************************************************

    public LimitToNControl()
    {
        m_iMaximumN = 9999;

        InitializeComponent();

        nudN.Minimum = MinimumN;
        nudN.Maximum = m_iMaximumN;

        AssertValid();
    }

    //*************************************************************************
    //  Property: N
    //
    /// <summary>
    /// Gets or sets the maximum number of objects to include.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of objects to include, as an Int32.  Must be in the
    /// range <see cref="MinimumN" /> to <see cref="MaximumN" />, or the
    /// value Int32.MaxValue for "no limit."  The default is Int32.MaxValue.
    /// </value>
    //*************************************************************************

    public Int32
    N
    {
        get
        {
            AssertValid();

            if (chkLimitToN.Checked)
            {
                return ( (Int32)nudN.Value );
            }

            return (Int32.MaxValue);
        }

        set
        {
            Boolean bLimitToN = (value < Int32.MaxValue);

            chkLimitToN.Checked = bLimitToN;

            if (bLimitToN)
            {
                nudN.Value = value;
            }

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MaximumN
    //
    /// <summary>
    /// Gets or sets the maximum value the user can enter.
    /// </summary>
    ///
    /// <value>
    /// The maximum value the user can enter.  The default is 9,999.
    /// </value>
    //*************************************************************************

    public Int32
    MaximumN
    {
        get
        {
            AssertValid();

            return (m_iMaximumN);
        }

        set
        {
            nudN.Maximum = m_iMaximumN = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ObjectName
    //
    /// <summary>
    /// Gets or sets the name of the objects being limited.
    /// </summary>
    ///
    /// <value>
    /// The name of the objects being limited.  The default is "people".
    /// </value>
    //*************************************************************************

    public String
    ObjectName
    {
        get
        {
            AssertValid();

            return (lblObjectName.Text);
        }

        set
        {
            lblObjectName.Text = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: chkLimitToN_CheckedChanged()
    //
    /// <summary>
    /// Handles the CheckedChanged event on the chkLimitToN CheckBox.
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
    chkLimitToN_CheckedChanged
    (
        object sender,
        EventArgs e
    )
    {
        AssertValid();

        nudN.Enabled = chkLimitToN.Checked;
    }

    //*************************************************************************
    //  Method: nudN_Validating()
    //
    /// <summary>
    /// Handles the Validating event on the nudN NumericUpDown.
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
    nudN_Validating
    (
        object sender,
        System.ComponentModel.CancelEventArgs e
    )
    {
        AssertValid();

        // The NumericUpDown control allows the number to be deleted, for some
        // reason.  Don't allow this.

        if (nudN.Text.Length == 0)
        {
            nudN.Text = MinimumN.ToString();
            nudN.Value = MinimumN;
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
        Debug.Assert(m_iMaximumN > 0);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// The minimum value of the <see cref="N" /> property when N is not
    /// Int32.MaxValue.
    /// </summary>
    ///
    /// <remarks>
    /// This is a constant for now, because no callers need to change it.
    /// </remarks>

    public const Int32 MinimumN = 10;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The maximum value of the N property when N is not Int32.MaxValue.

    protected Int32 m_iMaximumN;
}
}
