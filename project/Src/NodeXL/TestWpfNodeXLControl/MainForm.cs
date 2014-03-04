﻿
using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Adapters;
using Smrf.NodeXL.Visualization.Wpf;
using Smrf.WpfGraphicsLib;

namespace TestWpfNodeXLControl
{
public partial class MainForm : Form
{
    protected Smrf.NodeXL.Visualization.Wpf.NodeXLWithAxesControl
        m_oNodeXLWithAxesControl;

    protected Smrf.NodeXL.Visualization.Wpf.NodeXLControl
        m_oNodeXLControl;

    public MainForm()
    {
        InitializeComponent();

        CreateNodeXLControl();

        usrGraphZoomAndScale.NodeXLControl = m_oNodeXLControl;

        chkShowVertexToolTips.Checked = m_oNodeXLControl.ShowVertexToolTips;
        chkShowAxes.Checked = m_oNodeXLWithAxesControl.ShowAxes;

        cbxMouseMode.PopulateWithEnumValues(
            typeof(Smrf.NodeXL.Visualization.Wpf.MouseMode), false);

        cbxMouseMode.SelectedValue =
            Smrf.NodeXL.Visualization.Wpf.MouseMode.Select;

        #if false
        m_oNodeXLControl.Layout =
            new Smrf.NodeXL.Visualization.CircleLayout();
        #endif

        PopulateGraph();
    }

    protected void
    CreateNodeXLControl()
    {
        m_oNodeXLWithAxesControl = new NodeXLWithAxesControl();
        m_oNodeXLWithAxesControl.XAxis.Label = "This is the x-axis";
        m_oNodeXLWithAxesControl.YAxis.Label = "This is the y-axis";
        m_oNodeXLControl = m_oNodeXLWithAxesControl.NodeXLControl;

        m_oNodeXLControl.SelectionChanged +=
            new System.EventHandler(this.m_oNodeXLControl_SelectionChanged);

        m_oNodeXLControl.VertexClick +=
            new Smrf.NodeXL.Core.VertexEventHandler(
            this.m_oNodeXLControl_VertexClick);

        m_oNodeXLControl.VertexDoubleClick +=
            new Smrf.NodeXL.Core.VertexEventHandler(
            this.m_oNodeXLControl_VertexDoubleClick);

        m_oNodeXLControl.VertexMouseHover +=
            new Smrf.NodeXL.Core.VertexEventHandler(
            this.m_oNodeXLControl_VertexMouseHover);

        m_oNodeXLControl.VertexMouseLeave +=
            new EventHandler(this.m_oNodeXLControl_VertexMouseLeave);

        m_oNodeXLControl.VerticesMoved +=
            new Smrf.NodeXL.Visualization.Wpf.VerticesMovedEventHandler(
            this.m_oNodeXLControl_VerticesMoved);

        m_oNodeXLControl.PreviewVertexToolTipShown +=
            new VertexToolTipShownEventHandler(
                this.m_oNodeXLControl_PreviewVertexToolTipShown);

        m_oNodeXLControl.GraphMouseDown +=
            new Smrf.NodeXL.Visualization.Wpf.GraphMouseButtonEventHandler(
            this.m_oNodeXLControl_GraphMouseDown);

        m_oNodeXLControl.GraphMouseUp +=
            new Smrf.NodeXL.Visualization.Wpf.GraphMouseButtonEventHandler(
            this.m_oNodeXLControl_GraphMouseUp);

        m_oNodeXLControl.GraphZoomChanged +=
            new System.EventHandler(this.m_oNodeXLControl_GraphZoomChanged);

        m_oNodeXLControl.LayingOutGraph +=
            new System.EventHandler(this.m_oNodeXLControl_LayingOutGraph);

        m_oNodeXLControl.GraphLaidOut +=
            new AsyncCompletedEventHandler(this.m_oNodeXLControl_GraphLaidOut);

        ehElementHost.Child = m_oNodeXLWithAxesControl;
    }

