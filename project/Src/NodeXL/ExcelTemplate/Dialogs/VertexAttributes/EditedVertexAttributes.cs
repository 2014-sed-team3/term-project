
using System;
using System.Drawing;
using System.Diagnostics;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: EditedVertexAttributes
//
/// <summary>
/// Stores a list of vertex attributes that were edited by <see
/// cref="VertexAttributesDialog" />.
/// </summary>
///
/// <remarks>
/// The list of vertices whose attributes were edited can be obtained from <see
/// cref="NodeXLControl.SelectedVertices" />.
/// </remarks>
//*****************************************************************************

public class EditedVertexAttributes : Object
{
    //*************************************************************************
    //  Constructor: EditedVertexAttributes()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="EditedVertexAttributes" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EditedVertexAttributes" /> class with default values.
    /// </summary>
    ///
    /// <remarks>
    /// All values are set to null.
    /// </remarks>
    //*************************************************************************

    public EditedVertexAttributes()
    :
    this(null, null, null, null, null, null, null, null, null, null, null,
        false)
    {
        // (Do nothing else.)

        AssertValid();
    }

    //*************************************************************************
    //  Constructor: EditedVertexAttributes()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="EditedVertexAttributes" /> class with specified values.
    /// </summary>
    ///
    /// <param name="color">
    /// The color that was applied to the selected vertices, or null if a color
    /// wasn't applied.
    /// </param>
    ///
    /// <param name="shape">
    /// The shape that was applied to the selected vertices, or null if a shape
    /// wasn't applied.
    /// </param>
    ///
    /// <param name="radius">
    /// The radius that was applied to the selected vertices, or null if a
    /// radius wasn't applied.  If not null, the radius must be between <see
    /// cref="VertexRadiusConverter.MinimumRadiusWorkbook" /> and <see
    /// cref="VertexRadiusConverter.MaximumRadiusWorkbook" />.
    /// </param>
    ///
    /// <param name="alpha">
    /// The alpha that was applied to the selected vertices, or null if an
    /// alpha wasn't applied.  If not null, the alpha must be between <see
    /// cref="AlphaConverter.MinimumAlphaWorkbook" /> and <see
    /// cref="AlphaConverter.MaximumAlphaWorkbook" />.
    /// </param>
    ///
    /// <param name="visibility">
    /// The visibility that was applied to the selected vertices, or null if a
    /// visibility wasn't applied.
    /// </param>
    ///
    /// <param name="label">
    /// The label that was applied to the selected vertices, or null if a label
    /// wasn't applied.
    /// </param>
    ///
    /// <param name="labelFillColor">
    /// The label fill color that was applied to the selected vertices, or null
    /// if a label fill color wasn't applied.
    /// </param>
    ///
    /// <param name="labelPosition">
    /// The label position that was applied to the selected vertices, or null
    /// if a label position wasn't applied.
    /// </param>
    ///
    /// <param name="toolTip">
    /// The tooltip that was applied to the selected vertices, or null if a
    /// tooltip wasn't applied.
    /// </param>
    ///
    /// <param name="locked">
    /// The locked flag that was applied to the selected vertices, or null if a
    /// locked flag wasn't applied.
    /// </param>
    ///
    /// <param name="marked">
    /// The "marked" flag that was applied to the selected vertices, or null if
    /// a marked flag wasn't applied.
    /// </param>
    ///
    /// <param name="workbookMustBeReread">
    /// true if the caller must read the workbook again.
    /// </param>
    //*************************************************************************

