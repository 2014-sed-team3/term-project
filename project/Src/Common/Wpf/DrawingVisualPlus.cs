
using System;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Diagnostics;

namespace Smrf.WpfGraphicsLib
{
//*****************************************************************************
//  Class: DrawingVisualPlus
//
/// <summary>
/// Represents a DrawingVisual with additional features.
/// </summary>
///
/// <remarks>
/// The <see cref="SetEffect" /> method can be used to set an Effect.
/// </remarks>
//*****************************************************************************

public class DrawingVisualPlus : DrawingVisual
{
    //*************************************************************************
    //  Constructor: DrawingVisualPlus()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="DrawingVisualPlus" />
    /// class.
    /// </summary>
    //*************************************************************************

    public DrawingVisualPlus()
    {
        // (Do nothing else.)

        AssertValid();
    }


    //*************************************************************************
    //  Method: SetEffect()
    //
    /// <summary>
    /// Sets an effect on the DrawingVisual.
    /// </summary>
    ///
    /// <param name="effect">
    /// The Effect to set.  Can be null.
    /// </param>
    ///
    /// <remarks>
    /// Due to an apparent programming mistake, the VisualEffect property on
    /// DrawingVisual is protected.  This method works around that limitation.
    ///
    /// <para>
    /// For more information, see the article "How to apply ShaderEffect to
    /// Visual?" at http://www.netframeworkdev.com/windows-presentation-
    /// foundation-wpf/how-to-apply-shadereffect-to-visual-78632.shtml.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public void
    SetEffect
    (
        Effect effect
    )
    {
        AssertValid();

        this.VisualEffect = effect;
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
