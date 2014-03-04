
using System;
using System.Configuration;
using System.Diagnostics;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: TopNByMetricUserSettings
//
/// <summary>
/// Stores the user settings for calculating one set of top-N-by metrics.
/// </summary>
///
/// <remarks>
/// Sample user settings stored in one instance of this class, in English: "Get
/// the top 10 vertex names on the vertex worksheet, ranked by closeness
/// centrality, along with the corresponding closeness centralities."
/// </remarks>
//*****************************************************************************

public class TopNByMetricUserSettings : Object
{
    //*************************************************************************
    //  Constructor: TopNByMetricUserSettings()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TopNByMetricUserSettings" /> class.
    /// </summary>
    //*************************************************************************

    public TopNByMetricUserSettings()
    {
        m_iN = 10;
        m_sWorksheetName = WorksheetNames.Vertices;
        m_sTableName = TableNames.Vertices;
        m_sRankedColumnName = String.Empty;
        m_sItemNameColumnName = VertexTableColumnNames.VertexName;

        AssertValid();
    }

    //*************************************************************************
    //  Property: N
    //
    /// <summary>
    /// Gets or sets the number of top items to get.
    /// </summary>
    ///
    /// <value>
    /// The number of top items to get.  Must be greater than 0.  The default
    /// value is 10.
    /// </value>
    //*************************************************************************

    public Int32
    N
    {
        get
        {
            AssertValid();

            return (m_iN);
        }

        set
        {
            m_iN = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: WorksheetName
    //
    /// <summary>
    /// Gets or sets the name of the worksheet to get top items from.
    /// </summary>
    ///
    /// <value>
    /// The name of the worksheet to get top items from.  Can't be null or
    /// empty.  The default value is <see cref="WorksheetNames.Vertices" />.
    /// </value>
    //*************************************************************************

    public String
    WorksheetName
    {
        get
        {
            AssertValid();

            return (m_sWorksheetName);
        }

        set
        {
            m_sWorksheetName = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: TableName
    //
    /// <summary>
    /// Gets or sets the name of the table to get top items from.
    /// </summary>
    ///
    /// <value>
    /// The name of the table to get top items from.  Can't be null or empty.
    /// The default value is <see cref="TableNames.Vertices" />.
    /// </value>
    //*************************************************************************

    public String
    TableName
    {
        get
        {
            AssertValid();

            return (m_sTableName);
        }

        set
        {
            m_sTableName = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: RankedColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column to rank items by.
    /// </summary>
    ///
    /// <value>
    /// The name of the column to rank items by.  Can't be null.  The default
    /// value is an empty string.
    /// </value>
    ///
    /// <remarks>
    /// Sample value when ranking vertices by closeness centrality: "Closeness
    /// Centrality".
    /// </remarks>
    //*************************************************************************

    public String
    RankedColumnName
    {
        get
        {
            AssertValid();

            return (m_sRankedColumnName);
        }

        set
        {
            m_sRankedColumnName = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ItemNameColumnName
    //
    /// <summary>
    /// Gets or sets the name of the column that names the ranked items.
    /// </summary>
    ///
    /// <value>
    /// The name of the column that names the ranked items.  Can't be null or
    /// empty.  The default value is <see
    /// cref="VertexTableColumnNames.VertexName" />.
    /// </value>
    ///
    /// <remarks>
    /// Sample value when ranking vertices by closeness centrality: "Vertex".
    /// </remarks>
    //*************************************************************************

    public String
    ItemNameColumnName
    {
        get
        {
            AssertValid();

            return (m_sItemNameColumnName);
        }

        set
        {
            m_sItemNameColumnName = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Method: Clone()
    //
    /// <summary>
    /// Creates a deep copy of the object.
    /// </summary>
    ///
    /// <returns>
    /// A deep copy of the object.
    /// </returns>
    //*************************************************************************

    public TopNByMetricUserSettings
    Clone()
    {
        AssertValid();

        TopNByMetricUserSettings oTopNByMetricUserSettings =
            new TopNByMetricUserSettings();

        oTopNByMetricUserSettings.N = m_iN;

        oTopNByMetricUserSettings.WorksheetName =
            String.Copy(m_sWorksheetName);

        oTopNByMetricUserSettings.TableName = String.Copy(m_sTableName);

        oTopNByMetricUserSettings.RankedColumnName =
            String.Copy(m_sRankedColumnName);

        oTopNByMetricUserSettings.ItemNameColumnName =
            String.Copy(m_sItemNameColumnName);

        return (oTopNByMetricUserSettings);
    }

    //*************************************************************************
    //  Method: ToString()
    //
    /// <summary>
    /// Formats the value of the current instance using the default format. 
    /// </summary>
    ///
    /// <returns>
    /// The formatted string.
    /// </returns>
    //*************************************************************************

    public override String
    ToString()
    {
        AssertValid();

        if (
            m_sWorksheetName != WorksheetNames.Vertices ||
            m_sTableName != TableNames.Vertices ||
            m_sItemNameColumnName != VertexTableColumnNames.VertexName
            )
        {
            // TopNByMetricsDialog is hard-coded for now to get the top
            // vertices from the vertex worksheet.  The dialog and this method
            // can be updated later to get the top items from any worksheet, if
            // necessary.

            throw new NotSupportedException();
        }

        Boolean bOneVertex = (m_iN == 1);

        return ( String.Format(
            "Top {0}{1}, Ranked by {2}"
            ,
            bOneVertex ? String.Empty : m_iN.ToString(),
            bOneVertex ? "Vertex" : " Vertices",
            m_sRankedColumnName
            ) );
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
        Debug.Assert(m_iN >= MinimumN);
        Debug.Assert(m_iN <= MaximumN);
        Debug.Assert( !String.IsNullOrEmpty(m_sWorksheetName) );
        Debug.Assert( !String.IsNullOrEmpty(m_sTableName) );
        Debug.Assert(m_sRankedColumnName != null);
        Debug.Assert( !String.IsNullOrEmpty(m_sItemNameColumnName) );
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Minimum value of the <see cref="N" /> property.

    public const Int32 MinimumN = 1;

    /// Maximum value of the <see cref="N" /> property.

    public const Int32 MaximumN = 100;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Number of top items to get.

    protected Int32 m_iN;

    /// Name of the worksheet to get top items from.

    protected String m_sWorksheetName;

    /// Name of the table to get top items from.

    protected String m_sTableName;

    /// Name of the column to rank items by.

    protected String m_sRankedColumnName;

    /// Name of the column that names the ranked items.

    protected String m_sItemNameColumnName;
}

}
