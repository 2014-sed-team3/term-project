
using System;
using System.Drawing;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ReadWorkbookContext
//
/// <summary>
/// Provides access to objects needed for converting an Excel workbook to a
/// NodeXL graph.
/// </summary>
///
/// <remarks>
/// If the X, Y, and Locked columns of the vertex table should be read, set
/// <see cref="IgnoreVertexLocations" /> to false and set <see
/// cref="GraphRectangle" /> to the rectangle the graph is being drawn
/// within.  Otherwise, leave <see cref="IgnoreVertexLocations" /> at its
/// default value of true.
///
/// <para>
/// Set <see cref="FillIDColumns" /> to true to store an ID in each row of the
/// edge and vertex tables.
/// </para>
///
/// <para>
/// Set <see cref="PopulateVertexWorksheet" /> to true to populate the vertex
/// table with unique vertex names from the edge worksheet before the workbook
/// is read.
/// </para>
///
/// <para>
/// Set <see cref="ReadEdgeWeights" /> to true to read the edge weight column
/// in the edge table.
/// </para>
///
/// <para>
/// Set <see cref="ReadGroups" /> to true to read the group worksheets.
/// </para>
///
/// <para>
/// To read labels specified on the vertex, edge or group worksheets, set <see
/// cref="ReadVertexLabels" />, <see cref="ReadEdgeLabels" /> or <see
/// cref="ReadGroupLabels" /> to true.
/// </para>
///
/// <para>
/// To read images specified on the vertex worksheet, set <see
/// cref="ReadVertexImages" /> to true, then set <see
/// cref="DefaultVertexImageSize" /> and <see cref="DefaultVertexShape" />.
/// </para>
///
/// <para>
/// To read all columns in the edge and vertex worksheets and store the cell
/// values as metadata on the graph's edge and vertex objects, set <see
/// cref="ReadAllEdgeAndVertexColumns" /> to true.
/// </para>
///
/// </remarks>
//*****************************************************************************

public class ReadWorkbookContext : Object
{
    //*************************************************************************
    //  Constructor: ReadWorkbookContext()
    //
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadWorkbookContext" />
    /// class.
    /// </summary>
    //*************************************************************************

    public ReadWorkbookContext()
    {
        m_bIgnoreVertexLocations = true;
        m_bFillIDColumns = false;
        m_bPopulateVertexWorksheet = false;
        m_bReadEdgeWeights = false;
        m_bReadGroups = false;
        m_bReadVertexColorFromGroups = true;
        m_bReadVertexShapeFromGroups = true;
        m_bReadVertexLabels = false;
        m_bReadEdgeLabels = false;
        m_bReadGroupLabels = false;
        m_bReadVertexImages = false;
        m_oDefaultVertexImageSize = new Nullable<Single>();
        m_eDefaultVertexShape = VertexShape.Disk;
        m_bReadAllEdgeAndVertexColumns = false;
        m_oGraphRectangle = Rectangle.FromLTRB(0, 0, 100, 100);
        m_oColorConverter2 = new ColorConverter2();
        m_oBooleanConverter = new BooleanConverter();
        m_oFontSizeConverter = new FontSizeConverter();
        m_oEdgeWidthConverter = new EdgeWidthConverter();
        m_oEdgeStyleConverter = new EdgeStyleConverter();
        m_oVertexRadiusConverter = new VertexRadiusConverter();

        m_oVertexLocationConverter =
            new VertexLocationConverter(m_oGraphRectangle);

        m_oVertexNameDictionary = new Dictionary<String, IVertex>();
        m_oEdgeRowIDDictionary = new Dictionary<Int32, IIdentityProvider>();
        m_oVertexRowIDDictionary = new Dictionary<Int32, IIdentityProvider>();

        AssertValid();
    }

    //*************************************************************************
    //  Property: IgnoreVertexLocations
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the X, Y, and Locked columns on
    /// the vertex worksheet should be ignored.
    /// </summary>
    ///
    /// <value>
    /// true to ignore the X, Y, and Locked columns on the vertex worksheet.
    /// The default is true.
    /// </value>
    ///
    /// <remarks>
    /// If you set this to false, you must also set <see
    /// cref="GraphRectangle" /> to the rectangle the graph is being drawn
    /// within.
    /// </remarks>
    //*************************************************************************

