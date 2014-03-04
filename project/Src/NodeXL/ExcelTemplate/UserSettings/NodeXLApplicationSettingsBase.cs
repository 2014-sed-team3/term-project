
// Define TRACE_CALLS to output debug messages to the trace listeners.

// #define TRACE_CALLS

using System;
using System.Configuration;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Smrf.XmlLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: NodeXLApplicationSettingsBase
//
/// <summary>
/// Base class for most NodeXL's user settings classes.
/// </summary>
///
/// <remarks>
/// This is the base class for all of NodeXL's user settings classes except
/// those used to save dialog positions, which are derived from <see
/// cref="Smrf.AppLib.FormSettings" />.
///
/// <para>
/// This class saves user settings to the workbook so that the settings travel
/// with the workbook.  It does this by intercepting calls to the <see
/// cref="this" /> property, the <see cref="Save" /> method, and the <see
/// cref="Reset" /> method.  Within each intercept, the workbook settings are
/// copied to the file that the base class uses to store settings, the base
/// class is allowed to do its usual work on the file, and if appropriate, the
/// file is then copied back to the workbook settings.  The net result is that
/// the base class effectively reads and writes the workbook settings.  The
/// actual file settings are used by NodeXL only when a new workbook is
/// created.
/// </para>
///
/// <para>
/// This is much easier than creating a custom settings provider, which is not
/// well documented and far from trivial.  That's especially true when settings
/// groups are used, as they are in NodeXL.
/// </para>
///
/// <para>
/// In this class, the file that the base class uses to store settings is
/// called the "standard settings file", and the settings stored in the
/// workbook are called the "workbook settings."
/// </para>
///
/// </remarks>
//*****************************************************************************

public class NodeXLApplicationSettingsBase : ApplicationSettingsBase
{
    //*************************************************************************
    //  Constructor: NodeXLApplicationSettingsBase()
    //
    /// <overloads>
    /// Initializes a new instance of the <see
    /// cref="NodeXLApplicationSettingsBase" /> class.
    /// </overloads>
    ///
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NodeXLApplicationSettingsBase" /> class.  Uses the workbook
    /// settings.
    /// </summary>
    //*************************************************************************

    public
    NodeXLApplicationSettingsBase()
    :
    this(true)
    {
        // (Do nothing else.)
    }

    //*************************************************************************
    //  Constructor: NodeXLApplicationSettingsBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="NodeXLApplicationSettingsBase" /> class with a "use workbook
    /// settings" flag.
    /// </summary>
    //*************************************************************************

    public
    NodeXLApplicationSettingsBase
    (
        Boolean useWorkbookSettings
    )
    {
        #if TRACE_CALLS
        Debug.WriteLine("NodeXLApplicationSettingsBase: Constructor");
        #endif

        m_bUseWorkbookSettings = useWorkbookSettings;
        m_bItemCalled = false;

        // AssertValid();
    }

    //*************************************************************************
    //  Property: Item[]
    //
    /// <summary>
    /// Gets or sets the value of the specified application settings property.
    /// </summary>
    ///
    /// <param name="propertyName">
    /// A String containing the name of the property to access.
    /// </param>
    ///
    /// <value>
    /// If found, the value of the named settings property; otherwise, null.
    /// </value>
    //*************************************************************************

    public override Object
    this
    [
        String propertyName
    ]
    {
        get
        {
            AssertValid();

            String sOldStandardSettings = null;

            if (m_bUseWorkbookSettings && !m_bItemCalled)
            {
                #if TRACE_CALLS

                Debug.WriteLine("NodeXLApplicationSettingsBase: Item[]"
                    + " getter.");

                #endif

                // Write the workbook settings to the standard settings file so
                // that the base class effectively reads from the workbook
                // settings.  The base class actually reads and caches all
                // property values on the first call to Item[] on this
                // instance, so this needs to be done just once.

                sOldStandardSettings = ReadStandardSettingsFile();
                CopyWorkbookSettingsToStandardSettings();
            }

            Object oValue = base[propertyName];

            if (sOldStandardSettings != null)
            {
                // Restore the old standard settings file.

                WriteStandardSettingsFile(sOldStandardSettings);
                m_bItemCalled = true;
            }

            return (oValue);
        }

        // Note that the Item setter doesn't need to be overridden.  When the
        // settings framework calls the setter, it actually writes to the
        // cached copy of the settings.  Settings don't get saved back to
        // persistent storage until Save() is called.
    }

    //*************************************************************************
    //  Method: OnNewWorkbook()
    //
    /// <summary>
    /// Performs tasks required when a new workbook is created.
    /// </summary>
    //*************************************************************************

