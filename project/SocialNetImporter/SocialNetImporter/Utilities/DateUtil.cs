using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smrf.AppLib
{
    public static class DateUtil
    {
        /// <summary>
        /// Method for converting a System.DateTime value to a UNIX Timestamp
        /// </summary>
        /// <param name="value">date to convert</param>
        /// <returns></returns>
        public static int ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (int)span.TotalSeconds;
        }

        /// <summary>
        /// Method for converting a UNIX Timestamp to a System.DateTime value
        /// </summary>
        /// <param name="unixTimeSTamp">UNIX Timestamp to convert.</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
