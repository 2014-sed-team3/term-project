
using System;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: SaveableImageFormats
//
/// <summary>
/// Contains information about image formats that can be saved to disk.
/// </summary>
///
/// <remarks>
/// This class encapsulates information about image formats that can be saved
/// to disk.  All properties and methods are static.
/// </remarks>
//*****************************************************************************

public static class SaveableImageFormats : Object
{
    //*************************************************************************
    //  Method: GetFileExtension()
    //
    /// <summary>
    /// Returns a file extension corresponding to a saveable image format.
    /// </summary>
    ///
    /// <param name="saveableImageFormat">
    /// One of the ImageFormat values returned by <cref name="ImageFormats" />.
    /// </param>
    ///
    /// <returns>
    /// The returned file extension is in lower case and does not include a
    /// period.  Sample: "bmp".
    /// </returns>
    //*************************************************************************

    public static String
    GetFileExtension
    (
        ImageFormat saveableImageFormat
    )
    {
        Int32 iIndex = Array.IndexOf(ImageFormats, saveableImageFormat);

        if (iIndex == -1)
        {
            throw new Exception(
                "SaveableImageFormats.GetFileExtension: No such format.");
        }

        return ( FileExtensions[iIndex] );
    }

    //*************************************************************************
    //  Method: InitializeListControl()
    //
    /// <summary>
    /// Initializes a ListControl with image formats that can be saved to disk.
    /// </summary>
    ///
    /// <param name="listControl">
    /// ListControl to initialize.
    /// </param>
    ///
    /// <remarks>
    /// After this method is called, the <see
    /// cref="ListControl.SelectedValue" /> property can be used to get the
    /// ImageFormat value selected by the user.
    /// </remarks>
    //*************************************************************************

    public static void
    InitializeListControl
    (
        ListControl listControl
    )
    {
        Debug.Assert(listControl != null);

        ListControlUtil.PopulateWithObjectsAndText(listControl,
            new Object [] {

            ImageFormat.Bmp, "BMP (.bmp)",
            ImageFormat.Gif, "GIF (.gif)",
            ImageFormat.Jpeg, "JPEG (.jpg)",
            ImageFormat.Png, "PNG (.png)",
            ImageFormat.Tiff, "TIFF (.tif)"
            }

            );

        listControl.SelectedValue = ImageFormat.Jpeg;
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// <summary>
    /// An array of image formats that can be saved to disk.
    /// </summary>

    public static readonly ImageFormat[] ImageFormats =
    {
        ImageFormat.Bmp,
        ImageFormat.Gif,
        ImageFormat.Jpeg,
        ImageFormat.Png,
        ImageFormat.Tiff
    };

    /// <summary>
    /// A filter string for the image formats that can be saved to disk,
    /// suitable for use in a common dialog.
    /// </summary>

    public static readonly String Filter =
        "BMP (*.bmp)|*.bmp|" +
        "GIF (*.gif)|*.gif|" +
        "JPEG (*.jpg)|*.jpg;*.jpeg;*.jfif|" +
        "PNG (*.png)|*.png|" +
        "TIFF (*.tif)|*.tif;*.tiff"
        ;

    /// <summary>
    /// One-based index of the PNG filter returned by <see cref="Filter" />.
    /// </summary>

    public static readonly Int32 PngFilterIndexOneBased = 4;


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// <summary>
    /// File extensions that correspond to <see cref="ImageFormats" />.
    /// </summary>

    private static readonly String [] FileExtensions =
    {
        "bmp",
        "gif",
        "jpg",
        "png",
        "tif"
    };
}

}
