
using System;
using System.Configuration;
using System.Diagnostics;
using Smrf.NodeXL.Layouts;
using Smrf.NodeXL.ApplicationUtil;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: LayoutUserSettings
//
/// <summary>
/// Stores the user's settings for all the graph layouts used by the
/// application.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("LayoutUserSettings") ]

public class LayoutUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: LayoutUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the LayoutUserSettings class.
    /// </summary>
    //*************************************************************************

    public LayoutUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: Layout
    //
    /// <summary>
    /// Gets or sets the layout type to use.
    /// </summary>
    ///
    /// <value>
    /// The layout type to use, as a <see cref="LayoutType" />.  The default is
    /// <see cref="LayoutType.FruchtermanReingold" />.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("FruchtermanReingold") ]

    public LayoutType
    Layout
    {
        get
        {
            AssertValid();

            return ( (LayoutType)this[LayoutKey] );
        }

        set
        {
            this[LayoutKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: Margin
    //
    /// <summary>
    /// Gets or sets the margin to subtract from each edge of the graph
    /// rectangle before laying out the graph.
    /// </summary>
    ///
    /// <value>
    /// The margin to subtract from each edge.  Must be greater than or equal
    /// to zero.  The default value is 6.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("6") ]

    public Int32
    Margin
    {
        get
        {
            AssertValid();

            return ( (Int32)this[MarginKey] );
        }

        set
        {
            this[MarginKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: LayoutStyle
    //
    /// <summary>
    /// Gets or sets the style to use when laying out the graph.
    /// </summary>
    ///
    /// <value>
    /// The style to use when laying out the graph.  The default value is
    /// <see cref="Smrf.NodeXL.Layouts.LayoutStyle.Normal" />.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Normal") ]

    public LayoutStyle
    LayoutStyle
    {
        get
        {
            AssertValid();

            return ( (LayoutStyle)this[LayoutStyleKey] );
        }

        set
        {
            this[LayoutStyleKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BoxLayoutAlgorithm
    //
    /// <summary>
    /// Gets or sets the box layout algorithm to use when laying out groups in the graph.
    /// </summary>
    ///
    /// <value>
    /// The box layout algorithm to use when laying out the graph.  The default value is
    /// <see cref="Smrf.NodeXL.Layouts.BoxLayoutAlgorithm.Treemap" />.
    /// </value>
    //*************************************************************************

    [UserScopedSettingAttribute()]
    [DefaultSettingValueAttribute("Treemap")]

    public BoxLayoutAlgorithm
    BoxLayoutAlgorithm
    {
        get
        {
            AssertValid();

            return ((BoxLayoutAlgorithm)this[BoxLayoutAlgorithmKey]);
        }

        set
        {
            this[BoxLayoutAlgorithmKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: GroupRectanglePenWidth
    //
    /// <summary>
    /// Gets or sets the width of the pen used to draw group rectangles.
    /// </summary>
    ///
    /// <value>
    /// The width of the pen used to draw group rectangles.  The default value
    /// is 1.0.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("1.0") ]

    public Double
    GroupRectanglePenWidth
    {
        get
        {
            AssertValid();

            return ( (Double)this[GroupRectanglePenWidthKey] );
        }

        set
        {
            this[GroupRectanglePenWidthKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: IntergroupEdgeStyle
    //
    /// <summary>
    /// Gets or sets a value that specifies how the edges that connect vertices
    /// in different groups should be shown.
    /// </summary>
    ///
    /// <value>
    /// A value that specifies how the intergroup edges should be shown.  The
    /// default value is <see
    /// cref="Smrf.NodeXL.Layouts.IntergroupEdgeStyle.Show" />.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Show") ]

    public IntergroupEdgeStyle
    IntergroupEdgeStyle
    {
        get
        {
            AssertValid();

            return ( (IntergroupEdgeStyle)this[IntergroupEdgeStyleKey] );
        }

        set
        {
            this[IntergroupEdgeStyleKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ImproveLayoutOfGroups
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the layout should attempt to
    /// improve the appearance of groups.
    /// </summary>
    ///
    /// <value>
    /// true to attempt to improve the appearance of groups.  The default value
    /// is false.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("false") ]

    public Boolean
    ImproveLayoutOfGroups
    {
        get
        {
            AssertValid();

            return ( (Boolean)this[ImproveLayoutOfGroupsKey] );
        }

        set
        {
            this[ImproveLayoutOfGroupsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: MaximumVerticesPerBin
    //
    /// <summary>
    /// Gets or sets the maximum number of vertices a binned component can
    /// have.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of vertices a binned component can have.  The
    /// default value is 3.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("3") ]

    public Int32
    MaximumVerticesPerBin
    {
        get
        {
            AssertValid();

            return ( (Int32)this[MaximumVerticesPerBinKey] );
        }

        set
        {
            this[MaximumVerticesPerBinKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: BinLength
    //
    /// <summary>
    /// Gets or sets the height and width of each bin, in graph rectangle
    /// units.
    /// </summary>
    ///
    /// <value>
    /// The height and width of each bin, in graph rectangle units.  The
    /// default value is 16.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("16") ]

    public Int32
    BinLength
    {
        get
        {
            AssertValid();

            return ( (Int32)this[BinLengthKey] );
        }

        set
        {
            this[BinLengthKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FruchtermanReingoldC
    //
    /// <summary>
    /// Gets or sets the constant that determines the strength of the
    /// attractive and repulsive forces between the vertices when using the
    /// FruchtermanReingoldLayout.
    /// </summary>
    ///
    /// <value>
    /// The "C" constant in the "Modelling the forces" section of the
    /// Fruchterman-Reingold paper.  Must be greater than 0.  The default value
    /// is 3.0.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("3.0") ]

    public Single
    FruchtermanReingoldC
    {
        get
        {
            AssertValid();

            return ( (Single)this[FruchtermanReingoldCKey] );
        }

        set
        {
            this[FruchtermanReingoldCKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FruchtermanReingoldIterations
    //
    /// <summary>
    /// Gets or sets the number of times to run the Fruchterman-Reingold
    /// algorithm when using the FruchtermanReingoldLayout.
    /// </summary>
    ///
    /// <value>
    /// The number of times to run the Fruchterman-Reingold algorithm when the
    /// graph is laid out, as an Int32.  Must be greater than zero.  The
    /// default value is 10.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("10") ]

    public Int32
    FruchtermanReingoldIterations
    {
        get
        {
            AssertValid();

            return ( (Int32)this[FruchtermanReingoldIterationsKey] );
        }

        set
        {
            this[FruchtermanReingoldIterationsKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: TransferToLayout()
    //
    /// <summary>
    /// Transfers the settings to an <see cref="ILayout" /> object.
    /// </summary>
    ///
    /// <param name="layout">
    /// Layout to transfer the settings to.
    /// </param>
    //*************************************************************************

    public void
    TransferToLayout
    (
        ILayout layout
    )
    {
        Debug.Assert(layout != null);
        AssertValid();

        layout.Margin = this.Margin;
        layout.LayoutStyle = this.LayoutStyle;
        layout.BoxLayoutAlgorithm = this.BoxLayoutAlgorithm;
        layout.GroupRectanglePenWidth = this.GroupRectanglePenWidth;
        layout.IntergroupEdgeStyle = this.IntergroupEdgeStyle;
        layout.ImproveLayoutOfGroups = this.ImproveLayoutOfGroups;
        layout.MaximumVerticesPerBin = this.MaximumVerticesPerBin;
        layout.BinLength = this.BinLength;

        if (layout is FruchtermanReingoldLayout)
        {
            FruchtermanReingoldLayout oFruchtermanReingoldLayout =
                (FruchtermanReingoldLayout)layout;

            oFruchtermanReingoldLayout.C = this.FruchtermanReingoldC;

            oFruchtermanReingoldLayout.Iterations =
                this.FruchtermanReingoldIterations;
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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the Layout property.

    protected const String LayoutKey = "Layout";

    /// Name of the settings key for the Margin property.

    protected const String MarginKey = "Margin";

    /// Name of the settings key for the LayoutStyle property.

    protected const String LayoutStyleKey = "LayoutStyle";

    /// Name of the settings key for the BoxLayoutAlgorithm property.

    protected const String BoxLayoutAlgorithmKey = "BoxLayoutAlgorithm";

    /// Name of the settings key for the GroupRectanglePenWidth property.

    protected const String GroupRectanglePenWidthKey =
        "GroupRectanglePenWidth";

    /// Name of the settings key for the IntergroupEdgeStyle property.

    protected const String IntergroupEdgeStyleKey = "IntergroupEdgeStyle";

    /// Name of the settings key for the ImproveLayoutOfGroups property.

    protected const String ImproveLayoutOfGroupsKey = "ImproveLayoutOfGroups";

    /// Name of the settings key for the MaximumVerticesPerBin property.

    protected const String MaximumVerticesPerBinKey = "MaximumVerticesPerBin";

    /// Name of the settings key for the BinLength property.

    protected const String BinLengthKey = "BinLength";

    /// Name of the settings key for the FruchtermanReingoldC property.

    protected const String FruchtermanReingoldCKey = "FruchtermanReingoldC";

    /// Name of the settings key for the FruchtermanReingoldIterations property.

    protected const String FruchtermanReingoldIterationsKey =
        "FruchtermanReingoldIterations";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
