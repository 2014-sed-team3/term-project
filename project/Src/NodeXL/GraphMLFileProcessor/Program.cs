
using System;
using System.Windows.Forms;

namespace Smrf.NodeXL.GraphMLFileProcessor
{
//*****************************************************************************
//  Class: Program
//
/// <summary>
/// The application's entry point.
/// </summary>
//*****************************************************************************

static class Program
{
    //*************************************************************************
    //  Method: Main()
    //
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    ///
    /// <param name="args">
    /// Command line arguments.
    /// </param>
    //*************************************************************************

    [STAThread]

    static void Main
    (
        string[] args
    )
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run( new MainForm() );
    }
}
}
