
using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using Smrf.NodeXL.ApplicationUtil;
using Smrf.NodeXL.Common;
using Smrf.AppLib;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: ApplicationUtil
//
/// <summary>
/// Contains utility methods dealing with the application as a whole.
/// </summary>
//*****************************************************************************

public static class ApplicationUtil
{
    //*************************************************************************
    //  Method: OpenHelp()
    //
    /// <summary>
    /// Opens the application's help file.
    /// </summary>
    //*************************************************************************

    public static void
    OpenHelp()
    {
        if (m_oHelpProcess != null)
        {
            // The help window is already open.  Activate it.

            Win32Functions.SetForegroundWindow(
                m_oHelpProcess.MainWindowHandle);

            return;
        }

        // It's assumed that the build process placed a copy of the help file
        // in the application folder.

        m_oHelpProcess = Process.Start( Path.Combine(
            GetApplicationFolder(), HelpFileName) );

        m_oHelpProcess.EnableRaisingEvents = true;

        m_oHelpProcess.Exited += (Object, EventArgs) =>
        {
            m_oHelpProcess.Dispose();
            m_oHelpProcess = null;
        };
    }

    //*************************************************************************
    //  Method: CreateNodeXLWorkbook()
    //
    /// <summary>
    /// Creates a new NodeXL workbook.
    /// </summary>
    ///
    /// <param name="excelApplication">
    /// The Excel application.
    /// </param>
    ///
    /// <returns>
    /// The new NodeXL workbook.
    /// </returns>
    ///
    /// <exception cref="IOException">
    /// Occurs when the NodeXL template can't be found.
    /// </exception>
    //*************************************************************************

    public static Workbook
    CreateNodeXLWorkbook
    (
        Microsoft.Office.Interop.Excel.Application excelApplication
    )
    {
        String sNodeXLTemplatePath;

        if ( !TryGetTemplatePath(out sNodeXLTemplatePath) )
        {
            throw new IOException( GetMissingTemplateMessage() );
        }

        return ( excelApplication.Workbooks.Add(sNodeXLTemplatePath) );
    }

    //*************************************************************************
    //  Method: OpenSampleNodeXLWorkbook()
    //
    /// <summary>
    /// Opens a sample NodeXL workbook.
    /// </summary>
    //*************************************************************************

    public static void
    OpenSampleNodeXLWorkbook()
    {
        // To create the sample workbook, an empty NodeXL workbook is created
        // from the NodeXL template, and then a GraphML file containing the
        // sample data is imported into it.  It would be simpler to just
        // distribute a complete sample workbook with NodeXL, but that workbook
        // would have to be updated every time the NodeXL template changes.
        // This way, the latest template is always used.

        String sSampleNodeXLWorkbookSubfolderPath = Path.Combine(
            GetApplicationFolder(), SampleNodeXLWorkbookSubfolder);

        XmlDocument oGraphMLDocument = new XmlDocument();

        try
        {
            oGraphMLDocument.Load( Path.Combine(
                sSampleNodeXLWorkbookSubfolderPath,
                SampleNodeXLWorkbookAsGraphMLFileName) );
        }
        catch (Exception oException)
        {
            ErrorUtil.OnException(oException);
            return;
        }

        // The sample workbook data contains placeholders for the path to the
        // image files used in the workbook.  Replace those placeholders with a
        // real image path.

        String sImagePath = Path.Combine(sSampleNodeXLWorkbookSubfolderPath,
            SampleNodeXLWorkbookImageSubfolder);

        oGraphMLDocument.LoadXml( oGraphMLDocument.OuterXml.Replace(
            SampleNodeXLWorkbookImagePlaceholder, sImagePath) );

        try
        {
            GraphMLToNodeXLWorkbookConverter.SaveGraphToNodeXLWorkbook(
                oGraphMLDocument, null, null, null, false);
        }
        catch (ConvertGraphMLToNodeXLWorkbookException
            oConvertGraphMLToNodeXLWorkbookException)
        {
            FormUtil.ShowWarning(
                oConvertGraphMLToNodeXLWorkbookException.Message);
        }
    }

    //*************************************************************************
    //  Method: GetApplicationFolder()
    //
    /// <summary>
    /// Gets the full path to the application's folder.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the application's folder.  Sample:
    /// "...\Some ClickOnce Folder".
    /// </returns>
    //*************************************************************************

