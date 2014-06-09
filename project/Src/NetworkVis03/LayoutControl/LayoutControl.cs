using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LayoutControls.Dialog;
using LayoutControls.Exceptions;
using LayoutControls.UserSettings;
using Smrf.AppLib;
using Smrf.NodeXL.ApplicationUtil;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Layouts;
using Smrf.NodeXL.Visualization.Wpf;

namespace LayoutControls
{
    public partial class LayoutControl : UserControl
    {
        public LayoutControl()
        {
            InitializeComponent();

            GeneralUserSettings oGeneralUserSettings = new GeneralUserSettings();
            oGeneralUserSettings.NotUseWorkbookSettings();
            LayoutUserSettings oLayoutUserSettings = new LayoutUserSettings();
            oLayoutUserSettings.NotUseWorkbookSettings();

            LayoutType eInitialLayout = oLayoutUserSettings.Layout;
//            LayoutType eInitialLayout = LayoutType.FruchtermanReingold;

            // Instantiate an object that populates the tssbLayout
            // ToolStripSplitButton and handles its LayoutChanged event.

            m_oLayoutManagerForToolStripSplitButton =
                new LayoutManagerForToolStripSplitButton();

            m_oLayoutManagerForToolStripSplitButton.AddItems(this.tssbLayout);
            m_oLayoutManagerForToolStripSplitButton.Layout = eInitialLayout;
            m_oLayoutManagerForToolStripSplitButton.LayoutChanged +=
                new EventHandler(
                    this.LayoutManagerForToolStripSplitButton_LayoutChanged);

            CreateNodeXLControl();
            CreateGraphZoomAndScaleControl();

            ApplyGeneralUserSettings(oGeneralUserSettings);
            ApplyLayoutUserSettings(oLayoutUserSettings);

            AssertValid();
        }

        public IGraph
        Graph
        {
            get
            {
                AssertValid();

                return (m_oNodeXLControl.Graph);
            }

            set
            {
                const String PropertyName = "Graph";

                m_oNodeXLControl.Graph = value;
                AssertValid();
            }
        }



        //*************************************************************************
        //  Method: CreateNodeXLControl()
        //
        /// <summary>
        /// Creates a NodeXLControl, hooks up its events, and assigns it as the
        /// child of an ElementHost.
        /// </summary>
        //*************************************************************************

        protected void
        CreateNodeXLControl()
        {
            // AssertValid();

            // Control hierarchy:
            //
            // 1. ehElementHost contains a NodeXLWithAxesControl.
            //
            // 2. The NodeXLWithAxesControl contains a NodeXLControl.

            m_oNodeXLControl = new NodeXLControl();

            m_oNodeXLWithAxesControl = new NodeXLWithAxesControl(m_oNodeXLControl);

            // Don't show the axes now.  If they are supposed to be shown, the
            // Ribbon, which is responsible for maintaining the visibility of the
            // axes, will send a NoParamCommand.ShowGraphAxes command to the
            // TaskPane later.

            m_oNodeXLWithAxesControl.ShowAxes = false;

            m_oNodeXLControl.LayingOutGraph += new System.EventHandler(
                this.oNodeXLControl_LayingOutGraph);

            m_oNodeXLControl.GraphLaidOut += new AsyncCompletedEventHandler(
                this.oNodeXLControl_GraphLaidOut);

            ehNodeXLControlHost.Child = m_oNodeXLWithAxesControl;
        }

        //*************************************************************************
        //  Method: CreateGraphZoomAndScaleControl()
        //
        /// <summary>
        /// Creates the control that can zoom and scale the NodeXLControl.
        /// </summary>
        //*************************************************************************

        protected void
        CreateGraphZoomAndScaleControl()
        {
            Debug.Assert(m_oNodeXLControl != null);

            GraphZoomAndScaleControl oGraphZoomAndScaleControl =
                new GraphZoomAndScaleControl();

            oGraphZoomAndScaleControl.NodeXLControl = m_oNodeXLControl;

            // The control lives within the second ToolStrip.

            this.tsMouse.Items.Insert(9, new ToolStripControlHost(
                oGraphZoomAndScaleControl));
        }
        //*************************************************************************
        //  Method: ApplyLayoutUserSettings()
        //
        /// <summary>
        /// Applies the user's layout settings to the NodeXLControl.
        /// </summary>
        ///
        /// <param name="oLayoutUserSettings">
        /// The user's layout settings.
        /// </param>
        //*************************************************************************