    protected void
    PopulateGraphFromFile()
    {
        IGraphAdapter oGraphAdapter = new SimpleGraphAdapter();

        /*
        ( (Smrf.NodeXL.Visualization.Wpf.VertexDrawer)
            m_oNodeXLControl.VertexDrawer ).Radius = 5;
        */

        m_oNodeXLControl.Graph = oGraphAdapter.LoadGraphFromFile(
            "..\\..\\SampleGraph.txt");

        AddToolTipsToVertices();

        m_oNodeXLControl.DrawGraph(true);
    }

    protected void
    PopulateGraph()
    {
        IGraph oGraph = m_oNodeXLControl.Graph;
        IVertexCollection oVertices = oGraph.Vertices;
        IEdgeCollection oEdges = oGraph.Edges;
        Double dWidth = this.Width;
        Double dHeight = this.Height;
        Random oRandom = new Random();

        // m_oNodeXLControl.Layout.Margin = 0;

        {
        #if false  // Two shapes only.

        IVertex oVertex1 = oVertices.Add();

        oVertex1.SetValue( ReservedMetadataKeys.PerVertexShape,
            VertexShape.Circle);

        oVertex1.SetValue(ReservedMetadataKeys.PerVertexRadius, 5.0F);
        oVertex1.SetValue(ReservedMetadataKeys.LockVertexLocation, true);
        oVertex1.Location = new System.Drawing.PointF(300, 300);

        oVertex1.SetValue(ReservedMetadataKeys.PerVertexLabel,
            "This is A: " + oVertex1.Location);

        IVertex oVertex2 = oVertices.Add();

        oVertex2.SetValue( ReservedMetadataKeys.PerVertexShape,
            VertexShape.Circle);

        oVertex2.SetValue(ReservedMetadataKeys.PerVertexRadius, 5.0F);
        oVertex2.SetValue(ReservedMetadataKeys.LockVertexLocation, true);
        oVertex2.Location = new System.Drawing.PointF(500, 300);

        oVertex2.SetValue(ReservedMetadataKeys.PerVertexLabel,
            "This is B: " + oVertex2.Location);

        IEdge oEdge = oEdges.Add(oVertex1, oVertex2, true);

        // oEdge.SetValue(ReservedMetadataKeys.PerEdgeWidth, 20F);

        m_oNodeXLControl.DrawGraph(true);

        return;

        #endif
        }

        {
        #if false  // Two labels only.

        IVertex oVertex1 = oVertices.Add();

        oVertex1.SetValue(ReservedMetadataKeys.PerVertexShape,
            VertexShape.Label);

        oVertex1.SetValue(ReservedMetadataKeys.PerVertexLabel,
            "This is a label.");

        oVertex1.SetValue(ReservedMetadataKeys.LockVertexLocation, true);
        oVertex1.Location = new System.Drawing.PointF(300, 300);

        IVertex oVertex2 = oVertices.Add();

        oVertex2.SetValue(ReservedMetadataKeys.PerVertexShape,
            VertexShape.Label);

        oVertex2.SetValue(ReservedMetadataKeys.PerVertexLabel,
            "This is another label.");

        oVertex2.SetValue(ReservedMetadataKeys.LockVertexLocation, true);
        oVertex2.Location = new System.Drawing.PointF(500, 100);

        oEdges.Add(oVertex1, oVertex2, true);

        m_oNodeXLControl.DrawGraph(true);

        return;

        #endif
        }

        Smrf.NodeXL.Visualization.Wpf.VertexShape[] aeShapes
            = (Smrf.NodeXL.Visualization.Wpf.VertexShape[])
            Enum.GetValues(typeof(Smrf.NodeXL.Visualization.Wpf.
                VertexShape));

        Int32 iShapes = aeShapes.Length;

        Int32 Vertices = 100;

        IVertex oFirstVertex = oVertices.Add();

        oFirstVertex.SetValue(ReservedMetadataKeys.PerVertexRadius, 4.0F);

        IVertex oPreviousVertex = oFirstVertex;

        for (Int32 i = 1; i < Vertices; i++)
        {
            IVertex oVertex = oVertices.Add();
            VertexShape eShape = aeShapes[ oRandom.Next(iShapes) ];
            oVertex.SetValue(ReservedMetadataKeys.PerVertexShape, eShape);

            #if false  // Hard-coded vertex shape.

            oVertex.SetValue(ReservedMetadataKeys.PerVertexShape,
                VertexDrawer.VertexShape.Diamond);

            #endif

            #if true  // Vertex color.

            oVertex.SetValue( ReservedMetadataKeys.PerColor,
                System.Windows.Media.Color.FromArgb(255,
                (Byte)oRandom.Next(256),
                (Byte)oRandom.Next(256),
                (Byte)oRandom.Next(256))
                );

            #endif

            #if true  // Vertex radius.

            Single fRadius = (Single)(
                Smrf.NodeXL.Visualization.Wpf.VertexDrawer.MinimumRadius +
                (0.1 * 
                Smrf.NodeXL.Visualization.Wpf.VertexDrawer.MaximumRadius
                - Smrf.NodeXL.Visualization.Wpf.VertexDrawer.MinimumRadius)
                    * oRandom.NextDouble() );

            oVertex.SetValue(ReservedMetadataKeys.PerVertexRadius, fRadius);

            #endif

            if (true && oRandom.Next(20) == 0)  // Image
            {
                oVertex.SetValue(ReservedMetadataKeys.PerVertexShape,
                    VertexShape.Image);

                oVertex.SetValue( ReservedMetadataKeys.PerVertexImage,
                    new System.Windows.Media.Imaging.BitmapImage(
                        new Uri( oRandom.Next(2) == 1 ?
                            "..\\..\\Images\\TestImage1.gif" :
                            "..\\..\\Images\\TestImage2.jpg",
                            UriKind.Relative)));
            }

            if (eShape == VertexShape.Label)
            {
                String sLabel = "This is a label";

                if (oRandom.Next(2) == 0)
                {
                    sLabel = LongLabel;
                }

                oVertex.SetValue(ReservedMetadataKeys.PerVertexLabel, sLabel);

                /*
                oVertex.SetValue( ReservedMetadataKeys.PerColor,
                    System.Windows.Media.Color.FromArgb(255, 0, 0, 0) );

                oVertex.SetValue(

                    ReservedMetadataKeys.PerVertexLabelFillColor,
                        System.Windows.Media.Color.FromArgb(255, 200, 200,
                            200) );

                oVertex.SetValue(ReservedMetadataKeys.PerAlpha, (Single)128);
                */
            }
            else
            {
                String sAnnotation = "This is an annotation.";

                oVertex.SetValue(ReservedMetadataKeys.PerVertexLabel,
                    sAnnotation);
            }

            if (true && oRandom.Next(1) == 1)  // Vertex visibility.
            {
                oVertex.SetValue(ReservedMetadataKeys.Visibility,
                    VisibilityKeyValue.Filtered);
            }

            #if false  // Vertex alpha.

            oVertex.SetValue(
                ReservedMetadataKeys.PerAlpha, (Single)oRandom.Next(256) );

            #endif

            #if false  // Vertex IsSelected.

            oVertex.SetValue(ReservedMetadataKeys.IsSelected, null);

            #endif

            oVertex.Location = new System.Drawing.PointF(
                (Single)( dWidth * oRandom.NextDouble() ),
                (Single)( dHeight * oRandom.NextDouble() )
                );


            IEdge oEdge = oEdges.Add(oFirstVertex, oVertex, true);

            oEdge.SetValue(ReservedMetadataKeys.PerEdgeLabel,
                "This is an edge label");


            #if false  // Edge color.

            oEdge.SetValue( ReservedMetadataKeys.PerColor,
                System.Windows.Media.Color.FromArgb(255,
                (Byte)oRandom.Next(256),
                (Byte)oRandom.Next(256),
                (Byte)oRandom.Next(256))
                );

            #endif

            #if false  // Edge width.

            Double dEdgeWidth = EdgeDrawer.MinimumWidth +
                (EdgeDrawer.MaximumWidth - EdgeDrawer.MinimumWidth)
                    * oRandom.NextDouble();

            oEdge.SetValue(ReservedMetadataKeys.PerEdgeWidth, dEdgeWidth);

            #endif

            #if true  // Edge visibility.

            oEdge.SetValue(ReservedMetadataKeys.Visibility,
                VisibilityKeyValue.Visible);

            #endif

            #if true  // Edge alpha.

            oEdge.SetValue( ReservedMetadataKeys.PerAlpha,
                (Single)oRandom.Next(256) );

            #endif

            #if false  // Edge IsSelected.

            oEdge.SetValue(ReservedMetadataKeys.IsSelected, null);

            #endif


            #if true
            if (oRandom.Next(1) == 0)
            {
                IEdge oRandomEdge = oEdges.Add(oPreviousVertex, oVertex, true);

                #if true  // Edge label.

                oRandomEdge.SetValue(ReservedMetadataKeys.PerEdgeLabel,
                    "This is a random edge label");

                #endif
            }
            #endif

            oPreviousVertex = oVertex;
        }

        AddToolTipsToVertices();
        SetBackgroundImage();

        m_oNodeXLControl.DrawGraph(true);
    }

