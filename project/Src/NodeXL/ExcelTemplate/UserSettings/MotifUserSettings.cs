
using System;
using System.Configuration;
using System.Diagnostics;
using Smrf.NodeXL.Algorithms;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: MotifUserSettings
//
/// <summary>
/// Stores the user's settings that specify how the graph's vertices should be
/// grouped by motif.
/// </summary>
//*****************************************************************************

[ SettingsGroupNameAttribute("MotifUserSettings") ]

public class MotifUserSettings : NodeXLApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: MotifUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the MotifUserSettings class.
    /// </summary>
    //*************************************************************************

    public MotifUserSettings()
    {
        // (Do nothing.)

        AssertValid();
    }

    //*************************************************************************
    //  Property: MotifsToCalculate
    //
    /// <summary>
    /// Gets or sets the motifs to calculate when grouping the graph's vertices
    /// by motif.
    /// </summary>
    ///
    /// <value>
    /// The motifs to calculate.  The default value is Fan|DConnector|Clique.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("Fan, DConnector, Clique") ]

    public Motifs
    MotifsToCalculate
    {
        get
        {
            AssertValid();

            return ( (Motifs)this[MotifsToCalculateKey] );
        }

        set
        {
            this[MotifsToCalculateKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DConnectorMinimumAnchorVertices
    //
    /// <summary>
    /// Gets or sets the minimum number of anchor vertices when grouping the
    /// graph's vertices by D-connector motifs.
    /// </summary>
    ///
    /// <value>
    /// The minimum number of anchor vertices.  The default value is 2.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("2") ]

    public Int32
    DConnectorMinimumAnchorVertices
    {
        get
        {
            AssertValid();

            return ( (Int32)this[DConnectorMinimumAnchorVerticesKey] );
        }

        set
        {
            this[DConnectorMinimumAnchorVerticesKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DConnectorMaximumAnchorVertices
    //
    /// <summary>
    /// Gets or sets the maximum number of anchor vertices when grouping the
    /// graph's vertices by D-connector motifs.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of anchor vertices.  The default value is 9999.
    /// </value>
    //*************************************************************************

    [ UserScopedSettingAttribute() ]
    [ DefaultSettingValueAttribute("9999") ]

    public Int32
    DConnectorMaximumAnchorVertices
    {
        get
        {
            AssertValid();

            return ( (Int32)this[DConnectorMaximumAnchorVerticesKey] );
        }

        set
        {
            this[DConnectorMaximumAnchorVerticesKey] = value;

            AssertValid();
        }
    }


    //*************************************************************************
    //  Property: CliqueMinimumMemberVertices
    //
    /// <summary>
    /// Gets or sets the minimum number of member vertices when grouping the
    /// graph's vertices by clique motifs.
    /// </summary>
    ///
    /// <value>
    /// The minimum number of member vertices.  The default value is 4.
    /// </value>
    //*************************************************************************

    [UserScopedSettingAttribute()]
    [DefaultSettingValueAttribute("4")]

    public Int32
    CliqueMinimumMemberVertices
    {
        get
        {
            AssertValid();

            return ((Int32)this[CliqueMinimumMemberVerticesKey]);
        }

        set
        {
            this[CliqueMinimumMemberVerticesKey] = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: CliqueMaximumMemberVertices
    //
    /// <summary>
    /// Gets or sets the maximum number of member vertices when grouping the
    /// graph's vertices by clique motifs.
    /// </summary>
    ///
    /// <value>
    /// The maximum number of member vertices.  The default value is 9999.
    /// </value>
    //*************************************************************************

    [UserScopedSettingAttribute()]
    [DefaultSettingValueAttribute("9999")]

    public Int32
    CliqueMaximumMemberVertices
    {
        get
        {
            AssertValid();

            return ((Int32)this[CliqueMaximumMemberVerticesKey]);
        }

        set
        {
            this[CliqueMaximumMemberVerticesKey] = value;

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

        // (Do nothing else.)
    }


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Name of the settings key for the MotifsToCalculate property.

    protected const String MotifsToCalculateKey =
        "MotifsToCalculate";

    /// Name of the settings key for the DConnectorMinimumAnchorVertices
    /// property.

    protected const String DConnectorMinimumAnchorVerticesKey =
        "DConnectorMinimumAnchorVertices";

    /// Name of the settings key for the DConnectorMaximumAnchorVertices
    /// property.

    protected const String DConnectorMaximumAnchorVerticesKey =
        "DConnectorMaximumAnchorVertices";

    /// Name of the settings key for the CliqueMinimumMemberVertices
    /// property.

    protected const String CliqueMinimumMemberVerticesKey =
        "CliqueMinimumMemberVertices";

    /// Name of the settings key for the CliqueMaximumMemberVertices
    /// property.

    protected const String CliqueMaximumMemberVerticesKey =
        "CliqueMaximumMemberVertices";

    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)
}

}
