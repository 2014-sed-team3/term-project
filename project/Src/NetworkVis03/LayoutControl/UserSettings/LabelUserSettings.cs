using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using LayoutControls.ValueConverter;
using Smrf.AppLib;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.WpfGraphicsLib;

namespace LayoutControls.UserSettings
{
//*****************************************************************************
//  Class: LabelUserSettings
//
/// <summary>
/// Stores the user's settings for graph labels.
/// </summary>
//*****************************************************************************

[ TypeConverter( typeof(LabelUserSettingsTypeConverter) ) ]

public class LabelUserSettings : Object
{
    //*************************************************************************
    //  Constructor: LabelUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the LabelUserSettings class.
    /// </summary>
    //*************************************************************************

    public LabelUserSettings()
    {
        m_oFont = (Font)( new FontConverter() ).ConvertFromInvariantString(
            GeneralUserSettings.DefaultFont);

        m_oVertexLabelFillColor = Color.White;
        m_eVertexLabelPosition = VertexLabelPosition.BottomCenter;

        m_iVertexLabelMaximumLength = m_iEdgeLabelMaximumLength =
            Int32.MaxValue;

        m_bVertexLabelWrapText = true;
        m_dVertexLabelWrapMaxTextWidth = 200;

        m_oEdgeLabelTextColor = Color.Black;

        m_oGroupLabelTextColor = Color.Black;
        m_fGroupLabelTextAlpha = 86;
        m_eGroupLabelPosition = VertexLabelPosition.MiddleCenter;

        AssertValid();
    }

    //*************************************************************************
    //  Property: Font
    //
    /// <summary>
    /// Gets or sets the font used for the graph's labels.
    /// </summary>
    ///
    /// <value>
    /// The label font, as a Font.  The default value is Microsoft Sans Serif,
    /// 8.25pt.
    /// </value>
    //*************************************************************************