        protected void
        ApplyLayoutUserSettings
        (
            LayoutUserSettings oLayoutUserSettings
        )
        {
//            Debug.Assert(oLayoutUserSettings != null);
//            AssertValid();

            // Either layout manager will work; arbitrarily use one of them to
            // create a layout.

            ILayout oLayout =
                m_oLayoutManagerForToolStripSplitButton.CreateLayout();

            oLayoutUserSettings.TransferToLayout(oLayout);
            m_oNodeXLControl.Layout = oLayout;
        }

        //*************************************************************************
        //  Method: ApplyGeneralUserSettings()
        //
        /// <summary>
        /// Applies the user's general settings to the NodeXLControl.
        /// </summary>
        ///
        /// <param name="oGeneralUserSettings">
        /// The user's general settings.
        /// </param>
        //*************************************************************************

        protected void
        ApplyGeneralUserSettings
        (
            GeneralUserSettings oGeneralUserSettings
        )
        {
            Debug.Assert(oGeneralUserSettings != null);
            AssertValid();

            oGeneralUserSettings.TransferToNodeXLWithAxesControl(
                m_oNodeXLWithAxesControl);
        }

        //*************************************************************************
        //  Method: EditGeneralUserSettings()
        //
        /// <summary>
        /// Shows the dialog that lets the user edit his general settings.
        /// </summary>
        //*************************************************************************

        protected void
        EditGeneralUserSettings()
        {
            AssertValid();

            if (m_oNodeXLControl.IsLayingOutGraph)
            {
                return;
            }

            GeneralUserSettings oGeneralUserSettings = new GeneralUserSettings();
            oGeneralUserSettings.NotUseWorkbookSettings();

            GeneralUserSettingsDialog oGeneralUserSettingsDialog =
                new GeneralUserSettingsDialog(oGeneralUserSettings);

            if (oGeneralUserSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                oGeneralUserSettings.Save();
                ApplyGeneralUserSettings(oGeneralUserSettings);
                m_oNodeXLControl.DrawGraph();
            }
        }

        //*************************************************************************
        //  Method: EditLayoutUserSettings()
        //
        /// <summary>
        /// Shows the dialog that lets the user edit his layout settings.
        /// </summary>
        //*************************************************************************

        protected void
        EditLayoutUserSettings()
        {
            AssertValid();

            if (m_oNodeXLControl.IsLayingOutGraph)
            {
                return;
            }

            LayoutUserSettings oLayoutUserSettings = new LayoutUserSettings();
            oLayoutUserSettings.NotUseWorkbookSettings();

            LayoutUserSettingsDialog oLayoutUserSettingsDialog =
                new LayoutUserSettingsDialog(oLayoutUserSettings);

            if (oLayoutUserSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                oLayoutUserSettings.Save();
                ApplyLayoutUserSettings(oLayoutUserSettings);
                m_oNodeXLControl.DrawGraph();
            }
        }
        //*************************************************************************
        //  Method: EnableGraphControls()
        //
        /// <summary>
        /// Enables or disables the controls used to interact with the graph.
        /// </summary>
        ///
        /// <param name="bEnable">
        /// true to enable the controls, false to disable them.
        /// </param>
        //*************************************************************************

        protected void
        EnableGraphControls
        (
            Boolean bEnable
        )
        {
            AssertValid();

            // A m_iEnableGraphControlsCount value of zero or greater should enable
            // the controls.

            if (bEnable)
            {
                m_iEnableGraphControlsCount++;
            }
            else
            {
                m_iEnableGraphControlsCount--;
            }

            Boolean bEnable2 = (m_iEnableGraphControlsCount >= 0);

            tsTop.Enabled = tsMouse.Enabled = bEnable2;

            this.UseWaitCursor = !bEnable2;

            // Setting this.UseWaitCursor affects the cursor when the mouse is
            // over a ToolStrip, but not when it's over the NodeXLControl.

            m_oNodeXLControl.Cursor =
                bEnable2 ? null : System.Windows.Input.Cursors.Wait;
        }


