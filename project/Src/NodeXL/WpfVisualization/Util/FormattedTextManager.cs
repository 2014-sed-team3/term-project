
using System;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
using Smrf.WpfGraphicsLib;
using System.Diagnostics;

namespace Smrf.NodeXL.Visualization.Wpf
{
//*****************************************************************************
//  Class: FormattedTextManager
//
/// <summary>
/// Manages the creation of FormattedText objects.
/// </summary>
///
/// <remarks>
/// Call <see cref="CreateFormattedText(String, Color, Double)" /> to create a
/// FormattedText object.  The typeface and font used for creating
/// FormattedText objects can be set using the <see cref="SetFont" /> method.
/// </remarks>
//*****************************************************************************

public class FormattedTextManager : VisualizationBase
{
    //*************************************************************************
    //  Constructor: FormattedTextManager()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="FormattedTextManager" />
    /// class.
    /// </summary>
    //*************************************************************************

    public FormattedTextManager()
    {
        m_oTypeface = new Typeface(SystemFonts.MessageFontFamily,
            FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

        m_dFontSize = 10;

        AssertValid();
    }

    //*************************************************************************
    //  Property: FontSize
    //
    /// <summary>
    /// Gets the font size in use, in WPF units.
    /// </summary>
    ///
    /// <value>
    /// The font size in use.
    /// </value>
    ///
    /// <remarks>
    /// The font size can be set using <see cref="SetFont" />.
    /// </remarks>
    //*************************************************************************

    public Double
    FontSize
    {
        get
        {
            AssertValid();

            return (m_dFontSize);
        }
    }

    //*************************************************************************
    //  Property: Typeface
    //
    /// <summary>
    /// Gets the typeface in use.
    /// </summary>
    ///
    /// <value>
    /// The typeface in use.
    /// </value>
    ///
    /// <remarks>
    /// The typeface can be set using <see cref="SetFont" />.
    /// </remarks>
    //*************************************************************************

    public Typeface
    Typeface
    {
        get
        {
            AssertValid();

            return (m_oTypeface);
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

        m_oTypeface = typeface;
        m_dFontSize = fontSize;
    }

    //*************************************************************************
    //  Method: CreateFormattedText()
    //
    /// <overloads>
    /// Creates a FormattedText object.
    /// </overloads>
    ///
    /// <summary>
    /// Creates a FormattedText object using the current font size.
    /// </summary>
    ///
    /// <param name="text">
    /// The text to draw.  Can't be null.
    /// </param>
    ///
    /// <param name="color">
    /// The text color.
    /// </param>
    ///
    /// <param name="graphScale">
    /// The graph's scale.  Must be greater than 0.
    /// </param>
    //*************************************************************************

    public FormattedText
    CreateFormattedText
    (
        String text,
        Color color,
        Double graphScale
    )
    {
        Debug.Assert(text != null);
        Debug.Assert(graphScale > 0);

        return ( CreateFormattedText(text, color, m_dFontSize, graphScale) );
    }

    //*************************************************************************
    //  Method: CreateFormattedText()
    //
    /// <summary>
    /// Creates a FormattedText object using a specified font size.
    /// </summary>
    ///
    /// <param name="text">
    /// The text to draw.  Can't be null.
    /// </param>
    ///
    /// <param name="color">
    /// The text color.
    /// </param>
    ///
    /// <param name="dFontSize">
    /// The font size to use, in WPF units.  This gets multipled by <paramref
    /// name="graphScale" /> to determine the actual font size used.
    /// </param>
    ///
    /// <param name="graphScale">
    /// The graph's scale.  Must be greater than 0.
    /// </param>
    //*************************************************************************

    public FormattedText
    CreateFormattedText
    (
        String text,
        Color color,
        Double dFontSize,
        Double graphScale
    )
    {
        Debug.Assert(text != null);
        Debug.Assert(dFontSize >= 0);
        Debug.Assert(graphScale > 0);

        SolidColorBrush oBrush = new SolidColorBrush(color);
        WpfGraphicsUtil.FreezeIfFreezable(oBrush);

        FormattedText oFormattedText = new FormattedText(text,
            CultureInfo.CurrentCulture, FlowDirection.LeftToRight, m_oTypeface,
            dFontSize * graphScale, oBrush);

        return (oFormattedText);
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

        Debug.Assert(m_oTypeface != null);
        Debug.Assert(m_dFontSize > 0);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The Typeface to use to draw labels.

    protected Typeface m_oTypeface;

    /// The font size to use to draw labels, in WPF units.

    protected Double m_dFontSize;
}

}