    protected void
    AddToolTipsToVertices()
    {
        foreach (IVertex oVertex in m_oNodeXLControl.Graph.Vertices)
        {
            oVertex.SetValue(ReservedMetadataKeys.PerVertexToolTip,
                String.Format(

                    "This is the tooltip for the vertex with ID {0}."
                    ,
                    oVertex.ID.ToString()
                ) );
        }
    }

    protected void
    AddSelectedVerticesToStatus()
    {
        AddToStatus("IDs in SelectedVertices:");

        foreach (IVertex oVertex in m_oNodeXLControl.SelectedVertices)
        {
            AddToStatus( oVertex.ID.ToString() );
        }
    }

    protected void
    AddSelectedEdgesToStatus()
    {
        AddToStatus("IDs in SelectedEdges:");

        foreach (IEdge oEdge in m_oNodeXLControl.SelectedEdges)
        {
            AddToStatus( oEdge.ID.ToString() );
        }
    }

    protected void
    SetBackgroundImage()
    {
        if (this.chkSetBackgroundImage.Checked)
        {
            String sAssemblyPath = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);

            WpfImageUtil oWpfImageUtil = new WpfImageUtil();

            BitmapSource oBitmapSource =
                oWpfImageUtil.GetImageSynchronousIgnoreDpi(
                    Path.Combine(sAssemblyPath,
                    "..\\..\\Images\\TestBackground.jpg") );

            m_oNodeXLControl.BackgroundImage = oBitmapSource;
        }
        else
        {
            m_oNodeXLControl.BackgroundImage = null;
        }
    }

