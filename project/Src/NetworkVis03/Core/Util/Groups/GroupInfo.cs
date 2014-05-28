
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace Smrf.NodeXL.Core
{
//*****************************************************************************
//  Class: GroupInfo
//
/// <summary>
/// Contains information about one vertex group.
/// </summary>
//*****************************************************************************

public class GroupInfo : Object
{
    //*************************************************************************
    //  Constructor: GroupInfo()
    //
    /// <overloads>
    /// Initializes a new instance of the <see cref="GroupInfo" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupInfo" /> class with
    /// a unique name and "is collapsed" flag.
    /// </summary>
    ///
    /// <param name="name">
    /// The unique name of the group.  Can be null or empty.
    /// </param>
    ///
    /// <param name="isCollapsed">
    /// true if the group should be collapsed.
    /// </param>
    ///
    /// <param name="collapsedAttributes">
    /// String containing the attributes describing what the group should look
    /// like when it is collapsed, or null to give the collapsed group a
    /// default appearance.  If not null, this should be a string that was
    /// returned by <see
    /// cref="Smrf.AppLib.PersistableStringDictionary.ToString" />.
    /// </param>
    //*************************************************************************

    public GroupInfo
    (
        String name,
        Boolean isCollapsed,
        String collapsedAttributes
    )
    {
        m_sName = name;
        m_oVertices = new LinkedList<IVertex>();
        m_oRectangle = Rectangle.Empty;
        m_sLabel = null;
        m_bIsCollapsed = isCollapsed;
        m_sCollapsedAttributes = collapsedAttributes;
        m_oCollapsedLocation = null;
        m_Connectivity = 0;

        // AssertValid();
    }
    
    //*************************************************************************
    //  Constructor: GroupInfo()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="GroupInfo" /> class
    /// without a name and without being collapsed.
    /// </summary>
    //*************************************************************************

    public GroupInfo()
    :
    this(null, false, null)
    {
        // AssertValid();
    }

    //*************************************************************************
    //  Property: Name
    //
    /// <summary>
    /// Gets or sets the group's unique name.
    /// </summary>
    ///
    /// <value>
    /// An unique name.  Can be null or empty.  The default value is null.
    /// </value>
    ///
    /// <remarks>
    /// Groups can be collapsed and expanded when using the NodeXLControl.  The
    /// group to collapse or expand is specified by name.
    /// </remarks>
    //*************************************************************************

    public String
    Name
    {
        get
        {
            AssertValid();

            return (m_sName);
        }

        set
        {
            m_sName = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Vertices
    //
    /// <summary>
    /// Gets a collection of the vertices in the group.
    /// </summary>
    ///
    /// <value>
    /// A collection of the vertices in the group.  Can be empty but not null.
    /// The default value is an empty collection.
    /// </value>
    //*************************************************************************

    public LinkedList<IVertex>
    Vertices
    {
        get
        {
            AssertValid();

            return (m_oVertices);
        }
    }

    //*************************************************************************
    //  Property: Rectangle
    //
    /// <summary>
    /// Gets or sets the rectangle the vertices were laid out within.
    /// </summary>
    ///
    /// <value>
    /// A System.Drawing.Rectangle.  The default value is Rectangle.Empty.
    /// </value>
    ///
    /// <remarks>
    /// If the graph is laid out using groups, this gets set to the rectangle
    /// the vertices were laid out within.
    /// </remarks>
    //*************************************************************************

    public Rectangle
    Rectangle
    {
        get
        {
            AssertValid();

            return (m_oRectangle);
        }

        set
        {
            m_oRectangle = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Label
    //
    /// <summary>
    /// Gets or sets the group's label.
    /// </summary>
    ///
    /// <value>
    /// An optional label.  Can be null or empty.  The default value is null.
    /// </value>
    ///
    /// <remarks>
    /// If the graph is laid out using groups, the label gets shown in the
    /// group's rectangle.
    /// </remarks>
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
    //  Property: IsCollapsed
    //
    /// <summary>
    /// Gets a flag indicating whether the group should be collapsed.
    /// </summary>
    ///
    /// <value>
    /// true if the group should be collapsed.
    /// </value>
    ///
    /// <remarks>
    /// Groups can be collapsed and expanded when using the NodeXLControl.
    /// </remarks>
    //*************************************************************************

    public Boolean
    IsCollapsed
    {
        get
        {
            AssertValid();

            return (m_bIsCollapsed);
        }
    }

    //*************************************************************************
    //  Property: CollapsedLocation
    //
    /// <summary>
    /// Gets or sets the location of the vertex that represents the collapsed
    /// group.
    /// </summary>
    ///
    /// <value>
    /// The vertex's location as a <see cref="PointF" />, or null if a
    /// collapsed location is not available.  The default value is null.
    /// </value>
    //*************************************************************************

    public Nullable<PointF>
    CollapsedLocation
    {
        get
        {
            AssertValid();

            return (m_oCollapsedLocation);
        }

        set
        {
            m_oCollapsedLocation = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: CollapsedAttributes
    //
    /// <summary>
    /// Gets or sets a string containing the attributes describing what the
    /// group should look like when it is collapsed.
    /// </summary>
    ///
    /// <value>
    /// A string that was returned by <see
    /// cref="Smrf.AppLib.PersistableStringDictionary.ToString" />, or null if
    /// the collapsed group should have a default appearance.
    /// </value>
    //*************************************************************************

    public String
    CollapsedAttributes
    {
        get
        {
            AssertValid();

            return (m_sCollapsedAttributes);
        }

        set
        {
            m_sCollapsedAttributes = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Connectivity
    //
    /// <summary>
    /// Gets or sets the group's connectivity metric for the Packed Rectangles 
    /// Group-in-a-Box layout.
    /// </summary>
    //*************************************************************************

    public int
    Connectivity
    {
        get
        {
            AssertValid();

            return m_Connectivity;
        }

        set
        {
            m_Connectivity = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: CompareConnectivity()
    //
    /// <summary>
    /// Compares the group's connectivity metric for the Packed Rectangles 
    /// Group-in-a-Box layout.
    /// </summary>
    /// 
    /// <param name="other">
    /// A group to compare this against.
    /// </param>
    /// 
    /// <returns>
    /// A signed number indicating the relative values of connectivity metric.
    /// </returns>
    //*************************************************************************

    public int
    CompareConnectivity(GroupInfo other)
    {

        if (other.Connectivity != this.Connectivity)
        {
            return this.Connectivity.CompareTo(other.Connectivity);
        }
        else
        {
            return this.Vertices.Count.CompareTo(other.Vertices.Count);
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

    public virtual void
    AssertValid()
    {
        // m_sName
        Debug.Assert(m_oVertices != null);
        // m_oRectangle
        // m_sLabel
        // m_bIsCollapsed
        // m_sCollapsedAttributes
        // m_oCollapsedLocation
        // m_Connectivity
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The group's unique name.  Can be null or empty.

    protected String m_sName;

    /// Group's connectivity metric for GIB layout algorithm

    protected int m_Connectivity;

    /// Collection of the group's vertices.

    protected LinkedList<IVertex> m_oVertices;

    /// The rectangle the group's vertices were laid out within.

    protected Rectangle m_oRectangle;

    /// Optional label to show in the group's rectangle.  Can be null or empty.

    protected String m_sLabel;

    /// true if the group should be collapsed.

    protected Boolean m_bIsCollapsed;

    /// String that contains the attributes describing what the group should
    /// look like when it is collapsed, or null.

    protected String m_sCollapsedAttributes;

    /// The location of the vertex that represents the collapsed group, or
    /// null.

    protected Nullable<PointF> m_oCollapsedLocation;
}

}
