﻿
using System;
using System.Windows;
using System.Windows.Media;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.WpfGraphicsLib;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: VertexAndEdgeDrawerBase
//
/// <summary>
/// Base class for classes that draw vertices and edges.
/// </summary>
//*****************************************************************************

public class VertexAndEdgeDrawerBase : DrawerBase
{
    //*************************************************************************
    //  Constructor: VertexAndEdgeDrawerBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexAndEdgeDrawerBase" /> class.
    /// </summary>
    //*************************************************************************

    public VertexAndEdgeDrawerBase()
    {
        m_bUseSelection = true;
        m_oColor = SystemColors.WindowTextColor;
        m_oSelectedColor = SystemColors.HighlightColor;
        m_btFilteredAlpha = 10;
        m_oFormattedTextManager = new FormattedTextManager();
        m_iMaximumLabelLength = Int32.MaxValue;

        CreateDrawingObjects();

        // AssertValid();
    }

    //*************************************************************************
    //  Property: UseSelection
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the selected state of a vertex
    /// or edge should be used.
    /// </summary>
    ///
    /// <value>
    /// If true, a vertex or edge is drawn using either <see cref="Color" /> or
    /// <see cref="SelectedColor" />, depending on whether the vertex or edge
    /// has been marked as selected with the <see
    /// cref="ReservedMetadataKeys.IsSelected" /> key.  If false,
    /// <see cref="Color" /> is used regardless of whether the vertex or edge
    /// has been marked as selected.
    /// </value>
    //*************************************************************************