    protected void
    AddToStatus
    (
        String sText
    )
    {
        // Add the text to the current results.  Precede it with a new line
        // if this isn't the first line.

        String sStatusText = txbStatus.Text;

        if (sStatusText.Length != 0)
            sStatusText += Environment.NewLine;

        sStatusText += sText;
        txbStatus.Text = sStatusText;

        // Scroll to the bottom.

        // txbStatus.Focus();
        txbStatus.Select(sStatusText.Length, 0);
        txbStatus.ScrollToCaret();
    }

    private void
    m_oNodeXLControl_SelectionChanged
    (
        object sender,
        EventArgs e
    )
    {
        AddToStatus("SelectionChanged");

        #if false

        AddSelectedVerticesToStatus();
        AddSelectedEdgesToStatus();

        #endif
    }

    private void
    m_oNodeXLControl_GraphZoomChanged
    (
        object sender,
        EventArgs e
    )
    {
        AddToStatus("GraphZoomChanged");
    }

    private void
    m_oNodeXLControl_LayingOutGraph
    (
        object sender,
        EventArgs e
    )
    {
        AddToStatus("LayingOutGraph");
    }

    private void
    m_oNodeXLControl_GraphLaidOut
    (
        object sender,
        AsyncCompletedEventArgs e
    )
    {
        AddToStatus("GraphLaidOut");
    }