    public Font
    Font
    {
        // Note that the font type is System.Drawing.Font, which is what the
        // System.Windows.Forms.FontDialog class uses.  It gets converted to
        // WPF font types in TransferToGraphDrawer().

        get
        {
            AssertValid();

            return (m_oFont);
        }

        set
        {
            m_oFont = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelFillColor
    //
    /// <summary>
    /// Gets or sets the fill color of vertices that that have the Label shape.
    /// </summary>
    ///
    /// <value>
    /// The fill color of vertices that have the Label shape, as a Color.  The
    /// default value is Color.White.
    /// </value>
    //*************************************************************************

    public Color
    VertexLabelFillColor
    {
        get
        {
            AssertValid();

            return (m_oVertexLabelFillColor);
        }

        set
        {
            m_oVertexLabelFillColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelPosition
    //
    /// <summary>
    /// Gets or sets the position of a vertex label drawn as an annotation.
    /// </summary>
    ///
    /// <value>
    /// The position of a vertex label drawn as an annotation.  The
    /// default value is <see
    /// cref="Smrf.NodeXL.Visualization.Wpf.VertexLabelPosition.BottomCenter" />.
    /// </value>
    //*************************************************************************

    public VertexLabelPosition
    VertexLabelPosition
    {
        get
        {
            AssertValid();

            return (m_eVertexLabelPosition);
        }

        set
        {
            m_eVertexLabelPosition = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelMaximumLength
    //
    /// <summary>
    /// Gets or sets the maximum number of characters to show in a vertex
    /// label.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of characters to show, or Int32.MaxValue for no
    /// maximum.  Must be greater than or equal to zero.  The default is
    /// Int32.MaxValue.
    /// </value>
    //*************************************************************************

    public Int32
    VertexLabelMaximumLength
    {
        get
        {
            AssertValid();

            return (m_iVertexLabelMaximumLength);
        }

        set
        {
            m_iVertexLabelMaximumLength = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelWrapText
    //
    /// <summary>
    /// Gets or sets a flag indicating whether vertex label text should be
    /// wrapped.
    /// </summary>
    ///
    /// <value>
    /// true to wrap vertex label text.  The default is true.
    /// </value>
    ///
    /// <remarks>
    /// If true, the label text is wrapped at approximately <see
    /// cref="VertexLabelWrapMaxTextWidth"/> WPF units.
    /// </remarks>
    //*************************************************************************

    public Boolean
    VertexLabelWrapText
    {
        get
        {
            AssertValid();

            return (m_bVertexLabelWrapText);
        }

        set
        {
            m_bVertexLabelWrapText = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: VertexLabelWrapMaxTextWidth
    //
    /// <summary>
    /// Gets or sets the maximum line length for vertex label text.
    /// </summary>
    ///
    /// <value>
    /// The maximum line length for vertex label text, in WPF units.  Must be
    /// greater than zero.  The default is 200.
    /// </value>
    ///
    /// <remarks>
    /// This is used only if <see cref="VertexLabelWrapText" /> is true.
    /// </remarks>
    //*************************************************************************

    public Double
    VertexLabelWrapMaxTextWidth
    {
        get
        {
            AssertValid();

            return (m_dVertexLabelWrapMaxTextWidth);
        }

        set
        {
            m_dVertexLabelWrapMaxTextWidth = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeLabelTextColor
    //
    /// <summary>
    /// Gets or sets the color of edge label text.
    /// </summary>
    ///
    /// <value>
    /// The color of edge label text, as a Color.  The default value is
    /// Color.Black.
    /// </value>
    //*************************************************************************

    public Color
    EdgeLabelTextColor
    {
        get
        {
            AssertValid();

            return (m_oEdgeLabelTextColor);
        }

        set
        {
            m_oEdgeLabelTextColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: EdgeLabelMaximumLength
    //
    /// <summary>
    /// Gets or sets the maximum number of characters to show in an edge label.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of characters to show, or Int32.MaxValue for no
    /// maximum.  Must be greater than or equal to zero.  The default is
    /// Int32.MaxValue.
    /// </value>
    //*************************************************************************

    public Int32
    EdgeLabelMaximumLength
    {
        get
        {
            AssertValid();

            return (m_iEdgeLabelMaximumLength);
        }

        set
        {
            m_iEdgeLabelMaximumLength = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: GroupLabelTextColor
    //
    /// <summary>
    /// Gets or sets the color of group label text when groups are laid out in
    /// boxes.
    /// </summary>
    ///
    /// <value>
    /// The color of group label text, as a Color.  The default value is
    /// Color.Black.
    /// </value>
    //*************************************************************************

    public Color
    GroupLabelTextColor
    {
        get
        {
            AssertValid();

            return (m_oGroupLabelTextColor);
        }

        set
        {
            m_oGroupLabelTextColor = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: GroupLabelTextAlpha
    //
    /// <summary>
    /// Gets or sets the alpha value to use for group label text when groups
    /// are laid out in boxes.
    /// </summary>
    ///
    /// <value>
    /// The alpha component of the color of group label text.  Must be between
    /// AlphaConverter.MinimumAlphaWorkbook and
    /// AlphaConverter.MaximumAlphaWorkbook.  The default value is 86.
    /// </value>
    //*************************************************************************

    public Single
    GroupLabelTextAlpha
    {
        get
        {
            AssertValid();

            return (m_fGroupLabelTextAlpha);
        }

        set
        {
            m_fGroupLabelTextAlpha = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: GroupLabelPosition
    //
    /// <summary>
    /// Gets or sets the position of a group label when groups are laid out in
    /// boxes.
    /// </summary>
    ///
    /// <value>
    /// The position of group labels.  The default value is <see
    /// cref="Smrf.NodeXL.Visualization.Wpf.VertexLabelPosition.MiddleCenter" />.
    /// </value>
    //*************************************************************************

    public VertexLabelPosition
    GroupLabelPosition
    {
        get
        {
            AssertValid();

            return (m_eGroupLabelPosition);
        }

        set
        {
            m_eGroupLabelPosition = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: Copy()
    //
    /// <summary>
    /// Creates a deep copy of the object.
    /// </summary>
    ///
    /// <returns>
    /// A deep copy of the object.
    /// </returns>
    //*************************************************************************

    public LabelUserSettings
    Copy()
    {
        AssertValid();

        LabelUserSettings oCopy = new LabelUserSettings();

        oCopy.Font = this.Font;
        oCopy.VertexLabelFillColor = this.VertexLabelFillColor;
        oCopy.VertexLabelPosition = this.VertexLabelPosition;
        oCopy.VertexLabelMaximumLength = this.VertexLabelMaximumLength;
        oCopy.VertexLabelWrapText = this.VertexLabelWrapText;
        oCopy.VertexLabelWrapMaxTextWidth = this.VertexLabelWrapMaxTextWidth;
        oCopy.EdgeLabelTextColor = this.EdgeLabelTextColor;
        oCopy.EdgeLabelMaximumLength = this.EdgeLabelMaximumLength;
        oCopy.GroupLabelTextColor = this.GroupLabelTextColor;
        oCopy.GroupLabelTextAlpha = this.GroupLabelTextAlpha;
        oCopy.GroupLabelPosition = this.GroupLabelPosition;

        return (oCopy);
    }

    //*************************************************************************
    //  Method: TransferToGraphDrawer()
    //
    /// <summary>
    /// Transfers the settings to a <see cref="GraphDrawer" />.
    /// </summary>
    ///
    /// <param name="graphDrawer">
    /// Graph drawer to transfer the settings to.
    /// </param>
    //*************************************************************************

    public void
    TransferToGraphDrawer
    (
        GraphDrawer graphDrawer
    )
    {
        Debug.Assert(graphDrawer != null);
        AssertValid();

        Font oFont = this.Font;

        System.Windows.Media.Typeface oTypeface =
            WpfGraphicsUtil.FontToTypeface(oFont);

        Double dFontSize = WpfGraphicsUtil.SystemDrawingFontSizeToWpfFontSize(
            oFont.Size);

        VertexDrawer oVertexDrawer = graphDrawer.VertexDrawer;

        oVertexDrawer.SetFont(oTypeface, dFontSize);

        oVertexDrawer.LabelFillColor =
            WpfGraphicsUtil.ColorToWpfColor(this.VertexLabelFillColor);

        oVertexDrawer.LabelPosition = this.VertexLabelPosition;
        oVertexDrawer.MaximumLabelLength = this.VertexLabelMaximumLength;
        oVertexDrawer.LabelWrapText = this.VertexLabelWrapText;

        oVertexDrawer.LabelWrapMaxTextWidth =
            this.VertexLabelWrapMaxTextWidth;

        EdgeDrawer oEdgeDrawer = graphDrawer.EdgeDrawer;

        oEdgeDrawer.SetFont(oTypeface, dFontSize);

        oEdgeDrawer.LabelTextColor =
            WpfGraphicsUtil.ColorToWpfColor(this.EdgeLabelTextColor);

        oEdgeDrawer.MaximumLabelLength = this.EdgeLabelMaximumLength;

        GroupDrawer oGroupDrawer = graphDrawer.GroupDrawer;

        oGroupDrawer.SetFont(oTypeface, GroupLabelFontSize);

        oGroupDrawer.LabelTextColor = WpfGraphicsUtil.ColorToWpfColor(

            Color.FromArgb( (new AlphaConverter() ).WorkbookToGraphAsByte(
                this.GroupLabelTextAlpha),

                this.GroupLabelTextColor)
            );

        oGroupDrawer.LabelPosition = this.GroupLabelPosition;
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
        Debug.Assert(m_oFont != null);
        // m_oVertexLabelFillColor
        // m_eVertexLabelPosition
        Debug.Assert(m_iVertexLabelMaximumLength >= 0);
        // m_bVertexLabelWrapText
        Debug.Assert(m_dVertexLabelWrapMaxTextWidth > 0);
        // m_oEdgeLabelTextColor
        Debug.Assert(m_iEdgeLabelMaximumLength >= 0);
        // m_oGroupLabelTextColor
        // m_eGroupLabelPosition

        Debug.Assert(m_fGroupLabelTextAlpha >=
            AlphaConverter.MinimumAlphaWorkbook);

        Debug.Assert(m_fGroupLabelTextAlpha <=
            AlphaConverter.MaximumAlphaWorkbook);
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Font size to use for group labels, in WPF units.  This is hard-coded
    /// for now.  It may be turned into a user setting in a future release.

    protected const Double GroupLabelFontSize = 19.0;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Font.

    protected Font m_oFont;

    /// The fill color of vertices that have the Label shape.

    protected Color m_oVertexLabelFillColor;

    /// The position of a vertex label drawn as an annotation.

    protected VertexLabelPosition m_eVertexLabelPosition;

    /// The maximum number of characters to show in a vertex label, or
    /// Int32.MaxValue for no maximum.

    protected Int32 m_iVertexLabelMaximumLength;

    /// true to wrap vertex label text.

    protected Boolean m_bVertexLabelWrapText;

    /// The maximum line length for vertex label text.

    protected Double m_dVertexLabelWrapMaxTextWidth;

    /// The color of edge label text.

    protected Color m_oEdgeLabelTextColor;

    /// The maximum number of characters to show in an edge label, or
    /// Int32.MaxValue for no maximum.

    protected Int32 m_iEdgeLabelMaximumLength;

    /// The color of group label text when groups are laid out in boxes.

    protected Color m_oGroupLabelTextColor;

    /// Alpha value of group label text when groups are laid out in boxes,
    /// between AlphaConverter.MinimumAlphaWorkbook and
    /// AlphaConverter.MaximumAlphaWorkbook.

    protected Single m_fGroupLabelTextAlpha;

    /// The position of a group label when groups are laid out in boxes.

    protected VertexLabelPosition m_eGroupLabelPosition;
}


//*****************************************************************************
//  Class: LabelUserSettingsTypeConverter
//
/// <summary>
/// Converts a LabelUserSettings object to and from a String.
/// </summary>
/// 
/// <remarks>
/// One of the properties of <see cref="GeneralUserSettings" /> is of type <see
/// cref="LabelUserSettings" />.  The application settings architecture
/// requires a type converter for such a complex type.
/// </remarks>
//*****************************************************************************

public class LabelUserSettingsTypeConverter : TypeConverter
{
    //*************************************************************************
    //  Constructor: LabelUserSettingsTypeConverter()
    //
    /// <summary>
    /// Initializes a new instance of the LabelUserSettingsTypeConverter class.
    /// </summary>
    //*************************************************************************

    public LabelUserSettingsTypeConverter()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Method: CanConvertTo()
    //
    /// <summary>
    /// Returns whether this converter can convert the object to the specified
    /// type, using the specified context.
    /// </summary>
    ///
    /// <param name="context">
    /// An ITypeDescriptorContext that provides a format context. 
    /// </param>
    ///
    /// <param name="sourceType">
    /// A Type that represents the type you want to convert to. 
    /// </param>
    ///
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    //*************************************************************************

    public override Boolean
    CanConvertTo
    (
        ITypeDescriptorContext context,
        Type sourceType
    )
    {
        AssertValid();

        return ( sourceType == typeof(String) );
    }

    //*************************************************************************
    //  Method: CanConvertFrom()
    //
    /// <summary>
    /// Returns whether this converter can convert an object of the given type
    /// to the type of this converter, using the specified context.
    /// </summary>
    ///
    /// <param name="context">
    /// An ITypeDescriptorContext that provides a format context. 
    /// </param>
    ///
    /// <param name="sourceType">
    /// A Type that represents the type you want to convert from. 
    /// </param>
    ///
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    //*************************************************************************

    public override Boolean
    CanConvertFrom
    (
        ITypeDescriptorContext context,
        Type sourceType
    )
    {
        AssertValid();

        return ( sourceType == typeof(String) );
    }

    //*************************************************************************
    //  Method: ConvertTo()
    //
    /// <summary>
    /// Converts the given value object to the specified type, using the
    /// specified context and culture information.
    /// </summary>
    ///
    /// <param name="context">
    /// An ITypeDescriptorContext that provides a format context. 
    /// </param>
    ///
    /// <param name="culture">
    /// A CultureInfo. If null is passed, the current culture is assumed. 
    /// </param>
    ///
    /// <param name="value">
    /// The Object to convert.
    /// </param>
    ///
    /// <param name="destinationType">
    /// The Type to convert the value parameter to. 
    /// </param>
    ///
    /// <returns>
    /// An Object that represents the converted value.
    /// </returns>
    //*************************************************************************

    public override Object
    ConvertTo
    (
        ITypeDescriptorContext context,
        CultureInfo culture,
        Object value,
        Type destinationType
    )
    {
        Debug.Assert(value != null);
        Debug.Assert(value is LabelUserSettings);
        Debug.Assert( destinationType == typeof(String) );
        AssertValid();

        LabelUserSettings oLabelUserSettings = (LabelUserSettings)value;

        // Use a simple tab-delimited format.  Note that newer settings have
        // been added to the end, so related settings are not all contiguous.
        // Sample string:
        //
        // "Microsoft Sans Serif, 8.25pt\tWhite\tBottomCenter\t2147483647\t
        // 4294967295\tBlack\ttrue\t200\tBlack\t86\tMiddleCenter";

        ColorConverter oColorConverter = new ColorConverter();

        return ( String.Format(CultureInfo.InvariantCulture,

            "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}"
            ,
            ( new FontConverter() ).ConvertToInvariantString(
                oLabelUserSettings.Font),

            oColorConverter.ConvertToInvariantString(
                oLabelUserSettings.VertexLabelFillColor),

            oLabelUserSettings.VertexLabelPosition,
            oLabelUserSettings.VertexLabelMaximumLength,
            oLabelUserSettings.EdgeLabelMaximumLength,

            oColorConverter.ConvertToInvariantString(
                oLabelUserSettings.EdgeLabelTextColor),

            oLabelUserSettings.VertexLabelWrapText,
            oLabelUserSettings.VertexLabelWrapMaxTextWidth,

            oColorConverter.ConvertToInvariantString(
                oLabelUserSettings.GroupLabelTextColor),

            oLabelUserSettings.GroupLabelTextAlpha,
            oLabelUserSettings.GroupLabelPosition
            ) );
    }

    //*************************************************************************
    //  Method: ConvertFrom()
    //
    /// <summary>
    /// Converts the given object to the type of this converter, using the
    /// specified context and culture information.
    /// </summary>
    ///
    /// <param name="context">
    /// An ITypeDescriptorContext that provides a format context. 
    /// </param>
    ///
    /// <param name="culture">
    /// A CultureInfo. If null is passed, the current culture is assumed. 
    /// </param>
    ///
    /// <param name="value">
    /// The Object to convert.
    /// </param>
    ///
    /// <returns>
    /// An Object that represents the converted value.
    /// </returns>
    //*************************************************************************

    public override Object
    ConvertFrom
    (
        ITypeDescriptorContext context,
        CultureInfo culture,
        Object value
    )
    {
        Debug.Assert(value != null);
        Debug.Assert(value is String);
        AssertValid();

        LabelUserSettings oLabelUserSettings = new LabelUserSettings();

        String [] asStrings = ( (String)value ).Split( new Char[] {'\t'} );

        Debug.Assert(asStrings.Length >= 5);

        ColorConverter oColorConverter = new ColorConverter();

        oLabelUserSettings.Font = (Font)
            ( new FontConverter() ).ConvertFromInvariantString(asStrings[0] );

        oLabelUserSettings.VertexLabelFillColor = (Color)
            oColorConverter.ConvertFromInvariantString(asStrings[1] );

        oLabelUserSettings.VertexLabelPosition = (VertexLabelPosition)
            Enum.Parse( typeof(VertexLabelPosition), asStrings[2] );

        oLabelUserSettings.VertexLabelMaximumLength =
            MathUtil.ParseCultureInvariantInt32( asStrings[3] );

        oLabelUserSettings.EdgeLabelMaximumLength =
            MathUtil.ParseCultureInvariantInt32(asStrings[4]);

        if (asStrings.Length > 5)
        {
            // Edge label text color wasn't added until version 1.0.1.154.

            oLabelUserSettings.EdgeLabelTextColor = (Color)
                oColorConverter.ConvertFromInvariantString(asStrings[5] );
        }

        if (asStrings.Length > 6)
        {
            // Vertex label wrapping wasn't added until version 1.0.1.175.

            oLabelUserSettings.VertexLabelWrapText =
                Boolean.Parse( asStrings[6] );

            oLabelUserSettings.VertexLabelWrapMaxTextWidth =
                MathUtil.ParseCultureInvariantDouble( asStrings[7] );
        }

        if (asStrings.Length > 8)
        {
            // Group label text color and alpha weren't added until version
            // 1.0.1.190.

            oLabelUserSettings.GroupLabelTextColor = (Color)
                oColorConverter.ConvertFromInvariantString( asStrings[8] );

            oLabelUserSettings.GroupLabelTextAlpha =
                MathUtil.ParseCultureInvariantSingle( asStrings[9] );
        }

        if (asStrings.Length > 10)
        {
            // Group label position wasn't added until version 1.0.1.215.

            oLabelUserSettings.GroupLabelPosition = (VertexLabelPosition)
                Enum.Parse( typeof(VertexLabelPosition), asStrings[10] );
        }

        return (oLabelUserSettings);
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