    public static String
    GetApplicationFolder()
    {
        // This works whether the application is running within ClickOnce, in
        // which case it returns something like this:
        //
        //   "...\Some ClickOnce Folder".
        //
        // ...or in the development environment, in which case it returns
        // something like this:
        //
        //   "C:\NodeXL\ExcelTemplate\bin\Debug"
        //

        return ( Path.GetDirectoryName( GetExecutingAssemblyPath() ) );
    }

    //*************************************************************************
    //  Method: GetSplashScreenPath()
    //
    /// <summary>
    /// Gets the full path to the HTML file for the application's splash
    /// screen.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the HTML file.  Sample:
    /// "C:\Program Files\...\NodeXL Excel Template\SplashScreen\
    /// SplashScreen.htm".
    /// </returns>
    //*************************************************************************

    public static String
    GetSplashScreenPath()
    {
        String sSplashScreenSubfolderPath = Path.Combine(
            GetApplicationFolder(), SplashScreenSubfolder);

        return ( Path.Combine(sSplashScreenSubfolderPath,
            SplashScreenFileName) );
    }

    //*************************************************************************
    //  Method: OnWorkbookStartup()
    //
    /// <summary>
    /// Performs tasks required when the workbook starts up.
    /// </summary>
    ///
    /// <param name="application">
    /// The Excel application object.
    /// </param>
    //*************************************************************************

    public static void
    OnWorkbookStartup
    (
        Microsoft.Office.Interop.Excel.Application application
    )
    {
        Debug.Assert(application != null);

        AutoCorrect oAutoCorrect = application.AutoCorrect;

        if (!oAutoCorrect.AutoExpandListRange)
        {
            const String CheckboxPath =
                "Office Button, Excel Options, Proofing, AutoCorrect Options,"
                + " AutoFormat As You Type, Include new rows and columns in"
                + " table"
                ;

            String Message =
                "Excel's \"auto table expansion\" feature is turned off, and"
                + " NodeXL cannot work properly without it.  Do you want"
                + " NodeXL to turn it on for you?"
                + "\n\n"
                + " (You can turn it off later if necessary using "
                + CheckboxPath
                + ".)"
                ;

            if (MessageBox.Show(Message, FormUtil.ApplicationName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1) ==
                DialogResult.Yes)
            {
                oAutoCorrect.AutoExpandListRange = true;
            }
            else
            {
                FormUtil.ShowError(
                    "NodeXL will not work properly.  You should turn on"
                    + " Excel's auto table expansion feature using "
                    + CheckboxPath
                    + "."
                    );
            }
        }
    }

    //*************************************************************************
    //  Method: OnWorkbookShutdown()
    //
    /// <summary>
    /// Performs tasks required when the workbook closes.
    /// </summary>
    //*************************************************************************

    public static void
    OnWorkbookShutdown()
    {
        if (m_oHelpProcess != null)
        {
            m_oHelpProcess.CloseMainWindow();
        }
    }

    //*************************************************************************
    //  Method: TryGetTemplatePath()
    //
    /// <summary>
    /// Attempts to get the full path to the application's template file.
    /// </summary>
    ///
    /// <param name="templatePath">
    /// Where the path to the template file gets stored regardless of the
    /// return value.
    /// </param>
    ///
    /// <remarks>
    /// true if the template file exists.
    /// </remarks>
    //*************************************************************************

    public static Boolean
    TryGetTemplatePath
    (
        out String templatePath
    )
    {
        String sTemplateFolderPath;

        if (RunningInDevelopmentEnvironment)
        {
            // Samples, depending on which program is being run:
            //
            //   1. "C:\NodeXL\ExcelTemplate\bin\Debug"
            //
            //   2. "C:\NodeXL\NetworkServer\bin\Debug"
            //
            //   3. "C:\NodeXL\GraphMLFileProcessor\bin\Debug"

            sTemplateFolderPath = Path.GetDirectoryName(
                GetExecutingAssemblyPath() );

            // The template in the development environment is under the
            // ExcelTemplate folder.  For cases 2 and 3, fix the folder.

            const String ExcelTemplateFolderName = "ExcelTemplate";

            sTemplateFolderPath = sTemplateFolderPath.Replace(
                "NetworkServer", ExcelTemplateFolderName);

            sTemplateFolderPath = sTemplateFolderPath.Replace(
                "GraphMLFileProcessor", ExcelTemplateFolderName);
        }
        else
        {
            // The deployment process puts the template file in a subfolder of
            // the deployment folder.

            sTemplateFolderPath = Path.Combine(
                GetApplicationFolder(),
                ProjectInformation.ExcelTemplateSubfolder
                );
        }

        templatePath = Path.Combine(
            sTemplateFolderPath, ProjectInformation.ExcelTemplateName);

        return ( File.Exists(templatePath) );
    }