    public void
    OnNewWorkbook()
    {
        AssertValid();

        // Start with default settings, then selectively copy the standard
        // settings file to the copy.

        SetWorkbookSettings(
            CopyAndFilterSettings(
                ReadStandardSettingsFile(),
                DefaultStandardSettingsFileContents
            ) );
    }

    //*************************************************************************
    //  Method: UseWorkbookSettingsForNewWorkbook()
    //
    /// <summary>
    /// Copies the workbook settings to the standard settings file so they will
    /// be used for new workbooks.
    /// </summary>
    //*************************************************************************

    public void
    UseWorkbookSettingsForNewWorkbooks()
    {
        AssertValid();

        // Make a copy of the standard settings, selectively copy the workbook
        // settings to the copy of the standard settings, then write the copy
        // to the standard settings file.

        WriteStandardSettingsFile(
            CopyAndFilterSettings(
                GetWorkbookSettings(),
                ReadStandardSettingsFile()
            ) );
    }

    //*************************************************************************
    //  Method: UseNewWorkbookSettingsForThisWorkbook()
    //
    /// <summary>
    /// Copies the standard settings file to the workbook settings.
    /// </summary>
    //*************************************************************************

    public void
    UseNewWorkbookSettingsForThisWorkbook()
    {
        AssertValid();

        CopyStandardSettingsToWorkbookSettings();
    }

    //*************************************************************************
    //  Method: Save()
    //
    /// <summary>
    /// Stores the current values of the application settings properties.
    /// </summary>
    //*************************************************************************

    public override void
    Save()
    {
        AssertValid();

        #if TRACE_CALLS
        Debug.WriteLine("NodeXLApplicationSettingsBase: Save()");
        #endif

        String sOldStandardSettings = null;

        if (m_bUseWorkbookSettings)
        {
            // Write the workbook settings to the standard settings file so
            // that the base class writes to the workbook settings.

            sOldStandardSettings = ReadStandardSettingsFile();
            CopyWorkbookSettingsToStandardSettings();
        }

        base.Save();

        if (m_bUseWorkbookSettings)
        {
            // Copy the saved settings to the workbook settings, then restore
            // the old standard settings file.

            CopyStandardSettingsToWorkbookSettings();
            WriteStandardSettingsFile(sOldStandardSettings);
        }
    }

    //*************************************************************************
    //  Method: Reset()
    //
    /// <summary>
    /// Restores the persisted application settings values to their
    /// corresponding default properties.
    /// </summary>
    //*************************************************************************

    public new void
    Reset()
    {
        AssertValid();

        #if TRACE_CALLS
        Debug.WriteLine("NodeXLApplicationSettingsBase: Reset()");
        #endif

        String sOldStandardSettings = null;

        if (m_bUseWorkbookSettings)
        {
            // Write the workbook settings to the standard settings file so
            // that the base class resets the workbook settings.

            sOldStandardSettings = ReadStandardSettingsFile();
            CopyWorkbookSettingsToStandardSettings();
        }

        base.Reset();

        // Although base.Reset() calls Reload(), that doesn't force the
        // properties to be read again until the first call to Item[] is made.
        // The properties need to be read again right now.  Force this by
        // reading any one of them.

        foreach (SettingsProperty oSettingsProperty in this.Properties)
        {
            Object oValue = this[oSettingsProperty.Name];
            break;
        }

        if (m_bUseWorkbookSettings)
        {
            // Copy the reset settings to the workbook settings, then restore
            // the old standard settings file.

            CopyStandardSettingsToWorkbookSettings();
            WriteStandardSettingsFile(sOldStandardSettings);
        }
    }

    //*************************************************************************
    //  Method: GetWorkbookSettings()
    //
    /// <summary>
    /// Gets the users settings for this workbook.
    /// </summary>
    ///
    /// <returns>
    /// The workbook's user settings.
    /// </returns>
    //*************************************************************************

    protected String
    GetWorkbookSettings()
    {
        AssertValid();

        return ( GetPerWorkbookSettings().WorkbookSettings );
    }

    //*************************************************************************
    //  Method: SetWorkbookSettings()
    //
    /// <summary>
    /// Sets the users settings for this workbook.
    /// </summary>
    ///
    /// <param name="sWorkbookSettings">
    /// The workbook's user settings.
    /// </param>
    //*************************************************************************

