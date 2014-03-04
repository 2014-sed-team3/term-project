
using System;
using System.Drawing;
using System.Diagnostics;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EditedEdgeAttributes
//
/// <summary>
/// Stores a list of edge attributes that were edited by <see
/// cref="EdgeAttributesDialog" />.
/// </summary>
///
/// <remarks>
/// The list of edges whose attributes were edited can be obtained from <see
/// cref="NodeXLControl.SelectedEdges" />.
/// </remarks>
//*****************************************************************************

public class EditedEdgeAttributes : Object
{
    //*************************************************************************
    //  Constructor: EditedEdgeAttributes()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="EditedEdgeAttributes" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="EditedEdgeAttributes" />
    /// class with default values.
    /// </summary>
    ///
    /// <remarks>
    /// All values are set to null.
    /// </remarks>
    //*************************************************************************

    public EditedEdgeAttributes()
    :
    this(null, null, null, null, null, null, null, null, false)
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: EditedEdgeAttributes()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="EditedEdgeAttributes" />
    /// class with specified values.
    /// </summary>
    ///
    /// <param name="color">
    /// The color that was applied to the selected edges, or null if a color
    /// wasn't applied.
    /// </param>
    ///
    /// <param name="width">
    /// The width that was applied to the selected edges, or null if a width
    /// wasn't applied.  If not null, the width must be between <see
    /// cref="EdgeWidthConverter.MinimumWidthWorkbook" /> and <see
    /// cref="EdgeWidthConverter.MaximumWidthWorkbook" />.
    /// </param>
    ///
    /// <param name="style">
    /// The style that was applied to the selected edges, or null if a style
    /// wasn't applied.
    /// </param>
    ///
    /// <param name="alpha">
    /// The alpha that was applied to the selected edges, or null if an alpha
    /// wasn't applied.  If not null, the alpha must be between <see
    /// cref="AlphaConverter.MinimumAlphaWorkbook" /> and <see
    /// cref="AlphaConverter.MaximumAlphaWorkbook" />.
    /// </param>
    ///
    /// <param name="visibility">
    /// The visibility that was applied to the selected edges, or null if a
    /// visibility wasn't applied.
    /// </param>
    ///
    /// <param name="label">
    /// The label that was applied to the selected edges, or null if a label
    /// wasn't applied.
    /// </param>
    ///
    /// <param name="labelTextColor">
    /// The label text color that was applied to the selected edges, or null if
    /// a label text color wasn't applied.
    /// </param>
    ///
    /// <param name="labelFontSize">
    /// The label font size that was applied to the selected edges, or null if
    /// a label font size wasn't applied.  If not null, the size must be
    /// between <see cref="FontSizeConverter.MinimumFontSizeWorkbook" /> and
    /// <see cref="FontSizeConverter.MaximumFontSizeWorkbook" />.
    /// </param>
    ///
    /// <param name="workbookMustBeReread">
    /// true if the caller must read the workbook again.
    /// </param>
    //*************************************************************************

