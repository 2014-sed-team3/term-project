﻿
using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace Smrf.WpfGraphicsLib
{
//*****************************************************************************
//  Class: WpfImageUtil
//
/// <summary>
/// Utility methods for working with images in WPF.
/// </summary>
//*****************************************************************************

public class WpfImageUtil
{
    //*************************************************************************
    //  Constructor: WpfImageUtil()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="WpfImageUtil" /> class.
    /// </summary>
    //*************************************************************************

    public WpfImageUtil()
    {
        m_oCachedErrorImage = null;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ScreenDpi
    //
    /// <summary>
    /// Gets or sets the DPI of the screen.
    /// </summary>
    ///
    /// <value>
    /// The DPI of the screen.  The default is 96 DPI.
    /// </value>
    ///
    /// <remarks>
    /// <see cref="GetImageSynchronousIgnoreDpi" /> uses this value to set the
    /// DPI of the image it returns.
    /// </remarks>
    //*************************************************************************

    public static Double
    ScreenDpi
    {
        get
        {
            return (m_dScreenDpi);
        }

        set
        {
            m_dScreenDpi = value;
        }
    }

    //*************************************************************************
    //  Method: GetImageSynchronous()
    //
    /// <summary>
    /// Synchronously gets an image from a specified URI and handles most
    /// errors by returning an error image.
    /// </summary>
    ///
    /// <param name="uriString">
    /// The URI to get the image from.  If the string is not a valid URI, an
    /// error image is returned.
    /// </param>
    ///
    /// <returns>
    /// The specified image, or an error image if the specified image isn't
    /// available.
    /// </returns>
    ///
    /// <remarks>
    /// There are two differences between using this method and using
    /// BitmapImage(URI):
    ///
    /// <list type="number">
    ///
    /// <item><description>
    /// If the URI is an URL, the image is downloaded synchronously instead of
    /// asynchronously.
    /// </description></item>
    ///
    /// <item><description>
    /// If the image isn't available, an error image is returned instead of an
    /// exception being thrown.
    /// </description></item>
    ///
    /// </list>
    /// </remarks>
    //*************************************************************************

    public BitmapSource
    GetImageSynchronous
    (
        String uriString
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(uriString) );
        AssertValid();

        const Int32 ErrorImageWidthAndHeight = 52;
        Uri oUri;

        if ( Uri.TryCreate(uriString, UriKind.Absolute, out oUri) )
        {
            if (oUri.Scheme == Uri.UriSchemeHttp ||
                oUri.Scheme == Uri.UriSchemeHttps)
            {
                try
                {
                    return ( GetImageSynchronousHttp(oUri) );
                }
                catch (WebException)
                {
                    // (These empty catch blocks cause an error image to be
                    // returned.)
                }
                catch (ArgumentException)
                {
                    /*
                    Attempting to download a possibly corrupt image from this
                    Twitter URL:

                    http://a1.twimg.com/profile_images/1112245377/
                        Photo_on_2010-08-27_at_18.51_normal.jpg

                    raised the following exception:

                    [ArgumentException]: An invalid character was found in text
                        content.
                    at System.Windows.Media.Imaging.BitmapFrameDecode.
                        get_ColorContexts()
                    at System.Windows.Media.Imaging.BitmapImage.
                        FinalizeCreation()
                    at System.Windows.Media.Imaging.BitmapImage.EndInit()
                    */
                }
                catch (FileFormatException)
                {
                    /*
                    A user attempted to download a possibly corrupt image from
                    Twitter on 5/6/2011.  The exact URL wasn't determined.

                    The following exception was raised:

                    [FileFormatException]: The image format is unrecognized.
                    [COMException]: Exception from HRESULT: 0x88982F07 at
                    System.Windows.Media.Imaging.BitmapDecoder.
                    SetupDecoderFromUriOrStream(Uri uri, Stream stream,
                    BitmapCacheOption cacheOption, Guid& clsId, Boolean&
                    isOriginalWritable, Stream& uriStream,
                    UnmanagedMemoryStream& unmanagedMemoryStream,
                    SafeFileHandle& safeFilehandle)
                    */
                }
                catch (IOException)
                {
                    /*
                    Attempting to download a possibly corrupt image from this
                    Twitter URL:

                    http://a2.twimg.com/profile_images/1126755034/
                    8FjbVD_normal.gif

                    raised the following exception:

                    [IOException]: Cannot read from the stream.
                    [COMException]: Exception from HRESULT: 0x88982F72
                    at System.Windows.Media.Imaging.BitmapDecoder.
                    SetupDecoderFromUriOrStream(Uri uri, Stream stream,
                    BitmapCacheOption cacheOption, Guid& clsId, Boolean&
                    isOriginalWritable, Stream& uriStream,
                    UnmanagedMemoryStream& unmanagedMemoryStream,
                    SafeFileHandle& safeFilehandle)
                    ...
                    */
                }
                catch (NotSupportedException)
                {
                    /*
                    Attempting to download a possibly corrupt image from a
                    Twitter URL (didn't record the URL) raised the following
                    exception:
                    
                   [NotSupportedException]: No imaging component suitable
                       to complete this operation was found.
                   */
                }
            }
            else if (oUri.Scheme == Uri.UriSchemeFile)
            {
                try
                {
                    BitmapImage oBitmapImage = new BitmapImage(oUri);
                    WpfGraphicsUtil.FreezeIfFreezable(oBitmapImage);

                    return (oBitmapImage);
                }
                catch (IOException)
                {
                }
                catch (ArgumentException)
                {
                    // This occurs when the URI contains an invalid character,
                    // such as "<".
                }
                catch (WebException)
                {
                    // This occurs when a non-existent drive letter is used.
                }
                catch (NotSupportedException)
                {
                    // Invalid image file.
                }
                catch (UnauthorizedAccessException)
                {
                    // The URI is actually a folder, for example.
                }
            }
        }

        if (m_oCachedErrorImage == null)
        {
            m_oCachedErrorImage = CreateErrorImage(ErrorImageWidthAndHeight);
        }

        return (m_oCachedErrorImage);
    }

    //*************************************************************************
    //  Method: GetImageSynchronousIgnoreDpi()
    //
    /// <summary>
    /// Synchronously gets an image from a specified URI, handles most errors
    /// by returning an error image, and ignores the DPI of the image.
    /// </summary>
    ///
    /// <param name="uriString">
    /// The URI to get the image from.  If the string is not a valid URI, an
    /// error image is returned.
    /// </param>
    ///
    /// <returns>
    /// The specified image, or an error image if the specified image isn't
    /// available.
    /// </returns>
    ///
    /// <remarks>
    /// There are three differences between using this method and using
    /// BitmapImage(URI):
    ///
    /// <list type="number">
    ///
    /// <item><description>
    /// If the URI is an URL, the image is downloaded synchronously instead of
    /// asynchronously.
    /// </description></item>
    ///
    /// <item><description>
    /// If the image isn't available, an error image is returned instead of an
    /// exception being thrown.
    /// </description></item>
    ///
    /// <item><description>
    /// If the image is marked with a DPI of something other than the screen
    /// resolution specified by <see cref="ScreenDpi" />, the marked DPI is
    /// ignored and the returned image will have a DPI of <see
    /// cref="ScreenDpi" />.  For example, with a 96 DPI screen, a
    /// 100x100-pixel image with a DPI of 72 will be returned as a
    /// 100x100-WPF-unit image with a DPI of 96.  This will cause the returned
    /// image to be displayed as 100x100 pixels.
    /// </description></item>
    ///
    /// </list>
    /// </remarks>
    //*************************************************************************

    public BitmapSource
    GetImageSynchronousIgnoreDpi
    (
        String uriString
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(uriString) );
        AssertValid();

        BitmapSource oBitmapSource = GetImageSynchronous(uriString);

        if (oBitmapSource.DpiX != m_dScreenDpi)
        {
            // The DPI properties of BitmapSource are read-only.  To work
            // around this, copy the BitmapSource to a new BitmapSource,
            // changing the DPI in the process.  Wasteful, but there doesn't
            // seem to be a better way.

            // The formula for stride, which isn't documented in MSDN, was
            // taken from here:
            //
            // http://social.msdn.microsoft.com/Forums/en/windowswic/thread/
            // 8e2d2dbe-6819-488b-ac49-bf5235d87bc4

            Int32 iStride = ( (oBitmapSource.PixelWidth *
                oBitmapSource.Format.BitsPerPixel) + 7 ) / 8;

            // Also, see this:
            //
            // http://social.msdn.microsoft.com/Forums/en-US/wpf/thread/
            // 56364b28-1277-4be8-8d45-78788cc4f2d7/

            Byte [] abtPixels = new Byte[oBitmapSource.PixelHeight * iStride];

            oBitmapSource.CopyPixels(abtPixels, iStride, 0);

            oBitmapSource = BitmapSource.Create(oBitmapSource.PixelWidth,
                oBitmapSource.PixelHeight, m_dScreenDpi, m_dScreenDpi,
                oBitmapSource.Format, oBitmapSource.Palette, abtPixels, iStride
                );

            WpfGraphicsUtil.FreezeIfFreezable(oBitmapSource);
        }

        return (oBitmapSource);
    }

    //*************************************************************************
    //  Method: GetImageSynchronousHttp()
    //
    /// <summary>
    /// Synchronously gets an image from a specified HTTP or HTTPS URI.
    /// </summary>
    ///
    /// <param name="uri">
    /// The URI to get the image from.  Must be HTTP or HTTPS.
    /// </param>
    ///
    /// <returns>
    /// The specified image.
    /// </returns>
    ///
    /// <remarks>
    /// No error handling is performed by this method.
    ///
    /// <para>
    /// This differs from using BitmapImage(URI) in that the image is
    /// downloaded synchronously instead of asynchronously.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public BitmapImage
    GetImageSynchronousHttp
    (
        Uri uri
    )
    {
        Debug.Assert(uri != null);

        Debug.Assert(uri.Scheme == Uri.UriSchemeHttp ||
            uri.Scheme == Uri.UriSchemeHttps);

        AssertValid();

        // Talk about inefficient...
        //
        // The following code uses HttpWebRequest to synchronously download the
        // image into a List<Byte>, then passes that List as a MemoryStream to
        // BitmapImage.StreamSource.  It works, but it involves way too much
        // Byte copying.  There has to be a better way to do this, but so far I
        // haven't found one.
        //
        // In the following post...
        //
        // http://stackoverflow.com/questions/426645/
        // how-to-render-an-image-in-a-background-wpf-process
        //
        // ...the poster suggests feeding the WebResponse.GetResponseStream()
        // indirectly to BitmapImage.StreamSource.  This sometimes works but
        // sometimes doesn't, which you can tell by checking
        // BitmapImage.IsDownloading at the end of his code.  It is true
        // sometimes, indicating that the Bitmap is still downloading.

        HttpWebRequest oHttpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
        oHttpWebRequest.Timeout = HttpWebRequestTimeoutMs;

        oHttpWebRequest.CachePolicy = new RequestCachePolicy(
            RequestCacheLevel.CacheIfAvailable);

        WebResponse oWebResponse = oHttpWebRequest.GetResponse();

        BinaryReader oBinaryReader = new BinaryReader(
            oWebResponse.GetResponseStream() );

        List<Byte> oResponseBytes = new List<Byte>();

        while (true)
        {
            Byte [] abtSomeResponseBytes = oBinaryReader.ReadBytes(8192);

            if (abtSomeResponseBytes.Length == 0)
            {
                break;
            }

            oResponseBytes.AddRange(abtSomeResponseBytes);
        }

        Byte [] abtAllResponseBytes = oResponseBytes.ToArray();
        oResponseBytes = null;

        MemoryStream oMemoryStream = new MemoryStream(abtAllResponseBytes,
            false);

        BitmapImage oBitmapImage = new BitmapImage();
        oBitmapImage.BeginInit();

        // Some Twitter images have corrupted color profiles, which causes
        // BitmapImage.EndInit() to raise an ArgumentException with the message
        // "Value does not fall within the expected range."  Follow the
        // suggestion at the following page and ignore color profiles.
        //
        // http://www.hanselman.com/blog/
        // DealingWithImagesWithBadMetadataCorruptedColorProfilesInWPF.aspx

        oBitmapImage.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;

        oBitmapImage.StreamSource = oMemoryStream;
        oBitmapImage.EndInit();

        WpfGraphicsUtil.FreezeIfFreezable(oBitmapImage);

        return (oBitmapImage);
    }

    //*************************************************************************
    //  Method: CreateErrorImage()
    //
    /// <summary>
    /// Returns a square image to display when an error is encountered.
    /// </summary>
    ///
    /// <param name="widthAndHeight">
    /// The image's width and height, in WPF units.
    /// </param>
    ///
    /// <returns>
    /// An error image.
    /// </returns>
    //*************************************************************************

    public BitmapSource
    CreateErrorImage
    (
        Int32 widthAndHeight
    )
    {
        Debug.Assert(widthAndHeight > 0);
        AssertValid();

        // Draw a square filled with white, outlined in black, and with a red
        // "X".

        const Int32 Margin = 4;
        Int32 iWidthAndHeightMinusMargin = widthAndHeight - Margin;
        DrawingVisual oDrawingVisual = new DrawingVisual();

        DrawingContext oDrawingContext = oDrawingVisual.RenderOpen();
        Rect oRectangle = new Rect( new Size(widthAndHeight, widthAndHeight) );

        WpfGraphicsUtil.DrawPixelAlignedRectangle(oDrawingContext,
            Brushes.White, new Pen(Brushes.Black, 1), oRectangle);

        Pen oXPen = new Pen(Brushes.Red, 1);

        oDrawingContext.DrawLine( oXPen,
            new Point(Margin, Margin),
            new Point(iWidthAndHeightMinusMargin, iWidthAndHeightMinusMargin)
            );

        oDrawingContext.DrawLine( oXPen,
            new Point(iWidthAndHeightMinusMargin, Margin),
            new Point(Margin, iWidthAndHeightMinusMargin)
            );

        oDrawingContext.Close();

        return ( DrawingVisualToRenderTargetBitmap(oDrawingVisual,
            widthAndHeight, widthAndHeight) );
    }

    //*************************************************************************
    //  Method: ResizeImage()
    //
    /// <overloads>
    /// Resizes an image.
    /// </overloads>
    ///
    /// <summary>
    /// Resizes an image to specified dimensions.
    /// </summary>
    ///
    /// <param name="image">
    /// The original image.
    /// </param>
    ///
    /// <param name="widthNew">
    /// Width of the resized image, in WPF units.
    /// </param>
    ///
    /// <param name="heightNew">
    /// Height of the resized image, in WPF units.
    /// </param>
    ///
    /// <returns>
    /// A resized copy of <paramref name="image" />.
    /// </returns>
    //*************************************************************************

    public BitmapSource
    ResizeImage
    (
        ImageSource image,
        Int32 widthNew,
        Int32 heightNew
    )
    {
        Debug.Assert(image != null);
        Debug.Assert(widthNew > 0);
        Debug.Assert(heightNew > 0);
        AssertValid();

        DrawingVisual oDrawingVisual = new DrawingVisual();
        DrawingContext oDrawingContext = oDrawingVisual.RenderOpen();

        oDrawingContext.DrawImage( image,
            new Rect( new Point(), new Size(widthNew, heightNew) ) );

        oDrawingContext.Close();

        return ( DrawingVisualToRenderTargetBitmap(oDrawingVisual, widthNew,
            heightNew) );
    }

    //*************************************************************************
    //  Method: ResizeImage()
    //
    /// <summary>
    /// Resizes an image while maintaining its aspect ratio.
    /// </summary>
    ///
    /// <param name="image">
    /// The original image.
    /// </param>
    ///
    /// <param name="longerDimensionNew">
    /// Width or height of the resized image, in WPF units.
    /// </param>
    ///
    /// <returns>
    /// A resized copy of <paramref name="image" />.  The copy has the same
    /// aspect ratio as the original, but its longer dimension is <paramref
    /// name="longerDimensionNew" />.
    /// </returns>
    //*************************************************************************

    public BitmapSource
    ResizeImage
    (
        ImageSource image,
        Int32 longerDimensionNew
    )
    {
        Debug.Assert(image != null);
        Debug.Assert(longerDimensionNew > 0);
        AssertValid();

        Double dWidthOriginal = image.Width;
        Double dHeightOriginal = image.Height;

        Double dLongerDimensionOriginal =
            Math.Max(dWidthOriginal, dHeightOriginal);

        Double dShorterDimensionOriginal =
            Math.Min(dWidthOriginal, dHeightOriginal);

        Debug.Assert(dLongerDimensionOriginal > 0);

        Int32 iShorterDimensionNew = (Int32)( dShorterDimensionOriginal *
            ( (Double)longerDimensionNew / dLongerDimensionOriginal ) );

        iShorterDimensionNew = Math.Max(1, iShorterDimensionNew);

        if (dWidthOriginal > dHeightOriginal)
        {
            return ( ResizeImage(image, longerDimensionNew,
                iShorterDimensionNew) );
        }

        return ( ResizeImage(image, iShorterDimensionNew,
            longerDimensionNew) );
    }

    //*************************************************************************
    //  Method: DrawingVisualToRenderTargetBitmap()
    //
    /// <summary>
    /// Renders a DrawingVisual on a RenderTargetBitmap.
    /// </summary>
    ///
    /// <param name="oDrawingVisual">
    /// The DrawingVisual to render.
    /// </param>
    ///
    /// <param name="iWidth">
    /// Width to use, in WPF units.
    /// </param>
    ///
    /// <param name="iHeight">
    /// Height to use, in WPF units.
    /// </param>
    ///
    /// <returns>
    /// A new, frozen RenderTargetBitmap, with a DPI of 96.
    /// </returns>
    //*************************************************************************

    protected RenderTargetBitmap
    DrawingVisualToRenderTargetBitmap
    (
        DrawingVisual oDrawingVisual,
        Int32 iWidth,
        Int32 iHeight
    )
    {
        Debug.Assert(oDrawingVisual != null);
        Debug.Assert(iWidth > 0);
        Debug.Assert(iHeight > 0);
        AssertValid();

        RenderTargetBitmap oRenderTargetBitmap = new RenderTargetBitmap(
            iWidth, iHeight, 96.0, 96.0, PixelFormats.Pbgra32);

        oRenderTargetBitmap.Render(oDrawingVisual);
        WpfGraphicsUtil.FreezeIfFreezable(oRenderTargetBitmap);

        return (oRenderTargetBitmap);
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
        // m_oCachedErrorImage
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// The timeout to use for Web requests, in milliseconds.

    protected const Int32 HttpWebRequestTimeoutMs = 10 * 1000;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Cached error image returned by GetImageSynchronous(), or null.

    protected BitmapSource m_oCachedErrorImage;

    /// The DPI of the screen.

    protected static Double m_dScreenDpi = 96.0;
}

}
