
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: AutoFillColorColumnResults
//
/// <summary>
/// Stores one color column of the results of a call to <see
/// cref="WorkbookAutoFiller.AutoFillWorkbook" />.
/// </summary>
//*****************************************************************************

public class AutoFillColorColumnResults : AutoFillColumnResults
{
    //*************************************************************************
    //  Constructor: AutoFillColorColumnResults()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="AutoFillColorColumnResults" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillColorColumnResults" /> class with default values that
    /// indicate that the column wasn't autofilled.
    /// </summary>
    //*************************************************************************

    public AutoFillColorColumnResults()
    : this(true, null, 0, 0, 0, Color.Black, Color.Black, null)
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: AutoFillColorColumnResults()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="AutoFillColorColumnResults" /> class with specified
    /// values.
    /// </summary>
    ///
    /// <param name="sourceColumnContainsNumbers">
    /// true if the source column contains numbers, false if it contains
    /// categories.
    /// </param>
    ///
    /// <param name="sourceColumnName">
    /// Name of the source column, or null if the column wasn't autofilled.
    /// Used only if <paramref name="sourceColumnContainsNumbers" /> is true.
    /// </param>
    ///
    /// <param name="sourceCalculationNumber1">
    /// The actual first source number used in the calculations.
    /// Used only if <paramref name="sourceColumnContainsNumbers" /> is true.
    /// </param>
    ///
    /// <param name="sourceCalculationNumber2">
    /// The actual second source number used in the calculations.
    /// Used only if <paramref name="sourceColumnContainsNumbers" /> is true.
    /// </param>
    ///
    /// <param name="decimalPlaces">
    /// The number of decimal places displayed in the column.
    /// Used only if <paramref name="sourceColumnContainsNumbers" /> is true.
    /// </param>
    ///
    /// <param name="destinationColor1">
    /// The first color used in the destination column.
    /// Used only if <paramref name="sourceColumnContainsNumbers" /> is true.
    /// </param>
    ///
    /// <param name="destinationColor2">
    /// The second color used in the destination column.
    /// Used only if <paramref name="sourceColumnContainsNumbers" /> is true.
    /// </param>
    ///
    /// <param name="categoryNames">
    /// Collection of category names from the source column.  Used only if
    /// <paramref name="sourceColumnContainsNumbers" /> is false, in which case
    /// it can be empty but not null.
    /// </param>
    //*************************************************************************

    public AutoFillColorColumnResults
    (
        Boolean sourceColumnContainsNumbers,
        String sourceColumnName,
        Double sourceCalculationNumber1,
        Double sourceCalculationNumber2,
        Int32 decimalPlaces,
        Color destinationColor1,
        Color destinationColor2,
        ICollection<String> categoryNames
    )
    : base(sourceColumnName, sourceCalculationNumber1,
        sourceCalculationNumber2, decimalPlaces)
    {
        m_bSourceColumnContainsNumbers = sourceColumnContainsNumbers;
        m_oDestinationColor1 = destinationColor1;
        m_oDestinationColor2 = destinationColor2;
        m_oCategoryNames = categoryNames;

        AssertValid();
    }

    //*************************************************************************
    //  Property: ColumnAutoFilledWithCategories
    //
    /// <summary>
    /// Gets a flag indicating whether the column was autofilled with
    /// categories.
    /// </summary>
    ///
    /// <returns>
    /// true if the column was autofilled and it was autofilled with
    /// categories, false if it wasn't autofilled or it was autofilled with
    /// numbers.
    /// </returns>
    //*************************************************************************

    public Boolean
    ColumnAutoFilledWithCategories
    {
        get
        {
            AssertValid();

            return (this.ColumnAutoFilled &&
                !this.SourceColumnContainsNumbers);
        }
    }

    //*************************************************************************
    //  Property: SourceColumnContainsNumbers
    //
    /// <summary>
    /// Gets a flag indicating whether the source column contains numbers.
    /// </summary>
    ///
    /// <value>
    /// true if the source column contains numbers, false if it contains
    /// categories.
    /// </value>
    //*************************************************************************

    public Boolean
    SourceColumnContainsNumbers
    {
        get
        {
            AssertValid();

            return (m_bSourceColumnContainsNumbers);
        }
    }

    //*************************************************************************
    //  Property: DestinationColor1
    //
    /// <summary>
    /// Gets the first color used in the destination column.
    /// </summary>
    ///
    /// <returns>
    /// The first color used in the destination column, or Color.Black if the
    /// column wasn't autofilled.
    /// </returns>
    ///
    /// <remarks>
    /// This is used only if <see cref="SourceColumnContainsNumbers" /> is
    /// true.
    /// </remarks>
    //*************************************************************************

    public Color
    DestinationColor1
    {
        get
        {
            AssertValid();

            return (m_oDestinationColor1);
        }
    }

