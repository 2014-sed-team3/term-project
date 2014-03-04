
using System;
using System.IO;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.SocialNetworkLib.Twitter;

namespace Smrf.Common.UnitTests
{
//*****************************************************************************
//  Class: TwitterStatusParserTest
//
/// <summary>
/// This is a Visual Studio test fixture for the <see
/// cref="TwitterStatusParser" /> class.
/// </summary>
//*****************************************************************************

[TestClassAttribute]

public class TwitterStatusParserTest : Object
{
    //*************************************************************************
    //  Constructor: TwitterStatusParserTest()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="TwitterStatusParserTest" /> class.
    /// </summary>
    //*************************************************************************

    public TwitterStatusParserTest()
    {
        // (Do nothing.)
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
        // (Do nothing.)
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
        // (Do nothing.)
    }

    //*************************************************************************
    //  Method: TestTryParseStatus()
    //
    /// <summary>
    /// Tests the TryParseStatus() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestTryParseStatus()
    {
        Dictionary<String, Object> statusValueDictionary =
            GetStatusValueDictionary();

        Int64 statusID;
        DateTime statusDateUtc;
        String screenName;
        String text;
        String rawStatusJson;
        Dictionary<String, Object> userValueDictionary;

        Boolean returnValue = TwitterStatusParser.TryParseStatus(
            statusValueDictionary,
            out statusID,
            out statusDateUtc,
            out screenName,
            out text,
            out rawStatusJson,
            out userValueDictionary
            );

        Assert.IsTrue(returnValue);

        Assert.AreEqual(330018625320787969, statusID);
        Assert.AreEqual(new DateTime(2013, 5, 2, 17, 59, 6), statusDateUtc);
        Assert.AreEqual("Andrew_Muras", screenName);

        Assert.AreEqual("Completed 1st SNA analysis a week ago using NodeXL-Marathon Oil used same tool and interesting perspective on analyzing COPs #apqckmconf",
            text);

        // The expected raw status JSON, which is stored in a test file, is not
        // identical to what came in the Twitter response.  Specifically, the
        // JavaScriptSerializer that TryParseStatus() used to rebuild the JSON
        // escapes single quotes and does not escape forward slashes.

        String expectedRawStatusJson = ReadTextFile(
            ExpectedRawStatusJsonFilePath);

        Assert.AreEqual(expectedRawStatusJson, rawStatusJson);

        // Confirm that the raw status JSON can be parsed again.

        Object deserializedStatus =
            ( new JavaScriptSerializer() ).DeserializeObject(rawStatusJson);

        Dictionary<String, Object> rebuiltStatusValueDictionary =
            ( Dictionary<String, Object> )deserializedStatus;

        Assert.AreEqual(statusValueDictionary.Count,
            rebuiltStatusValueDictionary.Count);
    }

    //*************************************************************************
    //  Method: TestUserValueDictionaryToRawJson()
    //
    /// <summary>
    /// Tests the UserValueDictionaryToRawJson() method.
    /// </summary>
    //*************************************************************************

    [TestMethodAttribute]

    public void
    TestUserValueDictionaryToRawJson()
    {
        Dictionary<String, Object> statusValueDictionary =
            GetStatusValueDictionary();

        Int64 statusID;
        DateTime statusDateUtc;
        String screenName;
        String text;
        String rawStatusJson;
        Dictionary<String, Object> userValueDictionary;

        Boolean returnValue = TwitterStatusParser.TryParseStatus(
            statusValueDictionary,
            out statusID,
            out statusDateUtc,
            out screenName,
            out text,
            out rawStatusJson,
            out userValueDictionary
            );

        Assert.IsTrue(returnValue);

        String rawUserJson = TwitterStatusParser.UserValueDictionaryToRawJson(
            userValueDictionary);

        // The expected raw user JSON, which is stored in a test file, is not
        // identical to what came in the Twitter response.  Specifically, the
        // JavaScriptSerializer that UserValueDictionaryToRawJson() used to
        // rebuild the JSON escapes single quotes and does not escape forward
        // slashes.

        String expectedRawUserJson = ReadTextFile(ExpectedRawUserJsonFilePath);

        Assert.AreEqual(expectedRawUserJson, rawUserJson);

        // Confirm that the raw user JSON can be parsed again.

        Object deserializedUser =
            ( new JavaScriptSerializer() ).DeserializeObject(rawUserJson);

        Dictionary<String, Object> rebuiltUserValueDictionary =
            ( Dictionary<String, Object> )deserializedUser;

        Assert.AreEqual(userValueDictionary.Count,
            rebuiltUserValueDictionary.Count);
    }

    //*************************************************************************
    //  Method: GetStatusValueDictionary()
    //
    /// <summary>
    /// Gets at status value dictionary from a test file.
    /// </summary>
    ///
    /// <remarks>
    /// A status value dictionary.
    /// </remarks>
    //*************************************************************************

    private Dictionary<String, Object>
    GetStatusValueDictionary()
    {
        // A sample response from Twitter is stored in a test file.  Get the
        // response string.

        String twitterSearchApiResponse;

        using ( StreamReader streamReader = new StreamReader(
            TwitterSearchApiResponseFilePath) )
        {
            twitterSearchApiResponse = streamReader.ReadToEnd();
        }

        Object deserializedTwitterSearchApiResponse = 
            ( new JavaScriptSerializer() ).DeserializeObject(
                twitterSearchApiResponse);

        // The top level of the Json response contains a set of name/value
        // pairs.  The value for the "statuses" name is the array of statuses.

        Dictionary<String, Object> responseDictionary =
            ( Dictionary<String, Object> )deserializedTwitterSearchApiResponse;

        Object [] statuses = ( Object [] )responseDictionary["statuses"];

        Dictionary<String, Object> statusValueDictionary =
            ( Dictionary<String, Object> )statuses[0];

        return (statusValueDictionary);
    }

    //*************************************************************************
    //  Method: ReadTextFile()
    //
    /// <summary>
    /// Reads a text file.
    /// </summary>
    ///
    /// <param name="textFilePath">
    /// Full path to the text file.
    /// </param>
    //*************************************************************************

    private String
    ReadTextFile
    (
        String textFilePath
    )
    {
        using ( StreamReader streamReader = new StreamReader(textFilePath) )
        {
            return ( streamReader.ReadToEnd() );
        }
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    // Full path of the file that contains the response to a Twitter search API
    // call.

    private const String TwitterSearchApiResponseFilePath =
        @"..\..\..\TestFiles\TwitterSearchApiResponse.txt";

    // Full path of the file that contains the expected raw status as JSON.

    private const String ExpectedRawStatusJsonFilePath =
        @"..\..\..\TestFiles\ExpectedRawStatusJson.txt";

    // Full path of the file that contains the expected raw user information as
    // JSON.

    private const String ExpectedRawUserJsonFilePath =
        @"..\..\..\TestFiles\ExpectedRawUserJson.txt";
}

}