    public Boolean
    UseSelection
    {
        get
        {
            AssertValid();

            return (m_bUseSelection);
        }

        set
        {
            if (m_bUseSelection == value)
            {
                return;
            }

            m_bUseSelection = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Color
    //
    /// <summary>
    /// Gets or sets the default color of unselected vertices or edges.
    /// </summary>
    ///
    /// <value>
    /// The default color of unselected vertices or edges, as a <see
    /// cref="Color" />.  The default value is <see
    /// cref="SystemColors.WindowTextColor" />.
    /// </value>
    ///
    /// <remarks>
    /// See <see cref="UseSelection" /> for details on selected vs. unselected
    /// vertices and edges.
    ///
    /// <para>
    /// The default color of an unselected vertex or edge can be overridden by
    /// setting the <see cref="ReservedMetadataKeys.PerColor" /> key on the
    /// vertex or edge.  The key's value can be of type System.Drawing.Color or
    /// System.Windows.Media.Color.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="SelectedColor" />
    //*************************************************************************

    public Color
    Color
    {
        get
        {
            AssertValid();

            return (m_oColor);
        }

        set
        {
            if (m_oColor == value)
            {
                return;
            }

            m_oColor = value;

            CreateDrawingObjects();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: SelectedColor
    //
    /// <summary>
    /// Gets or sets the color of selected vertices or edges.
    /// </summary>
    ///
    /// <value>
    /// The color of selected vertices or edges, as a <see cref="Color" />.
    /// The default value is <see cref="SystemColors.HighlightColor" />.
    /// </value>
    ///
    /// <remarks>
    /// See <see cref="UseSelection" /> for details on selected vs. unselected
    /// vertices or edges.
    ///
    /// <para>
    /// The color of selected vertices and edges cannot be overridden on a
    /// per-vertex or per-edge basis.
    /// </para>
    ///
    /// </remarks>
    ///
    /// <seealso cref="Color" />
    //*************************************************************************

    public Color
    SelectedColor
    {
        get
        {
            AssertValid();

            return (m_oSelectedColor);
        }

        set
        {
            if (m_oSelectedColor == value)
            {
                return;
            }

            m_oSelectedColor = value;

            CreateDrawingObjects();

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FilteredAlpha
    //
    /// <summary>
    /// Gets or sets the alpha value to use for vertices and edges that are
    /// filtered.
    /// </summary>
    ///
    /// <value>
    /// The alpha value to use for vertices and edges that have a <see
    /// cref="ReservedMetadataKeys.Visibility" /> value of <see
    /// cref="VisibilityKeyValue.Filtered" />.  Must be between 0 (invisible)
    /// and 255 (opaque).  The default value is 10.
    /// </value>
    //*************************************************************************

    public Byte
    FilteredAlpha
    {
        get
        {
            AssertValid();

            return (m_btFilteredAlpha);
        }

        set
        {
            const String PropertyName = "FilteredAlpha";

            if (m_btFilteredAlpha == value)
            {
                return;
            }

            this.ArgumentChecker.CheckPropertyInRange(PropertyName, value,
                0, 255);

            m_btFilteredAlpha = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MaximumLabelLength
    //
    /// <summary>
    /// Gets or sets the maximum number of characters to show in a label.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of characters to show, or Int32.MaxValue for no
    /// maximum.  Must be greater than or equal to zero.  The default is
    /// Int32.MaxValue.
    /// </value>
    //*************************************************************************

    public Int32
    MaximumLabelLength
    {
        get
        {
            AssertValid();

            return (m_iMaximumLabelLength);
        }

        set
        {
            m_iMaximumLabelLength = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: SetFont()
    //
    /// <summary>
    /// Sets the font used to draw labels.
    /// </summary>
    ///
    /// <param name="typeface">
    /// The Typeface to use.
    /// </param>
    ///
    /// <param name="fontSize">
    /// The font size to use, in WPF units.
    /// </param>
    ///
    /// <remarks>
    /// The default font is the SystemFonts.MessageFontFamily at size 10.
    /// </remarks>
    //*************************************************************************

    public void
    SetFont
    (
        Typeface typeface,
        Double fontSize
    )
    {
        Debug.Assert(typeface != null);
        Debug.Assert(fontSize > 0);
        AssertValid();

        m_oFormattedTextManager.SetFont(typeface, fontSize);
    }

    //*************************************************************************
    //  Method: GetVisibility()
    //
    /// <summary>
    /// Gets the visibility of a vertex or edge.
    /// </summary>
    ///
    /// <param name="vertexOrEdge">
    /// The vertex or edge.
    /// </param>
    ///
    /// <returns>
    /// If the <see cref="ReservedMetadataKeys.Visibility" /> key is present on
    /// <paramref name="vertexOrEdge" />, the key's value is returned as a
    /// <see cref="VisibilityKeyValue" />.  Otherwise, <see
    /// cref="VisibilityKeyValue.Visible" /> is returned.
    /// </returns>
    //*************************************************************************

    public static VisibilityKeyValue
    GetVisibility
    (
        IMetadataProvider vertexOrEdge
    )
    {
        Debug.Assert(vertexOrEdge != null);

        Object oVisibilityKeyValue;

        if ( vertexOrEdge.TryGetValue(ReservedMetadataKeys.Visibility,
            typeof(VisibilityKeyValue), out oVisibilityKeyValue) )
        {
            return ( (VisibilityKeyValue)oVisibilityKeyValue );
        }

        return (VisibilityKeyValue.Visible);
    }

    //*************************************************************************
    //  Method: GetDrawAsSelected()
    //
    /// <summary>
    /// Gets a flag indicating whether a vertex or edge should be drawn as
    /// selected.
    /// </summary>
    ///
    /// <param name="vertexOrEdge">
    /// The vertex or edge.
    /// </param>
    ///
    /// <returns>
    /// true if the edge or vertex should be drawn as selected.
    /// </returns>
    //*************************************************************************

    public Boolean
    GetDrawAsSelected
    (
        IMetadataProvider vertexOrEdge
    )
    {
        Debug.Assert(vertexOrEdge != null);
        AssertValid();

        return ( m_bUseSelection &&
            vertexOrEdge.ContainsKey(ReservedMetadataKeys.IsSelected) );
    }

    //*************************************************************************
    //  Method: GetColor()
    //
    /// <overloads>
    /// Gets the color of a vertex or edge.
    /// </overloads>
    ///
    /// <summary>
    /// Gets the color of a vertex or edge.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// The vertex or edge to get the color for.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The visibility of the vertex or edge.  This can be obtained with <see
    /// cref="GetVisibility" />.
    /// </param>
    ///
    /// <param name="bDrawAsSelected">
    /// true if <paramref name="oVertexOrEdge" /> should be drawn as selected.
    /// </param>
    ///
    /// <returns>
    /// The color of the vertex or edge.  This includes any per-vertex or
    /// per-edge alpha value specified on the vertex or edge, along with the
    /// visibility specified by <paramref name="eVisibility" />.
    /// </returns>
    //*************************************************************************

    protected Color
    GetColor
    (
        IMetadataProvider oVertexOrEdge,
        VisibilityKeyValue eVisibility,
        Boolean bDrawAsSelected
    )
    {
        Debug.Assert(oVertexOrEdge != null);
        AssertValid();

        if (bDrawAsSelected)
        {
            Color oColor = m_oSelectedColor;

            if (eVisibility == VisibilityKeyValue.Filtered)
            {
                oColor.A = m_btFilteredAlpha;
            }

            return (oColor);
        }

        return ( GetColor(oVertexOrEdge, eVisibility,
            ReservedMetadataKeys.PerColor, m_oColor, true) );
    }

    //*************************************************************************
    //  Method: GetColor()
    //
    /// <summary>
    /// Gets a color for an unselected vertex or edge given a default color and
    /// a metadata key.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// The unselected vertex or edge to get a color for.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The visibility of the vertex or edge.  This can be obtained with <see
    /// cref="GetVisibility" />.  Not used if <paramref name="bApplyAlpha" />
    /// is false.
    /// </param>
    ///
    /// <param name="sKey">
    /// The metadata key to check for a per-vertex or per-edge color.
    /// </param>
    ///
    /// <param name="oDefaultColor">
    /// The default color to use if <paramref name="sKey" /> isn't specified
    /// on the vertex or edge.
    /// </param>
    ///
    /// <param name="bApplyAlpha">
    /// If true, <paramref name="eVisibility" /> and any per-vertex or per-edge
    /// alpha value is applied to the color.
    /// </param>
    ///
    /// <returns>
    /// The color for the vertex or edge.  If <paramref name="bApplyAlpha" />
    /// is true, this includes any per-vertex or per-edge alpha value specified
    /// on the vertex or edge, along with the visibility specified by <paramref
    /// name="eVisibility" />.
    /// </returns>
    //*************************************************************************

    protected Color
    GetColor
    (
        IMetadataProvider oVertexOrEdge,
        VisibilityKeyValue eVisibility,
        String sKey,
        Color oDefaultColor,
        Boolean bApplyAlpha
    )
    {
        Debug.Assert(oVertexOrEdge != null);
        Debug.Assert( !String.IsNullOrEmpty(sKey) );
        AssertValid();

        Byte btDefaultAlpha = oDefaultColor.A;

        // Start with the default color.

        Color oColor = oDefaultColor;

        // Check for a per-vertex or per-edge color.

        Color oPerColor;

        if ( TryGetColorValue(oVertexOrEdge, sKey, out oPerColor) )
        {
            oColor = oPerColor;
            oColor.A = btDefaultAlpha;
        }

        if (bApplyAlpha)
        {
            // Apply the vertex or edge's alpha.

            oColor.A = GetAlpha(oVertexOrEdge, eVisibility, oColor.A);
        }

        return (oColor);
    }

    //*************************************************************************
    //  Method: GetAlpha()
    //
    /// <summary>
    /// Get the alpha value to use for an unselected vertex or edge.
    /// </summary>
    ///
    /// <param name="oVertexOrEdge">
    /// The unselected vertex or edge to get the alpha value for.
    /// </param>
    ///
    /// <param name="eVisibility">
    /// The visibility of the vertex or edge.  This can be obtained with <see
    /// cref="GetVisibility" />.
    /// </param>
    ///
    /// <param name="btDefaultAlpha">
    /// The alpha value to return in the vertex or edge is visible and has no
    /// per-vertex or per-edge alpha.
    /// </param>
    ///
    /// <returns>
    /// The alpha value to use, between 0 (transparent) and 255 (opaque).
    /// </returns>
    //*************************************************************************

    protected Byte
    GetAlpha
    (
        IMetadataProvider oVertexOrEdge,
        VisibilityKeyValue eVisibility,
        Byte btDefaultAlpha
    )
    {
        Debug.Assert(oVertexOrEdge != null);
        AssertValid();

        if (eVisibility == VisibilityKeyValue.Filtered)
        {
            // The vertex or edge is filtered.

            return (m_btFilteredAlpha);
        }

        // Check for a per-vertex or per-edge alpha.

        Object oPerAlphaAsObject;

        if ( oVertexOrEdge.TryGetValue(ReservedMetadataKeys.PerAlpha,
            typeof(Single), out oPerAlphaAsObject) )
        {
            Single fPerAlpha = (Single)oPerAlphaAsObject;

            if (fPerAlpha < 0F || fPerAlpha > 255F)
            {
                Debug.Assert(oVertexOrEdge is IIdentityProvider);

                throw new FormatException( String.Format(

                    "{0}: The {1} with the ID {2} has an out-of-range"
                    + " {3} value.  Valid values are between 0 and 255."
                    ,
                    this.ClassName,
                    (oVertexOrEdge is IVertex) ? "vertex" : "edge",
                    ( (IIdentityProvider)oVertexOrEdge ).ID,
                    "ReservedMetadataKeys.PerAlpha"
                    ) );
            }

            return ( (Byte)fPerAlpha );
        }

        return (btDefaultAlpha);
    }

    //*************************************************************************
    //  Method: CreateDrawingObjects()
    //
    /// <summary>
    /// Creates a set of drawing objects for use by the derived class.
    /// </summary>
    //*************************************************************************

    protected void
    CreateDrawingObjects()
    {
        // AssertValid();

        m_oDefaultBrush = CreateFrozenSolidColorBrush(m_oColor);

        m_oDefaultPen = CreateFrozenPen(m_oDefaultBrush, DefaultPenThickness,
            DefaultDashStyle);
    }

    //*************************************************************************
    //  Method: TruncateLabel()
    //
    /// <summary>
    /// Truncates a label if it exceeds a maximum length.
    /// </summary>
    ///
    /// <param name="sLabel">
    /// The label to truncate.  Can't be null.
    /// </param>
    ///
    /// <returns>
    /// The original or truncated label.
    /// </returns>
    ///
    /// <returns>
    /// If <see cref="MaximumLabelLength" /> is not Int32.MaxValue, this
    /// method returns <paramref name="sLabel" /> truncated to the maximum
    /// number of characters.
    /// </returns>
    //*************************************************************************

    protected String
    TruncateLabel
    (
        String sLabel
    )
    {
        Debug.Assert(sLabel != null);
        AssertValid();

        return (
            (m_iMaximumLabelLength == Int32.MaxValue ||
                sLabel.Length <= m_iMaximumLabelLength) ?

            sLabel : sLabel.Substring(0, m_iMaximumLabelLength)
            );
    }

    //*************************************************************************
    //  Method: GetBrush()
    //
    /// <summary>
    /// Gets a SolidColorBrush to use to draw a vertex or edge.
    /// </summary>
    ///
    /// <param name="oColor">
    /// The vertex or edge color.
    /// </param>
    ///
    /// <returns>
    /// A SolidColorBrush to use to draw a vertex or edge.
    /// </returns>
    //*************************************************************************

    protected SolidColorBrush
    GetBrush
    (
        Color oColor
    )
    {
        AssertValid();

        if (oColor == m_oDefaultBrush.Color)
        {
            return (m_oDefaultBrush);
        }

        return ( CreateFrozenSolidColorBrush(oColor) );
    }

    //*************************************************************************
    //  Method: GetPen()
    //
    /// <overloads>
    /// Gets a pen to use to draw a vertex or edge.
    /// </overloads>
    ///
    /// <summary>
    /// Gets a solid pen to use to draw a vertex or edge.
    /// </summary>
    ///
    /// <param name="oColor">
    /// The vertex or edge color.
    /// </param>
    ///
    /// <param name="dThickness">
    /// The pen thickness.
    /// </param>
    ///
    /// <returns>
    /// A pen to use to draw a vertex or edge.  The pen has the default dash
    /// style, which is DashStyles.Solid.
    /// </returns>
    //*************************************************************************

    protected Pen
    GetPen
    (
        Color oColor,
        Double dThickness
    )
    {
        Debug.Assert(dThickness > 0);
        AssertValid();

        return ( GetPen(oColor, dThickness, DefaultDashStyle) );
    }

    //*************************************************************************
    //  Method: GetPen()
    //
    /// <summary>
    /// Gets a pen to use to draw a vertex or edge, using a specified dash
    /// style.
    /// </summary>
    ///
    /// <param name="oColor">
    /// The vertex or edge color.
    /// </param>
    ///
    /// <param name="dThickness">
    /// The pen thickness.
    /// </param>
    ///
    /// <param name="oDashStyle">
    /// The pen's dash style.
    /// </param>
    ///
    /// <returns>
    /// A pen to use to draw a vertex or edge.
    /// </returns>
    //*************************************************************************

    protected Pen
    GetPen
    (
        Color oColor,
        Double dThickness,
        DashStyle oDashStyle
    )
    {
        Debug.Assert(dThickness > 0);
        Debug.Assert(oDashStyle != null);
        AssertValid();

        Debug.Assert(m_oDefaultPen.Brush is SolidColorBrush);

        if (
            oColor == ( (SolidColorBrush)m_oDefaultPen.Brush ).Color &&
            dThickness == m_oDefaultPen.Thickness &&
            oDashStyle == DefaultDashStyle
            )
        {
            return (m_oDefaultPen);
        }

        return ( CreateFrozenPen( GetBrush(oColor), dThickness, oDashStyle) );
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

        // m_bUseSelection
        // m_oColor
        // m_oSelectedColor
        Debug.Assert(m_btFilteredAlpha >= 0);
        Debug.Assert(m_btFilteredAlpha <= 255);
        Debug.Assert(m_iMaximumLabelLength >= 0);
        Debug.Assert(m_oFormattedTextManager != null);
        Debug.Assert(m_oDefaultBrush != null);
        Debug.Assert(m_oDefaultPen != null);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Default pen thickness.

    protected const Double DefaultPenThickness = 1;

    /// Default pen dash style.

    protected static readonly DashStyle DefaultDashStyle = DashStyles.Solid;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true if vertices or edges marked as selected should be drawn as
    /// selected, false to draw all vertices or edges as unselected.

    protected Boolean m_bUseSelection;

    /// Color of an unselected vertex or edge.

    protected Color m_oColor;

    /// Color of a selected vertex or edge.

    protected Color m_oSelectedColor;

    /// Alpha value to use for vertices and edges that are filtered.

    protected Byte m_btFilteredAlpha;

    /// Manages the creation of FormattedText objects.

    protected FormattedTextManager m_oFormattedTextManager;

    /// The maximum number of characters to show in a label, or Int32.MaxValue
    /// for no maximum.

    protected Int32 m_iMaximumLabelLength;

    /// Default brush to use.

    protected SolidColorBrush m_oDefaultBrush;

    /// Default pen to use.

    protected Pen m_oDefaultPen;
}

}
