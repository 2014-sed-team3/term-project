
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: GroupVisibilityConverter
//
/// <summary>
/// Class that converts a group visibility between values used in the Excel
/// workbook and values used in the NodeXL graph.
/// </summary>
//*****************************************************************************

public class GroupVisibilityConverter :
    TextValueConverterBase<GroupWorksheetReader.Visibility>
{
    //*************************************************************************
    //  Constructor: GroupVisibilityConverter()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GroupVisibilityConverter" /> class.
    /// </summary>
    //*************************************************************************

    public GroupVisibilityConverter()
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

        return ( new GraphValueInfo [] {

            new GraphValueInfo( GroupWorksheetReader.Visibility.Show,
                new String [] {"Show", "1",} ),

            new GraphValueInfo( GroupWorksheetReader.Visibility.Skip,
                new String [] {"Skip", "0",} ),

            new GraphValueInfo( GroupWorksheetReader.Visibility.Hide,
                new String [] {"Hide", "2",} ),
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
