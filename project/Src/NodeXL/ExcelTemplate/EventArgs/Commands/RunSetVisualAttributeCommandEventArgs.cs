
using System;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: RunSetVisualAttributeCommandEventArgs
//
/// <summary>
/// Provides information for the command that needs to be run when the user
/// sets a visual attribute in the Ribbon.
/// </summary>
///
/// <remarks>
/// See <see cref="RunCommandEventArgs" /> for information about how NodeXL
/// sends commands from one UI object to another.
///
/// <para>
/// The first event handler that sets the specified visual attribute should set
/// the <see cref="VisualAttributeSet" /> property to true, and other event
/// handlers should then ignore the event.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class RunSetVisualAttributeCommandEventArgs : RunCommandEventArgs
{
    //*************************************************************************
    //  Constructor: RunSetVisualAttributeCommandEventArgs()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="RunSetVisualAttributeCommandEventArgs" /> class.
    /// </summary>
    ///
    /// <param name="visualAttribute">
    /// Specifies the visual attribute to set.  Must be only one of the flags
    /// in the <see cref="VisualAttributes" /> enumeration; it cannot be an
    /// ORed combination.
    /// </param>
    ///
    /// <param name="attributeValue">
    /// The visual attribute value, or null if the value isn't known yet and
    /// must be obtained from the user.
    /// </param>
    //*************************************************************************

    public RunSetVisualAttributeCommandEventArgs
    (
        VisualAttributes visualAttribute,
        Object attributeValue
    )
    {
        m_eVisualAttribute = visualAttribute;
        m_oAttributeValue = attributeValue;
        m_bVisualAttributeSet = false;

        AssertValid();
    }

    //*************************************************************************
    //  Property: VisualAttribute
    //
    /// <summary>
    /// Gets a flag indicating which columns changed.
    /// </summary>
    ///
    /// <value>
    /// Specifies the visual attribute to set.  This is only one of the flags
    /// in the <see cref="VisualAttributes" /> enumeration; it cannot be an
    /// ORed combination.
    /// </value>
    //*************************************************************************

    public VisualAttributes
    VisualAttribute
    {
        get
        {
            AssertValid();

            return (m_eVisualAttribute);
        }
    }

    //*************************************************************************
    //  Property: AttributeValue
    //
    /// <summary>
    /// Gets the visual attribute value.
    /// </summary>
    ///
    /// <value>
    /// The visual attribute value, or null if the value isn't known yet and
    /// must be obtained from the user.
    /// </value>
    //*************************************************************************

    public Object
    AttributeValue
    {
        get
        {
            AssertValid();

            return (m_oAttributeValue);
        }
    }

    //*************************************************************************
    //  Property: VisualAttributeSet
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the specified visual attribute
    /// has been set.
    /// </summary>
    ///
    /// <value>
    /// true if the specified visual attribute has been set.
    /// </value>
    ///
    /// <remarks>
    /// The first event handler that sets the specified visual attribute should
    /// set this property to true, and other event handlers should then ignore
    /// the event.
    /// </remarks>
    //*************************************************************************

    public Boolean
    VisualAttributeSet
    {
        get
        {
            AssertValid();

            return (m_bVisualAttributeSet);
        }

        set
        {
            m_bVisualAttributeSet = value;

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

    // [Conditional("DEBUG")] 

    public override void
    AssertValid()
    {
        base.AssertValid();

        // m_eVisualAttribute
        // m_oAttributeValue;
        // m_bVisualAttributeSet
    }


    //*************************************************************************
    //  Protected member data
    //*************************************************************************

    /// One of the flags in the VisualAttributes enumeration.

    protected VisualAttributes m_eVisualAttribute;

    /// The visual attribute value, or null if the value isn't known yet and
    /// must be obtained from the user.

    protected Object m_oAttributeValue;

    /// true if the specified visual attribute has been set.

    protected Boolean m_bVisualAttributeSet;
}

}
