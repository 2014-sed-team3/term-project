
using System;
using System.Windows.Forms;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EdgeColumnComboBox
//
/// <summary>
/// Represents a ComboBox that lets the user select a column from the edge
/// table.
/// </summary>
///
/// <remarks>
/// Call <see cref="ColumnComboBox.PopulateWithTableColumnNames" /> to populate
/// the ComboBox with the edge table column names.
/// </remarks>
//*****************************************************************************

public class EdgeColumnComboBox : ColumnComboBox
{
    //*************************************************************************
    //  Constructor: EdgeColumnComboBox()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EdgeColumnComboBox" />
    /// class.
    /// </summary>
    //*************************************************************************

    public EdgeColumnComboBox()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: GetTableColumnNames()
    //
    /// <summary>
    /// Gets the names of the table's columns.
    /// </summary>
    ///
    /// <param name="table">
    /// The table containing the columns.
    /// </param>
    ///
    /// <returns>
    /// The table column names.
    /// </returns>
    //*************************************************************************

    public static String []
    GetTableColumnNames
    (
        Microsoft.Office.Interop.Excel.ListObject table
    )
    {
        return ( ExcelTableUtil.GetTableColumnNames(table,
            TableColumnNamesToExclude, ExcelTableUtil.NoColumnNames) );
    }

    //*************************************************************************
    //  Method: GetTableColumnNamesInternal()
    //
    /// <summary>
    /// Gets the names of the table's columns.
    /// </summary>
    ///
    /// <param name="oTable">
    /// The table containing the columns.
    /// </param>
    ///
    /// <returns>
    /// The table column names.
    /// </returns>
    //*************************************************************************

    protected override String []
    GetTableColumnNamesInternal
    (
        Microsoft.Office.Interop.Excel.ListObject oTable
    )
    {
        AssertValid();

        return ( GetTableColumnNames(oTable) );
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

    /// Table column names to exclude from the ComboBox.

    protected static readonly String [] TableColumnNamesToExclude =
        new String [] {
            EdgeTableColumnNames.Vertex1Name,
            EdgeTableColumnNames.Vertex2Name,
            EdgeTableColumnNames.Color,
            EdgeTableColumnNames.Width,
            EdgeTableColumnNames.Style,
            CommonTableColumnNames.Alpha,
            CommonTableColumnNames.Visibility,
            EdgeTableColumnNames.Label,
            EdgeTableColumnNames.LabelTextColor,
            EdgeTableColumnNames.LabelFontSize,
            CommonTableColumnNames.ID,
            CommonTableColumnNames.DynamicFilter,
            CommonTableColumnNames.AddColumnsHere,
            };
}

}
