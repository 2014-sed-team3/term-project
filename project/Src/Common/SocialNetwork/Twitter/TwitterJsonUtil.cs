
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Smrf.SocialNetworkLib.Twitter
{
//*****************************************************************************
//  Class: TwitterJsonUtil
//
/// <summary>
/// Provides utility methods for parsing JSON received from Twitter.
/// </summary>
//*****************************************************************************

public static class TwitterJsonUtil
{
    //*************************************************************************
    //  Method: TryGetJsonValueFromDictionary()
    //
    /// <overloads>
    /// Attempts to get a value from a JSON value dictionary.
    /// </overloads>
    ///
    /// <summary>
    /// Attempts to get a non-empty string value from a JSON value dictionary.
    /// </summary>
    ///
    /// <param name="valueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.
    /// </param>
    ///
    /// <param name="name">
    /// The name of the value to get.
    /// </param>
    ///
    /// <param name="value">
    /// Where the non-empty value gets stored if true is returned.  If false is
    /// returned, this gets set to null.
    /// </param>
    ///
    /// <returns>
    /// true if the non-empty value was obtained.
    /// </returns>
    ///
    /// <remarks>
    /// This method attempts to get the specified value, which can be of any
    /// JSON type, from <paramref name="valueDictionary" />.  If the value is
    /// found, this method converts it to a string, stores it at <paramref
    /// name="value" />, and returns true.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetJsonValueFromDictionary
    (
        Dictionary<String, Object> valueDictionary,
        String name,
        out String value
    )
    {
        Debug.Assert(valueDictionary != null);
        Debug.Assert( !String.IsNullOrEmpty(name) );

        Object valueAsObject;

        if ( valueDictionary.TryGetValue(name, out valueAsObject) )
        {
            return ( TryConvertJsonValueToString(valueAsObject, out value) );
        }

        value = null;
        return (false);
    }

    //*************************************************************************
    //  Method: TryGetJsonValueFromDictionary()
    //
    /// <summary>
    /// Attempts to get an Int64 value from a JSON value dictionary.
    /// </summary>
    ///
    /// <param name="valueDictionary">
    /// Name/value pairs parsed from a Twitter JSON response.
    /// </param>
    ///
    /// <param name="name">
    /// The name of the value to get.
    /// </param>
    ///
    /// <param name="value">
    /// Where the value gets stored if true is returned.  If false is returned,
    /// this gets set to Int64.MinValue.
    /// </param>
    ///
    /// <returns>
    /// true if the value was obtained.
    /// </returns>
    ///
    /// <remarks>
    /// This method attempts to get the specified value, which must be of type
    /// Int64, from <paramref name="valueDictionary" />.  If the value is
    /// found, it gets stored at <paramref name="value" /> and true is
    /// returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetJsonValueFromDictionary
    (
        Dictionary<String, Object> valueDictionary,
        String name,
        out Int64 value
    )
    {
        Debug.Assert(valueDictionary != null);
        Debug.Assert( !String.IsNullOrEmpty(name) );

        Object valueAsObject;

        if (
            valueDictionary.TryGetValue(name, out valueAsObject)
            &&
            valueAsObject is Int64
            )
        {
            value = (Int64)valueAsObject;
            return (true);
        }

        value = Int64.MinValue;
        return (false);
    }

    //*************************************************************************
    //  Method: TryConvertJsonValueToString()
    //
    /// <summary>
    /// Attempts to convert a JSON value to a non-empty string.
    /// </summary>
    ///
    /// <param name="value">
    /// A JSON value to convert.  Can be null.
    /// </param>
    ///
    /// <param name="valueAsString">
    /// Where the non-empty value gets stored if true is returned.  If false is
    /// returned, this gets set to null.
    /// </param>
    ///
    /// <returns>
    /// true if the value was converted.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="value" /> is not null and can be converted to a
    /// non-empty string, the string gets stored at <paramref
    /// name="valueAsString" /> and true is returned.  false is returned
    /// otherwise.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryConvertJsonValueToString
    (
        Object value,
        out String valueAsString
    )
    {
        if (value != null)
        {
            valueAsString = value.ToString();

            if (valueAsString.Length > 0)
            {
                return (true);
            }
        }

        valueAsString = null;
        return (false);
    }
}

}