    //*************************************************************************
    //  Method: GetMissingTemplateMessage()
    //
    /// <summary>
    /// Gets a user-friendly message to display when the application's template
    /// file can't be found.
    /// </summary>
    ///
    /// <returns>
    /// A user-friendly message.
    /// </returns>
    //*************************************************************************

    public static String
    GetMissingTemplateMessage()
    {
        String sTemplatePath;

        ApplicationUtil.TryGetTemplatePath(out sTemplatePath);

        return ( String.Format(

            "The {0} Excel template couldn't be found."
            + "\r\n\r\n"
            + "The {0} setup program should have copied the template to"
            + " {1}.  If you moved the template somewhere else, you won't"
            + " be able to use this feature."
            ,
            ApplicationUtil.ApplicationName,
            sTemplatePath
            ) );
    }

    //*************************************************************************
    //  Property: RunningInDevelopmentEnvironment
    //
    /// <summary>
    /// Gets a flag indicating whether the application is running in a
    /// development environment.
    /// </summary>
    ///
    /// <value>
    /// true if the application is running in a development environment, false
    /// if it is running in an installed environment.
    /// </value>
    //*************************************************************************

    private static Boolean
    RunningInDevelopmentEnvironment
    {
        get
        {
            String sExecutingAssemblyPath =
                GetExecutingAssemblyPath().ToLower();

            return (
                sExecutingAssemblyPath.IndexOf(@"bin\debug") >= 0 ||
                sExecutingAssemblyPath.IndexOf(@"bin\release") >= 0
                );
        }
    }

    //*************************************************************************
    //  Method: GetExecutingAssemblyPath()
    //
    /// <summary>
    /// Gets the full path to the executing assembly.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the executing assembly.  Sample:
    /// "...\Some ClickOnce Folder\Smrf.NodeXL.ExcelTemplate.dll".
    /// </returns>
    //*************************************************************************

    private static String
    GetExecutingAssemblyPath()
    {
        // CodeBase returns an URI, such as "file://folder/subfolder/etc".
        // Convert it to a local path.

        Uri oUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);

        return (oUri.LocalPath);
    }


    //*************************************************************************
    //  Private members
    //*************************************************************************

    /// Process for the application's help file, or null if the help file isn't
    /// open.

    private static Process m_oHelpProcess = null;


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Subfolder under the application folder where the splash screen is
    /// stored.

    private const String SplashScreenSubfolder =
        "SplashScreen";

    /// File name of the splash screen's HTML file.

    private const String SplashScreenFileName = "SplashScreen.htm";

    /// File name of the application's help file.

    private const String HelpFileName = "NodeXLExcelTemplate.chm";

    /// Subfolder under the application folder where the files uses in the
    /// sample NodeXLWorkbook are stored.

    private const String SampleNodeXLWorkbookSubfolder =
        "SampleNodeXLWorkbook";

    /// File name of the GraphML file used to create the sample NodeXLWorkbook.

    private const String SampleNodeXLWorkbookAsGraphMLFileName =
        "SampleNodeXLWorkbookAsGraphML.graphml";

    /// Subfolder under SampleNodeXLWorkbookSubfolder where the images used in
    /// the sample NodeXLWorkbook are stored.

    private const String SampleNodeXLWorkbookImageSubfolder =
        "Images";

    /// Placeholder in the GraphML file for the path to the images used in the
    /// sample NodeXL workbook.

    private const String SampleNodeXLWorkbookImagePlaceholder = "[ImagePath]";


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Application name.

    public const String ApplicationName = "NodeXL";

    /// Solution ID, as a GUID string.

    public const String SolutionID = "aa51c0f3-62b4-4782-83a8-a15dcdd17698";
}

}
