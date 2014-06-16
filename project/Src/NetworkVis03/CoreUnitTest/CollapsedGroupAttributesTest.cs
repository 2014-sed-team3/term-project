
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: CollapsedGroupAttributesTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="CollapsedGroupAttributes" /> class.
/// </summary>
///
/// <remarks>
/// Most of the testing is performed elsewhere, on the base
/// PersistableStringDictionary class.  This fixture tests only those methods
/// that CollapsedGroupAttributes adds to the base class.
/// </remarks>
//*****************************************************************************

[TestClassAttribute]

public class CollapsedGroupAttributesTest : Object
{
    //*************************************************************************
    //  Constructor: CollapsedGroupAttributesTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="CollapsedGroupAttributesTest" /> class.
    /// </summary>
    //*************************************************************************

    public CollapsedGroupAttributesTest()
    {
        m_oCollapsedGroupAttributes = null;
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
        m_oCollapsedGroupAttributes = new CollapsedGroupAttributes();
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
        m_oCollapsedGroupAttributes = null;
    }

    //*************************************************************************
    //  Method: TestToAndFromString()
    //
    /// <summary>
    /// Tests the ToString() and FromString() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestToAndFromString()
    {
        // Typical case.

        m_oCollapsedGroupAttributes.Add("Key1", "Value1");
        m_oCollapsedGroupAttributes.Add("Key2", "Value2");
        m_oCollapsedGroupAttributes.Add("Key3", "Value3");

        String sSavedString = m_oCollapsedGroupAttributes.ToString();

        CollapsedGroupAttributes oCollapsedGroupAttributes2 =
            CollapsedGroupAttributes.FromString(sSavedString);

        Assert.AreEqual( "Value1", oCollapsedGroupAttributes2["Key1"] );
        Assert.AreEqual( "Value2", oCollapsedGroupAttributes2["Key2"] );
        Assert.AreEqual( "Value3", oCollapsedGroupAttributes2["Key3"] );
    }

    //*************************************************************************
    //  Method: TestToAndFromString2()
    //
    /// <summary>
    /// Tests the ToString() and FromString() methods.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestToAndFromString2()
    {
        // Empty and null keys, which aren't persisted.

        m_oCollapsedGroupAttributes.Add("Key1", "Value1");
        m_oCollapsedGroupAttributes.Add("KeyX", "");
        m_oCollapsedGroupAttributes.Add("Key2", "Value2");
        m_oCollapsedGroupAttributes.Add("Key3", "Value3");
        m_oCollapsedGroupAttributes.Add("KeyY", null);

        String sSavedString = m_oCollapsedGroupAttributes.ToString();

        CollapsedGroupAttributes oCollapsedGroupAttributes2 =
            CollapsedGroupAttributes.FromString(sSavedString);

        Assert.AreEqual( "Value1", oCollapsedGroupAttributes2["Key1"] );
        Assert.AreEqual( "Value2", oCollapsedGroupAttributes2["Key2"] );
        Assert.AreEqual( "Value3", oCollapsedGroupAttributes2["Key3"] );
        Assert.IsFalse( oCollapsedGroupAttributes2.ContainsKey("KeyX") );
        Assert.IsFalse( oCollapsedGroupAttributes2.ContainsKey("KeyY") );
    }

    //*************************************************************************
    //  Method: TestFromString()
    //
    /// <summary>
    /// Tests the FromString() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestFromString()
    {
        // Empty string, which is allowed.

        CollapsedGroupAttributes oCollapsedGroupAttributes2 =
            CollapsedGroupAttributes.FromString(String.Empty);

        Assert.AreEqual(0, oCollapsedGroupAttributes2.Count);
    }

    //*************************************************************************
    //  Method: TestGetGroupType()
    //
    /// <summary>
    /// Tests the GetGroupType() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetGroupType()
    {
        // Value not found.

        Assert.AreEqual( String.Empty,
            m_oCollapsedGroupAttributes.GetGroupType() );
    }

    //*************************************************************************
    //  Method: TestGetGroupType2()
    //
    /// <summary>
    /// Tests the GetGroupType() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestGetGroupType2()
    {
        // Value found.

        const String GroupType = "TheGroupType";

        m_oCollapsedGroupAttributes.Add(CollapsedGroupAttributeKeys.Type,
            GroupType);

        Assert.AreEqual( GroupType,
            m_oCollapsedGroupAttributes.GetGroupType() );
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object being tested.

    protected CollapsedGroupAttributes m_oCollapsedGroupAttributes;
}

}
