using System;
using LayoutControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Adapters;

namespace LayoutControlUnitTest
{
    [TestClass]
    public class SetAndShowGraphTest
    {
        private LayoutControl m_layoutControl;
        public SetAndShowGraphTest()
        {
            m_layoutControl = new LayoutControl();
        }

        [TestMethod]
        public void SetAndShowGraphSuccessTest()
        {
            IGraphAdapter oGraphAdapter = new SimpleGraphAdapter();
            //layoutControl1.Graph = oGraphAdapter.LoadGraphFromFile("..\\..\\SampleGraph.txt");
            m_layoutControl.SetAndShowGraph(oGraphAdapter.LoadGraphFromFile("..\\..\\..\\SampleData\\SampleGraph.txt"));

            Assert.IsNotNull(m_layoutControl.Graph);
        }
    }
}
