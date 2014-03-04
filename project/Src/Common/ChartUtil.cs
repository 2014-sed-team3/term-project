
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Smrf.ChartLib
{
//*****************************************************************************
//  Class: ChartUtil
//
/// <summary>
/// Static utility methods that can be used in the implementation of charting
/// components.
/// </summary>
//*****************************************************************************

static class ChartUtil
{
    //*************************************************************************
    //  Method: GetAxisGridlineValues()
    //
    /// <summary>
    /// Gets an array of Double values for drawing axis gridlines on a linear
    /// axis, along with the number of decimal places to use when formatting
    /// the values.
    /// </summary>
    ///
    /// <param name="limit1">
    /// Value that will be displayed at the left end of the axis.
    /// </param>
    ///
    /// <param name="limit2">
    /// Value that will be displayed at the right end of the axis.  This does
    /// not have to be greater than <paramref name="limit1" />.
    /// </param>
    ///
    /// <param name="gridlineValues">
    /// Where the array of values to draw gridlines for gets stored.  Sample
    /// arrays: {0, 20, 40, 60, 80, 100}, {-3.5, -3.0, -2.5},
    /// {0.8, 0.9, 1.0, 1.1, 1.2}.  The values are always in increasing order,
    /// even if <paramref name="limit2" /> is less than <paramref
    /// name="limit1" />.
    /// </param>
    ///
    /// <param name="decimalPlacesToShow">
    /// Where the number of decimal places to use when formatting the gridline
    /// values gets stored.
    /// </param>
    ///
    /// <remarks>
    /// Given a linear chart axis that will display a specified range of
    /// values, this method returns an array of values for which gridlines
    /// should be drawn.  The values are computed in such a way that 1) the
    /// minimum gridline value is less than or equal to the lesser of <paramref
    /// name="limit1" /> and <paramref name="limit2" />; 2) the maximum
    /// gridline value is greater than or equal to the larger of <paramref
    /// name="limit2" /> and <paramref name="limit1" />; 3) the number of
    /// gridline values is as close to 6 as possible, which makes the number of
    /// axis intervals as close to 5 as possible; and 4) the difference between
    /// gridlines values is n*2, n*5, or n*10, where n is a power of 10.
    /// </remarks>
    //*************************************************************************

    public static void
    GetAxisGridlineValues
    (
        Double limit1,
        Double limit2,
        out Double [] gridlineValues,
        out Int32 decimalPlacesToShow
    )
    {
        gridlineValues = null;
        decimalPlacesToShow = 0;

        Double dMinimum, dMaximum;

        if (limit1 < limit2)
        {
            dMinimum = limit1;
            dMaximum = limit2;
        }
        else
        {
            dMinimum = limit2;
            dMaximum = limit1;
        }

        // Handle the one-value case, for which the maximum and minimum are the
        // same.  We don't want an axis of zero width.

        if (dMinimum == dMaximum)
        {
            dMaximum = (dMinimum == 0) ? 1.0F : (dMinimum + 1F);
        }

        // For the comments below, assume that dMinimum = -2.0 and
        // dMaximum = 103.0.

        // Compute the range.  Sample: 105.0.

        Double dRange = dMaximum - dMinimum;

        // Get the log of the range.  Sample: 2.02.

        Double dLogRange = Math.Log10(dRange);

        // Truncate the log and raise the result to a power of 10.  This gives
        // a factor that will be applied below to the series 1, 2, and 5.
        // Sample factor: 100.

        Double dFactor = Math.Pow(10F, (Int32)dLogRange);

        // For this sample, the range should be divided by 100, 200, and 500 to
        // determine which one yields an interval count closest to 5.  For
        // better results in some cases, also try 10, 20, 50, 1000, 2000,
        // and 5000.

        Double [] adTestDivisors = new Double []
        {
             1.0F,  2.0F,  5.0F,
             0.1F,  0.2F,  0.5F,
            10.0F, 20.0F, 50.0F 
        };

        Double dBestRangePerInterval = Double.MinValue;
        Int32 iBestNumberOfIntervals = Int32.MinValue;

        foreach (Double dTestDivisor in adTestDivisors)
        {
            // Get the range per interval and number of intervals for this
            // divisor.

            Double dTestRangePerInterval = dFactor / dTestDivisor;

            Int32 iTestNumberOfIntervals =
                (Int32)Math.Ceiling(dRange / dTestRangePerInterval);

            // Is the number of intervals closer to 5 than the best case?

            if ( Math.Abs(iTestNumberOfIntervals - 5) <
                Math.Abs(iBestNumberOfIntervals - 5) )
            {
                // Yes.  This is the new best case.

                dBestRangePerInterval = dTestRangePerInterval;
                iBestNumberOfIntervals = iTestNumberOfIntervals;
            }
        }

        // Sample results: dBestRangePerInterval = 20,
        // iBestNumberOfIntervals = 5.

        // Get the first gridline value.  Sample: -20.0.

        Double dFirstGridlineValue =
            (Math.Floor(dMinimum / dBestRangePerInterval)
            * dBestRangePerInterval);

        // Get all the values.  Sample: -20, 0, 20, ..., 120.

        LinkedList<Double> oGridlineValues = new LinkedList<Double>();
        Double dGridlineValue = dFirstGridlineValue;

        while (true)
        {
            oGridlineValues.AddLast(dGridlineValue);

            if (dMaximum <= dGridlineValue)
            {
                break;
            }

            dGridlineValue += dBestRangePerInterval;
        }

        Debug.Assert(oGridlineValues.Count >= 2);

        gridlineValues = oGridlineValues.ToArray();

        // Get the decimal part of the range.

        Double dDecimalPartOfRangePerInterval = Math.Abs(
            dBestRangePerInterval - Math.Truncate(dBestRangePerInterval)
            );

        // Figure out how many decimal places it takes to show the interval.

        if (dDecimalPartOfRangePerInterval == 0)
        {
            decimalPlacesToShow = 0;
        }
        else
        {
            decimalPlacesToShow = -(Int32)Math.Floor(
                Math.Log10(dDecimalPartOfRangePerInterval)
                );
        }
    }

    //*************************************************************************
    //  Method: GetLogAxisGridlineValues()
    //
    /// <summary>
    /// Gets an array of values for drawing axis gridlines on a log axis.
    /// </summary>
    ///
    /// <param name="limit1">
    /// Value that will be displayed at the left end of the axis.
    /// </param>
    ///
    /// <param name="limit2">
    /// Value that will be displayed at the right end of the axis.  This does
    /// not have to be greater than <paramref name="limit1" />.
    /// </param>
    ///
    /// <returns>
    /// Array of values to draw gridlines for.  Sample arrays: {1, 10, 100},
    /// {0.1, 1.0, 10}.
    /// </returns>
    ///
    /// <remarks>
    /// Given a log chart axis that will display a specified range of values,
    /// this method returns an array of values for which gridlines should be
    /// drawn.  The values are computed in such a way that 1) the minimum
    /// gridline value is the largest power of 10 that is less than or equal to
    /// the lesser of <paramref name="limit1" /> and <paramref
    /// name="limit2" />; 2) the maximum gridline value is the smallest power
    /// of 10 that is greater than or equal to the larger of <paramref
    /// name="limit1" /> and <paramref name="limit2" />; 3) there is a gridline
    /// for each power of 10 in between the minimum and maximum gridline
    /// values.
    /// </remarks>
    //*************************************************************************

    public static Double []
    GetLogAxisGridlineValues
    (
        Double limit1,
        Double limit2
    )
    {
        if (limit1 <= 0)
        {
            throw new InvalidOperationException(
                "ChartUtil.GetLogAxisGridlineValues: limit1 value must be"
                + " > 0 when using log scaling.");
        }

        if (limit2 <= 0)
        {
            throw new InvalidOperationException(
                "ChartUtil.GetLogAxisGridlineValues: limit2 value must be"
                + " > 0 when using log scaling.");
        }

        Double dMinimum, dMaximum;

        if (limit1 < limit2)
        {
            dMinimum = limit1;
            dMaximum = limit2;
        }
        else
        {
            dMinimum = limit2;
            dMaximum = limit1;
        }

        // For the comments below, assume that dMinimum = 0.3 and
        // dMaximum = 103.0.

        // Get the first gridline value.  Sample: 0.1.

        Double dFirstGridlineValue =
            Math.Pow( 10.0, Math.Floor( Math.Log10(dMinimum) ) );

        // Get the last gridline value.  Sample: 1000.0.

        Double dLastGridlineValue =
            Math.Pow( 10.0, Math.Ceiling( Math.Log10(dMaximum) ) );

        // Handle the one-value case, for which the maximum and minimum are the
        // same.  We don't want an axis of zero width.

        if (dLastGridlineValue == dFirstGridlineValue)
        {
            dLastGridlineValue *= 10.0;
        }

        Debug.Assert(dLastGridlineValue > dFirstGridlineValue);
        Debug.Assert(dFirstGridlineValue != 0);

        // Compute the total number of gridline values.  Sample: 5.

        Int32 iGridlineValues = 1 +
            (Int32)Math.Log10(dLastGridlineValue / dFirstGridlineValue);

        Debug.Assert(iGridlineValues >= 2);

        Double [] adGridlineValues = new Double[iGridlineValues];

        // Set the end values.

        adGridlineValues[0] = dFirstGridlineValue;
        adGridlineValues[iGridlineValues - 1] = dLastGridlineValue;

        // Fill in the middle values.

        for (Int32 i = 1; i < iGridlineValues - 1; i++)
        {
            adGridlineValues[i] = adGridlineValues[i - 1] * 10.0;
        }

        return (adGridlineValues);
    }
}

}
