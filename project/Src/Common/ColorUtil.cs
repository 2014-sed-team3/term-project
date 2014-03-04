using System;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Smrf.GraphicsLib
{
//*****************************************************************************
//  Class: ColorUtil
//
/// <summary>
/// Utility methods for working with colors.
/// </summary>
///
/// <remarks>
/// This class adds functionality to the System.Drawing.Color class.  A better
/// design would have a new ColorPlus class inherit from Color, with new
/// methods added to ColorPlus, but because the Color class is sealed, that
/// isn't possible.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class ColorUtil
{
    //*************************************************************************
    //  Method: GetUniqueColor()
    //
    /// <summary>
    /// Gets one of a series of unique colors.
    /// </summary>
    ///
    /// <param name="index">
    /// The index of the unique color within the series.  Must be greater than
    /// or equal to zero.
    /// </param>
    ///
    /// <param name="totalUniqueColors">
    /// The total number of unique color within the series.  Must be greater
    /// than zero.
    /// </param>
    ///
    /// <returns>
    /// One unique color in a series.
    /// </returns>
    ///
    /// <remarks>
    /// This method is meant to be called <paramref name="totalUniqueColors" />
    /// times to obtain a series of unique colors.
    ///
    /// <para>
    /// This method cycles through a fixed set of colors, then repeats the set
    /// as many times as necessary with increasing luminance components.
    /// </para>
    ///
    /// <para>
    /// The number of colors in the fixed set is <see
    /// cref="UniqueColorHues" />.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public static Color
    GetUniqueColor
    (
        Int32 index,
        Int32 totalUniqueColors
    )
    {
        Debug.Assert(index >= 0);
        Debug.Assert(totalUniqueColors > 0);

        // Get a hue/luminance pair.

        HueAndLuminance oHueLuminancePair =
            HueLuminancePairs[index % UniqueColorHues];

        // Figure out the luminance to use.  This may be larger than the
        // luminance specified in the hue/luminance pair.

        Int32 iLuminances = (Int32)Math.Ceiling(
            (Single)totalUniqueColors / (Single)(UniqueColorHues) );

        Single fMinimumLuminance = oHueLuminancePair.Luminance;
        Single fLuminance;

        if (iLuminances == 1)
        {
            fLuminance = fMinimumLuminance;
        }
        else
        {
            Int32 iLuminanceIndex = index / UniqueColorHues;

            fLuminance = fMinimumLuminance
                + ( (Single)iLuminanceIndex /  (Single)(iLuminances - 1) )
                * (MaximumLuminance - fMinimumLuminance);
        }

        // Convert the HLS values to a Color.

        return ( ColorTranslator.FromWin32(ColorHLSToRGB(
            (Int32)oHueLuminancePair.Hue,
            (Int32)fLuminance,
            (Int32)Saturation
            ) ) );
    }

    //*************************************************************************
    //  Method: TryConvertFromInvariantString()
    //
    /// <summary>
    /// Attempts to convert a string to a color.
    /// </summary>
    ///
    /// <param name="theString">
    /// The string that might contain a color.  Valid colors are those
    /// recognized by the ColorConverter class, as well as named colors that
    /// have spaces in them.  Both "LightBlue" and "LightBlue" are valid
    /// colors, for example.
    /// </param>
    ///
    /// <param name="colorConverter">
    /// The ColorConverter object to use.
    /// </param>
    ///
    /// <param name="color">
    /// Where the converted color gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if <paramref name="theString" /> contains a valid color.
    /// </returns>
    ///
    /// <remarks>
    /// If <paramref name="theString" /> contains a valid color, the converted
    /// color gets stored at <paramref name="color" /> and true is returned.
    /// Otherwise, false is returned.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryConvertFromInvariantString
    (
        String theString,
        ColorConverter colorConverter,
        out Color color
    )
    {
        Debug.Assert(colorConverter != null);
        Debug.Assert(theString != null);

        color = Color.Empty;

        // Remove spaces in named colors, so that "Light Blue" becomes
        // "LightBlue", for example.

        theString = theString.Replace(" ", String.Empty);

        if (theString.Length == 0)
        {
            // ColorConverter converts an empty string to Color.Black.  Bypass
            // ColorConverter.

            return (false);
        }

        try
        {
            color = (Color)colorConverter.ConvertFromInvariantString(
                theString);
        }
        catch (Exception)
        {
            // (Format errors raise a System.Exception with an inner exception
            // of type FormatException.  Go figure.)

            return (false);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ToHtmlString()
    //
    /// <summary>
    /// Converts a Color to an HTML string in the format #rrggbb.
    /// </summary>
    ///
    /// <param name="color">
    /// The color to convert.
    /// </param>
    ///
    /// <returns>
    /// The color, in #rrggbb format.
    /// </returns>
    //*************************************************************************

    public static String
    ToHtmlString
    (
        Color color
    )
    {
        return ( String.Format(
            "#{0:x2}{1:x2}{2:x2}"
            ,
            color.R,
            color.G,
            color.B
            ) );
    }

    //*************************************************************************
    //  Method: ColorHLSToRGB()
    //
    /// <summary>
    /// Windows API, converts an HLS color to the RGB color space.
    /// </summary>
    ///
    /// <param name="wHue">
    /// Hue component.  Ranges from 0 to 240.
    /// </param>
    ///
    /// <param name="wLuminance">
    /// Luminance component.  Ranges from 0 to 240, where 0 represents black
    /// and 240 represents white.
    /// </param>
    ///
    /// <param name="wSaturation">
    /// Saturation component.  Ranges from 0 to 240, where 0 is grayscale and
    /// 240 is the most saturated.
    /// </param>
    ///
    /// <returns>
    /// RGB color.
    /// </returns>
    //*************************************************************************

    [DllImport("shlwapi.dll")]

    private static extern Int32
    ColorHLSToRGB
    (
        Int32 wHue,
        Int32 wLuminance,
        Int32 wSaturation
    );


    //*************************************************************************
    //  Private structures
    //*************************************************************************

    /// Hue/luminance pairs.

    private struct HueAndLuminance
    {
        /// Hue ranges from 0 to 240.

        public Int32 Hue;

        /// Luminance ranges from 0 (black) to 240 (white).

        public Int32 Luminance;

        public HueAndLuminance
        (
            Int32 hue,
            Int32 luminance
        )
        {
            Hue = hue;
            Luminance = luminance;
        }
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Hue/luminance pairs used for the first set of colors returned by
    /// GetUniqueColors().  The saturation component for each color is 240.

    private static HueAndLuminance [] HueLuminancePairs =
    {
        new HueAndLuminance(155,  45),
        new HueAndLuminance(136, 107),
        new HueAndLuminance(100,  47),
        new HueAndLuminance( 85,  83),
        new HueAndLuminance(  0,  90),
        new HueAndLuminance( 21, 108),
        new HueAndLuminance( 30, 120),
        new HueAndLuminance( 50,  94),
        new HueAndLuminance(216,  94),
        new HueAndLuminance(192,  45),
        new HueAndLuminance(179,  90),
        new HueAndLuminance(130,  61),
    };

    /// Saturation to use for all colors.  Saturation ranges from 0 to 240,
    /// where 0 is grayscale and 240 is the most saturated.

    private const Single Saturation = 240F;

    /// Maximum luminance to use.  Don't get too close to white, which would
    /// prevent bright colors from being distinguishable from one another.

    private const Single MaximumLuminance = 220F;


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// Number of unique hues used by <see cref="GetUniqueColor" />.
    /// </summary>

    public static readonly Int32 UniqueColorHues = HueLuminancePairs.Length;
}

}
