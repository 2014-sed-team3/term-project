using System;
using System.ComponentModel;
using System.Reflection.Emit;
using LayoutControls.UserSettings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smrf.NodeXL.Core;

namespace LayoutControlUnitTest
{
    [TestClass]
    public class GeneralUserSettingsTest
    {
        private GeneralUserSettings m_GeneralUserSettings;

        [TestMethod]
        public void CheckDefaultSettings()
        {
            m_GeneralUserSettings = new GeneralUserSettings();
            Assert.AreEqual(m_GeneralUserSettings.NewWorkbookGraphDirectedness, GraphDirectedness.Undirected);
            Assert.AreEqual(m_GeneralUserSettings.ReadVertexLabels, true);
            String labelSettings = (GeneralUserSettings.DefaultFont +
                                    "\tWhite\tBottomCenter\t2147483647\t2147483647\tBlack" +
                                    "\tTrue\t200\tBlack\t86\tMiddleCenter");
            var conv = new LabelUserSettingsTypeConverter();
            var lab = (LabelUserSettings) conv.ConvertFrom(labelSettings);
            Assert.AreEqual(conv.ConvertTo(m_GeneralUserSettings.LabelUserSettings, typeof(String)), labelSettings);
        }
    }
}
