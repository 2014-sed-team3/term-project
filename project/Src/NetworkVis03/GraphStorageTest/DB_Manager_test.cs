using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphStorageManagement;
using System.Data;
using Smrf.NodeXL.Core;

namespace GraphStorageTest
{
    [TestClass]
    public class DB_Manager_test
    {
        [TestMethod]
        public void Testlist_net()
        {
            DB_Manager db = new DB_Manager("networkvis");
            DataTable dt = db.list_network();
            String[] post = db.list_post_schema();
            db.list_post_like_schema();
            Assert.AreEqual(true, dt != null);
            Assert.AreEqual(true, post.Length > 0);
            db.close();
        }
        [TestMethod]
        public void TestGetNetWorkFromDatabase()
        {
            DB_Manager db = new DB_Manager("networkvis");
            Graph g = db.get_network("ABC");
            Assert.AreEqual(true, g != null);
            db.close();
        }
        [TestMethod]
        public void TestGetNetWorkFromImporter()
        {
            DB_Manager db = new DB_Manager("networkvis");
            String[] post = db.list_post_schema();
            Assert.AreEqual(true, post.Length > 1);

            db.setting.vertexCol = post[0];
            db.setting.edgeCol = post[1];
            Graph g = db.get_network("ABC",0);
            Assert.AreEqual(true, g != null);
            db.close();
        }
        public void gen_graph_call_back(object sender, IgraphGenerateEvent e)
        {

        }
        public void get_network_call_back(object sender, statusEventHandler e)
        {

        }
        [TestMethod]
        public void testConstructor()
        {
            EventHandler<IgraphGenerateEvent> handler = new System.EventHandler<IgraphGenerateEvent>(this.gen_graph_call_back);
            EventHandler<statusEventHandler> handler2 = new System.EventHandler<statusEventHandler>(this.get_network_call_back);
            DB_Manager db = new DB_Manager("networkvis");
            NetworkID UI_1 = new NetworkID(db, handler);
            GenerateGraph UI_2 = new GenerateGraph(db, handler);
            AskNetWorkID UI_3 = new AskNetWorkID(handler2);

            Assert.AreEqual(true, UI_1 != null);
            Assert.AreEqual(true, UI_2 != null);
            Assert.AreEqual(true, UI_3 != null);
        }
    }
}
