
using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: CheckedListBoxPlus
//
/// <summary>
/// Represents a CheckedListBox with additional features.
/// </summary>
//*****************************************************************************

public class CheckedListBoxPlus : CheckedListBox
{
    //*************************************************************************
    //  Constructor: CheckedListBoxPlus()
    //
    /// <summary>
    /// Initializes a new instance of the CheckedListBoxPlus class.
    /// </summary>
    //*************************************************************************

    public CheckedListBoxPlus()
    {
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: SetAllItemsChecked()
    //
    /// <summary>
    /// Checks or unchecks all items.
    /// </summary>
    ///
    /// <param name="setChecked">
    /// true to check all items, false to uncheck them.
    /// </param>
    //*************************************************************************

    public void
    SetAllItemsChecked
    (
        Boolean setChecked
    )
    {
        Int32 iItems = this.Items.Count;

        for (Int32 i = 0; i < iItems; i++)
        {
            this.SetItemChecked(i, setChecked);
        }
    }

    //*************************************************************************
    //  Method: PopulateWithObjectsAndText()
    //
    /// <summary>
    /// Populates the CheckedListBox with arbitrary objects and associated
    /// text.
    /// </summary>
    ///
    /// <param name="objectTextPairs">
    /// One or more object/text pairs.  The text is what gets displayed in the
    /// CheckedListBox.  The associated object, which can be of any type, is
    /// hidden from the user but can be retrieved using the <see
    /// cref="ListControl.SelectedValue" /> property.
    /// </param>
    ///
    /// <remarks>
    /// When you populate the CheckedListBox with this method, you can set and
    /// get the selected object with the <see
    /// cref="ListControl.SelectedValue" /> property.
    /// </remarks>
    ///
    /// <example>
    /// This example populates a CheckedListBox with three items.  The user
    /// sees "None", "10%", and "20%" in the list.  The <see
    /// cref="ListControl.SelectedValue" /> property returns either 0, 0.1, or
    /// 0.2, depending on which item the user has selected.
    ///
    /// <code>
    /// checkedListBoxPlus.PopulateWithObjectsAndText(
    /// 0, "None", 0.1, "10%", 0.2, "20%");
    /// </code>
    ///
    /// </example>
    //*************************************************************************

    public void
    PopulateWithObjectsAndText
    (
        params Object [] objectTextPairs
    )
    {
        ListControlUtil.PopulateWithObjectsAndText(this, objectTextPairs);
    }
}

}