    public Boolean
    IgnoreVertexLocations
    {
        get
        {
            AssertValid();

            return (m_bIgnoreVertexLocations);
        }

        set
        {
            m_bIgnoreVertexLocations = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: FillIDColumns
    //
    /// <summary>
    /// Gets or sets a flag indicating whether an ID should be stored in each
    /// row of the edge and vertex tables.
    /// </summary>
    ///
    /// <value>
    /// true to store an ID in each row of the edge and vertex tables.  The
    /// default is false.
    /// </value>
    ///
    /// <remarks>
    /// The ID is used to link rows in the edge and vertex tables to edges and
    /// vertices in the NodeXL graph.
    /// </remarks>
    //*************************************************************************

    public Boolean
    FillIDColumns
    {
        get
        {
            AssertValid();

            return (m_bFillIDColumns);
        }

        set
        {
            m_bFillIDColumns = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: PopulateVertexWorksheet
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the vertex table should be
    /// populated with unique vertex names from the edge worksheet before the
    /// workbook is read.
    /// </summary>
    ///
    /// <value>
    /// true to populate the vertex table with unique vertex names.  The
    /// default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    PopulateVertexWorksheet
    {
        get
        {
            AssertValid();

            return (m_bPopulateVertexWorksheet);
        }

        set
        {
            m_bPopulateVertexWorksheet = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadEdgeWeights
    //
    /// <summary>
    /// Gets or sets a flag indicating whether to read the edge weight column
    /// in the edge table.
    /// </summary>
    ///
    /// <value>
    /// true to read the edge weight column.  The default is false.
    /// </value>
    ///
    /// <remarks>
    /// If true, the <see cref="ReservedMetadataKeys.EdgeWeight" /> value is
    /// set on each edge.
    /// </remarks>
    //*************************************************************************

    public Boolean
    ReadEdgeWeights
    {
        get
        {
            AssertValid();

            return (m_bReadEdgeWeights);
        }

        set
        {
            m_bReadEdgeWeights = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadGroups
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the group worksheets should be
    /// read.
    /// </summary>
    ///
    /// <value>
    /// true to read the group worksheets.  The default is false.
    /// </value>
    ///
    /// <remarks>
    /// If <see cref="ReadGroups" /> is true, the following metadata key gets
    /// added to the graph after the workbook is read:
    ///
    /// <list>
    ///
    /// <item><term>
    /// <see cref="ReservedMetadataKeys.GroupInfo" />
    /// </term></item>
    ///
    /// </list>
    ///
    /// <para>
    /// The key's value is an array of <see cref="ExcelTemplateGroupInfo" />
    /// objects, one for each of the graph's group.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    ReadGroups
    {
        get
        {
            AssertValid();

            return (m_bReadGroups);
        }

        set
        {
            m_bReadGroups = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadVertexColorFromGroups
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the color of a group's vertices
    /// should be read from the Groups worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to set the color of a group's vertices to the vertex color
    /// specified on the Groups worksheet, false to not modify the color of the
    /// vertices.  The default is true.
    /// </value>
    //*************************************************************************

    public Boolean
    ReadVertexColorFromGroups
    {
        get
        {
            AssertValid();

            return (m_bReadVertexColorFromGroups);
        }

        set
        {
            m_bReadVertexColorFromGroups = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadVertexShapeFromGroups
    //
    /// <summary>
    /// Gets or sets a flag indicating whether the shape of a group's vertices
    /// should be read from the Groups worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to set the shape of a group's vertices to the vertex shape
    /// specified on the Groups worksheet, false to not modify the shape of the
    /// vertices.  The default is true.
    /// </value>
    //*************************************************************************

    public Boolean
    ReadVertexShapeFromGroups
    {
        get
        {
            AssertValid();

            return (m_bReadVertexShapeFromGroups);
        }

        set
        {
            m_bReadVertexShapeFromGroups = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadVertexLabels
    //
    /// <summary>
    /// Gets or sets a flag indicating whether labels should be read from the
    /// vertex worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to read labels from the vertex worksheet.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    ReadVertexLabels
    {
        get
        {
            AssertValid();

            return (m_bReadVertexLabels);
        }

        set
        {
            m_bReadVertexLabels = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadEdgeLabels
    //
    /// <summary>
    /// Gets or sets a flag indicating whether labels should be read from the
    /// edge worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to read labels from the edge worksheet.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    ReadEdgeLabels
    {
        get
        {
            AssertValid();

            return (m_bReadEdgeLabels);
        }

        set
        {
            m_bReadEdgeLabels = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadGroupLabels
    //
    /// <summary>
    /// Gets or sets a flag indicating whether labels should be read from the
    /// group worksheet.
    /// </summary>
    ///
    /// <value>
    /// true to read labels from the group worksheet.  The default is false.
    /// </value>
    //*************************************************************************

    public Boolean
    ReadGroupLabels
    {
        get
        {
            AssertValid();

            return (m_bReadGroupLabels);
        }

        set
        {
            m_bReadGroupLabels = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadVertexImages
    //
    /// <summary>
    /// Gets or sets a flag indicating whether images on the vertex worksheet
    /// should be read.
    /// </summary>
    ///
    /// <value>
    /// true to read the images.  The default is false.
    /// </value>
    ///
    /// <remarks>
    /// If set to true, <see cref="DefaultVertexImageSize" /> and <see
    /// cref="DefaultVertexShape" /> should also be set.
    /// </remarks>
    //*************************************************************************

    public Boolean
    ReadVertexImages
    {
        get
        {
            AssertValid();

            return (m_bReadVertexImages);
        }

        set
        {
            m_bReadVertexImages = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DefaultVertexImageSize
    //
    /// <summary>
    /// Gets or sets the default size of vertices drawn as images.
    /// </summary>
    ///
    /// <value>
    /// The default size of vertices drawn as images, as a
    /// Nullable&lt;Single&gt;, or a Nullable with no value to use the actual
    /// image sizes.  The default is a Nullable with no value.
    /// </value>
    ///
    /// <remarks>
    /// This is used only if <see cref="ReadVertexImages" /> is true.
    /// </remarks>
    //*************************************************************************

    public Nullable<Single>
    DefaultVertexImageSize
    {
        get
        {
            AssertValid();

            return (m_oDefaultVertexImageSize);
        }

        set
        {
            m_oDefaultVertexImageSize = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: DefaultVertexShape
    //
    /// <summary>
    /// Gets or sets the default vertex shape.
    /// </summary>
    ///
    /// <value>
    /// The default vertex shape.  The default is a <see
    /// cref="VertexShape.Disk" />.
    /// </value>
    ///
    /// <remarks>
    /// This is used only if <see cref="ReadVertexImages" /> is true.
    /// </remarks>
    //*************************************************************************

    public VertexShape
    DefaultVertexShape
    {
        get
        {
            AssertValid();

            return (m_eDefaultVertexShape);
        }

        set
        {
            m_eDefaultVertexShape = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ReadAllEdgeAndVertexColumns
    //
    /// <summary>
    /// Gets or sets a flag indicating whether all columns on the edge and
    /// vertex worksheets should be read.
    /// </summary>
    ///
    /// <value>
    /// true to read all columns on the edge and vertex worksheets.  The
    /// default is false.
    /// </value>
    ///
    /// <remarks>
    /// If set to true, all columns in the edge and vertex worksheets are read
    /// and the cell values are stored as metadata on the graph's edge and
    /// vertex objects.  For example, if the vertex worksheet has a column
    /// named "My Column", then a key named My Column is added to every vertex
    /// whose cell has a non-empty value and the key's value is a String that
    /// contains that value, with leading and trailing spaces removed.  No
    /// other conversions are performed on the values.
    ///
    /// <para>
    /// Also, if set to true, the names of all columns in the edge and vertex
    /// worksheets are stored as metadata on the graph object using the <see
    /// cref="ReservedMetadataKeys.AllEdgeMetadataKeys" /> and
    /// <see cref="ReservedMetadataKeys.AllVertexMetadataKeys" /> keys.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    public Boolean
    ReadAllEdgeAndVertexColumns
    {
        get
        {
            AssertValid();

            return (m_bReadAllEdgeAndVertexColumns);
        }

        set
        {
            m_bReadAllEdgeAndVertexColumns = value;

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: GraphRectangle
    //
    /// <summary>
    /// Gets or sets the <see cref="System.Drawing.Rectangle" /> the graph is
    /// being drawn within.
    /// </summary>
    ///
    /// <value>
    /// The <see cref="System.Drawing.Rectangle" /> the graph is being drawn
    /// within.  The default is a 100x100 square.
    /// </value>
    //*************************************************************************

    public Rectangle
    GraphRectangle
    {
        get
        {
            AssertValid();

            return (m_oGraphRectangle);
        }

        set
        {
            m_oGraphRectangle = value;

            m_oVertexLocationConverter =
                new VertexLocationConverter(m_oGraphRectangle);

            AssertValid();
        }
    }

    //*************************************************************************
    //  Property: ColorConverter2
    //
    /// <summary>
    /// Gets an object for converting strings to colors.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="ColorConverter2" /> object.
    /// </value>
    //*************************************************************************

    public ColorConverter2
    ColorConverter2
    {
        get
        {
            AssertValid();

            return (m_oColorConverter2);
        }
    }

    //*************************************************************************
    //  Property: BooleanConverter
    //
    /// <summary>
    /// Gets an object for converting strings to Booleans.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="BooleanConverter" /> object.
    /// </value>
    //*************************************************************************

    public BooleanConverter
    BooleanConverter
    {
        get
        {
            AssertValid();

            return (m_oBooleanConverter);
        }
    }

    //*************************************************************************
    //  Property: FontSizeConverter
    //
    /// <summary>
    /// Gets an object that converts a font size between values used in the
    /// Excel workbook and values used in the NodeXL graph.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="FontSizeConverter" /> object.
    /// </value>
    //*************************************************************************

    public FontSizeConverter
    FontSizeConverter
    {
        get
        {
            AssertValid();

            return (m_oFontSizeConverter);
        }
    }

    //*************************************************************************
    //  Property: EdgeWidthConverter
    //
    /// <summary>
    /// Gets an object that converts an edge width between values used in the
    /// Excel workbook and values used in the NodeXL graph.
    /// </summary>
    ///
    /// <value>
    /// An <see cref="EdgeWidthConverter" /> object.
    /// </value>
    //*************************************************************************

    public EdgeWidthConverter
    EdgeWidthConverter
    {
        get
        {
            AssertValid();

            return (m_oEdgeWidthConverter);
        }
    }

    //*************************************************************************
    //  Property: EdgeStyleConverter
    //
    /// <summary>
    /// Gets an object that converts an edge style between values used in the
    /// Excel workbook and values used in the NodeXL graph.
    /// </summary>
    ///
    /// <value>
    /// An <see cref="EdgeStyleConverter" /> object.
    /// </value>
    //*************************************************************************

    public EdgeStyleConverter
    EdgeStyleConverter
    {
        get
        {
            AssertValid();

            return (m_oEdgeStyleConverter);
        }
    }

    //*************************************************************************
    //  Property: VertexRadiusConverter
    //
    /// <summary>
    /// Gets an object that converts a vertex radius between values used in the
    /// Excel workbook and values used in the NodeXL graph.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="VertexRadiusConverter" /> object.
    /// </value>
    //*************************************************************************

    public VertexRadiusConverter
    VertexRadiusConverter
    {
        get
        {
            AssertValid();

            return (m_oVertexRadiusConverter);
        }
    }

    //*************************************************************************
    //  Property: VertexLocationConverter
    //
    /// <summary>
    /// Gets an object that converts a vertex location between coordinates used
    /// in the Excel workbook and coordinates used in the NodeXL graph.
    /// </summary>
    ///
    /// <value>
    /// A <see cref="VertexLocationConverter" /> object.
    /// </value>
    ///
    /// <remarks>
    /// You must set the <see cref="GraphRectangle" /> property before using
    /// this property.  Otherwise, the vertex locations will be scaled to the
    /// default graph rectangle, which will result in incorrect vertex
    /// locations in the graph.
    /// </remarks>
    //*************************************************************************

    public VertexLocationConverter
    VertexLocationConverter
    {
        get
        {
            AssertValid();

            return (m_oVertexLocationConverter);
        }
    }

    //*************************************************************************
    //  Property: VertexNameDictionary
    //
    /// <summary>
    /// Gets a dictionary that maps vertex names from the edge and vertex
    /// worksheets to vertex objects in the graph.
    /// </summary>
    ///
    /// <value>
    /// Vertex dictionary.  The key is the vertex name from the edge or vertex
    /// worksheet and the value is the IVertex object.
    /// </value>
    //*************************************************************************

    public Dictionary<String, IVertex>
    VertexNameDictionary
    {
        get
        {
            AssertValid();

            return (m_oVertexNameDictionary);
        }
    }

    //*************************************************************************
    //  Property: EdgeRowIDDictionary
    //
    /// <summary>
    /// Gets a dictionary that maps edge row IDs from the edge worksheet to
    /// edge objects in the graph.
    /// </summary>
    ///
    /// <value>
    /// Edge dictionary.  The key is the edge row ID from the edge worksheet
    /// and the value is the IEdge object.
    /// </value>
    ///
    /// <remarks>
    /// The dictionary gets populated by calls to <see
    /// cref="AddToRowIDDictionary" />, but only if <see
    /// cref="FillIDColumns" /> is true.
    /// </remarks>
    //*************************************************************************

    public Dictionary<Int32, IIdentityProvider>
    EdgeRowIDDictionary
    {
        get
        {
            AssertValid();

            return (m_oEdgeRowIDDictionary);
        }
    }

    //*************************************************************************
    //  Property: VertexRowIDDictionary
    //
    /// <summary>
    /// Gets a dictionary that maps vertex row IDs from the vertex worksheet to
    /// vertex objects in the graph.
    /// </summary>
    ///
    /// <value>
    /// Vertex dictionary.  The key is the vertex row ID from the vertex
    /// worksheet and the value is the IVertex object.
    /// </value>
    ///
    /// <remarks>
    /// The dictionary gets populated by calls to <see
    /// cref="AddToRowIDDictionary" />, but only if <see
    /// cref="FillIDColumns" /> is true.
    /// </remarks>
    //*************************************************************************

    public Dictionary<Int32, IIdentityProvider>
    VertexRowIDDictionary
    {
        get
        {
            AssertValid();

            return (m_oVertexRowIDDictionary);
        }
    }

    //*************************************************************************
    //  Method: AddToRowIDDictionary()
    //
    /// <summary>
    /// Adds an edge or vertex to the edge row ID dictionary or the vertex row
    /// ID dictionary and sets the edge or vertex's Tag to the row ID.
    /// </summary>
    ///
    /// <param name="row">
    /// Row containing the edge or vertex data.
    /// </param>
    ///
    /// <param name="edgeOrVertex">
    /// Edge or vertex to get the row ID for.
    /// </param>
    ///
    /// <param name="isEdge">
    /// true if <paramref name="edgeOrVertex" /> is an edge, false if it is a
    /// vertex.
    /// </param>
    ///
    /// <remarks>
    /// If <see cref="FillIDColumns" /> is false, this method does nothing.  In
    /// that case, the state of the ID columns is unknown and the IDs shouldn't
    /// be used.
    /// </remarks>
    //*************************************************************************

    public void
    AddToRowIDDictionary
    (
        ExcelTableReader.ExcelTableRow row,
        IIdentityProvider edgeOrVertex,
        Boolean isEdge
    )
    {
        Debug.Assert(row != null);
        Debug.Assert(edgeOrVertex != null);
        AssertValid();

        if (!m_bFillIDColumns)
        {
            return;
        }

        Dictionary<Int32, IIdentityProvider> oRowIDDictionary = isEdge ?
            m_oEdgeRowIDDictionary : m_oVertexRowIDDictionary;

        // Because the derived class fills in its ID column if the column
        // exists, each cell in the column should be valid.

        String sRowID;
        Int32 iRowID = Int32.MinValue;

        if (
            row.TryGetNonEmptyStringFromCell(CommonTableColumnNames.ID,
                out sRowID)
            &&
            Int32.TryParse(sRowID, out iRowID)
            )
        {
            oRowIDDictionary.Add(iRowID, edgeOrVertex);

            Debug.Assert(edgeOrVertex is IMetadataProvider);

            // Store the row ID in the edge or vertex tag.

            ( (IMetadataProvider)edgeOrVertex ).Tag = iRowID;
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

    public  void
    AssertValid()
    {
        // m_bIgnoreVertexLocations
        // m_bFillIDColumns
        // m_bPopulateVertexWorksheet
        // m_bReadEdgeWeights
        // m_bReadGroups
        // m_bReadVertexColorFromGroups
        // m_bReadVertexShapeFromGroups
        // m_bReadVertexLabels
        // m_bReadEdgeLabels
        // m_bReadGroupLabels
        // m_bReadVertexImages
        // m_oDefaultVertexImageSize
        // m_eDefaultVertexShape
        // m_bReadAllEdgeAndVertexColumns
        // m_oGraphRectangle
        Debug.Assert(m_oColorConverter2 != null);
        Debug.Assert(m_oBooleanConverter != null);
        Debug.Assert(m_oFontSizeConverter != null);
        Debug.Assert(m_oEdgeWidthConverter != null);
        Debug.Assert(m_oEdgeStyleConverter != null);
        Debug.Assert(m_oVertexRadiusConverter != null);
        Debug.Assert(m_oVertexLocationConverter != null);
        Debug.Assert(m_oVertexNameDictionary != null);
        Debug.Assert(m_oEdgeRowIDDictionary != null);
        Debug.Assert(m_oVertexRowIDDictionary != null);
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true to ignore the X, Y, and Locked columns on the vertex worksheet.

    protected Boolean m_bIgnoreVertexLocations;

    /// true to store an ID in each row of the edge and vertex tables.

    protected Boolean m_bFillIDColumns;

    /// true to populate the vertex table with unique vertex names.

    protected Boolean m_bPopulateVertexWorksheet;

    /// true to read any edge weight column and set the edge weight value on
    /// each edge.

    protected Boolean m_bReadEdgeWeights;

    /// true to read the group worksheets.

    protected Boolean m_bReadGroups;

    /// true to set the color of a group's vertices to the vertex color
    /// specified on the Groups worksheet.

    protected Boolean m_bReadVertexColorFromGroups;

    /// true to set the shape of a group's vertices to the vertex shape
    /// specified on the Groups worksheet.

    protected Boolean m_bReadVertexShapeFromGroups;

    /// true to read the labels on the vertex worksheet.

    protected Boolean m_bReadVertexLabels;

    /// true to read the labels on the edge worksheet.

    protected Boolean m_bReadEdgeLabels;

    /// true to read the labels on the group worksheet.

    protected Boolean m_bReadGroupLabels;

    /// true to read the images on the vertex worksheet.

    protected Boolean m_bReadVertexImages;

    /// The default size of vertices drawn as images.

    protected Nullable<Single> m_oDefaultVertexImageSize;

    /// The default vertex shape.

    protected VertexShape m_eDefaultVertexShape;

    /// true to read all columns on the edge and vertex worksheets.

    protected Boolean m_bReadAllEdgeAndVertexColumns;

    /// The rectangle the graph is being drawn within.

    protected Rectangle m_oGraphRectangle;

    /// Object for converting strings to colors.

    protected ColorConverter2 m_oColorConverter2;

    /// Object for converting strings to Booleans.

    protected BooleanConverter m_oBooleanConverter;

    /// Object that converts a font size between values used in the Excel
    /// workbook and values used in the NodeXL graph.

    protected FontSizeConverter m_oFontSizeConverter;

    /// Object that converts an edge width between values used in the Excel
    /// workbook and values used in the NodeXL graph.

    protected EdgeWidthConverter m_oEdgeWidthConverter;

    /// Object that converts an edge style between values used in the Excel
    /// workbook and values used in the NodeXL graph.

    protected EdgeStyleConverter m_oEdgeStyleConverter;

    /// Object that converts a vertex radius between values used in the Excel
    /// workbook and values used in the NodeXL graph.

    protected VertexRadiusConverter m_oVertexRadiusConverter;

    /// Object that converts a vertex location between coordinates used in the
    /// Excel workbook and coordinates used in the NodeXL graph.

    protected VertexLocationConverter m_oVertexLocationConverter;

    /// Vertex dictionary.  The key is the vertex name and the value is the
    /// vertex.

    protected Dictionary<String, IVertex> m_oVertexNameDictionary;

    /// Edge dictionary.  The key is the edge row ID from the edge worksheet
    /// and the value is the IEdge object.

    protected Dictionary<Int32, IIdentityProvider> m_oEdgeRowIDDictionary;

    /// Vertex dictionary.  The key is the vertex row ID from the vertex
    /// worksheet and the value is the IVertex object.

    protected Dictionary<Int32, IIdentityProvider> m_oVertexRowIDDictionary;
}

}
