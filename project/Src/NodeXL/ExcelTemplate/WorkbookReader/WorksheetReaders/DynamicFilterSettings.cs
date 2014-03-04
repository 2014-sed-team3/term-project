
using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: DynamicFilterSettings
//
/// <summary>
/// Provides access to settings for dynamic filtering.
/// </summary>
///
/// <remarks>
/// A dynamic filter is a control that can be adjusted to selectively show and
/// hide edges and vertices in the graph in real time.  This class maintains
/// the filter settings, which are stored in a table in a hidden worksheet.
///
/// <para>
/// Call the <see cref="Open" /> method to gain access to the settings, then
/// use <see cref="TryGetSettings" /> and <see cref="SetSettings" /> to get and
/// set the settings for one filter.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class DynamicFilterSettings : WorksheetReaderBase
{
    //*************************************************************************
    //  Constructor: DynamicFilterSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicFilterSettings" />
    /// class.
    /// </summary>
    ///
    /// <param name="workbook">
    /// The workbook containing the settings.
    /// </param>
    //*************************************************************************

    public DynamicFilterSettings
    (
        Microsoft.Office.Interop.Excel.Workbook workbook
    )
    {
        m_oWorkbook = workbook;
        m_oDynamicFilterSettingsTable = null;

        m_oDynamicFilterSettingsDictionary =
            new Dictionary<String, SettingsForOneFilter>();

        AssertValid();
    }

    //*************************************************************************
    //  Method: Open()
    //
    /// <summary>
    /// Attempts to gain access to all the filter settings.
    /// </summary>
    ///
    /// <remarks>
    /// Call this before calling any other method.
    ///
    /// <para>
    /// A <see cref="WorkbookFormatException" /> is thrown if the dynamic
    /// filter settings table can't be read.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    Open()
    {
        Debug.Assert(m_oDynamicFilterSettingsTable == null);
        AssertValid();

        // Get the table that contains the dynamic filter settings.

        if (!ExcelTableUtil.TryGetTable(m_oWorkbook,
            WorksheetNames.Miscellaneous, TableNames.DynamicFilterSettings,
            out m_oDynamicFilterSettingsTable) )
        {
            OnWorkbookFormatError( String.Format(

                "A table that is required to use this feature is missing."
                + "\r\n\r\n{0}"
                ,
                ErrorUtil.GetTemplateMessage()
                ) );
        }

        foreach (ExcelTableReader.ExcelTableRow oRow in
            ( new ExcelTableReader(m_oDynamicFilterSettingsTable) ).GetRows() )
        {
            String sTableName, sColumnName;
            Double dSelectedMinimum, dSelectedMaximum;

            if (
                oRow.TryGetNonEmptyStringFromCell(
                    DynamicFilterSettingsTableColumnNames.TableName,
                    out sTableName)
                &&
                oRow.TryGetNonEmptyStringFromCell(
                    DynamicFilterSettingsTableColumnNames.ColumnName,
                    out sColumnName)
                &&
                oRow.TryGetDoubleFromCell(
                    DynamicFilterSettingsTableColumnNames.SelectedMinimum,
                    out dSelectedMinimum)
                &&
                oRow.TryGetDoubleFromCell(
                    DynamicFilterSettingsTableColumnNames.SelectedMaximum,
                    out dSelectedMaximum)
               )
            {
                // Create a SettingsForOneFilter object for each filter and
                // store it in a dictionary.

                SettingsForOneFilter oSettingsForOneFilter =
                    new SettingsForOneFilter();

                oSettingsForOneFilter.SelectedMinimum =
                    (Decimal)dSelectedMinimum;

                oSettingsForOneFilter.SelectedMaximum =
                    (Decimal)dSelectedMaximum;

                oSettingsForOneFilter.SelectedMinimumAddress =
                    ExcelUtil.GetRangeAddressAbsolute(
                        oRow.GetRangeForCell(
                        DynamicFilterSettingsTableColumnNames.SelectedMinimum)
                        );

                oSettingsForOneFilter.SelectedMaximumAddress =
                    ExcelUtil.GetRangeAddressAbsolute(
                        oRow.GetRangeForCell(
                        DynamicFilterSettingsTableColumnNames.SelectedMaximum)
                        );

                m_oDynamicFilterSettingsDictionary.Add(
                    GetDictionaryKey(sTableName, sColumnName),
                        oSettingsForOneFilter);
            }
        }
    }

    //*************************************************************************
    //  Method: TryGetSettings()
    //
    /// <summary>
    /// Attempts to get the settings for one filter.
    /// </summary>
    ///
    /// <param name="tableName">
    /// Name of the table containing the column being filtered on.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column being filtered on.
    /// </param>
    ///
    /// <param name="selectedMinimum">
    /// Where the filter's minimum value gets stored if true is returned.
    /// </param>
    ///
    /// <param name="selectedMaximum">
    /// Where the filter's maximum value gets stored if true is returned.
    /// </param>
    ///
    /// <param name="selectedMinimumAddress">
    /// Where the address of the settings table cell containing the filter's
    /// minimum value gets stored if true is returned.  The address is
    /// absolute ("$G$5", for example.)
    /// </param>
    ///
    /// <param name="selectedMaximumAddress">
    /// Where the address of the settings table cell containing the filter's
    /// maximum value gets stored if true is returned.  The address is
    /// absolute ("$G$6", for example.)
    /// </param>
    ///
    /// <returns>
    /// true if the settings were obtained, false if the settings weren't in
    /// the settings table.
    /// </returns>
    ///
    /// <remarks>
    /// Call <see cref="Open" /> before calling this method.
    /// </remarks>
    //*************************************************************************

    public Boolean
    TryGetSettings
    (
        String tableName,
        String columnName,
        out Decimal selectedMinimum,
        out Decimal selectedMaximum,
        out String selectedMinimumAddress,
        out String selectedMaximumAddress
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(tableName) );
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        AssertValid();

        CheckOpen();

        selectedMinimum = selectedMaximum = Decimal.MinValue;
        selectedMinimumAddress = selectedMaximumAddress = null;

        SettingsForOneFilter oSettingsForOneFilter;

        if ( m_oDynamicFilterSettingsDictionary.TryGetValue(
            GetDictionaryKey(tableName, columnName),
            out oSettingsForOneFilter) )
        {
            selectedMinimum = oSettingsForOneFilter.SelectedMinimum;
            selectedMaximum = oSettingsForOneFilter.SelectedMaximum;

            selectedMinimumAddress =
                oSettingsForOneFilter.SelectedMinimumAddress;

            selectedMaximumAddress =
                oSettingsForOneFilter.SelectedMaximumAddress;

            return (true);
        }

        return (false);
    }

    //*************************************************************************
    //  Method: SetSettings()
    //
    /// <summary>
    /// Sets the settings for one filter.
    /// </summary>
    ///
    /// <param name="tableName">
    /// Name of the table containing the column being filtered on.
    /// </param>
    ///
    /// <param name="columnName">
    /// Name of the column being filtered on.
    /// </param>
    ///
    /// <param name="selectedMinimum">
    /// The filter's minimum value.
    /// </param>
    ///
    /// <param name="selectedMaximum">
    /// The filter's maximum value.
    /// </param>
    ///
    /// <param name="selectedMinimumAddress">
    /// Where the address of the settings table cell containing the filter's
    /// minimum value gets stored.  The address is absolute ("$G$5", for
    /// example.)
    /// </param>
    ///
    /// <param name="selectedMaximumAddress">
    /// Where the address of the settings table cell containing the filter's
    /// maximum value gets stored.  The address is absolute ("$G$6", for
    /// example.)
    /// </param>
    ///
    /// <remarks>
    /// Call <see cref="Open" /> before calling this method.
    /// </remarks>
    //*************************************************************************

    public void
    SetSettings
    (
        String tableName,
        String columnName,
        Decimal selectedMinimum,
        Decimal selectedMaximum,
        out String selectedMinimumAddress,
        out String selectedMaximumAddress
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(tableName) );
        Debug.Assert( !String.IsNullOrEmpty(columnName) );
        Debug.Assert(selectedMaximum >= selectedMinimum);
        AssertValid();

        CheckOpen();

        selectedMinimumAddress = selectedMaximumAddress = null;

        SettingsForOneFilter oSettingsForOneFilter;
        String sDictionaryKey = GetDictionaryKey(tableName, columnName);
        Range oSelectedMinimumCell, oSelectedMaximumCell;

        if ( m_oDynamicFilterSettingsDictionary.TryGetValue(
            sDictionaryKey, out oSettingsForOneFilter) )
        {
            // The settings are already in the table.  Update the table row.

            Debug.Assert(m_oDynamicFilterSettingsTable.Parent is Worksheet);

            Worksheet oWorksheet =
                (Worksheet)m_oDynamicFilterSettingsTable.Parent;

            oSelectedMinimumCell = oWorksheet.get_Range(
                oSettingsForOneFilter.SelectedMinimumAddress, Missing.Value);

            oSelectedMinimumCell.set_Value(Missing.Value, selectedMinimum);

            oSelectedMaximumCell = oWorksheet.get_Range(
                oSettingsForOneFilter.SelectedMaximumAddress, Missing.Value);

            oSelectedMaximumCell.set_Value(Missing.Value, selectedMaximum);

            // Update the dictionary entry.

            oSettingsForOneFilter.SelectedMinimum = selectedMinimum;
            oSettingsForOneFilter.SelectedMaximum = selectedMaximum;
        }
        else
        {
            // The settings aren't in the table yet.  Add a row to the table.

            Range oNewRowRange = ExcelTableUtil.AddTableRow(
                m_oDynamicFilterSettingsTable,

                DynamicFilterSettingsTableColumnNames.TableName,
                    tableName,

                DynamicFilterSettingsTableColumnNames.ColumnName,
                    columnName,

                DynamicFilterSettingsTableColumnNames.SelectedMinimum,
                    selectedMinimum,

                DynamicFilterSettingsTableColumnNames.SelectedMaximum,
                    selectedMaximum
                );

            oSelectedMinimumCell = (Range)oNewRowRange.Cells[ 1,
                GetTableColumnIndex(m_oDynamicFilterSettingsTable,
                DynamicFilterSettingsTableColumnNames.SelectedMinimum, true) ];

            oSelectedMaximumCell = (Range)oNewRowRange.Cells[ 1,
                GetTableColumnIndex(m_oDynamicFilterSettingsTable,
                DynamicFilterSettingsTableColumnNames.SelectedMaximum, true) ];

            // Add a dictionary entry.

            oSettingsForOneFilter = new SettingsForOneFilter();

            oSettingsForOneFilter.SelectedMinimum = selectedMinimum;
            oSettingsForOneFilter.SelectedMaximum = selectedMaximum;

            oSettingsForOneFilter.SelectedMinimumAddress =
                ExcelUtil.GetRangeAddressAbsolute(oSelectedMinimumCell);

            oSettingsForOneFilter.SelectedMaximumAddress =
                ExcelUtil.GetRangeAddressAbsolute(oSelectedMaximumCell);

            m_oDynamicFilterSettingsDictionary.Add(sDictionaryKey,
                oSettingsForOneFilter);
        }

        selectedMinimumAddress = oSettingsForOneFilter.SelectedMinimumAddress;
        selectedMaximumAddress = oSettingsForOneFilter.SelectedMaximumAddress;

        Debug.Assert( !String.IsNullOrEmpty(selectedMinimumAddress) );
        Debug.Assert( !String.IsNullOrEmpty(selectedMaximumAddress) );
    }

    //*************************************************************************
    //  Method: GetDictionaryKey()
    //
    /// <summary>
    /// Gets a key to use for the m_oDynamicFilterSettingsDictionary.
    /// </summary>
    ///
    /// <param name="sTableName">
    /// Name of the table containing the column being filtered on.
    /// </param>
    ///
    /// <param name="sColumnName">
    /// Name of the column being filtered on.
    /// </param>
    ///
    /// <returns>
    /// A key to use for the m_oDynamicFilterSettingsDictionary.
    /// </returns>
    //*************************************************************************

    protected String
    GetDictionaryKey
    (
        String sTableName,
        String sColumnName
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sTableName) );
        Debug.Assert( !String.IsNullOrEmpty(sColumnName) );
        AssertValid();

        // A tab can't occur in either a table or column name, so create the
        // key by concatenating them with a tab.

        return (sTableName + '\t' + sColumnName);
    }

    //*************************************************************************
    //  Method: CheckOpen()
    //
    /// <summary>
    /// Throws an exception if <see cref="Open" /> hasn't been successfully
    /// called.
    /// </summary>
    //*************************************************************************

    protected void
    CheckOpen()
    {
        AssertValid();

        if (m_oDynamicFilterSettingsTable == null)
        {
            Debug.Assert(false);

            throw new InvalidOperationException( String.Format(

                "{0}.Open() must be called before any other methods are"
                + " called."
                ,
                this.ClassName
                ) );
        }
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

        Debug.Assert(m_oWorkbook != null);
        // m_oDynamicFilterSettingsTable
        Debug.Assert(m_oDynamicFilterSettingsDictionary != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The workbook containing the settings.

    protected Microsoft.Office.Interop.Excel.Workbook m_oWorkbook;

    /// Table that stores the settings for all dynamic filters, or null if
    /// Open() hasn't been successfully called.

    protected ListObject m_oDynamicFilterSettingsTable;

    /// Dictionary of filter settings, with one key/value pair for each filter.
    /// The key is the concatenation of the table and column names as returned
    /// by GetDictionaryKey(), and the value is a SettingsForOneFilter object.

    protected Dictionary<String, SettingsForOneFilter>
        m_oDynamicFilterSettingsDictionary;


    //*************************************************************************
    //  Embedded class: SettingsForOneFilter
    //
    /// <summary>
    /// Contains the settings for one dynamic filter.
    /// </summary>
    //*************************************************************************

    protected class SettingsForOneFilter
    {
        /// The filter's minimum value.

        public Decimal SelectedMinimum;

        /// The filter's maximum value.

        public Decimal SelectedMaximum;

        /// The absolute address of the settings table cell containing the
        /// filter's minimum value.

        public String SelectedMinimumAddress;

        /// The absolute address of the settings table cell containing the
        /// filter's maximum value.

        public String SelectedMaximumAddress;
    }
}
}