    protected void
    SetWorkbookSettings
    (
        String sWorkbookSettings
    )
    {
        AssertValid();
        Debug.Assert( !String.IsNullOrEmpty(sWorkbookSettings) );

        GetPerWorkbookSettings().WorkbookSettings = sWorkbookSettings;
    }

    //*************************************************************************
    //  Method: GetPerWorkbookSettings()
    //
    /// <summary>
    /// Gets a new PerWorkbookSettings object.
    /// </summary>
    ///
    /// <returns>
    /// A new PerWorkbookSettings object where the workbook settings are
    /// stored.
    /// </returns>
    //*************************************************************************

    protected PerWorkbookSettings
    GetPerWorkbookSettings()
    {
        AssertValid();

        Debug.Assert(Globals.ThisWorkbook != null);

        return ( new PerWorkbookSettings( Globals.ThisWorkbook.InnerObject) );
    }

    #if TRACE_CALLS

    //*************************************************************************
    //  Method: OnSettingsLoaded()
    //
    /// <summary>
    /// Handles the SettingsLoaded event.
    /// </summary>
    ///
    /// <param name="sender">
    /// Standard event argument.
    /// </param>
    ///
    /// <param name="e">
    /// Standard event argument.
    /// </param>
    //*************************************************************************

    protected override void
    OnSettingsLoaded
    (
        Object sender,
        SettingsLoadedEventArgs e
    )
    {
        AssertValid();

        Debug.WriteLine("NodeXLApplicationSettingsBase: OnSettingsLoaded()");
        base.OnSettingsLoaded(sender, e);
    }

    #endif

    //*************************************************************************
    //  Method: CopyStandardSettingsToWorkbookSettings()
    //
    /// <summary>
    /// Copies the standard settings file to the workbook settings.
    /// </summary>
    //*************************************************************************

    protected void
    CopyStandardSettingsToWorkbookSettings()
    {
        AssertValid();

        SetWorkbookSettings( ReadStandardSettingsFile() );
    }

    //*************************************************************************
    //  Method: CopyWorkbookSettingsToStandardSettings()
    //
    /// <summary>
    /// Copies the workbook settings to the standard settings file.
    /// </summary>
    //*************************************************************************

    protected void
    CopyWorkbookSettingsToStandardSettings()
    {
        AssertValid();

        WriteStandardSettingsFile( GetWorkbookSettings() );
    }

    //*************************************************************************
    //  Method: ReadStandardSettingsFile()
    //
    /// <summary>
    /// Reads the standard settings file.
    /// </summary>
    ///
    /// <returns>
    /// The contents of the standard settings file.  Never null or empty.
    /// </returns>
    //*************************************************************************

    protected String
    ReadStandardSettingsFile()
    {
        AssertValid();

        // Make sure the standard settings file exists.

        CreateStandardSettingsFolder(true);

        using ( StreamReader oStreamReader = new StreamReader(
            GetStandardSettingsFileStream(false) ) )
        {
            return ( oStreamReader.ReadToEnd() );
        }
    }

    //*************************************************************************
    //  Method: WriteStandardSettingsFile()
    //
    /// <summary>
    /// Writes the standard settings file.
    /// </summary>
    ///
    /// <param name="sFileContents">
    /// The file contents to write.
    /// </param>
    //*************************************************************************

