
using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: ListControlUtil
//
/// <summary>
/// Utility methods for working with list controls.
/// </summary>
//*****************************************************************************

public static class ListControlUtil
{
    //*************************************************************************
    //  Method: PopulateWithEnumValues()
    //
    /// <summary>
    /// Populates a ListControl with all values in an enumeration.
    /// </summary>
    ///
    /// <param name="listControl">
    /// ListControl to populate.
    /// </param>
    ///
    /// <param name="enumType">
    /// Enumeration to populate the ListControl with.
    /// </param>
    ///
    /// <param name="formatForUser">
    /// If true, spaces are inserted between each word in the enum values.  The
    /// value "DaysActiveInNewsgroup" gets displayed as "Days active in
    /// newsgroup", for example.  If false, the values are displayed as is:
    /// "DaysActiveInNewsgroup", for example.
    /// </param>
    ///
    /// <remarks>
    /// This method populates a ListControl with all values in an enumeration.
    /// The user sees the string version of each value in the list.  The
    /// <see cref="ListControl.SelectedValue" /> property returns the selected
    /// value in the enumeration.
    /// </remarks>
    //*************************************************************************

    public static void
    PopulateWithEnumValues
    (
        ListControl listControl,
        Type enumType,
        Boolean formatForUser
    )
    {
        Debug.Assert(listControl != null);

        // Get an array of values in the enumeration.

        Array aoEnumValues = Enum.GetValues(enumType);
        Int32 iEnumValues = aoEnumValues.Length;

        // Create an array of ObjectWithText objects.

        ArrayList oObjectWithText = new ArrayList(iEnumValues);

        for (Int32 i = 0; i < iEnumValues; i++)
        {
            Object oEnumValue = aoEnumValues.GetValue(i);
            String sEnumString = oEnumValue.ToString();

            if (formatForUser)
            {
                sEnumString = EnumUtil.SplitName(sEnumString,
                    EnumSplitStyle.FirstWordStartsUpperCase);
            }

            oObjectWithText.Add( new ObjectWithText(oEnumValue, sEnumString) );
        }

        // Tell the ListControl which properties on the objects in the array
        // should be used.

        listControl.DisplayMember = "Text";
        listControl.ValueMember = "Object";

        // Populate the ListControl.

        listControl.DataSource = oObjectWithText;
    }

    //*************************************************************************
    //  Method: PopulateWithObjectsAndText()
    //
    /// <summary>
    /// Populates a ListControl with arbitrary objects and associated text.
    /// </summary>
    ///
    /// <param name="listControl">
    /// Object to populate.
    /// </param>
    ///
    /// <param name="objectTextPairs">
    /// One or more object/text pairs.  The text is what gets displayed in the
    /// ListControl.  The associated object, which can be of any type, is
    /// hidden from the user but can be retrieved using the
    /// <see cref="ListControl.SelectedValue" />
    /// property.
    /// </param>
    ///
    /// <remarks>
    /// When you populate a ListControl with this method, you can set and get
    /// the selected object with the <see cref="ListControl.SelectedValue" />
    /// property.
    /// </remarks>
    ///
    /// <example>
    /// See <see cref="ComboBoxPlus"/> for an example.
    /// </example>
    //*************************************************************************

    public static void
    PopulateWithObjectsAndText
    (
        ListControl listControl,
        Object [] objectTextPairs
    )
    {
        Debug.Assert(listControl != null);

        Int32 iObjectsAndText = objectTextPairs.Length;
        Debug.Assert(iObjectsAndText % 2 == 0);

        // Create an array of ObjectWithText objects.

        ArrayList oObjectWithText = new ArrayList(iObjectsAndText / 2);

        for (Int32 i = 0; i < iObjectsAndText; i += 2)
        {
            Debug.Assert( objectTextPairs[i + 1].GetType() == typeof(String) );

            oObjectWithText.Add( new ObjectWithText( objectTextPairs[i + 0],
                (String)objectTextPairs[i + 1] ) );
        }

        // Tell the ListControl which properties on the ObjectWithText objects
        // should be used.

        listControl.DisplayMember = "Text";
        listControl.ValueMember = "Object";

        // Populate the ListControl.

        listControl.DataSource = oObjectWithText;
    }
}

}
