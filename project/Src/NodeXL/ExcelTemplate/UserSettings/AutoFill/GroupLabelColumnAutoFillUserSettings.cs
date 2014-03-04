
using System;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupLabelColumnAutoFillUserSettings
//
/// <summary>
/// Stores the user's settings for autofilling the group label column.
/// </summary>
///
/// <remarks>
/// The AutoFill feature automatically fills various attribute columns using
/// values from user-specified source columns.  This class stores the settings
/// for the group label column.
/// </remarks>
//*****************************************************************************

[ TypeConverterAttribute(
    typeof(GroupLabelColumnAutoFillUserSettingsTypeConverter) ) ]

public class GroupLabelColumnAutoFillUserSettings : Object
{
    //*************************************************************************
    //  Constructor: GroupLabelColumnAutoFillUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GroupLabelColumnAutoFillUserSettings" /> class.
    /// </summary>
    //*************************************************************************

    public GroupLabelColumnAutoFillUserSettings()
    {
        m_bPrependWithGroupName = false;

        AssertValid();
    }

    //*************************************************************************
    //  Property: PrependWithGroupName
    //
    /// <summary>
    /// Gets or sets a flag specifying whether the group name should be
    /// prepended to the autofilled group label.
    /// </summary>
    ///
    /// <value>
    /// true to prepend the group name.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    PrependWithGroupName
    {
        get
        {
            AssertValid();

            return (m_bPrependWithGroupName);
        }

        set
        {
            m_bPrependWithGroupName = value;

            AssertValid();
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
        // m_bPrependWithGroupName
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true to prepend the group name.

    protected Boolean m_bPrependWithGroupName;
}


//*****************************************************************************
//  Class: GroupLabelColumnAutoFillUserSettingsTypeConverter
//
/// <summary>
/// Converts a GroupLabelColumnAutoFillUserSettings object to and from a
/// String.
/// </summary>
//*****************************************************************************

public class GroupLabelColumnAutoFillUserSettingsTypeConverter :
    UserSettingsTypeConverterBase
{
    //*************************************************************************
    //  Constructor: GroupLabelColumnAutoFillUserSettingsTypeConverter()
    //
    /// <summary>
    /// Initializes a new instance of the
    /// GroupLabelColumnAutoFillUserSettingsTypeConverter class.
    /// </summary>
    //*************************************************************************

    public GroupLabelColumnAutoFillUserSettingsTypeConverter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: ConvertTo()
    //
    /// <summary>
    /// Converts the given value object to the specified type, using the
    /// specified context and culture information.
    /// </summary>
    ///
    /// <param name="context">
    /// An ITypeDescriptorContext that provides a format context. 
    /// </param>
    ///
    /// <param name="culture">
    /// A CultureInfo. If nullNothingnullptra null reference is passed, the
    /// current culture is assumed. 
    /// </param>
    ///
    /// <param name="value">
    /// The Object to convert.
    /// </param>
    ///
    /// <param name="destinationType">
    /// The Type to convert the value parameter to. 
    /// </param>
    ///
    /// <returns>
    /// An Object that represents the converted value.
    /// </returns>
    //*************************************************************************

    public override Object
    ConvertTo
    (
        ITypeDescriptorContext context,
        CultureInfo culture,
        Object value,
        Type destinationType
    )
    {
        Debug.Assert(value != null);
        Debug.Assert(value is GroupLabelColumnAutoFillUserSettings);
        Debug.Assert( destinationType == typeof(String) );
        AssertValid();

        GroupLabelColumnAutoFillUserSettings
            oGroupLabelColumnAutoFillUserSettings =
            (GroupLabelColumnAutoFillUserSettings)value;

        return ( oGroupLabelColumnAutoFillUserSettings
            .PrependWithGroupName.ToString(CultureInfo.InvariantCulture) );
    }

    //*************************************************************************
    //  Method: ConvertFrom()
    //
    /// <summary>
    /// Converts the given object to the type of this converter, using the
    /// specified context and culture information.
    /// </summary>
    ///
    /// <param name="context">
    /// An ITypeDescriptorContext that provides a format context. 
    /// </param>
    ///
    /// <param name="culture">
    /// A CultureInfo. If null is passed, the current culture is assumed. 
    /// </param>
    ///
    /// <param name="value">
    /// The Object to convert.
    /// </param>
    ///
    /// <returns>
    /// An Object that represents the converted value.
    /// </returns>
    //*************************************************************************

    public override Object
    ConvertFrom
    (
        ITypeDescriptorContext context,
        CultureInfo culture,
        Object value
    )
    {
        Debug.Assert(value != null);
        Debug.Assert(value is String);
        AssertValid();

        GroupLabelColumnAutoFillUserSettings
            oGroupLabelColumnAutoFillUserSettings =
            new GroupLabelColumnAutoFillUserSettings();

        oGroupLabelColumnAutoFillUserSettings.PrependWithGroupName =
            Boolean.Parse( (String)value );

        return (oGroupLabelColumnAutoFillUserSettings);
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
