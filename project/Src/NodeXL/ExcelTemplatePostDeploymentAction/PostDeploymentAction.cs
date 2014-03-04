
using System;
using Microsoft.VisualStudio.Tools.Applications.Deployment;
using Microsoft.VisualStudio.Tools.Applications;
using System.IO;
using IWshRuntimeLibrary;
using Smrf.NodeXL.Common;

namespace Smrf.NodeXL.ExcelTemplatePostDeploymentAction
{
//*****************************************************************************
//  Class: PostDeploymentAction
//
/// <summary>
/// Performs tasks after the NodeXL Excel Template project has been deployed.
/// </summary>
//*****************************************************************************

public class PostDeploymentAction : IAddInPostDeploymentAction
{
    //*************************************************************************
    //  Method: Execute()
    //
    /// <summary>
    /// Executes the post-deployment action.
    /// </summary>
    ///
    /// <param name="args">
    /// Standard arguments.
    /// </param>
    //*************************************************************************

    public void
    Execute
    (
        AddInPostDeploymentActionArgs args
    )
    {
        switch (args.InstallationStatus)
        {
            case AddInInstallationStatus.InitialInstall:
            case AddInInstallationStatus.Update:

                SetAssemblyLocationInTemplateFile(args);
                CreateStartMenuShortcut(args);

                break;

            case AddInInstallationStatus.Uninstall:

                DeleteStartMenuShortcut(args);

                break;
        }
    }

    //*************************************************************************
    //  Method: SetAssemblyLocationInTemplateFile()
    //
    /// <summary>
    /// Sets the _AssemblyLocation custom property in the NodeXL template file.
    /// </summary>
    ///
    /// <param name="args">
    /// Standard arguments.
    /// </param>
    //*************************************************************************

    private void
    SetAssemblyLocationInTemplateFile
    (
        AddInPostDeploymentActionArgs args
    )
    {
        // Before this method is called, the _AssemblyLocation property in the
        // template file is set to this:
        //
        //  Smrf.NodeXL.ExcelTemplate.vsto|aa51c0f3-62b4-4782-83a8-a15dcdd17698|vstolocal
        //
        // The "vstolocal" is a marker added by Visual Studio to assist during
        // development, but it has to be removed during deployment.  Also, the
        // relative path to the Smrf.NodeXL.ExcelTemplate.vsto deployment
        // manifest needs to be replaced with an absolute path.
        //
        // This method changes the _AssemblyLocation property to something like
        // this:
        //
        //   http://SomeSite/SomePath/Smrf.NodeXL.ExcelTemplate.vsto|aa51c0f3-62b4-4782-83a8-a15dcdd17698
        //
        // It gets the absolute path to the deployment manifest from the
        // AddInPostDeploymentActionArgs.ManifestLocation property.
        //

        String nodeXLTemplatePath = GetNodeXLTemplatePath(args);

        ServerDocument.RemoveCustomization(nodeXLTemplatePath);

        ServerDocument.AddCustomization(
            nodeXLTemplatePath, args.ManifestLocation);
    }

    //*************************************************************************
    //  Method: CreateStartMenuShortcut()
    //
    /// <summary>
    /// Creates a start menu shortcut for the NodeXL Excel Template.
    /// </summary>
    ///
    /// <param name="args">
    /// Standard arguments.
    /// </param>
    //*************************************************************************

    private void
    CreateStartMenuShortcut
    (
        AddInPostDeploymentActionArgs args
    )
    {
        // This was adapted from Rustam Irzaev's reply to this post:
        //
        //   http://stackoverflow.com/questions/4897655/create-shortcut-on-desktop-c-sharp

        WshShell wshShell = new WshShell();

        IWshShortcut shortcut = (IWshShortcut)wshShell.CreateShortcut(
            GetStartMenuShortcutFilePath() );

        shortcut.Description = StartMenuShortcutDescription;
        shortcut.TargetPath = GetNodeXLTemplatePath(args);
        shortcut.Save();
    }

    //*************************************************************************
    //  Method: DeleteStartMenuShortcut()
    //
    /// <summary>
    /// Deletes the start menu shortcut for the NodeXL Excel Template.
    /// </summary>
    ///
    /// <param name="args">
    /// Standard arguments.
    /// </param>
    //*************************************************************************

    private void
    DeleteStartMenuShortcut
    (
        AddInPostDeploymentActionArgs args
    )
    {
        String startMenuShortcutFilePath = GetStartMenuShortcutFilePath();

        if ( System.IO.File.Exists(startMenuShortcutFilePath) )
        {
            try
            {
                System.IO.File.Delete(startMenuShortcutFilePath);
            }
            catch
            {
                // Ignore file deletion errors.
            }
        }
    }

    //*************************************************************************
    //  Method: GetNodeXLTemplatePath()
    //
    /// <summary>
    /// Gets the full path to the NodeXL template file.
    /// </summary>
    ///
    /// <param name="args">
    /// Standard arguments.
    /// </param>
    ///
    /// <returns>
    /// The full path to the NodeXL template file.
    /// </returns>
    //*************************************************************************

    private String
    GetNodeXLTemplatePath
    (
        AddInPostDeploymentActionArgs args
    )
    {
        String sNodeXLTemplatePath = Path.Combine(
            args.AddInPath, ProjectInformation.ExcelTemplateSubfolder);

        sNodeXLTemplatePath = Path.Combine(
            sNodeXLTemplatePath, ProjectInformation.ExcelTemplateName);

        return (sNodeXLTemplatePath);
    }

    //*************************************************************************
    //  Method: GetStartMenuShortcutFilePath()
    //
    /// <summary>
    /// Gets the full path to the shortcut file added to the Windows start
    /// menu.
    /// </summary>
    ///
    /// <returns>
    /// The full path to the shortcut file added to the Windows start menu.
    /// </returns>
    //*************************************************************************

    private String
    GetStartMenuShortcutFilePath()
    {
        return ( Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
            StartMenuShortcutFileName
            ) );
    }


    //*************************************************************************
    //  Private constants
    //*************************************************************************

    /// Name of the shortcut file added to the Windows start menu, without a
    /// path.

    private const String StartMenuShortcutFileName =
        "NodeXL Excel Template.lnk";

    /// Description of the shortcut file added to the Windows start menu.

    private const String StartMenuShortcutDescription =
        "Create an Excel workbook using the NodeXL Excel Template";
}
}
