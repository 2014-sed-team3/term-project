
using System;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: CryptographyUtil
//
/// <summary>
/// Cryptography utility methods.
/// </summary>
//*****************************************************************************

public static class CryptographyUtil
{
    //*************************************************************************
    //  Method: GetMD5Hash()
    //
    /// <summary>
    /// Computes the MD5 hash value for a string.
    /// </summary>
    ///
    /// <param name="inputString">
    /// The string to compute the hash value for.
    /// </param>
    ///
    /// <returns>
    /// The MD5 hash value, as a 32-character hexadecimal string.  Sample:
    /// "ed076287532e86365e841e92bfc50d8c".
    /// </returns>
    //*************************************************************************

    public static String
    GetMD5Hash
    (
        string inputString
    )
    {
        // This was adapted from sample code in the "MD5CryptoServiceProvider
        // Class" topic in MSDN.

        Byte [] hashAsBytes = ( new MD5CryptoServiceProvider() ).ComputeHash(
            Encoding.Unicode.GetBytes(inputString) );

        StringBuilder hashAsString = new StringBuilder();
        Int32 bytes = hashAsBytes.Length;

        for (int i = 0; i < bytes; i++)
        {
            hashAsString.Append( hashAsBytes[i].ToString("x2") );
        }

        return ( hashAsString.ToString() );
    }
}

}
