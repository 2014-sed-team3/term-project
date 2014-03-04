
using System;
using System.Text;
using System.Globalization;
using System.Diagnostics;

namespace Smrf.DateTimeLib
{
//*****************************************************************************
//  Class: DateTimeUtil2
//
/// <summary>
/// Static utility methods involving dates and times.
/// </summary>
///
/// <remarks>
/// This is a replacement for DateTimeUtil, which should not be used in new
/// projects.
/// </remarks>
//*****************************************************************************

public static class DateTimeUtil2
{
    //*************************************************************************
    //  Method: ToCultureInvariantString()
    //
    /// <summary>
    /// Converts a DateTime to a culture-invariant string.
    /// </summary>
    ///
    /// <param name="dateTime">
    /// The date to convert.
    /// </param>
    ///
    /// <returns>
    /// The date as a culture-invariant UTC string.  Sample:
    /// "2006-04-17 21:22:48".  Note the lack of a terminating "Z".
    /// </returns>
    ///
    /// <remarks>
    /// The returned string can be parsed back to DateTime using <see
    /// cref="FromCultureInvariantString" />.
    /// </remarks>
    //*************************************************************************

    public static String
    ToCultureInvariantString
    (
        DateTime dateTime
    )
    {
        // Start with the the "UniversalSortableDateTimePattern," which
        // produces this:
        //
        // 2006-04-17 21:22:48Z
        //
        // ...and then remove the "Z".
        //
        // 2006-04-17 21:22:48

        return ( dateTime.ToString("u").Replace("Z", String.Empty) );
    }

    //*************************************************************************
    //  Method: FromCultureInvariantString()
    //
    /// <summary>
    /// Parses a DateTime from a string returned by <see
    /// cref="ToCultureInvariantString" />.
    /// </summary>
    ///
    /// <param name="cultureInvariantString">
    /// The string returned by <see cref="ToCultureInvariantString" />.
    /// </param>
    ///
    /// <returns>
    /// The parsed DateTime.
    /// </returns>
    //*************************************************************************

    public static DateTime
    FromCultureInvariantString
    (
        String cultureInvariantString
    )
    {
        // ToCultureInvariantString() removed the "Z".  Add it back before
        // parsing the string.

        return ( DateTime.ParseExact(cultureInvariantString + "Z", "u",
            CultureInfo.InvariantCulture) );
    }

    //*************************************************************************
    //  Method: ToCultureInvariantFileName()
    //
    /// <summary>
    /// Creates a file name based on a DateTime, in a culture-invariant format.
    /// </summary>
    ///
    /// <param name="dateTime">
    /// The date to convert.
    /// </param>
    ///
    /// <returns>
    /// A file name, without a path or extension, using the specified DateTime
    /// in local time.  Sample: "2006-04-17 21-22-48".
    /// </returns>
    //*************************************************************************

    public static String
    ToCultureInvariantFileName
    (
        DateTime dateTime
    )
    {
        return ( dateTime.ToString("yyyy-MM-dd HH-mm-ss") );
    }

    //*************************************************************************
    //  Method: RemoveTime()
    //
    /// <summary>
    /// Copies a DateTime and sets the copy's time to 12:00 AM.
    /// </summary>
    ///
    /// <param name="dateTime">
    /// DateTime to copy.  Does not get modified.
    /// </param>
    ///
    /// <returns>
    /// A copy of <paramref name="dateTime" /> with the time set to 12:00 AM.
    /// </returns>
    //*************************************************************************

    public static DateTime
    RemoveTime
    (
        DateTime dateTime
    )
    {
        return ( new DateTime(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            0,
            0,
            0,
            dateTime.Kind
            ) );
    }

    //*************************************************************************
    //  Method: FormatDuration()
    //
    /// <summary>
    /// Formats the duration of a period of time.
    /// </summary>
    ///
    /// <param name="period">
    /// The period to format.  Must be greater than or equal to zero.
    /// </param>
    ///
    /// <returns>
    /// Samples: "2-day, 23-hour, 4-minute", "14-hour, 12-minute", "0-minute".
    /// </returns>
    ///
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="period" /> is negative.
    /// </exception>
    //*************************************************************************

    public static String
    FormatDuration
    (
        TimeSpan period
    )
    {
        if (period.Ticks < 0)
        {
            throw new ArgumentException();
        }

        StringBuilder oDuration = new StringBuilder();
        Boolean bAlwaysAppendRemainingComponents = false;

        bAlwaysAppendRemainingComponents = AppendDurationComponent(
            period.Days, "day", bAlwaysAppendRemainingComponents, oDuration);

        AppendDurationComponent(period.Hours, "hour",
            bAlwaysAppendRemainingComponents, oDuration);

        bAlwaysAppendRemainingComponents = true;

        AppendDurationComponent(period.Minutes, "minute",
            bAlwaysAppendRemainingComponents, oDuration);

        return ( oDuration.ToString() );
    }

    //*************************************************************************
    //  Method: UnixTimestampToDateTimeUtc()
    //
    /// <summary>
    /// Converts a Unix timestamp to a DateTime.
    /// </summary>
    ///
    /// <param name="unixTimestampUtc">
    /// Unix timestamp to convert, in UTC.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="unixTimestampUtc" /> converted to a DateTime, in UTC.
    /// </returns>
    //*************************************************************************

    public static DateTime
    UnixTimestampToDateTimeUtc
    (
        UInt32 unixTimestampUtc
    )
    {
        // A Unix timestamp is the number of seconds since 1/1/1970.

        DateTime oDateTime = new DateTime(1970, 1, 1, 0, 0, 0,
            DateTimeKind.Utc);

        return ( oDateTime.AddSeconds(unixTimestampUtc) );
    }

    //*************************************************************************
    //  Method: AppendDurationComponent()
    //
    /// <summary>
    /// Appends a component of a duration to a StringBuilder.
    /// </summary>
    ///
    /// <param name="iComponent">
    /// A TimeSpan component, such as number of days.
    /// </param>
    ///
    /// <param name="sComponentDescription">
    /// A description of the component, such as "day".
    /// </param>
    ///
    /// <param name="bAlwaysAppend">
    /// true to always append the component, false to append the component only
    /// if it's greater than zero.
    /// </param>
    ///
    /// <param name="oDuration">
    /// The StringBuilder to append the component to.
    /// </param>
    ///
    /// <remarks>
    /// true if the component was appended.
    /// </remarks>
    //*************************************************************************

    private static Boolean
    AppendDurationComponent
    (
        Int32 iComponent,
        String sComponentDescription,
        Boolean bAlwaysAppend,
        StringBuilder oDuration
    )
    {
        Debug.Assert(iComponent >= 0);
        Debug.Assert( !String.IsNullOrEmpty(sComponentDescription) );
        Debug.Assert(oDuration != null);

        if (bAlwaysAppend || iComponent > 0)
        {
            oDuration.AppendFormat(
                "{0}{1}-{2}"
                ,
                oDuration.Length > 0 ? ", " : String.Empty,
                iComponent,
                sComponentDescription
                );

            return (true);
        }

        return (false);
    }
}

}
