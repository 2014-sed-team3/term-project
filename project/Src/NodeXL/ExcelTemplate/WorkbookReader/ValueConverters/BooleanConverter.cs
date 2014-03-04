﻿
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: BooleanConverter
//
/// <summary>
/// Class that converts a Boolean between values used in the Excel workbook and
/// values used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class BooleanConverter : TextValueConverterBase<Boolean>
{
    //*************************************************************************
    //  Constructor: BooleanConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="BooleanConverter" />
    /// class.
    /// </summary>
    //*************************************************************************

    public BooleanConverter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetGraphValueInfos()
    //
    /// <summary>
    /// Gets an array of GraphValueInfo objects, one for each valid graph
    /// value.
    /// </summary>
    //*************************************************************************

    protected override GraphValueInfo []
    GetGraphValueInfos()
    {
        AssertValid();

        // The parenthetical entries are for backward compatibility with older
        // NodeXL workbooks.  Parentheses are not used in newer value
        // converters.

        return ( new GraphValueInfo [] {

            new GraphValueInfo(true,
                new String [] {"Yes", "Yes (1)", "1"} ),

            new GraphValueInfo(false,
                new String [] {"No", "No (0)", "0",} ),
            } );
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    // [Conditional("DEBUG")]

    public override void
    AssertValid()
    {
        base.AssertValid();

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
