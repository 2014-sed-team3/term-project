
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.AppLib;

namespace Smrf.NodeXL.Algorithms
{
//*****************************************************************************
//  Class: GraphMetricCalculatorBase
//
/// <summary>
/// Base class for classes that implement <see
/// cref="IGraphMetricCalculator" />.
/// </summary>
//*****************************************************************************

public abstract class GraphMetricCalculatorBase :
    Object, IGraphMetricCalculator
{
    //*************************************************************************
    //  Constructor: GraphMetricCalculatorBase()
    //
    /// <summary>
    /// Static constructor for the <see cref="GraphMetricCalculatorBase" />
    /// class.
    /// </summary>
    //*************************************************************************

    static GraphMetricCalculatorBase()
    {
        m_sSnapGraphMetricCalculatorPath = null;
    }

    //*************************************************************************
    //  Constructor: GraphMetricCalculatorBase()
    //
    /// <summary>
    /// Initializes a new instance of the <see
    /// cref="GraphMetricCalculatorBase" /> class.
    /// </summary>
    //*************************************************************************

    public GraphMetricCalculatorBase()
    {
        // (Do nothing.)

        // AssertValid();
    }

    //*************************************************************************
    //  Property: GraphMetricDescription
    //
    /// <summary>
    /// Gets a description of the graph metrics calculated by the
    /// implementation.
    /// </summary>
    ///
    /// <value>
    /// A description suitable for use within the sentence "Calculating
    /// [GraphMetricDescription]."
    /// </value>
    //*************************************************************************

    public abstract String
    GraphMetricDescription
    {
        get;
    }

    //*************************************************************************
    //  Method: SetSnapGraphMetricCalculatorPath()
    //
    /// <summary>
    /// Sets the path to the executable that calculates graph metrics using
    /// the SNAP library.
    /// </summary>
    ///
    /// <param name="snapGraphMetricCalculatorPath">
    /// Full path to the executable.  Sample:
    /// "C:\MyProgram\SnapGraphMetricCalculator.exe".
    /// </param>
    ///
    /// <remarks>
    /// Some of the derived classes use a separate executable to do their graph
    /// metric calculations.  This executable, which is custom built for
    /// NodeXL, uses the SNAP library created by Jure Leskovec at Stanford.
    /// The SNAP code is written in C++ and is optimized for speed and
    /// scalability, so it can calculate certain graph metrics much faster than
    /// could be done in C# code.
    ///
    /// <para>
    /// By default, the NodeXL build process copies the executable, which is
    /// named SnapGraphMetricCalculator.exe, to the Algorithm project's output
    /// directory, which is either bin\Debug or bin\Release.  Also by default,
    /// derived classes look for this executable in the same folder as the
    /// Algorithm assembly.  That means that for many projects, the executable
    /// will be automatically found and this method does not need to be called.
    /// </para>
    ///
    /// <para>
    /// If your application's deployment places the executable somewhere else,
    /// however, you must call this static method once to provide the derived
    /// classes with the executable's path.
    /// </para>
    ///
    /// </remarks>
    //*************************************************************************

    static public void
    SetSnapGraphMetricCalculatorPath
    (
        String snapGraphMetricCalculatorPath
    )
    {
        m_sSnapGraphMetricCalculatorPath = snapGraphMetricCalculatorPath;
    }

    //*************************************************************************
    //  Method: CalculateProgressInPercent()
    //
    /// <summary>
    /// Calculates progress in percent.
    /// </summary>
    ///
    /// <param name="calculationsCompleted">
    /// Number of calculations that have been performed so far.
    /// </param>
    ///
    /// <param name="totalCalculations">
    /// Total number of calculations.  Can be zero.
    /// </param>
    ///
    /// <returns>
    /// The progress of the calculations, in percent.
    /// </returns>
    //*************************************************************************

    public static Int32
    CalculateProgressInPercent
    (
        Int32 calculationsCompleted,
        Int32 totalCalculations
    )
    {
        Debug.Assert(calculationsCompleted >= 0);
        Debug.Assert(totalCalculations >= 0);
        Debug.Assert(calculationsCompleted <= totalCalculations);

        Int32 iPercentProgress = 0;

        if (totalCalculations > 0)
        {
            iPercentProgress = (Int32) (100F *
                (Single)calculationsCompleted / (Single)totalCalculations);
        }

        return (iPercentProgress);
    }

    //*************************************************************************
    //  Method: ReportProgressAndCheckCancellationPending()
    //
    /// <summary>
    /// Reports progress to the calling thread and checks for cancellation if a
    /// <see cref="BackgroundWorker" /> is in use.
    /// </summary>
    ///
    /// <param name="iCalculationsSoFar">
    /// Number of calculations that have been performed so far.
    /// </param>
    ///
    /// <param name="iTotalCalculations">
    /// Total number of calculations.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// The <see cref="BackgroundWorker" /> object that is performing all graph
    /// metric calculations, or null if a <see cref="BackgroundWorker" /> is
    /// not in use.
    /// </param>
    ///
    /// <returns>
    /// false if the user wants to cancel.
    /// </returns>
    //*************************************************************************

    protected Boolean
    ReportProgressAndCheckCancellationPending
    (
        Int32 iCalculationsSoFar,
        Int32 iTotalCalculations,
        BackgroundWorker oBackgroundWorker
    )
    {
        Debug.Assert(iCalculationsSoFar >= 0);
        Debug.Assert(iTotalCalculations >= 0);
        Debug.Assert(iCalculationsSoFar <= iTotalCalculations);
        AssertValid();

        if (oBackgroundWorker != null)
        {
            if (oBackgroundWorker.CancellationPending)
            {
                return (false);
            }

            ReportProgress(iCalculationsSoFar, iTotalCalculations,
                oBackgroundWorker);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: ReportProgress()
    //
    /// <summary>
    /// Reports progress to the calling thread if a <see
    /// cref="BackgroundWorker" /> is in use.
    /// </summary>
    ///
    /// <param name="iCalculationsSoFar">
    /// Number of calculations that have been performed so far.
    /// </param>
    ///
    /// <param name="iTotalCalculations">
    /// Total number of calculations.
    /// </param>
    ///
    /// <param name="oBackgroundWorker">
    /// The <see cref="BackgroundWorker" /> object that is performing all graph
    /// metric calculations, or null if a <see cref="BackgroundWorker" /> is
    /// not in use.
    /// </param>
    ///
    /// <remarks>
    /// If <paramref name="oBackgroundWorker" /> is null, this method does
    /// nothing.
    /// </remarks>
    //*************************************************************************

    protected void
    ReportProgress
    (
        Int32 iCalculationsSoFar,
        Int32 iTotalCalculations,
        BackgroundWorker oBackgroundWorker
    )
    {
        Debug.Assert(iCalculationsSoFar >= 0);
        Debug.Assert(iTotalCalculations >= 0);
        Debug.Assert(iCalculationsSoFar <= iTotalCalculations);
        AssertValid();

        if (oBackgroundWorker != null)
        {
            String sProgress = String.Format(

                "Calculating {0}."
                ,
                this.GraphMetricDescription
                );

            oBackgroundWorker.ReportProgress(

                CalculateProgressInPercent(iCalculationsSoFar,
                    iTotalCalculations),

                sProgress);
        }
    }

    //*************************************************************************
    //  Method: ReportCannotCalculateGraphMetrics()
    //
    /// <summary>
    /// Throws an exception to the calling thread indicating a condition that
    /// prevents the graph metrics from being calculated.
    /// </summary>
    ///
    /// <param name="sMessage">
    /// Error message, suitable for displaying to the user.
    /// </param>
    ///
    /// <remarks>
    /// This method throws a <see cref="GraphMetricException" />.
    /// </remarks>
    //*************************************************************************

    protected void
    ReportCannotCalculateGraphMetrics
    (
        String sMessage
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sMessage) );
        AssertValid();

        throw new GraphMetricException(
            sMessage + "  No graph metrics have been calculated.");
    }

    //*************************************************************************
    //  Method: CalculateEdgesInFullyConnectedNeighborhood()
    //
    /// <summary>
    /// Calculates the number of edges there would be in the neighborhood of a
    /// vertex if the neighborhood were fully connected.
    /// </summary>
    ///
    /// <param name="iAdjacentVertices">
    /// The number of the vertex's adjacent vertices.
    /// </param>
    ///
    /// <param name="bGraphIsDirected">
    /// true if the graph is directed, false if it's undirected.
    /// </param>
    ///
    /// <returns>
    /// The number of edges in the fully connected neighborhood.
    /// </returns>
    //*************************************************************************

    protected Int32
    CalculateEdgesInFullyConnectedNeighborhood
    (
        Int32 iAdjacentVertices,
        Boolean bGraphIsDirected
    )
    {
        Debug.Assert(iAdjacentVertices >= 0);
        AssertValid();

        return ( ( iAdjacentVertices * (iAdjacentVertices - 1) ) /
            (bGraphIsDirected ? 1: 2) );
    }

    //*************************************************************************
    //  Enum: SnapGraphMetrics
    //
    /// <summary>
    /// Specifies one or more graph metrics that can be calculated by the SNAP
    /// library.
    /// </summary>
    ///
    /// <remarks>
    /// See the SnapGraphMetrics enumeration in the Snap/GraphMetricCalculator
    /// project for details on how graph metrics must be specified.
    /// </remarks>
    //*************************************************************************

    [System.FlagsAttribute]

    protected enum SnapGraphMetrics
    {
        /// <summary>
        /// Closeness centrality.  Type: Per-vertex.
        /// </summary>

        ClosenessCentrality = 1,

        /// <summary>
        /// Betweenness centrality.  Type: Per-vertex.
        /// </summary>

        BetweennessCentrality = 2,

        /// <summary>
        /// Eigenvector centrality.  Type: Per-vertex.
        /// </summary>

        EigenvectorCentrality = 4,

        /// <summary>
        /// PageRank.  Type: Per-vertex.
        /// </summary>

        PageRank = 8,

        /// <summary>
        /// Maximum geodesic distance, also known as graph diameter, and
        /// average geodesic distance.  Type: Per-graph.
        /// </summary>

        GeodesicDistances = 16,

        /// <summary>
        /// Clusters using the Girvan-Newman algorithm.  Type: Clusters.
        /// </summary>

        GirvanNewmanClusters = 32,

        /// <summary>
        /// Clusters using the Clauset-Newman-Moore algorithm.  Type: Clusters.
        /// </summary>
    
        ClausetNewmanMooreClusters = 64,

        /// <summary>
        /// Modularity.  Can be calculated only when the graph has groups.
        /// Type: Per-graph.
        /// </summary>

        Modularity = 128,

        /// <summary>
        /// Cliques.  Type: Clusters.
        /// </summary>
    
        Cliques = 256,

        /// <summary>
        /// No graph metrics.
        /// </summary>

        None = 0,
    };

    //*************************************************************************
    //  Method: CalculateSnapGraphMetrics()
    //
    /// <overloads>
    /// Calculates one or more graph metrics using the SNAP executable.
    /// </overloads>
    ///
    /// <summary>
    /// Calculates one or more graph metrics that don't involve groups using
    /// the SNAP executable.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="eSnapGraphMetrics">
    /// One or more graph metrics to calculate.
    /// </param>
    ///
    /// <returns>
    /// Full path to a temporary output file containing the calculated metrics.
    /// The caller must delete this file after it is done with it.
    /// </returns>
    ///
    /// <remarks>
    /// If an error occurs while calling the executable, an IOException is
    /// thrown.
    /// </remarks>
    //*************************************************************************

    protected String
    CalculateSnapGraphMetrics
    (
        IGraph oGraph,
        SnapGraphMetrics eSnapGraphMetrics
    )
    {
        AssertValid();

        return ( CalculateSnapGraphMetrics(oGraph, eSnapGraphMetrics, null) );
    }

    //*************************************************************************
    //  Method: CalculateSnapGraphMetrics()
    //
    /// <summary>
    /// Calculates one or more graph metrics (possibly including those that
    /// involve groups) using the SNAP executable.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="eSnapGraphMetrics">
    /// One or more graph metrics to calculate.
    /// </param>
    ///
    /// <param name="oGroups">
    /// List of the graph's groups, or null if none of the graph metrics
    /// involve groups.  If not null, the list must contain at least one group
    /// and all vertices in all the groups must have at least one incident
    /// edge.
    /// </param>
    ///
    /// <returns>
    /// Full path to a temporary output file containing the calculated metrics.
    /// The caller must delete this file after it is done with it.
    /// </returns>
    ///
    /// <remarks>
    /// If an error occurs while calling the executable, an IOException is
    /// thrown.
    /// </remarks>
    //*************************************************************************

    protected String
    CalculateSnapGraphMetrics
    (
        IGraph oGraph,
        SnapGraphMetrics eSnapGraphMetrics,
        List<GroupInfo> oGroups
    )
    {
        AssertValid();

        String sInputFilePath = null;
        String sGroupInputFilePath = null;
        String sOutputFilePath = null;

        try
        {
            // Create the input file(s) that SNAP expects.
            //
            // Do not use Path.GetTempFileName() here.  That was used at one
            // time, but it threw random "[UnauthorizedAccessException]: Access
            // to the path is denied" exceptions when graph metrics were
            // calculated in simultaneous instances of the NodeXLNetworkServer
            // program.  This might have been related to the known Windows bug
            // "The GetTempFileName function fails together with an access
            // denied error in Windows 7 or in Windows Server 2008 R2" at
            // http://support.microsoft.com/kb/982613.

            sInputFilePath = FileUtil.GetPathToUseForTemporaryFile();
            CreateSnapInputFile(oGraph, sInputFilePath);

            if (oGroups != null)
            {
                // The SNAP executable expects a group input text file that has
                // the same name as the input file but with "groups" appended
                // to the name.

                sGroupInputFilePath = sInputFilePath + "groups";
                CreateSnapGroupInputFile(oGroups, sGroupInputFilePath);
            }

            // SNAP will overwrite this output file, which initially has zero
            // length:

            sOutputFilePath = FileUtil.GetPathToUseForTemporaryFile();

            // The arguments should look like this:
            //
            // InputFilePath IsDirected GraphMetricsToCalculate OutputFilePath

            String sArguments = String.Format(
                "\"{0}\" {1} {2} \"{3}\""
                ,
                sInputFilePath,

                oGraph.Directedness == GraphDirectedness.Directed ?
                    "true" : "false",

                (Int32)eSnapGraphMetrics,
                sOutputFilePath
                );

            String sStandardError;

            if ( !TryCallSnapGraphMetricCalculator(eSnapGraphMetrics,
                sArguments, out sStandardError) )
            {
                throw new IOException(
                    "A problem occurred while calling the executable that"
                    + " calculates SNAP graph metrics.  Details: "
                    + sStandardError
                    );
            }

            Debug.Assert( File.Exists(sOutputFilePath) );
        }
        catch
        {
            // Delete the output file on error.

            DeleteSnapFile(sOutputFilePath);
            throw;
        }
        finally
        {
            // Always delete the input file(s).

            DeleteSnapFile(sInputFilePath);
            DeleteSnapFile(sGroupInputFilePath);
        }

        return (sOutputFilePath);
    }

    //*************************************************************************
    //  Method: CreateSnapInputFile()
    //
    /// <summary>
    /// Creates the SNAP input text file that contains the graph's edges.
    /// </summary>
    ///
    /// <param name="oGraph">
    /// The graph to calculate metrics for.
    /// </param>
    ///
    /// <param name="sSnapInputFilePath">
    /// Full path to the temporary file to create.
    /// </param>
    //*************************************************************************

    protected void
    CreateSnapInputFile
    (
        IGraph oGraph,
        String sSnapInputFilePath
    )
    {
        Debug.Assert(oGraph != null);
        Debug.Assert( !String.IsNullOrEmpty(sSnapInputFilePath) );
        AssertValid();

        // The SNAP executable expects an input text file that contains one
        // line per edge.  The line contains a tab-separated pair of integer
        // vertex IDs.  Create the file.

        using ( StreamWriter oStreamWriter =
            new StreamWriter(sSnapInputFilePath) ) 
        {
            foreach (IEdge oEdge in oGraph.Edges)
            {
                IVertex [] oVertices = oEdge.Vertices;

                oStreamWriter.WriteLine(
                    "{0}\t{1}"
                    ,
                    oVertices[0].ID,
                    oVertices[1].ID
                    );
            }
        }
    }

    //*************************************************************************
    //  Method: CreateSnapGroupInputFile()
    //
    /// <summary>
    /// Creates the SNAP input text file that contains the graph's groups.
    /// </summary>
    ///
    /// <param name="oGroups">
    /// List of the graph's groups.  The list must contain at least one group
    /// and all vertices in all the groups must have at least one incident
    /// edge.
    /// </param>
    ///
    /// <param name="sSnapGroupInputFilePath">
    /// Full path to the temporary file to create.
    /// </param>
    //*************************************************************************

    protected void
    CreateSnapGroupInputFile
    (
        List<GroupInfo> oGroups,
        String sSnapGroupInputFilePath
    )
    {
        Debug.Assert(oGroups != null);
        Debug.Assert( !String.IsNullOrEmpty(sSnapGroupInputFilePath) );
        AssertValid();

        // The SNAP executable expects a group input text file that contains
        // one line per group.  The line contains the tab-separated integer
        // IDs of the group's vertices.  Create the file.

        using ( StreamWriter oStreamWriter =
            new StreamWriter(sSnapGroupInputFilePath) ) 
        {
            foreach (GroupInfo oGroup in oGroups)
            {
                Debug.Assert(oGroup.Vertices.Count > 0);

                Boolean bNeedTab = false;

                foreach (IVertex oVertex in oGroup.Vertices)
                {
                    Debug.Assert(oVertex.Degree > 0);

                    if (bNeedTab)
                    {
                        oStreamWriter.Write('\t');
                    }

                    oStreamWriter.Write(oVertex.ID);
                    bNeedTab = true;
                }

                oStreamWriter.WriteLine();
            }
        }
    }

    //*************************************************************************
    //  Method: DeleteSnapFile()
    //
    /// <summary>
    /// Deletes a file created while calculating SNAP graph metrics, if the
    /// file exists.
    /// </summary>
    ///
    /// <param name="sSnapFilePath">
    /// Full path to the file to delete.  Can be null, and the file doesn't
    /// have to exist.
    /// </param>
    //*************************************************************************

    protected void
    DeleteSnapFile
    (
        String sSnapFilePath
    )
    {
        AssertValid();

        if ( sSnapFilePath != null && File.Exists(sSnapFilePath) )
        {
            File.Delete(sSnapFilePath);
        }
    }

    //*************************************************************************
    //  Method: ParseSnapInt32GraphMetricValue()
    //
    /// <summary>
    /// Parses an Int32 graph metric value read from the output file created by
    /// the SNAP command-line executable.
    /// </summary>
    ///
    /// <param name="asFieldsFromSnapOutputFileLine">
    /// Fields read from one line of the output file created by the SNAP
    /// command-line executable.
    /// </param>
    ///
    /// <param name="iFieldIndex">
    /// The zero-based index of the field to parse.
    /// </param>
    ///
    /// <returns>
    /// The parsed Int32 graph metric value.
    /// </returns>
    //*************************************************************************

    protected Int32
    ParseSnapInt32GraphMetricValue
    (
        String [] asFieldsFromSnapOutputFileLine,
        Int32 iFieldIndex
    )
    {
        Debug.Assert(asFieldsFromSnapOutputFileLine != null);
        Debug.Assert(iFieldIndex >= 0);
        Debug.Assert(iFieldIndex < asFieldsFromSnapOutputFileLine.Length);
        AssertValid();

        Int32 iGraphMetricValue;

        if ( !MathUtil.TryParseCultureInvariantInt32(
            asFieldsFromSnapOutputFileLine[iFieldIndex], out iGraphMetricValue)
            )
        {
            throw new FormatException(
                "A field read from the SNAP output file is not an Int32."
                );
        }

        return (iGraphMetricValue);
    }

    //*************************************************************************
    //  Method: ParseSnapDoubleGraphMetricValue()
    //
    /// <summary>
    /// Parses a Double graph metric value read from the output file created by
    /// the SNAP command-line executable.
    /// </summary>
    ///
    /// <param name="asFieldsFromSnapOutputFileLine">
    /// Fields read from one line of the output file created by the SNAP
    /// command-line executable.
    /// </param>
    ///
    /// <param name="iFieldIndex">
    /// The zero-based index of the field to parse.
    /// </param>
    ///
    /// <returns>
    /// The parsed Double graph metric value.
    /// </returns>
    //*************************************************************************

    protected Double
    ParseSnapDoubleGraphMetricValue
    (
        String [] asFieldsFromSnapOutputFileLine,
        Int32 iFieldIndex
    )
    {
        Debug.Assert(asFieldsFromSnapOutputFileLine != null);
        Debug.Assert(iFieldIndex >= 0);
        Debug.Assert(iFieldIndex < asFieldsFromSnapOutputFileLine.Length);
        AssertValid();

        Double dGraphMetricValue;

        if ( !MathUtil.TryParseCultureInvariantDouble(
            asFieldsFromSnapOutputFileLine[iFieldIndex], out dGraphMetricValue)
            )
        {
            throw new FormatException(
                "A field read from the SNAP output file is not a Double."
                );
        }

        return (dGraphMetricValue);
    }

    //*************************************************************************
    //  Method: TryCallSnapGraphMetricCalculator()
    //
    /// <summary>
    /// Calls the SNAP command-line executable.
    /// </summary>
    ///
    /// <param name="eSnapGraphMetrics">
    /// One or more graph metrics to calculate.
    /// </param>
    ///
    /// <param name="sArguments">
    /// Command line arguments.
    /// </param>
    ///
    /// <param name="sStandardError">
    /// Where the standard error string gets stored if false is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the executable was successfully called, false if the executable
    /// reported an error via the StandardError stream.
    /// </returns>
    //*************************************************************************

    protected Boolean
    TryCallSnapGraphMetricCalculator
    (
        SnapGraphMetrics eSnapGraphMetrics,
        String sArguments,
        out String sStandardError
    )
    {
        ProcessStartInfo oProcessStartInfo = new ProcessStartInfo(
            GetSnapGraphMetricCalculatorPath(eSnapGraphMetrics), sArguments);

        oProcessStartInfo.UseShellExecute = false;
        oProcessStartInfo.RedirectStandardError = true;
        oProcessStartInfo.CreateNoWindow = true;

        Process oProcess = new Process();
        oProcess.StartInfo = oProcessStartInfo;
        oProcess.Start();
        sStandardError = oProcess.StandardError.ReadToEnd();
        oProcess.WaitForExit();

        return (sStandardError.Length == 0);
    }

    //*************************************************************************
    //  Method: GetSnapGraphMetricCalculatorPath()
    //
    /// <summary>
    /// Gets the path to the executable that calculates graph metrics using
    /// the SNAP library.
    /// </summary>
    ///
    /// <param name="eSnapGraphMetrics">
    /// One or more graph metrics to calculate.
    /// </param>
    ///
    /// <returns>
    /// The full path to the executable.
    /// </returns>
    ///
    /// <exception cref="IOException">
    /// Thrown if the executable doesn't exist.
    /// </exception>
    //*************************************************************************

    protected String
    GetSnapGraphMetricCalculatorPath
    (
        SnapGraphMetrics eSnapGraphMetrics
    )
    {
        AssertValid();

        String sSnapGraphMetricCalculatorPath =
            m_sSnapGraphMetricCalculatorPath;

        if (sSnapGraphMetricCalculatorPath == null)
        {
            // SetSnapGraphMetricCalculatorPath() hasn't been called.  Use a
            // default path.

            sSnapGraphMetricCalculatorPath = Path.Combine(

                Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location),

                DefaultSnapGraphMetricCalculatorFileName
                );
        }

        if (eSnapGraphMetrics == SnapGraphMetrics.Cliques)
        {
            // This is an ugly, temporary kludge.
            //
            // An executable based on the Snap-09-12-28.zip release is used for
            // all graph metrics except cliques.  It is stable and reliable and
            // has been used by NodeXL for years.
            //
            // That release did not calculate cliques, however.  A new
            // executable is based on the Snap-11-12-31.zip release, which does
            // calculate cliques.  However, the newer executable crashes in a
            // simple repro case when ClausetNewmanMooreClusters are
            // calculated.
            //
            // While this problem is worked out, two SNAP executables ship with
            // NodeXL.

            sSnapGraphMetricCalculatorPath =
                sSnapGraphMetricCalculatorPath.Replace(
                    DefaultSnapGraphMetricCalculatorFileName,
                    "SnapGraphMetricCalculatorForCliquesOnly.exe"
                    );
        }

        if ( !File.Exists(sSnapGraphMetricCalculatorPath) )
        {
            throw new IOException(
                "The executable that calculates SNAP graph metrics can't be"
                + " found.  See the comments for the GraphMetricCalculatorBase"
                + ".SetSnapGraphMetricCalculatorPath() method."
                );
        }

        return (sSnapGraphMetricCalculatorPath);
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
        // (Do nothing.)
    }


    //*************************************************************************
    //  Public constants
    //*************************************************************************

    /// Message to show when a graph metric can't be calculated.

    public const String NotApplicableMessage = "Not Applicable";


    /// Default file name of the executable that computes graph metrics using
    /// the SNAP library.  Not used if m_sSnapGraphMetricCalculatorPath is set.

    public const String DefaultSnapGraphMetricCalculatorFileName =
        "SnapGraphMetricCalculator.exe";


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    // (None.)


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    /// Path to the executable that computes graph metrics using the SNAP
    /// library, or null to use a default path.

    private static String m_sSnapGraphMetricCalculatorPath;
}

}
