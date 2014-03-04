using System;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: CollectionUtil
//
/// <summary>
/// Utility methods for working with collections.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class CollectionUtil
{
    //*************************************************************************
    //  Method: GetDictionaryKey()
    //
    /// <summary>
    /// Combines two unique Int32 numbers into an Int64 suitable for use as a
    /// dictionary key, optionally taking the order of the numbers into
    /// account.
    /// </summary>
    ///
    /// <param name="uniqueNumber1">
    /// The first unique Int32.
    /// </param>
    ///
    /// <param name="uniqueNumber2">
    /// The second unique Int32.
    /// </param>
    ///
    /// <param name="useOrder">
    /// true to take the order of the numbers into account.
    /// </param>
    ///
    /// <returns>
    /// An Int64 suitable for use as a dictionary key.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="useOrder" /> is false, the number pairs (A,B) and
    /// (B,A) yield the same Int64.
    ///
    /// <para>
    /// If <paramref name="useOrder" /> is true, the number pairs (A,B) and
    /// (B,A) yield different Int64s.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Int64
    GetDictionaryKey
    (
        Int32 uniqueNumber1,
        Int32 uniqueNumber2,
        Boolean useOrder
    )
    {
        // In the unordered case, guarantee that (A,B) and (B,A) are considered
        // duplicates by always combining them in the same order.

        Int64 i64HighBits, i64LowBits;

        if (useOrder || uniqueNumber1 < uniqueNumber2)
        {
            i64HighBits = (Int64)uniqueNumber1;
            i64LowBits = (Int64)uniqueNumber2;
        }
        else
        {
            i64HighBits = (Int64)uniqueNumber2;
            i64LowBits = (Int64)uniqueNumber1;
        }

        return ( (i64HighBits << 32) + i64LowBits );
    }

    //*************************************************************************
    //  Method: ParseDictionaryKey()
    //
    /// <summary>
    /// Retrieves the two unique Int32 numbers that were combined into an Int64
    /// by <see cref="GetDictionaryKey" />.
    /// </summary>
    ///
    /// <param name="dictionaryKey">
    /// The dictionary key returned by <see cref="GetDictionaryKey" />.
    /// </param>
    ///
    /// <param name="parsedNumber1">
    /// Where the first parsed Int32 gets stored.
    /// </param>
    ///
    /// <param name="parsedNumber2">
    /// Where the second parsed Int32 gets stored.
    /// </param>
    ///
    /// <remarks>
    /// Note that the parsed numbers may be in a different order from the
    /// unique numbers that were passed to <see cref="GetDictionaryKey" />.
    /// They are guaranteed to be in the same order only if the useOrder
    /// argument was true.
    /// </remarks>
    //*************************************************************************

    public static void
    ParseDictionaryKey
    (
        Int64 dictionaryKey,
        out Int32 parsedNumber1,
        out Int32 parsedNumber2
    )
    {
        parsedNumber1 = (Int32)(dictionaryKey >> 32);
        parsedNumber2 = (Int32)(dictionaryKey);
    }
}

}
