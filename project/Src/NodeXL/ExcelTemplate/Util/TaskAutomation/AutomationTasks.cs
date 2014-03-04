
using System;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Enum: AutomationTasks
//
/// <summary>
/// Specifies one or more tasks to automate.
/// </summary>
///
/// <remarks>
/// The flags can be ORed together.
/// </remarks>
//*****************************************************************************

[System.FlagsAttribute]

public enum
AutomationTasks
{
    /// <summary>
    /// No tasks.
    /// </summary>

    None = 0,

    /// <summary>
    /// Merge duplicate edges.
    /// </summary>

    MergeDuplicateEdges = 1,

    /// <summary>
    /// Calculate graph metrics.
    /// </summary>

    CalculateGraphMetrics = 2,

    /// <summary>
    /// Autofill the workbook.
    /// </summary>

    AutoFillWorkbook = 4,

    /// <summary>
    /// Create subgraph images.
    /// </summary>

    CreateSubgraphImages = 8,

    /// <summary>
    /// Calculate clusters.
    /// </summary>

    CalculateClusters = 16,

    /// <summary>
    /// Read the workbook.
    /// </summary>

    ReadWorkbook = 32,

    /// <summary>
    /// If the workbook has never been saved, save it.
    /// </summary>

    SaveWorkbookIfNeverSaved = 64,

    /// <summary>
    /// Save an image of the graph to a file.
    /// </summary>

    SaveGraphImageFile = 128,

    /// <summary>
    /// Export the graph to the NodeXL Graph Gallery.
    /// </summary>

    ExportToNodeXLGraphGallery = 256,

    /// <summary>
    /// Export the graph to email.
    /// </summary>

    ExportToEmail = 512,
}

}
