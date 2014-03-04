
using System;
using System.Windows.Forms;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ColumnComboBox
//
/// <summary>
/// Base class for ComboBoxes that let the user select a column from an Excel
/// table.
/// </summary>
//*****************************************************************************

public abstract class ColumnComboBox : ComboBoxPlus
{
    //*************************************************************************
    //  Constructor: ColumnComboBox()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnComboBox" /> class.
    /// </summary>
    //*************************************************************************

    public ColumnComboBox()
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Method: PopulateWithTableColumnNames()
    //
    /// <summary>
    /// Populates the ComboBox with the names of the table columns.
    /// </summary>
    ///
    /// <param name="table">
    /// The table containing the columns.
    /// </param>
    //*************************************************************************

    public void
    PopulateWithTableColumnNames
    (
        Microsoft.Office.Interop.Excel.ListObject table
    )
    {
        Debug.Assert(table != null);
        AssertValid();

        ComboBox.ObjectCollection oItems = this.Items;
        oItems.Clear();
        oItems.AddRange( GetTableColumnNamesInternal(table) );
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

    protected abstract String []
    GetTableColumnNamesInternal
    (
        Microsoft.Office.Interop.Excel.ListObject oTable
    );


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public virtual void
    AssertValid()
    {
        // (Do nothing.)
    }
}

}