        protected void ShowGraph(Boolean bLayOutGraph)
//        public void ShowGraph(Boolean bLayOutGraph)
        {
            AssertValid();

            if (m_oNodeXLControl.IsLayingOutGraph)
            {
                return;
            }
            GeneralUserSettings oGeneralUserSettings = new GeneralUserSettings();
            oGeneralUserSettings.NotUseWorkbookSettings();
            LayoutUserSettings oLayoutUserSettings = new LayoutUserSettings();
            oLayoutUserSettings.NotUseWorkbookSettings();

            ApplyGeneralUserSettings(oGeneralUserSettings);
            ApplyLayoutUserSettings(oLayoutUserSettings);
            EnableGraphControls(false);

            try
            {
//                IGraphAdapter oGraphAdapter = new SimpleGraphAdapter();
//                m_oNodeXLControl.Graph = oGraphAdapter.LoadGraphFromFile(
//                    "..\\..\\SampleGraph.txt");

                //IGraph oGraph = oWorkbookReader.ReadWorkbook(
                //    m_oWorkbook, oReadWorkbookContext);

                // Load the NodeXLControl with the resulting graph.

                //m_oNodeXLControl.Graph = oGraph;

                // Collapse any groups that are supposed to be collapsed.

                //CollapseOrExpandGroups(GetGroupNamesToCollapse(oGraph), true,
                //    false);

                // Enable tooltips in case tooltips were specified in the workbook.

                m_oNodeXLControl.ShowVertexToolTips = true;

                // If the dynamic filter dialog is open, read the dynamic filter
                // columns it filled in.

                m_oNodeXLControl.DrawGraph(bLayOutGraph);

            }
            catch (Exception oException)
            {
                // If exceptions aren't caught here, Excel consumes them without
                // indicating that anything is wrong.  This can result in the graph
                // controls remaining disabled, among other problems.

                //ErrorUtil.OnException(oException);
            }
            finally
            {
                EnableGraphControls(true);
            }

            // Change the button text to indicate that if any of the buttons is
            // clicked again, the graph will be read again.

            tsbShowGraph.Text = "Refresh Graph";

        }

        public void SetAndShowGraph(IGraph oGraph)
        {
            this.Graph = oGraph;
            ShowGraph(true);
        }

        //*************************************************************************
        //  Method: OnLayoutChanged
        //
        /// <summary>
        /// Handles the LayoutChanged event on the program's layout managers.
        /// </summary>
        ///
        /// <param name="eLayout">
        /// The new layout. 
        /// </param>
        //*************************************************************************

        protected void
        OnLayoutChanged
        (
            LayoutType eLayout
        )
        {
            AssertValid();

            if (m_bHandlingLayoutChanged)
            {
                // Prevent an endless loop when the layout managers are
                // synchronized.

                return;
            }

            m_bHandlingLayoutChanged = true;


            // Save and apply the new layout.

            LayoutUserSettings oLayoutUserSettings = new LayoutUserSettings();
            oLayoutUserSettings.NotUseWorkbookSettings();
            oLayoutUserSettings.Layout = eLayout;
            oLayoutUserSettings.Save();
            ApplyLayoutUserSettings(oLayoutUserSettings);

            // If the layout was just changed from Null to something else and the
            // X and Y columns were autofilled, the X and Y autofill results need
            // to be cleared.

            m_bHandlingLayoutChanged = false;
        }
        //*************************************************************************
        //  Method: LayoutManagerForToolStripSplitButton_LayoutChanged()
        //
        /// <summary>
        /// Handles the LayoutChanged event on the
        /// m_oLayoutManagerForToolStripSplitButton layout manager.
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

        protected void
        LayoutManagerForToolStripSplitButton_LayoutChanged
        (
            object sender,
            EventArgs e
        )
        {
            AssertValid();

            OnLayoutChanged(m_oLayoutManagerForToolStripSplitButton.Layout);
        }
        //*************************************************************************
        //  Method: oNodeXLControl_LayingOutGraph()
        //
        /// <summary>
        /// Handles the LayingOutGraph event on the oNodeXLControl control.
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

        private void
        oNodeXLControl_LayingOutGraph
        (
            object sender,
            EventArgs e
        )
        {
            AssertValid();

            EnableGraphControls(false);
        }
        //*************************************************************************
        //  Method: oNodeXLControl_GraphLaidOut()
        //
        /// <summary>
        /// Handles the GraphLaidOut event on the oNodeXLControl control.
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