    protected void
    WriteStandardSettingsFile
    (
        String sFileContents
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sFileContents) );
        AssertValid();

        // Make sure the standard settings folder exists.

        CreateStandardSettingsFolder(false);

        using ( StreamWriter oStreamWriter = new StreamWriter(
            GetStandardSettingsFileStream(true) ) )
        {
            oStreamWriter.Write(sFileContents);
        }
    }

    //*************************************************************************
    //  Method: CopyAndFilterSettings()
    //
    /// <summary>
    /// Selectively copies settings.
    /// </summary>
    ///
    /// <param name="sSourceSettings">
    /// The source settings.
    /// </param>
    ///
    /// <param name="sDestinationSettings">
    /// The current destination settings.
    /// </param>
    ///
    /// <returns>
    /// The copied, edited destination settings.
    /// </returns>
    ///
    /// <remarks>
    /// This method makes a copy of <paramref name="sDestinationSettings" />,
    /// then selectively copies settings from <paramref
    /// name="sSourceSettings" /> to the copy.  It returns the edited copy.
    /// </remarks>
    //*************************************************************************

    protected String
    CopyAndFilterSettings
    (
        String sSourceSettings,
        String sDestinationSettings
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sSourceSettings) );
        Debug.Assert( !String.IsNullOrEmpty(sDestinationSettings) );
        AssertValid();

        // We don't want to modify the user's notification, dialog position,
        // or password settings that are stored in the destination.  Use
        // XmlDocuments to copy only the other settings from the source to the
        // destination.

        XmlDocument oSourceDocument = new XmlDocument();
        oSourceDocument.LoadXml(sSourceSettings);

        XmlDocument oDestinationDocument = new XmlDocument();
        oDestinationDocument.LoadXml(sDestinationSettings);

        // See the DefaultStandardSettingsFileContents constant for an example
        // of what the XML documents look like.

        const String SectionGroupXPath =
            "configuration/configSections/sectionGroup";

        const String UserSettingsXPath = "configuration/userSettings";

        XmlNode oSourceSectionGroupNode =
            XmlUtil2.SelectRequiredSingleNode(oSourceDocument,
                SectionGroupXPath, null);

        XmlNode oDestinationSectionGroupNode =
            XmlUtil2.SelectRequiredSingleNode(oDestinationDocument,
                SectionGroupXPath, null);

        XmlNode oSourceUserSettingsNode =
            XmlUtil2.SelectRequiredSingleNode(oSourceDocument,
                UserSettingsXPath, null);

        XmlNode oDestinationUserSettingsNode =
            XmlUtil2.SelectRequiredSingleNode(oDestinationDocument,
                UserSettingsXPath, null);

        foreach (XmlNode oSourceUserSettingsChildNode in
            oSourceUserSettingsNode.ChildNodes)
        {
            // Work item:
            //
            // Figure out a more robust scheme that doesn't use hard-coded
            // class names.

            String sChildName = oSourceUserSettingsChildNode.Name;

            if (
                sChildName.IndexOf("Dialog") >= 0
                ||
                sChildName.IndexOf("NotificationUserSettings") >= 0
                ||
                sChildName.IndexOf("PasswordUserSettings") >= 0
                )
            {
                continue;
            }

            // This user settings child node needs to be copied from the source
            // to the destination.

            XmlNode oSourceUserSettingsChildNodeClone =
                oDestinationDocument.ImportNode(
                    oSourceUserSettingsChildNode, true);

            // Does the node already exist in the destination?

            XmlNode oDestinationUserSettingsChildNode =
                oDestinationUserSettingsNode.SelectSingleNode(sChildName);

            if (oDestinationUserSettingsChildNode == null)
            {
                // No.  Append the node and its corresponding section child
                // node to the destination.

                oDestinationUserSettingsNode.AppendChild(
                    oSourceUserSettingsChildNodeClone);

                XmlNode oSourceSectionNode =
                    XmlUtil2.SelectRequiredSingleNode(
                        oSourceSectionGroupNode,
                        "section[@name=\"" + sChildName + "\"]",
                        null);

                oDestinationSectionGroupNode.AppendChild(
                    oDestinationDocument.ImportNode(
                        oSourceSectionNode, true) );
            }
            else
            {
                // Yes.  Replace the destination node.

                oDestinationUserSettingsNode.ReplaceChild(
                    oSourceUserSettingsChildNodeClone,
                    oDestinationUserSettingsChildNode);
            }
        }

        // Write the edited copy to a string in such a way that it includes a
        // UTF-8 encoding attribute.

        oSourceDocument = null;
        MemoryStream oMemoryStream = new MemoryStream();
        oDestinationDocument.Save(oMemoryStream);
        oDestinationDocument = null;

        return ( Encoding.UTF8.GetString( oMemoryStream.ToArray() ) );
    }

    //*************************************************************************
    //  Method: CreateStandardSettingsFolder()
    //
    /// <summary>
    /// Creates the standard settings folder and optionally the standard
    /// settings file if they don't exist.
    /// </summary>
    ///
    /// <param name="bAlsoCreateStandardSettingsFile">
    /// true to also create the standard settings file.
    /// </param>
    //*************************************************************************

    protected void
    CreateStandardSettingsFolder
    (
        Boolean bAlsoCreateStandardSettingsFile
    )
    {
        AssertValid();

        String sStandardSettingsFilePath;

        try
        {
            sStandardSettingsFilePath = GetStandardSettingsFilePath();
        }
        catch (IOException oIOException)
        {
            if ( FileIsLocked(oIOException) )
            {
                // Ignore the locked file.  The lock will be handled within 
                // the call to GetStandardSettingsFileStream().

                return;
            }

            throw (oIOException);
        }

        String sStandardSettingsFolderPath = Path.GetDirectoryName(
            sStandardSettingsFilePath);

        if ( !Directory.Exists(sStandardSettingsFolderPath) )
        {
            Directory.CreateDirectory(sStandardSettingsFolderPath);
        }

        if (bAlsoCreateStandardSettingsFile)
        {
            if ( !File.Exists(sStandardSettingsFilePath) )
            {
                WriteStandardSettingsFile(DefaultStandardSettingsFileContents);
            }
        }
    }

    //*************************************************************************
    //  Method: GetStandardSettingsFileStream()
    //
    /// <summary>
    /// Gets a locked FileStream for the standard settings file.
    /// </summary>
    ///
    /// <param name="bForWriting">
    /// true if the FileStream is for writing, false if for reading.
    /// </param>
    //*************************************************************************

    protected FileStream
    GetStandardSettingsFileStream
    (
        Boolean bForWriting
    )
    {
        AssertValid();

        // The following code was adapted from the post "How to check For File
        // Lock in C# ?" at http://stackoverflow.com/questions/1304/
        // how-to-check-for-file-lock-in-c

        Int32 iAttempts = 0;

        while (true)
        {
            try
            {
                iAttempts++;

                return (File.Open(GetStandardSettingsFilePath(),
                    bForWriting ? FileMode.Create : FileMode.OpenOrCreate,
                    bForWriting ? FileAccess.Write : FileAccess.Read,
                    bForWriting ? FileShare.None : FileShare.Read
                    ) );
            }
            catch (IOException oIOException)
            {
                if ( !FileIsLocked(oIOException) )
                {
                    throw (oIOException);
                }

                if (iAttempts > MaximumStandardSettingsFileAttempts)
                {
                    throw new StandardSettingsFileLockedException();
                }

                System.Threading.Thread.Sleep(
                    StandardSettingsFileLockedSleepIntervalMs);
            }
        }
    }

    //*************************************************************************
    //  Method: FileIsLocked()
    //
    /// <summary>
    /// Determines whether an IOException was thrown because a file was locked.
    /// </summary>
    ///
    /// <param name="oIOException">
    /// The IOException that was thrown.
    /// </param>
    //*************************************************************************

    protected Boolean FileIsLocked
    (
        IOException oIOException
    )
    {
        Debug.Assert(oIOException != null);
        AssertValid();

        Int32 iErrorCode =
            Marshal.GetHRForException(oIOException) & ( (1 << 16) - 1 );

        return (iErrorCode == 32 || iErrorCode == 33);
    }


    //*************************************************************************
    //  Method: GetStandardSettingsFilePath()
    //
    /// <summary>
    /// Gets the full path to the standard settings file.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the standard settings file.
    /// </returns>
    //*************************************************************************

    protected static String
    GetStandardSettingsFilePath()
    {
        // Sample file path:
        //
        // C:\Users\UserName\AppData\Local\Microsoft_Corporation\
        // NodeXLGraph.xltx_Path_exlviddqfuofdx2qvofipdzmollu2gfx\
        // 12.0.6504.5001\user.config

        try
        {
            return ( ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
        }
        catch (ConfigurationErrorsException oConfigurationErrorsException)
        {
            // When the standard settings file is locked, a
            // ConfigurationErrorsException is thrown.  It has an inner
            // IOException that indicates the file is locked.

            if (oConfigurationErrorsException.InnerException is IOException)
            {
                throw oConfigurationErrorsException.InnerException;
            }

            throw oConfigurationErrorsException;
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
        // m_bUseWorkbookSettings
        // m_bItemCalled
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Contents of a default settings file.

    public const String DefaultStandardSettingsFileContents =
        "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n"
        + "<configuration>\r\n"
        + "<configSections>\r\n"
        + "<sectionGroup name=\"userSettings\""
        + " type=\"System.Configuration.UserSettingsGroup, System,"
        + " Version=4.0.0.0, Culture=neutral,"
        + " PublicKeyToken=b77a5c561934e089\">"
        + "</sectionGroup>"
        + "</configSections>\r\n"
        + "<userSettings>\r\n"
        + "</userSettings>\r\n"
        + "</configuration>\r\n"
        ;


    //*************************************************************************
    //  Protected constants
    //*************************************************************************

    /// Maximum number of times to try opening the standard settings file.

    protected const Int32 MaximumStandardSettingsFileAttempts = 5;

    /// Number of milliseconds to wait before attempting to open the standard
    /// settings file again.

    protected const Int32 StandardSettingsFileLockedSleepIntervalMs = 1000;


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    /// true to use the workbook settings instead of the standard settings.

    protected Boolean m_bUseWorkbookSettings;

    /// true if the Item[] property getter has been called.

    protected Boolean m_bItemCalled;
}

}
