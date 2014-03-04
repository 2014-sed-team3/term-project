
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Layouts;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ExcelTemplateGroupInfo
//
/// <summary>
/// Contains information about one group of vertices.
/// </summary>
///
/// <remarks>
/// This class is derived from <see cref="GroupInfo" />.  It adds group
/// information that is specific to the ExcelTemplate project.
/// </remarks>
//*****************************************************************************

public class ExcelTemplateGroupInfo : GroupInfo
{
    //*************************************************************************
    //  Constructor: ExcelTemplateGroupInfo()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ExcelTemplateGroupInfo" />
    /// class.
    /// </summary>
    ///
    /// <param name="name">
    /// The unique name of the group.
    /// </param>
    ///
    /// <param name="rowID">
    /// The group's row ID in the group worksheet, or null if an ID isn't
    /// available.
    /// </param>
    ///
    /// <param name="vertexColor">
    /// The color to use for each of the group's vertices.
    /// </param>
    ///
    /// <param name="vertexShape">
    /// The shape to use for each of the group's vertices.
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

    public ExcelTemplateGroupInfo
    (
        String name,
        Nullable<Int32> rowID,
        Color vertexColor,
        VertexShape vertexShape,
        Boolean isCollapsed,
        String collapsedAttributes
    )
    : base(name, isCollapsed, collapsedAttributes)
    {
        Debug.Assert( !String.IsNullOrEmpty(name) );

        m_iRowID = rowID;
        m_oVertexColor = vertexColor;
        m_eVertexShape = vertexShape;

        AssertValid();
    }

    //*************************************************************************
    //  Property: RowID
    //
    /// <summary>
    /// Gets the group's row ID in the group worksheet.
    /// </summary>
    ///
    /// <value>
    /// The group's row ID in the group worksheet, or null if an ID isn't
    /// available.
    /// </value>
    //*************************************************************************

    public Nullable<Int32>
    RowID
    {
        get
        {
            AssertValid();

            return (m_iRowID);
        }
    }

    //*************************************************************************
    //  Property: VertexColor
    //
    /// <summary>
    /// Gets the color to use for each of the group's vertices.
    /// </summary>
    ///
    /// <value>
    /// The color to use for each of the group's vertices.
    /// </value>
    //*************************************************************************

    public Color
    VertexColor
    {
        get
        {
            AssertValid();

            return (m_oVertexColor);
        }
    }

    //*************************************************************************
    //  Property: VertexShape
    //
    /// <summary>
    /// Gets the shape to use for each of the group's vertices.
    /// </summary>
    ///
    /// <value>
    /// The shape to use for each of the group's vertices.
    /// </value>
    //*************************************************************************

    public VertexShape
    VertexShape
    {
        get
        {
            AssertValid();

            return (m_eVertexShape);
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

        Debug.Assert( !String.IsNullOrEmpty(m_sName) );
        // m_iRowID
        // m_oVertexColor
        // m_eVertexShape
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// The group's row ID in the group worksheet, or null.

    protected Nullable<Int32> m_iRowID;

    /// The color to use for each of the group's vertices.

    protected Color m_oVertexColor;

    /// The shape to use for each of the group's vertices.

    protected VertexShape m_eVertexShape;
}

}
