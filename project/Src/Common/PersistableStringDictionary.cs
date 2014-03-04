
using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: PersistableStringDictionary
//
/// <summary>
/// String/String dictionary that can be saved to and loaded from a string.
/// </summary>
///
/// <remarks>
/// The entire dictionary can be saved to a single delimited string using <see
/// cref="ToString" />, and the collection can be restored from the delimited
/// string using <see cref="FromString" />.
/// </remarks>
//*****************************************************************************

public class PersistableStringDictionary : Dictionary<String, String>
{
    //*************************************************************************
    //  Constructor: PersistableStringDictionary()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="PersistableStringDictionary" /> class.
    /// </summary>
    //*************************************************************************

    public PersistableStringDictionary()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: Add()
    //
    /// <summary>
    /// Adds an Int32 value to the collection.
    /// </summary>
    ///
    /// <param name="key">
    /// The key to add.
    /// </param>
    ///
    /// <param name="value">
    /// The value to add.
    /// </param>
    //*************************************************************************

    public void
    Add
    (
        String key,
        Int32 value
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(key) );
        AssertValid();

        this.Add( key, value.ToString(CultureInfo.InvariantCulture) );
    }

    //*************************************************************************
    //  Method: Add()
    //
    /// <summary>
    /// Adds a Double value to the collection.
    /// </summary>
    ///
    /// <param name="key">
    /// The key to add.
    /// </param>
    ///
    /// <param name="value">
    /// The value to add.
    /// </param>
    //*************************************************************************

    public void
    Add
    (
        String key,
        Double value
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(key) );
        AssertValid();

        this.Add( key, value.ToString(CultureInfo.InvariantCulture) );
    }

    //*************************************************************************
    //  Method: Add()
    //
    /// <summary>
    /// Adds a Boolean value to the collection.
    /// </summary>
    ///
    /// <param name="key">
    /// The key to add.
    /// </param>
    ///
    /// <param name="value">
    /// The value to add.
    /// </param>
    //*************************************************************************

    public void
    Add
    (
        String key,
        Boolean value
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(key) );
        AssertValid();

        this.Add( key, value.ToString(CultureInfo.InvariantCulture) );
    }

    //*************************************************************************
    //  Method: TryGetValue()
    //
    /// <summary>
    /// Attempts to get an Int32 value from the collection.
    /// </summary>
    ///
    /// <param name="key">
    /// The key to get.
    /// </param>
    ///
    /// <param name="value">
    /// Where the value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains an Int32 value in the specified key.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryGetValue
    (
        String key,
        out Int32 value
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(key) );
        AssertValid();

        value = Int32.MinValue;
        String sValue;

        return (
            this.TryGetValue(key, out sValue)
            &&
            Int32.TryParse(sValue, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out value)
            );
    }

    //*************************************************************************
    //  Method: TryGetValue()
    //
    /// <summary>
    /// Attempts to get a Double value from the collection.
    /// </summary>
    ///
    /// <param name="key">
    /// The key to get.
    /// </param>
    ///
    /// <param name="value">
    /// Where the value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains a Double value in the specified key.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryGetValue
    (
        String key,
        out Double value
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(key) );
        AssertValid();

        value = Double.MinValue;
        String sValue;

        return (
            this.TryGetValue(key, out sValue)
            &&
            Double.TryParse(sValue, NumberStyles.Float,
                CultureInfo.InvariantCulture, out value)
            );
    }

    //*************************************************************************
    //  Method: TryGetValue()
    //
    /// <summary>
    /// Attempts to get a Boolean value from the collection.
    /// </summary>
    ///
    /// <param name="key">
    /// The key to get.
    /// </param>
    ///
    /// <param name="value">
    /// Where the value gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the collection contains a Boolean value in the specified key.
    /// </returns>
    //*************************************************************************

    public Boolean
    TryGetValue
    (
        String key,
        out Boolean value
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(key) );
        AssertValid();

        value = false;
        String sValue;

        // Note that there is no overload of Boolean.TryParse() that takes a
        // CultureInfo object, but it uses the culture-invariant culture.
        // That's what we want.

        return (
            this.TryGetValue(key, out sValue)
            &&
            Boolean.TryParse(sValue, out value)
            );
    }

    //*************************************************************************
    //  Method: ToString()
    //
    /// <summary>
    /// Saves the collection of key/value pairs to a string.
    /// </summary>
    ///
    /// <returns>
    /// The collection as a delimited string.
    /// </returns>
    ///
    /// <remarks>
    /// Key/value pairs for which the value is empty or null are NOT saved to
    /// the string.
    ///
    /// <para>
    /// A new collection can be created from the returned string using the
    /// static <see cref="FromString" /> method.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public new String
    ToString()
    {
        AssertValid();

        StringBuilder oStringBuilder = new StringBuilder();

        foreach (KeyValuePair<String, String> oKeyValuePair in this)
        {
            String sValue = oKeyValuePair.Value;

            if ( !String.IsNullOrEmpty(sValue) )
            {
                if (oStringBuilder.Length > 0)
                {
                    oStringBuilder.Append(KeyValuePairSeparator);
                }

                oStringBuilder.Append(oKeyValuePair.Key);
                oStringBuilder.Append(KeyValueSeparator);
                oStringBuilder.Append(sValue);
            }
        }

        return ( oStringBuilder.ToString() );
    }

    //*************************************************************************
    //  Method: FromString()
    //
    /// <summary>
    /// Loads a new collection of key/value pairs from a string.
    /// </summary>
    ///
    /// <param name="attributes">
    /// The collection as a string.  Can be empty but not null.
    /// </param>
    ///
    /// <returns>
    /// A new collection.
    /// </returns>
    ///
    /// <remarks>
    /// <paramref name="attributes" /> should be a string that was returned
    /// from <see cref="ToString" />.
    /// </remarks>
    //*************************************************************************

    public static PersistableStringDictionary
    FromString
    (
        String attributes
    )
    {
        Debug.Assert(attributes != null);

        PersistableStringDictionary oPersistableStringDictionary =
            new PersistableStringDictionary();

        PopulateDictionary(oPersistableStringDictionary, attributes);

        return (oPersistableStringDictionary);
    }

    //*************************************************************************
    //  Method: PopulateDictionary()
    //
    /// <summary>
    /// For derived classes only.  Populates an existing collection of
    /// key/value pairs from a string.
    /// </summary>
    ///
    /// <param name="dictionary">
    /// The dictionary to populate.
    /// </param>
    ///
    /// <param name="attributes">
    /// The collection as a string.  Can be empty but not null.
    /// </param>
    ///
    /// <remarks>
    /// This is meant for use only by derived implementations of FromString().
    /// It's a workaround for the fact that if DerivedClass is derived from
    /// PersistableStringDictionary, the DerivedClass.FromString() method can't
    /// call the base-class FromString() method and cast the return value to a
    /// DerivedClass object.  Such a cast throws an exception at runtime.
    /// Instead, the derived method can create its own DerivedClass object,
    /// then populate it using this method.
    /// </remarks>
    //*************************************************************************

    public static void
    PopulateDictionary
    (
        Dictionary<String, String> dictionary,
        String attributes
    )
    {
        Debug.Assert(attributes != null);

        // Be forgiving of bad strings.

        foreach ( String sKeyValuePair in attributes.Split(
            KeyValuePairSeparator, StringSplitOptions.RemoveEmptyEntries) )
        {
            String [] asKeyAndValue =
                sKeyValuePair.Split(KeyValueSeparator,
                    StringSplitOptions.RemoveEmptyEntries);

            if (asKeyAndValue.Length == 2)
            {
                dictionary[ asKeyAndValue[0] ] = asKeyAndValue[1];
            }
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

    public virtual void
    AssertValid()
    {
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Separator between one key/value pair and the next.

    protected static readonly Char[] KeyValuePairSeparator =
        StringUtil.FieldSeparator;

    /// Separator between a key and its value.

    protected static readonly Char[] KeyValueSeparator =
        StringUtil.SubFieldSeparator;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
