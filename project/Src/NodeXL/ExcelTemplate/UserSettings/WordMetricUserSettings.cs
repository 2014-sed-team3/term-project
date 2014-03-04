
using System;
using System.Configuration;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: WordMetricUserSettings
//
/// <summary>
/// Stores the user's settings for calculating word metrics.
/// </summary>
//*****************************************************************************

[ TypeConverterAttribute( typeof(WordMetricUserSettingsTypeConverter) ) ]

public class WordMetricUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: WordMetricUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the WordMetricUserSettings class.
    /// </summary>
    //*************************************************************************

    public WordMetricUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: TextColumnIsOnEdgeWorksheet
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the column containing the text
    /// is on the edge worksheet.
    /// </summary>
    ///
    /// <value>
    /// true if the column is on the edge worksheet, false if it is on the
    /// vertex worksheet.  The default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    TextColumnIsOnEdgeWorksheet
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[TextColumnIsOnEdgeWorksheetKey] );
        }

        set
        {
            this[TextColumnIsOnEdgeWorksheetKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: TextColumnName
    //
    /// <summary>
    /// Gets or sets the name of the text column.
    /// </summary>
    ///
    /// <value>
    /// The name of the text column.  The default value is String.Empty.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("") ]

    public String
    TextColumnName
    {
        get
        {
            AssertValid();

            return ( (String)this[TextColumnNameKey] );
        }

        set
        {
            this[TextColumnNameKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: CountByGroup
    //
    /// <summary>
    /// Gets or sets a flag indicating whether terms should be counted by
    /// group.
    /// </summary>
    ///
    /// <value>
    /// true if terms should be counted by group, false if they should be
    /// counted only for the graph as a whole.  The default value is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    CountByGroup
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[CountByGroupKey] );
        }

        set
        {
            this[CountByGroupKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SkipSingleTerms
    //
    /// <summary>
    /// Gets or sets a flag indicating whether terms that occur only once
    /// should be skipped rather than counted.
    /// </summary>
    ///
    /// <value>
    /// true to skip terms that occur only once, false to count them.  The
    /// default value is true.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("true") ]

    public Boolean
    SkipSingleTerms
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[SkipSingleTermsKey] );
        }

        set
        {
            this[SkipSingleTermsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: WordsToSkip
    //
    /// <summary>
    /// Gets or sets the words that should be skipped.
    /// </summary>
    ///
    /// <value>
    /// The words to skip, delimited by spaces, commas, carriage returns or
    /// linefeeds.  The case of the words is not important.  The default
    /// contains several such words.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]

    [ DefaultSettingValueAttribute(
        WordCounter.SampleSpaceDelimitedEnglishStopWords) ]

    public String
    WordsToSkip
    {
        get
        {
            AssertValid();

            return ( (String)this[WordsToSkipKey] );
        }

        set
        {
            this[WordsToSkipKey] = value;

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

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    // These are public to allow them to be shared with
    // WordMetricUserSettingsTypeConverter.  There is no point in having that
    // class define its own keys.

    /// Name of the settings key for the TextColumnIsOnEdgeWorksheet property.

    public const String TextColumnIsOnEdgeWorksheetKey =
        "TextColumnIsOnEdgeWorksheet";

    /// Name of the settings key for the TextColumnName property.

    public const String TextColumnNameKey =
        "TextColumnName";

    /// Name of the settings key for the CountByGroup property.

    public const String CountByGroupKey =
        "CountByGroup";

    /// Name of the settings key for the SkipSingleTerms property.

    public const String SkipSingleTermsKey =
        "SkipSingleTerms";

    /// Name of the settings key for the WordsToSkip property.

    public const String WordsToSkipKey =
        "WordsToSkip";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}


//*****************************************************************************
//  Class: WordMetricUserSettingsTypeConverter
//
/// <summary>
/// Converts a <see cref="WordMetricUserSettings" /> object to and from a
/// String.
/// </summary>
/// 
/// <remarks>
/// The <see cref="GraphMetricUserSettings.WordMetricUserSettings" /> property
/// is of type <see cref="WordMetricUserSettings" />.  The application settings
/// architecture requires a type converter for such a nested type.
/// </remarks>
//*****************************************************************************

public class WordMetricUserSettingsTypeConverter :
    UserSettingsTypeConverterBase
{
    //*************************************************************************
    //  Constructor: WordMetricUserSettingsTypeConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="WordMetricUserSettingsTypeConverter" /> class.
    /// </summary>
    //*************************************************************************

    public WordMetricUserSettingsTypeConverter()
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
    /// A CultureInfo. If null is passed, the current culture is assumed. 
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
        Debug.Assert(value is WordMetricUserSettings);
        Debug.Assert( destinationType == typeof(String) );
        AssertValid();

        // Note:
        //
        // Earlier user settings type converter classes used a string of
        // ordered, tab-delimited values to persist the user settings.  That
        // was a brittle solution.  Newer classes, including this one, use an
        // unordered dictionary.

        WordMetricUserSettings oWordMetricUserSettings =
            (WordMetricUserSettings)value;

        PersistableStringDictionary oDictionary =
            new PersistableStringDictionary();

        oDictionary.Add(
            WordMetricUserSettings.TextColumnIsOnEdgeWorksheetKey,
            oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet);

        oDictionary.Add(WordMetricUserSettings.TextColumnNameKey,
            oWordMetricUserSettings.TextColumnName);

        oDictionary.Add(WordMetricUserSettings.CountByGroupKey,
            oWordMetricUserSettings.CountByGroup);

        oDictionary.Add(WordMetricUserSettings.SkipSingleTermsKey,
            oWordMetricUserSettings.SkipSingleTerms);

        oDictionary.Add(WordMetricUserSettings.WordsToSkipKey,
            oWordMetricUserSettings.WordsToSkip);

        return ( oDictionary.ToString() );
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
    /// A CultureInfo. If nullNothingnullptra null reference is passed, the
    /// current culture is assumed. 
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

        PersistableStringDictionary oDictionary =
            PersistableStringDictionary.FromString( (String)value );

        WordMetricUserSettings oWordMetricUserSettings =
            new WordMetricUserSettings();

        Boolean bValue;
        String sValue;

        if ( oDictionary.TryGetValue(
            WordMetricUserSettings.TextColumnIsOnEdgeWorksheetKey,
            out bValue) )
        {
            oWordMetricUserSettings.TextColumnIsOnEdgeWorksheet = bValue;
        }

        if ( oDictionary.TryGetValue(
            WordMetricUserSettings.TextColumnNameKey, out sValue) )
        {
            oWordMetricUserSettings.TextColumnName = sValue;
        }

        if ( oDictionary.TryGetValue(
            WordMetricUserSettings.CountByGroupKey, out bValue) )
        {
            oWordMetricUserSettings.CountByGroup = bValue;
        }

        if ( oDictionary.TryGetValue(
            WordMetricUserSettings.SkipSingleTermsKey, out bValue) )
        {
            oWordMetricUserSettings.SkipSingleTerms = bValue;
        }

        oWordMetricUserSettings.WordsToSkip =
            oDictionary.TryGetValue(WordMetricUserSettings.WordsToSkipKey,
                out sValue) ? sValue : String.Empty;

        return (oWordMetricUserSettings);
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
