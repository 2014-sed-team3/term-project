﻿
using System;
using System.IO;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: StringStream
//
/// <summary>
/// Wraps a String into a MemoryStream.
/// </summary>
//*****************************************************************************

public class StringStream : MemoryStream
{
    //*************************************************************************
    //  Constructor: StringStream()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="StringStream" /> class.
    /// </summary>
    ///
    /// <param name="stringToWrap">
    /// String to wrap into a MemoryStream.
    /// </param>
    //*************************************************************************

    public StringStream
    (
        String stringToWrap
    )
    : 
    base(System.Text.Encoding.UTF8.GetBytes(stringToWrap), false)
    {
    }
}


}