    public EditedVertexAttributes
    (
        Nullable<Color> color,
        Nullable<VertexShape> shape,
        Nullable<Single> radius,
        Nullable<Single> alpha,
        Nullable<VertexWorksheetReader.Visibility> visibility,
        String label,
        Nullable<Color> labelFillColor,
        Nullable<VertexLabelPosition> labelPosition,
        String toolTip,
        Nullable<Boolean> locked,
        Nullable<Boolean> marked,
        Boolean workbookMustBeReread
    )
    {
        m_oColor = color;
        m_eShape = shape;
        m_fRadius = radius;
        m_fAlpha = alpha;
        m_eVisibility = visibility;
        m_sLabel = label;
        m_oLabelFillColor = labelFillColor;
        m_eLabelPosition = labelPosition;
        m_sToolTip = toolTip;
        m_bLocked = locked;
        m_bMarked = marked;
        m_bWorkbookMustBeReread = workbookMustBeReread;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Color
    //
    /// <summary>
    /// Gets or sets the color that was applied to the selected vertices.
    /// </summary>
    ///
    /// <value>
    /// The color that was applied to the selected vertices, or null if a color
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
    //  Property: Shape
    //
    /// <summary>
    /// Gets or sets the shape that was applied to the selected vertices.
    /// </summary>
    ///
    /// <value>
    /// The shape that was applied to the selected vertices, or null if a shape
    /// wasn't applied.
    /// </value>
    //*************************************************************************

    public Nullable<VertexShape>
    Shape
    {
        get
        {
            AssertValid();

            return (m_eShape);
        }

        set
        {
            m_eShape = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Radius
    //
    /// <summary>
    /// Gets or sets the radius that was applied to the selected vertices.
    /// </summary>
    ///
    /// <value>
    /// The radius that was applied to the selected vertices, or null if a
    /// radius wasn't applied.
    /// </value>
    ///
    /// <remarks>
    /// If not null, the value is between <see
    /// cref="VertexRadiusConverter.MinimumRadiusWorkbook" /> and <see
    /// cref="VertexRadiusConverter.MaximumRadiusWorkbook" />.
    /// </remarks>
    //*************************************************************************

    public Nullable<Single>
    Radius
    {
        get
        {
            AssertValid();

            return (m_fRadius);
        }

        set
        {
            m_fRadius = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Alpha
    //
    /// <summary>
    /// Gets or sets the alpha that was applied to the selected vertices.
    /// </summary>
    ///
    /// <value>
    /// The alpha that was applied to the selected vertices, or null if an
    /// alpha wasn't applied.
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
    /// Gets or sets the visibility that was applied to the selected vertices.
    /// </summary>
    ///
    /// <value>
    /// The visibility that was applied to the selected vertices, or null if a
    /// visibility wasn't applied.
    /// </value>
    //*************************************************************************

    public Nullable<VertexWorksheetReader.Visibility>
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
    /// Gets or sets the label that was applied to the selected vertices.
    /// </summary>
    ///
    /// <value>
    /// The label that was applied to the selected vertices, or null or empty
    /// if a label wasn't applied.
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
    //  Property: LabelFillColor
    //
    /// <summary>
    /// Gets or sets the label fill color that was applied to the selected
    /// vertices.
    /// </summary>
    ///
    /// <value>
    /// The label fill color that was applied to the selected vertices, or null
    /// if a label fill color wasn't applied.
    /// </value>
    //*************************************************************************

    public Nullable<Color>
    LabelFillColor
    {
        get
        {
            AssertValid();

            return (m_oLabelFillColor);
        }

        set
        {
            m_oLabelFillColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LabelPosition
    //
    /// <summary>
    /// Gets or sets the label position that was applied to the selected
    /// vertices.
    /// </summary>
    ///
    /// <value>
    /// The label position that was applied to the selected vertices, or null
    /// if a label position wasn't applied.
    /// </value>
    //*************************************************************************

    public Nullable<VertexLabelPosition>
    LabelPosition
    {
        get
        {
            AssertValid();

            return (m_eLabelPosition);
        }

        set
        {
            m_eLabelPosition = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ToolTip
    //
    /// <summary>
    /// Gets or sets the tooltip that was applied to the selected vertices.
    /// </summary>
    ///
    /// <value>
    /// The tooltip that was applied to the selected vertices, or null or
    /// empty if a tooltip wasn't applied.
    /// </value>
    //*************************************************************************

    public String
    ToolTip
    {
        get
        {
            AssertValid();

            return (m_sToolTip);
        }

        set
        {
            m_sToolTip = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Locked
    //
    /// <summary>
    /// Gets or sets the locked flag that was applied to the selected vertices.
    /// </summary>
    ///
    /// <value>
    /// The locked flag that was applied to the selected vertices, or null if a
    /// locked flag wasn't applied.
    /// </value>
    //*************************************************************************

    public Nullable<Boolean>
    Locked
    {
        get
        {
            AssertValid();

            return (m_bLocked);
        }

        set
        {
            m_bLocked = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Marked
    //
    /// <summary>
    /// Gets or sets the "marked" flag that was applied to the selected
    /// vertices.
    /// </summary>
    ///
    /// <value>
    /// The marked flag that was applied to the selected vertices, or null if a
    /// marked flag wasn't applied.
    /// </value>
    //*************************************************************************

    public Nullable<Boolean>
    Marked
    {
        get
        {
            AssertValid();

            return (m_bMarked);
        }

        set
        {
            m_bMarked = value;

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
        // m_eShape

        Debug.Assert(!m_fRadius.HasValue ||
            m_fRadius.Value >= VertexRadiusConverter.MinimumRadiusWorkbook);

        Debug.Assert(!m_fRadius.HasValue ||
            m_fRadius.Value <= VertexRadiusConverter.MaximumRadiusWorkbook);

        Debug.Assert(!m_fAlpha.HasValue ||
            m_fAlpha.Value >= AlphaConverter.MinimumAlphaWorkbook);

        Debug.Assert(!m_fAlpha.HasValue ||
            m_fAlpha.Value <= AlphaConverter.MaximumAlphaWorkbook);

        // m_eVisibility
        // m_sLabel
        // m_oLabelFillColor
        // m_eLabelPosition
        // m_sToolTip
        // m_bLocked
        // m_bMarked
        // m_bWorkbookMustBeReread
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Color that was applied to the selected vertices, or null if a color
    /// wasn't applied.

    protected Nullable<Color> m_oColor;

    /// Shape that was applied to the selected vertices, or null if a shape
    /// wasn't applied.

    protected Nullable<VertexShape> m_eShape;

    /// Radius that was applied to the selected vertices, or null if a radius
    /// wasn't applied.

    protected Nullable<Single> m_fRadius;

    /// Alpha that was applied to the selected vertices, or null if an alpha
    /// wasn't applied.

    protected Nullable<Single> m_fAlpha;

    /// Visibility that was applied to the selected vertices, or null if a
    /// visibility wasn't applied.

    protected Nullable<VertexWorksheetReader.Visibility> m_eVisibility;

    /// Label that was applied to the selected vertices, or null if a label
    /// wasn't applied.

    protected String m_sLabel;

    /// Label fill color that was applied to the selected vertices, or null if
    /// a label color wasn't applied.

    protected Nullable<Color> m_oLabelFillColor;

    /// Label position that was applied to the selected vertices, or null if a
    /// label position wasn't applied.

    protected Nullable<VertexLabelPosition> m_eLabelPosition;

    /// Tooltip that was applied to the selected vertices, or null if a
    /// tooltip wasn't applied.

    protected String m_sToolTip;

    /// Locked flag that was applied to the selected vertices, or null if a
    /// locked flag wasn't applied.

    protected Nullable<Boolean> m_bLocked;

    /// "Mark" flag that should be set in the workbook for the selected
    /// vertices, or null if a marked flag shouldn't be set.

    protected Nullable<Boolean> m_bMarked;

    /// true if the caller must read the workbook again.

    protected Boolean m_bWorkbookMustBeReread;
}

}
