
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: INumericValueConverter
//
/// <summary>
/// Converts numeric values in the Excel workbook to and from numeric values in
/// the NodeXL graph.
/// </summary>
//*****************************************************************************

public interface INumericValueConverter
{
    //*************************************************************************
    //  Method: WorkbookToGraph()
    //
    /// <summary>
    /// Converts an Excel workbook value to a value suitable for use in a
    /// NodeXL graph.
    /// </summary>
    ///
    /// <param name="valueWorkbook">
    /// Value read from the Excel workbook.
    /// </param>
    ///
    /// <returns>
    /// A value suitable for use in a NodeXL graph.
    /// </returns>
    //*************************************************************************

    Single
    WorkbookToGraph
    (
        Single valueWorkbook
    );

    //*************************************************************************
    //  Method: GraphToWorkbook()
    //
    /// <summary>
    /// Converts a NodeXL graph value to a value suitable for use in an Excel
    /// workbook.
    /// </summary>
    ///
    /// <param name="valueGraph">
    /// Value stored in a NodeXL graph.
    /// </param>
    ///
    /// <returns>
    /// A value suitable for use in an Excel workbook.
    /// </returns>
    //*************************************************************************

    Single
    GraphToWorkbook
    (
        Single valueGraph
    );
}

}