    public EditedEdgeAttributes
    (
        Nullable<Color> color,
        Nullable<Single> width,
        Nullable<EdgeStyle> style,
        Nullable<Single> alpha,
        Nullable<EdgeWorksheetReader.Visibility> visibility,
        String label,
        Nullable<Color> labelTextColor,
        Nullable<Single> labelFontSize,
        Boolean workbookMustBeReread
    )
    {
        m_oColor = color;
        m_fWidth = width;
        m_eStyle = style;
        m_fAlpha = alpha;
        m_eVisibility = visibility;
        m_sLabel = label;
        m_oLabelTextColor = labelTextColor;
        m_fLabelFontSize = labelFontSize;
        m_bWorkbookMustBeReread = workbookMustBeReread;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Color
    //
    /// <summary>
    /// Gets or sets the color that was applied to the selected edges.
    /// </summary>
    ///
    /// <value>
    /// The color that was applied to the selected edges, or null if a color
    /// wasn't applied.
    /// </value>
    //*************************************************************************

    public Nullable<Color>
    Color
    {
        get
        {
            AssertValid();

            return (m_oColor);
        }

        set
        {
            m_oColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Width
    //
    /// <summary>
    /// Gets or sets the width that was applied to the selected edges.
    /// </summary>
    ///
    /// <value>
    /// The width that was applied to the selected edges, or null if a width
    /// wasn't applied.
    /// </value>
    ///
    /// <remarks>
    /// If not null, the value is between <see
    /// cref="EdgeWidthConverter.MinimumWidthWorkbook" /> and <see
    /// cref="EdgeWidthConverter.MaximumWidthWorkbook" />.
    /// </remarks>
    //*************************************************************************

    public Nullable<Single>
    Width
    {
        get
        {
            AssertValid();

            return (m_fWidth);
        }

        set
        {
            m_fWidth = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Style
    //
    /// <summary>
    /// Gets or sets the style that was applied to the selected edges.
    /// </summary>
    ///
    /// <value>
    /// The style that was applied to the selected edges, or null if a style
    /// wasn't applied.
    /// </value>
    //*************************************************************************

    public Nullable<EdgeStyle>
    Style
    {
        get
        {
            AssertValid();

            return (m_eStyle);
        }

        set
        {
            m_eStyle = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Alpha
    //
    /// <summary>
    /// Gets or sets the alpha that was applied to the selected edges.
    /// </summary>
    ///
    /// <value>
    /// The alpha that was applied to the selected edges, or null if an alpha
    /// wasn't applied.
    /// </value>
    ///
    /// <remarks>
    /// If not null, the value is between <see
    /// cref="AlphaConverter.MinimumAlphaWorkbook" /> and <see
    /// cref="AlphaConverter.MaximumAlphaWorkbook" />.
    /// </remarks>
    //*************************************************************************

    public Nullable<Single>
    Alpha
    {
        get
        {
            AssertValid();

            return (m_fAlpha);
        }

        set
        {
            m_fAlpha = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Visibility
    //
    /// <summary>
    /// Gets or sets the visibility that was applied to the selected edges.
    /// </summary>
    ///
    /// <value>
    /// The visibility that was applied to the selected edges, or null if a
    /// visibility wasn't applied.
    /// </value>
    //*************************************************************************

    public Nullable<EdgeWorksheetReader.Visibility>
    Visibility
    {
        get
        {
            AssertValid();

            return (m_eVisibility);
        }

        set
        {
            m_eVisibility = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Label
    //
    /// <summary>
    /// Gets or sets the label that was applied to the selected edges.
    /// </summary>
    ///
    /// <value>
    /// The label that was applied to the selected edges, or null or empty if
    /// a label wasn't applied.
    /// </value>
    //*************************************************************************

    public String
    Label
    {
        get
        {
            AssertValid();

            return (m_sLabel);
        }

        set
        {
            m_sLabel = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LabelTextColor
    //
    /// <summary>
    /// Gets or sets the label text color that was applied to the selected
    /// edges.
    /// </summary>
    ///
    /// <value>
    /// The label text color that was applied to the selected edges, or null if
    /// a label text color wasn't applied.
    /// </value>
    //*************************************************************************

    public Nullable<Color>
    LabelTextColor
    {
        get
        {
            AssertValid();

            return (m_oLabelTextColor);
        }

        set
        {
            m_oLabelTextColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LabelFontSize
    //
    /// <summary>
    /// Gets or sets the label font size that was applied to the selected
    /// edges.
    /// </summary>
    ///
    /// <value>
    /// The label font size that was applied to the selected edges, or null if
    /// a size wasn't applied.
    /// </value>
    ///
    /// <remarks>
    /// If not null, the value is between <see
    /// cref="FontSizeConverter.MinimumFontSizeWorkbook" /> and <see
    /// cref="FontSizeConverter.MaximumFontSizeWorkbook" />.
    /// </remarks>
    //*************************************************************************

    public Nullable<Single>
    LabelFontSize
    {
        get
        {
            AssertValid();

            return (m_fLabelFontSize);
        }

        set
        {
            m_fLabelFontSize = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: WorkbookMustBeReread
    //
    /// <summary>
    /// Gets or sets a flag indicating if the caller must read the workbook
    /// again.
    /// </summary>
    ///
    /// <value>
    /// true if the caller must read the workbook again.
    /// </value>
    //*************************************************************************

    public Boolean
    WorkbookMustBeReread
    {
        get
        {
            AssertValid();

            return (m_bWorkbookMustBeReread);
        }

        set
        {
            m_bWorkbookMustBeReread = value;

            AssertValid();
        }
    }


    //*************************************************************************
    //  Method: AssertValid()
    //
    /// <summary>
    /// Asserts if the object is in an invalid state.  Debug-only.
    /// </summary>
    //*************************************************************************

    [Conditional("DEBUG")]

    public void
    AssertValid()
    {
        // m_oColor

        Debug.Assert(!m_fWidth.HasValue ||
            m_fWidth.Value >= EdgeWidthConverter.MinimumWidthWorkbook);

        Debug.Assert(!m_fWidth.HasValue ||
            m_fWidth.Value <= EdgeWidthConverter.MaximumWidthWorkbook);

        // m_eStyle

        Debug.Assert(!m_fAlpha.HasValue ||
            m_fAlpha.Value >= AlphaConverter.MinimumAlphaWorkbook);

        Debug.Assert(!m_fAlpha.HasValue ||
            m_fAlpha.Value <= AlphaConverter.MaximumAlphaWorkbook);

        // m_eVisibility
        // m_sLabel
        // m_oLabelTextColor

        Debug.Assert(!m_fLabelFontSize.HasValue ||
            m_fLabelFontSize.Value >=
                FontSizeConverter.MinimumFontSizeWorkbook);

        Debug.Assert(!m_fLabelFontSize.HasValue ||
            m_fLabelFontSize.Value <=
                FontSizeConverter.MaximumFontSizeWorkbook);

        // m_bWorkbookMustBeReread
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Color that was applied to the selected edges, or null if a color wasn't
    /// applied.

    protected Nullable<Color> m_oColor;

    /// Width that was applied to the selected edges, or null if a width wasn't
    /// applied.

    protected Nullable<Single> m_fWidth;

    /// Style that was applied to the selected edges, or null if a style wasn't
    /// applied.

    protected Nullable<EdgeStyle> m_eStyle;

    /// Alpha that was applied to the selected edges, or null if an alpha
    /// wasn't applied.

    protected Nullable<Single> m_fAlpha;

    /// Visibility that was applied to the selected edges, or null if a
    /// visibility wasn't applied.

    protected Nullable<EdgeWorksheetReader.Visibility> m_eVisibility;

    /// Label that was applied to the selected edges, or null if a label wasn't
    /// applied.

    protected String m_sLabel;

    /// Label text color that was applied to the selected edges, or null if a
    /// label color wasn't applied.

    protected Nullable<Color> m_oLabelTextColor;

    /// Label font size that was applied to the selected edges, or null if a
    /// size wasn't applied.

    protected Nullable<Single> m_fLabelFontSize;

    /// true if the caller must read the workbook again.

    protected Boolean m_bWorkbookMustBeReread;
}

}
