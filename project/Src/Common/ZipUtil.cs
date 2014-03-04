
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Ionic.Zip;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: ZipUtil
//
/// <summary>
/// Utility methods for working with Zip files.
/// </summary>
///
/// <remarks>
/// This class assumes that the Ionic.Zip.Reduced.dll assembly from DotNetZip
/// (http://dotnetzip.codeplex.com/) is referenced in the application.
/// </remarks>
//*****************************************************************************

public static class ZipUtil
{
    //*************************************************************************
    //  Method: ZipOneTextFile()
    //
    /// <summary>
    /// Zips a file that contains text into a Zip file.
    /// </summary>
    ///
    /// <param name="textFileContents">
    /// The contents of the text file.
    /// </param>
    ///
    /// <param name="textFileName">
    /// The name of the text file that <paramref name="textFileContents" />
    /// came from, without a path.
    /// </param>
    ///
    /// <returns>
    /// A Zip file, as an array of bytes.  The zip file contains one text file,
    /// named <paramref name="textFileName" />, and the text file contains the
    /// text <paramref name="textFileContents" />.
    /// </returns>
    //*************************************************************************

    public static Byte []
    ZipOneTextFile
    (
        String textFileContents,
        String textFileName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(textFileContents) );
        Debug.Assert( !String.IsNullOrEmpty(textFileName) );

        using ( ZipFile oZipFile = new ZipFile() )
        {
            oZipFile.AddEntry(textFileName, textFileContents, Encoding.UTF8);

            MemoryStream oMemoryStream = new MemoryStream();
            oZipFile.Save(oMemoryStream);

            return ( oMemoryStream.ToArray() );
        }
    }

    //*************************************************************************
    //  Method: UnzipOneTextFile()
    //
    /// <summary>
    /// Unzips a text file contained in a Zip file.
    /// </summary>
    ///
    /// <param name="zipFileContents">
    /// A Zip file that contains one text file.  The text file can have any
    /// name.
    /// </param>
    ///
    /// <returns>
    /// The unziped text file, as a String.
    /// </returns>
    //*************************************************************************

    public static String
    UnzipOneTextFile
    (
        Byte [] zipFileContents
    )
    {
        Debug.Assert(zipFileContents != null);
        Debug.Assert(zipFileContents.Length > 0);

        using ( MemoryStream oZippedMemoryStream =
            new MemoryStream(zipFileContents) )
        {
            ReadOptions oReadOptions = new ReadOptions();
            oReadOptions.Encoding = Encoding.UTF8;

            using ( ZipFile oZipFile =
                ZipFile.Read(oZippedMemoryStream, oReadOptions) )
            {
                ICollection<ZipEntry> oEntries = oZipFile.Entries;

                if (oEntries.Count != 1)
                {
                    throw new ApplicationException(
                        "The Zip file does not contain one and only one"
                        + " text file."
                        );
                }

                ZipEntry oTextEntry = oEntries.First();

                using ( MemoryStream oUnzippedMemoryStream =
                    new MemoryStream() )
                {
                    oTextEntry.Extract(oUnzippedMemoryStream);

                    return ( Encoding.UTF8.GetString(
                        oUnzippedMemoryStream.ToArray() ) );
                }
            }
        }
    }
}

}
