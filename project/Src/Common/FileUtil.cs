
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: FileUtil
//
/// <summary>
/// Static utility methods for working with files.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class FileUtil
{
    //*************************************************************************
    //  Method: ReadTextFile()
    //
    /// <summary>
    /// Reads the entire contents of a text file using the UTF-8 encoding.
    /// </summary>
    ///
    /// <param name="filePath">
    /// Full path to the text file to read.
    /// </param>
    ///
    /// <returns>
    /// The text file contents.
    /// </returns>
    //*************************************************************************

    public static String
    ReadTextFile
    (
        String filePath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(filePath) );

        using ( StreamReader oStreamReader = new StreamReader(filePath) )
        {
            return ( oStreamReader.ReadToEnd() );
        }
    }

    //*************************************************************************
    //  Method: ReadBinaryFile()
    //
    /// <summary>
    /// Reads the entire contents of a binary file.
    /// </summary>
    ///
    /// <param name="filePath">
    /// Full path to the binary file to read.
    /// </param>
    ///
    /// <returns>
    /// The binary file contents.
    /// </returns>
    //*************************************************************************

    public static Byte []
    ReadBinaryFile
    (
        String filePath
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(filePath) );

        using (FileStream oFileStream = new FileStream(
            filePath, FileMode.Open, FileAccess.Read) )
        {
            using ( BinaryReader oBinaryReader =
                new BinaryReader(oFileStream) )
            {
                Debug.Assert(oFileStream.Length <= Int32.MaxValue);

                return ( oBinaryReader.ReadBytes(
                    (Int32)oFileStream.Length) );
            }
        }
    }

    //*************************************************************************
    //  Method: GetPathToUseForTemporaryFile()
    //
    /// <summary>
    /// Gets a unique file path to use for a temporary file.
    /// </summary>
    ///
    /// <returns>
    /// The full path to use for the temporary file.
    /// </returns>
    ///
    /// <remarks>
    /// Unlike <see cref="Path.GetTempFileName" />, this method does not create
    /// the temporary file.
    /// </remarks>
    //*************************************************************************

    public static String
    GetPathToUseForTemporaryFile()
    {
        return ( Path.Combine(
            Path.GetTempPath(), Guid.NewGuid().ToString() ) );
    }

    //*************************************************************************
    //  Method: ReplaceIllegalFileNameChars()
    //
    /// <summary>
    /// Removes illegal characters from a file name.
    /// </summary>
    ///
    /// <param name="fileName">
    /// File name that may contain illegal characters.  Can be empty but not
    /// null.
    /// </param>
    ///
    /// <param name="replacementString">
    /// String to replace each illegal character with.  Can be empty but not
    /// null.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="fileName" /> with any illegal characters replaced.
    /// </returns>
    //*************************************************************************

    public static String
    ReplaceIllegalFileNameChars
    (
        String fileName,
        String replacementString
    )
    {
        Debug.Assert(fileName != null);
        Debug.Assert(replacementString != null);

        // Use a regular expression to do the work.

        Regex oRegex = new Regex(
            "[" + Regex.Escape(
            new String( Path.GetInvalidFileNameChars() ) ) + "]"
            );

        return ( oRegex.Replace(fileName, replacementString) );
    }

    //*************************************************************************
    //  Method: EncodeIllegalFileNameChars()
    //
    /// <summary>
    /// Encodes illegal characters in a file name.
    /// </summary>
    ///
    /// <param name="fileName">
    /// File name that may contain illegal characters.
    /// </param>
    ///
    /// <returns>
    /// <paramref name="fileName" /> with any illegal characters hex-encoded.
    /// </returns>
    //*************************************************************************

    public static String
    EncodeIllegalFileNameChars
    (
        String fileName
    )
    {
        Debug.Assert(fileName != null);

        String sEncodedFileName = fileName;

        foreach (char cInvalidFileNameChar in Path.GetInvalidFileNameChars() )
        {
            sEncodedFileName = sEncodedFileName.Replace(
                cInvalidFileNameChar.ToString(),
                Uri.HexEscape(cInvalidFileNameChar) );
        }

        return (sEncodedFileName);
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    // Are these constants defined anywhere in the .NET Framework?  They were
    // taken from PathTooLongException.Message, which is this:
    //
    // "The specified path, file name, or both are too long. The fully
    // qualified file name must be less than 260 characters, and the directory
    // name must be less than 248 characters."
    //

    /// Maximum file name length.

    public const Int32 MaximumFileNameLength = 259;

    /// Maximum folder name length.

    public const Int32 MaximumFolderNameLength = 247;
}

}
