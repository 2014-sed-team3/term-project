
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.ExcelTemplate;
using Smrf.NodeXL.Visualization.Wpf;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: VertexRadiusConverterTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="VertexRadiusConverter" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class VertexRadiusConverterTest : Object
{
    //*************************************************************************
    //  Constructor: VertexRadiusConverterTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="VertexRadiusConverterTest" /> class.
    /// </summary>
    //*************************************************************************

    public VertexRadiusConverterTest()
    {
        m_oVertexRadiusConverter = null;
    }

    //*************************************************************************
    //  Method: SetUp()
    //
    /// <summary>
    /// Gets run before each test.
    /// </summary>
    //*************************************************************************

    [TestInitializeAttribute]

    public void
    SetUp()
    {
        m_oVertexRadiusConverter = new VertexRadiusConverter();
    }

    //*************************************************************************
    //  Method: TearDown()
    //
    /// <summary>
    /// Gets run after each test.
    /// </summary>
    //*************************************************************************

    [TestCleanupAttribute]

    public void
    TearDown()
    {
        m_oVertexRadiusConverter = null;
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph()
    {
        // Verify that the workbook value gets pinned on the low end.

        Single fRadiusWorkbook = VertexRadiusConverter.MinimumRadiusWorkbook;

        Assert.AreEqual(
            m_oVertexRadiusConverter.WorkbookToGraph(fRadiusWorkbook / 2f),
            m_oVertexRadiusConverter.WorkbookToGraph(fRadiusWorkbook)
            );
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph2()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph2()
    {
        // Verify that the workbook value gets pinned on the high end.

        Single fRadiusWorkbook = VertexRadiusConverter.MaximumRadiusWorkbook;

        Assert.AreEqual(
            m_oVertexRadiusConverter.WorkbookToGraph(fRadiusWorkbook * 2f),
            m_oVertexRadiusConverter.WorkbookToGraph(fRadiusWorkbook)
            );
    }

    //*************************************************************************
    //  Method: TestWorkbookToGraph3()
    //
    /// <summary>
    /// Tests the WorkbookToGraph() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToGraph3()
    {
        // Verify that the ratio of the squares of the graph values, which is
        // the ratio of their areas, is equal to the ratio of workbook values.

        Nullable<Single> fPreviousGraphValue = null;
        const Single WorkbookValueIncrement = 1;

        for
            (
            Single fWorkbookValue =
                VertexRadiusConverter.MinimumRadiusWorkbook;

            fWorkbookValue <= VertexRadiusConverter.MaximumRadiusWorkbook;

            fWorkbookValue += WorkbookValueIncrement
            )
        {
            Single fGraphValue = m_oVertexRadiusConverter.WorkbookToGraph(
                fWorkbookValue);

            if (fPreviousGraphValue.HasValue)
            {
                Assert.AreEqual(

                    (Single)Math.Pow(
                        fGraphValue / (Single)fPreviousGraphValue, 2.0),

                    fWorkbookValue / (fWorkbookValue - WorkbookValueIncrement),

                    0.001f
                    );
            }

            fPreviousGraphValue = fGraphValue;
        }
    }

    //*************************************************************************
    //  Method: TestGraphToWorkbook()
    //
    /// <summary>
    /// Tests the GraphToWorkbook() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphToWorkbook()
    {
        // Below minimum graph value.

        Single fRadiusGraph = m_oVertexRadiusConverter.WorkbookToGraph(
            VertexRadiusConverter.MinimumRadiusWorkbook) / 2f;

        Single fExpectedRadiusWorkbook =
            VertexRadiusConverter.MinimumRadiusWorkbook;

        Single fWorkbookValue =
            m_oVertexRadiusConverter.GraphToWorkbook(fRadiusGraph);

        Assert.AreEqual(fExpectedRadiusWorkbook, fWorkbookValue);
    }

    //*************************************************************************
    //  Method: TestGraphToWorkbook2()
    //
    /// <summary>
    /// Tests the GraphToWorkbook() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphToWorkbook2()
    {
        // Above maximum graph value.

        Single fRadiusGraph = m_oVertexRadiusConverter.WorkbookToGraph(
            VertexRadiusConverter.MaximumRadiusWorkbook) * 2f;

        Single fExpectedRadiusWorkbook =
            VertexRadiusConverter.MaximumRadiusWorkbook;

        Single fRadiusWorkbook =
            m_oVertexRadiusConverter.GraphToWorkbook(fRadiusGraph);

        Assert.AreEqual(fExpectedRadiusWorkbook, fRadiusWorkbook);
    }

    //*************************************************************************
    //  Method: TestGraphToWorkbook3()
    //
    /// <summary>
    /// Tests the GraphToWorkbook() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphToWorkbook3()
    {
        // Verify that the ratio of workbook values is equal to the ratio of
        // the squares of the graph values, which is the ratio of their areas.

        Nullable<Single> fPreviousWorkbookValue = null;
        const Single GraphValueIncrement = 0.1f;

        for
            (
            Single fGraphValue = m_oVertexRadiusConverter.WorkbookToGraph(
                VertexRadiusConverter.MinimumRadiusWorkbook);

            fGraphValue <= m_oVertexRadiusConverter.WorkbookToGraph(
                VertexRadiusConverter.MaximumRadiusWorkbook);

            fGraphValue += GraphValueIncrement
            )
        {
            Single fWorkbookValue =
                m_oVertexRadiusConverter.GraphToWorkbook(fGraphValue);

            if (fPreviousWorkbookValue.HasValue)
            {
                Assert.AreEqual(

                    (Single)Math.Pow(
                        fGraphValue / (fGraphValue - GraphValueIncrement), 2.0),

                    fWorkbookValue / (Single)fPreviousWorkbookValue,

                    0.001f
                    );
            }

            fPreviousWorkbookValue = fWorkbookValue;
        }
    }

    //*************************************************************************
    //  Method: TestGraphToWorkbook4()
    //
    /// <summary>
    /// Tests the GraphToWorkbook() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGraphToWorkbook4()
    {
        // Verify that the calculated values round-trip.

        const Single WorkbookValueIncrement = 0.1f;

        for
            (
            Single fWorkbookValue =
                VertexRadiusConverter.MinimumRadiusWorkbook;

            fWorkbookValue <= VertexRadiusConverter.MaximumRadiusWorkbook;

            fWorkbookValue += WorkbookValueIncrement
            )
        {
            Single fGraphValue = m_oVertexRadiusConverter.WorkbookToGraph(
                fWorkbookValue);

            Assert.AreEqual(
                fWorkbookValue,
                m_oVertexRadiusConverter.GraphToWorkbook(fGraphValue),
                0.001f
                );
        }
    }

    //*************************************************************************
    //  Method: TestWorkbookToLongerImageDimension()
    //
    /// <summary>
    /// Tests the WorkbookToLongerImageDimension() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToLongerImageDimension()
    {
        // Minimum.

        Single fRadiusWorkbook = VertexRadiusConverter.MinimumRadiusWorkbook;

        Single fLongerImageDimension =
            m_oVertexRadiusConverter.WorkbookToLongerImageDimension(
                fRadiusWorkbook);

        Assert.AreEqual(GetExpectedLongerImageDimension(fRadiusWorkbook),
            fLongerImageDimension);
    }

    //*************************************************************************
    //  Method: TestWorkbookToLongerImageDimension2()
    //
    /// <summary>
    /// Tests the WorkbookToLongerImageDimension() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToLongerImageDimension2()
    {
        // Maximum.

        Single fRadiusWorkbook = VertexRadiusConverter.MaximumRadiusWorkbook;

        Single fLongerImageDimension =
            m_oVertexRadiusConverter.WorkbookToLongerImageDimension(
                fRadiusWorkbook);

        Assert.AreEqual(GetExpectedLongerImageDimension(fRadiusWorkbook),
            fLongerImageDimension);
    }

    //*************************************************************************
    //  Method: TestWorkbookToLongerImageDimension3()
    //
    /// <summary>
    /// Tests the WorkbookToLongerImageDimension() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToLongerImageDimension3()
    {
        // Midpoint.

        Single fRadiusWorkbook = VertexRadiusConverter.MinimumRadiusWorkbook
            + (VertexRadiusConverter.MaximumRadiusWorkbook -
                VertexRadiusConverter.MinimumRadiusWorkbook) / 2F;

        Single fLongerImageDimension =
            m_oVertexRadiusConverter.WorkbookToLongerImageDimension(
                fRadiusWorkbook);

        Assert.AreEqual(GetExpectedLongerImageDimension(fRadiusWorkbook),
            fLongerImageDimension);
    }

    //*************************************************************************
    //  Method: TestWorkbookToLongerImageDimension4()
    //
    /// <summary>
    /// Tests the WorkbookToLongerImageDimension() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToLongerImageDimension4()
    {
        // Below minimum.

        Single fRadiusWorkbook =
            VertexRadiusConverter.MinimumRadiusWorkbook - 1F;

        Single fLongerImageDimension =
            m_oVertexRadiusConverter.WorkbookToLongerImageDimension(
                fRadiusWorkbook);

        Assert.AreEqual(GetExpectedLongerImageDimension(fRadiusWorkbook),
            fLongerImageDimension);
    }

    //*************************************************************************
    //  Method: TestWorkbookToLongerImageDimension5()
    //
    /// <summary>
    /// Tests the WorkbookToLongerImageDimension() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToLongerImageDimension5()
    {
        // Above maximum.

        Single fRadiusWorkbook =
            VertexRadiusConverter.MaximumRadiusWorkbook + 1F;

        Single fLongerImageDimension =
            m_oVertexRadiusConverter.WorkbookToLongerImageDimension(
                fRadiusWorkbook);

        Assert.AreEqual(GetExpectedLongerImageDimension(fRadiusWorkbook),
            fLongerImageDimension);
    }

    //*************************************************************************
    //  Method: TestWorkbookToLabelFontSize()
    //
    /// <summary>
    /// Tests the WorkbookToLabelFontSize() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToLabelFontSize()
    {
        // Minimum.

        Single fRadiusWorkbook = VertexRadiusConverter.MinimumRadiusWorkbook;

        Single fExpectedLabelFontSize =
            VertexRadiusConverter.MinimumLabelFontSize;

        Single fLabelFontSize =
            m_oVertexRadiusConverter.WorkbookToLabelFontSize(fRadiusWorkbook);

        Assert.AreEqual(fExpectedLabelFontSize, fLabelFontSize);
    }

    //*************************************************************************
    //  Method: TestWorkbookToLabelFontSize2()
    //
    /// <summary>
    /// Tests the WorkbookToLabelFontSize() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToLabelFontSize2()
    {
        // Maximum.

        Single fRadiusWorkbook = VertexRadiusConverter.MaximumRadiusWorkbook;

        Single fExpectedLabelFontSize =
            VertexRadiusConverter.MaximumLabelFontSize;

        Single fLabelFontSize =
            m_oVertexRadiusConverter.WorkbookToLabelFontSize(fRadiusWorkbook);

        Assert.AreEqual(fExpectedLabelFontSize, fLabelFontSize);
    }

    //*************************************************************************
    //  Method: TestWorkbookToLabelFontSize3()
    //
    /// <summary>
    /// Tests the WorkbookToLabelFontSize() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToLabelFontSize3()
    {
        // Midpoint.

        Single fRadiusWorkbook = VertexRadiusConverter.MinimumRadiusWorkbook
            + (VertexRadiusConverter.MaximumRadiusWorkbook -
                VertexRadiusConverter.MinimumRadiusWorkbook) / 2F;

        Single fExpectedLabelFontSize =
            VertexRadiusConverter.MinimumLabelFontSize
            + (VertexRadiusConverter.MaximumLabelFontSize -
                VertexRadiusConverter.MinimumLabelFontSize) / 2F;

        Single fLabelFontSize =
            m_oVertexRadiusConverter.WorkbookToLabelFontSize(fRadiusWorkbook);

        Assert.AreEqual(fExpectedLabelFontSize, fLabelFontSize);
    }

    //*************************************************************************
    //  Method: TestWorkbookToLabelFontSize4()
    //
    /// <summary>
    /// Tests the WorkbookToLabelFontSize() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToLabelFontSize4()
    {
        // Below minimum.

        Single fRadiusWorkbook =
            VertexRadiusConverter.MinimumRadiusWorkbook - 1F;

        Single fExpectedLabelFontSize =
            VertexRadiusConverter.MinimumLabelFontSize;

        Single fLabelFontSize =
            m_oVertexRadiusConverter.WorkbookToLabelFontSize(fRadiusWorkbook);

        Assert.AreEqual(fExpectedLabelFontSize, fLabelFontSize);
    }

    //*************************************************************************
    //  Method: TestWorkbookToLabelFontSize5()
    //
    /// <summary>
    /// Tests the WorkbookToLabelFontSize() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestWorkbookToLabelFontSize5()
    {
        // Above maximum.

        Single fRadiusWorkbook =
            VertexRadiusConverter.MaximumRadiusWorkbook + 1F;

        Single fExpectedLabelFontSize =
            VertexRadiusConverter.MaximumLabelFontSize;

        Single fLabelFontSize =
            m_oVertexRadiusConverter.WorkbookToLabelFontSize(fRadiusWorkbook);

        Assert.AreEqual(fExpectedLabelFontSize, fLabelFontSize);
    }

    //*************************************************************************
    //  Method: GetExpectedLongerImageDimension()
    //
    /// <summary>
    /// Gets the expected return value from the
    /// WorkbookToLongerImageDimension() method.
    /// </summary>
    ///
    /// <param name="fRadiusWorkbook">
    /// The workbook value.
    /// </param>
    //*************************************************************************

    protected Single
    GetExpectedLongerImageDimension
    (
        Single fRadiusWorkbook
    )
    {
        return ( 2.0F *
            m_oVertexRadiusConverter.WorkbookToGraph(fRadiusWorkbook) );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object to test.

    protected VertexRadiusConverter m_oVertexRadiusConverter;
}

}