    private void
    m_oNodeXLControl_VertexClick
    (
        object sender,
        VertexEventArgs vertexEventArgs
    )
    {
        AddToStatus("VertexClick: " + vertexEventArgs.Vertex);

        #if false

        // Retrieve the clicked vertex.

        IVertex oClickedVertex = vertexEventArgs.Vertex;

        // Get a new image for the vertex.

        oClickedVertex.SetValue( ReservedMetadataKeys.PerVertexImage,
            new System.Windows.Media.Imaging.BitmapImage(
                new Uri("C:\\Temp\\1.jpg") ) );

        // m_oNodeXLControl.ForceRedraw();

        #endif
    }

    private void
    m_oNodeXLControl_VertexDoubleClick
    (
        object sender,
        VertexEventArgs vertexEventArgs
    )
    {
        AddToStatus("VertexDoubleClick: " + vertexEventArgs.Vertex);
    }

    private void
    m_oNodeXLControl_VertexMouseHover
    (
        object sender,
        VertexEventArgs vertexEventArgs
    )
    {
        AddToStatus("VertexMouseHover: " + vertexEventArgs.Vertex);
    }

    private void
    m_oNodeXLControl_VertexMouseLeave
    (
        object sender,
        EventArgs eventArgs
    )
    {
        AddToStatus("VertexMouseLeave");
    }

    private void
    m_oNodeXLControl_VerticesMoved
    (
        object sender,
        VerticesMovedEventArgs verticesMovedEventArgs
    )
    {
        AddToStatus("VerticesMoved: " +
            verticesMovedEventArgs.MovedVertices.Count);
    }

    private void
    m_oNodeXLControl_PreviewVertexToolTipShown
    (
        object sender,
        VertexToolTipShownEventArgs vertexToolTipShownEventArgs
    )
    {
        AddToStatus("PreviewVertexToolTipShown");

        #if false
        System.Windows.Controls.TextBox oTextBox =
            new System.Windows.Controls.TextBox();

        oTextBox.MinWidth = 500;
        oTextBox.MinHeight = 100;

        vertexToolTipShownEventArgs.VertexToolTip = oTextBox;
        #endif
    }

    private void
    m_oNodeXLControl_GraphMouseDown
    (
        object sender,
        GraphMouseButtonEventArgs graphMouseButtonEventArgs
    )
    {
        IVertex oClickedVertex = graphMouseButtonEventArgs.Vertex;

        if (oClickedVertex == null)
        {
            AddToStatus("GraphMouseDown: No vertex.");
        }
        else
        {
            AddToStatus("GraphMouseDown: " + oClickedVertex);
        }
    }

    private void
    m_oNodeXLControl_GraphMouseUp
    (
        object sender,
        GraphMouseButtonEventArgs graphMouseButtonEventArgs
    )
    {
        IVertex oClickedVertex = graphMouseButtonEventArgs.Vertex;

        if (oClickedVertex == null)
        {
            AddToStatus("GraphMouseUp: No vertex.");
        }
        else
        {
            AddToStatus("GraphMouseUp: " + oClickedVertex);
        }
    }

    private void
    btnClearStatus_Click
    (
        object sender,
        EventArgs e
    )
    {
        txbStatus.Clear();
    }

    private void
    btnDeselectAll_Click
    (
        object sender,
        EventArgs e
    )
    {
        m_oNodeXLControl.DeselectAll();
    }

    private void
    chkShowVertexToolTips_CheckedChanged
    (
        object sender,
        EventArgs e
    )
    {
        m_oNodeXLControl.ShowVertexToolTips = chkShowVertexToolTips.Checked;
    }