    //*************************************************************************
    //  Property: DestinationColor2
    //
    /// <summary>
    /// Gets the second color used in the destination column.
    /// </summary>
    ///
    /// <returns>
    /// The second color used in the destination column, or Color.Black if the
    /// column wasn't autofilled.
    /// </returns>
    ///
    /// <remarks>
    /// This is used only if <see cref="SourceColumnContainsNumbers" /> is
    /// true.
    /// </remarks>
    //*************************************************************************

    public Color
    DestinationColor2
    {
        get
        {
            AssertValid();

            return (m_oDestinationColor2);
        }
    }

    //*************************************************************************
    //  Property: CategoryNames
    //
    /// <summary>
    /// Gets the collection of category names from the source column.
    /// </summary>
    ///
    /// <returns>
    /// The collection of category names.
    /// </returns>
    ///
    /// <remarks>
    /// This is used only if <see cref="SourceColumnContainsNumbers" /> is
    /// false.
    /// </remarks>
    //*************************************************************************

    public ICollection<String>
    CategoryNames
    {
        get
        {
            AssertValid();

            return (m_oCategoryNames);
        }
    }

    //*************************************************************************
    //  Method: ConvertToString()
    //
    /// <summary>
    /// Converts the object to a string that can be persisted.
    /// </summary>
    ///
    /// <returns>
    /// The object converted into a String.
    /// </returns>
    ///
    /// <remarks>
    /// Use <see cref="ConvertFromString" /> to go in the other direction.
    /// </remarks>
    //*************************************************************************

    public new String
    ConvertToString()
    {
        AssertValid();

        ColorConverter oColorConverter = new ColorConverter();

        String [] asCategoryNames = (m_oCategoryNames == null) ?
            new String [0] : m_oCategoryNames.ToArray();

        return ( String.Join(PerWorkbookSettings.FieldSeparatorString,

            new String [] {
                base.ConvertToString(),

                ( new System.ComponentModel.BooleanConverter() ).
                    ConvertToInvariantString(m_bSourceColumnContainsNumbers),

                oColorConverter.ConvertToInvariantString(
                    m_oDestinationColor1),

                oColorConverter.ConvertToInvariantString(
                    m_oDestinationColor2),

                String.Join(PerWorkbookSettings.SubFieldSeparatorString,
                    asCategoryNames)
            } ) );
    }

    //*************************************************************************
    //  Method: ConvertFromString()
    //
    /// <summary>
    /// Sets the column mapping from a persisted string.
    /// </summary>
    ///
    /// <param name="asFields">
    /// Array of column result fields.
    /// </param>
    ///
    /// <param name="iStartIndex">
    /// Index of the next unread field in <paramref name="asFields" />.
    /// </param>
    ///
    /// <returns>
    /// Index of the next unread field in <paramref name="asFields" />.
    /// </returns>
    //*************************************************************************

    public new Int32
    ConvertFromString
    (
        String [] asFields,
        Int32 iStartIndex
    )
    {
        Debug.Assert(asFields != null);
        Debug.Assert(iStartIndex >= 0);

        iStartIndex = base.ConvertFromString(asFields, iStartIndex);

        Debug.Assert(iStartIndex + 3 < asFields.Length);

        ColorConverter oColorConverter = new ColorConverter();

        m_bSourceColumnContainsNumbers =
            (Boolean)( new System.ComponentModel.BooleanConverter() ).
                ConvertFromInvariantString( asFields[iStartIndex + 0] );

        m_oDestinationColor1 =
            (Color)oColorConverter.ConvertFromInvariantString(
                asFields[iStartIndex + 1] );

        m_oDestinationColor2 =
            (Color)oColorConverter.ConvertFromInvariantString(
                asFields[iStartIndex + 2] );

        m_oCategoryNames = asFields[iStartIndex + 3].Split(
            PerWorkbookSettings.SubFieldSeparator);

        // Note:
        //
        // If another field is added here, the total expected field count in 
        // AutoFillWorkbookResults.ConvertFromString() must be increased.

        return (iStartIndex + 4);
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

        // m_bSourceColumnContainsNumbers
        // m_oDestinationColor1
        // m_oDestinationColor2

        Debug.Assert(m_bSourceColumnContainsNumbers ||
            m_oCategoryNames != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true if the source column contains numbers, false if it contains
    /// categories.

    protected Boolean m_bSourceColumnContainsNumbers;

    /// The first color used in the destination column.  Used only if
    /// m_bSourceColumnContainsNumbers is true.

    protected Color m_oDestinationColor1;

    /// The second color used in the destination column.  Used only if
    /// m_bSourceColumnContainsNumbers is true.

    protected Color m_oDestinationColor2;

    /// Collection of category names.  Used only if
    /// m_bSourceColumnContainsNumbers is false, in which case it can be empty
    /// but not null.

    protected ICollection<String> m_oCategoryNames;
}

}
