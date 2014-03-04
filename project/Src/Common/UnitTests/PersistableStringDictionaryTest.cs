
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.AppLib;

namespace Smrf.Common.UnitTests
{
//*****************************************************************************
//  Class: PersistableStringDictionaryTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="PersistableStringDictionary" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class PersistableStringDictionaryTest : Object
{
    //*************************************************************************
    //  Constructor: PersistableStringDictionaryTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="PersistableStringDictionaryTest" /> class.
    /// </summary>
    //*************************************************************************

    public PersistableStringDictionaryTest()
    {
        m_oPersistableStringDictionary = null;
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
        m_oPersistableStringDictionary = new PersistableStringDictionary();
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
        m_oPersistableStringDictionary = null;
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

        m_oPersistableStringDictionary.Add("Key1", "Value1");
        m_oPersistableStringDictionary.Add("Key2", "Value2");
        m_oPersistableStringDictionary.Add("Key3", "Value3");

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Assert.AreEqual( "Value1", oPersistableStringDictionary2["Key1"] );
        Assert.AreEqual( "Value2", oPersistableStringDictionary2["Key2"] );
        Assert.AreEqual( "Value3", oPersistableStringDictionary2["Key3"] );
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

        m_oPersistableStringDictionary.Add("Key1", "Value1");
        m_oPersistableStringDictionary.Add("KeyX", "");
        m_oPersistableStringDictionary.Add("Key2", "Value2");
        m_oPersistableStringDictionary.Add("Key3", "Value3");
        m_oPersistableStringDictionary.Add("KeyY", null);

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Assert.AreEqual( "Value1", oPersistableStringDictionary2["Key1"] );
        Assert.AreEqual( "Value2", oPersistableStringDictionary2["Key2"] );
        Assert.AreEqual( "Value3", oPersistableStringDictionary2["Key3"] );
        Assert.IsFalse( oPersistableStringDictionary2.ContainsKey("KeyX") );
        Assert.IsFalse( oPersistableStringDictionary2.ContainsKey("KeyY") );
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

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(String.Empty);

        Assert.AreEqual(0, oPersistableStringDictionary2.Count);
    }

    //*************************************************************************
    //  Method: TestAdd()
    //
    /// <summary>
    /// Tests the Add(String, Int32) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd()
    {
        m_oPersistableStringDictionary.Add("Key1", 123456789);

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Assert.AreEqual( "123456789", oPersistableStringDictionary2["Key1"] );
    }

    //*************************************************************************
    //  Method: TestAdd2()
    //
    /// <summary>
    /// Tests the Add(String, Double) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd2()
    {
        m_oPersistableStringDictionary.Add("Key1", 123456789.1234);

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Assert.AreEqual( "123456789.1234",
            oPersistableStringDictionary2["Key1"] );
    }

    //*************************************************************************
    //  Method: TestAdd3()
    //
    /// <summary>
    /// Tests the Add(String, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd3()
    {
        m_oPersistableStringDictionary.Add("Key1", true);

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Assert.AreEqual( "True", oPersistableStringDictionary2["Key1"] );
    }

    //*************************************************************************
    //  Method: TestAdd4()
    //
    /// <summary>
    /// Tests the Add(String, Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestAdd4()
    {
        m_oPersistableStringDictionary.Add("Key1", false);

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Assert.AreEqual( "False", oPersistableStringDictionary2["Key1"] );
    }

    //*************************************************************************
    //  Method: TestTryGetValue()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Int32) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetValue()
    {
        // Value found.

        m_oPersistableStringDictionary.Add("Key1", 123456789);

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Int32 iValue;

        Assert.IsTrue(
            oPersistableStringDictionary2.TryGetValue("Key1", out iValue) );

        Assert.AreEqual(123456789, iValue);
    }

    //*************************************************************************
    //  Method: TestTryGetValue2()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Int32) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetValue2()
    {
        // Value not found.

        m_oPersistableStringDictionary.Add("Key1", 123456789);

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Int32 iValue;

        Assert.IsFalse(
            oPersistableStringDictionary2.TryGetValue("KeyX", out iValue) );
    }

    //*************************************************************************
    //  Method: TestTryGetValue3()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Int32) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetValue3()
    {
        // Value not an Int32.

        m_oPersistableStringDictionary.Add("Key1", "abc");

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Int32 iValue;

        Assert.IsFalse(
            oPersistableStringDictionary2.TryGetValue("Key1", out iValue) );
    }

    //*************************************************************************
    //  Method: TestTryGetValue4()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Double) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetValue4()
    {
        // Value found.

        m_oPersistableStringDictionary.Add("Key1", -123456789.54321);

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Double dValue;

        Assert.IsTrue(
            oPersistableStringDictionary2.TryGetValue("Key1", out dValue) );

        Assert.AreEqual(-123456789.54321, dValue);
    }

    //*************************************************************************
    //  Method: TestTryGetValue5()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Double) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetValue5()
    {
        // Value not found.

        m_oPersistableStringDictionary.Add("Key1", 123456789.1);

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Double dValue;

        Assert.IsFalse(
            oPersistableStringDictionary2.TryGetValue("KeyX", out dValue) );
    }

    //*************************************************************************
    //  Method: TestTryGetValue6()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Double) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetValue6()
    {
        // Value not an Double.

        m_oPersistableStringDictionary.Add("Key1", "abc");

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Double dValue;

        Assert.IsFalse(
            oPersistableStringDictionary2.TryGetValue("Key1", out dValue) );
    }

    //*************************************************************************
    //  Method: TestTryGetValue7()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetValue7()
    {
        // Value found.

        m_oPersistableStringDictionary.Add("Key1", true);

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Boolean bValue;

        Assert.IsTrue(
            oPersistableStringDictionary2.TryGetValue("Key1", out bValue) );

        Assert.IsTrue(bValue);
    }

    //*************************************************************************
    //  Method: TestTryGetValue8()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetValue8()
    {
        // Write a lower-case string, look for value.

        m_oPersistableStringDictionary.Add("Key1", "true");

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Boolean bValue;

        Assert.IsTrue(
            oPersistableStringDictionary2.TryGetValue("Key1", out bValue) );

        Assert.IsTrue(bValue);
    }

    //*************************************************************************
    //  Method: TestTryGetValue9()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetValue9()
    {
        // Write a lower-case string, look for value.

        m_oPersistableStringDictionary.Add("Key1", "false");

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Boolean bValue;

        Assert.IsTrue(
            oPersistableStringDictionary2.TryGetValue("Key1", out bValue) );

        Assert.IsFalse(bValue);
    }

    //*************************************************************************
    //  Method: TestTryGetValue10()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetValue10()
    {
        // Value found.

        m_oPersistableStringDictionary.Add("Key1", false);

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Boolean bValue;

        Assert.IsTrue(
            oPersistableStringDictionary2.TryGetValue("Key1", out bValue) );

        Assert.IsFalse(bValue);
    }

    //*************************************************************************
    //  Method: TestTryGetValue11()
    //
    /// <summary>
    /// Tests the TryGetValue(String, out Boolean) method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryGetValue11()
    {
        // Value not a Boolean.

        m_oPersistableStringDictionary.Add("Key1", "abc");

        String sSavedString = m_oPersistableStringDictionary.ToString();

        PersistableStringDictionary oPersistableStringDictionary2 =
            PersistableStringDictionary.FromString(sSavedString);

        Boolean bValue;

        Assert.IsFalse(
            oPersistableStringDictionary2.TryGetValue("Key1", out bValue) );
    }

    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// Object being tested.

    protected PersistableStringDictionary m_oPersistableStringDictionary;
}

}