        private void
        oNodeXLControl_GraphLaidOut
        (
            object sender,
            AsyncCompletedEventArgs e
        )
        {
            AssertValid();

            EnableGraphControls(true);

            if (e.Error is OutOfMemoryException)
            {
                FormUtil.ShowError(
                    "The computer does not have enough memory to lay out the"
                    + " graph.  Try the following to fix the problem:"
                    + "\r\n\r\n"
                    + "1. Select a different layout algorithm."
                    + "\r\n\r\n"
                    + "2. Close other programs."
                    + "\r\n\r\n"
                    + "3. Restart the computer."
                    + "\r\n\r\n"
                    + "4. Reduce the number of edges in the graph."
                    + "\r\n\r\n"
                    + "5. Add more memory to the computer."
                    );
            }
            else if (e.Error != null)
            {
                ErrorUtil.OnException(e.Error);
            }
        }

        protected void showVertexLabel()
        {
            foreach (IVertex oVertex in m_oNodeXLControl.Graph.Vertices)
            {
                oVertex.SetValue(ReservedMetadataKeys.PerVertexLabel, oVertex.Name);
            }
            ShowGraph(false);
        }
        protected void hideVertexLabel()
        {
            foreach (IVertex oVertex in m_oNodeXLControl.Graph.Vertices)
            {
                oVertex.SetValue(ReservedMetadataKeys.PerVertexLabel, "");
            }
            ShowGraph(false);
        }

        //*************************************************************************
        //  Method: MouseModeButton_Click()
        //
        /// <summary>
        /// Handles the Click event on all the ToolStripButtons that correspond
        /// to values in the MouseMode enumeration.
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
        private void
        tsbMouseModeButton_Click
        (
            object sender,
            EventArgs e
        )
        {
            AssertValid();

            // tsToolStrip2 contains a ToolStripButton for each value in the
            // MouseMode enumeration.  They should act like radio buttons.

            foreach (ToolStripItem oToolStripItem in tsMouse.Items)
            {
                if (oToolStripItem is ToolStripButton)
                {
                    ToolStripButton oToolStripButton =
                        (ToolStripButton)oToolStripItem;

                    if (oToolStripButton.Tag is MouseMode)
                    {
                        // Check the clicked button and uncheck the others.

                        Boolean bChecked = false;

                        if (oToolStripButton == sender)
                        {
                            m_oNodeXLControl.MouseMode =
                                (MouseMode)oToolStripButton.Tag;

                            bChecked = true;
                        }

                        oToolStripButton.Checked = bChecked;
                    }
                }
            }
        }
        private void tsbShowGraph_Click(object sender, EventArgs e)
        {
            ShowGraph(true);
        }
        private void tsbShowHideLabel_Click(object sender, EventArgs e)
        {
            AssertValid();
            if (tsbShowHideLabel.Text == "Show Label")
            {
                showVertexLabel();
                tsbShowHideLabel.Text = "Hide Label";
            }
            else
            {
                hideVertexLabel();
                tsbShowHideLabel.Text = "Show Label";
            }
        }
        private void tssbOption_ButtonClick(object sender, EventArgs e)
        {
            tssbOption.ShowDropDown();
        }

        private void tsmiLayoutOption_Click(object sender, EventArgs e)
        {
            AssertValid();
            EditLayoutUserSettings();
        }

        private void tsmiGraphOption_Click(object sender, EventArgs e)
        {
            AssertValid();
            EditGeneralUserSettings();
        }



        //*************************************************************************
        //  Method: AssertValid()
        //
        /// <summary>
        /// Asserts if the object is in an invalid state.  Debug-only.
        /// </summary>
        //*************************************************************************

        [Conditional("DEBUG")]

        public void
        AssertValid()
        {
            Debug.Assert(m_oNodeXLControl != null);
            Debug.Assert(m_oNodeXLWithAxesControl != null);

            Debug.Assert(m_oLayoutManagerForToolStripSplitButton != null);

            // m_bHandlingLayoutChanged
            // m_iEnableGraphControlsCount
            // m_oEdgeRowIDDictionary
            // m_oVertexRowIDDictionary
            // m_oSaveGraphImageFileDialog
            // m_oReadabilityMetricsDialog
        }


        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        /// The control that displays the graph.
        protected NodeXLControl m_oNodeXLControl;

        /// The parent of oNodeXLControl.
        protected NodeXLWithAxesControl m_oNodeXLWithAxesControl;

        /// Helper objects for managing layouts.
        protected LayoutManagerForToolStripSplitButton
            m_oLayoutManagerForToolStripSplitButton;

        /// Gets incremented by EnableGraphControls(true) and decremented by
        /// EnableGraphControls(false).
        protected Int32 m_iEnableGraphControlsCount;

        /// true if the LayoutChanged event on a layout manager is being handled.
        protected Boolean m_bHandlingLayoutChanged;

    }
}