    private void
    chkShowAxes_CheckedChanged
    (
        object sender,
        EventArgs e
    )
    {
        m_oNodeXLWithAxesControl.ShowAxes = chkShowAxes.Checked;
    }

    private void
    chkSetBackgroundImage_CheckedChanged
    (
        object sender,
        EventArgs e
    )
    {
        SetBackgroundImage();
        m_oNodeXLControl.DrawGraph(false);
    }

    private void
    btnSelectedVertices_Click
    (
        object sender,
        EventArgs e
    )
    {
        AddSelectedVerticesToStatus();
    }

    private void
    btnSelectedEdges_Click
    (
        object sender,
        EventArgs e
    )
    {
        AddSelectedEdgesToStatus();
    }

    private void
    btnSetVertexSelected_Click
    (
        object sender,
        EventArgs e
    )
    {
        try
        {
            Int32 iVertexID = Int32.Parse(txbVertexID.Text);

            IVertex oVertex;

            if ( !m_oNodeXLControl.Graph.Vertices.Find(
                iVertexID, out oVertex) )
            {
                throw new ArgumentException("No such ID.");
            }

            m_oNodeXLControl.SetVertexSelected(oVertex,
                chkVertexSelected.Checked, chkAlsoIncidentEdges.Checked);
        }
        catch (Exception oException)
        {
            MessageBox.Show(oException.Message);
        }
    }

    private void
    btnSetEdgeSelected_Click
    (
        object sender,
        EventArgs e
    )
    {
        try
        {
            Int32 iEdgeID = Int32.Parse(txbEdgeID.Text);

            IEdge oEdge;

            if ( !m_oNodeXLControl.Graph.Edges.Find(iEdgeID, out oEdge) )
            {
                throw new ArgumentException("No such ID.");
            }

            m_oNodeXLControl.SetEdgeSelected(oEdge,
                chkEdgeSelected.Checked, chkAlsoAdjacentVertices.Checked);
        }
        catch (Exception oException)
        {
            MessageBox.Show(oException.Message);
        }
    }

    private void
    cbxMouseMode_SelectedIndexChanged
    (
        object sender,
        EventArgs e
    )
    {
        m_oNodeXLControl.MouseMode =
            (Smrf.NodeXL.Visualization.Wpf.MouseMode)
            cbxMouseMode.SelectedValue;
    }

    private void
    btnHideSelected_Click
    (
        object sender,
        EventArgs e
    )
    {
        foreach (IVertex oSelectedVertex in m_oNodeXLControl.SelectedVertices)
        {
            oSelectedVertex.SetValue(ReservedMetadataKeys.Visibility,
                VisibilityKeyValue.Hidden);
        }

        foreach (IEdge oSelectedEdge in m_oNodeXLControl.SelectedEdges)
        {
            oSelectedEdge.SetValue(ReservedMetadataKeys.Visibility,
                VisibilityKeyValue.Hidden);
        }

        m_oNodeXLControl.DrawGraph();
    }

    private void
    btnShowSelected_Click
    (
        object sender,
        EventArgs e
    )
    {
        foreach (IVertex oSelectedVertex in m_oNodeXLControl.SelectedVertices)
        {
            oSelectedVertex.RemoveKey(ReservedMetadataKeys.Visibility);
        }

        foreach (IEdge oSelectedEdge in m_oNodeXLControl.SelectedEdges)
        {
            oSelectedEdge.RemoveKey(ReservedMetadataKeys.Visibility);
        }

        m_oNodeXLControl.DrawGraph();
    }

    protected const String LongLabel =
        "The Width of the resulting rectangle is increased or decreased by"
        + " twice the specified width offset, because it is applied to both"
        + " the left and right sides of the rectangle. Likewise, the Height of"
        + " the resulting rectangle is increased or decreased by twice the"
        + " specified height."
        ;
}

}
